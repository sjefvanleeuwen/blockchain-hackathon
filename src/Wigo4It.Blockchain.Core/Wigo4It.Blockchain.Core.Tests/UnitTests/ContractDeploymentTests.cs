using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexTypes;
using Nethereum.TestRPCRunner;
using Nethereum.Web3;
using Wigo4It.Blockchain.Core.Contracts;

namespace Wigo4It.Blockchain.Core.Tests.UnitTests
{
    [TestClass]
    public class ContractDeplomentTests
    {
        [TestMethod]
        public void ShouldDeployAContractWithConstructor()
        {
            using (var testrpcRunner = new TestRPCEmbeddedRunner())
            {
                try
                {
                    testrpcRunner.RedirectOuputToDebugWindow = true;
                    testrpcRunner.StartTestRPC();
                    var web3 = new Web3();
                    var addressFrom = (web3.Eth.Accounts.SendRequestAsync()).Result[0];
                    // Deploy the contract and add the multiplier to the constructor
                    var transactionHash =
                        MultiplicationService.DeployContractAsync(web3, addressFrom, new HexBigInteger(900000), 4).Result;
                    Assert.IsNotNull(transactionHash);
                    var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
                    var contract =  new MultiplicationService(web3, receipt.ContractAddress);
                    // Do a multiplication
                    var callResult = contract.MultiplyAsyncCall(4).Result;
                    Assert.AreEqual(16, callResult);
                }
                finally
                {
                    testrpcRunner.StopTestRPC();
                }
            }
        }
    }
}