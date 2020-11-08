using AVEncoderDecoder.Image;

namespace AVEncoderDecoder
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
                DecodedBlocks = encoder.EncodingBlocks,
                Image = new PPM(image.FileName, image.Width, image.Height, image.MaxValue),
            };

            decoder.StartDecoding();

            decoder.Image.WritePPM();
        }
    }
}
