using System;
using System.Collections.Generic;
using System.Text;

namespace RandomLevel
{

    internal interface IGrid
    {
        int SizeX { get; }
        int SizeY { get; }
        char[,] Grid { get; }
    }

    class Level : IGrid
    {
        private readonly Random _rng; // TODO: use a seed
        public int Size = 21;
        private readonly char[,] _grid;
        private readonly CorridorPathFinder _pathFinder;
        public const char FilledChar = 'X';

        public Level(int seed, int size)
        {
            Seed = seed;
            Size = size;
            _grid = new char[size, size];
            ClearGrid();
            _pathFinder = new CorridorPathFinder(_grid);
            _rng = new Random(seed);
        }

        private void ClearGrid()
        {
            for (var i = 0; i < SizeX; ++i)
            {
                for (var j = 0; j < SizeY; ++j)
                {
                    _grid[j, i] = FilledChar;
                }
            }
        }

        public int Seed { get; set; }

        public int SizeX
        {
            get { return _grid.GetUpperBound(0) + 1; }
        }

        public int SizeY
        {
            get { return _grid.GetUpperBound(1) + 1; }
        }

        public char[,] Grid { get { return _grid; } }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < SizeX; ++i)
            {
                for (var j = 0; j < SizeY; ++j)
                {
                    sb.Append(_grid[j, i]);
                    if (j == SizeY - 1)
                        sb.AppendLine();
                }
            }
            return sb.ToString();
        }


        public void CreateCorridors(GridNode start, GridNode end)
        {
            var path = _pathFinder.GetCorridorPath(start, end);

            foreach (var node in path)
            {
                _grid[node.X, node.Y] = ' ';
            }
        }

        public void ReflectVertically()
        {
            int half = SizeY / 2;
            for (var i = 0; i < SizeX; ++i)
            {
                for (var j = 0; j < half; ++j)
                {
                    _grid[i, SizeX - 1 - j] = _grid[i, j];
                }
            }
        }

        public void ReflectAndMirrorVertically()
        {
            int half = SizeY / 2;
            for (var i = 0; i < SizeX; ++i)
            {
                for (var j = 0; j < half; ++j)
                {
                    _grid[SizeX - 1 - i, SizeY - 1 - j] = _grid[i, j];
                }
            }
        }

        public void FillRoom(Point point, int tl, int tr)
        {
            for (var i = tl; i < tl + point.X; ++i)
            {
                for (var j = tr; j < tr + point.Y; ++j)
                {
                    _grid[i, j] = ' ';
                }
            }
        }

        public Tuple<int, int> PlaceRoom(int horizontalSize, int verticalSize)
        {
            var rect = new Point
            {
                X = 5 + _rng.Next() % (horizontalSize / 4),
                Y = 5 + _rng.Next() % (verticalSize / 4)
            };
            if (3 * rect.X > rect.Y)
                rect.X /= 2;
            if (3 * rect.Y > rect.X)
                rect.Y /= 2;
            if (rect.X < 3)
                rect.X = 3;
            if (rect.Y < 3)
                rect.Y = 3;

            int cornerX;
            int cornerY;
            var c = 250;

            do
            {
                cornerX = 1 + _rng.Next() % (horizontalSize - 1 - rect.X);
                cornerY = 1 + _rng.Next() % (verticalSize - 1 - rect.Y);
            } while (!IsEmpty(rect, cornerX, cornerY) && --c != 0);

            FillRoom(rect, cornerX, cornerY);
            return Tuple.Create(cornerX + (rect.X / 2), cornerY + (rect.Y / 2));
        }

        public bool IsEmpty(Point point, int tl, int tr)
        {
            const int margin = 1;
            for (var i = Math.Max(tl - margin, 0); i < Math.Min(tl + point.X + margin, SizeX); ++i)
            {
                for (var j = Math.Max(tr - margin, 0); j < Math.Min(tr + point.Y + margin, SizeY); ++j)
                {
                    if (_grid[i, j] != FilledChar)
                        return false;
                }
            }
            return true;
        }

        public struct Point
        {
            public int X;
            public int Y;
        }
    }
}