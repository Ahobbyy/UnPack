using System;
using Multiplayer;
using UnityEngine;

public class CandyCaneSW : MonoBehaviour
{
	[Tooltip("The orientation information for a generated configurable joint")]
	public Transform axis;

	[Tooltip("The amount of damping to overcome and pull")]
	public float holdDamper = 5000f;

	[Tooltip("The amount of change in degrees for this to break")]
	public float breakAngleTravel = 90f;

	[Tooltip("Ground plane for this to ignore when poking out of the ground")]
	public Transform groundedIgnoreCollider;

	[Tooltip("Sound to play when this cane is pulled from the ground")]
	public AudioSource audioSource;

	private ConfigurableJoint joint;

	private Vector3 anchor;

	private float totalTravel;

	private NetIdentity identity;

	private uint evtPull;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void OnEnable()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Enabled "));
		}
		identity = ((Component)this).GetComponentInParent<NetIdentity>();
		evtPull = identity.RegisterEvent(OnPull);
		if (!NetGame.isClient)
		{
			if ((Object)(object)joint == (Object)null)
			{
				joint = CreateJoint();
			}
			anchor = axis.TransformPoint(Vector3.get_forward());
			totalTravel = 0f;
			if ((Object)(object)groundedIgnoreCollider != (Object)null)
			{
				IgnoreCollision.Ignore(((Component)this).get_transform(), groundedIgnoreCollider);
			}
		}
	}

	private void OnPull(NetStream stream)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Pulled "));
		}
		if ((Object)(object)audioSource != (Object)null)
		{
			audioSource.Play();
		}
	}

	private ConfigurableJoint CreateJoint()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Create Joint "));
		}
		ConfigurableJoint obj = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
		((Joint)obj).set_anchor(axis.get_localPosition());
		((Joint)obj).set_axis(((Component)this).get_transform().InverseTransformDirection(axis.get_forward()));
		obj.set_secondaryAxis(((Component)this).get_transform().InverseTransformDirection(axis.get_right()));
		obj.set_xMotion((ConfigurableJointMotion)0);
		obj.set_yMotion((ConfigurableJointMotion)0);
		obj.set_zMotion((ConfigurableJointMotion)0);
		obj.set_angularXMotion((ConfigurableJointMotion)2);
		obj.set_angularYMotion((ConfigurableJointMotion)2);
		obj.set_angularZMotion((ConfigurableJointMotion)2);
		JointDrive val = default(JointDrive);
		((JointDrive)(ref val)).set_positionSpring(0f);
		((JointDrive)(ref val)).set_positionDamper(holdDamper);
		((JointDrive)(ref val)).set_maximumForce(float.PositiveInfinity);
		obj.set_angularXDrive(val);
		val = default(JointDrive);
		((JointDrive)(ref val)).set_positionSpring(0f);
		((JointDrive)(ref val)).set_positionDamper(holdDamper);
		((JointDrive)(ref val)).set_maximumForce(float.PositiveInfinity);
		obj.set_angularYZDrive(val);
		return obj;
	}

	public void FixedUpdate()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)joint == (Object)null || NetGame.isClient || ReplayRecorder.isPlaying)
		{
			return;
		}
		Vector3 val = axis.TransformPoint(Vector3.get_forward());
		Vector3 val2 = val - anchor;
		float num = ((Vector3)(ref val2)).get_magnitude() - 0.1f;
		if (!(num > 0f))
		{
			return;
		}
		totalTravel += num / (breakAngleTravel * ((float)Math.PI / 180f));
		anchor = Vector3.MoveTowards(anchor, val, num);
		if (totalTravel > 1f)
		{
			identity.BeginEvent(evtPull);
			identity.EndEvent();
			OnPull(null);
			Object.Destroy((Object)(object)joint);
			((Component)this).GetComponent<Rigidbody>().set_angularDrag(0.05f);
			((Component)this).GetComponent<Rigidbody>().set_useGravity(true);
			if ((Object)(object)groundedIgnoreCollider != (Object)null)
			{
				IgnoreCollision.Unignore(((Component)this).get_transform(), groundedIgnoreCollider);
			}
		}
		else
		{
			ConfigurableJoint obj = joint;
			JointDrive angularYZDrive = default(JointDrive);
			((JointDrive)(ref angularYZDrive)).set_positionSpring(0f);
			((JointDrive)(ref angularYZDrive)).set_positionDamper(holdDamper * (1f - totalTravel));
			((JointDrive)(ref angularYZDrive)).set_maximumForce(float.PositiveInfinity);
			obj.set_angularYZDrive(angularYZDrive);
		}
	}

	private void Awake()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Awake "));
		}
	}

	private void OnBeep(NetStream stream)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Beep "));
		}
		audioSource.Play();
	}

	public CandyCaneSW()
		: this()
	{
	}
}
