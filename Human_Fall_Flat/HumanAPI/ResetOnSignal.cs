using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class ResetOnSignal : Node
	{
		public float height;

		public NodeInput input;

		public override void Process()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			if (input.value > 0.5f)
			{
				Respawn(((Component)this).get_transform(), Vector3.get_up() * height);
				IReset[] componentsInChildren = ((Component)this).GetComponentsInChildren<IReset>(true);
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].ResetState(Game.instance.currentCheckpointNumber, Game.instance.currentCheckpointSubObjectives);
				}
				IPostReset[] componentsInChildren2 = ((Component)this).GetComponentsInChildren<IPostReset>(true);
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					componentsInChildren2[j].PostResetState(Game.instance.currentCheckpointNumber);
				}
			}
		}

		private void Respawn(Transform transform, Vector3 offset)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			IRespawnable[] components = ((Component)transform).GetComponents<IRespawnable>();
			for (int i = 0; i < components.Length; i++)
			{
				components[i].Respawn(offset);
			}
			for (int j = 0; j < transform.get_childCount(); j++)
			{
				Respawn(transform.GetChild(j), offset);
			}
		}
	}
}
