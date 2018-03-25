using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Wigo4It.Blockchain.Core.Contracts
{
    public class MultiplicationService
    {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'a','type':'int256'}],'name':'multiply','outputs':[{'name':'r','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[{'name':'multiplier','type':'int256'}],'payable':false,'stateMutability':'nonpayable','type':'constructor'},{'anonymous':false,'inputs':[{'indexed':true,'name':'a','type':'int256'},{'indexed':true,'name':'sender','type':'address'},{'indexed':false,'name':'result','type':'int256'}],'name':'Multiplied','type':'event'}]";

        public static string BYTE_CODE = "0x6060604052341561000f57600080fd5b60405160208061011e83398101604052808051600055505060e9806100356000396000f300606060405260043610603e5763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416631df4f14481146043575b600080fd5b3415604d57600080fd5b60566004356068565b60405190815260200160405180910390f35b600054810273ffffffffffffffffffffffffffffffffffffffff3316827f841774c8b4d8511a3974d7040b5bc3c603d304c926ad25d168dacd04e25c4bed8360405190815260200160405180910390a39190505600a165627a7a7230582073cbabe575b8ba302a21139c97e0744697c0e6efbe358645dafdbf117053388f0029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount);
        }

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom, HexBigInteger gas = null, params object[] values)
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, values);
        }

        private Contract contract;

        public MultiplicationService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        public Function GetFunctionMultiply()
        {
            return contract.GetFunction("multiply");
        }
        public Function GetFunctionTest()
        {
            return contract.GetFunction("test");
        }

        public Event GetEventMultiplied()
        {
            return contract.GetEvent("Multiplied");
        }

        public Task<BigInteger> MultiplyAsyncCall(BigInteger a)
        {
            var function = GetFunctionMultiply();
            return function.CallAsync<BigInteger>(a);
        }

        public Task<string> MultiplyAsync(string addressFrom, BigInteger a, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetFunctionMultiply();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, a);
        }
        public Task<string> TestAsync(string addressFrom, BigInteger multiplier, HexBigInteger gas = null, HexBigInteger valueAmount = null)
        {
            var function = GetFunctionTest();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, multiplier);
        }



    }


    public class MultipliedEventDTO
    {
        [Parameter("int256", "a", 1, true)]
        public BigInteger A { get; set; }

        [Parameter("address", "sender", 2, true)]
        public string Sender { get; set; }

        [Parameter("int256", "result", 3, false)]
        public BigInteger Result { get; set; }

    }
}

