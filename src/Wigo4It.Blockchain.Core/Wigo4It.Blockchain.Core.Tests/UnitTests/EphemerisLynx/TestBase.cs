using Nethereum.ABI;
using Nethereum.TestRPCRunner;
using Nethereum.Web3;

namespace Wigo4It.Blockchain.Core.Tests.UnitTests.EphemerisLynx
{
    public abstract class TestBase
    {
        public string[] Address { get; set; }
        public Web3 Web3 { get; set; }
        public TestRPCEmbeddedRunner Rpc { get; set; }

        public async void Startup()
        {
            Rpc = new TestRPCEmbeddedRunner { RedirectOuputToDebugWindow = true };
       
            Rpc.StartTestRPC();
            Web3 = new Web3();

            Address = Web3.Eth.Accounts.SendRequestAsync().Result;
        }
    }
}

