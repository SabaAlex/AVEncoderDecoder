using AVEncoderDecoder.Operations;
using System.Collections.Generic;
using System.Linq;

namespace AVEncoderDecoder.Blocks
{
    public class EncodingBlocks
    {
        public List<Block> EncodedY { get; set; }
        public List<Block> EncodedU { get; set; }
        public List<Block> EncodedV { get; set; }

        public EncodingBlocks()
        {
            EncodedY = new List<Block>();
            EncodedU = new List<Block>();
            EncodedV = new List<Block>();
        }

        public void SubtractFromAll(double value)
        {
            EncodedY = SubtractFromEachBlock(EncodedY, value);
            EncodedU = SubtractFromEachBlock(EncodedU, value);
            EncodedV = SubtractFromEachBlock(EncodedV, value);
        }

        public void AddToAll(int value)
        {
            EncodedY = AddToEachBlock(EncodedY, value);
            EncodedU = AddToEachBlock(EncodedU, value);
            EncodedV = AddToEachBlock(EncodedV, value);
        }

        private List<Block> SubtractFromEachBlock(List<Block> blocks, double value)
            => blocks.Select(block => {
                block.SubtractValueFromStore(value);
                return block;
            }).ToList();

        private List<Block> AddToEachBlock(List<Block> blocks, int value)
            => blocks.Select(block => {
                block.AddValueToGStore(value);
                return block;
            }).ToList();
    }
}
