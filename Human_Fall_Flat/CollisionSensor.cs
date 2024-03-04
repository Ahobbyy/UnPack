using System;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSensor : MonoBehaviour
{
	private const float JOINT_BREAK_FORCE = 20000f;

	public CollisionSensor otherSide;

	public bool forwardCollisionAudio = true;

	private GrabManager grabManager;

	private GroundManager groundManager;

	private float handToHandClimb = 0.2f;

	public float knockdown;

	public bool groundCheck;

	public bool grab;

	public Vector3 grabPosition;

	public Vector3 targetPosition;

	private float grabPrecision = 0.1f;

	public Collider grabFilter;

	public bool onGround;

	public Transform groundObject;

	public ConfigurableJoint grabJoint;

	public Rigidbody grabBody;

	public GameObject grabObject;

	public Action<GameObject, Vector3, PhysicMaterial, Vector3> onCollideTap;

	public Action<GameObject, Vector3, PhysicMaterial, Vector3> onGrabTap;

	public Action offGrabTap;

	public Action<CollisionSensor, Collision> onGroundTap2;

	public Action<CollisionSensor> offGroundTap2;

	public Action<CollisionSensor, Collision> onStayTap2;

	private List<Collider> activeCollider = new List<Collider>();

	private List<Collider> ativeGrounded = new List<Collider>();

	private float blockGrab;

	private Transform thisTransform;

	private Rigidbody thisBody;

	[NonSerialized]
	public Human human;

	private Vector3 entryTangentVelocityImpulse;

	private Vector3 normalTangentVelocityImpulse;

	public float groundAngle;

	private ReleaseGrabTrigger blockGrabTrigger;

	public bool IsGrabbed(GameObject go)
	{
		if ((Object)(object)grabObject == (Object)null)
		{
			return false;
		}
		Transform val = grabObject.get_transform();
		while ((Object)(object)val != (Object)null)
		{
			if ((Object)(object)((Component)val).get_gameObject() == (Object)(object)go)
			{
				return true;
			}
			val = val.get_parent();
		}
		return false;
	}

	private void OnEnable()
	{
		thisTransform = ((Component)this).get_transform();
		thisBody = ((Component)this).GetComponent<Rigidbody>();
		grabManager = ((Component)this).GetComponentInParent<GrabManager>();
		groundManager = ((Component)this).GetComponentInParent<GroundManager>();
		human = ((Component)this).GetComponentInParent<Human>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			entryTangentVelocityImpulse = (normalTangentVelocityImpulse = collision.GetNormalTangentVelocitiesAndImpulse(thisBody));
			HandleCollision(collision, enter: true);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			normalTangentVelocityImpulse = collision.GetNormalTangentVelocitiesAndImpulse(thisBody);
			HandleCollision(collision, enter: false);
			if (onStayTap2 != null)
			{
				onStayTap2(this, collision);
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (groundCheck && (Object)(object)collision.get_transform().get_root() != (Object)(object)thisTransform.get_root())
		{
			if (onGround && offGroundTap2 != null)
			{
				offGroundTap2(this);
			}
			onGround = false;
			groundObject = null;
		}
	}

	private void HandleCollision(Collision collision, bool enter)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length == 0 || !((Object)(object)thisTransform != (Object)null))
		{
			return;
		}
		Transform transform = collision.get_transform();
		if (!((Object)(object)transform.get_root() != (Object)(object)thisTransform.get_root()))
		{
			return;
		}
		Rigidbody rigidbody = collision.get_rigidbody();
		Collider collider = collision.get_collider();
		ContactPoint[] contacts = collision.get_contacts();
		if (contacts.Length != 0)
		{
			if (grab && ((Object)(object)grabFilter == (Object)null || (Object)(object)grabFilter == (Object)(object)collider))
			{
				GrabCheck(transform, rigidbody, collider, contacts);
			}
			if (groundCheck)
			{
				GroundCheck(collision);
			}
			if (enter && onCollideTap != null)
			{
				onCollideTap(((Component)this).get_gameObject(), ((ContactPoint)(ref collision.get_contacts()[0])).get_point(), collision.get_collider().get_sharedMaterial(), normalTangentVelocityImpulse);
			}
		}
	}

	private void GroundCheck(Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Vector3 impulse = collision.GetImpulse();
		float num = Vector3.Angle(((Vector3)(ref impulse)).get_normalized(), Vector3.get_up());
		if (num < groundAngle)
		{
			groundAngle = num;
		}
		ContactPoint[] contacts = collision.get_contacts();
		float num2 = 90f;
		foreach (ContactPoint contact in contacts)
		{
			float surfaceAngle = GetSurfaceAngle(contact, Vector3.get_up());
			if (surfaceAngle < 60f)
			{
				groundManager.ReportSurfaceAngle(surfaceAngle);
				if (surfaceAngle < num2)
				{
					num2 = surfaceAngle;
				}
			}
		}
		if (num2 < 60f)
		{
			if (num2 < groundAngle)
			{
				groundAngle = num2;
			}
			if (!onGround && onGroundTap2 != null)
			{
				onGroundTap2(this, collision);
			}
			onGround = true;
			groundObject = collision.get_transform();
			groundManager.ObjectEnter(((Component)groundObject).get_gameObject());
		}
	}

	private void GrabCheck(Transform collisionTransform, Rigidbody collisionRigidbody, Collider collider, ContactPoint[] contacts)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)grabFilter != (Object)null || blockGrab > 0f || !((Object)(object)grabJoint == (Object)null) || !((Object)(object)blockGrabTrigger == (Object)null) || !(((Component)collisionTransform).get_tag() != "NoGrab"))
		{
			return;
		}
		Vector3 val = targetPosition - ((ContactPoint)(ref contacts[0])).get_point();
		if (((Vector3)(ref val)).get_magnitude() > 0.5f)
		{
			return;
		}
		bool flag = (Object)(object)((Component)collider).GetComponentInParent<Human>() != (Object)null;
		if (human.onGround && ((Component)collider).get_tag() != "Target" && !flag)
		{
			bool flag2 = Object.op_Implicit((Object)(object)((Component)collider).GetComponentInParent<Rigidbody>());
			Vector3 val2 = ((Component)this).get_transform().get_position() - targetPosition;
			Vector3 normal = ((ContactPoint)(ref contacts[0])).get_normal();
			if (Vector3.Dot(normal, ((Vector3)(ref val2)).get_normalized()) < (flag2 ? 0.4f : 0.7f) || Vector3.Dot(normal, val2) < (flag2 ? 0.05f : 0.2f))
			{
				return;
			}
		}
		IGrabbable componentInParent = ((Component)collisionTransform).GetComponentInParent<IGrabbable>();
		if (componentInParent != null)
		{
			grabObject = ((Component)((componentInParent is MonoBehaviour) ? componentInParent : null)).get_gameObject();
		}
		else if ((Object)(object)grabBody != (Object)null)
		{
			grabObject = ((Component)grabBody).get_gameObject();
		}
		else
		{
			grabObject = ((Component)collider).get_gameObject();
		}
		if (CheatCodes.climbCheat || human.state != HumanState.Climb || !((Object)(object)otherSide.grabObject == (Object)(object)grabObject) || !(((Component)this).get_transform().get_position().y > ((Component)otherSide).get_transform().get_position().y + handToHandClimb))
		{
			grabJoint = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
			if (Object.op_Implicit((Object)(object)collisionRigidbody))
			{
				((Joint)grabJoint).set_connectedBody(collisionRigidbody);
			}
			grabJoint.set_xMotion((ConfigurableJointMotion)0);
			grabJoint.set_yMotion((ConfigurableJointMotion)0);
			grabJoint.set_zMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularXMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularYMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularZMotion((ConfigurableJointMotion)0);
			((Joint)grabJoint).set_breakForce(20000f);
			((Joint)grabJoint).set_breakTorque(20000f);
			((Joint)grabJoint).set_enablePreprocessing(false);
			grabBody = collisionRigidbody;
			grabManager.ObjectGrabbed(grabObject);
			if (onGrabTap != null)
			{
				onGrabTap(((Component)this).get_gameObject(), ((ContactPoint)(ref contacts[0])).get_point(), collider.get_sharedMaterial(), normalTangentVelocityImpulse);
			}
		}
	}

	private float GetSurfaceAngle(ContactPoint contact, Vector3 direction)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Vector3.Angle(((ContactPoint)(ref contact)).get_normal(), direction);
	}

	private bool SurfaceWithinAngle(ContactPoint contact, Vector3 direction, float angle)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		bool result = false;
		if (Vector3.Angle(((ContactPoint)(ref contact)).get_normal(), direction) <= angle)
		{
			result = true;
		}
		return result;
	}

	public void ReleaseGrab(float blockTime = 0f)
	{
		if ((Object)(object)grabJoint != (Object)null)
		{
			if ((Object)(object)grabObject != (Object)null)
			{
				grabManager.ObjectReleased(grabObject);
			}
			Object.Destroy((Object)(object)grabJoint);
			grabJoint = null;
			grabBody = null;
			grabObject = null;
			if (offGrabTap != null)
			{
				offGrabTap();
			}
		}
		blockGrab = Mathf.Max(blockGrab, blockTime);
	}

	public void BlockGrab(ReleaseGrabTrigger releaseGrabTrigger)
	{
		blockGrabTrigger = releaseGrabTrigger;
		ReleaseGrab();
	}

	public void UnblockBlockGrab()
	{
		blockGrabTrigger = null;
	}

	private void FixedUpdate()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		if (!grab && (Object)(object)grabJoint != (Object)null)
		{
			ReleaseGrab();
		}
		blockGrab -= Time.get_fixedDeltaTime();
		if ((Object)(object)grabFilter == (Object)null || !grab || !((Object)(object)grabJoint == (Object)null) || !(blockGrab <= 0f))
		{
			return;
		}
		Vector3 position = ((Component)this).get_transform().get_position();
		Vector3 val = grabPosition - position;
		float radius = ((Component)this).GetComponent<SphereCollider>().get_radius();
		Collider val2 = grabFilter;
		Transform component = ((Component)val2).GetComponent<Transform>();
		Rigidbody componentInParent = ((Component)val2).GetComponentInParent<Rigidbody>();
		IGrabbable componentInParent2 = ((Component)component).GetComponentInParent<IGrabbable>();
		if (componentInParent2 != null)
		{
			grabObject = ((Component)((componentInParent2 is MonoBehaviour) ? componentInParent2 : null)).get_gameObject();
		}
		else if ((Object)(object)grabBody != (Object)null)
		{
			grabObject = ((Component)grabBody).get_gameObject();
		}
		else
		{
			grabObject = ((Component)val2).get_gameObject();
		}
		if (CheatCodes.climbCheat || human.state != HumanState.Climb || !((Object)(object)otherSide.grabObject == (Object)(object)grabObject) || !(grabPosition.y > ((Component)otherSide).get_transform().get_position().y + handToHandClimb))
		{
			grabJoint = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)grabJoint).set_autoConfigureConnectedAnchor(false);
			((Joint)grabJoint).set_anchor(((Component)this).get_transform().InverseTransformPoint(position + ((Vector3)(ref val)).get_normalized() * radius));
			if (Object.op_Implicit((Object)(object)componentInParent))
			{
				((Joint)grabJoint).set_connectedBody(componentInParent);
				((Joint)grabJoint).set_connectedAnchor(((Component)componentInParent).get_transform().InverseTransformPoint(grabPosition));
			}
			else
			{
				((Joint)grabJoint).set_connectedAnchor(grabPosition);
			}
			grabJoint.set_xMotion((ConfigurableJointMotion)0);
			grabJoint.set_yMotion((ConfigurableJointMotion)0);
			grabJoint.set_zMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularXMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularYMotion((ConfigurableJointMotion)0);
			grabJoint.set_angularZMotion((ConfigurableJointMotion)0);
			((Joint)grabJoint).set_breakForce(20000f);
			((Joint)grabJoint).set_breakTorque(20000f);
			((Joint)grabJoint).set_enablePreprocessing(false);
			grabBody = componentInParent;
			grabManager.ObjectGrabbed(grabObject);
			if (onGrabTap != null)
			{
				onGrabTap(((Component)this).get_gameObject(), grabPosition, val2.get_sharedMaterial(), normalTangentVelocityImpulse);
			}
		}
	}

	private void OnJointBreak(float breakForce)
	{
		if (breakForce >= 20000f)
		{
			Debug.LogError((object)("Joint break force: " + breakForce));
			ReleaseGrab();
		}
	}

	public CollisionSensor()
		: this()
	{
	}
}
