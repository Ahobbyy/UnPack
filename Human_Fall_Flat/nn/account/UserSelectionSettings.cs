using System.Runtime.InteropServices;
using System.Text;

namespace nn.account
{
	public struct UserSelectionSettings
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public Uid[] invalidUidList;

		[MarshalAs(UnmanagedType.U1)]
		public bool isSkipEnabled;

		[MarshalAs(UnmanagedType.U1)]
		public bool isNetworkServiceAccountRequired;

		[MarshalAs(UnmanagedType.U1)]
		public bool showSkipButton;

		[MarshalAs(UnmanagedType.U1)]
		public bool additionalSelect;

		public void SetDefault()
		{
			invalidUidList = new Uid[8];
			isSkipEnabled = false;
			isNetworkServiceAccountRequired = false;
			showSkipButton = false;
			additionalSelect = false;
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("skip:{0} netAccount:{1} skipButton:{2} addSelect:{3} ignore:[ ", isSkipEnabled, isNetworkServiceAccountRequired, showSkipButton, additionalSelect);
			for (int i = 0; i < 8; i++)
			{
				if (invalidUidList[i] != Uid.Invalid)
				{
					stringBuilder.Append(invalidUidList[i].ToString());
					stringBuilder.Append(" ");
				}
				stringBuilder.Append("]");
			}
			return stringBuilder.ToString();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(UserSelectionSettings lhs, UserSelectionSettings rhs)
		{
			return Nn.OperatorEquals(lhs, rhs);
		}

		public static bool operator !=(UserSelectionSettings lhs, UserSelectionSettings rhs)
		{
			return !(lhs == rhs);
		}
	}
}
