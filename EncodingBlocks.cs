using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab_CSharp
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
    }
}
