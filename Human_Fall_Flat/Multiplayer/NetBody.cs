using System;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;
using UnityEngine.Events;

namespace Multiplayer
{
	[RequireComponent(typeof(NetIdentity))]
	public class NetBody : MonoBehaviour, INetBehavior, IRespawnable
	{
		public Transform relativeTo;

		public NetBodySyncPosition syncPosition = NetBodySyncPosition.World;

		public NetBodySyncRotation syncRotation = NetBodySyncRotation.World;

		public float posRangeOverride;

		public int posPrecision;

		public int rotPrecision;

		public bool smooth;

		public bool respawn;

		public float despawnHeight = -50f;

		public float respawnHeight = 5f;

		public bool isKinematic;

		public bool manageActive = true;

		public UnityEvent m_respawnEvent;

		[NonSerialized]
		public float posRange = 500f;

		[NonSerialized]
		public ushort possmall = 4;

		[NonSerialized]
		public ushort poslarge = 8;

		[NonSerialized]
		public ushort posfull = 18;

		[NonSerialized]
		public ushort rotsmall = 4;

		[NonSerialized]
		public ushort rotlarge = 6;

		[NonSerialized]
		public ushort rotfull = 9;

		private NetFloat eulerEncoder = new NetFloat(180f, 12, 4, 8);

		private Rigidbody body;

		public bool syncLocalScale;

		private Vector3 startLocalScale;

		[NonSerialized]
		public Vector3 startPos;

		[NonSerialized]
		public Quaternion startRot;

		private Quaternion baseRot;

		private Quaternion baseRotInv;

		private Vector3 basePos;

		[NonSerialized]
		public NetQuaternion collectedRot;

		[NonSerialized]
		public NetQuaternion appliedRot;

		[NonSerialized]
		public float appliedEuler;

		[NonSerialized]
		public float collectedEuler;

		private NetVector3 zero;

		private NetQuaternion identity;

		private NetBodySleep sleep;

		[NonSerialized]
		public bool disableSleep;

		private bool isStarted;

		private bool hasNetSetActive;

		private int mResetFramesAdditionalDelay;

		private int mJustReset;

		private Vector3 mResetPosition;

		private Quaternion mResetRotation;

		private const int kResetFramesToReset = 5;

		private string[] mBadObjectNames = new string[2] { "MotorBoat", "BigBoat" };

		private SmoothQuaternion smoothRot;

		private SmoothVector3 smoothPos;

		[NonSerialized]
		public bool isVisible = true;

		private List<RespawningBodyOverride> overrides;

		public Rigidbody RigidBody => body;

		public bool HandlingReset { get; private set; }

		public bool ResetLastFrame { get; private set; }

