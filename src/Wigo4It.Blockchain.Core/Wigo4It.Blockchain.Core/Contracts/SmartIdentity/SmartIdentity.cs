using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace Wigo4It.Blockchain.Core.Contracts
{
   public class SmartIdentityService
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'_newowner','type':'address'}],'name':'setOwner','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_attributeHash','type':'bytes32'},{'name':'_endorsementHash','type':'bytes32'}],'name':'removeEndorsement','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_attributeHash','type':'bytes32'},{'name':'_endorsementHash','type':'bytes32'}],'name':'acceptEndorsement','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'kill','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_myEncryptionPublicKey','type':'string'}],'name':'setEncryptionPublicKey','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'encryptionPublicKey','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[],'name':'removeOverride','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_override','type':'address'}],'name':'setOverride','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_mySigningPublicKey','type':'string'}],'name':'setSigningPublicKey','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[],'name':'getOwner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_hash','type':'bytes32'}],'name':'addAttribute','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'attributes','outputs':[{'name':'hash','type':'bytes32'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'signingPublicKey','outputs':[{'name':'','type':'string'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_attributeHash','type':'bytes32'},{'name':'_endorsementHash','type':'bytes32'}],'name':'checkEndorsementExists','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_hash','type':'bytes32'}],'name':'removeAttribute','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_attributeHash','type':'bytes32'},{'name':'_endorsementHash','type':'bytes32'}],'name':'addEndorsement','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_oldhash','type':'bytes32'},{'name':'_newhash','type':'bytes32'}],'name':'updateAttribute','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'sender','type':'address'},{'indexed':false,'name':'status','type':'uint256'},{'indexed':false,'name':'notificationMsg','type':'bytes32'}],'name':'ChangeNotification','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052341561000f57600080fd5b60008054600160a060020a03338116600160a060020a031992831617928390556001805490921692169190911790556013194301600255610e52806100556000396000f3006060604052600436106100f05763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166313af403581146100f55780633f65d97f14610128578063401611ed1461014157806341c0e1b51461015a5780635940f55c1461017f5780636b902cf5146101d0578063833ffb631461025a578063837e6a941461026d578063874b4fcc1461028c578063893d20e8146102dd57806389f0151c1461030c578063b115ce0d14610322578063cb5ab1be14610338578063e70622631461034b578063e7996f0714610364578063f3945ca01461037a578063f9c5e0aa14610393575b600080fd5b341561010057600080fd5b610114600160a060020a03600435166103ac565b604051901515815260200160405180910390f35b341561013357600080fd5b610114600435602435610431565b341561014c57600080fd5b610114600435602435610587565b341561016557600080fd5b61016d610610565b60405190815260200160405180910390f35b341561018a57600080fd5b61011460046024813581810190830135806020601f8201819004810201604051908101604052818152929190602084018383808284375094965061063b95505050505050565b34156101db57600080fd5b6101e36106a9565b60405160208082528190810183818151815260200191508051906020019080838360005b8381101561021f578082015183820152602001610207565b50505050905090810190601f16801561024c5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b341561026557600080fd5b610114610747565b341561027857600080fd5b610114600160a060020a03600435166107d1565b341561029757600080fd5b61011460046024813581810190830135806020601f8201819004810201604051908101604052818152929190602084018383808284375094965061084a95505050505050565b34156102e857600080fd5b6102f06108b8565b604051600160a060020a03909116815260200160405180910390f35b341561031757600080fd5b6101146004356108e8565b341561032d57600080fd5b61016d600435610997565b341561034357600080fd5b6101e36109a9565b341561035657600080fd5b610114600435602435610a14565b341561036f57600080fd5b610114600435610b11565b341561038557600080fd5b610114600435602435610bc0565b341561039e57600080fd5b610114600435602435610caf565b600154600090600160a060020a0390811690331681146103cb57600080fd5b4360146002540111156103dd57600080fd5b4360025560008054600160a060020a031916600160a060020a03851617905561042760037f4f776e657220686173206265656e206368616e67656400000000000000000000610d45565b5060019392505050565b6000828152600560209081526040808320848452600181019092528220805433600160a060020a03908116911614156104ca5760008481526001808401602052604082208054600160a060020a031916815590810191909155600201805460ff191690556104c060037f456e646f7273656d656e742072656d6f76656400000000000000000000000000610d45565b506001925061057f565b60005433600160a060020a0390811691161480156104ed5750600281015460ff16155b1561054e5760008481526001808401602052604082208054600160a060020a031916815590810191909155600201805460ff191690556104c060037f456e646f7273656d656e742064656e6965640000000000000000000000000000610d45565b61057960037f456e646f7273656d656e742072656d6f76616c206661696c6564000000000000610d45565b50600080fd5b505092915050565b6000805481908190600160a060020a0390811690331681146105a857600080fd5b60008681526005602090815260408083208884526001808201909352922060028101805460ff1916909217909155909350915061060660037f456e646f7273656d656e7420686173206265656e206163636570746564000000610d45565b5050505092915050565b60008054600160a060020a03908116903316811461062d57600080fd5b600054600160a060020a0316ff5b60008054600160a060020a03908116903316811461065857600080fd5b43601460025401111561066a57600080fd5b600383805161067d929160200190610d95565b5061042760037f456e6372797074696f6e206b6579206164646564000000000000000000000000610d45565b60038054600181600116156101000203166002900480601f01602080910402602001604051908101604052809291908181526020018280546001816001161561010002031660029004801561073f5780601f106107145761010080835404028352916020019161073f565b820191906000526020600020905b81548152906001019060200180831161072257829003601f168201915b505050505081565b60008054600160a060020a03908116903316811461076457600080fd5b43601460025401111561077657600080fd5b4360025560005460018054600160a060020a031916600160a060020a039092169190911790556107c760037f4f7665727269646520686173206265656e2072656d6f76656400000000000000610d45565b50600191505b5090565b60008054600160a060020a0390811690331681146107ee57600080fd5b43601460025401111561080057600080fd5b4360025560018054600160a060020a031916600160a060020a03851617905561042760037f4f7665727269646520686173206265656e206368616e67656400000000000000610d45565b60008054600160a060020a03908116903316811461086757600080fd5b43601460025401111561087957600080fd5b600483805161088c929160200190610d95565b5061042760037f5369676e696e67206b6579206164646564000000000000000000000000000000610d45565b600154600090600160a060020a0390811690331681146108d757600080fd5b5050600054600160a060020a031690565b600080548190600160a060020a03908116903316811461090757600080fd5b43601460025401111561091957600080fd5b6000848152600560205260409020805490925084141561095e5761057960037f4120686173682065786973747320666f72207468652061747472696275746500610d45565b83825561098c60047f41747472696275746520686173206265656e2061646465640000000000000000610d45565b506001949350505050565b60056020526000908152604090205481565b60048054600181600116156101000203166002900480601f01602080910402602001604051908101604052809291908181526020018280546001816001161561010002031660029004801561073f5780601f106107145761010080835404028352916020019161073f565b6000828152600560205260408120805482908514610a6157610a5760017f41747472696275746520646f65736e2774206578697374000000000000000000610d45565b506000925061057f565b50600083815260018083016020526040909120908101548414610aa957610a5760017f456e646f7273656d656e7420646f65736e277420657869737400000000000000610d45565b600281015460ff16151560011415610ae6576104c060047f456e646f7273656d656e742065786973747320666f7220617474726962757465610d45565b610a5760017f456e646f7273656d656e74206861736e2774206265656e206163636570746564610d45565b600080548190600160a060020a039081169033168114610b3057600080fd5b436014600254011115610b4257600080fd5b600084815260056020526040902080549092508414610b865761057960027f48617368206e6f7420666f756e6420666f722061747472696275746500000000610d45565b60008481526005602052604081205561098c60037f41747472696275746520686173206265656e2072656d6f766564000000000000610d45565b6000828152600560205260408120805482908514610c035761057960017f41747472696275746520646f65736e2774206578697374000000000000000000610d45565b5060008381526001808301602052604090912090810154841415610c4c5761057960017f456e646f7273656d656e7420616c726561647920657869737473000000000000610d45565b600181018490558054600160a060020a03191633600160a060020a031617815560028101805460ff19169055610ca360047f456e646f7273656d656e7420686173206265656e206164646564000000000000610d45565b50600195945050505050565b60008054600160a060020a039081169033168114610ccc57600080fd5b436014600254011115610cde57600080fd5b610d0960057f417474656d7074696e6720746f20757064617465206174747269627574650000610d45565b50610d1384610b11565b50610d1d836108e8565b5061098c60037f41747472696275746520686173206265656e20757064617465640000000000005b60008054600160a060020a03167f1c8764925b0427a02294e72402c4dcbad3f0e6d0a8b9b541f1469dc769f9b60b848460405191825260208201526040908101905180910390a250600192915050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f10610dd657805160ff1916838001178555610e03565b82800160010185558215610e03579182015b82811115610e03578251825591602001919060010190610de8565b506107cd92610e239250905b808211156107cd5760008155600101610e0f565b905600a165627a7a72305820ae2b867a76a41e200ba1323983c74efafd7c57ef36cf4c2d4a79f9c397dca3f30029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount );
        }

        private Contract contract;

        public SmartIdentityService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        /// <summary>
        /// This function gives the override address the ability to change owner.
        /// This could allow the identity to be moved to a multi-sig contract.
        /// See https://github.com/ethereum/dapp-bin/blob/master/wallet/wallet.sol
        /// for a multi-sig wallet example. </summary>
        /// <returns></returns>
        public Function GetFunctionSetOwner() {
            return contract.GetFunction("setOwner");
        }
        public Function GetFunctionRemoveEndorsement() {
            return contract.GetFunction("removeEndorsement");
        }
        public Function GetFunctionAcceptEndorsement() {
            return contract.GetFunction("acceptEndorsement");
        }
        public Function GetFunctionKill() {
            return contract.GetFunction("kill");
        }
        public Function GetFunctionSetEncryptionPublicKey() {
            return contract.GetFunction("setEncryptionPublicKey");
        }
        public Function GetFunctionEncryptionPublicKey() {
            return contract.GetFunction("encryptionPublicKey");
        }
        public Function GetFunctionRemoveOverride() {
            return contract.GetFunction("removeOverride");
        }
        public Function GetFunctionSetOverride() {
            return contract.GetFunction("setOverride");
        }
        public Function GetFunctionSetSigningPublicKey() {
            return contract.GetFunction("setSigningPublicKey");
        }
        public Function GetFunctionGetOwner() {
            return contract.GetFunction("getOwner");
        }
        public Function GetFunctionAddAttribute() {
            return contract.GetFunction("addAttribute");
        }
        public Function GetFunctionAttributes() {
            return contract.GetFunction("attributes");
        }
        public Function GetFunctionSigningPublicKey() {
            return contract.GetFunction("signingPublicKey");
        }
        public Function GetFunctionCheckEndorsementExists() {
            return contract.GetFunction("checkEndorsementExists");
        }
        public Function GetFunctionRemoveAttribute() {
            return contract.GetFunction("removeAttribute");
        }
        public Function GetFunctionAddEndorsement() {
            return contract.GetFunction("addEndorsement");
        }
        public Function GetFunctionUpdateAttribute() {
            return contract.GetFunction("updateAttribute");
        }

        public Event GetEventChangeNotification() {
            return contract.GetEvent("ChangeNotification");
        }

        public Task<bool> SetOwnerAsyncCall(string _newowner) {
            var function = GetFunctionSetOwner();
            return function.CallAsync<bool>(_newowner);
        }
        public Task<bool> RemoveEndorsementAsyncCall(byte[] _attributeHash, byte[] _endorsementHash) {
            var function = GetFunctionRemoveEndorsement();
            return function.CallAsync<bool>(_attributeHash, _endorsementHash);
        }
        public Task<bool> AcceptEndorsementAsyncCall(byte[] _attributeHash, byte[] _endorsementHash) {
            var function = GetFunctionAcceptEndorsement();
            return function.CallAsync<bool>(_attributeHash, _endorsementHash);
        }
        public Task<BigInteger> KillAsyncCall() {
            var function = GetFunctionKill();
            return function.CallAsync<BigInteger>();
        }
        public Task<bool> SetEncryptionPublicKeyAsyncCall(string _myEncryptionPublicKey) {
            var function = GetFunctionSetEncryptionPublicKey();
            return function.CallAsync<bool>(_myEncryptionPublicKey);
        }
        public Task<string> EncryptionPublicKeyAsyncCall() {
            var function = GetFunctionEncryptionPublicKey();
            return function.CallAsync<string>();
        }
        public Task<bool> RemoveOverrideAsyncCall() {
            var function = GetFunctionRemoveOverride();
            return function.CallAsync<bool>();
        }
        public Task<bool> SetOverrideAsyncCall(string _override) {
            var function = GetFunctionSetOverride();
            return function.CallAsync<bool>(_override);
        }
        public Task<bool> SetSigningPublicKeyAsyncCall(string _mySigningPublicKey) {
            var function = GetFunctionSetSigningPublicKey();
            return function.CallAsync<bool>(_mySigningPublicKey);
        }
        public Task<string> GetOwnerAsyncCall() {
            var function = GetFunctionGetOwner();
            return function.CallAsync<string>();
        }
        public Task<bool> AddAttributeAsyncCall(byte[] _hash) {
            var function = GetFunctionAddAttribute();
            return function.CallAsync<bool>(_hash);
        }
        public Task<byte[]> AttributesAsyncCall(byte[] a) {
            var function = GetFunctionAttributes();
            return function.CallAsync<byte[]>(a);
        }
        public Task<string> SigningPublicKeyAsyncCall() {
            var function = GetFunctionSigningPublicKey();
            return function.CallAsync<string>();
        }
        public Task<bool> CheckEndorsementExistsAsyncCall(byte[] _attributeHash, byte[] _endorsementHash) {
            var function = GetFunctionCheckEndorsementExists();
            return function.CallAsync<bool>(_attributeHash, _endorsementHash);
        }
        public Task<bool> RemoveAttributeAsyncCall(byte[] _hash) {
            var function = GetFunctionRemoveAttribute();
            return function.CallAsync<bool>(_hash);
        }
        public Task<bool> AddEndorsementAsyncCall(byte[] _attributeHash, byte[] _endorsementHash) {
            var function = GetFunctionAddEndorsement();
            return function.CallAsync<bool>(_attributeHash, _endorsementHash);
        }
        public Task<bool> UpdateAttributeAsyncCall(byte[] _oldhash, byte[] _newhash) {
            var function = GetFunctionUpdateAttribute();
            return function.CallAsync<bool>(_oldhash, _newhash);
        }

        public Task<string> SetOwnerAsync(string addressFrom, string _newowner, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetOwner();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _newowner);
        }
        public Task<string> RemoveEndorsementAsync(string addressFrom, byte[] _attributeHash, byte[] _endorsementHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionRemoveEndorsement();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _attributeHash, _endorsementHash);
        }
        public Task<string> AcceptEndorsementAsync(string addressFrom, byte[] _attributeHash, byte[] _endorsementHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAcceptEndorsement();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _attributeHash, _endorsementHash);
        }
        public Task<string> KillAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionKill();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> SetEncryptionPublicKeyAsync(string addressFrom, string _myEncryptionPublicKey, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetEncryptionPublicKey();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _myEncryptionPublicKey);
        }
        public Task<string> RemoveOverrideAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionRemoveOverride();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> SetOverrideAsync(string addressFrom, string _override, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetOverride();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _override);
        }
        public Task<string> SetSigningPublicKeyAsync(string addressFrom, string _mySigningPublicKey, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetSigningPublicKey();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _mySigningPublicKey);
        }
        public Task<string> GetOwnerAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionGetOwner();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> AddAttributeAsync(string addressFrom, byte[] _hash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAddAttribute();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _hash);
        }
        public Task<string> CheckEndorsementExistsAsync(string addressFrom, byte[] _attributeHash, byte[] _endorsementHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionCheckEndorsementExists();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _attributeHash, _endorsementHash);
        }
        public Task<string> RemoveAttributeAsync(string addressFrom, byte[] _hash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionRemoveAttribute();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _hash);
        }
        public Task<string> AddEndorsementAsync(string addressFrom, byte[] _attributeHash, byte[] _endorsementHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionAddEndorsement();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _attributeHash, _endorsementHash);
        }
        public Task<string> UpdateAttributeAsync(string addressFrom, byte[] _oldhash, byte[] _newhash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionUpdateAttribute();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _oldhash, _newhash);
        }



    }


    public class ChangeNotificationEventDTO 
    {
        [Parameter("address", "sender", 1, true)]
        public string Sender {get; set;}

        [Parameter("uint256", "status", 2, false)]
        public BigInteger Status {get; set;}

        [Parameter("bytes32", "notificationMsg", 3, false)]
        public byte[] NotificationMsg {get; set;}

    }


}

