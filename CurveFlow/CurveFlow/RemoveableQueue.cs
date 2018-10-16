using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveFlow
{
	internal class RemoveableQueue<T>
	{
		public int Count { get { return list.Count; } }
		LinkedList<T> list = new LinkedList<T>();
		public void Enqueue(T t)
		{
			list.AddLast(t);
		}
		public T Dequeue()
		{
			T result = list.First.Value;
			list.RemoveFirst();
			return result;
		}
		public bool Remove(T t)
		{
			return list.Remove(t);
		}
		public int GetPosition(T t)
		{
			return -1;
		}
	}
}
