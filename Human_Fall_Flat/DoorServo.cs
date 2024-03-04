using UnityEngine;

public class DoorServo : MonoBehaviour
{
	public SignalBase triggerSignal;

	public ServoSound servoSound1;

	public ServoSound servoSound2;

	public Rigidbody leftDoor;

	public Rigidbody rightDoor;

	public float minWidth;

	public float maxWidth;

	public float maxTensionWidth = 0.2f;

	public float maxVelocity = 1f;

	private ConfigurableJoint leftJoint;

	private ConfigurableJoint rightJoint;

	private Vector3 leftStart;

	private Vector3 rightStart;

	public float targetWidth;

	public float currentWidth;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		leftStart = leftDoor.get_position();
		rightStart = rightDoor.get_position();
		targetWidth = Mathf.Lerp(minWidth, maxWidth, triggerSignal.value);
		leftJoint = ((Component)leftDoor).GetComponent<ConfigurableJoint>();
		rightJoint = ((Component)rightDoor).GetComponent<ConfigurableJoint>();
		leftJoint.set_targetPosition(new Vector3((0f - targetWidth) / 2f, 0f, 0f));
		rightJoint.set_targetPosition(new Vector3((0f - targetWidth) / 2f, 0f, 0f));
	}

	private void FixedUpdate()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		float num = targetWidth;
		float num2 = Mathf.Lerp(minWidth, maxWidth, triggerSignal.value);
		targetWidth = Mathf.MoveTowards(targetWidth, num2, maxVelocity * Time.get_fixedDeltaTime());
		Vector3 val = leftDoor.get_position() - rightDoor.get_position();
		currentWidth = ((Vector3)(ref val)).get_magnitude();
		targetWidth = Mathf.MoveTowards(currentWidth, targetWidth, maxTensionWidth);
		float num3 = targetWidth;
		if (num != targetWidth)
		{
			Vector3 val2 = default(Vector3);
			((Vector3)(ref val2))._002Ector((0f - num3) / 2f, 0f, 0f);
			if (leftJoint.get_targetPosition() != val2)
			{
				leftJoint.set_targetPosition(new Vector3((0f - num3) / 2f, 0f, 0f));
				leftDoor.WakeUp();
			}
			if (rightJoint.get_targetPosition() != val2)
			{
				rightJoint.set_targetPosition(new Vector3((0f - num3) / 2f, 0f, 0f));
				rightDoor.WakeUp();
			}
			if ((Object)(object)servoSound1 != (Object)null && !servoSound1.isPlaying)
			{
				servoSound1.Play();
			}
			if ((Object)(object)servoSound2 != (Object)null && !servoSound2.isPlaying)
			{
				servoSound2.Play();
			}
		}
		else
		{
			if ((Object)(object)servoSound1 != (Object)null && servoSound1.isPlaying)
			{
				servoSound1.Stop();
			}
			if ((Object)(object)servoSound2 != (Object)null && servoSound2.isPlaying)
			{
				servoSound2.Stop();
			}
		}
	}

	public DoorServo()
		: this()
	{
	}
}
