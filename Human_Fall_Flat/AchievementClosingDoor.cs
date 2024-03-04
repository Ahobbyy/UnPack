using HumanAPI;

public class AchievementClosingDoor : Node
{
	private NodeInput inTriggerVolume;

	public ServoMotorNew motor;

	public float speedThreshold = 2f;

	public override void Process()
	{
		if (inTriggerVolume.value >= 0.5f && motor.Speed > speedThreshold)
		{
			_ = motor.input.value;
			_ = 0f;
		}
	}
}
