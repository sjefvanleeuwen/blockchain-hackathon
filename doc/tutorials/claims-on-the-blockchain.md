## Claims endorsed on the Blockchain

A lot of processes in our social domain, being it work, healthcare, income needs endorsements from different stakeholders
in a process so the government can deliver and administer services for its citizens.

In this tutorial we are going to build a Claims contract in its simplest form. We are not going to look at security or 
scalability at this point so we can keep the system as simple as possible. 

Please consider claims that could be used in this Feature described in Gherkin syntax:

<pre style='color:#000000;background:#ffffff;'><span style='color:#e34adc; '>Feature:</span> Self reliance
  Citizens that are part of the social care act need products and services to be self reliant
  For <span style='color:#800000; font-weight:bold; '>this</span> they receive endorsements from district nurses and their township 

<span style='color:#e34adc; '>Scenario:</span> Obtain product endorsment
    Given <span style='color:#800000; '>"</span><span style='color:#0000e6; '>Richard</span><span style='color:#800000; '>"</span> <span style='color:#603000; '>gets</span> an indication <span style='color:#800000; font-weight:bold; '>for</span> the social care act from <span style='color:#800000; '>"</span><span style='color:#0000e6; '>Bob</span><span style='color:#800000; '>"</span>
      And <span style='color:#800000; '>"</span><span style='color:#0000e6; '>Eline</span><span style='color:#800000; '>"</span> gives an endorsement <span style='color:#800000; font-weight:bold; '>for</span> product <span style='color:#800000; '>"</span><span style='color:#0000e6; '>Electric Bike</span><span style='color:#800000; '>"</span>
     When <span style='color:#800000; '>"</span><span style='color:#0000e6; '>Jahir</span><span style='color:#800000; '>"</span> asks <span style='color:#800000; font-weight:bold; '>if</span> the endorsement is valid
     Then the result should be <span style='color:#800000; '>"</span><span style='color:#0000e6; '>True</span><span style='color:#800000; '>"</span>
</pre>
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

<pre style='color:#d1d1d1;background:#000000;'>pragma solidity <span style='color:#d2cd86; '>^</span><span style='color:#009f00; '>0.4</span><span style='color:#d2cd86; '>.</span><span style='color:#008c00; '>21</span><span style='color:#b060b0; '>;</span>

