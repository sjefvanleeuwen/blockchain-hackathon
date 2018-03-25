using System;
using DefaultNamespace;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using TechTalk.SpecFlow;
using Wigo4It.Blockchain.Core.Contracts;

namespace Wigo4It.Blockchain.Core.Tests
{
    [Binding]
    public class SelfRelianceSteps
    {
        [Given(@"""(.*)"" gets an indication for the social care act from ""(.*)""")]
        public void GivenGetsAnIndicationForTheSocialCareActFrom(string p0, string p1)
        {
            // deploy needed contracts
            var web3 = new Web3();
            var addressFrom = web3.Eth.Accounts.SendRequestAsync().Result[0];

            var tr1Hash = SmartIdentityRegistryService.DeployContractAsync(web3, addressFrom, new HexBigInteger(900000)).Result;
            var receipt = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(tr1Hash).Result;
       
           // var tr2Hash = SmartIdentityService.DeployContractAsync(web3, addressFrom, new HexBigInteger(900000)).Result;
           // var receipt2 = web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(tr2Hash).Result;

            //  var tr3Hash = MigrationsService.DeployContractAsync(web3, addressFrom, new HexBigInteger(1)).Result;
            // endRequestAndWaitForReceiptAsync
            //var transactionHash = await
            //    web3.Eth.DeployContract.SendRequestAsync(abi, contractByteCode, addressFrom,
            //        new HexBigInteger(900000), multiplier);

            //var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);

            //ScenarioContext.Current.Add("multiplicationContract", web3.Eth.GetContract(abi, receipt.ContractAddress));


        }

        [Given(@"""(.*)"" gives an endorsement for product ""(.*)""")]
        public void GivenGivesAnEndorsementForProduct(string p0, string p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"""(.*)"" asks if the endorsement is valid")]
        public void WhenAsksIfTheEndorsementIsValid(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the result should be ""(.*)""")]
        public void ThenTheResultShouldBe(string p0)
        {
            ScenarioContext.Current.Pending();
        }

        //[BeforeFeature("needsTestRPC")]
        //public static void StartTestRpc()
        //{
        //    GlobalFeatureSteps.StartTestRpc();
        //}

        //[AfterFeature("needsTestRPC")]
        //public static void StopTestRpc()
        //{
        //    GlobalFeatureSteps.StopTestRpc();
        //}
    }
}
