using UnityEngine;

public class AchievementBigWheelVolume : MonoBehaviour
{
	public AchievementBigWheel achievementComponent;

	private void OnTriggerEnter(Collider other)
	{
		Human component = ((Component)other).GetComponent<Human>();
		if ((Object)(object)component != (Object)null)
		{
			achievementComponent.TriggerVolumeEntered(component);
		}
	}

	public AchievementBigWheelVolume()
		: this()
	{
	}
}
