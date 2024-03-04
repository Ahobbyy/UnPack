using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(BoxCollider))]
	public class ReverbZone : MonoBehaviour
	{
		public float weight = 1f;

		public float level;

		public float delay = 0.5f;

		public float diffusion = 0.5f;

		public float innerZoneOffset = 2f;

		public float lowPass = 22000f;

		public float highPass = 10f;

		private BoxCollider collider;

		private void OnEnable()
		{
			collider = ((Component)this).GetComponent<BoxCollider>();
		}

		public void OnDrawGizmosSelected()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			collider = ((Component)this).GetComponent<BoxCollider>();
			Matrix4x4 matrix = Gizmos.get_matrix();
			Gizmos.set_matrix(((Component)this).get_transform().get_localToWorldMatrix());
			Gizmos.set_color(new Color(1f, 0f, 0f, 0.8f));
			Gizmos.DrawCube(collider.get_center(), collider.get_size() - Vector3.get_one() * innerZoneOffset * 2f);
			Gizmos.set_color(new Color(1f, 1f, 0f, 0.2f));
			Gizmos.DrawCube(collider.get_center(), collider.get_size());
			Gizmos.set_matrix(matrix);
		}

		public void OnTriggerEnter(Collider other)
		{
			if (((Component)other).get_tag() == "Player")
			{
				ReverbManager.instance.ZoneEntered(this);
			}
		}

		public void OnTriggerExit(Collider other)
		{
			if (((Component)other).get_tag() == "Player")
			{
				ReverbManager.instance.ZoneLeft(this);
			}
		}

		public float GetWeight(Vector3 pos)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = ((Component)this).get_transform().InverseTransformPoint(pos) - collider.get_center();
			float num = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().x / 2f - Mathf.Abs(val.x));
			float num2 = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().y / 2f - Mathf.Abs(val.y));
			float num3 = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().z / 2f - Mathf.Abs(val.z));
			return Mathf.Min(Mathf.Min(num, num2), num3) * weight;
		}

		public ReverbZone()
			: this()
		{
		}
	}
}
