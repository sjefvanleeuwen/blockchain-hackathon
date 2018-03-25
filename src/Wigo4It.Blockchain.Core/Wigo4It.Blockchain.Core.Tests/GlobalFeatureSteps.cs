using Nethereum.TestRPCRunner;
using TechTalk.SpecFlow;

namespace Wigo4It.Blockchain.Core.Tests
{
    public static class GlobalFeatureSteps
    {
        //NOTE: We are using [assembly: CollectionBehavior(DisableTestParallelization = true)]
        // so we can avoid clashing on different instances of test rpc
        // and run the tests sequentially
        // another option would be to execute testrpc and assign port numbers using a safe thread Interlocked.Increment.
        public static void StartTestRpc()
        {
            var testrpcRunner = new TestRPCEmbeddedRunner { RedirectOuputToDebugWindow = true };
            testrpcRunner.StartTestRPC();
            FeatureContext.Current.Add("testRpc", testrpcRunner);
        }

        public static void StopTestRpc()
        {
            if (!(FeatureContext.Current["testRpc"] is TestRPCEmbeddedRunner testRpc)) return;
            testRpc.StopTestRPC();
            testRpc.Dispose();
        }

    }
}
