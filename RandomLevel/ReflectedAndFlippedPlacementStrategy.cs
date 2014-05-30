using System;
using System.Collections.Generic;

namespace RandomLevel
{
    class ReflectedAndFlippedPlacementStrategy
    {
        private readonly int _numRooms;
        private readonly char[,] _grid;
        private readonly int _sizeX;
        private readonly int _sizeY;
        private readonly Level _level;
        private readonly Random _rng;

        public ReflectedAndFlippedPlacementStrategy(Level level, int numberOfRooms)
        {
            _numRooms = numberOfRooms;
            _rng = new Random(level.Seed);
            _grid = level.Grid;
            _level = level;
            _sizeX = _grid.GetUpperBound(0);
            _sizeY = _grid.GetUpperBound(1);
        }

        public void PlaceLevel() 
        {
            const int startRoom = 3;
            _level.FillRoom(new Level.Point { X = startRoom, Y = startRoom }, 1, 1);
            _level.FillRoom(new Level.Point { X = startRoom, Y = startRoom }, _sizeX - 1 - startRoom, _sizeY - 1 - startRoom);

            const int centerRoom = 6;
            _level.FillRoom(new Level.Point { X = centerRoom, Y = centerRoom }, (_sizeX / 2) - (centerRoom / 2), (_sizeY / 2) - (centerRoom / 2));

            var roomCenter = new List<GridNode>();

            var captureRoom = _level.PlaceRoom(_sizeX, _sizeY / 2);
            roomCenter.Add(new GridNode(captureRoom.Item1, captureRoom.Item2, _sizeX, _sizeY));


            for (var i = 0; i < _numRooms - 2; ++i)
            {
                var r = _level.PlaceRoom(_sizeX, _sizeY / 2);
                roomCenter.Add(new GridNode(r.Item1, r.Item2, _sizeX, _sizeY));
            }

            for (var j = 2; j < _sizeY - 2; ++j)
            {
                _grid[2, j] = ' ';
                _grid[j, 2] = ' ';
                _grid[_sizeX - 2, j] = ' ';
                _grid[j, _sizeY - 2] = ' ';
            }

            var r1 = new GridNode(3, 3, _sizeX, _sizeY);
            var corner1 = new GridNode(_sizeX - 3, 3, _sizeX, _sizeY);
            var corner2 = new GridNode(3, _sizeY - 3, _sizeX, _sizeY);

            var r2 = new GridNode(_sizeX - 3, _sizeY - 3, _sizeX, _sizeY);


            foreach (var node in roomCenter)
            {
                _level.CreateCorridors(r1, node);
                _level.CreateCorridors(node, r2);
            }

            var centre = new GridNode(_sizeX / 2, _sizeY / 2, _sizeX, _sizeY);
            _level.CreateCorridors(r1, centre);
            _level.CreateCorridors(corner1, r1);
            _level.CreateCorridors(corner1, centre);
            _level.CreateCorridors(corner2, r1);
            _level.CreateCorridors(corner2, centre);
            _level.CreateCorridors(centre, r2);

            _level.ReflectAndMirrorVertically();

            _level.CreateCorridors(r1, r2);
  

            PlacePointsOfInterest(captureRoom);
        }

        private void PlacePointsOfInterest(Tuple<int, int> captureRoom)
        {
            var rOrB = _rng.Next() % 2 == 0;
            _grid[2, 2] = rOrB ? 'R' : 'B';
            _grid[_sizeX - 2, _sizeY - 2] = rOrB ? 'B' : 'R';
            _grid[_sizeX/2, _sizeY/2] = 'C';
            _grid[captureRoom.Item1, captureRoom.Item2] = 'C';
            _grid[_sizeX - captureRoom.Item1, _sizeY - captureRoom.Item2] = 'C';
        }
    }
}