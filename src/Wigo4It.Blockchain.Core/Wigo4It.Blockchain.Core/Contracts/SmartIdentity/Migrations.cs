using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Contracts;

namespace Wigo4It.Blockchain.Core.Contracts
{
   public class MigrationsService
   {
        private readonly Web3 web3;

        public static string ABI = @"[{'constant':false,'inputs':[{'name':'new_address','type':'address'}],'name':'upgrade','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'constant':true,'inputs':[],'name':'last_completed_migration','outputs':[{'name':'','type':'uint256'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':true,'inputs':[],'name':'owner','outputs':[{'name':'','type':'address'}],'payable':false,'stateMutability':'view','type':'function'},{'constant':false,'inputs':[{'name':'completed','type':'uint256'}],'name':'setCompleted','outputs':[],'payable':false,'stateMutability':'nonpayable','type':'function'},{'inputs':[],'payable':false,'stateMutability':'nonpayable','type':'constructor'}]";

        public static string BYTE_CODE = "0x6060604052341561000f57600080fd5b60008054600160a060020a033316600160a060020a03199091161790556101e78061003b6000396000f3006060604052600436106100615763ffffffff7c01000000000000000000000000000000000000000000000000000000006000350416630900f0108114610066578063445df0ac146100875780638da5cb5b146100ac578063fdacd576146100db575b600080fd5b341561007157600080fd5b610085600160a060020a03600435166100f1565b005b341561009257600080fd5b61009a610186565b60405190815260200160405180910390f35b34156100b757600080fd5b6100bf61018c565b604051600160a060020a03909116815260200160405180910390f35b34156100e657600080fd5b61008560043561019b565b6000805433600160a060020a03908116911614156101825781905080600160a060020a031663fdacd5766001546040517c010000000000000000000000000000000000000000000000000000000063ffffffff84160281526004810191909152602401600060405180830381600087803b151561016d57600080fd5b6102c65a03f1151561017e57600080fd5b5050505b5050565b60015481565b600054600160a060020a031681565b60005433600160a060020a03908116911614156101b85760018190555b505600a165627a7a72305820accdeee8d27c44308de8e06679a7fc4b1ae7337d86093e5c897dfe6128a43fa90029";

        public static Task<string> DeployContractAsync(Web3 web3, string addressFrom,  HexBigInteger gas = null, HexBigInteger valueAmount = null) 
        {
            return web3.Eth.DeployContract.SendRequestAsync(ABI, BYTE_CODE, addressFrom, gas, valueAmount );
        }

        private Contract contract;

        public MigrationsService(Web3 web3, string address)
        {
            this.web3 = web3;
            this.contract = web3.Eth.GetContract(ABI, address);
        }

        public Function GetFunctionUpgrade() {
            return contract.GetFunction("upgrade");
        }
        public Function GetFunctionLast_completed_migration() {
            return contract.GetFunction("last_completed_migration");
        }
        public Function GetFunctionOwner() {
            return contract.GetFunction("owner");
        }
        public Function GetFunctionSetCompleted() {
            return contract.GetFunction("setCompleted");
        }


        public Task<BigInteger> Last_completed_migrationAsyncCall() {
            var function = GetFunctionLast_completed_migration();
            return function.CallAsync<BigInteger>();
        }
        public Task<string> OwnerAsyncCall() {
            var function = GetFunctionOwner();
            return function.CallAsync<string>();
        }

        public Task<string> UpgradeAsync(string addressFrom, string new_address, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionUpgrade();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, new_address);
        }
        public Task<string> SetCompletedAsync(string addressFrom, BigInteger completed, HexBigInteger gas = null, HexBigInteger valueAmount = null) {
            var function = GetFunctionSetCompleted();
            return function.SendTransactionAsync(addressFrom, gas, valueAmount, completed);
        }



    }



}

