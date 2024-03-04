using UnityEngine;

public class ScrewHinge : MonoBehaviour
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

	private Rigidbody vrb;

	[SerializeField]
	private float screwRatio;

	[SerializeField]
	private GameObject vertical;

	[SerializeField]
	private float heightMax;

	[SerializeField]
	private float heightMin;

	private void Start()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		rb = ((Component)this).GetComponent<Rigidbody>();
		vrb = vertical.GetComponent<Rigidbody>();
		preAngle = ((Component)this).get_transform().get_rotation();
		preHeight = ((Component)this).get_transform().get_position().y;
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		diffAngle = Quaternion.Angle(((Component)this).get_transform().get_rotation(), preAngle);
		if (diffAngle > deadAngle)
		{
			vrb.set_isKinematic(true);
			if (((Quaternion)(ref preAngle)).get_eulerAngles().y > ((Component)this).get_transform().get_eulerAngles().y)
			{
				diffAngle = 0f - diffAngle;
			}
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(((Component)this).get_transform().get_position().x, ((Component)this).get_transform().get_position().y + diffAngle * screwRatio, ((Component)this).get_transform().get_position().z);
			vrb.MovePosition(val);
		}
		else
		{
			vrb.set_isKinematic(false);
			diffHeight = vertical.get_transform().get_position().y - preHeight;
			Vector3 val2 = default(Vector3);
			((Vector3)(ref val2))._002Ector(((Component)this).get_transform().get_eulerAngles().x, ((Component)this).get_transform().get_eulerAngles().y + diffHeight / screwRatio, ((Component)this).get_transform().get_eulerAngles().z);
			vrb.MoveRotation(Quaternion.Euler(val2));
		}
		preHeight = vertical.get_transform().get_position().y;
		preAngle = ((Component)this).get_transform().get_rotation();
	}

	public ScrewHinge()
		: this()
	{
	}
}
