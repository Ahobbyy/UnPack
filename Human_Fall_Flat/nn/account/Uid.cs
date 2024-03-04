namespace nn.account
{
	public struct Uid
	{
		public static readonly Uid Invalid;

		public ulong _data0;

		public ulong _data1;

		public bool IsValid()
		{
			if (_data0 == 0L)
			{
				return _data1 != 0;
			}
			return true;
		}

		public override string ToString()
		{
			return string.Format("{0,0:X16}{1,0:X16}", _data0, _data1);
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(Uid lhs, Uid rhs)
		{
			return Nn.OperatorEquals(lhs, rhs);
		}

		public static bool operator !=(Uid lhs, Uid rhs)
		{
			return !(lhs == rhs);
		}
	}
}
