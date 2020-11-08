using AVEncoderDecoder.Blocks;
using AVEncoderDecoder.Image;
using AVEncoderDecoder.Operations;
using System.Collections.Generic;
using System.Linq;

namespace AVEncoderDecoder
{
    class Encoder
    {
        private static double[,] Q = {
            {6, 4,  4,  6,  10, 16, 20, 24},
            {5, 5,  6,  8,  10, 23, 24, 22},
            {6, 5,  6,  10, 16, 23, 28, 22},
            {6, 7,  9,  12, 20, 35, 32, 25},
            {7, 9,  15, 22, 27, 44, 41, 31},
           {10, 14, 22, 26, 32, 42, 45, 37},
           {20, 26, 31, 35, 41, 48, 48, 40},
           {29, 37, 38, 39, 45, 40, 41, 40}
        };

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
            EncodingBlocks.EncodedU = ResizeListOfBlocks(Image.SplitInBlocks(StorageType.U, Image.YUVMatrix.U));
            EncodingBlocks.EncodedV = ResizeListOfBlocks(Image.SplitInBlocks(StorageType.V, Image.YUVMatrix.V));

            EncodingBlocks.SubtractFromAll(128);

            FDCTBlocks(EncodingBlocks.EncodedY);
            FDCTBlocks(EncodingBlocks.EncodedU);
            FDCTBlocks(EncodingBlocks.EncodedV);

            Quantize(EncodingBlocks.EncodedY);
            Quantize(EncodingBlocks.EncodedU);
            Quantize(EncodingBlocks.EncodedV);
        }

        #region Encoder Resize

        private List<Block> ResizeListOfBlocks(List<Block> blocks)
            => blocks.Select(block => block.ResizeBlock()).ToList();
        #endregion

        #region Endcoder FDCT
        private void FDCTBlocks(List<Block> encodedBlocks)
            => encodedBlocks.ForEach(encodedBlock => encodedBlock.GStorage = FDCT(encodedBlock.Storage));

        int[,] FDCT(double[,] matrix)
        {
            int[,] G = new int[8, 8];
            double constant = (double)1 / 4;

            for (int u = 0; u < 8; u++)
                for (int v = 0; v < 8; v++)
                    G[u,v] = (int)(constant * Util.Alpha(u) * Util.Alpha(v) * Util.OuterFDCT(matrix, u, v));

            return G;
        }

        #endregion

        #region Encoder Quantization

        private void Quantize(List<Block> encodedBlocks)
            => encodedBlocks.ForEach(encodedBlock => {
                encodedBlock.GStorage = Matrices.DivideMatricesElements(encodedBlock.GStorage, Q);
            });

        #endregion
    }
}
