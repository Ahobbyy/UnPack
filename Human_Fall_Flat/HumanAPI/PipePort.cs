using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class PipePort : SteamPort, IPostEndReset, IPostReset
	{
		private const float kDisconnectIfBelow = -40f;

		private const float kPipeSpringTolerance = 0.025f;

		public bool isMale;

		public bool connectable = true;

		[Tooltip("After triggering this checkpoint pipe will no longer reset to it's starting position")]
		public int saveCheckpoint = -1;

		public Collider pipeCollider;

		public PipePort startPipe;

		public NodeOutput leak;

		private float breakDelay = 3f;

		private float breakTreshold = 0.2f;

		private float tensionDistance = 0.1f;

		private float bendArm = 2f;

		private float springPull = 50000f;

		private float damperPull = 1000f;

		private float springBend = 5000f;

		private float damperBend = 500f;

		public static List<PipePort> malePipes = new List<PipePort>();

		public static List<PipePort> femalePipes = new List<PipePort>();

		private SpringJoint springIn;

		private SpringJoint springCenter;

		private bool isMaster;

		private PipePort connectedPipe;

		private PipePort ignorePipe;

		private float breakableIn;

		internal Rigidbody parentBody;

		private int order;

		public float tensionPhase;

		public float oldTensionPhase;

		public override bool isOpen => (Object)(object)connectedPipe == (Object)null;

		public override float ownPressure => 0f;

		public override SteamPort connectedPort => connectedPipe;

		public static List<PipePort> GetList(bool male)
		{
			if (!male)
			{
				return femalePipes;
			}
			return malePipes;
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			if (!connectable)
			{
				return;
			}
			if ((Object)(object)pipeCollider == (Object)null)
			{
				pipeCollider = ((Component)this).GetComponentInParent<Collider>();
				if ((Object)(object)pipeCollider == (Object)null)
				{
					Debug.LogError((object)"No collider for pipePort");
				}
			}
			GetList(isMale).Add(this);
			parentBody = ((Component)this).GetComponentInParent<Rigidbody>();
			order = ((Object)this).GetInstanceID();
			List<PipePort> list = GetList(!isMale);
			for (int i = 0; i < list.Count; i++)
			{
				Physics.IgnoreCollision(pipeCollider, list[i].pipeCollider);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if (connectable)
			{
				if (Object.op_Implicit((Object)(object)connectedPipe))
				{
					DisconnectPipe();
				}
				GetList(isMale).Remove(this);
			}
		}

		private PipePort Scan()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			float num = 0.5f;
			float num2 = 0.5f;
			Vector3 position = ((Component)this).get_transform().get_position();
			Vector3 val = -((Component)this).get_transform().get_forward();
			PipePort result = null;
			float num3 = 0f;
			List<PipePort> list = GetList(!isMale);
			for (int i = 0; i < list.Count; i++)
			{
				PipePort pipePort = list[i];
				Vector3 val2 = position - ((Component)pipePort).get_transform().get_position();
				float num4 = Mathf.InverseLerp(num, 0f, ((Vector3)(ref val2)).get_magnitude());
				float num5 = Mathf.InverseLerp(num2, 1f, Vector3.Dot(val, ((Component)pipePort).get_transform().get_forward()));
				float num6 = num4 * num5;
				if (num3 < num6)
				{
					num3 = num6;
					result = pipePort;
				}
			}
			if (!(num3 > 0.1f))
			{
				return null;
			}
			return result;
		}

		public void ScanAndConnect()
		{
			PipePort pipePort = Scan();
			if ((Object)(object)pipePort != (Object)null && (Object)(object)pipePort != (Object)(object)ignorePipe)
			{
				ConnectPipe(pipePort);
			}
		}

		private void FixedUpdate()
		{
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)parentBody == (Object)null || !connectable)
			{
				return;
			}
			bool flag = GrabManager.IsGrabbedAny(((Component)parentBody).get_gameObject());
			if (flag && (Object)(object)connectedPipe == (Object)null)
			{
				PipePort pipePort = Scan();
				if ((Object)(object)pipePort != (Object)null && (Object)(object)pipePort != (Object)(object)ignorePipe)
				{
					ConnectPipe(pipePort);
					return;
				}
			}
			if (!flag)
			{
				ignorePipe = null;
			}
			if (!((Object)(object)connectedPipe != (Object)null) || !isMaster)
			{
				return;
			}
			if (((Component)this).get_transform().get_position().y < -40f && (Object)(object)springIn != (Object)null)
			{
				DisconnectPipe();
				return;
			}
			if (breakableIn > 0f)
			{
				ApplyJointForce(loose: false, Apply: false);
				breakableIn -= Time.get_fixedDeltaTime();
				return;
			}
			bool loose = flag || ((Object)(object)connectedPipe.parentBody != (Object)null && GrabManager.IsGrabbedAny(((Component)connectedPipe.parentBody).get_gameObject()));
			ApplyJointForce(loose, Apply: false);
			Vector3 val = ((Component)this).get_transform().get_position() - ((Component)connectedPipe).get_transform().get_position();
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			if ((Object)(object)springIn != (Object)null && magnitude > breakTreshold)
			{
				DisconnectPipe();
			}
		}

		public void ApplyJointForce(bool loose, bool Apply)
		{
			tensionPhase = Mathf.MoveTowards(tensionPhase, (float)((!loose) ? 1 : 0), Time.get_fixedDeltaTime());
			if (Apply || oldTensionPhase != tensionPhase)
			{
				springIn.set_spring(Mathf.Lerp(springBend / 1000f, springBend, tensionPhase));
				springCenter.set_spring(Mathf.Lerp(springPull / 1000f, springPull, tensionPhase));
				springIn.set_damper(damperBend);
				springCenter.set_damper(damperPull);
			}
			oldTensionPhase = tensionPhase;
		}

		public void DisconnectPipe()
		{
			PipePort pipePort = connectedPipe;
			Object.Destroy((Object)(object)springIn);
			Object.Destroy((Object)(object)springCenter);
			connectedPipe.ignorePipe = this;
			ignorePipe = connectedPipe;
			connectedPipe = null;
			if ((Object)(object)pipePort.springIn != (Object)null)
			{
				Object.Destroy((Object)(object)pipePort.springIn);
				Object.Destroy((Object)(object)pipePort.springCenter);
			}
			pipePort.connectedPipe = null;
			SteamSystem.Recalculate(node);
			SteamSystem.Recalculate(pipePort.node);
		}

		public bool ConnectPipe(PipePort other)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			connectedPipe = other;
			other.connectedPipe = this;
			isMaster = true;
			connectedPipe.isMaster = false;
			springIn = Attach(other, ((Component)this).get_transform().get_position() + bendArm * ((Component)this).get_transform().get_forward(), ((Component)connectedPipe).get_transform().get_position() - (bendArm + tensionDistance) * ((Component)connectedPipe).get_transform().get_forward());
			springCenter = Attach(other, ((Component)this).get_transform().get_position(), ((Component)connectedPipe).get_transform().get_position());
			tensionPhase = 0.5f;
			ApplyJointForce(loose: false, Apply: true);
			SteamSystem.Recalculate(node);
			SteamSystem.Recalculate(other.node);
			breakableIn = breakDelay;
			other.breakableIn = breakDelay;
			return true;
		}

		private SpringJoint Attach(PipePort pipe, Vector3 anchor, Vector3 connectedAnchor)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)parentBody == (Object)null)
			{
				return null;
			}
			SpringJoint val = ((Component)parentBody).get_gameObject().AddComponent<SpringJoint>();
			((Joint)val).set_anchor(((Component)parentBody).get_transform().InverseTransformPoint(anchor));
			((Joint)val).set_autoConfigureConnectedAnchor(false);
			val.set_damper(0f);
			val.set_tolerance(0.025f);
			Rigidbody val2 = pipe.parentBody;
			if ((Object)(object)val2 != (Object)null)
			{
				((Joint)val).set_connectedBody(val2);
				((Joint)val).set_connectedAnchor(((Component)val2).get_transform().InverseTransformPoint(connectedAnchor));
				((Joint)val).set_enableCollision(true);
			}
			else
			{
				((Joint)val).set_connectedAnchor(connectedAnchor);
			}
			return val;
		}

		public void PostResetState(int checkpoint)
		{
			if ((saveCheckpoint <= 0 || checkpoint < saveCheckpoint) && (Object)(object)connectedPipe != (Object)null && (Object)(object)connectedPipe != (Object)(object)startPipe && (Object)(object)connectedPipe.startPipe != (Object)(object)this)
			{
				DisconnectPipe();
			}
		}

		public void PostEndResetState(int checkpoint)
		{
			if ((saveCheckpoint <= 0 || checkpoint < saveCheckpoint) && (Object)(object)startPipe != (Object)null && (Object)(object)connectedPipe == (Object)null && (Object)(object)parentBody != (Object)null && ((Component)this).get_gameObject().get_activeInHierarchy() && ((Component)startPipe).get_gameObject().get_activeInHierarchy())
			{
				ConnectPipe(startPipe);
			}
		}

		public override void ApplySystemState(bool isOpenSystem, float pressure)
		{
			if (isOpen)
			{
				leak.SetValue(pressure);
			}
			else
			{
				leak.SetValue(0f);
			}
		}
	}
}
