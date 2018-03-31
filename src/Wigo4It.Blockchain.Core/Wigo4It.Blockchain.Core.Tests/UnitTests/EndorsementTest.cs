using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
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

            Assert.IsFalse(string.IsNullOrEmpty(owner));
            Assert.IsFalse(string.IsNullOrEmpty(endorser));
            Assert.IsFalse(string.IsNullOrEmpty(thirdparty));

        }

        private TransactionReceipt _contract;

        public void ShouldDeployContractForOwnerWithEnoughGasAndReturnReceipt()
        {
            gas = web3.Eth.DeployContract.EstimateGasAsync(SmartIdentityService.ABI, SmartIdentityService.BYTE_CODE,
                owner).Result;
            var transactionHash =
                SmartIdentityService.DeployContractAsync(web3, owner, gas).Result;
            Assert.IsNotNull(transactionHash);
            _contract = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash).Result;
            Assert.IsNotNull(_contract);
        }

        public void ShouldAddAnAttributeForOwner()
        {
            var contract = web3.Eth.GetContract(SmartIdentityService.ABI, _contract.ContractAddress);
            var evt = contract.GetEvent("ChangeNotification");
            var f = contract.GetFunction("addAttribute");
           // var g = f.EstimateGasAsync().Result;
       
            //var receipt = f.SendTransactionAndWaitForReceiptAsync(
            //    owner,
            //    new HexBigInteger(900000),
            //    new HexBigInteger(1),new HexBigInteger("0xca02b2202ffaacbd499438ef6d594a48f7a7631b60405ec8f30a0d7c096d54d5")).Result;

            //var filterInput =
            //    evt.CreateFilterInput(new BlockParameter(receipt.BlockNumber), BlockParameter.CreateLatest());
            //var logs = evt.GetAllChanges<ChangeNotificationEventDTO>(filterInput).Result;
        }
    }
}
