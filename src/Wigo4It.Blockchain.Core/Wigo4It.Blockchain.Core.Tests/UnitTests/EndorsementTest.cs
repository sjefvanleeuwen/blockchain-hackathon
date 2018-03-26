using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexTypes;
using Nethereum.TestRPCRunner;
using Nethereum.Web3;
using Wigo4It.Blockchain.Core.Contracts;
using Wigo4It.Blockchain.Core.Extensions;

namespace Wigo4It.Blockchain.Core.Tests.UnitTests
{
    [TestClass]
    public class EndorsementTest
    {
        private string smartIdentity;
        private string owner;
        private string endorser;
        private string thirdparty;
        private byte[] attributeHash1;
        private byte[] attributeHash2;
        private byte[] attributeHash3;
        private byte[] endorsementHash;
        private byte[] endorsementHash2;
        private TestContext _testContext;
        private TestRPCEmbeddedRunner rpc;
        private Web3 web3;
        private HexBigInteger gas;


        [TestMethod]
        public void ShouldProperlyExecuteAllOrderedTests()
        {
            attributeHash1 = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096d54d5".ConvertToByteArray();
            attributeHash2 = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096dc3ff".ConvertToByteArray();
            attributeHash3 = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096c18ac".ConvertToByteArray();
            endorsementHash = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096d54d5".ConvertToByteArray();
            endorsementHash2 = "0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096d59ab".ConvertToByteArray();

            rpc = new TestRPCEmbeddedRunner { RedirectOuputToDebugWindow = true };
            rpc.StartTestRPC();

            ShouldReadAndPopulate3Accounts();
            ShouldDeployContractForOwnerWithEnoughGasAndReturnReceipt();
            ShouldAddAnAttributeForOwner();

            rpc.Dispose();
        }

        public void ShouldReadAndPopulate3Accounts()
        {
            web3 = new Web3();
            var addresses = (web3.Eth.Accounts.SendRequestAsync()).Result;
            owner = addresses[0];
            endorser = addresses[1];
            thirdparty = addresses[2];
        }

        public void ShouldDeployContractForOwnerWithEnoughGasAndReturnReceipt()
        {
            gas = web3.Eth.DeployContract.EstimateGasAsync(SmartIdentityService.ABI, SmartIdentityService.BYTE_CODE,
                owner).Result;
            var transactionHash =
                SmartIdentityService.DeployContractAsync(web3, owner, gas).Result;
            Assert.IsNotNull(transactionHash);
            var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
            Assert.IsNotNull(receipt);
        }

        public void ShouldAddAnAttributeForOwner()
        {
            var s = new SmartIdentityService(web3, owner);
            var t = s.AddAttributeAsync(owner, attributeHash1,gas).Result;
        }
    }
}