contract ClaimAndEndorse <span style='color:#b060b0; '>{</span>
  struct ENDORSEMENT <span style='color:#b060b0; '>{</span>
  uint creationTime<span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 struct CLAIM <span style='color:#b060b0; '>{</span>
  uint creationTime<span style='color:#b060b0; '>;</span>
  uint claimHash<span style='color:#b060b0; '>;</span>
  mapping <span style='color:#d2cd86; '>(</span>address <span style='color:#d2cd86; '>=</span><span style='color:#d2cd86; '>></span> ENDORSEMENT<span style='color:#d2cd86; '>)</span> endorsements<span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 mapping <span style='color:#d2cd86; '>(</span>address <span style='color:#d2cd86; '>=</span><span style='color:#d2cd86; '>></span> 
  mapping <span style='color:#d2cd86; '>(</span>uint <span style='color:#9999a9; '>/* CLAIM GUID */</span> <span style='color:#d2cd86; '>=</span><span style='color:#d2cd86; '>></span> CLAIM<span style='color:#d2cd86; '>)</span><span style='color:#d2cd86; '>)</span> claims<span style='color:#b060b0; '>;</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> setClaim<span style='color:#d2cd86; '>(</span>uint claimGuid<span style='color:#d2cd86; '>,</span> uint claimHash<span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  CLAIM c <span style='color:#d2cd86; '>=</span> claims<span style='color:#d2cd86; '>[</span>msg<span style='color:#d2cd86; '>.</span>sender<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span><span style='color:#b060b0; '>;</span>
  <span style='color:#e66170; font-weight:bold; '>if</span><span style='color:#d2cd86; '>(</span>c<span style='color:#d2cd86; '>.</span>claimHash <span style='color:#d2cd86; '>></span> <span style='color:#008c00; '>0</span><span style='color:#d2cd86; '>)</span> <span style='color:#e66170; font-weight:bold; '>throw</span><span style='color:#b060b0; '>;</span> <span style='color:#9999a9; '>// unset first!</span>
  c<span style='color:#d2cd86; '>.</span>creationTime <span style='color:#d2cd86; '>=</span> now<span style='color:#b060b0; '>;</span>
  c<span style='color:#d2cd86; '>.</span>claimHash <span style='color:#d2cd86; '>=</span> claimHash<span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> unsetClaim<span style='color:#d2cd86; '>(</span>uint claimGuid<span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  <span style='color:#e66170; font-weight:bold; '>delete</span> claims<span style='color:#d2cd86; '>[</span>msg<span style='color:#d2cd86; '>.</span>sender<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span><span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> setEndorsement<span style='color:#d2cd86; '>(</span>
  address claimer<span style='color:#d2cd86; '>,</span> uint claimGuid<span style='color:#d2cd86; '>,</span> uint expectedClaimHash
 <span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  CLAIM c <span style='color:#d2cd86; '>=</span> claims<span style='color:#d2cd86; '>[</span>claimer<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span><span style='color:#b060b0; '>;</span>
  <span style='color:#e66170; font-weight:bold; '>if</span><span style='color:#d2cd86; '>(</span>c<span style='color:#d2cd86; '>.</span>claimHash <span style='color:#d2cd86; '>!=</span> expectedClaimHash<span style='color:#d2cd86; '>)</span> <span style='color:#e66170; font-weight:bold; '>throw</span><span style='color:#b060b0; '>;</span>
  ENDORSEMENT e <span style='color:#d2cd86; '>=</span> c<span style='color:#d2cd86; '>.</span>endorsements<span style='color:#d2cd86; '>[</span>msg<span style='color:#d2cd86; '>.</span>sender<span style='color:#d2cd86; '>]</span><span style='color:#b060b0; '>;</span>
  e<span style='color:#d2cd86; '>.</span>creationTime <span style='color:#d2cd86; '>=</span> now<span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> unsetEndorsement<span style='color:#d2cd86; '>(</span>address claimer<span style='color:#d2cd86; '>,</span> uint claimGuid<span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  <span style='color:#e66170; font-weight:bold; '>delete</span> claims<span style='color:#d2cd86; '>[</span>claimer<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span>
          <span style='color:#d2cd86; '>.</span>endorsements<span style='color:#d2cd86; '>[</span>msg<span style='color:#d2cd86; '>.</span>sender<span style='color:#d2cd86; '>]</span><span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> checkClaim<span style='color:#d2cd86; '>(</span>
  address claimer<span style='color:#d2cd86; '>,</span> uint claimGuid<span style='color:#d2cd86; '>,</span> uint expectedClaimHash
 <span style='color:#d2cd86; '>)</span> constant returns <span style='color:#d2cd86; '>(</span>bool<span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  <span style='color:#e66170; font-weight:bold; '>return</span> claims<span style='color:#d2cd86; '>[</span>claimer<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>.</span>claimHash 
         <span style='color:#d2cd86; '>==</span> expectedClaimHash<span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
 
 <span style='color:#e66170; font-weight:bold; '>function</span> checkEndorsement<span style='color:#d2cd86; '>(</span>
  address claimer<span style='color:#d2cd86; '>,</span> uint claimGuid<span style='color:#d2cd86; '>,</span> address endorsedBy
 <span style='color:#d2cd86; '>)</span> constant returns <span style='color:#d2cd86; '>(</span>bool<span style='color:#d2cd86; '>)</span> <span style='color:#b060b0; '>{</span>
  <span style='color:#e66170; font-weight:bold; '>return</span> claims<span style='color:#d2cd86; '>[</span>claimer<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>[</span>claimGuid<span style='color:#d2cd86; '>]</span>
   <span style='color:#d2cd86; '>.</span>endorsements<span style='color:#d2cd86; '>[</span>endorsedBy<span style='color:#d2cd86; '>]</span><span style='color:#d2cd86; '>.</span>creationTime <span style='color:#d2cd86; '>></span> <span style='color:#008c00; '>0</span><span style='color:#b060b0; '>;</span>
 <span style='color:#b060b0; '>}</span>
<span style='color:#b060b0; '>}</span>
</pre>

Todo: Unfinished: Part II
