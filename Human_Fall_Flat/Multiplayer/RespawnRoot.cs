using UnityEngine;

namespace Multiplayer
{
	public class RespawnRoot : MonoBehaviour
	{
		public void Respawn(Vector3 offset)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			IRespawnable[] componentsInChildren = ((Component)((Component)this).get_transform()).GetComponentsInChildren<IRespawnable>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Respawn(offset);
			}
		}

		public RespawnRoot()
			: this()
		{
		}
	}
}
