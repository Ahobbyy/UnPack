namespace nn.account
{
	public struct NetworkServiceAccountId
	{
		public ulong id;

		public override string ToString()
		{
			return $"{id:X}";
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public static bool operator ==(NetworkServiceAccountId lhs, NetworkServiceAccountId rhs)
		{
			return Nn.OperatorEquals(lhs, rhs);
		}

		public static bool operator !=(NetworkServiceAccountId lhs, NetworkServiceAccountId rhs)
		{
			return !(lhs == rhs);
		}
	}
}
