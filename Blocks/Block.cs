namespace AVEncoderDecoder.Blocks
{
    public enum StorageType
    {
        Y,
        U,
        V
    }

    public class Block
    {
        public int Size { get; set; }
        public double[,] Storage { get; set; }
        public int[,] GStorage { get; set; }
        public StorageType StoreType { get; set; }

        public Block(int size, StorageType storeType)
        {
            Size = size;
            StoreType = storeType;

            Storage = new double[Size, Size];

            GStorage = new int[Size, Size];
        }

        public override string ToString()
        {
            var stringToPrint = "";

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    stringToPrint += $"{Storage[i, j]} ";
                }
                stringToPrint += "\n";
            }

            return stringToPrint;
        }
    }
}
