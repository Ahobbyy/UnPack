using System.Runtime.InteropServices;
using nn.util;

namespace nn.hid
{
	public struct ControllerSupportArg
	{
		private const int ExplainTextSize = 516;

		public byte playerCountMin;

		public byte playerCountMax;

		[MarshalAs(UnmanagedType.U1)]
		public bool enableTakeOverConnection;

		[MarshalAs(UnmanagedType.U1)]
		public bool enableLeftJustify;

		[MarshalAs(UnmanagedType.U1)]
		public bool enablePermitJoyDual;

		[MarshalAs(UnmanagedType.U1)]
		public bool enableSingleMode;

		[MarshalAs(UnmanagedType.U1)]
		public bool enableIdentificationColor;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public Color4u8[] identificationColor;

		[MarshalAs(UnmanagedType.I1)]
		public bool enableExplainText;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 516)]
		private byte[] explainText;

		public void SetDefault()
		{
			playerCountMin = 0;
			playerCountMax = 4;
			enableTakeOverConnection = true;
			enableLeftJustify = true;
			enablePermitJoyDual = true;
			enableSingleMode = false;
			enableIdentificationColor = false;
			identificationColor = new Color4u8[4];
			enableExplainText = false;
			explainText = new byte[516];
		}

		public override string ToString()
		{
			return $"Min{playerCountMin} Max{playerCountMax} TOC{enableTakeOverConnection} LJ{enableLeftJustify} PJD{enablePermitJoyDual} SM{enableSingleMode} IC{enableIdentificationColor} C0{identificationColor[0]} C1{identificationColor[1]} C2{identificationColor[2]} C3{identificationColor[3]} ET{enableExplainText}";
		}
	}
}
