using System;
using System.Collections.Generic;
using System.Text;

namespace Lab_CSharp
{
    class Encoder
    {
        private PPM Image { get; set; }
        public EncodingBlocks EncodingBlocks { get; set; }

        public Encoder(PPM image)
        {
            Image = image;

            EncodingBlocks = new EncodingBlocks();
        }

        public void StartEncoding()
        {
            Image.InitYUVFromRGB();

            EncodingBlocks.EncodedY = Image.SplitInBlocks(StorageType.Y, Image.YUVMatrix.Y);
            EncodingBlocks.EncodedU = Image.SplitInBlocks(StorageType.U, Image.YUVMatrix.U);
            EncodingBlocks.EncodedV = Image.SplitInBlocks(StorageType.V, Image.YUVMatrix.V);
        }
    }
}
