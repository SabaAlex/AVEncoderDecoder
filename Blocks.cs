using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lab_CSharp
{
    public static class Blocks
    {
        public static List<Block> SplitInBlocks(this PPM image, StorageType type, double[,] matrix)
        {
            List<Block> encoded = new List<Block>();

            for (int i = 0; i < image.Height; i += 8)
                for (int j = 0; j < image.Width; j += 8)
                {
                    Block store = SubMatrix(type, i, j, matrix);
                    if (type == StorageType.Y)
                        encoded.Add(store);
                    else
                        encoded.Add(store.BlockAverage420().ResizeBlock());
                }
            
           return encoded;

        }

        private static Block SubMatrix(StorageType type, int iStartPos, int jStartPos, double[,] matrix)
        {
            Block store = new Block(8, type);

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    store.Storage[i, j] = matrix[i + iStartPos, j + jStartPos];

            return store;
        }

        public static Block BlockAverage420(this Block block) {

            Block sampleStore = new Block(4, block.StoreType);
            int line = 0;
            int column = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    sampleStore.Storage[i, j] = 
                        (   block.Storage[line,     column] +
                            block.Storage[line,     column + 1] +
                            block.Storage[line + 1, column] +
                            block.Storage[line + 1, column + 1])
                            / 4.0;

                    column += 2;
                }
                line += 2;
                column = 0;
            }
            return sampleStore;
        }

        public static Block ResizeBlock(this Block blockStore)
        {
            Block sampleStore = new Block(8, blockStore.StoreType);

            int line = 0;
            int column = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double value = blockStore.Storage[i, j];

                    sampleStore.Storage[line,       column] = value;
                    sampleStore.Storage[line,       column + 1] = value;
                    sampleStore.Storage[line + 1,   column] = value;
                    sampleStore.Storage[line + 1,   column + 1] = value;

                    column += 2;
                }
                line += 2;
                column = 0;
            }

            return sampleStore;
        }
    }
}
