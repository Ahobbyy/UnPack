using UnityEngine;

namespace HumanAPI
{
	public class AmbienceZone : MonoBehaviour
	{
		public int priority;

		public float transitionDuration = 3f;

		public float mainVerbLevel;

		public float musicLevel;

		public float ambienceLevel;

		public float effectsLevel;

		public float physicsLevel;

		public float characterLevel;

		public float ambienceFxLevel;

		public AmbienceSource[] sources;

		public float[] volumes;

		public float GetLevel(AmbienceSource source)
		{
			if (sources == null)
			{
				return 0f;
			}
			for (int i = 0; i < sources.Length; i++)
			{
				if ((Object)(object)sources[i] == (Object)(object)source)
				{
					return volumes[i];
				}
			}
			return 0f;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (Ambience.instance.useTriggers)
			{
				Ambience.instance.EnterZone(this);
			}
		}

		public void OnTriggerExit(Collider other)
		{
			if (Ambience.instance.useTriggers)
			{
				Ambience.instance.LeaveZone(this);
			}
		}

		public void OnDrawGizmosSelected()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			Matrix4x4 matrix = Gizmos.get_matrix();
			Gizmos.set_matrix(((Component)this).get_transform().get_localToWorldMatrix());
			Gizmos.set_color(new Color(0f, 0f, 1f, 0.5f));
			BoxCollider component = ((Component)this).GetComponent<BoxCollider>();
			if ((Object)(object)component != (Object)null)
			{
				Gizmos.DrawCube(component.get_center(), component.get_size());
			}
			MeshCollider component2 = ((Component)this).GetComponent<MeshCollider>();
			if ((Object)(object)component2 != (Object)null)
			{
				Gizmos.DrawMesh(component2.get_sharedMesh());
			}
			Gizmos.set_matrix(matrix);
		}

		public AmbienceZone()
			: this()
		{
		}
	}
}
