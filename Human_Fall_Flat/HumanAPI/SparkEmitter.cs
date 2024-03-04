using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class SparkEmitter : Node
	{
		[Tooltip("Min number of particles per second")]
		public float maxRate;

		[Tooltip("The Max number of particles per second")]
		public float minRate;

		public NodeInput input;

		[Tooltip("Nummber of particles to spawn in a burst on the on signal")]
		public int burstOn;

		[Tooltip("number of particles to spawn in a burst on the off signal")]
		public int burstOff;

		public SignalBase burstSignal;

		private bool isOn;

		private float delay;

		private void Awake()
		{
			delay = Random.Range(1f / maxRate, 1f / minRate);
			if ((Object)(object)burstSignal != (Object)null)
			{
				burstSignal.onValueChanged += BurstSignal_onValueChanged;
				isOn = burstSignal.boolValue;
			}
		}

		public override void Process()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			bool flag = Mathf.Abs(input.value) >= 0.5f;
			if (isOn != flag)
			{
				isOn = flag;
				SparkPool.instance.Emit(isOn ? burstOn : burstOff, ((Component)this).get_transform().get_position());
			}
		}

		private void BurstSignal_onValueChanged(float obj)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (isOn != burstSignal.boolValue)
			{
				isOn = burstSignal.boolValue;
				SparkPool.instance.Emit(isOn ? burstOn : burstOff, ((Component)this).get_transform().get_position());
			}
		}

		private void Update()
		{
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			if (((Object)(object)burstSignal != (Object)null || input != null) && !isOn)
			{
				return;
			}
			delay -= Time.get_deltaTime();
			if (delay <= 0f)
			{
				float num = float.MaxValue;
				for (int i = 0; i < NetGame.instance.local.players.Count; i++)
				{
					Vector3 val = ((Component)NetGame.instance.local.players[i].human.ragdoll).get_transform().get_position() - ((Component)this).get_transform().get_position();
					num = Mathf.Min(((Vector3)(ref val)).get_magnitude(), num);
				}
				if (num < 30f)
				{
					SparkPool.instance.Emit(1, ((Component)this).get_transform().get_position());
				}
				delay = Random.Range(1f / maxRate, 1f / minRate);
			}
		}
	}
}
