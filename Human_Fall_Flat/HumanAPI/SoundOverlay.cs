using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class SoundOverlay
	{
		private List<string> expandedGroups = new List<string>();

		public SoundManagerType type;

		private Vector2 soundScrollPos;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private SoundManager.SoundState clipboard;

		public void OnGUI()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			SoundManager soundManager = null;
			soundManager = type switch
			{
				SoundManagerType.Level => SoundManager.level, 
				SoundManagerType.Menu => SoundManager.menu, 
				SoundManagerType.Character => SoundManager.character, 
				_ => null, 
			};
			if (!((Object)(object)soundManager == (Object)null))
			{
				if (GUILayout.Button("Save Sounds", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
				{
					soundManager.Save(saveMaster: true);
				}
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
				soundScrollPos = GUILayout.BeginScrollView(soundScrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width((float)Screen.get_width()) });
				ShowSounds(soundManager);
				GUILayout.EndScrollView();
				GUILayout.EndHorizontal();
			}
		}

		private void ShowSounds(SoundManager activeSounds)
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0374: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_053c: Unknown result type (might be due to invalid IL or missing references)
			//IL_059f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)activeSounds == (Object)null)
			{
				return;
			}
			for (int i = 0; i < activeSounds.groups.Count; i++)
			{
				SoundManager.SoundGroup soundGroup = activeSounds.groups[i];
				string text = soundGroup.name;
				if (string.IsNullOrEmpty(text))
				{
					text = "none";
				}
				bool flag = expandedGroups.Contains(text);
				if (flag != GUILayout.Toggle(flag, $"{text} ({soundGroup.sounds.Count})", (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					if (!flag)
					{
						expandedGroups.Add(text);
					}
					else
					{
						expandedGroups.Remove(text);
					}
				}
				if (!flag)
				{
					continue;
				}
				for (int j = 0; j < soundGroup.sounds.Count; j++)
				{
					Rect rect = GUILayoutUtility.GetRect((float)Screen.get_width(), 0f);
					((Rect)(ref rect)).set_height(24f);
					if (j % 2 == 0)
					{
						AudioUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f, 0.5f));
					}
					else
					{
						AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
					}
					GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect)).get_height()) });
					SoundManager.SoundMaster soundMaster = soundGroup.sounds[j];
					string name = ((Object)soundMaster.master).get_name();
					bool flag2 = SoundManager.hasSolo && !soundMaster.isMuted;
					if (flag2 != GUILayout.Toggle(flag2, name, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
					{
						SoundManager.SoloSound(soundMaster, !flag2);
					}
					if (GUILayout.Button(soundMaster.master.sampleLabel, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
					{
						SamplePicker.pickSampleSound = soundMaster;
						if (soundMaster.master.soundSample != null)
						{
							SamplePicker.pickCategoryName = soundMaster.master.soundSample.category;
						}
					}
					GUILayout.Label("lvl", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 1f, soundMaster.master.baseVolume, AudioSliderType.Volume);
					if (num != soundMaster.master.baseVolume)
					{
						soundMaster.SetBaseVolume(num);
					}
					GUILayout.Label("tune", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num2 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), -2400f, 2400f, 0f, soundMaster.master.basePitch, AudioSliderType.Pitch);
					if (num2 != soundMaster.master.basePitch)
					{
						soundMaster.SetBasePitch(num2);
					}
					GUILayout.Label("dist", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num3 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(100f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 10f, 300f, 30f, soundMaster.master.maxDistance, AudioSliderType.Log2);
					if (num3 != soundMaster.master.maxDistance)
					{
						soundMaster.SetMaxDistance(num3);
						soundMaster.ApplyAttenuation();
					}
					GUILayout.Label("lp", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num4 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 1f, 30f, 2f, soundMaster.master.lpStart, AudioSliderType.Log2);
					if (num4 != soundMaster.master.lpStart)
					{
						soundMaster.SetLpStart(num4);
						soundMaster.ApplyAttenuation();
					}
					float num5 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0.1f, 1f, 0.5f, soundMaster.master.lpPower, AudioSliderType.Linear);
					if (num5 != soundMaster.master.lpPower)
					{
						soundMaster.SetLpPower(num5);
						soundMaster.ApplyAttenuation();
					}
					GUILayout.Label("att", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num6 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(75f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 1f, 30f, 1f, soundMaster.master.falloffStart, AudioSliderType.Log2);
					if (num6 != soundMaster.master.falloffStart)
					{
						soundMaster.SetFalloffStart(num6);
						soundMaster.ApplyAttenuation();
					}
					float num7 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 0.5f, soundMaster.master.falloffPower, AudioSliderType.Linear);
					if (num7 != soundMaster.master.falloffPower)
					{
						soundMaster.SetFalloffPower(num7);
						soundMaster.ApplyAttenuation();
					}
					GUILayout.Label("spread", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num8 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 0.5f, soundMaster.master.spreadNear, AudioSliderType.Linear);
					if (num8 != soundMaster.master.spreadNear)
					{
						soundMaster.SetSpreadNear(num8);
						soundMaster.ApplyAttenuation();
					}
					float num9 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 0f, soundMaster.master.spreadFar, AudioSliderType.Linear);
					if (num9 != soundMaster.master.spreadFar)
					{
						soundMaster.SetSpreadFar(num9);
						soundMaster.ApplyAttenuation();
					}
					GUILayout.Label("3d", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					float num10 = AudioUI.DrawHorizontalSlider(GUILayoutUtility.GetRect(50f, ((Rect)(ref rect)).get_height(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), 0f, 1f, 0.5f, soundMaster.master.spatialNear, AudioSliderType.Linear);
					if (num10 != soundMaster.master.spatialNear)
					{
						soundMaster.SetSpatialNear(num10);
						soundMaster.ApplyAttenuation();
					}
					if (GUILayout.Button("C", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }))
					{
						clipboard = SoundManager.Serialize(soundMaster);
					}
					if (GUILayout.Button("V", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }) && clipboard != null)
					{
						SoundManager.Deserialize(soundMaster, clipboard, pasteSample: false);
					}
					if (GUILayout.Button("S", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(32f) }) && clipboard != null)
					{
						SoundManager.Deserialize(soundMaster, clipboard, pasteSample: true);
					}
					bool flag3 = SoundManager.main.state.GetSoundState(soundMaster.master.fullName) != null;
					if (!flag3 && !soundMaster.master.useMaster && GUILayout.Button("Create Master", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }))
					{
						soundMaster.SetUseMaster(useMaster: true);
					}
					else if ((flag3 || soundMaster.master.useMaster) && soundMaster.master.useMaster != GUILayout.Toggle(soundMaster.master.useMaster, "Master", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }))
					{
						soundMaster.SetUseMaster(!soundMaster.master.useMaster);
						if (soundMaster.master.useMaster)
						{
							SoundManager.SoundState soundState = SoundManager.main.state.GetSoundState(soundMaster.master.fullName);
							if (soundState != null)
							{
								SoundManager.Deserialize(soundMaster, soundState, pasteSample: true);
							}
							SoundManager.GrainState grainState = SoundManager.main.state.GetGrainState(soundMaster.master.fullName);
							if (grainState != null)
							{
								SoundManager.Deserialize(soundMaster, grainState);
							}
						}
					}
					GUILayout.EndHorizontal();
				}
			}
		}
	}
}
