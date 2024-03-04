using System;
using System.Collections.Generic;
using System.Linq;
using HumanAPI;
using UnityEngine;

public class MagneticBody : Node
{
	public NodeInput input;

	public bool toggleViaSignal;

	[NonSerialized]
	public bool magnetActive = true;

	public bool disableOnContact;

	private List<Rigidbody> contactMagnetBodies = new List<Rigidbody>();

	[Tooltip("After a short time the magnets are fixed together")]
	public bool fixInPlace;

	[Tooltip("Once the Magnets are fixed turn off the Magnet ")]
	public bool disableOnFix;

	[Tooltip("The specific body to fix to this Magnet")]
	public Rigidbody bodyToFix;

	[Tooltip("How long to waut before fixing something")]
	public float timeToFix;

	private Joint fixJoint;

	private float fixTimer;

	private MagneticPoint[] magneticPoints;

	private List<MagneticPoint> nearbyMagnetics = new List<MagneticPoint>();

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	public Rigidbody Body { get; private set; }

	public IEnumerable<MagneticPoint> NearbyMagnetics { get; private set; }

	public bool IsInContact(Rigidbody body)
	{
		return contactMagnetBodies.Contains(body);
	}

	private void Start()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Start "));
		}
		Body = ((Component)this).GetComponentInParent<Rigidbody>();
		magneticPoints = ((Component)this).GetComponentsInChildren<MagneticPoint>();
		MagneticPoint[] array = magneticPoints;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].magneticBody = this;
		}
		NearbyMagnetics = nearbyMagnetics.Distinct();
	}

	private void FixedUpdate()
	{
		if (!fixInPlace || !((Object)(object)fixJoint == (Object)null) || !((Object)(object)bodyToFix != (Object)null))
		{
			return;
		}
		bool flag = false;
		foreach (Rigidbody contactMagnetBody in contactMagnetBodies)
		{
			if ((Object)(object)bodyToFix == (Object)(object)contactMagnetBody && !GrabManager.IsGrabbedAny(((Component)contactMagnetBody).get_gameObject()))
			{
				flag = true;
			}
		}
		if (flag)
		{
			fixTimer += Time.get_fixedDeltaTime();
			if (fixTimer >= timeToFix)
			{
				fixJoint = (Joint)(object)((Component)bodyToFix).get_gameObject().AddComponent<FixedJoint>();
				fixJoint.set_connectedBody(Body);
				CalculateMagnetActive();
				fixTimer = 0f;
			}
		}
		else
		{
			fixTimer = 0f;
		}
	}

	private void CalculateMagnetActive()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Magnet Active"));
		}
		magnetActive = (!toggleViaSignal || Mathf.Abs(input.value) >= 0.5f) && (!disableOnFix || (Object)(object)fixJoint == (Object)null);
	}

	public override void Process()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Process "));
		}
		base.Process();
		CalculateMagnetActive();
	}

	private void OnTriggerEnter(Collider other)
	{
		MagneticBody componentInParent = ((Component)other).get_gameObject().GetComponentInParent<MagneticBody>();
		if (!((Object)(object)componentInParent != (Object)null))
		{
			return;
		}
		MagneticPoint[] array = componentInParent.magneticPoints;
		if (array == null)
		{
			return;
		}
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Other Magnet "));
		}
		MagneticPoint[] array2 = array;
		foreach (MagneticPoint magneticPoint in array2)
		{
			if (((Behaviour)magneticPoint).get_isActiveAndEnabled())
			{
				nearbyMagnetics.Add(magneticPoint);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		MagneticBody componentInParent = ((Component)other).get_gameObject().GetComponentInParent<MagneticBody>();
		if (!((Object)(object)componentInParent != (Object)null))
		{
			return;
		}
		MagneticPoint[] array = componentInParent.magneticPoints;
		if (array == null)
		{
			return;
		}
		MagneticPoint[] array2 = array;
		foreach (MagneticPoint magneticPoint in array2)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Magnet Gone "));
			}
			if (((Behaviour)magneticPoint).get_isActiveAndEnabled())
			{
				nearbyMagnetics.Remove(magneticPoint);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((Object)(object)collision.get_collider().get_attachedRigidbody() == (Object)null)
		{
			return;
		}
		MagneticBody componentInChildren = ((Component)collision.get_collider().get_attachedRigidbody()).get_gameObject().GetComponentInChildren<MagneticBody>();
		if (!((Object)(object)componentInChildren == (Object)null) && !((Object)(object)componentInChildren == (Object)(object)this))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Adding Magnet "));
			}
			contactMagnetBodies.Add(componentInChildren.Body);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if ((Object)(object)collision.get_collider().get_attachedRigidbody() == (Object)null)
		{
			return;
		}
		MagneticBody componentInChildren = ((Component)collision.get_collider().get_attachedRigidbody()).get_gameObject().GetComponentInChildren<MagneticBody>();
		if (!((Object)(object)componentInChildren == (Object)null))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Removing Magnet "));
			}
			contactMagnetBodies.Remove(componentInChildren.Body);
		}
	}
}
