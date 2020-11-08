using AVEncoderDecoder.Blocks;
using AVEncoderDecoder.Image;
using AVEncoderDecoder.Operations;
using System.Collections.Generic;

namespace AVEncoderDecoder
{
    class Decoder
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

        public EncodingBlocks DecodedBlocks { get; set; }
        public PPM Image { get; set; }

        public Decoder() { }

        private double[,] DecodeBlocks(List<Block> encodedBlocks)
        {
            double[,] matrix = new double[Image.Height, Image.Width];

            int line = 0;
            int column = 0;

            encodedBlocks.ForEach(encodedBlock => {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        matrix[line + i, column + j] = encodedBlock.GStorage[i, j];


                column += 8;

                if (column == Image.Width)
                {
                    line += 8;
                    column = 0;
                }
            });

            return matrix;
        }

        public void StartDecoding()
        {
            Dequantizatize(DecodedBlocks.EncodedY);
            Dequantizatize(DecodedBlocks.EncodedU);
            Dequantizatize(DecodedBlocks.EncodedV);

            IDCTBlocks(DecodedBlocks.EncodedY);
            IDCTBlocks(DecodedBlocks.EncodedU);
            IDCTBlocks(DecodedBlocks.EncodedV);

            DecodedBlocks.AddToAll(128);

            Image.YUVMatrix.Y = DecodeBlocks(DecodedBlocks.EncodedY);
            Image.YUVMatrix.U = DecodeBlocks(DecodedBlocks.EncodedU);
            Image.YUVMatrix.V = DecodeBlocks(DecodedBlocks.EncodedV);

            Image.InitRGBromYUV();
        }

        #region Decoder IDCT
        private void IDCTBlocks(List<Block> decodedBlocks)
            => decodedBlocks.ForEach(decodedBlock => {
                decodedBlock.GStorage = IDCT(decodedBlock.GStorage);
            });

        private int[,] IDCT(int[,] matrix)
        {
            int[, ] F = new int[8, 8];
            double constant = (double)1 / 4;

            for (int x = 0; x < 8; x++)
                for (int y = 0; y < 8; y++)
                    F[x, y] = (int)(constant * Util.OuterIDCT(matrix, x, y));

            return F;
        }
        #endregion

        #region Decoder Dequantization

        private void Dequantizatize(List<Block> decodedBlocks)
            => decodedBlocks.ForEach(decodedBlock => {
                decodedBlock.GStorage = Matrices.MultiplyMatricesElements(decodedBlock.GStorage, Q);
            });
        #endregion
    }
}
