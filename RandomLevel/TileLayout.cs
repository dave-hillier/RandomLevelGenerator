using System;
using System.Collections.Generic;

namespace RandomLevel
{
    internal class TileLayout
    {
        class TileFlagsPair
        {
            public string Name;
            public TileFlags Flags;
        }

        private static readonly TileFlagsPair[] Tiles =
        {
            new TileFlagsPair {Name = TileConstants.EmptyTile, Flags = TileFlags.North | TileFlags.NorthEast | TileFlags.NorthWest | TileFlags.West | TileFlags.Center | TileFlags.East | TileFlags.South | TileFlags.SouthEast },
            new TileFlagsPair {Name = TileConstants.WallWithEntranceTile, Flags = TileFlags.North | TileFlags.NorthEast | TileFlags.West | TileFlags.Center | TileFlags.East | TileFlags.South | TileFlags.SouthEast },
            new TileFlagsPair {Name = TileConstants.TwoExitsTile, Flags = TileFlags.North | TileFlags.Center | TileFlags.East | TileFlags.South | TileFlags.West | TileFlags.SouthEast},
            new TileFlagsPair {Name = TileConstants.WallTile, Flags = TileFlags.North | TileFlags.Center | TileFlags.South | TileFlags.NorthEast | TileFlags.East | TileFlags.SouthEast },
            new TileFlagsPair {Name = TileConstants.CornerCorridorTile, Flags = TileFlags.West | TileFlags.Center| TileFlags.East | TileFlags.North | TileFlags.NorthEast },
            new TileFlagsPair {Name = TileConstants.CornerCorridor2Tile, Flags = TileFlags.West | TileFlags.Center| TileFlags.East | TileFlags.South | TileFlags.SouthEast},
            new TileFlagsPair {Name = TileConstants.CrossRoadTile, Flags = TileFlags.North | TileFlags.Center | TileFlags.East | TileFlags.South | TileFlags.West },
            new TileFlagsPair {Name = TileConstants.CornerTile, Flags = TileFlags.Center | TileFlags.East | TileFlags.South | TileFlags.SouthEast},
            new TileFlagsPair {Name = TileConstants.TJunctionTile, Flags = TileFlags.North | TileFlags.Center | TileFlags.East | TileFlags.South },
            new TileFlagsPair {Name = TileConstants.RightTurnTile, Flags = TileFlags.South | TileFlags.Center | TileFlags.East },
            new TileFlagsPair {Name = TileConstants.CorridorTile, Flags = TileFlags.North | TileFlags.Center | TileFlags.South },
            new TileFlagsPair {Name = TileConstants.CorridorEndTile, Flags = TileFlags.West | TileFlags.Center },            
            new TileFlagsPair {Name = TileConstants.CornerOuterTile, Flags = TileFlags.SouthWest | TileFlags.Center},

            new TileFlagsPair {Name = TileConstants.Filled, Flags = TileFlags.Empty},
        };

        public static IEnumerable<Tile> GetTilesAssets(TileFlags[,] tileCodes)
        {
            var sizeX = tileCodes.GetUpperBound(0);
            var sizeY = tileCodes.GetUpperBound(1);

            WriteTileCodes(sizeX, sizeY, tileCodes);

            var tiles = new List<Tile>();
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Tile tile;
                    if (TryMatchTile(i, j, out tile, tileCodes[i, j]))
                        tiles.Add(tile);
                }
            }
            return tiles;
        }

        public static bool TryMatchTile(int i, int j, out Tile tile, TileFlags tileFlags) 
        {
            foreach (var tileFlagsPair in Tiles)
            {
                var match = TryRotateMatchTile(tileFlagsPair, i, j, out tile, tileFlags);
                if (match)
                    return true;
            }
            tile = null;
            return false;
        }

        private static bool TryRotateMatchTile(TileFlagsPair tileFlagsPair, int i, int j, out Tile tile, TileFlags tileFlags)
        {
            var flags = tileFlagsPair.Flags;
            for (int r = 0; r < 4; ++r)
            {
                bool match = (flags & tileFlags) == 0;

                if (match)
                {
                    tile = new Tile {TileResource = tileFlagsPair.Name, X = i, Z = j, Orientation = r*90};
                    return true;
                }

                flags = RotateClockwise(flags);
            }
            tile = null;
            return false;
        }

        private static void WriteTileCodes(int sizeX, int sizeY, TileFlags[,] tileCodes)
        {
            Console.WriteLine("Tiles");
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    Console.Write("{0:X2} ", (byte)tileCodes[i, j]);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public static TileFlags RotateClockwise(TileFlags flags)
        {
            var value = (byte)flags;
            var rotated = (byte)((value << 2) | (value >> (8 - 2)));
            return flags & TileFlags.Center | (TileFlags)rotated;
        }

        public class Tile
        {
            public int X { get; set; }
            public int Z { get; set; }
            public string TileResource { get; set; }
            public int Orientation { get; set; }
        }
    }

}