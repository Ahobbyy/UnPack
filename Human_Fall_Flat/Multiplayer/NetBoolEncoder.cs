namespace Multiplayer
{
	public static class NetBoolEncoder
	{
		public static int CalculateMaxDeltaSizeInBits()
		{
			return 1;
		}

		public static void CollectState(NetStream stream, bool value)
		{
			stream.Write(value);
		}

		public static bool ApplyState(NetStream state)
		{
			return state.ReadBool();
		}

		public static bool ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			state0.ReadBool();
			return state1.ReadBool();
		}

		public static bool CalculateDelta(NetStream state0, NetStream state1, NetStream delta, bool writeChanged = true)
		{
			bool num = state0?.ReadBool() ?? false;
			bool flag = state1.ReadBool();
			bool flag2 = num != flag;
			delta.Write(flag2);
			return flag2;
		}

		public static void AddDelta(NetStream state0, NetStream delta, NetStream result, bool readChanged = true)
		{
			bool flag = state0?.ReadBool() ?? false;
			bool v = delta.ReadBool() ^ flag;
			result.Write(v);
		}
	}
}
