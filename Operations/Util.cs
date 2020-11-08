using System;

namespace AVEncoderDecoder.Operations
{
    public static class Util
    {
        #region Util FDCT utils
        public static double OuterFDCT(double[,] matrix, int u, int v)
        {
            double sum = 0.0;
            for (int x = 0; x < 8; x++)
                sum += InnerFDCT(matrix, u, v, x);
            return sum;
        }

        public static double InnerFDCT(double[,] matrix, int u, int v, int x)
        {
            double sum = 0.0;
            for (int y = 0; y < 8; y++)
                sum += ProductFDCT(matrix[x, y], x, y, u, v);
            return sum;
        }

        public static double ProductFDCT(double matrixValue, int x, int y, int u, int v)
        {
            double cosU = Math.Cos((2 * x + 1) * u * Math.PI / 16);

            double cosV = Math.Cos((2 * y + 1) * v * Math.PI / 16);

            return matrixValue * cosU * cosV;
        }

        #endregion

        #region Util IDCT utils

        public static double OuterIDCT(int[,] matrix, int x, int y)
        {
            double sum = 0.0;
            for (int u = 0; u < 8; u++)
                sum += InnerIDCT(matrix, x, y, u);
            return sum;
        }

        public static double InnerIDCT(int[,] matrix, int x, int y, int u)
        {
            double sum = 0.0;
            for (int v = 0; v < 8; v++)
                sum += ProductIDCT(matrix[u, v], x, y, u, v);
            return sum;
        }

        public static double ProductIDCT(double matrixValue, int x, int y, int u, int v)
        {
            double cosU = Math.Cos((2 * x + 1) * u * Math.PI / 16);

            double cosV = Math.Cos((2 * y + 1) * v * Math.PI / 16);

            return Alpha(u) * Alpha(v) * matrixValue * cosU * cosV;
        }
        #endregion

        public static double Alpha(int value)
        {
            return value > 0 ? 1 : (1 / Math.Sqrt(2.0));
        }
    }
}
