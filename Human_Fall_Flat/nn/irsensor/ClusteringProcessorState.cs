using System.Runtime.InteropServices;
using System.Text;

namespace nn.irsensor
{
	public struct ClusteringProcessorState
	{
		public long samplingNumber;

		public long timeStampNanoSeconds;

		public sbyte objectCount;

		public byte _reserved0;

		public byte _reserved1;

		public byte _reserved2;

		public IrCameraAmbientNoiseLevel ambientNoiseLevel;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public ClusteringData[] objects;

		public void SetDefault()
		{
			objects = new ClusteringData[16];
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("({0} {1} {2} {3})\n", samplingNumber, timeStampNanoSeconds, objectCount, ambientNoiseLevel.ToString());
			for (int i = 0; i < objectCount; i++)
			{
				stringBuilder.AppendFormat("object[{0}]:{1}\n", i, objects[i].ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
