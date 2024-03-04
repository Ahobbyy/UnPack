using System;
using UnityEngine;

public class SnowboardAchievement : MonoBehaviour
{
	private class SnowboardData
	{
		public Human h0;

		public Human h1;

		public bool passedFirstCheckpoint;
	}

	public static SnowboardAchievement instance;

	public SnowBoard[] snowboards;

	private SnowboardData[] snowboardData;

	private void Start()
	{
		instance = this;
		snowboardData = new SnowboardData[snowboards.Length];
		for (int i = 0; i < snowboardData.Length; i++)
		{
			snowboardData[i] = new SnowboardData();
		}
	}

	public void RegisterAttach(SnowBoard snowboard, Human human)
	{
		int num = Array.IndexOf(snowboards, snowboard);
		if (num >= 0)
		{
			SnowboardData snowboardData = this.snowboardData[num];
			if ((Object)(object)snowboardData.h0 == (Object)null)
			{
				snowboardData.h0 = human;
				snowboardData.passedFirstCheckpoint = false;
			}
			else if ((Object)(object)snowboardData.h1 == (Object)null)
			{
				snowboardData.h1 = human;
				snowboardData.passedFirstCheckpoint = false;
			}
			else
			{
				Debug.LogWarning((object)"Tried to attach a third human to a snowboard");
			}
		}
		else
		{
			Debug.LogWarning((object)"Snowboard hasn't been registered in SnowboardAchievement object");
		}
	}

	public void RegisterDetach(SnowBoard snowboard, Human human)
	{
		int num = Array.IndexOf(snowboards, snowboard);
		if (num >= 0)
		{
			SnowboardData snowboardData = this.snowboardData[num];
			if ((Object)(object)snowboardData.h0 == (Object)(object)human)
			{
				snowboardData.h0 = null;
				snowboardData.passedFirstCheckpoint = false;
			}
			else if ((Object)(object)snowboardData.h1 == (Object)(object)human)
			{
				snowboardData.h1 = null;
				snowboardData.passedFirstCheckpoint = false;
			}
			else
			{
				Debug.LogWarning((object)"Tried to detach a human that hadn't been attached to a snowboard");
			}
		}
		else
		{
			Debug.LogWarning((object)"Snowboard hasn't been registered in SnowboardAchievement object");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		SnowBoard componentInParent = ((Component)other).GetComponentInParent<SnowBoard>();
		if (!((Object)(object)componentInParent != (Object)null))
		{
			return;
		}
		int num = Array.IndexOf(snowboards, componentInParent);
		if (num >= 0)
		{
			SnowboardData data = snowboardData[num];
			Human human;
			if ((Object)(object)data.h0 != (Object)null && (Object)(object)data.h1 == (Object)null)
			{
				human = data.h0;
			}
			else
			{
				if (!((Object)(object)data.h1 != (Object)null) || !((Object)(object)data.h0 == (Object)null))
				{
					return;
				}
				human = data.h1;
			}
			if (Array.FindIndex(snowboardData, (SnowboardData d) => d != data && ((Object)(object)d.h0 == (Object)(object)human || (Object)(object)d.h1 == (Object)(object)human)) >= 0)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_ICE_TRICKY);
			}
		}
		else
		{
			Debug.LogWarning((object)"Snowboard hasn't been registered in SnowboardAchievement object");
		}
	}

	private void Update()
	{
	}

	public SnowboardAchievement()
		: this()
	{
	}
}
