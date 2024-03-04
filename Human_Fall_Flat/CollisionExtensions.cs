using HumanAPI;
using UnityEngine;

public static class CollisionExtensions
{
	public static void Analyze(this Collision collision, out Vector3 pos, out float impulse, out float normalVelocity, out float tangentVelocity, out PhysicMaterial mat1, out PhysicMaterial mat2, out int id2, out float volume2, out float pitch2)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		ContactPoint[] contacts = collision.get_contacts();
		id2 = int.MaxValue;
		volume2 = 1f;
		pitch2 = 1f;
		mat1 = (mat2 = null);
		normalVelocity = 0f;
		tangentVelocity = 0f;
		pos = ((ContactPoint)(ref collision.get_contacts()[0])).get_point();
		Collider val = null;
		int num = 0;
		Vector3 relativeVelocity = collision.get_relativeVelocity();
		Vector3 val3;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint val2 = contacts[i];
			float num2 = Vector3.Dot(((ContactPoint)(ref val2)).get_normal(), relativeVelocity);
			val3 = relativeVelocity - ((ContactPoint)(ref val2)).get_normal() * num2;
			float magnitude = ((Vector3)(ref val3)).get_magnitude();
			if (num2 > normalVelocity)
			{
				normalVelocity = num2;
				tangentVelocity = magnitude;
				pos = ((ContactPoint)(ref val2)).get_point();
				mat1 = ((ContactPoint)(ref val2)).get_thisCollider().get_sharedMaterial();
				mat2 = ((ContactPoint)(ref val2)).get_otherCollider().get_sharedMaterial();
				val = ((ContactPoint)(ref val2)).get_otherCollider();
			}
			num++;
		}
		val3 = collision.get_impulse();
		impulse = ((Vector3)(ref val3)).get_magnitude();
		if ((Object)(object)val != (Object)null)
		{
			CollisionAudioSensor componentInParent = ((Component)val).GetComponentInParent<CollisionAudioSensor>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				id2 = componentInParent.id;
				volume2 = componentInParent.volume;
				pitch2 = componentInParent.pitch;
			}
		}
	}

	public static Vector3 GetNormalTangentVelocitiesAndImpulse(this Collision collision, Rigidbody thisBody)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		ContactPoint[] contacts = collision.get_contacts();
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		Vector3 relativeVelocity = collision.get_relativeVelocity();
		Vector3 val2;
		for (int i = 0; i < contacts.Length; i++)
		{
			ContactPoint val = contacts[i];
			float num4 = Vector3.Dot(((ContactPoint)(ref val)).get_normal(), relativeVelocity);
			val2 = relativeVelocity - ((ContactPoint)(ref val)).get_normal() * num4;
			float magnitude = ((Vector3)(ref val2)).get_magnitude();
			if (num4 > num)
			{
				num = num4;
				num2 = magnitude;
			}
			num3++;
		}
		float num5 = num;
		float num6 = num2;
		val2 = collision.get_impulse();
		return new Vector3(num5, num6, ((Vector3)(ref val2)).get_magnitude());
	}

	public static Vector3 GetPoint(this Collision collision)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return ((ContactPoint)(ref collision.get_contacts()[0])).get_point();
	}

	public static Vector3 GetImpulse(this Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 impulse = collision.get_impulse();
		if (Vector3.Dot(impulse, ((ContactPoint)(ref collision.get_contacts()[0])).get_normal()) < 0f)
		{
			return -impulse;
		}
		return impulse;
	}
}
