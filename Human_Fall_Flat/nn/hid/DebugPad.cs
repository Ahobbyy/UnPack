using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class DebugPad
	{
		public const int StateCountMax = 16;

		public static void Initialize()
		{
		}

		public static void GetState(ref DebugPadState pOutValue)
		{
		}

		public static int GetStates([Out] DebugPadState[] pOutValues, int count)
		{
			return 0;
		}
	}
}
