using System;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class CandyCane : MonoBehaviour
{
	public Transform axis;

	public float holdDamper = 5000f;

	public float breakAngleTravel = 90f;

	public Transform groundedIgnoreCollider;

	public Sound2 sound;

	private ConfigurableJoint joint;

	private Vector3 anchor;

	private float totalTravel;

	private NetIdentity identity;

	private uint evtPull;

	private void OnEnable()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		identity = ((Component)this).GetComponentInParent<NetIdentity>();
		evtPull = identity.RegisterEvent(OnPull);
		if (NetGame.isServer)
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
		if ((Object)(object)sound != (Object)null)
		{
			sound.PlayOneShot();
		}
	}

	private ConfigurableJoint CreateJoint()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
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
	}

	private void OnBeep(NetStream stream)
	{
		sound.PlayOneShot();
	}

	public CandyCane()
		: this()
	{
	}
}
