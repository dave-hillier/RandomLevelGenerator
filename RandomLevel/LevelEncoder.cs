namespace RandomLevel
{
    internal static class LevelEncoder
    {
        public static TileFlags[,] ToTileCodes(this IGrid level)
        {
            var sizeX = level.SizeX;
            var sizeY = level.SizeY;

            var tiles = new TileFlags[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    var tile = GetTileFlags(level, i, j);
                    tiles[i, j] = tile;
                }
            }
            return tiles;
        }

        public static TileFlags GetTileFlags(IGrid level, int i, int j)
        {
            TileFlags tile = 0;
            tile |= IsFilled(i - 1,     j - 1,      level) ? TileFlags.NorthWest : 0;
            tile |= IsFilled(i,         j - 1,      level) ? TileFlags.North : 0;
            tile |= IsFilled(i + 1,     j - 1,      level) ? TileFlags.NorthEast : 0;
            tile |= IsFilled(i + 1,     j,          level) ? TileFlags.East : 0;
            tile |= IsFilled(i + 1,     j + 1,      level) ? TileFlags.SouthEast : 0;
            tile |= IsFilled(i,         j + 1,      level) ? TileFlags.South : 0;
            tile |= IsFilled(i - 1,     j + 1,      level) ? TileFlags.SouthWest : 0;
            tile |= IsFilled(i - 1,     j,          level) ? TileFlags.West : 0;
            tile |= IsFilled(i,         j,          level) ? TileFlags.Center : 0;
            return tile;
        }

        private static bool IsFilled(int x, int y, IGrid level)
        {
            if (x < level.SizeX && y < level.SizeY && x >= 0 && y >= 0)
                return level.Grid[y, x] == 'X';
            return true;
        }
    }
}