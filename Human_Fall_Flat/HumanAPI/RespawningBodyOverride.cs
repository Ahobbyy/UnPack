using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class RespawningBodyOverride : MonoBehaviour
	{
		public NetBody netBody;

		public Checkpoint checkpoint;

		internal Matrix4x4 initialToNewLocationMatrtix;

		internal Quaternion initialToNewLocationRotation;

		private void Awake()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			initialToNewLocationMatrtix = ((Component)this).get_transform().get_localToWorldMatrix() * ((Component)netBody).get_transform().get_worldToLocalMatrix();
			initialToNewLocationRotation = ((Component)this).get_transform().get_rotation() * Quaternion.Inverse(((Component)netBody).get_transform().get_rotation());
			NetBody[] componentsInChildren = ((Component)netBody).GetComponentsInChildren<NetBody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].AddOverride(this);
			}
		}

		public RespawningBodyOverride()
			: this()
		{
		}
	}
}
