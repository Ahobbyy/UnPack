using UnityEngine;

public class FixOnRelease : MonoBehaviour, IGrabbable
{
	public Rigidbody lockParent;

	public FixedJoint joint;

	public void OnGrab()
	{
		if ((Object)(object)joint != (Object)null)
		{
			Object.Destroy((Object)(object)joint);
		}
		joint = null;
	}

	public void OnRelease()
	{
		joint = ((Component)this).get_gameObject().AddComponent<FixedJoint>();
		((Joint)joint).set_connectedBody(lockParent);
	}

	private void Start()
	{
		joint = ((Component)this).get_gameObject().AddComponent<FixedJoint>();
		((Joint)joint).set_connectedBody(lockParent);
	}

	public FixOnRelease()
		: this()
	{
	}
}
