using UnityEngine;

public class AngleSnapping : MonoBehaviour, IGrabbable
{
	public float strength = 1f;

	private bool isGrabbed;

	private void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		if (isGrabbed)
		{
			Vector3 eulerAngles = ((Component)this).get_transform().get_eulerAngles();
			float num = Mathf.Abs(eulerAngles.y % 90f);
			if ((num > 65f && num < 89f) || (num < 30f && num > 1f))
			{
				eulerAngles.y = Mathf.Round(eulerAngles.y / 90f) * 90f;
			}
			((Component)this).get_transform().set_rotation(Quaternion.Slerp(((Component)this).get_transform().get_rotation(), Quaternion.Euler(eulerAngles), Time.get_deltaTime() * strength));
		}
	}

	public void OnGrab()
	{
		isGrabbed = true;
	}

	public void OnRelease()
	{
		isGrabbed = false;
	}

	public AngleSnapping()
		: this()
	{
	}
}
