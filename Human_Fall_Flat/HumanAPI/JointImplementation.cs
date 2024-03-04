using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public abstract class JointImplementation : JointBase, INetBehavior
	{
		public Transform body;

		public Transform anchor;

		public bool useTransformAnchor;

		public bool enableCollision;

		[NonSerialized]
		public float centerValue;

		protected Rigidbody rigid;

		protected Rigidbody anchorRigid;

		protected Transform anchorTransform;

		protected Quaternion relativeRotation;

		protected Vector3 relativePosition;

		protected ConfigurableJoint joint;

		internal bool syncNetBody = true;

		private bool initialized;

		public int precision;

		private NetFloat encoder;

		public bool jointCreated => (Object)(object)joint != (Object)null;

		private void Awake()
		{
			EnsureInitialized();
		}

		public override void EnsureInitialized()
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			if (initialized)
			{
				return;
			}
			initialized = true;
			useTension = spring == 0f && damper == 0f;
			tensionDist = Mathf.Max(tensionDist, maxSpeed * Time.get_fixedDeltaTime());
			rigid = ((Component)body).GetComponent<Rigidbody>();
			_isKinematic = (Object)(object)rigid == (Object)null || rigid.get_isKinematic();
			relativeRotation = body.get_rotation();
			relativePosition = body.get_position();
			if ((Object)(object)anchor != (Object)null)
			{
				if (useTransformAnchor)
				{
					anchorTransform = anchor;
				}
				else
				{
					anchorRigid = ((Component)anchor).GetComponentInParent<Rigidbody>();
					anchorTransform = (((Object)(object)anchorRigid != (Object)null) ? ((Component)anchorRigid).get_transform() : anchor);
				}
				relativePosition = anchorTransform.InverseTransformPoint(relativePosition);
				relativeRotation = Quaternion.Inverse(anchorTransform.get_rotation()) * relativeRotation;
			}
			CreateMainJoint();
		}

		public abstract void CreateMainJoint();

		public void DestroyMainJoint()
		{
			if ((Object)(object)joint != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)joint);
				joint = null;
			}
		}

		public override float GetTarget()
		{
			return target;
		}

		public override void SetTarget(float target)
		{
			base.target = target;
		}

		public virtual void ResetState(int checkpoint, int subObjectives)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)joint == (Object)null)
			{
				Vector3 position = relativePosition;
				Quaternion val = relativeRotation;
				if ((Object)(object)anchorTransform != (Object)null)
				{
					val = anchorTransform.get_rotation() * val;
					position = anchorTransform.TransformPoint(relativePosition);
				}
				body.set_position(position);
				body.set_rotation(val);
				CreateMainJoint();
			}
			else
			{
				SetValue(0f);
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
			encoder = new NetFloat(1f, (ushort)(12 + precision), (ushort)(3 + precision), (ushort)(8 + precision));
		}

		public void SetMaster(bool isMaster)
		{
		}

		public virtual void CollectState(NetStream stream)
		{
			if (syncNetBody && useLimits)
			{
				encoder.CollectState(stream, Mathf.InverseLerp(minValue, maxValue, GetValue()));
			}
		}

		public virtual void ApplyState(NetStream state)
		{
			if (syncNetBody && useLimits)
			{
				float value = Mathf.Lerp(minValue, maxValue, encoder.ApplyState(state));
				SetValue(value);
			}
		}

		public virtual void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			if (syncNetBody && useLimits)
			{
				float value = Mathf.Lerp(minValue, maxValue, encoder.ApplyLerpedState(state0, state1, mix));
				SetValue(value);
			}
		}

		public virtual void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (syncNetBody && useLimits)
			{
				encoder.CalculateDelta(state0, state1, delta);
			}
		}

		public virtual void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (syncNetBody && useLimits)
			{
				encoder.AddDelta(state0, delta, result);
			}
		}

		public virtual int CalculateMaxDeltaSizeInBits()
		{
			if (!syncNetBody || !useLimits)
			{
				return 0;
			}
			return encoder.CalculateMaxDeltaSizeInBits();
		}
	}
}
