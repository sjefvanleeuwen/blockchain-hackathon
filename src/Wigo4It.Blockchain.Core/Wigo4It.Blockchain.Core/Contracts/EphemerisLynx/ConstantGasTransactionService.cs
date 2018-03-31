﻿using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Wigo4It.Blockchain.Core.Contracts.EphemerisLynx
{
    class ConstantGasTransactionService : ITransactionService
    {
        private const int EstimateMultiplier = 3;

        private string _privateKey;
        private string _addressFrom;
        private Web3 _web3;

        public ConstantGasTransactionService(string key, Web3 web3)
        {
            _web3 = web3;
            _privateKey = key;
            _addressFrom = Web3.GetAddressFromPrivateKey(_privateKey);
        }

        public async Task<string> SignAndSendTransaction(string data, string to, HexBigInteger value = null, HexBigInteger gasPrice = null)
        {
            value = value ?? new HexBigInteger(0);
            gasPrice = gasPrice ?? new HexBigInteger(0);

            HexBigInteger gasLimit = new HexBigInteger(4000000);

            HexBigInteger nonce = await _web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(_addressFrom);
            string transaction = Web3.OfflineTransactionSigner.SignTransaction(_privateKey, to, value, nonce, gasPrice, gasLimit, data);
            return await _web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + transaction);
        }
    }
}
