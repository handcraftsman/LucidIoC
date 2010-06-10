using System;

namespace gar3t.LucidIoC.Tests
{
	public class NonDisposableTester : IComparable
	{
		public int CompareTo(object obj)
		{
			throw new NotImplementedException();
		}
	}
}