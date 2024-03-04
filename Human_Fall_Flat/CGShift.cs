using UnityEngine;

public class CGShift : MonoBehaviour, IReset
{
	[Tooltip("Offset in local space")]
	public Vector3 localOffset;

	[Tooltip("Local offset for the effect")]
	public Vector3 offset;

	[Tooltip("Multiplier for the centre of gravity")]
	public Vector3 inertiaMultiplier = Vector3.get_one();

	private Vector3 originalCG;

	private Vector3 originalInertia;

	public bool showdebug;

	private bool SeenString1;

	private bool SeenString2;

	private bool SeenString3;

	private Rigidbody body;

	private bool applied;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		body = ((Component)this).GetComponent<Rigidbody>();
		originalCG = body.get_centerOfMass();
		originalInertia = body.get_inertiaTensor();
	}

	private void OnEnable()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (localOffset == Vector3.get_zero() && offset != Vector3.get_zero())
		{
			if (showdebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Local offset is zero and offset is not zero "));
			}
			localOffset = body.get_centerOfMass() + ((Component)this).get_transform().InverseTransformVector(offset);
		}
		body.set_centerOfMass(localOffset);
		body.set_inertiaTensor(Vector3.Scale(originalInertia, inertiaMultiplier));
	}

	public void OnDrawGizmosSelected()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if (Application.get_isPlaying())
		{
			Gizmos.DrawSphere(((Component)this).GetComponent<Rigidbody>().get_worldCenterOfMass(), 0.05f);
			if (!SeenString1)
			{
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Application is playing , draw the gizmo "));
				}
				SeenString1 = true;
			}
			return;
		}
		if (localOffset == Vector3.get_zero() && offset != Vector3.get_zero())
		{
			if (!SeenString3)
			{
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Local offset is zero and offset is not zero "));
				}
				SeenString3 = true;
			}
			Gizmos.DrawSphere(((Component)this).GetComponent<Rigidbody>().get_worldCenterOfMass() + offset, 0.05f);
			return;
		}
		if (!SeenString2)
		{
			if (showdebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Local offset is not zero and offset is zero "));
			}
			SeenString2 = true;
		}
		Gizmos.DrawSphere(((Component)this).get_transform().TransformPoint(localOffset), 0.05f);
	}

	public void ResetCG()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (showdebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Trying to reset the CG effect "));
		}
		if (!Object.op_Implicit((Object)(object)body))
		{
			Debug.Log((object)(((Object)this).get_name() + " Body is not set "));
		}
		if (!((Object)(object)body == (Object)null))
		{
			body.set_centerOfMass(originalCG);
			body.set_inertiaTensor(originalInertia);
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (showdebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Trying to reset the state of the effect "));
		}
		if (!((Object)(object)body == (Object)null))
		{
			body.set_centerOfMass(localOffset);
			body.set_inertiaTensor(Vector3.Scale(originalInertia, inertiaMultiplier));
		}
	}

	public CGShift()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
