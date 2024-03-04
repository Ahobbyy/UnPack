using System.Collections.Generic;
using UnityEngine;

public class Current : MonoBehaviour
{
	public float radius = 10f;

	public float weight = 1f;

	public float power = 1f;

	public Vector3 flow;

	public Vector3 globalFlow;

	private static List<Current> all = new List<Current>();

	private static Current[] allArray;

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		all.Add(this);
		globalFlow = ((Component)this).get_transform().TransformVector(flow);
		allArray = null;
	}

	private void OnDisable()
	{
		all.Remove(this);
		allArray = null;
	}

	public static Vector3 Sample(Vector3 pos, out float weight)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		if (allArray == null)
		{
			allArray = all.ToArray();
		}
		weight = 0f;
		Vector3 val = Vector3.get_zero();
		for (int i = 0; i < allArray.Length; i++)
		{
			Current current = allArray[i];
			Vector3 val2 = pos - ((Component)current).get_transform().get_position();
			float num = 1f - ((Vector3)(ref val2)).get_magnitude() / current.radius;
			if (!(num < 0f))
			{
				num = Mathf.Pow(num, current.power);
				weight += num;
				val += num * current.globalFlow;
			}
		}
		if (weight > 1f)
		{
			val /= weight;
			weight = Mathf.Clamp01(weight);
		}
		return val;
	}

	public void OnDrawGizmosSelected()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.DrawWireSphere(((Component)this).get_transform().get_position(), radius);
		Gizmos.DrawRay(((Component)this).get_transform().get_position(), ((Component)this).get_transform().TransformVector(flow));
	}

	public Current()
		: this()
	{
	}
}
