using System.Linq;
using System.Collections.Generic;

namespace RandomLevel
{
	class PriorityQueue<TPriority, TValue>
	{
		private readonly SortedDictionary<TPriority, Queue<TValue>> _list = new SortedDictionary<TPriority, Queue<TValue>>();
		public void Enqueue(TPriority priority, TValue value)
		{
			Queue<TValue> q;
			if (!_list.TryGetValue(priority, out q))
			{
				q = new Queue<TValue>();
				_list.Add(priority, q);
			}
			q.Enqueue(value);
		}
		public TValue Dequeue()
		{
			// will throw if there isn’t any first element!
			var pair = _list.First();
			var v = pair.Value.Dequeue();
			if (pair.Value.Count == 0) // nothing left of the top priority.
				_list.Remove(pair.Key);
			return v;
		}
		public bool IsEmpty
		{
			get { return !_list.Any(); }
		}
	}
}

