using UnityEngine;

public class VoronoiShatterLinked : MonoBehaviour
{
	public VoronoiShatter toShatter;

	public float impulseTreshold = 10f;

	public float breakTreshold = 100f;

	public float humanHardness;

	public float maxMomentum;

	private Vector3 adjustedImpulse = Vector3.get_zero();

	private Vector3 lastFrameImpact;

	private Vector3 maxImpactPoint;

	private float maxImpact;

	public void OnCollisionEnter(Collision collision)
	{
		if (!((Object)(object)toShatter == (Object)null) && !toShatter.shattered)
		{
			HandleCollision(collision, enter: false);
		}
	}

	public void OnCollisionStay(Collision collision)
	{
		if (!((Object)(object)toShatter == (Object)null) && !toShatter.shattered)
		{
			HandleCollision(collision, enter: false);
		}
	}

	private void HandleCollision(Collision collision, bool enter)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length == 0)
		{
			return;
		}
		Vector3 val = -collision.GetImpulse();
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		if (magnitude < impulseTreshold)
		{
			return;
		}
		float num = 1f;
		Transform val2 = collision.get_transform();
		while ((Object)(object)val2 != (Object)null)
		{
			if (Object.op_Implicit((Object)(object)((Component)val2).GetComponent<Human>()))
			{
				num = humanHardness;
				break;
			}
			ShatterHardness component = ((Component)val2).GetComponent<ShatterHardness>();
			if ((Object)(object)component != (Object)null)
			{
				num = component.hardness;
				break;
			}
			val2 = val2.get_parent();
		}
		if (magnitude * num > maxImpact)
		{
			maxImpact = magnitude * num;
			maxImpactPoint = ((ContactPoint)(ref collision.get_contacts()[0])).get_point();
		}
		adjustedImpulse += val * num;
	}

	private void FixedUpdate()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)toShatter == (Object)null) && !toShatter.shattered)
		{
			if (((Vector3)(ref adjustedImpulse)).get_magnitude() > maxMomentum)
			{
				maxMomentum = ((Vector3)(ref adjustedImpulse)).get_magnitude();
			}
			maxImpact = 0f;
			Vector3 val = adjustedImpulse + lastFrameImpact;
			if (((Vector3)(ref val)).get_magnitude() > breakTreshold)
			{
				toShatter.ShatterLocal(maxImpactPoint, adjustedImpulse + lastFrameImpact);
			}
			lastFrameImpact = adjustedImpulse;
			adjustedImpulse = Vector3.get_zero();
		}
	}

	public VoronoiShatterLinked()
		: this()
	{
	}//IL_0017: Unknown result type (might be due to invalid IL or missing references)
	//IL_001c: Unknown result type (might be due to invalid IL or missing references)

}
