using Multiplayer;
using UnityEngine;

public class MassChangeOnGrab : MonoBehaviour, IGrabbable
{
	public float massMultiplyOnGrab = 1f;

	public Rigidbody rigid;

	private float mass;

	private void OnEnable()
	{
		if (!NetGame.isClient)
		{
			if ((Object)(object)rigid == (Object)null)
			{
				rigid = ((Component)this).GetComponent<Rigidbody>();
			}
			mass = rigid.get_mass();
		}
	}

	public void OnGrab()
	{
		if (!NetGame.isClient)
		{
			rigid.set_mass(mass * massMultiplyOnGrab);
		}
	}

	public void OnRelease()
	{
		if (!NetGame.isClient)
		{
			rigid.set_mass(mass);
		}
	}

	public MassChangeOnGrab()
		: this()
	{
	}
}
