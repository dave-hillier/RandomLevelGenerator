using System;

namespace RandomLevel
{
    internal class MainClass
    {

        
        public static void Main(string[] args)
        {
            var level = new Level(3);

            var placementStrategy = new ReflectedAndFlippedPlacementStrategy(level);
            placementStrategy.PlaceLevel();

            var output = level.ToString();
            Console.WriteLine(output);
            Console.WriteLine(); 
            Console.WriteLine();

            TileLayout.Layout(level);


            //Console.ReadKey();
        }
    }
}