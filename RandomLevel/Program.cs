using System;
using System.Collections.Generic;

namespace RandomLevel
{
	class MainClass
	{
		const int NumRooms = 10;
		const int size = 26;
		static char[,] grid = new char[size, size];
		struct Rect { public int x; public int y; }
		static Random rng = new Random ();
		const char FilledChar = 'X';

		public static void Main (string[] args)
		{
			InitGrid ();
			Print ();
		}

		static void Print ()
		{
			foreach (var coord in GridCoordinates()) {
				int i = coord.Item1;
				int j = coord.Item2;
				Console.Write (grid [j, i]);
				if (j == size - 1)
					Console.WriteLine ();
			}
		}

		static void InitGrid ()
		{
			foreach (var coord in GridCoordinates()) {
				int i = coord.Item1;
				int j = coord.Item2;
				grid [j, i] = FilledChar;
			}

			const int startRoom = 3;
			FillRoom (new Rect { x = startRoom, y = startRoom }, 1, 1);
			FillRoom (new Rect { x = startRoom, y = startRoom }, size - 1 - startRoom, size - 1 - startRoom);


			const int centerRoom = 6;
			FillRoom (new Rect { x = centerRoom, y = centerRoom }, (size/2) - (centerRoom/2), (size/2) - (centerRoom/2));

			PlaceRooms ();
			//PlaceRooms2 ();
		}

		static void CreateCorridors (int sx, int sy, int ex, int ey)
		{
			CreateCorridors (new GridNode (sx, sy, size, size),
				new GridNode (ex, ey, size, size));
		}
		 
		static void CreateCorridors (GridNode start, GridNode end)
		{
			var path = AStarPathFinder.FindPath (start, end, GetDistance, EstimateCost);

			foreach (var node in path) {
				grid [node.x, node.y] = ' ';
			}
		}

		static double GetDistance (GridNode n1, GridNode n2)
		{
			// TODO: Euclidian? Manhattan?
			return Math.Abs(n2.x - n1.x) + Math.Abs(n2.y - n1.y);
		}

		static double EstimateCost2 (GridNode start, GridNode destination)
		{
			int x = destination.x - start.x;
			int y = destination.y - start.y;

			var distance = Math.Sqrt(x*x + y*y);
			return 0.5 * distance * EstimateCost (start, destination);
		}
	
		static double EstimateCost (GridNode start, GridNode destination)
		{
			// TODO: Padding parameter
			// Avoid edges
			if (start.x == 0)
				return 200;
			if (start.y == 0)
				return 200;
			if (start.x == size-1)
				return 200;
			if (start.y == size-1)
				return 200;

			// Prefer spaces
			if (grid [start.x, start.y] == FilledChar)
				return 150;

			if (grid [start.x+1, start.y] == FilledChar)
				return 100;
			if (grid [start.x-1, start.y] == FilledChar)
				return 100;
			if (grid [start.x, start.y+1] == FilledChar)
				return 100;
			if (grid [start.x, start.y-1] == FilledChar)
				return 100;

			return 20;
		}

		static void PlaceRooms ()
		{
			// TODO: Place a capture point

			var roomCenter = new List<GridNode> ();

			var captureRoom = PlaceRoom (size, size/2);
			roomCenter.Add(new GridNode(captureRoom.Item1, captureRoom.Item2, size, size));


			for (int i = 0; i < NumRooms-2; ++i) {
				var r = PlaceRoom (size, size/2);
				roomCenter.Add(new GridNode(r.Item1, r.Item2, size, size));
			}

			var r1 = new GridNode (3, 3, size, size);
			var corner1 = new GridNode (size-3, 3, size, size);
			var corner2 = new GridNode (3, size-3, size, size);

			var r2 = new GridNode (size-3, size-3, size, size);


			foreach (var node in roomCenter) {
				Console.WriteLine ("Placing corridors to ({0}, {1})", node.x, node.y);

				CreateCorridors (r1, node);
				CreateCorridors (node, r2);
			}

			var centre = new GridNode(size/2, size/2, size, size);
			CreateCorridors (r1, centre);
			CreateCorridors (corner1, r1);
			CreateCorridors (corner1, centre);
			CreateCorridors (corner2, r1);
			CreateCorridors (corner2, centre);
			CreateCorridors (centre, r2);

			ReflectAndMirrorVertically ();

			CreateCorridors (r1, r2);

			var rOrB = rng.Next() % 2 == 0;
			grid [2, 2] = rOrB ? 'R' : 'B';
			grid [size-3, size-3] = rOrB ? 'B' : 'R';
			grid [size/2, size/2] = 'C';
			grid [captureRoom.Item1, captureRoom.Item2] = 'C';
			grid [size-1-captureRoom.Item1, size-1-captureRoom.Item2] = 'C';
		}

		static void ReflectVertically ()
		{
			int half = size / 2;
			for (var i = 0; i < size; ++i) {
				for (var j = 0; j < half; ++j) {
					grid [i, size - 1 - j] = grid [i, j];
				}
			}
		}

		static void ReflectAndMirrorVertically ()
		{
			int half = size / 2;
			for (var i = 0; i < size; ++i) {
				for (var j = 0; j < half; ++j) {
					grid [size - 1 - i, size - 1 - j] = grid [i, j];
				}
			}
		}

		static bool IsEmpty(int i, int j)
		{
			return grid [i, j] != ' ';
		}

		static void FillRoom (Rect rect, int tl, int tr)
		{
			for (var i = tl; i < tl + rect.x; ++i) {
				for (var j = tr; j < tr + rect.y; ++j) {
					grid [i, j] = ' ';
				}
			}
		}

		static Tuple<int,int> PlaceRoom (int horizontalSize, int verticalSize)
		{
			var rect = new Rect {
				x = 5 + rng.Next() % (horizontalSize / 4),
				y = 5 + rng.Next() % (verticalSize / 4)
			};
			if (3 * rect.x > rect.y)
				rect.x /= 2;
			if (3 * rect.y > rect.x)
				rect.y /= 2;
			if (rect.x < 3)
				rect.x = 3;
			if (rect.y < 3)
				rect.y = 3;

			int cornerX;
			int cornerY;
			int c = 250;
			
			do {
				cornerX = 1 + rng.Next () % (horizontalSize - 1 - rect.x);
				cornerY = 1 + rng.Next () % (verticalSize - 1 - rect.y);
			} while(!IsEmpty (rect, cornerX, cornerY) && --c != 0);
			Console.WriteLine("{0}", c == 0 ? "Limit hit" : "Placed OK");

			//if (c != 0)
			FillRoom (rect, cornerX, cornerY);
			return Tuple.Create (cornerX + (rect.x/2), cornerY + (rect.y/2));
		}

		static bool IsEmpty (Rect rect, int tl, int tr)
		{
			const int margin = 1;
			for (var i = Math.Max (tl - margin, 0); i < Math.Min(tl+rect.x+margin, size); ++i) {
				for (var j = Math.Max (tr - margin, 0); j < Math.Min(tr+rect.y+margin, size); ++j) {
					if (grid[i,j] != FilledChar)
						return false;
				}
			}
			return true;
		}
		static IEnumerable<Tuple<int, int>> GridCoordinates()
		{
			for (var i = 0; i < size; ++i) {
				for (var j = 0; j < size; ++j) {
					yield return Tuple.Create (i, j);
				}
			}
		}
	}
}
