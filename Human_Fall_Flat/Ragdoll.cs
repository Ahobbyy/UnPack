using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
	public HumanSegment partHead;

	public HumanSegment partChest;

	public HumanSegment partWaist;

	public HumanSegment partHips;

	public HumanSegment partLeftArm;

	public HumanSegment partLeftForearm;

	public HumanSegment partLeftHand;

	public HumanSegment partLeftThigh;

	public HumanSegment partLeftLeg;

	public HumanSegment partLeftFoot;

	public HumanSegment partRightArm;

	public HumanSegment partRightForearm;

	public HumanSegment partRightHand;

	public HumanSegment partRightThigh;

	public HumanSegment partRightLeg;

	public HumanSegment partRightFoot;

	public HumanSegment partBall;

	public Transform[] bones;

	public float handLength;

	private bool initialized;

	private void Awake()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (!initialized)
		{
			initialized = true;
			CollectSegments();
			SetupColliders();
			Vector3 val = partLeftArm.transform.get_position() - partLeftForearm.transform.get_position();
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			val = partLeftForearm.transform.get_position() - partLeftHand.transform.get_position();
			handLength = magnitude + ((Vector3)(ref val)).get_magnitude();
		}
	}

	private void CollectSegments()
	{
		Dictionary<string, Transform> dictionary = new Dictionary<string, Transform>();
		Transform[] componentsInChildren = ((Component)this).GetComponentsInChildren<Transform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			dictionary[((Object)componentsInChildren[i]).get_name().ToLower()] = componentsInChildren[i];
		}
		partHead = FindSegment(dictionary, "head");
		partChest = FindSegment(dictionary, "chest");
		partWaist = FindSegment(dictionary, "waist");
		partHips = FindSegment(dictionary, "hips");
		partLeftArm = FindSegment(dictionary, "leftArm");
		partLeftForearm = FindSegment(dictionary, "leftForearm");
		partLeftHand = FindSegment(dictionary, "leftHand");
		partLeftThigh = FindSegment(dictionary, "leftThigh");
		partLeftLeg = FindSegment(dictionary, "leftLeg");
		partLeftFoot = FindSegment(dictionary, "leftFoot");
		partRightArm = FindSegment(dictionary, "rightArm");
		partRightForearm = FindSegment(dictionary, "rightForearm");
		partRightHand = FindSegment(dictionary, "rightHand");
		partRightThigh = FindSegment(dictionary, "rightThigh");
		partRightLeg = FindSegment(dictionary, "rightLeg");
		partRightFoot = FindSegment(dictionary, "rightFoot");
		SetupHeadComponents(partHead);
		SetupBodyComponents(partChest);
		SetupBodyComponents(partWaist);
		SetupBodyComponents(partHips);
		SetupLimbComponents(partLeftArm);
		SetupLimbComponents(partLeftForearm);
		SetupLimbComponents(partLeftThigh);
		SetupLimbComponents(partLeftLeg);
		SetupLimbComponents(partRightArm);
		SetupLimbComponents(partRightForearm);
		SetupLimbComponents(partRightThigh);
		SetupLimbComponents(partRightLeg);
		SetupFootComponents(partLeftFoot);
		SetupFootComponents(partRightFoot);
		SetupHandComponents(partLeftHand);
		SetupHandComponents(partRightHand);
		partLeftHand.sensor.otherSide = partRightHand.sensor;
		partRightHand.sensor.otherSide = partLeftHand.sensor;
		AddAntistretch(partLeftHand, partChest);
		AddAntistretch(partRightHand, partChest);
		AddAntistretch(partLeftFoot, partHips);
		AddAntistretch(partRightFoot, partHips);
	}

	private void AddAntistretch(HumanSegment seg1, HumanSegment seg2)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		ConfigurableJoint obj = ((Component)seg1.rigidbody).get_gameObject().AddComponent<ConfigurableJoint>();
		ConfigurableJointMotion val = (ConfigurableJointMotion)1;
		obj.set_zMotion((ConfigurableJointMotion)1);
		ConfigurableJointMotion xMotion;
		obj.set_yMotion(xMotion = val);
		obj.set_xMotion(xMotion);
		SoftJointLimit linearLimit = default(SoftJointLimit);
		Vector3 val2 = seg1.transform.get_position() - seg2.transform.get_position();
		((SoftJointLimit)(ref linearLimit)).set_limit(((Vector3)(ref val2)).get_magnitude());
		obj.set_linearLimit(linearLimit);
		((Joint)obj).set_autoConfigureConnectedAnchor(false);
		((Joint)obj).set_anchor(Vector3.get_zero());
		((Joint)obj).set_connectedBody(seg2.rigidbody);
		((Joint)obj).set_connectedAnchor(Vector3.get_zero());
	}

	public void BindBall(Transform ballTransform)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		partBall = BindSegmanet(ballTransform);
		SpringJoint component = ((Component)partBall.rigidbody).GetComponent<SpringJoint>();
		((Joint)component).set_autoConfigureConnectedAnchor(false);
		_003F val = component;
		_003F val2 = partHips.transform;
		Vector3 position = ((Component)this).get_transform().get_position();
		Vector3 up = Vector3.get_up();
		Collider collider = partBall.collider;
		((Joint)val).set_connectedAnchor(((Transform)val2).InverseTransformPoint(position + up * (((SphereCollider)((collider is SphereCollider) ? collider : null)).get_radius() + component.get_maxDistance())));
		((Joint)component).set_connectedBody(partHips.rigidbody);
	}

	private void SetupHeadComponents(HumanSegment part)
	{
		part.sensor = ((Component)part.transform).get_gameObject().AddComponent<CollisionSensor>();
		((Component)part.transform).get_gameObject().AddComponent<CollisionAudioSensor>();
		part.sensor.knockdown = 2f;
	}

	private void SetupBodyComponents(HumanSegment part)
	{
		part.sensor = ((Component)part.transform).get_gameObject().AddComponent<CollisionSensor>();
		((Component)part.transform).get_gameObject().AddComponent<CollisionAudioSensor>();
		part.sensor.knockdown = 1f;
	}

	private void SetupLimbComponents(HumanSegment part)
	{
		part.sensor = ((Component)part.transform).get_gameObject().AddComponent<CollisionSensor>();
	}

	private void SetupHandComponents(HumanSegment part)
	{
		part.sensor = ((Component)part.transform).get_gameObject().AddComponent<CollisionSensor>();
		((Component)part.transform).get_gameObject().AddComponent<CollisionAudioSensor>();
	}

	private void SetupFootComponents(HumanSegment part)
	{
		part.sensor = ((Component)part.transform).get_gameObject().AddComponent<CollisionSensor>();
		((Component)part.transform).get_gameObject().AddComponent<FootCollisionAudioSensor>();
		part.sensor.groundCheck = true;
	}

	private void SetupColliders()
	{
		Physics.IgnoreCollision(partChest.collider, partHead.collider);
		Physics.IgnoreCollision(partChest.collider, partLeftArm.collider);
		Physics.IgnoreCollision(partChest.collider, partLeftForearm.collider);
		Physics.IgnoreCollision(partChest.collider, partRightArm.collider);
		Physics.IgnoreCollision(partChest.collider, partRightForearm.collider);
		Physics.IgnoreCollision(partChest.collider, partWaist.collider);
		Physics.IgnoreCollision(partHips.collider, partChest.collider);
		Physics.IgnoreCollision(partHips.collider, partWaist.collider);
		Physics.IgnoreCollision(partHips.collider, partLeftThigh.collider);
		Physics.IgnoreCollision(partHips.collider, partLeftLeg.collider);
		Physics.IgnoreCollision(partHips.collider, partLeftFoot.collider);
		Physics.IgnoreCollision(partHips.collider, partRightThigh.collider);
		Physics.IgnoreCollision(partHips.collider, partRightLeg.collider);
		Physics.IgnoreCollision(partHips.collider, partRightFoot.collider);
		Physics.IgnoreCollision(partLeftForearm.collider, partLeftArm.collider);
		Physics.IgnoreCollision(partLeftForearm.collider, partLeftHand.collider);
		Physics.IgnoreCollision(partLeftArm.collider, partLeftHand.collider);
		Physics.IgnoreCollision(partRightForearm.collider, partRightArm.collider);
		Physics.IgnoreCollision(partRightForearm.collider, partRightHand.collider);
		Physics.IgnoreCollision(partRightArm.collider, partRightHand.collider);
		Physics.IgnoreCollision(partLeftThigh.collider, partLeftLeg.collider);
		Physics.IgnoreCollision(partRightThigh.collider, partRightLeg.collider);
	}

	private HumanSegment FindSegment(Dictionary<string, Transform> children, string name)
	{
		return BindSegmanet(children[name.ToLower()]);
	}

	private HumanSegment BindSegmanet(Transform transform)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		HumanSegment obj = new HumanSegment
		{
			transform = transform
		};
		obj.collider = ((Component)obj.transform).GetComponent<Collider>();
		obj.rigidbody = ((Component)obj.transform).GetComponent<Rigidbody>();
		obj.startupRotation = obj.transform.get_localRotation();
		obj.sensor = ((Component)obj.transform).GetComponent<CollisionSensor>();
		obj.bindPose = obj.transform.get_worldToLocalMatrix() * ((Component)this).get_transform().get_localToWorldMatrix();
		return obj;
	}

	internal void StretchHandsLegs(Vector3 direction, Vector3 right, int force)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		partLeftHand.rigidbody.SafeAddForce((direction - right) * (float)force / 2f, (ForceMode)0);
		partRightHand.rigidbody.SafeAddForce((direction + right) * (float)force / 2f, (ForceMode)0);
		partLeftFoot.rigidbody.SafeAddForce((-direction - right) * (float)force / 2f, (ForceMode)0);
		partRightFoot.rigidbody.SafeAddForce((-direction + right) * (float)force / 2f, (ForceMode)0);
		partLeftForearm.rigidbody.SafeAddForce((direction - right) * (float)force / 2f, (ForceMode)0);
		partRightForearm.rigidbody.SafeAddForce((direction + right) * (float)force / 2f, (ForceMode)0);
		partLeftLeg.rigidbody.SafeAddForce((-direction - right) * (float)force / 2f, (ForceMode)0);
		partRightLeg.rigidbody.SafeAddForce((-direction + right) * (float)force / 2f, (ForceMode)0);
	}

	public void AllowHandBallRotation(bool allow)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		ConfigurableJointMotion val = (ConfigurableJointMotion)(allow ? 2 : 0);
		ConfigurableJoint component = ((Component)partLeftHand.rigidbody).GetComponent<ConfigurableJoint>();
		component.set_angularXMotion(val);
		component.set_angularYMotion(val);
		component.set_angularZMotion(val);
		ConfigurableJoint component2 = ((Component)partRightHand.rigidbody).GetComponent<ConfigurableJoint>();
		component2.set_angularXMotion(val);
		component2.set_angularYMotion(val);
		component2.set_angularZMotion(val);
	}

	public void Lock()
	{
		Rigidbody[] componentsInChildren = ((Component)this).GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].set_isKinematic(true);
		}
	}

	public void ToggleHeavyArms(bool left, bool right)
	{
		if (left)
		{
			Rigidbody rigidbody = partLeftHand.rigidbody;
			Rigidbody rigidbody2 = partLeftForearm.rigidbody;
			float num;
			partLeftArm.rigidbody.set_mass(num = 20f);
			float mass;
			rigidbody2.set_mass(mass = num);
			rigidbody.set_mass(mass);
		}
		if (right)
		{
			Rigidbody rigidbody3 = partRightHand.rigidbody;
			Rigidbody rigidbody4 = partRightForearm.rigidbody;
			float num;
			partRightArm.rigidbody.set_mass(num = 20f);
			float mass;
			rigidbody4.set_mass(mass = num);
			rigidbody3.set_mass(mass);
		}
	}

	public void ReleaseHeavyArms()
	{
		Rigidbody rigidbody = partLeftHand.rigidbody;
		Rigidbody rigidbody2 = partLeftForearm.rigidbody;
		float num;
		partLeftArm.rigidbody.set_mass(num = 5f);
		float mass;
		rigidbody2.set_mass(mass = num);
		rigidbody.set_mass(mass);
		Rigidbody rigidbody3 = partRightHand.rigidbody;
		Rigidbody rigidbody4 = partRightForearm.rigidbody;
		partRightArm.rigidbody.set_mass(num = 5f);
		rigidbody4.set_mass(mass = num);
		rigidbody3.set_mass(mass);
	}

	public Ragdoll()
		: this()
	{
	}
}
