using System.Collections;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(Rigidbody))]
	public class PoweredElevator : Node
	{
		public NodeInput input;

		public NodeOutput output;

		public NodeOutput invertedOutput;

		public float speed = 1f;

		public float unitsToMove = 5f;

		public float holdTime = 2f;

		private Rigidbody rb;

		private Vector3 startPos;

		private Vector3 endPos;

		private Vector3 direction = Vector3.get_up();

		private Vector3 oldDirection = Vector3.get_up();

		private Vector3 destination;

		private bool isAtEnd;

		private void Start()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			rb = ((Component)this).GetComponent<Rigidbody>();
			startPos = ((Component)this).get_transform().get_position();
			endPos = ((Component)this).get_transform().get_position() + Vector3.get_up() * unitsToMove;
			destination = endPos;
		}

		public override void Process()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			direction = Vector3.get_up();
			output.SetValue(1f);
			invertedOutput.SetValue(0f);
		}

		private void FixedUpdate()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val;
			if ((double)input.value > 0.5)
			{
				rb.MovePosition(((Component)this).get_transform().get_position() + direction * speed * Time.get_deltaTime());
				val = ((Component)this).get_transform().get_position() - destination;
				if ((double)((Vector3)(ref val)).get_sqrMagnitude() < 0.01 && !isAtEnd)
				{
					isAtEnd = true;
					direction = Vector3.get_zero();
					((MonoBehaviour)this).StartCoroutine(SwitchDirection());
				}
			}
			else if ((double)input.value < 0.5)
			{
				val = ((Component)this).get_transform().get_position() - startPos;
				if ((double)((Vector3)(ref val)).get_sqrMagnitude() > 0.01)
				{
					rb.MovePosition(((Component)this).get_transform().get_position() + -Vector3.get_up() * speed * Time.get_deltaTime());
					return;
				}
				rb.set_velocity(Vector3.get_zero());
				invertedOutput.SetValue(1f);
				output.SetValue(0f);
			}
		}

		private IEnumerator SwitchDirection()
		{
			invertedOutput.SetValue(1f);
			output.SetValue(0f);
			yield return (object)new WaitForSeconds(holdTime);
			direction = ((oldDirection == Vector3.get_up()) ? (-Vector3.get_up()) : Vector3.get_up());
			oldDirection = direction;
			destination = ((destination == endPos) ? startPos : endPos);
			isAtEnd = false;
			output.SetValue(1f);
			invertedOutput.SetValue(0f);
		}
	}
}
