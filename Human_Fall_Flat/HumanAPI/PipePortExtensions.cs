using UnityEngine;

namespace HumanAPI
{
	public static class PipePortExtensions
	{
		public static bool CanConnect(this PipePort lhs, PipePort rhs)
		{
			if ((Object)(object)rhs != (Object)null && (Object)(object)rhs.connectedPort == (Object)null && (Object)(object)lhs.connectedPort == (Object)null && lhs.connectable && rhs.connectable)
			{
				return lhs.isMale != rhs.isMale;
			}
			return false;
		}
	}
}
