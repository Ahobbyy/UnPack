using System;

namespace nn.account
{
	public sealed class AsyncContext : IDisposable
	{
		internal IntPtr _context = IntPtr.Zero;

		~AsyncContext()
		{
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
		}

		public Result Cancel()
		{
			return default(Result);
		}

		public Result HasDone(ref bool pOut)
		{
			pOut = true;
			return default(Result);
		}

		public Result GetResult()
		{
			return default(Result);
		}
	}
}
