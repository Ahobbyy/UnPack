using HumanAPI;
using UnityEngine;

public class WaterMotor : Node, IReset
{
	public NodeInput input;

	public SignalBase triggerSignal;

	public float force;

	public float forceLinear;

	public float forceSquare;

	public Transform forcePoint;

	public ServoSound servoSound;

	public Sound2 motorSound;

	public float pitchHigh = 1f;

	public float pitchLow = 1f;

	private FloatingMesh floatingmesh;

	private Rigidbody rigidbody;

	private void Awake()
	{
		if ((Object)(object)floatingmesh == (Object)null)
		{
			floatingmesh = ((Component)this).GetComponentInParent<FloatingMesh>();
		}
		if ((Object)(object)rigidbody == (Object)null)
		{
			rigidbody = ((Component)this).GetComponentInParent<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		num = ((!((Object)(object)triggerSignal != (Object)null)) ? input.value : triggerSignal.value);
		if ((Object)(object)motorSound != (Object)null)
		{
			if (num != 0f && !motorSound.isPlaying)
			{
				motorSound.Play();
				char choice = (((Object)(object)floatingmesh.sensor.waterBody == (Object)null) ? 'B' : 'A');
				motorSound.Switch(choice);
			}
			else if (num == 0f && motorSound.isPlaying)
			{
				motorSound.Stop();
			}
		}
		else if ((Object)(object)servoSound != (Object)null)
		{
			if (num != 0f && !servoSound.isPlaying)
			{
				servoSound.Play();
				bool flag = (Object)(object)floatingmesh.sensor.waterBody == (Object)null;
				servoSound.secondMedium = flag && servoSound.loopClips2.Length != 0;
			}
			else if (num == 0f && servoSound.isPlaying)
			{
				servoSound.Stop();
			}
		}
		Rigidbody val = rigidbody;
		Vector3 velocity = val.get_velocity();
		float magnitude = ((Vector3)(ref velocity)).get_magnitude();
		if (num != 0f)
		{
			val.SafeAddForceAtPosition(forcePoint.get_forward() * (force + forceLinear * magnitude + forceSquare * magnitude * magnitude) * num, forcePoint.get_position(), (ForceMode)0);
		}
		if ((Object)(object)motorSound != (Object)null)
		{
			char choice2 = (((Object)(object)floatingmesh.sensor.waterBody == (Object)null) ? 'B' : 'A');
			motorSound.Switch(choice2);
			motorSound.SetPitch(Mathf.Lerp(pitchLow, pitchHigh, Mathf.Abs(num)));
		}
		else
		{
			if (!((Object)(object)servoSound != (Object)null))
			{
				return;
			}
			bool flag2 = (Object)(object)floatingmesh.sensor.waterBody == (Object)null && servoSound.loopClips2.Length != 0;
			if (servoSound.secondMedium != flag2)
			{
				servoSound.secondMedium = flag2;
				if (num != 0f)
				{
					servoSound.CrossfadeLoop();
				}
			}
			servoSound.SetPitch(Mathf.Lerp(pitchLow, pitchHigh, Mathf.Abs(num)));
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if ((Object)(object)servoSound != (Object)null && servoSound.isPlaying)
		{
			servoSound.Stop();
		}
	}
}
