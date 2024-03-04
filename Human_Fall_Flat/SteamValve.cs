using HumanAPI;
using UnityEngine;

public class SteamValve : Node
{
	public NodeInput input;

	private bool isOn;

	public float threshold = 0.95f;

	public float burstDuration = 3f;

	public float rate;

	public Sound2 whistleSound;

	public Sound2 hissSound;

	private ParticleSystem particles;

	private float timer;

	private float remainingBurstTime;

	private float sinceLastEmit;

	protected override void OnEnable()
	{
		base.OnEnable();
		isOn = Mathf.Abs(input.value) >= 0.5f;
		particles = ((Component)this).GetComponent<ParticleSystem>();
	}

	public override void Process()
	{
		base.Process();
		bool flag = Mathf.Abs(input.value) >= threshold;
		if (flag == isOn)
		{
			return;
		}
		isOn = flag;
		if (isOn)
		{
			remainingBurstTime = burstDuration;
			if ((Object)(object)whistleSound != (Object)null)
			{
				whistleSound.PlayOneShot();
			}
		}
		if ((Object)(object)hissSound != (Object)null)
		{
			if (isOn && !hissSound.isPlaying)
			{
				hissSound.Play(forceLoop: true);
			}
			if (!isOn && hissSound.isPlaying)
			{
				hissSound.Stop();
			}
		}
	}

	public void Update()
	{
		if (!(remainingBurstTime <= 0f))
		{
			remainingBurstTime -= Time.get_deltaTime();
			sinceLastEmit += Time.get_deltaTime();
			if (sinceLastEmit > 1f / rate)
			{
				sinceLastEmit -= 1f / rate;
				Emit();
			}
		}
	}

	public void Emit()
	{
		particles.Emit(1);
	}
}
