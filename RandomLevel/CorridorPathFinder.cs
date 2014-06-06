using System;
using System.Collections.Generic;

namespace RandomLevel
{
    class CorridorPathFinder
    {
        private readonly char[,] _grid;

        public CorridorPathFinder(char[,] grid)
        {
            _grid = grid;
        }

        public IEnumerable<GridNode> GetCorridorPath(GridNode start, GridNode end)
        {
            return AStarPathFinder.FindPath(start, end, GetDistance, EstimateCost);
        }

        public static double GetDistance(GridNode n1, GridNode n2)
        {
            return Math.Abs(n2.X - n1.X) + Math.Abs(n2.Y - n1.Y);
        }

        public double EstimateCost2(GridNode start, GridNode destination)
        {
            var x = destination.X - start.X;
            var y = destination.Y - start.Y;

            var distance = Math.Sqrt(x*x + y*y);
            return 0.5*distance*EstimateCost(start, destination);
        }

        public double EstimateCost(GridNode start, GridNode destination)
        {
            int sizeX = _grid.GetUpperBound(0) + 1;
            int sizeY = _grid.GetUpperBound(1) + 1;

            // TODO: Padding parameter
            // Avoid edges
            if (start.X == 0)
                return 200;
            if (start.Y == 0)
                return 200;
            if (start.X == sizeX - 1)
                return 200;
            if (start.Y == sizeY - 1)
                return 200;

            // Prefer spaces
            if (_grid[start.X, start.Y] == Level.FilledChar) 
                return 150;

            if (_grid[start.X + 1, start.Y] == Level.FilledChar)
                return 100;
            if (_grid[start.X - 1, start.Y] == Level.FilledChar)
                return 100;
            if (_grid[start.X, start.Y + 1] == Level.FilledChar)
                return 100;
            if (_grid[start.X, start.Y - 1] == Level.FilledChar)
                return 100;

            return 20;
        }
    }
}