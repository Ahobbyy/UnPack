using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Level/Checkpoint", 10)]
	public class Checkpoint : LevelObject
	{
		[Tooltip("The number of the current checkpoint - increment with each new point")]
		public int number;

		[Tooltip("This is considered a 'Sub-objective' of the checkpoint number above. Sub-objectives can be completed in any order (Set to anything above than 0)")]
		public int subObjective;

		[Tooltip("If ticked, players in a multiplayer game will spawn more tightly at this checkpoint")]
		public bool tightSpawn;

		public bool debugLog;

		public virtual void OnTriggerEnter(Collider other)
		{
			if (base.active && !(((Component)other).get_tag() != "Player"))
			{
				Dependencies.Get<IGame>().EnterCheckpoint(number, subObjective);
				if (debugLog)
				{
					Debug.Log((object)("Checkpoint passed: " + ((Object)((Component)this).get_gameObject()).get_name()));
				}
			}
		}

		protected void Pass()
		{
			if (base.active)
			{
				Dependencies.Get<IGame>().EnterCheckpoint(number, subObjective);
				if (debugLog)
				{
					Debug.Log((object)("Checkpoint passed: " + ((Object)((Component)this).get_gameObject()).get_name()));
				}
			}
		}

		public virtual void LoadHere()
		{
		}

		public virtual void RespawnHere()
		{
		}
	}
}
