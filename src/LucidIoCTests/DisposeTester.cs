using System;

namespace gar3t.LucidIoC.Tests
{
	public class DisposeTester : IDisposable
	{
		public bool Disposed { get; private set; }

		public void Dispose()
		{
			Disposed = true;
		}
	}
}