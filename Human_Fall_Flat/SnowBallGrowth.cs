using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class SnowBallGrowth : Node, IPostEndReset, IPreReset
{
	public float growthMultiplier = 1f;

	public NodeInput melt;

	public float meltSpeed = 0.1f;

	private Rigidbody rb;

	private Quaternion lastRot;

	[SerializeField]
	private float minimumScale = 0.25f;

	[SerializeField]
	private float radius = 0.45f;

	[SerializeField]
	private float density = 35f;

	[SerializeField]
	private Transform ignoreHat;

	private const string kSnowMaterialIndicator = "Snow";

	private Dictionary<Material, bool> snowyMaterials = new Dictionary<Material, bool>();

	private void Awake()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (NetGame.isClient)
		{
			return;
		}
		Quaternion rotation = ((Component)this).get_transform().get_rotation();
		float num = Quaternion.Angle(lastRot, rotation);
		lastRot = rotation;
		float num2 = 0f;
		RaycastHit val = default(RaycastHit);
		if (((Object)(object)InsideIceCaves.instance == (Object)null || !InsideIceCaves.instance.SnowballInsideCave(this)) && Physics.Raycast(((Component)this).get_transform().get_position(), Vector3.get_down(), ref val, ((Component)this).get_transform().get_lossyScale().x / 2f + 0.2f, 1))
		{
			MeshRenderer component = ((Component)((RaycastHit)(ref val)).get_collider()).get_gameObject().GetComponent<MeshRenderer>();
			bool value = false;
			if ((Object)(object)component != (Object)null)
			{
				Material[] sharedMaterials = ((Renderer)component).get_sharedMaterials();
				foreach (Material val2 in sharedMaterials)
				{
					if (snowyMaterials.TryGetValue(val2, out value))
					{
						break;
					}
					value = ((Object)val2).get_name().Contains("Snow");
					snowyMaterials.Add(val2, value);
				}
			}
			if (value)
			{
				num2 = Mathf.Abs(num) * growthMultiplier / 360f;
			}
		}
		num2 -= melt.value * meltSpeed * Time.get_fixedDeltaTime();
		if (num2 != 0f)
		{
			UpdateSize(num2);
		}
	}

	private void UpdateSize(float growth)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).get_transform().get_localScale() + new Vector3(growth, growth, growth);
		if (val.x > minimumScale && val.y > minimumScale && val.z > minimumScale)
		{
			((Component)this).get_transform().set_localScale(val);
			rb.SetDensity(density);
			rb.set_mass(rb.get_mass());
			AdjustHandAnchors(growth);
		}
	}

	public void AdjustHandAnchors(float growth)
	{
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			AdjustHandAnchors(human.ragdoll.partLeftHand.sensor.grabJoint, growth);
			AdjustHandAnchors(human.ragdoll.partRightHand.sensor.grabJoint, growth);
		}
	}

	public void AdjustHandAnchors(ConfigurableJoint joint, float growth)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)joint == (Object)null) && !((Object)(object)((Joint)joint).get_connectedBody() != (Object)(object)rb))
		{
			Vector3 val = ((Component)((Joint)joint).get_connectedBody()).get_transform().TransformVector(((Joint)joint).get_connectedAnchor());
			((Joint)joint).set_autoConfigureConnectedAnchor(false);
			val *= ((Component)((Joint)joint).get_connectedBody()).get_transform().get_localScale().x * radius / ((Vector3)(ref val)).get_magnitude();
			((Joint)joint).set_connectedAnchor(((Component)((Joint)joint).get_connectedBody()).get_transform().InverseTransformVector(val));
		}
	}

	void IPostEndReset.PostEndResetState(int checkpoint)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		lastRot = ((Component)this).get_transform().get_rotation();
	}

	private void OnJointBreak(float breakForce)
	{
		IgnoreCollision.Unignore(((Component)this).get_transform(), ignoreHat);
	}

	void IPreReset.PreResetState(int checkpoint)
	{
		IgnoreCollision.Ignore(((Component)this).get_transform(), ignoreHat);
	}
}
