using System.Collections.Generic;

namespace RandomLevel
{
	interface IHasNeighbours<out TNode>  
	{
		IEnumerable<TNode> Neighbours { get; }
	}
	
}
