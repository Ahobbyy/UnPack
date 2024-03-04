using System;

namespace ReliableNetcode
{
	internal class ReliableConfig
	{
		public string Name;

		public int MaxPacketSize;

		public int FragmentThreshold;

		public int MaxFragments;

		public int FragmentSize;

		public int SentPacketBufferSize;

		public int ReceivedPacketBufferSize;

		public int FragmentReassemblyBufferSize;

		public float RTTSmoothFactor;

		public float PacketLossSmoothingFactor;

		public float BandwidthSmoothingFactor;

		public int PacketHeaderSize;

		public Action<byte[], int> TransmitPacketCallback;

		public Action<ushort, byte[], int> ProcessPacketCallback;

		public Action<ushort> AckPacketCallback;

		public static ReliableConfig DefaultConfig()
		{
			return new ReliableConfig
			{
				Name = "endpoint",
				MaxPacketSize = 65536,
				FragmentThreshold = 1024,
				MaxFragments = 16,
				FragmentSize = 1024,
				SentPacketBufferSize = 256,
				ReceivedPacketBufferSize = 256,
				FragmentReassemblyBufferSize = 64,
				RTTSmoothFactor = 0.25f,
				PacketLossSmoothingFactor = 0.1f,
				BandwidthSmoothingFactor = 0.1f,
				PacketHeaderSize = 28
			};
		}
	}
}