		public void Start()
		{
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			if (!isStarted)
			{
				isStarted = true;
				if (NetGame.isClient)
				{
					hasNetSetActive = (Object)(object)((Component)this).GetComponent<NetSetActive>() != (Object)null;
				}
				NetBodyResetOverride component = ((Component)this).GetComponent<NetBodyResetOverride>();
				if (Object.op_Implicit((Object)(object)component))
				{
					mResetFramesAdditionalDelay = component.FramesDelay;
				}
				body = ((Component)this).GetComponent<Rigidbody>();
				isKinematic = body.get_isKinematic();
				if (syncPosition == NetBodySyncPosition.Relative)
				{
					posRange = 10f;
					posfull -= 6;
				}
				if (syncPosition == NetBodySyncPosition.Local)
				{
					posRange = 10f;
					posfull -= 6;
				}
				if (posRangeOverride != 0f)
				{
					posRange = posRangeOverride;
				}
				eulerEncoder.fullBits = (ushort)(eulerEncoder.fullBits + rotPrecision);
				eulerEncoder.deltaSmall = (ushort)(eulerEncoder.deltaSmall + rotPrecision);
				eulerEncoder.deltaLarge = (ushort)(eulerEncoder.deltaLarge + rotPrecision);
				possmall = (ushort)(possmall + posPrecision);
				poslarge = (ushort)(poslarge + posPrecision);
				posfull = (ushort)(posfull + posPrecision);
				rotfull = (ushort)(rotfull + rotPrecision);
				rotsmall = (ushort)(rotsmall + rotPrecision);
				rotlarge = (ushort)(rotlarge + rotPrecision);
				startPos = ((Component)this).get_transform().get_position();
				startRot = ((Component)this).get_transform().get_rotation();
				startLocalScale = ((Component)this).get_transform().get_localScale();
				if (syncPosition == NetBodySyncPosition.Relative)
				{
					basePos = ((Component)this).get_transform().get_position() - relativeTo.get_position();
				}
				else if (syncPosition == NetBodySyncPosition.Absolute)
				{
					basePos = Vector3.get_zero();
				}
				else
				{
					basePos = ((syncPosition == NetBodySyncPosition.Local) ? ((Component)this).get_transform().get_localPosition() : ((Component)this).get_transform().get_position());
				}
				if (syncRotation == NetBodySyncRotation.Relative)
				{
					baseRot = Quaternion.Inverse(relativeTo.get_rotation()) * ((Component)this).get_transform().get_rotation();
				}
				else if (syncRotation == NetBodySyncRotation.Absolute)
				{
					baseRot = Quaternion.get_identity();
				}
				else
				{
					baseRot = ((syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ) ? ((Component)this).get_transform().get_localRotation() : ((Component)this).get_transform().get_rotation());
				}
				baseRotInv = Quaternion.Inverse(baseRot);
				zero = NetVector3.Quantize(Vector3.get_zero(), posRange, posfull);
				identity = NetQuaternion.Quantize(Quaternion.get_identity(), rotfull);
				if (!disableSleep)
				{
					sleep = new NetBodySleep(body);
				}
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
			if (!isStarted)
			{
				Start();
			}
		}

		public void SetMaster(bool master)
		{
			if (!isStarted)
			{
				Start();
			}
			body.set_isKinematic(!master || isKinematic);
		}

		private void FixedUpdate()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			if (!isStarted)
			{
				Start();
			}
			HandleRespawn();
			if (ResetLastFrame)
			{
				ResetLastFrame = false;
			}
			if (mJustReset > 0)
			{
				mJustReset--;
				if (mJustReset >= 5 - (1 + mResetFramesAdditionalDelay))
				{
					body.set_isKinematic(true);
					body.set_position(mResetPosition);
					body.set_rotation(mResetRotation);
					body.set_angularVelocity(Vector3.get_zero());
					body.set_velocity(Vector3.get_zero());
				}
				if (mJustReset == 0)
				{
					HandlingReset = false;
					ResetLastFrame = true;
					if (!isKinematic)
					{
						body.set_isKinematic(false);
						UpdateVisibility();
					}
				}
			}
			if (sleep != null)
			{
				sleep.HandleSleep();
			}
		}

