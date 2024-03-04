using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Level/Fall Trigger", 10)]
	public class FallTrigger : LevelObject
	{
		public bool fallAchievement = true;

		public void OnTriggerEnter(Collider other)
		{
			if (base.active)
			{
				HumanBase componentInParent = ((Component)other).GetComponentInParent<HumanBase>();
				if ((Object)(object)componentInParent != (Object)null)
				{
					Dependencies.Get<IGame>().Fall(componentInParent, drown: false, fallAchievement);
				}
			}
		}
	}
}
