﻿using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nethereum.BlockchainProcessing.Processing
{
    public class JsonBlockProgressRepository : IBlockProgressRepository
    {
        public class BlockProcessingProgress
        {
            public ulong? To { get; set; }
        }

        private BlockProcessingProgress _progress;

        public string JsonFileNameAndPath { get; } 

        public JsonBlockProgressRepository(string jsonFileNameAndPath)
        {
            this.JsonFileNameAndPath = jsonFileNameAndPath;
            Initialise();
        }

        public Task<ulong?> GetLastBlockNumberProcessedAsync()
        {
            return Task.FromResult(_progress.To);
        }

        public Task UpsertProgressAsync(ulong blockNumber)
        {
            _progress.To = blockNumber;
            Persist();
            return Task.CompletedTask;
        }

        private void Persist()
        {
            File.WriteAllText(JsonFileNameAndPath, JsonConvert.SerializeObject(_progress));
        }

        private void Initialise()
        {
            if (!File.Exists(JsonFileNameAndPath))
            {
                CreateAndPersist();
            }
            else
            {
                Load();
                if (_progress == null)
                {
                    CreateAndPersist();
                }
            }
        }

        private void Load()
        {
            var json = File.ReadAllText(JsonFileNameAndPath);
            _progress = JsonConvert.DeserializeObject<BlockProcessingProgress>(json);
        }

        private void CreateAndPersist()
        {
            _progress = new BlockProcessingProgress();
            Persist();
        }
    }
}