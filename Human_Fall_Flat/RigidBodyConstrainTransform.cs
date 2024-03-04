using UnityEngine;

public class RigidBodyConstrainTransform : MonoBehaviour
{
	private Rigidbody mRigidBody;

	private RigidbodyConstraints mConstraints;

	public bool FreezePositionAtStart = true;

	public bool FreezeRotationAtStart = true;

	public bool ConstrainPosition
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mRigidBody != (Object)null)
			{
				if ((mRigidBody.get_constraints() & 0xE) == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (value)
			{
				Rigidbody obj = mRigidBody;
				obj.set_constraints((RigidbodyConstraints)(obj.get_constraints() | 0xE));
			}
			else
			{
				Rigidbody obj2 = mRigidBody;
				obj2.set_constraints((RigidbodyConstraints)(obj2.get_constraints() & -15));
			}
		}
	}

	public bool ConstrainRotation
	{
		get
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)mRigidBody != (Object)null)
			{
				if ((mRigidBody.get_constraints() & 0x70) == 0)
				{
					return false;
				}
				return true;
			}
			return false;
		}
		set
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (value)
			{
				Rigidbody obj = mRigidBody;
				obj.set_constraints((RigidbodyConstraints)(obj.get_constraints() | 0x70));
			}
			else
			{
				Rigidbody obj2 = mRigidBody;
				obj2.set_constraints((RigidbodyConstraints)(obj2.get_constraints() & -113));
			}
		}
	}

	private void Awake()
	{
		mRigidBody = ((Component)this).GetComponent<Rigidbody>();
		Debug.Assert((Object)(object)mRigidBody != (Object)null);
	}

	private void Start()
	{
		ConstrainPosition = FreezePositionAtStart;
		ConstrainRotation = FreezeRotationAtStart;
	}

	public RigidBodyConstrainTransform()
		: this()
	{
	}
}
