using UnityEngine;

namespace ProBuilder2.Examples
{
	public class CameraControls : MonoBehaviour
	{
		private const string INPUT_MOUSE_SCROLLWHEEL = "Mouse ScrollWheel";

		private const string INPUT_MOUSE_X = "Mouse X";

		private const string INPUT_MOUSE_Y = "Mouse Y";

		private const float MIN_CAM_DISTANCE = 10f;

		private const float MAX_CAM_DISTANCE = 40f;

		[Range(2f, 15f)]
		public float orbitSpeed = 6f;

		[Range(0.3f, 2f)]
		public float zoomSpeed = 0.8f;

		private float distance;

		public float idleRotation = 1f;

		private Vector2 dir = new Vector2(0.8f, 0.2f);

		private void Start()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			distance = Vector3.Distance(((Component)this).get_transform().get_position(), Vector3.get_zero());
		}

		private void LateUpdate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			Quaternion localRotation = ((Component)this).get_transform().get_localRotation();
			Vector3 eulerAngles = ((Quaternion)(ref localRotation)).get_eulerAngles();
			eulerAngles.z = 0f;
			if (Input.GetMouseButton(0))
			{
				float axis = Input.GetAxis("Mouse X");
				float num = 0f - Input.GetAxis("Mouse Y");
				eulerAngles.x += num * orbitSpeed;
				eulerAngles.y += axis * orbitSpeed;
				dir.x = axis;
				dir.y = num;
				((Vector2)(ref dir)).Normalize();
			}
			else
			{
				eulerAngles.y += Time.get_deltaTime() * idleRotation * dir.x;
				eulerAngles.x += Time.get_deltaTime() * Mathf.PerlinNoise(Time.get_time(), 0f) * idleRotation * dir.y;
			}
			((Component)this).get_transform().set_localRotation(Quaternion.Euler(eulerAngles));
			((Component)this).get_transform().set_position(((Component)this).get_transform().get_localRotation() * (Vector3.get_forward() * (0f - distance)));
			if (Input.GetAxis("Mouse ScrollWheel") != 0f)
			{
				float axis2 = Input.GetAxis("Mouse ScrollWheel");
				distance -= axis2 * (distance / 40f) * (zoomSpeed * 1000f) * Time.get_deltaTime();
				distance = Mathf.Clamp(distance, 10f, 40f);
				((Component)this).get_transform().set_position(((Component)this).get_transform().get_localRotation() * (Vector3.get_forward() * (0f - distance)));
			}
		}

		public CameraControls()
			: this()
		{
		}//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)

	}
}
