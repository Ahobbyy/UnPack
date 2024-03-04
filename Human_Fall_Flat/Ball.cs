using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	public LayerMask collisionLayers;

	private GrabManager grabManager;

	private Human human;

	private Ragdoll ragdoll;

	private float ballRadius;

	private List<Collision> collisions = new List<Collision>();

	private List<Vector3> contacts = new List<Vector3>();

	public Vector3 lastImpulse;

	public Vector3 lastNonZeroImpulse;

	public float timeSinceLastNonzeroImpulse;

	private void OnEnable()
	{
		human = ((Component)this).GetComponent<Human>();
		ragdoll = ((Component)this).GetComponent<Ragdoll>();
		ballRadius = ((Component)this).GetComponent<SphereCollider>().get_radius();
		grabManager = ((Component)this).GetComponent<GrabManager>();
	}

	private void FixedUpdate()
	{
		ImpulseForce2();
		collisions.Clear();
		contacts.Clear();
	}

	private void ImpulseForce2()
	{
	}

	public void OnCollisionEnter(Collision collision)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			HandleCollision(collision);
			collisions.Add(collision);
			for (int i = 0; i < collision.get_contacts().Length; i++)
			{
				contacts.Add(((ContactPoint)(ref collision.get_contacts()[i])).get_point());
			}
		}
	}

	public void OnCollisionStay(Collision collision)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			HandleCollision(collision);
			collisions.Add(collision);
			for (int i = 0; i < collision.get_contacts().Length; i++)
			{
				contacts.Add(((ContactPoint)(ref collision.get_contacts()[i])).get_point());
			}
		}
	}

	private void HandleCollision(Collision collision)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		Vector3 impulse = collision.GetImpulse();
		if (impulse.y > 0f && human.onGround)
		{
			timeSinceLastNonzeroImpulse = Time.get_time();
		}
		Vector3 walkDirection = human.controls.walkDirection;
		Debug.DrawRay(((ContactPoint)(ref collision.get_contacts()[0])).get_point(), impulse / 10f, Color.get_red(), 0.5f);
		if (Vector3.Dot(impulse, walkDirection) >= 0f)
		{
			return;
		}
		float num = 0f;
		RaycastHit val3 = default(RaycastHit);
		for (int i = 0; i < collision.get_contacts().Length; i++)
		{
			Vector3 point = ((ContactPoint)(ref collision.get_contacts()[i])).get_point();
			Vector3 val = point + walkDirection * 0.07f + Vector3.get_up() * 0.07f;
			Vector3 val2 = point - walkDirection * 0.07f - Vector3.get_up() * 0.07f;
			Debug.DrawRay(val, Vector3.get_down() * 0.1f, Color.get_blue());
			if (Physics.Raycast(val, Vector3.get_down(), ref val3, 0.1f, LayerMask.op_Implicit(collisionLayers)))
			{
				Debug.DrawRay(((RaycastHit)(ref val3)).get_point(), ((RaycastHit)(ref val3)).get_normal(), Color.get_red());
			}
			Debug.DrawRay(val2, walkDirection * 0.1f, Color.get_blue());
			if (Physics.Raycast(val2, walkDirection, ref val3, 0.1f, LayerMask.op_Implicit(collisionLayers)))
			{
				Debug.DrawRay(((RaycastHit)(ref val3)).get_point(), ((RaycastHit)(ref val3)).get_normal(), Color.get_red());
			}
			if (Physics.Raycast(val, Vector3.get_down(), ref val3, 0.1f, LayerMask.op_Implicit(collisionLayers)) && ((RaycastHit)(ref val3)).get_normal().y > 0.7f && Physics.Raycast(val2, walkDirection, ref val3, 0.1f, LayerMask.op_Implicit(collisionLayers)) && ((RaycastHit)(ref val3)).get_normal().y < 0.4f)
			{
				Debug.DrawLine(((Component)this).get_transform().get_position(), ((ContactPoint)(ref collision.get_contacts()[i])).get_point(), Color.get_red());
				num = 1.5f;
				break;
			}
		}
		if ((Object)(object)human.ragdoll.partLeftHand.sensor.grabJoint != (Object)null && (Object)(object)human.ragdoll.partRightHand.sensor.grabJoint != (Object)null)
		{
			float num2 = (((Object)(object)human.ragdoll.partLeftHand.sensor.grabJoint != (Object)null) ? Vector3.Dot(human.ragdoll.partLeftHand.transform.get_position() - ((Component)this).get_transform().get_position(), walkDirection) : 0f);
			float num3 = (((Object)(object)human.ragdoll.partRightHand.sensor.grabJoint != (Object)null) ? Vector3.Dot(human.ragdoll.partRightHand.transform.get_position() - ((Component)this).get_transform().get_position(), walkDirection) : 0f);
			num = Mathf.Max(num, (num2 + num3) / 2f);
		}
		if (num > 0f)
		{
			Vector3 val4 = impulse.ZeroY();
			impulse = Vector3.get_up() * ((Vector3)(ref val4)).get_magnitude() * num - val4 / 2f;
			human.ragdoll.partBall.rigidbody.SafeAddForce(impulse, (ForceMode)1);
			human.groundManager.DistributeForce(-impulse / Time.get_fixedDeltaTime(), ((Component)this).get_transform().get_position());
		}
	}

	public Ball()
		: this()
	{
	}
}
