﻿using System.Threading.Tasks;

namespace Nethereum.BlockchainProcessing.Processing
{
    public interface IWaitStrategy
    {
        Task Apply(uint retryCount);
    }
}