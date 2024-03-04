using UnityEngine;

namespace HumanAPI
{
	public class SignalSetTranslation : Node
	{
		[Tooltip("The incoming value to do something with")]
		public NodeInput input;

		[Tooltip("The amount of motion to apply when getting a signal0")]
		public float multiplier = 1f;

		[Tooltip("The axis to move the thing in ")]
		public Vector3 translationVector = new Vector3(1f, 0f, 0f);

		[Tooltip("Affect local translation rather than global. Doesn't currently work for rigid bodies")]
		public bool transformLocal;

		[Tooltip("Trigge3r signal coming from somewhere ")]
		public SignalBase triggerSignal;

		private Rigidbody body;

		private Vector3 initialPosition;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		protected override void OnEnable()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " On Enable "));
			}
			base.OnEnable();
			body = ((Component)this).GetComponent<Rigidbody>();
			if (transformLocal && (Object)(object)body == (Object)null)
			{
				initialPosition = ((Component)this).get_transform().get_localPosition();
			}
			else
			{
				initialPosition = ((Component)this).get_transform().get_position();
			}
			if ((Object)(object)triggerSignal != (Object)null)
			{
				triggerSignal.onValueChanged += SignalChanged;
				SignalChanged(triggerSignal.value);
			}
		}

		protected override void OnDisable()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " On Disable"));
			}
			base.OnDisable();
			if ((Object)(object)triggerSignal != (Object)null)
			{
				triggerSignal.onValueChanged -= SignalChanged;
			}
		}

		public override void Process()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Process Signal "));
			}
			base.Process();
			SignalChanged(input.value);
		}

		private void SignalChanged(float val)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " We are running the signal changed stuff "));
			}
			Vector3 val2 = val * multiplier * translationVector;
			if ((Object)(object)body != (Object)null)
			{
				body.MovePosition(initialPosition + val2);
			}
			else if (transformLocal)
			{
				((Component)this).get_transform().set_localPosition(initialPosition + val2);
			}
			else
			{
				((Component)this).get_transform().set_position(initialPosition + val2);
			}
		}
	}
}
