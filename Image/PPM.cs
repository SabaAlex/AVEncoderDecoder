using AVEncoderDecoder.Image.Colors;
using System;

namespace AVEncoderDecoder.Image
{
    public class PPM
    {
        public string FileName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MaxValue { get; set; }
        public string Format { get; set; }

        public RGBColors RGBMatrix { get; set; }
        public YUVColors YUVMatrix { get; set; }

        public PPM(string filename, int width, int height, int maxValue)
        {
            FileName = filename;
            Width = width;
            Height = height;
            MaxValue = maxValue;
            YUVMatrix = new YUVColors();
            RGBMatrix = new RGBColors();
        }

        private static double[,] RGBConverstionMatrix = new double[3, 3]
        {
                { 0.299,                      - 0.168736,                    0.5        },      //R
                { 0.587,                      - 0.331264,                  - 0.418688   },      //G
                { 0.114,                        0.5,                       - 0.081312   }       //B
        };


        private static double[,] YUVConverstionMatrix = new double[3, 3]
        {
                { 1,                   1,                   1       },      //Y
                { 0,                 - 0.344136,            1.7790  },      //U
                { 1.4019,            - 0.714136,            0       }       //V
        };

        public void InitYUVFromRGB()
        {
            var matrixMapY = new double[Height, Width];
            var matrixMapU = new double[Height, Width];
            var matrixMapV = new double[Height, Width];

            for (int i = 0; i < Height; i++)
            {

                for (int j = 0; j < Width; j++)
                {
                    matrixMapY[i, j] =          + RGBConverstionMatrix[0, 0] * RGBMatrix.R[i, j]
                                                + RGBConverstionMatrix[1, 0] * RGBMatrix.G[i, j]
                                                + RGBConverstionMatrix[2, 0] * RGBMatrix.B[i, j];

                    matrixMapU[i, j] = 128      + RGBConverstionMatrix[0, 1] * RGBMatrix.R[i, j]
                                                + RGBConverstionMatrix[1, 1] * RGBMatrix.G[i, j]
                                                + RGBConverstionMatrix[2, 1] * RGBMatrix.B[i, j];

                    matrixMapV[i, j] = 128      + RGBConverstionMatrix[0, 2] * RGBMatrix.R[i, j]
                                                + RGBConverstionMatrix[1, 2] * RGBMatrix.G[i, j]
                                                + RGBConverstionMatrix[2, 2] * RGBMatrix.B[i, j];
                }
            }

            YUVMatrix = new YUVColors()
            {
                Y = matrixMapY,
                U = matrixMapU,
                V = matrixMapV,
            };
        }

        public void InitRGBromYUV()
        {

            var matrixMapR = new int[Height, Width];
            var matrixMapG = new int[Height, Width];
            var matrixMapB = new int[Height, Width];

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    var R =     + YUVConverstionMatrix[0, 0] * YUVMatrix.Y[i, j]
                                + YUVConverstionMatrix[1, 0] * (YUVMatrix.U[i, j] - 128)
                                + YUVConverstionMatrix[2, 0] * (YUVMatrix.V[i, j] - 128);

                    var G =     + YUVConverstionMatrix[0, 1] * YUVMatrix.Y[i, j]
                                + YUVConverstionMatrix[1, 1] * (YUVMatrix.U[i, j] - 128)
                                + YUVConverstionMatrix[2, 1] * (YUVMatrix.V[i, j] - 128);

                    var B =     + YUVConverstionMatrix[0, 2] * YUVMatrix.Y[i, j]
                                + YUVConverstionMatrix[1, 2] * (YUVMatrix.U[i, j] - 128)
                                + YUVConverstionMatrix[2, 2] * (YUVMatrix.V[i, j] - 128);

                    StabilizeColor(R, out matrixMapR[i, j]);
                    StabilizeColor(G, out matrixMapG[i, j]);
                    StabilizeColor(B, out matrixMapB[i, j]);
                }
            }

            RGBMatrix = new RGBColors()
            {
                R = matrixMapR,
                G = matrixMapG,
                B = matrixMapB,
            };
        }

        private static void StabilizeColor(double color, out int stabilizedColor)
        {
            stabilizedColor = color switch
            {
                double value when value > 255 => 255,
                double value when value < 0 => 0,
                _ => Convert.ToInt32(color),
            };
        }
    }
}
