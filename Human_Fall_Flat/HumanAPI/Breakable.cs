using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class Breakable : Node, INetBehavior, IGrabbable
	{
		[Tooltip("This is the thing that will break")]
		public Rigidbody connectedBody;

		[Tooltip("This is the noise the thing will make when breaking")]
		public Sound2 sound;

		[NonSerialized]
		[Tooltip("This is whether or not the thing has broken")]
		public bool shattered;

		[NonSerialized]
		[Tooltip("This is the joint around which the thing will break")]
		public Joint joint;

		[Tooltip("Use ths in order to print debug info to the Log")]
		public bool showDebug;

		[Tooltip("The object has broken signal")]
		public NodeInput breakSignal;

		[Tooltip("Whether the object is broken")]
		public NodeOutput broken;

		[Tooltip("The threshold past which the signal will be sent ")]
		public float signalTreshold = 0.5f;

		public bool breakOnImpact;

		public float impulseTreshold = 10f;

		public float breakTreshold = 100f;

		public float accumulatedBreakTreshold = 300f;

		public float humanHardness;

		private float accumulatedImpact;

		private Vector3 adjustedImpulse = Vector3.get_zero();

		private Vector3 lastFrameImpact;

		[Tooltip("Should this break when pulled on by Bob ")]
		public bool breakOnPull;

		private bool grabbed;

		private float usedForceRelief;

		[Tooltip("The fore needed to break this thing by hand ")]
		public float grabbedBreakForce = 1500f;

		[Tooltip("The Torque needed to break this thing by Hand ")]
		public float grabbedBreakTorque = 1500f;

		[Tooltip("Use to Calc the jount break force by hand")]
		public float pullRelief = 1500f;

		[Tooltip("Used to Calc the break force for the thing no matter the interaction ")]
		public float breakForce = float.PositiveInfinity;

		[Tooltip("Used to Calc the Torque needed to break this thing no matter the interaction")]
		public float breakTorque = float.PositiveInfinity;

		[Tooltip("Used to set the direction the lock will break in when needed ")]
		public Vector3 breakNormal = Vector3.get_forward();

		public Joint CreateJoint()
		{
			FixedJoint obj = ((Component)this).get_gameObject().AddComponent<FixedJoint>();
			((Joint)obj).set_connectedBody(connectedBody);
			((Joint)obj).set_breakForce(breakForce);
			((Joint)obj).set_breakTorque(breakTorque);
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " A new joint finalised "));
			}
			return (Joint)(object)obj;
		}

		public void Shatter()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Shatter "));
			}
			shattered = true;
			if ((Object)(object)sound != (Object)null)
			{
				sound.PlayOneShot();
			}
			if ((Object)(object)joint != (Object)null)
			{
				Object.Destroy((Object)(object)joint);
				joint = null;
			}
			broken.SetValue(1f);
		}

		protected virtual void FixedUpdate()
		{
			if (!shattered)
			{
				if (breakOnImpact)
				{
					FixedUpdateOnImpact();
				}
				if (breakOnPull)
				{
					FixedUpdateOnPull();
				}
			}
		}

		public override void Process()
		{
			base.Process();
			if (!shattered && breakSignal.value > signalTreshold)
			{
				Shatter();
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " breakSignal.value " + breakSignal.value));
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " signalTreshold " + signalTreshold));
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			if (!shattered && breakOnImpact)
			{
				HandleCollision(collision, enter: false);
			}
		}

		public void OnCollisionStay(Collision collision)
		{
			if (!shattered && breakOnImpact)
			{
				HandleCollision(collision, enter: false);
			}
		}

		private void HandleCollision(Collision collision, bool enter)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			if (collision.get_contacts().Length == 0)
			{
				return;
			}
			Vector3 impulse = collision.GetImpulse();
			if (((Vector3)(ref impulse)).get_magnitude() < impulseTreshold)
			{
				return;
			}
			float num = 1f;
			Transform val = collision.get_transform();
			if (((Object)val).get_name().StartsWith("Lock"))
			{
				return;
			}
			while ((Object)(object)val != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " T != Null "));
				}
				if (Object.op_Implicit((Object)(object)((Component)val).GetComponent<Human>()))
				{
					num = humanHardness;
					break;
				}
				ShatterHardness component = ((Component)val).GetComponent<ShatterHardness>();
				if ((Object)(object)component != (Object)null)
				{
					num = component.hardness;
					break;
				}
				val = val.get_parent();
			}
			adjustedImpulse += impulse * num;
			Vector3 val2;
			if (showDebug)
			{
				string name = ((Object)this).get_name();
				val2 = adjustedImpulse;
				Debug.Log((object)(name + " adjustedImpulse " + ((object)(Vector3)(ref val2)).ToString()));
			}
			if (showDebug)
			{
				string name2 = ((Object)this).get_name();
				val2 = impulse;
				Debug.Log((object)(name2 + " impulse " + ((object)(Vector3)(ref val2)).ToString()));
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " hardnessValue " + num));
			}
		}

		private void FixedUpdateOnImpact()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			accumulatedImpact += ((Vector3)(ref adjustedImpulse)).get_magnitude();
			if (accumulatedImpact > accumulatedBreakTreshold)
			{
				shattered = true;
			}
			Vector3 val = adjustedImpulse + lastFrameImpact;
			if (((Vector3)(ref val)).get_magnitude() > breakTreshold)
			{
				shattered = true;
			}
			if (shattered)
			{
				Shatter();
			}
			lastFrameImpact = adjustedImpulse;
			adjustedImpulse = Vector3.get_zero();
		}

		private void ResetOnImpact()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset on Impact "));
			}
			accumulatedImpact = 0f;
			lastFrameImpact = (adjustedImpulse = Vector3.get_zero());
		}

		public void OnGrab()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " On Grab "));
			}
			if (breakOnPull && !grabbed)
			{
				grabbed = true;
				if ((Object)(object)joint != (Object)null)
				{
					joint.set_breakForce(grabbedBreakForce);
					joint.set_breakTorque(grabbedBreakTorque);
				}
			}
		}

		public void OnRelease()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " On Release "));
			}
			if (breakOnPull && grabbed)
			{
				grabbed = false;
				if ((Object)(object)joint != (Object)null)
				{
					joint.set_breakForce(breakForce);
					joint.set_breakTorque(breakTorque);
				}
			}
		}

		public void OnJointBreak(float breakForce)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Joint Break  "));
			}
			Shatter();
		}

		private void FixedUpdateOnPull()
		{
			if (!grabbed || (Object)(object)joint == (Object)null)
			{
				usedForceRelief = 0f;
				return;
			}
			float num = 0f;
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				num += GetForceRelief(human, human.ragdoll.partLeftHand) + GetForceRelief(human, human.ragdoll.partRightHand);
			}
			usedForceRelief = Mathf.Min(num, Mathf.MoveTowards(usedForceRelief, num, num * Time.get_fixedDeltaTime()));
			joint.set_breakForce(Mathf.Max(0f, grabbedBreakForce - usedForceRelief * pullRelief));
		}

		private float GetForceRelief(Human human, HumanSegment hand)
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Get Force Relief "));
			}
			if (!hand.sensor.IsGrabbed(((Component)this).get_gameObject()))
			{
				return 0f;
			}
			Vector3 val = ((Component)this).get_transform().TransformVector(breakNormal);
			Vector3 normalized = ((Vector3)(ref val)).get_normalized();
			float num = Mathf.InverseLerp(0.5f, 0.9f, Vector3.Dot(normalized, human.controls.walkDirection));
			return Mathf.Max(0f, num);
		}

		private void ResetOnPull()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset on Pull "));
			}
			usedForceRelief = 0f;
			joint.set_breakForce(breakForce);
			joint.set_breakTorque(breakTorque);
		}

		private void OnDrawGizmos()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (breakOnPull)
			{
				Gizmos.set_color(Color.get_yellow());
				Vector3 position = ((Component)this).get_transform().get_position();
				Vector3 val = ((Component)this).get_transform().TransformVector(breakNormal);
				Gizmos.DrawRay(position, ((Vector3)(ref val)).get_normalized());
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
		}

		public void SetMaster(bool isMaster)
		{
		}

		public void CollectState(NetStream stream)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " CollectState "));
			}
			NetBoolEncoder.CollectState(stream, shattered);
		}

		private void ApplyShatter(bool shatter)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply Shatter "));
			}
			if (shattered != shatter)
			{
				if (shatter)
				{
					Shatter();
				}
				else
				{
					ResetState(0, 0);
				}
			}
		}

		public virtual void ResetState(int checkpoint, int subObjectives)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset the state "));
			}
			shattered = false;
			broken.SetValue(0f);
			((Component)this).GetComponent<NetBody>().ResetState(checkpoint, subObjectives);
			if ((Object)(object)connectedBody != (Object)null)
			{
				((Component)connectedBody).GetComponent<NetBody>().ResetState(checkpoint, subObjectives);
			}
			if ((Object)(object)joint == (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " A new joint is being created "));
				}
				joint = CreateJoint();
			}
			if (breakOnImpact)
			{
				ResetOnImpact();
			}
			if (breakOnPull)
			{
				ResetOnPull();
			}
		}

		public void ApplyState(NetStream state)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply State "));
			}
			ApplyShatter(NetBoolEncoder.ApplyState(state));
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Lerped State "));
			}
			ApplyShatter(NetBoolEncoder.ApplyLerpedState(state0, state1, mix));
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc Delta "));
			}
			NetBoolEncoder.CalculateDelta(state0, state1, delta);
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Add Delta "));
			}
			NetBoolEncoder.AddDelta(state0, delta, result);
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Max Delta size "));
			}
			return NetBoolEncoder.CalculateMaxDeltaSizeInBits();
		}
	}
}
