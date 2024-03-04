using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class HumanAnalytics : MonoBehaviour
{
	public static HumanAnalytics instance;

	private void OnEnable()
	{
		instance = this;
	}

	public void LoadLevel(string levelName, int levelNumber, int checkpoint, float timeSinceStart)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Analytics.CustomEvent("loadLevel", (IDictionary<string, object>)new Dictionary<string, object>
		{
			{ "level", levelName },
			{
				"cp",
				checkpoint.ToString()
			}
		});
	}

	public void GameOver()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Analytics.CustomEvent("gameComplete", (IDictionary<string, object>)new Dictionary<string, object>());
	}

	public void BeginMultiplayer(bool host)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Analytics.CustomEvent("multiplayer", (IDictionary<string, object>)new Dictionary<string, object> { { "isServer", host } });
	}

	public void LogVersion(string version)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Analytics.CustomEvent("version", (IDictionary<string, object>)new Dictionary<string, object> { { "version", version } });
	}

	public HumanAnalytics()
		: this()
	{
	}
}
