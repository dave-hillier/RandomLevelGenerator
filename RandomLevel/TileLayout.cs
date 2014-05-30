using System;
using System.Collections.Generic;

namespace RandomLevel
{
    class TileConstants
    {

    }

    class TileLayout
    {
        const string EmptyTile = "Tile_01"; // TileFlags.Empty

        const string CorridorTile = "Tile_03";
        const TileFlags CorridorFlags = TileFlags.West | TileFlags.East;

        const string WallTile = "Tile_07";
        const TileFlags WallFlags = TileFlags.West;

        const string CornerTile = "Tile_24";
        const TileFlags CornerFlags = TileFlags.West /*| TileFlags.NorthWest*/ | TileFlags.North;

        const string Filled = "Tile_21"; // TileFlags.Filled

        const string RightTurnTile = "Tile_04";
        const TileFlags RightTurnFlags =
            TileFlags.NorthWest | TileFlags.North | TileFlags.NorthEast | TileFlags.SouthEast | TileFlags.SouthWest |
            TileFlags.West;

        const string WallWithEntranceTile = "Tile_16";
        const TileFlags WallWithEntranceFlags = TileFlags.NorthWest | TileFlags.SouthWest;

        const string TJunctionTile = "Tile_10";
        const TileFlags TJunctionFlags =
            TileFlags.NorthWest | TileFlags.NorthEast | TileFlags.SouthEast | TileFlags.SouthWest | TileFlags.West;

        const string CrossRoadTile = "Tile_02";
        const TileFlags CrossRoad = TileFlags.NorthWest | TileFlags.NorthEast | TileFlags.SouthEast | TileFlags.SouthWest;

        const string TwoExitsTile = "Tile_25";
        const TileFlags TwoExitsFlags = TileFlags.NorthWest | TileFlags.NorthEast | TileFlags.SouthWest;

        const string CorridorEndTile = "Tile_17";
        const TileFlags CorridorEndFlags =
            TileFlags.NorthWest | TileFlags.NorthEast | TileFlags.SouthEast | TileFlags.SouthWest | TileFlags.West |
            TileFlags.East | TileFlags.South;

        private const string CornerOuterTile = EmptyTile; // TODO: Replace!
        const TileFlags CornerOuterFlags = TileFlags.SouthWest;

        const string CornerCorridorTile = "Tile_05";
        //const TileFlags CornerCorridorFlags = TileFlags.NorthWest | TileFlags.South | TileFlags.SouthEast;
        const TileFlags CornerCorridorFlags = TileFlags.NorthWest | TileFlags.South;

        const string CornerCorridor2Tile = "Tile_06";
        //const TileFlags CornerCorridor2Flags = TileFlags.SouthWest | TileFlags.North | TileFlags.NorthEast;
        private const TileFlags CornerCorridor2Flags = TileFlags.SouthWest | TileFlags.North;

