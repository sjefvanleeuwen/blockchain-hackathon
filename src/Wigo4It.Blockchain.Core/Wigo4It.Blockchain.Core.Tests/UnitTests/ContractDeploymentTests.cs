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
        public void ShouldDeploySmartIdentityRegistry()
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
                        SmartIdentityRegistryService.DeployContractAsync(web3, addressFrom, new HexBigInteger(900000)).Result;
                    Assert.IsNotNull(transactionHash);
                    var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
                    Assert.IsNotNull(receipt);
                    var s  = new SmartIdentityRegistryService(web3, addressFrom);
                }
                finally
                {
                    testrpcRunner.StopTestRPC();
                }
            }

        }

        [TestMethod]
        public void ShouldDeploySmartIdentityMigrations()
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
                        MigrationsService.DeployContractAsync(web3, addressFrom, new HexBigInteger(900000)).Result;
                    Assert.IsNotNull(transactionHash);
                    var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
                    Assert.IsNotNull(receipt);
                }
                finally
                {
                    testrpcRunner.StopTestRPC();
                }
            }

        }
    
        [TestMethod]
        public void ShouldDeploySmartIdentity()
        {
            using (var testrpcRunner = new TestRPCEmbeddedRunner())
            {
                try
                {

                    testrpcRunner.RedirectOuputToDebugWindow = true;
                    testrpcRunner.StartTestRPC();
                    var web3 = new Web3();
                    var addresses = (web3.Eth.Accounts.SendRequestAsync()).Result;
                    var alice = addresses[0];
                    var retailBank = addresses[1];
                    var commercialBank = addresses[2];

                    var kycAttributeHash = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096dc3ff";
                    var endorsementHash = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096d54d5";

                    //smartIdentity.GetOwnerAsync()

                    // Deploy the contract and add the multiplier to the constructor
                   var gas= web3.Eth.DeployContract.EstimateGasAsync(SmartIdentityService.ABI, SmartIdentityService.BYTE_CODE,
                        alice).Result;
                    var transactionHash =
                        SmartIdentityService.DeployContractAsync(web3, alice, gas).Result;
                    Assert.IsNotNull(transactionHash);
                    var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
                    Assert.IsNotNull(receipt);
                }
                finally
                {
                    testrpcRunner.StopTestRPC();
                }
            }
        }

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