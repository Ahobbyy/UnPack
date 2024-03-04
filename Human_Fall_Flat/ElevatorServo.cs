using UnityEngine;

public class ElevatorServo : MonoBehaviour
{
	public ServoSound servoSound;

	public SignalBase triggerSignal;

	public float maxVelocity = 1f;

	public Vector3 offPos;

	public Vector3 onPos;

	private ConfigurableJoint joint;

	private Rigidbody body;

	private Vector3 startPos;

	private Vector3 targetPos;

	private void OnEnable()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		targetPos = (triggerSignal.boolValue ? onPos : offPos);
		body = ((Component)this).GetComponent<Rigidbody>();
		if (!body.get_isKinematic())
		{
			joint = ((Component)this).GetComponent<ConfigurableJoint>();
			joint.set_targetPosition(targetPos);
		}
		startPos = body.get_position();
	}

	private void FixedUpdate()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = targetPos;
		Vector3 val2 = (triggerSignal.boolValue ? onPos : offPos);
		targetPos = Vector3.MoveTowards(targetPos, val2, maxVelocity * Time.get_fixedDeltaTime());
		if (val != targetPos)
		{
			if (!body.get_isKinematic())
			{
				joint.set_targetPosition(targetPos);
			}
			else
			{
				body.MovePosition(startPos + ((Component)this).get_transform().get_parent().TransformVector(targetPos));
			}
			body.WakeUp();
			if ((Object)(object)servoSound != (Object)null && !servoSound.isPlaying)
			{
				servoSound.Play();
			}
		}
		else if ((Object)(object)servoSound != (Object)null && servoSound.isPlaying)
		{
			servoSound.Stop();
		}
	}

	public ElevatorServo()
		: this()
	{
	}
}
