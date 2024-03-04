using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmasLandAchievement : MonoBehaviour
{
	public List<GameObject> playersJumping = new List<GameObject>();

	private bool haveSnowBoard;

	public void SetJumping(GameObject user)
	{
		((MonoBehaviour)this).StartCoroutine(Jumping(user));
	}

	public void SetJumpingWithSnowBoard()
	{
		((MonoBehaviour)this).StartCoroutine(WithSnowBoard());
	}

	private IEnumerator Jumping(GameObject user)
	{
		playersJumping.Add(user);
		for (int i = 0; i < 120; i++)
		{
			yield return null;
		}
		yield return null;
		playersJumping.Remove(user);
	}

	private IEnumerator WithSnowBoard()
	{
		haveSnowBoard = true;
		for (int i = 0; i < 120; i++)
		{
			yield return null;
		}
		yield return null;
		haveSnowBoard = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player" && playersJumping.Contains(((Component)((Component)other).get_transform().get_parent()).get_gameObject()) && haveSnowBoard)
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_XMAS_SKI_LAND);
		}
	}

	public XmasLandAchievement()
		: this()
	{
	}
}
