using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class BendableSegment : MonoBehaviour
	{
		public Bendable bandable;

		public int index;

		public float treshold = 100f;

		public float clampImpact = 130f;

		public int windowFrames = 20;

		public float maxTotal;

		public float maxImpactThisFrame;

		public Vector3 maxImpactVector;

		public float lastImpact;

		public float total;

		private List<float> impacts = new List<float>();

		private Vector3 point;

		public void OnCollisionEnter(Collision collision)
		{
			if (collision.get_gameObject().get_layer() != LayerMask.NameToLayer("Player"))
			{
				HandleCollision(collision);
			}
		}

		public void OnCollisionStay(Collision collision)
		{
			if (collision.get_gameObject().get_layer() != LayerMask.NameToLayer("Player"))
			{
				HandleCollision(collision);
			}
		}

		private void FixedUpdate()
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Clamp(Mathf.Min(lastImpact, maxImpactThisFrame), 0f, clampImpact);
			lastImpact = maxImpactThisFrame;
			impacts.Add(num);
			total += num;
			if (total > treshold)
			{
				bandable.ReportBend(index, maxImpactVector);
			}
			if (total > maxTotal)
			{
				maxTotal = total;
			}
			if (impacts.Count > 20)
			{
				total -= impacts[0];
				impacts.RemoveAt(0);
			}
			maxImpactThisFrame = 0f;
			maxImpactVector = Vector3.get_zero();
		}

		private void HandleCollision(Collision collision)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			if (collision.get_contacts().Length != 0)
			{
				point = collision.GetPoint();
				Vector3 val = collision.GetImpulse();
				BendableTool component = ((Component)collision.get_rigidbody()).GetComponent<BendableTool>();
				if ((Object)(object)component != (Object)null)
				{
					val *= component.forceMultiplier;
				}
				float magnitude = ((Vector3)(ref val)).get_magnitude();
				if (magnitude > maxImpactThisFrame)
				{
					maxImpactThisFrame = magnitude;
					maxImpactVector = val;
				}
			}
		}

		public void OnDrawGizmosSelected()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Gizmos.DrawRay(point, maxImpactVector * 100f);
		}

		public BendableSegment()
			: this()
		{
		}
	}
}
