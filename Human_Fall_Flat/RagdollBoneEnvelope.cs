using System;
using UnityEngine;

public class RagdollBoneEnvelope : MonoBehaviour
{
	public float innerThickness;

	public float outerThickness;

	public float power;

	public float innerWeight;

	public float outerWeight;

	public Vector3 centerOffset;

	public float lengthMultiplier;

	[NonSerialized]
	public Vector3 start;

	[NonSerialized]
	public Vector3 direction;

	[NonSerialized]
	public float height;

	[NonSerialized]
	public float radius;

	public float innerRadius
	{
		get
		{
			float num = radius - ((innerThickness != 0f) ? innerThickness : 0.125f);
			if (!(num > 0f))
			{
				return 0f;
			}
			return num;
		}
	}

	public float outerRadius => radius + ((outerThickness != 0f) ? outerThickness : 0.125f);

	public void ReadCollider()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		Collider component = ((Component)this).GetComponent<Collider>();
		if (component is SphereCollider)
		{
			SphereCollider val = (SphereCollider)(object)((component is SphereCollider) ? component : null);
			start = val.get_center() + centerOffset;
			direction = Vector3.get_zero();
			height = 0f;
			radius = val.get_radius();
		}
		else if (component is CapsuleCollider)
		{
			CapsuleCollider val2 = (CapsuleCollider)(object)((component is CapsuleCollider) ? component : null);
			direction = new Vector3(1f, 0f, 0f);
			if (val2.get_direction() == 1)
			{
				direction = new Vector3(0f, 1f, 0f);
			}
			else if (val2.get_direction() == 2)
			{
				direction = new Vector3(0f, 0f, 1f);
			}
			height = Math.Max(0f, val2.get_height() - 2f * val2.get_radius());
			if (lengthMultiplier != 0f)
			{
				height *= lengthMultiplier;
			}
			start = val2.get_center() - direction * height / 2f + centerOffset;
			radius = val2.get_radius();
		}
	}

	public RagdollBoneEnvelope()
		: this()
	{
	}
}
