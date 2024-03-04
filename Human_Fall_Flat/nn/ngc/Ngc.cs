using System.Runtime.InteropServices;

namespace nn.ngc
{
	public static class Ngc
	{
		public static readonly ErrorRange ResultNotInitialized = new ErrorRange(146, 1, 2);

		public static readonly ErrorRange ResultAlreadyInitialized = new ErrorRange(146, 2, 3);

		public static readonly ErrorRange ResultInvalidPointer = new ErrorRange(146, 3, 4);

		public static readonly ErrorRange ResultInvalidSize = new ErrorRange(146, 4, 5);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_ngc_CountNumbers")]
		public static extern int CountNumbers(string str);
	}
}
