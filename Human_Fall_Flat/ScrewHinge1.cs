using UnityEngine;

public class ScrewHinge1 : MonoBehaviour
{
	private Quaternion preAngle;

	private float preHeight;

	[SerializeField]
	private float deadAngle;

	[SerializeField]
	private float diffAngle;

	[SerializeField]
	private float diffHeight;

	private Rigidbody rb;

	[SerializeField]
	private float screwRatio;

	[SerializeField]
	private float heightMax;

	[SerializeField]
	private float heightMin;

	private void Start()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		rb = ((Component)this).GetComponent<Rigidbody>();
		preAngle = ((Component)this).get_transform().get_rotation();
		preHeight = ((Component)this).get_transform().get_position().y;
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		preHeight = ((Component)this).get_transform().get_position().y;
		diffAngle = Quaternion.Angle(((Component)this).get_transform().get_rotation(), preAngle);
		if (diffAngle > deadAngle)
		{
			if (((Quaternion)(ref preAngle)).get_eulerAngles().y > ((Component)this).get_transform().get_eulerAngles().y)
			{
				diffAngle = 0f - diffAngle;
			}
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(((Component)this).get_transform().get_position().x, ((Component)this).get_transform().get_position().y + diffAngle * screwRatio, ((Component)this).get_transform().get_position().z);
			bool flag = false;
			if (val.y > heightMax && ((Quaternion)(ref preAngle)).get_eulerAngles().y < ((Component)this).get_transform().get_eulerAngles().y)
			{
				flag = true;
			}
			if (val.y < heightMin && ((Quaternion)(ref preAngle)).get_eulerAngles().y > ((Component)this).get_transform().get_eulerAngles().y)
			{
				flag = true;
			}
			if (!flag)
			{
				rb.MovePosition(val);
			}
			else
			{
				((Component)this).get_transform().set_rotation(preAngle);
			}
		}
		preAngle = ((Component)this).get_transform().get_rotation();
	}

	public ScrewHinge1()
		: this()
	{
	}
}
