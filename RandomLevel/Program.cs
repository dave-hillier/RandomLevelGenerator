using System;
using System.Collections.Generic;

namespace RandomLevel
{
    internal class MainClass
    {
        private const int NumRooms = 10;
        private const int Size = 26;
        private const char FilledChar = 'X';
        private static readonly char[,] Grid = new char[Size,Size];

        private static readonly Random Rng = new Random();

        public static void Main(string[] args)
        {
            InitGrid();
            Print();
            Console.ReadKey();
        }

        private static void Print()
        {
            foreach (var coord in GridCoordinates())
            {
                var i = coord.Item1;
                var j = coord.Item2;
                Console.Write(Grid[j, i]);
                if (j == Size - 1)
                    Console.WriteLine();
            }
        }

        private static void InitGrid()
        {
            foreach (var coord in GridCoordinates())
            {
                var i = coord.Item1;
                var j = coord.Item2;
                Grid[j, i] = FilledChar;
            }

            const int startRoom = 3;
            FillRoom(new Rect {X = startRoom, Y = startRoom}, 1, 1);
            FillRoom(new Rect {X = startRoom, Y = startRoom}, Size - 1 - startRoom, Size - 1 - startRoom);


            const int centerRoom = 6;
            FillRoom(new Rect {X = centerRoom, Y = centerRoom}, (Size/2) - (centerRoom/2), (Size/2) - (centerRoom/2));

            PlaceRooms();
            //PlaceRooms2 ();
        }

        private static void CreateCorridors(GridNode start, GridNode end)
        {
            var path = AStarPathFinder.FindPath(start, end, GetDistance, EstimateCost);

            foreach (var node in path)
            {
                Grid[node.X, node.Y] = ' ';
            }
        }

        private static double GetDistance(GridNode n1, GridNode n2)
        {
            // TODO: Euclidian? Manhattan?
            return Math.Abs(n2.X - n1.X) + Math.Abs(n2.Y - n1.Y);
        }

        private static double EstimateCost2(GridNode start, GridNode destination)
        {
            var x = destination.X - start.X;
            var y = destination.Y - start.Y;

            var distance = Math.Sqrt(x*x + y*y);
            return 0.5*distance*EstimateCost(start, destination);
        }

        private static double EstimateCost(GridNode start, GridNode destination)
        {
            // TODO: Padding parameter
            // Avoid edges
            if (start.X == 0)
                return 200;
            if (start.Y == 0)
                return 200;
            if (start.X == Size - 1)
                return 200;
            if (start.Y == Size - 1)
                return 200;

            // Prefer spaces
            if (Grid[start.X, start.Y] == FilledChar)
                return 150;

            if (Grid[start.X + 1, start.Y] == FilledChar)
                return 100;
            if (Grid[start.X - 1, start.Y] == FilledChar)
                return 100;
            if (Grid[start.X, start.Y + 1] == FilledChar)
                return 100;
            if (Grid[start.X, start.Y - 1] == FilledChar)
                return 100;

            return 20;
        }

        private static void PlaceRooms()
        {
            // TODO: Place a capture point

            var roomCenter = new List<GridNode>();

            var captureRoom = PlaceRoom(Size, Size/2);
            roomCenter.Add(new GridNode(captureRoom.Item1, captureRoom.Item2, Size, Size));


            for (var i = 0; i < NumRooms - 2; ++i)
            {
                var r = PlaceRoom(Size, Size/2);
                roomCenter.Add(new GridNode(r.Item1, r.Item2, Size, Size));
            }

            var r1 = new GridNode(3, 3, Size, Size);
            var corner1 = new GridNode(Size - 3, 3, Size, Size);
            var corner2 = new GridNode(3, Size - 3, Size, Size);

            var r2 = new GridNode(Size - 3, Size - 3, Size, Size);


            foreach (var node in roomCenter)
            {
                //Console.WriteLine("Placing corridors to ({0}, {1})", node.X, node.Y);

                CreateCorridors(r1, node);
                CreateCorridors(node, r2);
            }

            var centre = new GridNode(Size/2, Size/2, Size, Size);
            CreateCorridors(r1, centre);
            CreateCorridors(corner1, r1);
            CreateCorridors(corner1, centre);
            CreateCorridors(corner2, r1);
            CreateCorridors(corner2, centre);
            CreateCorridors(centre, r2);

            ReflectAndMirrorVertically();

            CreateCorridors(r1, r2);

            var rOrB = Rng.Next()%2 == 0;
            Grid[2, 2] = rOrB ? 'R' : 'B';
            Grid[Size - 3, Size - 3] = rOrB ? 'B' : 'R';
            Grid[Size/2, Size/2] = 'C';
            Grid[captureRoom.Item1, captureRoom.Item2] = 'C';
            Grid[Size - 1 - captureRoom.Item1, Size - 1 - captureRoom.Item2] = 'C';
        }

        private static void ReflectVertically()
        {
            const int half = Size/2;
            for (var i = 0; i < Size; ++i)
            {
                for (var j = 0; j < half; ++j)
                {
                    Grid[i, Size - 1 - j] = Grid[i, j];
                }
            }
        }

        private static void ReflectAndMirrorVertically()
        {
            const int half = Size/2;
            for (var i = 0; i < Size; ++i)
            {
                for (var j = 0; j < half; ++j)
                {
                    Grid[Size - 1 - i, Size - 1 - j] = Grid[i, j];
                }
            }
        }

        private static void FillRoom(Rect rect, int tl, int tr)
        {
            for (var i = tl; i < tl + rect.X; ++i)
            {
                for (var j = tr; j < tr + rect.Y; ++j)
                {
                    Grid[i, j] = ' ';
                }
            }
        }

        private static Tuple<int, int> PlaceRoom(int horizontalSize, int verticalSize)
        {
            var rect = new Rect
                {
                    X = 5 + Rng.Next()%(horizontalSize/4),
                    Y = 5 + Rng.Next()%(verticalSize/4)
                };
            if (3*rect.X > rect.Y)
                rect.X /= 2;
            if (3*rect.Y > rect.X)
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
                cornerX = 1 + Rng.Next()%(horizontalSize - 1 - rect.X);
                cornerY = 1 + Rng.Next()%(verticalSize - 1 - rect.Y);
            } while (!IsEmpty(rect, cornerX, cornerY) && --c != 0);
            //Console.WriteLine("{0}", c == 0 ? "Limit hit" : "Placed OK");

            //if (c != 0)
            FillRoom(rect, cornerX, cornerY);
            return Tuple.Create(cornerX + (rect.X/2), cornerY + (rect.Y/2));
        }

        private static bool IsEmpty(Rect rect, int tl, int tr)
        {
            const int margin = 1;
            for (var i = Math.Max(tl - margin, 0); i < Math.Min(tl + rect.X + margin, Size); ++i)
            {
                for (var j = Math.Max(tr - margin, 0); j < Math.Min(tr + rect.Y + margin, Size); ++j)
                {
                    if (Grid[i, j] != FilledChar)
                        return false;
                }
            }
            return true;
        }

        private static IEnumerable<Tuple<int, int>> GridCoordinates()
        {
            for (var i = 0; i < Size; ++i)
            {
                for (var j = 0; j < Size; ++j)
                {
                    yield return Tuple.Create(i, j);
                }
            }
        }

        private struct Rect
        {
            public int X;
            public int Y;
        }
    }
}