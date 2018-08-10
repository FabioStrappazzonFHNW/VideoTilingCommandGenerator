using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTilingCommandGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("usage: VideoTilingCommandGenerator tileResolution [tilings...]\n\n"+
                    "tileResolution:\tResolution in Pixel of each tile\n"+
                    "tilings: list of tilings\n\n"+
                    "example: VideoTilingCommandGenerator 512 1 2 4 8 (generates a command that will create 85 tiles with 512x512 resolution each)");
            }
            else
            {
                var resolution = int.Parse(args[0]);
                int[] tilings = new int[args.Length-1];

                for(int i = 1; i < args.Length; i++)
                {
                    tilings[i - 1] = int.Parse(args[i]);
                }
                var command = BuildCommand(resolution, tilings);
                Console.WriteLine(command);
            }
        }

        public static string BuildCommand(int resolution, int[] dimensions)
        {
            int totalTiles = 0;
            foreach (int d in dimensions)
            {
                totalTiles += d * d;
            }
            string command = $"ffmpeg -y -framerate 25 -i %d.jp2 -filter_complex \"[0]format=yuv420p, split={totalTiles}";
            foreach (int d in dimensions)
            {

                for (int i = 0; i < d; i++)
                {
                    for (int j = 0; j < d; j++)
                    {
                        command += $"[{d * d}_{i}{j}]";
                    }
                }
            }
            foreach (int d in dimensions)
            {
                for (int i = 0; i < d; i++)
                {
                    for (int j = 0; j < d; j++)
                    {
                        command += $";[{d * d}_{i}{j}] crop=iw/{d}:ih/{d}:{j}*iw/{d}:{i}*ih/{d},scale={resolution}:{resolution}:flags=neighbor[{d * d}_{i}{j}]";
                    }
                }
            }
            command += "\" ";
            foreach (int d in dimensions)
            {
                for (int i = 0; i < d; i++)
                {
                    for (int j = 0; j < d; j++)
                    {
                        command += $"-c:v libx264 -preset ultrafast -profile:v high -level 5.1 -flags +cgop -g 50 -keyint_min 50 -hls_time 2 -map \"[{d * d}_{i}{j}]\" t{d * d}_{i}-{j}l.m3u8 ";
                    }
                }
            }
            return command;
        }
    }
}
