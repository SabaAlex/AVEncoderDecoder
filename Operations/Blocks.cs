using AVEncoderDecoder.Blocks;
using AVEncoderDecoder.Image;
using System.Collections.Generic;

namespace AVEncoderDecoder.Operations
{
    public static class Blocks
    {
        #region Block Spliting
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
                        encoded.Add(store.BlockAverage420());
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
        #endregion

        #region Block Resizing
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

        #endregion

        #region Block Value Manipulation
        public static void AddValueToGStore(this Block block, int value)
        {
            for (int i = 0; i < block.Size; i++)

                for (int j = 0; j < block.Size; j++)

                    block.GStorage[i, j] = block.GStorage[i, j] + value;
        }

        public static void SubtractValueFromStore(this Block block, double value)
        {
            for (int i = 0; i < block.Size; i++)

                for (int j = 0; j < block.Size; j++)

                    block.Storage[i, j] = block.Storage[i, j] - value;
        }

        #endregion
    }
}
