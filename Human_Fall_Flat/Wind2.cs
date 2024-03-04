using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class Wind2 : Node
{
	public NodeInput input;

	public Vector3 wind;

	public float cDrag = 1f;

	public float humanFlyForce = 100f;

	private Vector3 worldWind;

	private float D;

	private Vector3 normal;

	private float storedValue;

	public Transform[] ignoreParents;

	private List<Rigidbody> bodiesAffected = new List<Rigidbody>();

	public float maxAcceleration = 20f;

	public float centeringSpring = 50f;

	public float centeringDamper = 10f;

	public float coreRadius = 1f;

	public float radialFalloff = 1f;

	public float dist = 5f;

	public float distFalloff = 5f;

	protected override void OnEnable()
	{
		base.OnEnable();
		Collider component = ((Component)this).GetComponent<Collider>();
		for (int i = 0; i < ignoreParents.Length; i++)
		{
			Collider[] componentsInChildren = ((Component)ignoreParents[i]).GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				Physics.IgnoreCollision(component, componentsInChildren[j]);
			}
		}
	}

	public override void Process()
	{
		base.Process();
		storedValue = input.value;
	}

	public void OnTriggerEnter(Collider other)
	{
		OnTriggerStay(other);
	}

	public void OnTriggerStay(Collider other)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody componentInParent = ((Component)other).GetComponentInParent<Rigidbody>();
		if ((Object)(object)componentInParent == (Object)null || componentInParent.get_isKinematic() || bodiesAffected.Contains(componentInParent))
		{
			return;
		}
		bodiesAffected.Add(componentInParent);
		worldWind = ((Component)this).get_transform().TransformVector(wind) * input.value;
		normal = ((Vector3)(ref worldWind)).get_normalized();
		D = Vector3.Dot(((Component)this).get_transform().get_position(), normal);
		if (!(worldWind == Vector3.get_zero()))
		{
			float num = Vector3.Dot(componentInParent.get_worldCenterOfMass(), normal) - D;
			Vector3 val = componentInParent.get_worldCenterOfMass() - num * normal - ((Component)this).get_transform().get_position();
			float num2 = Mathf.InverseLerp(coreRadius + radialFalloff, coreRadius, ((Vector3)(ref val)).get_magnitude());
			float num3 = Mathf.InverseLerp(dist + distFalloff, dist, num);
			Human component = ((Component)componentInParent).GetComponent<Human>();
			float num4 = componentInParent.get_mass();
			if ((Object)(object)component != (Object)null)
			{
				num4 = component.mass / (float)component.rigidbodies.Length;
			}
			float num5 = num4;
			Vector3 val2 = worldWind - componentInParent.get_velocity();
			Vector3 val3 = ((Vector3)(ref val2)).get_magnitude() * (worldWind - componentInParent.get_velocity()) * cDrag * num5;
			if (((Vector3)(ref val)).get_magnitude() > 0.1f)
			{
				val3 += (0f - componentInParent.get_mass()) * val * centeringSpring;
				val3 += componentInParent.get_mass() * Vector3.Project(-componentInParent.get_velocity(), val) * centeringDamper;
			}
			val3 *= num2 * num3;
			val3 = Vector3.ClampMagnitude(val3, num4 * maxAcceleration);
			if (((Vector3)(ref val3)).get_magnitude() / num4 > 10f && GrabManager.IsGrabbedAny(((Component)componentInParent).get_gameObject()))
			{
				GrabManager.Release(((Component)componentInParent).get_gameObject(), 0.2f);
			}
			componentInParent.AddForce(val3);
			Debug.DrawRay(componentInParent.get_worldCenterOfMass(), val3 / 10f / componentInParent.get_mass(), Color.get_cyan(), 0.1f);
			if ((Object)(object)component != (Object)null)
			{
				componentInParent.AddForce(component.controls.walkDirection * humanFlyForce);
			}
		}
	}

	private void FixedUpdate()
	{
		bodiesAffected.Clear();
	}
}
