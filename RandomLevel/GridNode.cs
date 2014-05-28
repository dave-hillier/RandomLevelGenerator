using System;
using System.Collections.Generic;

namespace RandomLevel
{
	class GridNode : IHasNeighbours<GridNode>
	{
		readonly int _x;
		readonly int _y;
		readonly int _sizeX;
		readonly int _sizeY;

		public GridNode(int x, int y, int sizeX, int sizeY)
		{
			_x = x;
			_y = y;
			_sizeX = sizeX;
			_sizeY = sizeY; 
		}

		public int x { get { return _x; } }

		public int y { get { return _y; } }

		public IEnumerable<GridNode> Neighbours {
			get {
				if (_x > 0) {
					yield return new GridNode (_x - 1, _y, _sizeX, _sizeY);
				}
				if (_y > 0) {
					yield return new GridNode (_x, _y - 1, _sizeX, _sizeY);
				}				
				if (_x < _sizeX - 1) {
					yield return new GridNode (_x  + 1, _y, _sizeX, _sizeY);
				}
				if (_y < _sizeY - 1) {
					yield return new GridNode (_x, _y + 1, _sizeX, _sizeY);
				}
			}
		}

		public override bool Equals(object obj)
		{
			var item = obj as GridNode;
			if (item == null)
				return false;

			return item._x == _x &&
				item._y == _y &&
				item._sizeX == _sizeX &&
				item._sizeY == _sizeY;
		}

		public override int GetHashCode()
		{
			return _x * 13 +  y * 317 + _sizeX * 619 + _sizeY * 238;
		}
	}
}

