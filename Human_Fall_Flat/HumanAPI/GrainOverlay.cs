using UnityEngine;

namespace HumanAPI
{
	public class GrainOverlay
	{
		private SoundManager activeSounds;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private Vector2 soundScrollPos;

		private float clipFrequency = 30f;

		private float clipVol = 1f;

		private float clipSlowVol = 1f;

		private float clipTune = 1f;

		private float clipSlowTune = 1f;

		private float clipSlowJitter = 5f;

		private float clipFastJitter;

		public void OnGUI()
		{
			activeSounds = (activeSounds = SoundManager.level);
			if (!((Object)(object)activeSounds == (Object)null))
			{
				if (GUILayout.Button("Save Sounds", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					activeSounds.Save(saveMaster: true);
				}
				ShowGrains();
			}
		}

		private void ShowGrains()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			soundScrollPos = GUILayout.BeginScrollView(soundScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width((float)Screen.get_width()) });
			int num = 0;
			for (int i = 0; i < activeSounds.sounds.Count; i++)
			{
				SoundManager.SoundMaster soundMaster = activeSounds.sounds[i];
				Grain grain = soundMaster.master as Grain;
				if ((Object)(object)grain == (Object)null)
				{
					continue;
				}
				Rect rect = GUILayoutUtility.GetRect((float)Screen.get_width(), 0f);
				((Rect)(ref rect)).set_height(24f);
				if (num % 2 == 0)
				{
					AudioUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
				}
				else
				{
					AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
				}
				num++;
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect)).get_height()) });
				GUILayout.Label(((Object)soundMaster.master).get_name(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) });
				if (GUILayout.Button(soundMaster.master.sampleLabel, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					SamplePicker.pickSampleSound = soundMaster;
					if (soundMaster.master.soundSample != null)
					{
						SamplePicker.pickCategoryName = soundMaster.master.soundSample.category;
					}
				}
				GUILayout.Label("freq", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num2 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0.5f, 50f, 30f, grain.frequencyAtMaxIntensity, AudioSliderType.Linear);
				if (num2 != grain.frequencyAtMaxIntensity)
				{
					soundMaster.SetGrainFrequency(num2);
				}
				GUILayout.Label("lvl", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num3 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 0f, soundMaster.master.baseVolume, AudioSliderType.Volume);
				if (num3 != soundMaster.master.baseVolume)
				{
					soundMaster.SetBaseVolume(num3);
				}
				GUILayout.Label("slow", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				num3 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 1f, grain.slowVolume, AudioSliderType.Volume);
				if (num3 != grain.slowVolume)
				{
					soundMaster.SetGrainSlowVolume(num3);
				}
				GUILayout.Label("tune", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num4 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), -2400f, 2400f, 1f, soundMaster.master.basePitch, AudioSliderType.Pitch);
				if (num4 != soundMaster.master.basePitch)
				{
					soundMaster.SetBasePitch(num4);
				}
				GUILayout.Label("slow", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				num4 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), -2400f, 2400f, 1f, grain.slowPitch, AudioSliderType.Pitch);
				if (num4 != grain.slowPitch)
				{
					soundMaster.SetGrainSlowTune(num4);
				}
				GUILayout.Label("jitter", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num5 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 10f, 0f, grain.fastJitter, AudioSliderType.Linear);
				if (num5 != grain.fastJitter)
				{
					soundMaster.SetGrainFastJitter(num5);
				}
				GUILayout.Label("slow", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				num5 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 10f, 5f, grain.slowJitter, AudioSliderType.Linear);
				if (num5 != grain.slowJitter)
				{
					soundMaster.SetGrainSlowJitter(num5);
				}
				if (GUILayout.Button("C", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }))
				{
					clipFrequency = grain.frequencyAtMaxIntensity;
					clipVol = grain.baseVolume;
					clipSlowVol = grain.slowVolume;
					clipTune = grain.basePitch;
					clipSlowTune = grain.slowPitch;
					clipFastJitter = grain.fastJitter;
					clipSlowJitter = grain.slowJitter;
				}
				if (GUILayout.Button("V", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }))
				{
					soundMaster.SetGrainFrequency(clipFrequency);
					soundMaster.SetBaseVolume(clipVol);
					soundMaster.SetGrainSlowVolume(clipSlowVol);
					soundMaster.SetBasePitch(clipTune);
					soundMaster.SetGrainSlowTune(clipSlowTune);
					soundMaster.SetGrainFastJitter(clipFastJitter);
					soundMaster.SetGrainSlowJitter(clipSlowJitter);
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}
	}
}
