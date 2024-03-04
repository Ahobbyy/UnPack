using UnityEngine;

namespace HumanAPI
{
	public class ReverbOverlay
	{
		private Reverb activeReverb;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private Vector2 soundScrollPos;

		private Reverb.ReverbZoneState clipboard;

		public void OnGUI()
		{
			if ((Object)(object)activeReverb == (Object)null)
			{
				activeReverb = Object.FindObjectOfType<Reverb>();
			}
			if (!((Object)(object)activeReverb == (Object)null))
			{
				if (GUILayout.Button("Save Reverb", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					activeReverb.Save();
				}
				ShowReverbs();
			}
		}

		private void ShowReverbs()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			soundScrollPos = GUILayout.BeginScrollView(soundScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width((float)Screen.get_width()) });
			for (int i = 0; i < activeReverb.zones.Length; i++)
			{
				ReverbZone reverbZone = activeReverb.zones[i];
				Rect rect = GUILayoutUtility.GetRect((float)Screen.get_width(), 0f);
				((Rect)(ref rect)).set_height(24f);
				if (ReverbManager.instance.zones.Contains(reverbZone))
				{
					AudioUI.DrawRect(rect, new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				else if (i % 2 == 0)
				{
					AudioUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
				}
				else
				{
					AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
				}
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect)).get_height()) });
				GUILayout.Label(((Object)reverbZone).get_name(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) });
				GUILayout.Label("level", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				Rect rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				reverbZone.level = AudioUI.DrawHorizontalSlider(rect2, -48f, 12f, 0f, reverbZone.level, AudioSliderType.Level);
				GUILayout.Label("delay", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				reverbZone.delay = AudioUI.DrawHorizontalSlider(rect2, 0.1f, 3f, 0.5f, reverbZone.delay, AudioSliderType.Linear);
				GUILayout.Label("diffusion", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				reverbZone.diffusion = AudioUI.DrawHorizontalSlider(rect2, 0.01f, 1f, 0.5f, reverbZone.diffusion, AudioSliderType.Linear);
				GUILayout.Label("HP", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				reverbZone.highPass = AudioUI.DrawHorizontalSlider(rect2, 10f, 22000f, 10f, reverbZone.highPass, AudioSliderType.Log10);
				GUILayout.Label("LP", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect2 = GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				reverbZone.lowPass = AudioUI.DrawHorizontalSlider(rect2, 10f, 22000f, 22000f, reverbZone.lowPass, AudioSliderType.Log10);
				if (GUILayout.Button("C", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }))
				{
					clipboard = Reverb.Serialize(reverbZone);
				}
				if (GUILayout.Button("V", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }) && clipboard != null)
				{
					Reverb.Deserialize(reverbZone, clipboard);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
	}
}
