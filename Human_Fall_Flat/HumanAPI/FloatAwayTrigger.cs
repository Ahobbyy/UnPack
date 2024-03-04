using UnityEngine;

namespace HumanAPI
{
	public class FloatAwayTrigger : LevelObject
	{
		public void OnTriggerExit(Collider other)
		{
			if (base.active)
			{
				HumanBase componentInParent = ((Component)other).GetComponentInParent<HumanBase>();
				if ((Object)(object)componentInParent != (Object)null)
				{
					Dependencies.Get<IGame>().Fall(componentInParent, drown: false, fallAchievement: false);
				}
			}
		}
	}
}
