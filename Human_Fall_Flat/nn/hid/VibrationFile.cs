using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class VibrationFile
	{
		public static readonly ErrorRange ResultInvalid = new ErrorRange(202, 140, 150);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_ParseVibrationFile")]
		public static extern Result Parse(ref VibrationFileInfo pOutInfo, ref VibrationFileParserContext pOutContext, byte[] address, int fileSize);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_RetrieveVibrationValue")]
		public static extern void RetrieveValue(ref VibrationValue pOutValue, int position, ref VibrationFileParserContext pContext);
	}
}
