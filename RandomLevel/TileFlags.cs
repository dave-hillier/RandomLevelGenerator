using System;

namespace RandomLevel
{
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

        Center = 1 << 8
    }
}