using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.TestRPCRunner;
using Nethereum.Web3;
using Wigo4It.Blockchain.Core.Contracts;
using Wigo4It.Blockchain.Core.Contracts.EphemerisLynx;
using Wigo4It.Blockchain.Core.Extensions;

namespace Wigo4It.Blockchain.Core.Tests.UnitTests.EphemerisLynx
{
    [TestClass]
    public class AttributeTest : TestBase
    {
        private CertificateService _attributeOwnedCertificate;
        private CertificateService _certificateNotOwnedByAttribute;

        [TestMethod]
        public void ShouldProperlyExecuteAllOrderedTests()
        {
            Startup();
            var s = DeployAttributeAsync("somewhere", "somehash").Result;

        }

        private async Task<AttributeService> DeployAttributeAsync(string location, string hash)
        {
            string owner = Address[1];
            string transactionHash = 
                AttributeService.DeployContractAsync(
                    Web3, owner, location,owner.ConvertToByteArray(), hash, owner,
                    new HexBigInteger(1)).Result;

            TransactionReceipt receipt = await
                Web3.Eth.Transactions.GetTransactionReceipt.
                    SendRequestAsync(transactionHash);

            return new AttributeService(Web3, "attribute1", receipt.ContractAddress);
        }
    }
}
