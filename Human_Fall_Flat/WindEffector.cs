using HumanAPI;
using UnityEngine;

public class WindEffector : Node
{
	public NodeInput input;

	public Vector3 wind;

	public float maxDist = 5f;

	public float distPower = 0.7f;

	public bool respectArea;

	public float coefDrag = 1f;

	public float cDamp = 0.5f;

	public float humanFlyForce = 100f;

	public float centerBend;

	public bool applyAcceleration;

	private Vector3 worldWind;

	private float D;

	private Vector3 normal;

	private float storedValue;

	public Rigidbody ignoreFan;

	public Transform[] ignoreParents;

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
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		base.Process();
		worldWind = ((Component)this).get_transform().TransformVector(wind) * input.value;
		normal = ((Vector3)(ref worldWind)).get_normalized();
		D = Vector3.Dot(((Component)this).get_transform().get_position(), normal);
		storedValue = input.value;
	}

	public void OnTriggerEnter(Collider other)
	{
		OnTriggerStay(other);
	}

	public void OnTriggerStay(Collider other)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody componentInParent = ((Component)other).GetComponentInParent<Rigidbody>();
		if ((Object)(object)componentInParent == (Object)null || (Object)(object)componentInParent == (Object)(object)ignoreFan || componentInParent.get_isKinematic())
		{
			return;
		}
		worldWind = ((Component)this).get_transform().TransformVector(wind) * input.value;
		normal = ((Vector3)(ref worldWind)).get_normalized();
		D = Vector3.Dot(((Component)this).get_transform().get_position(), normal);
		if (!(worldWind == Vector3.get_zero()))
		{
			float num = Vector3.Dot(componentInParent.get_worldCenterOfMass(), normal) - D;
			float num2 = num / (maxDist * Mathf.Abs(storedValue));
			num2 = ((!(num2 > 0f)) ? Mathf.Clamp01(Mathf.Pow(Mathf.Clamp01(1f + num2), distPower)) : Mathf.Clamp01(Mathf.Pow(Mathf.Clamp01(1f - num2), distPower)));
			Vector3 val2;
			if (centerBend > 0f)
			{
				Vector3 val = componentInParent.get_worldCenterOfMass() - num * normal - ((Component)this).get_transform().get_position();
				val2 = normal - val / centerBend * num2;
				normal = ((Vector3)(ref val2)).get_normalized();
				worldWind = normal * ((Vector3)(ref worldWind)).get_magnitude();
			}
			float num3 = Vector3.Dot(componentInParent.get_velocity(), normal);
			float num4 = ((Vector3)(ref worldWind)).get_magnitude() * num2 - num3;
			float num5 = coefDrag * Mathf.Sign(num4) * num4 * num4;
			float num6 = (0f - num3) * componentInParent.get_mass() * cDamp / Time.get_fixedDeltaTime();
			Human component = ((Component)componentInParent).GetComponent<Human>();
			if (applyAcceleration)
			{
				num5 = ((!((Object)(object)component != (Object)null)) ? (num5 * componentInParent.get_mass()) : (num5 * (component.mass / (float)component.rigidbodies.Length)));
			}
			if (respectArea)
			{
				Vector3 val3 = normal;
				float num7 = num5;
				Bounds bounds = other.get_bounds();
				val2 = ((Bounds)(ref bounds)).get_size();
				componentInParent.SafeAddForce(val3 * (num7 * ((Vector3)(ref val2)).get_sqrMagnitude() + num6), (ForceMode)0);
			}
			else
			{
				componentInParent.AddForce(normal * (num5 + num6));
			}
			Debug.DrawRay(componentInParent.get_worldCenterOfMass(), normal * (num5 + num6) / 500f, Color.get_cyan(), 0.3f);
			if ((Object)(object)component != (Object)null)
			{
				componentInParent.AddForce(component.controls.walkDirection * humanFlyForce);
			}
		}
	}
}
