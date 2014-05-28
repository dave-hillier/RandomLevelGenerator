using System;
using System.Collections.Generic;
using System.Collections;

namespace RandomLevel
{
	interface IHasNeighbours<N>  
	{
		IEnumerable<N> Neighbours { get; }
	}
	
}
