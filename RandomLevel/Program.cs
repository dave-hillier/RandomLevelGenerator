using System;

namespace RandomLevel
{
    internal class MainClass
    {

        
        public static void Main(string[] args)
        {
            var level = new Level(3, 21);

            var placementStrategy = new ReflectedAndFlippedPlacementStrategy(level, numberOfRooms: 10);
            placementStrategy.PlaceLevel();

            var output = level.ToString();
            Console.WriteLine(output);
            Console.WriteLine(); 
            Console.WriteLine();

            var tileCodes = level.ToTileCodes();

            var tiles = TileLayout.GetTilesAssets(tileCodes);

            foreach (var tile in tiles)
            {
                Console.WriteLine("        new Tile {{ Name=\"{0}\", Position=new Point({1}, {2}), Orientation={3}} }},", tile.TileResource, tile.X, tile.Z, tile.Orientation);
            }


            //Console.ReadKey();
        }
    }
}