using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Data
{
	public class PriorityQueue <T> where T : IComparable<T>
	{
		List<T> data;

		public PriorityQueue()
		{
			data = new List<T>();
		}

		public int Count {
			get
			{
				return data.Count;
			}
		}

		public void Enqueue(T item)
		{
			data.Add(item);

			var ci = data.Count - 1;
			while (ci > 0)
			{
				int pi = (ci - 1) / 2; //parent
				if (data[ci].CompareTo(data[pi]) >= 0)
				{
					break;
				}
				//swap
				T tmp = data[ci];
				data[ci] = data[pi];
				data[pi] = tmp;

				ci = pi;
			}
		}
		public T Dequeue()
		{
			int li = data.Count - 1;
			if (li < 0)
			{
				return default(T);
			}
			T frontItem = data[0];
			data[0] = data[li];
			data.RemoveAt(li);

			li--;
			int pi = 0;
			while (true)
			{
				int ci = pi * 2 + 1;
				if (ci > li)
				{
					break;
				}
				int rc = ci + 1;
				if (rc <= li && data[rc].CompareTo(data[ci]) < 0)
				{
					ci = rc;
				}
				if (data[pi].CompareTo(data[ci]) <= 0)
				{
					break;
				}
				T tmp = data[pi];
				data[pi] = data[ci];
				data[ci] = tmp;
				pi = ci;
			}
			return frontItem;
		}

        public bool IsConsistent()
        {
            // is the heap property true for all data?
            if (data.Count == 0)
                return true;
            int li = data.Count - 1; // last index
            for (int pi = 0; pi < data.Count; ++pi)
            { // each parent index
                int lci = 2 * pi + 1; // left child index
                int rci = 2 * pi + 2; // right child index

                if (lci <= li && data[pi].CompareTo(data[lci]) > 0)
                    return false; // if lc exists and it's greater than parent then bad.
                if (rci <= li && data[pi].CompareTo(data[rci]) > 0)
                    return false; // check the right child too.
            }
            return true; // passed all checks
        }
    }
}