        public static void Layout(Level level)
        {
            var tileCodes = ToTileCodes(level);

            var sizeX = level.SizeX;
            var sizeY = level.SizeY;

            WriteTileCodes(sizeX, sizeY, tileCodes);

            var tiles = new List<Tile>();
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    if (tileCodes[i, j] == TileFlags.Empty)
                    {
                        tiles.Add(new Tile { TileResource = EmptyTile, X = i, Z = j });
                    }
                    else if ((tileCodes[i, j] & TileFlags.Filled) != 0)
                    {
                        tiles.Add(new Tile { TileResource = Filled, X = i, Z = j });
                    }
                    else
                    {
                        Tile tile;

                        var corridor = TryMatch(CorridorTile, CorridorFlags, tileCodes, i, j, out tile);
                        if (corridor)
                            tiles.Add(tile);
                        else
                        {
                            var corner = TryMatch(CornerTile, CornerFlags, tileCodes, i, j, out tile);
                            if (corner)
                                tiles.Add(tile);
                            else
                            {
                                var corridorCorner = TryMatch(CornerCorridorTile, CornerCorridorFlags, tileCodes, i, j, out tile);
                                if (corridorCorner)
                                    tiles.Add(tile);
                                else
                                {
                                    var corridorCorner2 = TryMatch(CornerCorridor2Tile, CornerCorridor2Flags, tileCodes, i, j, out tile);
                                    if (corridorCorner2)
                                        tiles.Add(tile);
                                    else
                                    {
                                        // TODO: junction
                                        // TODO: cross road
                                        var wall = TryMatch(WallTile, WallFlags, tileCodes, i, j, out tile);
                                        if (wall)
                                            tiles.Add(tile);
                                        else
                                        {
                                            var entrance = TryMatch(WallWithEntranceTile, WallWithEntranceFlags, tileCodes,
                                                i, j, out tile);
                                            if (entrance)
                                                tiles.Add(tile);
                                            else
                                            {
                                                var outerCorner = TryMatch(CornerOuterTile, CornerOuterFlags, tileCodes, i,
                                                    j, out tile);
                                                if (outerCorner)
                                                    tiles.Add(tile);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var tile in tiles)
            {
                Console.WriteLine("        new Tile {{ Name=\"{0}\", Position=new Point({1}, {2}), Orientation={3}, Mirrored={4} }},", tile.TileResource, tile.X, tile.Z, tile.Orientation, tile.Mirrored ? "true" : "false");
            }
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

        private static bool TryMatch(string tileName, TileFlags flags, TileFlags[,] tileCodes, int i, int j, out Tile tile)
        {
            tile = null;

            if ((tileCodes[i, j] & TileFlags.Filled) != 0)
                return false;

            for (int r = 0; r < 4; ++r)
            {
                bool match = (flags & tileCodes[i, j]) == flags;

                if (match)
                {
                    tile = new Tile { TileResource = tileName, X = i, Z = j, Orientation = r * 90 };
                    return true;
                }

                flags = RotateClockwise(flags);
            }

            // TODO: Mirror?

            return false;
        }

        public static TileFlags Mirror(TileFlags flags)
        {
            TileFlags result = flags & (TileFlags.North | TileFlags.East | TileFlags.Filled);
            result |= (flags & TileFlags.East) != 0 ? TileFlags.West : 0;
            result |= (flags & TileFlags.West) != 0 ? TileFlags.East : 0;
            result |= (flags & TileFlags.NorthEast) != 0 ? TileFlags.NorthWest : 0;
            result |= (flags & TileFlags.NorthWest) != 0 ? TileFlags.NorthEast : 0;
            result |= (flags & TileFlags.SouthEast) != 0 ? TileFlags.SouthWest : 0;
            result |= (flags & TileFlags.SouthWest) != 0 ? TileFlags.SouthEast : 0;
            return result;


        }

        public static TileFlags RotateClockwise(TileFlags flags)
        {
            var value = (byte)flags;
            var rotated = (byte)((value << 2) | (value >> (8 - 2)));
            return flags & TileFlags.Filled | (TileFlags)rotated;
        }

        private class Tile
        {
            public bool Mirrored { get; set; }
            public int X { get; set; }
            public int Z { get; set; }
            public string TileResource { get; set; }
            public int Orientation { get; set; }
        }

        [Flags]
        public enum TileFlags
        {
            Empty = 0,

            NorthWest = 1,
            North = 1 << 1,
            NorthEast = 1 << 2,
            East = 1 << 3,
            SouthEast = 1 << 4,
            South = 1 << 5,
            SouthWest = 1 << 6,
            West = 1 << 7,

            Filled = 1 << 8
        }

        private static TileFlags[,] ToTileCodes(Level level)
        {
            var sizeX = level.SizeX;
            var sizeY = level.SizeY;

            var tiles = new TileFlags[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    TileFlags tile = 0;
                    tile |= GetValue(i - 1, j + 1, level) ? TileFlags.NorthWest : 0;
                    tile |= GetValue(i, j + 1, level) ? TileFlags.North : 0;
                    tile |= GetValue(i + 1, j + 1, level) ? TileFlags.NorthEast : 0;
                    tile |= GetValue(i + 1, j, level) ? TileFlags.East : 0;
                    tile |= GetValue(i + 1, j - 1, level) ? TileFlags.SouthEast : 0;
                    tile |= GetValue(i, j - 1, level) ? TileFlags.South : 0;
                    tile |= GetValue(i - 1, j - 1, level) ? TileFlags.SouthWest : 0;
                    tile |= GetValue(i - 1, j, level) ? TileFlags.West : 0;
                    tile |= GetValue(i, j, level) ? TileFlags.Filled : 0;
                    tiles[i, j] = tile;
                }
            }
            return tiles;
        }

        private static bool GetValue(int x, int y, Level level)
        {
            if (x < level.SizeX && y < level.SizeY && x > 0 && y > 0)
                return level.Grid[x, y] == Level.FilledChar;
            return true;
        }
    }
}