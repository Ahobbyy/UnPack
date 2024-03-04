using System;
using UnityEngine;

namespace SimpleLightProbePlacer
{
	[Serializable]
	public struct Volume
	{
		[SerializeField]
		private Vector3 m_origin;

		[SerializeField]
		private Vector3 m_size;

		public Vector3 Origin => m_origin;

		public Vector3 Size => m_size;

		public Volume(Vector3 origin, Vector3 size)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			m_origin = origin;
			m_size = size;
		}

		public static bool operator ==(Volume left, Volume right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Volume left, Volume right)
		{
			return !left.Equals(right);
		}

		public bool Equals(Volume other)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (Origin == other.Origin)
			{
				return Size == other.Size;
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Volume)
			{
				return Equals((Volume)obj);
			}
			return false;
		}

		public override int GetHashCode()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Origin;
			int num = ((object)(Vector3)(ref val)).GetHashCode() * 397;
			val = Size;
			return num ^ ((object)(Vector3)(ref val)).GetHashCode();
		}

		public override string ToString()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return $"Origin: {Origin}, Size: {Size}";
		}
	}
}
