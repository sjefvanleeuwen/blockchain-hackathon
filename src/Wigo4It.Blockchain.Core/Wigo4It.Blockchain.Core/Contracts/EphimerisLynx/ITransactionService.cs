﻿using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Wigo4It.Blockchain.Core.Contracts.EphimerisLynx
{
    interface ITransactionService
    {
        Task<string> SignAndSendTransaction(string data, string to, HexBigInteger value = null, HexBigInteger gasPrice = null);
    }
}