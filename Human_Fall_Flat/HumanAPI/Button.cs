using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Button", 10)]
	public class Button : Node
	{
		public ConfigurableJoint slideJoint;

		private Rigidbody slideJointRigidBody;

		public float buttonForce = 100f;

		public float buttonSpeed = 0.25f;

		public float motionRange = 0.05f;

		public ButtonActivationMode activationMode;

		public ButtonAction buttonAction;

		public bool trueWhenPressed = true;

		public NodeInput input;

		public NodeInput reset;

		public NodeOutput value;

		private Transform buttonTransform;

		private bool inputState;

		private bool pressedState;

		private bool lightOn;

		[Tooltip("Set to true to see the debug output from this button")]
		public bool showdebug;

		[Tooltip("Whether or not the button should use the action assigned")]
		public bool usebuttonaction;

		private bool grabbed;

		private float bypassTimer;

		private bool isDown;

		private float bounceBackTimer;

		protected void Awake()
		{
			if ((Object)(object)slideJoint == (Object)null)
			{
				slideJoint = ((Component)this).GetComponent<ConfigurableJoint>();
			}
			if ((Object)(object)slideJoint != (Object)null && (Object)(object)slideJointRigidBody == (Object)null)
			{
				slideJointRigidBody = ((Component)slideJoint).GetComponent<Rigidbody>();
			}
			buttonTransform = ((Component)slideJoint).get_transform();
			slideJoint.SetXMotionAnchorsAndLimits(motionRange / 2f, motionRange);
			SetForce(buttonForce);
			inputState = Mathf.Abs(input.value) >= 0.5f;
			Snap(inputState);
		}

		private void SetForce(float buttonForce)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			JointDrive xDrive = slideJoint.get_xDrive();
			((JointDrive)(ref xDrive)).set_maximumForce(buttonForce);
			((JointDrive)(ref xDrive)).set_positionSpring(buttonForce / motionRange);
			((JointDrive)(ref xDrive)).set_positionDamper(buttonForce / buttonSpeed);
			slideJoint.set_xDrive(xDrive);
		}

		public override void Process()
		{
			base.Process();
			bool flag = Mathf.Abs(input.value) >= 0.5f;
			if (inputState != flag)
			{
				inputState = flag;
				Snap(inputState);
			}
			if (reset.value > 0.5f)
			{
				ResetButton();
			}
		}

		private void OnPress()
		{
			if (!usebuttonaction)
			{
				return;
			}
			switch (buttonAction)
			{
			case ButtonAction.Hold:
				value.SetValue(1f);
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " doing the hold actions "));
				}
				PressedSettings();
				break;
			case ButtonAction.Press:
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Doing the press actions "));
				}
				PressedSettings();
				break;
			case ButtonAction.Toggle:
				value.SetValue((!(input.value > 0.5f)) ? 1 : 0);
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Doing the toggle actions "));
				}
				pressedState = true;
				Snap(pressedState);
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		private void OnRelease()
		{
			if (!usebuttonaction)
			{
				return;
			}
			switch (buttonAction)
			{
			case ButtonAction.Hold:
				value.SetValue(0f);
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Releasing with a Hold action "));
				}
				ReleaseSettings();
				break;
			case ButtonAction.Press:
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Releasing with a press action  "));
				}
				pressedState = false;
				Snap(pressedState);
				value.SetValue(1f);
				break;
			case ButtonAction.Toggle:
				value.SetValue((input.value > 0.5f) ? 1 : 0);
				pressedState = false;
				if ((double)input.value > 0.5)
				{
					if (showdebug)
					{
						Debug.Log((object)(((Object)this).get_name() + " Input Value is 1 "));
					}
					value.SetValue(1f);
					Snap(pressed: false);
				}
				else
				{
					if (showdebug)
					{
						Debug.Log((object)(((Object)this).get_name() + " Input value is 0  "));
					}
					value.SetValue(0f);
					Snap(pressed: false);
				}
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		private void ReleaseSettings()
		{
			pressedState = false;
			Snap(pressedState);
			value.SetValue(0f);
		}

		private void PressedSettings()
		{
			pressedState = true;
			Snap(pressedState);
			value.SetValue(1f);
		}

		public void ResetButton()
		{
			if (showdebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Resetting this button "));
			}
			switch (buttonAction)
			{
			case ButtonAction.Press:
				pressedState = false;
				value.SetValue(0f);
				break;
			case ButtonAction.Toggle:
				ReleaseSettings();
				break;
			}
			reset.value = 0f;
		}

		private void Snap(bool pressed)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			if (showdebug)
			{
				Debug.Log((object)(((Object)this).get_name() + pressed));
			}
			if (activationMode == ButtonActivationMode.Touch)
			{
				slideJoint.set_targetPosition(new Vector3(pressed ? (0f - motionRange) : motionRange, 0f, 0f));
			}
			if ((Object)(object)slideJointRigidBody != (Object)null)
			{
				slideJointRigidBody.WakeUp();
			}
			bypassTimer = 0.1f;
			if (SignalManager.skipTransitions)
			{
				slideJoint.ApplyXMotionTarget();
			}
		}

		public void Update()
		{
			if (NetGame.isClient && !ReplayRecorder.isPlaying)
			{
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Client , dont do anything  "));
				}
				return;
			}
			float num = ((activationMode != ButtonActivationMode.Touch) ? CalculateNormalizedOffset() : (-1f));
			bool flag = num > 0.5f;
			bool flag2 = num < 0.25f;
			bool flag3 = activationMode != 0 && CalculateGrabbed();
			if (flag && buttonAction == ButtonAction.Hold && (Object)(object)slideJointRigidBody != (Object)null && slideJointRigidBody.IsSleeping())
			{
				slideJointRigidBody.WakeUp();
			}
			if (!pressedState && (flag || flag3))
			{
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Pressed state should be true "));
				}
				if (usebuttonaction)
				{
					OnPress();
				}
				else
				{
					PressedSettings();
				}
			}
			else if (pressedState && flag2 && !flag3)
			{
				if (showdebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Pressed state should be false "));
				}
				if (usebuttonaction && pressedState)
				{
					if (showdebug)
					{
						Debug.Log((object)(((Object)this).get_name() + " using button action "));
					}
					OnRelease();
				}
				else if (!usebuttonaction && pressedState)
				{
					if (showdebug)
					{
						Debug.Log((object)(((Object)this).get_name() + " not using button action "));
					}
					ReleaseSettings();
				}
			}
			if (flag3 && activationMode != 0)
			{
				SetForce(buttonForce + 2000f);
			}
			else
			{
				SetForce(buttonForce);
			}
		}

		private bool CalculateGrabbed()
		{
			return GrabManager.IsGrabbedAny(((Component)buttonTransform).get_gameObject());
		}

		private float CalculateNormalizedOffset()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = buttonTransform.TransformDirection(((Joint)slideJoint).get_axis());
			Vector3 val2 = buttonTransform.TransformPoint(((Joint)slideJoint).get_anchor());
			Vector3 val3 = ((Component)((Joint)slideJoint).get_connectedBody()).get_transform().TransformPoint(((Joint)slideJoint).get_connectedAnchor());
			return Vector3.Dot(val2 - val3, val) / motionRange + 0.5f;
		}
	}
}
