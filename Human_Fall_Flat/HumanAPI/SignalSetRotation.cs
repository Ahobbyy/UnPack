using UnityEngine;

namespace HumanAPI
{
	public class SignalSetRotation : Node
	{
		[Tooltip("The input the node gets from the graph")]
		public NodeInput input;

		[Tooltip("This value is signalled when a single full rotation has been achieved")]
		public NodeOutput tickedOver;

		[Tooltip("The amount to increaase any rotation by as a multiplier")]
		public float multiplier = 1f;

		[Tooltip("The direction the thing will rotate in")]
		public Vector3 rotationVector = new Vector3(1f, 0f, 0f);

		[Tooltip("If this body has a configurable joint, apply the rotation to that instead of the body itself")]
		public bool applyToJoint = true;

		[Tooltip("Signal system this node is using")]
		public SignalBase triggerSignal;

		public bool incrementlMovement;

		private float storedValue;

		private float storedRotation;

		private float storedOutput;

		private ConfigurableJoint joint;

		private Rigidbody body;

		private Quaternion initialRotation;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		protected override void OnEnable()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enabled "));
			}
			base.OnEnable();
			joint = ((Component)this).GetComponent<ConfigurableJoint>();
			body = ((Component)this).GetComponent<Rigidbody>();
			initialRotation = ((Component)body).get_transform().get_rotation();
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
				Debug.Log((object)(((Object)this).get_name() + " Disabled "));
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
				Debug.Log((object)(((Object)this).get_name() + " Process "));
			}
			base.Process();
			if (incrementlMovement)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Do the incremental stuff  "));
					Debug.Log((object)(((Object)this).get_name() + " Sored value * Multiplier =  " + storedValue * multiplier));
				}
				if (input.value == 0f)
				{
					storedValue = input.value + storedValue;
				}
				else
				{
					storedValue = 1f + storedValue;
				}
				SignalChanged(storedValue);
				storedRotation = storedValue * multiplier;
				if (storedRotation < 0f)
				{
					storedRotation *= -1f;
				}
				if (storedRotation >= 360f)
				{
					if (showDebug)
					{
						Debug.Log((object)(((Object)this).get_name() + " Gone too far "));
					}
					storedValue = 0f;
					storedRotation = 0f;
					storedOutput += 1f;
					tickedOver.SetValue(storedOutput);
				}
			}
			else
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Do the normal stuff "));
				}
				SignalChanged(input.value);
			}
		}

		private void SignalChanged(float val)
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Signal Changed "));
				Debug.Log((object)(((Object)this).get_name() + " Value = " + val));
			}
			Quaternion val2 = Quaternion.AngleAxis(val * multiplier, rotationVector);
			if (applyToJoint && (Object)(object)joint != (Object)null)
			{
				joint.set_targetRotation(val2);
				return;
			}
			Quaternion val3 = initialRotation * val2;
			if (body.get_isKinematic())
			{
				((Component)body).get_transform().set_rotation(val3);
			}
			else
			{
				body.MoveRotation(val3);
			}
		}
	}
}
