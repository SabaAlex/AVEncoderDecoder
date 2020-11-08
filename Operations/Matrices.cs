using System;

namespace AVEncoderDecoder.Operations
{
    public static class Matrices
    {
        public static int[,] DivideMatricesElements(int[,] A, double[,] B)
        {
            int[,] result = new int[8, 8];
            double resultHolder;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    resultHolder = A[i, j] / B[i, j];

                    if (resultHolder < 0)
                        result[i, j] = (int)Math.Ceiling(resultHolder);
                    else
                        result[i, j] = (int)Math.Floor(resultHolder);
                }

            return result;
        }

        public static int[,] MultiplyMatricesElements(int[,] A, double[,] B)
        {
            int[,] result = new int[8, 8];

            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    result[i, j] = (int)(A[i, j] * B[i, j]);

            return result;
        }
    }
}
