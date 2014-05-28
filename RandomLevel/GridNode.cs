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

		public int X { get { return _x; } }

		public int Y { get { return _y; } }

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

	    protected bool Equals(GridNode other)
	    {
	        return _x == other._x && _y == other._y && _sizeX == other._sizeX && _sizeY == other._sizeY;
	    }

	    public override bool Equals(object obj)
	    {
	        if (ReferenceEquals(null, obj)) return false;
	        if (ReferenceEquals(this, obj)) return true;
	        if (obj.GetType() != this.GetType()) return false;
	        return Equals((GridNode) obj);
	    }

	    public override int GetHashCode()
	    {
	        unchecked
	        {
	            var hashCode = _x;
	            hashCode = (hashCode*397) ^ _y;
	            hashCode = (hashCode*397) ^ _sizeX;
	            hashCode = (hashCode*397) ^ _sizeY;
	            return hashCode;
	        }
	    }

	    public static bool operator ==(GridNode left, GridNode right)
	    {
	        return Equals(left, right);
	    }

	    public static bool operator !=(GridNode left, GridNode right)
	    {
	        return !Equals(left, right);
	    }
	}
}

