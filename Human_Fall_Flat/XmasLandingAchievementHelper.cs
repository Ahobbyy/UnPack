using UnityEngine;

public class XmasLandingAchievementHelper : MonoBehaviour
{
	public XmasLandAchievement achiScript;

	private void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			achiScript.SetJumping(((Component)((Component)other).get_transform().get_parent()).get_gameObject());
		}
		if ((Object)(object)((Component)other).GetComponentInParent<SnowBoard>() != (Object)null)
		{
			achiScript.SetJumpingWithSnowBoard();
		}
	}

	public XmasLandingAchievementHelper()
		: this()
	{
	}
}
