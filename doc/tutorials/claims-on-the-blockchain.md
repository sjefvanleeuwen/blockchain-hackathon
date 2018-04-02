## Claims endorsed on the Blockchain

A lot of processes in our social domain, being it work, healthcare, income needs endorsements from different stakeholders
in a process so the government can deliver and administer services for its citizens.

In this tutorial we are going to build a Claims contract in its simplest form. We are not going to look at security or 
scalability at this point so we can keep the system as simple as possible. 

Please consider claims that could be used in this Feature described in Gherkin syntax:

<div id="preview">
          <p>
            <strong>Feature</strong>
          </p>
          
          <!-- HTML generated using hilite.me --><div style="background: #ffffff; overflow:auto;width:auto;border:solid gray;border-width:.1em .1em .1em .8em;padding:.2em .6em;"><pre style="margin: 0; line-height: 125%"><span style="color: #000080; font-weight: bold">Feature:</span> Self reliance
Citizens that are part of the social care act need products and services to be self reliant
For this they receive endorsements from district nurses and their township 

  <span style="color: #000080; font-weight: bold">Scenario:</span> Obtain product endorsment
<span style="color: #000080; font-weight: bold">    Given </span>"<span style="color: #0000FF">Richard</span>" gets an indication for the social care act from "<span style="color: #0000FF">Bob</span>"
      <span style="color: #000080; font-weight: bold">And </span>"<span style="color: #0000FF">Eline</span>" gives an endorsement for product "<span style="color: #0000FF">Electric Bike</span>"
     <span style="color: #000080; font-weight: bold">When </span>"<span style="color: #0000FF">Jahir</span>" asks if the endorsement is valid
     <span style="color: #000080; font-weight: bold">Then </span>the result should be "<span style="color: #0000FF">True</span>"
</pre></div>

When we break this down, civil servant Bob should verify the first claim by Richard. He then gets access to services
rendered to him under the ssocial care act. District Nurse Eline can verify Richard's claim and give him a prescription
the product can then be delivered by Jahir which can then verify Richard's claim that a registered Nurse has given 
him the prescription. 

__Note__: Ultimately, though not included in the feature, Jahir can read the claim that Richard is a customer under the
social care act, and could request a payment from Bob in the form of cryptocurrency to be paid out by the government.
The cryptocurrency could represent the national currency value and does not have to be a volatile currency such as
bitcoin. This way Richard does not have to wait for funds to be released and can immediately use the product or services.

## Modeling A Generic Claims contract in Solidity

1. Underlaying code, defines two structs, CLAIM has a 1..n mapping to the ENDORSEMENT struct. Thus a claim can hold
multiple endorsements issued by an address.

2. The direct mapping on the contract itself allows the mapping of an address to multiple claims. So an address can hold
multiple claims.

3. The code then has a couple of functions to interact with the claims.

<div id="preview">
          <p>
            <strong>Complete listing</strong>
          </p>
          
          <!-- HTML generated using hilite.me --><div style="background: #ffffff; overflow:auto;width:auto;border:solid gray;border-width:.1em .1em .1em .8em;padding:.2em .6em;"><pre style="margin: 0; line-height: 125%">pragma solidity ^<span style="color: #0000FF">0.4</span>.<span style="color: #0000FF">21</span>;
<pre>
contract ClaimAndEndorse {
  struct ENDORSEMENT {
  uint creationTime;
 }
 
 struct CLAIM {
  uint creationTime;
  uint claimHash;
  mapping (address =&gt; ENDORSEMENT) endorsements;
 }
 
 mapping (address =&gt; 
  mapping (uint <span style="color: #008800; font-style: italic">/* CLAIM GUID */</span> =&gt; CLAIM)) claims;
 
 <span style="color: #000080; font-weight: bold">function</span> setClaim(uint claimGuid, uint claimHash) {
  CLAIM c = claims[msg.sender][claimGuid];
  <span style="color: #000080; font-weight: bold">if</span>(c.claimHash &gt; <span style="color: #0000FF">0</span>) <span style="color: #000080; font-weight: bold">throw</span>; <span style="color: #008800; font-style: italic">// unset first!</span>
  c.creationTime = now;
  c.claimHash = claimHash;
 }
 
 <span style="color: #000080; font-weight: bold">function</span> unsetClaim(uint claimGuid) {
  <span style="color: #000080; font-weight: bold">delete</span> claims[msg.sender][claimGuid];
 }
 
 <span style="color: #000080; font-weight: bold">function</span> setEndorsement(
  address claimer, uint claimGuid, uint expectedClaimHash
 ) {
  CLAIM c = claims[claimer][claimGuid];
  <span style="color: #000080; font-weight: bold">if</span>(c.claimHash != expectedClaimHash) <span style="color: #000080; font-weight: bold">throw</span>;
  ENDORSEMENT e = c.endorsements[msg.sender];
  e.creationTime = now;
 }
 
 <span style="color: #000080; font-weight: bold">function</span> unsetEndorsement(address claimer, uint claimGuid) {
  <span style="color: #000080; font-weight: bold">delete</span> claims[claimer][claimGuid]
          .endorsements[msg.sender];
 }
 
 <span style="color: #000080; font-weight: bold">function</span> checkClaim(
  address claimer, uint claimGuid, uint expectedClaimHash
 ) constant returns (bool) {
  <span style="color: #000080; font-weight: bold">return</span> claims[claimer][claimGuid].claimHash 
         == expectedClaimHash;
 }
 
 <span style="color: #000080; font-weight: bold">function</span> checkEndorsement(
  address claimer, uint claimGuid, address endorsedBy
 ) constant returns (bool) {
  <span style="color: #000080; font-weight: bold">return</span> claims[claimer][claimGuid]
   .endorsements[endorsedBy].creationTime &gt; <span style="color: #0000FF">0</span>;
 }
}
</pre></div>


Todo: Unfinished: Part II
