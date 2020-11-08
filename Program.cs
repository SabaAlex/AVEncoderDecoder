using System;

namespace Lab_CSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var image = PPMStream.ReadPPM("nt-P3.ppm");

            var encoder = new Encoder(image);

            encoder.StartEncoding();

            var decoder = new Decoder()
            {
                EncodedBlocks = encoder.EncodingBlocks,
                Image = new PPM(image.FileName, image.Width, image.Height, image.MaxValue),
            };

            decoder.StartDecoding();

            decoder.Image.WritePPM();
        }
    }
}
