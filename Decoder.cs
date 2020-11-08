using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab_CSharp
{
    class Decoder
    {
        public EncodingBlocks EncodedBlocks { get; set; }
        public PPM Image { get; set; }

        public Decoder() { }

        private double[,] DecodeBlock(List<Block> encodedBlocks)
        {
            double[,] matrix = new double[Image.Height, Image.Width];

            int line = 0;
            int column = 0;

            foreach (var block in encodedBlocks)
            {
                for (int i = 0; i < 8; i++) 
                    for (int j = 0; j < 8; j++)
                            matrix[line + i, column + j] = block.Storage[i,j];
                        

                column += 8;

                if (column == Image.Width)
                {
                    line += 8;
                    column = 0;
                }

            }

            return matrix;
        }

        public void StartDecoding()
        {
            Image.YUVMatrix.Y = DecodeBlock(EncodedBlocks.EncodedY);
            Image.YUVMatrix.U = DecodeBlock(EncodedBlocks.EncodedU);
            Image.YUVMatrix.V = DecodeBlock(EncodedBlocks.EncodedV);

            Image.InitRGBromYUV();
        }
    }
}
