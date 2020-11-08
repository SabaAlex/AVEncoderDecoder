using Lab_CSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class PPMStream
{
    private static string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

    public static PPM ReadPPM(string file)
    {
        using (StreamReader reader = new StreamReader(Path.Combine(projectPath, file)))
        {
            if (reader.ReadLine() != "P3")
                return null;

            int width, height;

            var ListForRatio = reader.ReadLine().Split(" ").ToList();

            if (!int.TryParse(ListForRatio[0].Trim(), out width) || !int.TryParse(ListForRatio[1].Trim(), out height))
                return null;

            var matrixMapR = new int[height, width];
            var matrixMapG = new int[height, width];
            var matrixMapB = new int[height, width];

            int maxValue;

            if (!int.TryParse(reader.ReadLine(), out maxValue))
                return null;

            //Read in the pixels
            for (int i = 0; i < height; i++)

                for (int j = 0; j < width; j++)
                {
                    matrixMapR[i, j] = int.Parse(reader.ReadLine());
                    matrixMapG[i, j] = int.Parse(reader.ReadLine());
                    matrixMapB[i, j] = int.Parse(reader.ReadLine());
                }

            var colorsRGB = new RGBColors()
            {
                R = matrixMapR,
                G = matrixMapG,
                B = matrixMapB,
            };

            return new PPM(file, width, height, maxValue)
            {
                RGBMatrix = colorsRGB
            };
        }
    }

    public static void WritePPM(this PPM image)
    {
        var complete = Path.Combine(projectPath, "Output\\");

        Directory.CreateDirectory(complete);

        using (FileStream fileStream = File.OpenWrite(Path.Combine(complete, $"{image.FileName}")))
        {
            var R = image.RGBMatrix.R;
            var G = image.RGBMatrix.G;
            var B = image.RGBMatrix.B;


            fileStream.Write(Encoding.ASCII.GetBytes("P3\n"));
            fileStream.Write(Encoding.ASCII.GetBytes("\n"));
            fileStream.Write(Encoding.ASCII.GetBytes($"{image.Width} {image.Height}\n"));
            fileStream.Write(Encoding.ASCII.GetBytes($"{image.MaxValue}\n"));


            for (int i = 0; i < image.Height; i++)
                for (int j = 0; j < image.Width; j++)
                {
                    fileStream.Write(Encoding.ASCII.GetBytes($"{R[i, j]}\n"));
                    fileStream.Write(Encoding.ASCII.GetBytes($"{G[i, j]}\n"));
                    fileStream.Write(Encoding.ASCII.GetBytes($"{B[i, j]}\n"));
                }
        }
    }
}