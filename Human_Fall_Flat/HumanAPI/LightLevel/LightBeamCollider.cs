using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightBeamCollider : MonoBehaviour
	{
		public LightBeam beam;

		private void Awake()
		{
			if (!Object.op_Implicit((Object)(object)beam))
			{
				beam = ((Component)this).GetComponentInParent<LightBeam>();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (!other.get_isTrigger())
			{
				if (!beam.hitColliders.ContainsKey(((Component)other).get_transform()))
				{
					beam.hitColliders.Add(((Component)other).get_transform(), ((Component)other).get_transform().get_position());
				}
				beam.UpdateLight();
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (!other.get_isTrigger())
			{
				if (beam.hitColliders.ContainsKey(((Component)other).get_transform()))
				{
					beam.hitColliders.Remove(((Component)other).get_transform());
				}
				beam.UpdateLight();
			}
		}

		public LightBeamCollider()
			: this()
		{
		}
	}
}
