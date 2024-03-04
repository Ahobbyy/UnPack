using System;
using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using Steamworks;
using UnityEngine;

public class Playtesting_WatermarkedHoodie : MonoBehaviour
{
	public Material hoodieMaterial;

	public List<string> streamerIds;

	public Texture[] numbers;

	public GameObject blackScreen;

	private string jsonSkin = "{\"type\":\"RagdollPreset\",\"workshopId\":0,\"title\":\"\",\"description\":\"\",\"main\":{\"modelPath\":\"builtin:LegacyBody\",\"color1\":\"00000000\",\"color2\":\"00000000\",\"color3\":\"00000000\"},\"head\":{\"modelPath\":\"builtin:HumanHardHat\",\"color1\":\"\",\"color2\":\"\",\"color3\":\"\"},\"upperBody\":{\"modelPath\":\"builtin:Hoodie\",\"color1\":\"00000000\",\"color2\":\"00000000\",\"color3\":\"00000000\"},\"lowerBody\":{\"modelPath\":\"\",\"color1\":\"\",\"color2\":\"\",\"color3\":\"\"}}";

	private void Start()
	{
		try
		{
			streamerIds = Readcsv();
			RagdollPresetMetadata preset = JsonUtility.FromJson<RagdollPresetMetadata>(jsonSkin);
			NetPlayer netPlayer = Object.FindObjectOfType<NetPlayer>();
			netPlayer.ApplyPreset(preset);
			((Renderer)((Component)((Component)netPlayer).get_transform().FindRecursive("HoodieSkinned")).GetComponent<SkinnedMeshRenderer>()).set_material(hoodieMaterial);
			string text = SteamUser.GetSteamID().ToString();
			if (streamerIds.Contains(text.ToLower()))
			{
				int num = streamerIds.IndexOf(text.ToLower()) + 1;
				hoodieMaterial.SetTexture("_NumberTex1", numbers[num / 10]);
				hoodieMaterial.SetTexture("_NumberTex2", numbers[num % 10]);
				blackScreen.SetActive(false);
			}
			else
			{
				blackScreen.SetActive(true);
			}
		}
		catch (Exception)
		{
			blackScreen.SetActive(true);
		}
	}

	private List<string> Readcsv()
	{
		Object obj = Resources.Load("StreamerIDs");
		List<string> list = new List<string>(((TextAsset)((obj is TextAsset) ? obj : null)).get_text().ToLower().Split('\n'));
		for (int num = list.Count - 1; num >= 0; num--)
		{
			list[num] = list[num].TrimEnd('\r');
		}
		return list;
	}

	public Playtesting_WatermarkedHoodie()
		: this()
	{
	}
}