		public void CollectState(NetStream stream)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			if (syncPosition == NetBodySyncPosition.Relative)
			{
				NetVector3.Quantize(((Component)this).get_transform().get_position() - relativeTo.get_position() - basePos, posRange, posfull).Write(stream);
			}
			else if (syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World)
			{
				NetVector3.Quantize(((syncPosition == NetBodySyncPosition.Local) ? ((Component)this).get_transform().get_localPosition() : ((Component)this).get_transform().get_position()) - basePos, posRange, posfull).Write(stream);
			}
			if (syncRotation == NetBodySyncRotation.Relative)
			{
				NetQuaternion netQuaternion = (collectedRot = NetQuaternion.Quantize(baseRotInv * Quaternion.Inverse(relativeTo.get_rotation()) * ((Component)this).get_transform().get_rotation(), rotfull));
				netQuaternion.Write(stream);
			}
			else if (syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				NetQuaternion netQuaternion2 = (collectedRot = NetQuaternion.Quantize(baseRotInv * ((syncRotation == NetBodySyncRotation.Local) ? ((Component)this).get_transform().get_localRotation() : ((Component)this).get_transform().get_rotation()), rotfull));
				netQuaternion2.Write(stream);
			}
			else if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				float value = (collectedEuler = syncRotation switch
				{
					NetBodySyncRotation.EulerX => 0f - Math3d.SignedVectorAngle(baseRotInv * ((Component)this).get_transform().get_localRotation() * Vector3.get_up(), Vector3.get_up(), Vector3.get_right()), 
					NetBodySyncRotation.EulerY => 0f - Math3d.SignedVectorAngle(baseRotInv * ((Component)this).get_transform().get_localRotation() * Vector3.get_forward(), Vector3.get_forward(), Vector3.get_up()), 
					NetBodySyncRotation.EulerZ => 0f - Math3d.SignedVectorAngle(baseRotInv * ((Component)this).get_transform().get_localRotation() * Vector3.get_right(), Vector3.get_right(), Vector3.get_forward()), 
					_ => throw new InvalidOperationException(), 
				});
				eulerEncoder.CollectState(stream, value);
			}
			if (syncLocalScale)
			{
				NetVector3.Quantize(((Component)this).get_transform().get_localScale(), posRange, posfull).Write(stream);
			}
		}

		public void ApplyState(NetStream state)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			if (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World)
			{
				Vector3 target = NetVector3.Read(state, posfull).Dequantize(posRange);
				ApplyPositionState(target);
			}
			if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				NetQuaternion netQuaternion = (appliedRot = NetQuaternion.Read(state, rotfull));
				Quaternion target2 = netQuaternion.Dequantize();
				ApplyQuaternionState(target2);
			}
			else if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				float diff = eulerEncoder.ApplyState(state);
				ApplyEulerState(diff);
			}
			if ((Object)(object)body != (Object)null)
			{
				Rigidbody obj = body;
				Vector3 angularVelocity;
				body.set_velocity(angularVelocity = Vector3.get_zero());
				obj.set_angularVelocity(angularVelocity);
			}
			if (syncLocalScale)
			{
				Vector3 localScale = NetVector3.Read(state, posfull).Dequantize(posRange);
				((Component)this).get_transform().set_localScale(localScale);
			}
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			Vector3 angularVelocity;
			if (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World)
			{
				Vector3 val = NetVector3.Read(state0, posfull).Dequantize(posRange);
				Vector3 val2 = NetVector3.Read(state1, posfull).Dequantize(posRange);
				angularVelocity = val - val2;
				if (((Vector3)(ref angularVelocity)).get_sqrMagnitude() > 15f)
				{
					val = val2;
				}
				Vector3 target = Vector3.Lerp(val, val2, mix);
				ApplyPositionState(target);
			}
			if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				Quaternion val3 = NetQuaternion.Read(state0, rotfull).Dequantize();
				NetQuaternion netQuaternion = (appliedRot = NetQuaternion.Read(state1, rotfull));
				Quaternion val4 = netQuaternion.Dequantize();
				Quaternion target2 = Quaternion.Slerp(val3, val4, mix);
				ApplyQuaternionState(target2);
			}
			else if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				float diff = eulerEncoder.ApplyLerpedState(state0, state1, mix);
				ApplyEulerState(diff);
			}
			if ((Object)(object)body != (Object)null)
			{
				Rigidbody obj = body;
				body.set_velocity(angularVelocity = Vector3.get_zero());
				obj.set_angularVelocity(angularVelocity);
			}
			if (syncLocalScale && (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World))
			{
				Vector3 val5 = NetVector3.Read(state0, posfull).Dequantize(posRange);
				Vector3 val6 = NetVector3.Read(state1, posfull).Dequantize(posRange);
				Vector3 localScale = Vector3.Lerp(val5, val6, mix);
				((Component)this).get_transform().set_localScale(localScale);
			}
		}

		private void ApplyPositionState(Vector3 target)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			target += basePos;
			if (smooth)
			{
				if (smoothPos == null)
				{
					smoothPos = new SmoothVector3();
				}
				target = smoothPos.Next(target);
			}
			if (syncPosition == NetBodySyncPosition.Relative)
			{
				target += relativeTo.get_position();
				if (((Component)this).get_transform().get_position() != target)
				{
					((Component)this).get_transform().set_position(target);
				}
			}
			else if (syncPosition == NetBodySyncPosition.Local)
			{
				if (((Component)this).get_transform().get_localPosition() != target)
				{
					((Component)this).get_transform().set_localPosition(target);
				}
			}
			else if (((Component)this).get_transform().get_position() != target)
			{
				((Component)this).get_transform().set_position(target);
			}
			if (!hasNetSetActive)
			{
				UpdateVisibility();
			}
		}

		private void ApplyQuaternionState(Quaternion target)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			target = baseRot * target;
			if (smooth)
			{
				if (smoothRot == null)
				{
					smoothRot = new SmoothQuaternion();
				}
				target = smoothRot.Next(target);
			}
			Quaternion val;
			if (syncRotation == NetBodySyncRotation.Relative)
			{
				target = relativeTo.get_rotation() * target;
				val = ((Component)this).get_transform().get_rotation();
				if (((Quaternion)(ref val)).get_eulerAngles() != ((Quaternion)(ref target)).get_eulerAngles())
				{
					((Component)this).get_transform().set_rotation(target);
				}
			}
			else if (syncRotation == NetBodySyncRotation.Local)
			{
				val = ((Component)this).get_transform().get_localRotation();
				if (((Quaternion)(ref val)).get_eulerAngles() != ((Quaternion)(ref target)).get_eulerAngles())
				{
					((Component)this).get_transform().set_localRotation(target);
				}
			}
			else
			{
				val = ((Component)this).get_transform().get_rotation();
				if (((Quaternion)(ref val)).get_eulerAngles() != ((Quaternion)(ref target)).get_eulerAngles())
				{
					((Component)this).get_transform().set_rotation(target);
				}
			}
		}

		private void ApplyEulerState(float diff)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			appliedEuler = diff;
			Quaternion val = baseRot;
			switch (syncRotation)
			{
			case NetBodySyncRotation.EulerX:
				val *= Quaternion.AngleAxis(diff, Vector3.get_right());
				break;
			case NetBodySyncRotation.EulerY:
				val *= Quaternion.AngleAxis(diff, Vector3.get_up());
				break;
			case NetBodySyncRotation.EulerZ:
				val *= Quaternion.AngleAxis(diff, Vector3.get_forward());
				break;
			}
			Quaternion localRotation = ((Component)this).get_transform().get_localRotation();
			if (((Quaternion)(ref localRotation)).get_eulerAngles() != ((Quaternion)(ref val)).get_eulerAngles())
			{
				((Component)this).get_transform().set_localRotation(val);
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			bool flag = false;
			NetVector3 netVector = default(NetVector3);
			NetVector3 netVector2 = default(NetVector3);
			NetQuaternion netQuaternion = default(NetQuaternion);
			NetQuaternion netQuaternion2 = default(NetQuaternion);
			if (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World || syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				if (syncPosition != 0)
				{
					netVector = ((state0 == null) ? zero : NetVector3.Read(state0, posfull));
					netVector2 = NetVector3.Read(state1, posfull);
					flag |= netVector2 != netVector;
				}
				if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
				{
					netQuaternion = ((state0 == null) ? identity : NetQuaternion.Read(state0, rotfull));
					netQuaternion2 = NetQuaternion.Read(state1, rotfull);
					flag |= netQuaternion2 != netQuaternion;
				}
				if (flag)
				{
					delta.Write(v: true);
					if (syncPosition != 0)
					{
						NetVector3.Delta(netVector, netVector2, poslarge).Write(delta, possmall, poslarge, posfull);
					}
					if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
					{
						NetQuaternion.Delta(netQuaternion, netQuaternion2, rotlarge).Write(delta, rotsmall, rotlarge, rotfull);
					}
				}
				else
				{
					delta.Write(v: false);
				}
			}
			if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				eulerEncoder.CalculateDelta(state0, state1, delta);
			}
			if (syncLocalScale)
			{
				NetVector3 netVector3 = default(NetVector3);
				NetVector3 netVector4 = default(NetVector3);
				netVector3 = ((state0 == null) ? zero : NetVector3.Read(state0, posfull));
				netVector4 = NetVector3.Read(state1, posfull);
				if (netVector4 != netVector3)
				{
					delta.Write(v: true);
					NetVector3.Delta(netVector3, netVector4, poslarge).Write(delta, possmall, poslarge, posfull);
				}
				else
				{
					delta.Write(v: false);
				}
			}
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World || syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				if (delta.ReadBool())
				{
					if (syncPosition != 0)
					{
						NetVector3 from = ((state0 == null) ? zero : NetVector3.Read(state0, posfull));
						NetVector3Delta delta2 = NetVector3Delta.Read(delta, possmall, poslarge, posfull);
						NetVector3.AddDelta(from, delta2).Write(result);
					}
					if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
					{
						NetQuaternion from2 = ((state0 == null) ? identity : NetQuaternion.Read(state0, rotfull));
						NetQuaternionDelta delta3 = NetQuaternionDelta.Read(delta, rotsmall, rotlarge, rotfull);
						NetQuaternion.AddDelta(from2, delta3).Write(result);
					}
				}
				else
				{
					if (syncPosition != 0)
					{
						((state0 == null) ? zero : NetVector3.Read(state0, posfull)).Write(result);
					}
					if (syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
					{
						((state0 == null) ? identity : NetQuaternion.Read(state0, rotfull)).Write(result);
					}
				}
			}
			if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				eulerEncoder.AddDelta(state0, delta, result);
			}
			if (syncLocalScale)
			{
				if (delta.ReadBool())
				{
					NetVector3 from3 = ((state0 == null) ? zero : NetVector3.Read(state0, posfull));
					NetVector3Delta delta4 = NetVector3Delta.Read(delta, possmall, poslarge, posfull);
					NetVector3.AddDelta(from3, delta4).Write(result);
				}
				else
				{
					((state0 == null) ? zero : NetVector3.Read(state0, posfull)).Write(result);
				}
			}
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			int num = 0;
			if (syncPosition == NetBodySyncPosition.Relative || syncPosition == NetBodySyncPosition.Absolute || syncPosition == NetBodySyncPosition.Local || syncPosition == NetBodySyncPosition.World || syncRotation == NetBodySyncRotation.Relative || syncRotation == NetBodySyncRotation.Absolute || syncRotation == NetBodySyncRotation.Local || syncRotation == NetBodySyncRotation.World)
			{
				num++;
				num += NetVector3Delta.CalculateMaxDeltaSizeInBits(possmall, poslarge, posfull);
				num += NetQuaternionDelta.CalculateMaxDeltaSizeInBits(rotsmall, rotlarge, rotfull);
			}
			if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				num += eulerEncoder.CalculateMaxDeltaSizeInBits();
			}
			if (syncLocalScale)
			{
				num++;
				num += NetVector3Delta.CalculateMaxDeltaSizeInBits(possmall, poslarge, posfull);
			}
			return num;
		}

		public void SetVisible(bool visible)
		{
			isVisible = visible;
			UpdateVisibility();
		}

		private void UpdateVisibility()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			if (manageActive)
			{
				bool flag = ((Component)this).get_transform().get_position().y > despawnHeight && isVisible;
				if (((Component)this).get_gameObject().get_activeSelf() != flag)
				{
					((Component)this).get_gameObject().SetActive(flag);
				}
			}
		}

		public void AddOverride(RespawningBodyOverride respawningBodyOverride)
		{
			if (overrides == null)
			{
				overrides = new List<RespawningBodyOverride>();
			}
			overrides.Add(respawningBodyOverride);
		}

		public void HandleRespawn()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			if (!(((Component)this).get_transform().get_position().y < despawnHeight))
			{
				return;
			}
			RespawnRoot componentInParent = ((Component)this).GetComponentInParent<RespawnRoot>();
			if (respawn)
			{
				if ((Object)(object)componentInParent != (Object)null)
				{
					componentInParent.Respawn(Vector3.get_up() * respawnHeight);
				}
				else
				{
					Respawn(Vector3.get_up() * respawnHeight);
				}
			}
			else if ((Object)(object)componentInParent == (Object)null)
			{
				UpdateVisibility();
			}
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			Respawn(Vector3.get_zero());
		}

		public void Respawn()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			((Component)this).get_gameObject().GetComponent<IPreRespawn>()?.PreRespawn();
			Respawn(Vector3.get_up() * respawnHeight);
		}

		private bool IsBadObject()
		{
			for (int i = 0; i < mBadObjectNames.Length; i++)
			{
				if (((Object)body).get_name().Equals(mBadObjectNames[i]))
				{
					return true;
				}
			}
			return false;
		}

		public void Respawn(Vector3 offset)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			if (ReplayRecorder.isPlaying || NetGame.isClient)
			{
				return;
			}
			GrabManager.Release(((Component)this).get_gameObject());
			Vector3 val = startPos + offset;
			Quaternion val2 = startRot;
			if (overrides != null)
			{
				RespawningBodyOverride respawningBodyOverride = null;
				for (int i = 0; i < overrides.Count; i++)
				{
					if (overrides[i].checkpoint.number <= Game.instance.currentCheckpointNumber && ((Object)(object)respawningBodyOverride == (Object)null || overrides[i].checkpoint.number > respawningBodyOverride.checkpoint.number))
					{
						respawningBodyOverride = overrides[i];
					}
				}
				if ((Object)(object)respawningBodyOverride != (Object)null)
				{
					val = ((Matrix4x4)(ref respawningBodyOverride.initialToNewLocationMatrtix)).MultiplyPoint3x4(val);
					val2 = respawningBodyOverride.initialToNewLocationRotation * val2;
				}
			}
			HandlingReset = true;
			mJustReset = 5;
			mResetPosition = val;
			mResetRotation = val2;
			((Component)this).get_transform().set_position(val);
			((Component)this).get_transform().set_rotation(val2);
			if (syncLocalScale)
			{
				((Component)this).get_transform().set_localScale(startLocalScale);
			}
			UpdateVisibility();
			if (m_respawnEvent != null)
			{
				m_respawnEvent.Invoke();
			}
			else
			{
				Debug.LogError((object)"Null respawn event.", (Object)(object)this);
			}
		}

		private float Max(float dist, Vector3 p)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = p - ((Component)this).get_transform().get_position();
			return Mathf.Max(dist, ((Vector3)(ref val)).get_magnitude());
		}

		public float FurthestPointDistance()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			Collider[] componentsInChildren = ((Component)this).GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (!((Object)(object)((Component)componentsInChildren[i]).GetComponentInParent<NetBody>() != (Object)(object)this))
				{
					Bounds bounds = componentsInChildren[i].get_bounds();
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_min().x, ((Bounds)(ref bounds)).get_min().y, ((Bounds)(ref bounds)).get_min().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_min().x, ((Bounds)(ref bounds)).get_min().y, ((Bounds)(ref bounds)).get_max().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_min().x, ((Bounds)(ref bounds)).get_max().y, ((Bounds)(ref bounds)).get_min().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_min().x, ((Bounds)(ref bounds)).get_max().y, ((Bounds)(ref bounds)).get_max().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_max().x, ((Bounds)(ref bounds)).get_min().y, ((Bounds)(ref bounds)).get_min().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_max().x, ((Bounds)(ref bounds)).get_min().y, ((Bounds)(ref bounds)).get_max().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_max().x, ((Bounds)(ref bounds)).get_max().y, ((Bounds)(ref bounds)).get_min().z));
					num = Max(num, new Vector3(((Bounds)(ref bounds)).get_max().x, ((Bounds)(ref bounds)).get_max().y, ((Bounds)(ref bounds)).get_max().z));
				}
			}
			return num;
		}

		public void CalculatePrecision(out float posMeter, out float rotDeg, out float rotMeter)
		{
			float num = posRange;
			ushort num2 = posfull;
			ushort num3 = rotfull;
			if (syncPosition == NetBodySyncPosition.Relative)
			{
				num = 10f;
				num2 = (ushort)(num2 - 6);
			}
			if (syncPosition == NetBodySyncPosition.Local)
			{
				num = 10f;
				num2 = (ushort)(num2 - 6);
			}
			if (posRangeOverride != 0f)
			{
				num = posRangeOverride;
			}
			num2 = (ushort)(num2 + posPrecision);
			num3 = (ushort)(num3 + rotPrecision);
			posMeter = num / (float)(1 << num2 - 1);
			rotDeg = 81f / (float)(1 << num3 - 2);
			if (syncRotation == NetBodySyncRotation.EulerX || syncRotation == NetBodySyncRotation.EulerY || syncRotation == NetBodySyncRotation.EulerZ)
			{
				rotDeg = 360f / (float)(1 << eulerEncoder.fullBits + rotPrecision);
			}
			rotMeter = Mathf.Sin(rotDeg * ((float)Math.PI / 180f)) * FurthestPointDistance();
			if (syncPosition == NetBodySyncPosition.None)
			{
				posMeter = 0.003f;
			}
			if (syncRotation == NetBodySyncRotation.None)
			{
				rotMeter = 0.003f;
			}
		}

		public NetBody()
			: this()
		{
		}
	}
}
