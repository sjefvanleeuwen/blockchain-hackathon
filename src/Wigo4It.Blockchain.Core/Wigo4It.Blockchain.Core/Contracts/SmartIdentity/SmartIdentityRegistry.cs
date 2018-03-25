using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace Wigo4It.Blockchain.Core.Contracts
{
   public class SmartIdentityRegistryService
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[],'name':'kill','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_contractHash','type':'bytes32'}],'name':'rejectContract','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[{'name':'','type':'bytes32'}],'name':'sicontracts','outputs':[{'name':'hash','type':'bytes32'},{'name':'submitter','type':'address'},{'name':'status','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'_contractHash','type':'bytes32'}],'name':'isValidContract','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_contractHash','type':'bytes32'}],'name':'approveContract','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_contractHash','type':'bytes32'}],'name':'submitContract','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':false,'inputs':[{'name':'_contractHash','type':'bytes32'}],'name':'deleteContract','outputs':[{'name':'','type':'bool'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";

        public static string BYTE_CODE = "0x6060604052341561000f57600080fd5b60008054600160a060020a033316600160a060020a031990911617905561038e8061003b6000396000f3006060604052600436106100825763ffffffff7c010000000000000000000000000000000000000000000000000000000060003504166341c0e1b581146100875780637522aff5146100ac578063923c4ad7146100d6578063a983c93214610118578063b6fae6c71461012e578063c750df7314610144578063dec0257d1461015a575b600080fd5b341561009257600080fd5b61009a610170565b60405190815260200160405180910390f35b34156100b757600080fd5b6100c260043561019b565b604051901515815260200160405180910390f35b34156100e157600080fd5b6100ec6004356101d7565b604051928352600160a060020a0390911660208301526040808301919091526060909101905180910390f35b341561012357600080fd5b6100c2600435610203565b341561013957600080fd5b6100c260043561024d565b341561014f57600080fd5b6100c2600435610289565b341561016557600080fd5b6100c26004356102d5565b60008054600160a060020a03908116903316811461018d57600080fd5b600054600160a060020a0316ff5b600080548190600160a060020a0390811690331681146101ba57600080fd5b505050600090815260016020819052604090912060029081015590565b6001602081905260009182526040909120805491810154600290910154600160a060020a039091169083565b6000818152600160208190526040822060020154141561022557506001610248565b6000828152600160205260409020600290810154141561024457600080fd5b5060005b919050565b600080548190600160a060020a03908116903316811461026c57600080fd5b505050600090815260016020819052604090912060020181905590565b60008181526001602081905260408220838155808201805473ffffffffffffffffffffffffffffffffffffffff191633600160a060020a03161790556002810192909255905b50919050565b60008181526001602052604081206002808201541461008257600181015433600160a060020a039081169116141561035d5760005433600160a060020a039081169116141561035d5760008381526001602081905260408220828155808201805473ffffffffffffffffffffffffffffffffffffffff191690556002019190915591506102cf565b6102cf5600a165627a7a72305820945a4b3d86a3cc9a6552f1d9fd12a9c3c0f6f0c7acdc2ac374d79fdbb68a1e1f0029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount );
        }

        private Contract contract;

        public SmartIdentityRegistryService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        public Function GetFunctionKill() {
            return contract.GetFunction("kill");
        }
        public Function GetFunctionRejectContract() {
            return contract.GetFunction("rejectContract");
        }
        public Function GetFunctionSicontracts() {
            return contract.GetFunction("sicontracts");
        }
        public Function GetFunctionIsValidContract() {
            return contract.GetFunction("isValidContract");
        }
        public Function GetFunctionApproveContract() {
            return contract.GetFunction("approveContract");
        }
        public Function GetFunctionSubmitContract() {
            return contract.GetFunction("submitContract");
        }
        public Function GetFunctionDeleteContract() {
            return contract.GetFunction("deleteContract");
        }


        public Task<BigInteger> KillAsyncCall() {
            var function = GetFunctionKill();
            return function.CallAsync<BigInteger>();
        }
        public Task<bool> RejectContractAsyncCall(byte[] _contractHash) {
            var function = GetFunctionRejectContract();
            return function.CallAsync<bool>(_contractHash);
        }
        public Task<bool> IsValidContractAsyncCall(byte[] _contractHash) {
            var function = GetFunctionIsValidContract();
            return function.CallAsync<bool>(_contractHash);
        }
        public Task<bool> ApproveContractAsyncCall(byte[] _contractHash) {
            var function = GetFunctionApproveContract();
            return function.CallAsync<bool>(_contractHash);
        }
        public Task<bool> SubmitContractAsyncCall(byte[] _contractHash) {
            var function = GetFunctionSubmitContract();
            return function.CallAsync<bool>(_contractHash);
        }
        public Task<bool> DeleteContractAsyncCall(byte[] _contractHash) {
            var function = GetFunctionDeleteContract();
            return function.CallAsync<bool>(_contractHash);
        }

        public Task<string> KillAsync(string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionKill();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount);
        }
        public Task<string> RejectContractAsync(string addressFrom, byte[] _contractHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionRejectContract();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _contractHash);
        }
        public Task<string> IsValidContractAsync(string addressFrom, byte[] _contractHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionIsValidContract();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _contractHash);
        }
        public Task<string> ApproveContractAsync(string addressFrom, byte[] _contractHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionApproveContract();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _contractHash);
        }
        public Task<string> SubmitContractAsync(string addressFrom, byte[] _contractHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSubmitContract();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _contractHash);
        }
        public Task<string> DeleteContractAsync(string addressFrom, byte[] _contractHash, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionDeleteContract();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, _contractHash);
        }

        public Task<SicontractsDTO> SicontractsAsyncCall(byte[] a) {
            var function = GetFunctionSicontracts();
            return function.CallDeserializingToObjectAsync<SicontractsDTO>(a);
        }


    }

    [FunctionOutput]
    public class SicontractsDTO 
    {
        [Parameter("bytes32", "hash", 1)]
        public byte[] Hash {get; set;}

        [Parameter("address", "submitter", 2)]
        public string Submitter {get; set;}

        [Parameter("uint256", "status", 3)]
        public BigInteger Status {get; set;}

    }



}

