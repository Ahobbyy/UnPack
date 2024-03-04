using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class CrumblingFloor : MonoBehaviour, INetBehavior
	{
		[Tooltip("The sound played when the player is stood on this thing")]
		public Sound2 sound2;

		[Tooltip("The time in seconds the player can stand on this thing")]
		public float standTime = 0.2f;

		[Tooltip("How long he player has to recover when standing on this thing ")]
		public float standRecovery = 2f;

		[Tooltip("The length of time in seconds the player can hold this thing before it breaks")]
		public float hangTime = 3f;

		[Tooltip("The length of time in seconds the player can use to recover when hanging onto this thing ")]
		public float hangRecovery = 3f;

		[Tooltip("The amount of force needed to break this thing")]
		public float breakForce = 10000f;

		private float hangHealth = 1f;

		private float standHealth = 1f;

		private bool moved;

		private Vector3 axis;

		private ConfigurableJoint leftJoint;

		private ConfigurableJoint rightJoint;

		private Vector3 startPos;

		private Quaternion startRot;

		public bool showDebug;

		private int brokenCount;

		private void OnEnable()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Ran the Enable stuff "));
			}
			if ((Object)(object)sound2 == (Object)null)
			{
				sound2 = ((Component)this).GetComponentInChildren<Sound2>();
			}
			startPos = ((Component)this).get_transform().get_position();
			startRot = ((Component)this).get_transform().get_rotation();
			Vector3 size = ((Component)this).GetComponent<BoxCollider>().get_size();
			if (size.x > size.y && size.x > size.z)
			{
				axis = Vector3.get_right() * size.x;
			}
			else if (size.y > size.z)
			{
				axis = Vector3.get_up() * size.y;
			}
			else
			{
				axis = Vector3.get_forward() * size.z;
			}
			leftJoint = AddJoint(-axis / 3f);
			rightJoint = AddJoint(axis / 3f);
		}

		private ConfigurableJoint AddJoint(Vector3 pos)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Adding joints "));
			}
			ConfigurableJoint obj = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)obj).set_anchor(pos);
			obj.set_xMotion((ConfigurableJointMotion)0);
			obj.set_yMotion((ConfigurableJointMotion)1);
			obj.set_zMotion((ConfigurableJointMotion)0);
			obj.set_angularXMotion((ConfigurableJointMotion)2);
			obj.set_angularYMotion((ConfigurableJointMotion)0);
			obj.set_angularZMotion((ConfigurableJointMotion)0);
			SoftJointLimit linearLimit = default(SoftJointLimit);
			((SoftJointLimit)(ref linearLimit)).set_limit(0.03f);
			obj.set_linearLimit(linearLimit);
			JointDrive yDrive = default(JointDrive);
			((JointDrive)(ref yDrive)).set_positionSpring(2000f);
			((JointDrive)(ref yDrive)).set_maximumForce(5000f);
			((JointDrive)(ref yDrive)).set_positionDamper(100f);
			obj.set_yDrive(yDrive);
			obj.set_targetPosition(-Vector3.get_up() / 2f);
			((Joint)obj).set_breakForce(breakForce);
			return obj;
		}

		public void FixedUpdate()
		{
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0276: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			if (ReplayRecorder.isPlaying || NetGame.isClient || (Object)(object)leftJoint == (Object)null || (Object)(object)rightJoint == (Object)null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				if (human.grabManager.IsGrabbed(((Component)this).get_gameObject()))
				{
					flag = true;
				}
				else if (human.groundManager.IsStanding(((Component)this).get_gameObject()))
				{
					flag2 = true;
					float num = human.ragdoll.partBall.transform.get_position().y - ((Component)this).get_transform().get_position().y;
					if (num > -0.3f && num < 0.2f)
					{
						return;
					}
				}
			}
			if (flag2)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " We are standing "));
				}
				standHealth -= Time.get_fixedDeltaTime() / standTime;
			}
			else
			{
				standHealth += Time.get_fixedDeltaTime() / standRecovery;
			}
			if (flag)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " We have been grabbed "));
				}
				hangHealth -= Time.get_fixedDeltaTime() / hangTime;
			}
			else
			{
				hangHealth += Time.get_fixedDeltaTime() / hangRecovery;
			}
			standHealth = Mathf.Clamp01(standHealth);
			hangHealth = Mathf.Clamp01(hangHealth);
			if (!moved && (standHealth < 0.5f || hangHealth < 0.5f))
			{
				moved = true;
				SoftJointLimit linearLimit;
				JointDrive yDrive;
				if (((Component)this).get_transform().TransformPoint(((Joint)leftJoint).get_anchor()).y < ((Component)this).get_transform().TransformPoint(((Joint)rightJoint).get_anchor()).y)
				{
					ConfigurableJoint obj = leftJoint;
					linearLimit = default(SoftJointLimit);
					((SoftJointLimit)(ref linearLimit)).set_limit(0.2f);
					obj.set_linearLimit(linearLimit);
					ConfigurableJoint obj2 = leftJoint;
					yDrive = default(JointDrive);
					((JointDrive)(ref yDrive)).set_positionSpring(1000f);
					((JointDrive)(ref yDrive)).set_maximumForce(0f);
					((JointDrive)(ref yDrive)).set_positionDamper(100f);
					obj2.set_yDrive(yDrive);
				}
				else
				{
					ConfigurableJoint obj3 = rightJoint;
					linearLimit = default(SoftJointLimit);
					((SoftJointLimit)(ref linearLimit)).set_limit(0.1f);
					obj3.set_linearLimit(linearLimit);
					ConfigurableJoint obj4 = rightJoint;
					yDrive = default(JointDrive);
					((JointDrive)(ref yDrive)).set_positionSpring(1000f);
					((JointDrive)(ref yDrive)).set_maximumForce(0f);
					((JointDrive)(ref yDrive)).set_positionDamper(100f);
					obj4.set_yDrive(yDrive);
				}
				sound2.PlayOneShot(0.5f, 1.2f);
			}
			else if (standHealth == 0f || hangHealth == 0f)
			{
				Object.Destroy((Object)(object)leftJoint);
				Object.Destroy((Object)(object)rightJoint);
				sound2.PlayOneShot();
			}
		}

		public void OnJointBreak(float breakForce)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Joint Break "));
			}
			if (brokenCount == 0)
			{
				sound2.PlayOneShot();
				if (Random.Range(0, 1) == 1)
				{
					Object.Destroy((Object)(object)leftJoint);
				}
				else
				{
					Object.Destroy((Object)(object)rightJoint);
				}
			}
			else if (brokenCount == 1)
			{
				if ((Object)(object)leftJoint != (Object)null)
				{
					Object.Destroy((Object)(object)leftJoint);
				}
				if ((Object)(object)rightJoint != (Object)null)
				{
					Object.Destroy((Object)(object)rightJoint);
				}
			}
			brokenCount++;
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			hangHealth = (standHealth = 1f);
			Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
			Vector3 position;
			((Component)this).get_transform().set_position(position = startPos);
			component.set_position(position);
			Quaternion rotation;
			((Component)this).get_transform().set_rotation(rotation = startRot);
			component.set_rotation(rotation);
			component.set_angularVelocity(position = Vector3.get_zero());
			component.set_velocity(position);
			if ((Object)(object)leftJoint != (Object)null)
			{
				Object.Destroy((Object)(object)leftJoint);
			}
			if ((Object)(object)rightJoint != (Object)null)
			{
				Object.Destroy((Object)(object)rightJoint);
			}
			leftJoint = AddJoint(-axis / 3f);
			rightJoint = AddJoint(axis / 3f);
		}

		private void ApplyState(bool left, bool right)
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply bool state "));
			}
			if (!left && (Object)(object)leftJoint != (Object)null)
			{
				sound2.PlayOneShot(0.5f, 1.2f);
				Object.Destroy((Object)(object)leftJoint);
			}
			if (!right && (Object)(object)rightJoint != (Object)null)
			{
				sound2.PlayOneShot(0.5f, 1.2f);
				Object.Destroy((Object)(object)rightJoint);
			}
			if (left && (Object)(object)leftJoint == (Object)null)
			{
				leftJoint = AddJoint(-axis / 3f);
			}
			if (right && (Object)(object)rightJoint == (Object)null)
			{
				rightJoint = AddJoint(axis / 3f);
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
				Debug.Log((object)(((Object)this).get_name() + " Collected State "));
			}
			NetBoolEncoder.CollectState(stream, (Object)(object)leftJoint != (Object)null);
			NetBoolEncoder.CollectState(stream, (Object)(object)rightJoint != (Object)null);
		}

		public void ApplyState(NetStream state)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Applying State "));
			}
			ApplyState(NetBoolEncoder.ApplyState(state), NetBoolEncoder.ApplyState(state));
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply Lerped State "));
			}
			ApplyState(NetBoolEncoder.ApplyLerpedState(state0, state1, mix), NetBoolEncoder.ApplyLerpedState(state0, state1, mix));
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc the Delta "));
			}
			NetBoolEncoder.CalculateDelta(state0, state1, delta);
			NetBoolEncoder.CalculateDelta(state0, state1, delta);
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Add the delta  "));
			}
			NetBoolEncoder.AddDelta(state0, delta, result);
			NetBoolEncoder.AddDelta(state0, delta, result);
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc max delta size in bits "));
			}
			return 2 * NetBoolEncoder.CalculateMaxDeltaSizeInBits();
		}

		public CrumblingFloor()
			: this()
		{
		}
	}
}
