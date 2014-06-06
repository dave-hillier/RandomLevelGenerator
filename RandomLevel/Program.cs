using System;
using System.Linq;
using System.Text;

namespace RandomLevel
{
    internal class MainClass
    {
        public static void Main(string[] args)
        {
            int size = 70;
            var level = new Level(123, 70);

            var placementStrategy = new ReflectedAndFlippedPlacementStrategy(level, numberOfRooms: size/2);
            placementStrategy.PlaceLevel();

            var output = level.ToString();
            Console.WriteLine(output);
            Console.WriteLine();
            Console.WriteLine();


            var tileCodes = level.ToTileCodes();

            WriteEncodedLevel(tileCodes);


            string myLevel = @"XXXXXXXXXXXXXXXXXXXXX
X                   X
X   X XXXXXXXXXXX   X
X   X XXXXXXXXXXX   X
XX XX       XXX   X X
XX XX XXXXX     XXX X
XX XX XXXXX XXXXXXX X
XX    XX     XXX    X
XXXXX     XXXXXX XXXX
XXXXXXX XXXXXXXX XXXX
XX      XX   X   XXXX
XX XXXX XX   X X XXXX
XX XXXX   XX   X    X
XX XXXXXX XXXX XXXX X
XX   XXXX    X XXXX X
XX    XXXXXX XXX    X
XXXXX XX     XXXXXX X
X   X XX XX XXXXX   X
X        XX         X
X   XXXXXXXXXXXXX   X
XXXXXXXXXXXXXXXXXXXXX
";
            var chars = To2dArray(myLevel);
            var codes = new TestGrid { Grid = chars, SizeX = 21, SizeY = 21 }.ToTileCodes();

            Console.WriteLine();
            Console.WriteLine();

            WriteEncodedLevel(codes);

        }

        private static char[,] To2dArray(string myLevel)
        {
            var x =
                myLevel.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().TrimEnd('\n').ToCharArray()).ToArray();
            var codes = new char[x.Length, x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    codes[i, j] = x[i][j];
                }
            }
            return codes;
        }

        private static void WriteEncodedLevel(TileFlags[,] tileCodes)
        {
            var sb = new StringBuilder();
            int k = 0;
            for (int i = 0; i < tileCodes.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < tileCodes.GetUpperBound(1) + 1; j++)
                {
                    var v = (int) tileCodes[i, j];
                    sb.AppendFormat("{0}, ", v);
                    ++k;
                }
                sb.AppendLine();
            }

            Console.WriteLine(sb.ToString());
        }

        private class TestGrid : IGrid
        {
            public int SizeX { get; set; }
            public int SizeY { get; set; }
            public char[,] Grid { get; set; }
        }
    }
}