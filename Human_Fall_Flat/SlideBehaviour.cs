using UnityEngine;

public class SlideBehaviour : MonoBehaviour
{
	public float SideDrag = 0.5f;

	public float groundDistance = 0.6f;

	private Rigidbody rb;

	private bool grounded = true;

	private void Start()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		grounded = Physics.Raycast(new Ray(((Component)this).get_transform().get_position(), -((Component)this).get_transform().get_up()), groundDistance);
		Color val = (grounded ? Color.get_green() : Color.get_red());
		Debug.DrawRay(((Component)this).get_transform().get_position(), -((Component)this).get_transform().get_up() * groundDistance, val);
	}

	private void FixedUpdate()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (grounded)
		{
			Vector3 val = ((Component)this).get_transform().InverseTransformVector(rb.get_velocity());
			rb.AddForce(-((Component)this).get_transform().get_right() * SideDrag * val.x + ((Component)this).get_transform().get_forward() * SideDrag * Mathf.Abs(val.x) * Mathf.Sign(val.z) * 0.5f);
			Debug.DrawRay(((Component)this).get_transform().get_position(), -((Component)this).get_transform().get_right() * SideDrag * val.x / 10f);
		}
	}

	public SlideBehaviour()
		: this()
	{
	}
}
