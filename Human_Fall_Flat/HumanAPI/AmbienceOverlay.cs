using System;
using UnityEngine;

namespace HumanAPI
{
	public class AmbienceOverlay
	{
		private Ambience activeAmbience;

		private AmbienceSource[] sources;

		private AmbienceZone[] zones;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private Vector2 scrollPos;

		private void Rebuild()
		{
			activeAmbience = Object.FindObjectOfType<Ambience>();
			if (!((Object)(object)activeAmbience == (Object)null))
			{
				sources = ((Component)activeAmbience).get_gameObject().GetComponentsInChildren<AmbienceSource>();
				zones = ((Component)activeAmbience).get_gameObject().GetComponentsInChildren<AmbienceZone>();
			}
		}

		public void OnGUI()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_042b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0430: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0484: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_052d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0532: Unknown result type (might be due to invalid IL or missing references)
			//IL_0534: Unknown result type (might be due to invalid IL or missing references)
			//IL_079b: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)activeAmbience == (Object)null)
			{
				Rebuild();
			}
			if ((Object)(object)activeAmbience == (Object)null)
			{
				return;
			}
			if (GUILayout.Button("Save Ambience", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(200f) }))
			{
				activeAmbience.Save();
				SoundManager.level.Save(saveMaster: true);
			}
			scrollPos = GUILayout.BeginScrollView(scrollPos, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width((float)Screen.get_width()) });
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			for (int i = 0; i < sources.Length; i++)
			{
				Rect rect = GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				if (sources[i].baseVolume * sources[i].rtVolume > 0f)
				{
					AudioUI.DrawRect(rect, new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				else
				{
					AudioUI.DrawRect(rect, new Color(0f, 0f, 0f, 0.5f));
				}
				GUI.Label(rect, ((Object)sources[i]).get_name());
			}
			GUILayout.EndHorizontal();
			Rect rect2 = GUILayoutUtility.GetRect(600f, 0f);
			((Rect)(ref rect2)).set_height(20f);
			AudioUI.DrawRect(rect2, new Color(0.5f, 0.5f, 0.5f, 0.8f));
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect2)).get_height()) });
			GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			for (int j = 0; j < sources.Length; j++)
			{
				if (GUI.Button(GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }), sources[j].sampleLabel, AudioUI.buttonStyle))
				{
					SoundManager.SoundMaster soundMaster = (SamplePicker.pickSampleSound = SoundManager.level.GetMaster(sources[j]));
					if (soundMaster.master.soundSample != null)
					{
						SamplePicker.pickCategoryName = soundMaster.master.soundSample.category;
					}
				}
			}
			GUILayout.Label("", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(100f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Verb", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Music", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Ambience", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Effects", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Physics", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("Character", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.Label("AmbienceFx", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(75f),
				GUILayout.ExpandWidth(false)
			});
			GUILayout.EndHorizontal();
			for (int k = 0; k < zones.Length; k++)
			{
				rect2 = GUILayoutUtility.GetRect(600f, 0f);
				((Rect)(ref rect2)).set_height(20f);
				if (k % 2 == 0)
				{
					AudioUI.DrawRect(rect2, new Color(0.2f, 0.2f, 0.2f, 0.5f));
				}
				else
				{
					AudioUI.DrawRect(rect2, new Color(0f, 0f, 0f, 0.5f));
				}
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(((Rect)(ref rect2)).get_height()) });
				Rect rect3 = GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				bool num = (Object)(object)activeAmbience.activeZone == (Object)(object)zones[k];
				if (num)
				{
					AudioUI.DrawRect(rect3, new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				bool flag = num & ((Object)(object)activeAmbience.forcedZone != (Object)null);
				if (flag != GUI.Toggle(rect3, flag, ((Object)zones[k]).get_name()))
				{
					if (!flag)
					{
						activeAmbience.ForceZone(zones[k]);
					}
					else
					{
						activeAmbience.ForceZone(null);
					}
				}
				for (int l = 0; l < sources.Length; l++)
				{
					rect3 = GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					DrawSlider(rect3, zones[k], sources[l]);
				}
				rect3 = GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				GUI.Label(rect3, ((Object)zones[k]).get_name());
				DrawMixer(zones[k], (AmbienceZone zone) => zone.mainVerbLevel, delegate(AmbienceZone zone, float v)
				{
					zone.mainVerbLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.musicLevel, delegate(AmbienceZone zone, float v)
				{
					zone.musicLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.ambienceLevel, delegate(AmbienceZone zone, float v)
				{
					zone.ambienceLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.effectsLevel, delegate(AmbienceZone zone, float v)
				{
					zone.effectsLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.physicsLevel, delegate(AmbienceZone zone, float v)
				{
					zone.physicsLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.characterLevel, delegate(AmbienceZone zone, float v)
				{
					zone.characterLevel = v;
				});
				DrawMixer(zones[k], (AmbienceZone zone) => zone.ambienceFxLevel, delegate(AmbienceZone zone, float v)
				{
					zone.ambienceFxLevel = v;
				});
				GUILayout.Label("x", AudioUI.style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				rect3 = GUILayoutUtility.GetRect(100f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
				float num2 = AudioUI.DrawHorizontalSlider(rect3, 500f, 10000f, 3000f, zones[k].transitionDuration * 1000f, AudioSliderType.Log2);
				if (num2 != zones[k].transitionDuration * 1000f)
				{
					zones[k].transitionDuration = num2 / 1000f;
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
		}

		private string FormatNumber(float num, int len = 5)
		{
			if (float.IsInfinity(num))
			{
				return "-Inf";
			}
			return num.ToString("0.0");
		}

		private void DrawMixer(AmbienceZone zone, Func<AmbienceZone, float> get, Action<AmbienceZone, float> set)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(75f, 20f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
			float num = get(zone);
			float num2 = AudioUI.DrawHorizontalSlider(rect, -24f, 24f, 0f, get(zone), AudioSliderType.Level);
			if (num != num2)
			{
				set(zone, num2);
				if ((Object)(object)activeAmbience.activeZone != (Object)null)
				{
					activeAmbience.TransitionToZone(activeAmbience.activeZone, 0.1f);
				}
			}
		}

		private void DrawSlider(Rect rect, AmbienceZone zone, AmbienceSource source)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			float level = zone.GetLevel(source);
			float num = AudioUI.DrawHorizontalSlider(rect, 0f, 1f, 0f, level, AudioSliderType.Volume);
			if (level == num)
			{
				return;
			}
			if (zone.sources != null && zone.sources.Length != 0)
			{
				for (int i = 0; i < zone.sources.Length; i++)
				{
					if ((Object)(object)zone.sources[i] == (Object)(object)source)
					{
						if (num == 0f)
						{
							zone.sources[i] = zone.sources[zone.sources.Length - 1];
							zone.volumes[i] = zone.volumes[zone.sources.Length - 1];
							Array.Resize(ref zone.volumes, zone.sources.Length - 1);
							Array.Resize(ref zone.sources, zone.sources.Length - 1);
						}
						else
						{
							zone.volumes[i] = num;
						}
						if ((Object)(object)activeAmbience.activeZone != (Object)null)
						{
							activeAmbience.TransitionToZone(activeAmbience.activeZone, 0.1f);
						}
						return;
					}
				}
			}
			if (zone.sources != null)
			{
				Array.Resize(ref zone.volumes, zone.sources.Length + 1);
				Array.Resize(ref zone.sources, zone.sources.Length + 1);
			}
			else
			{
				zone.volumes = new float[1];
				zone.sources = new AmbienceSource[1];
			}
			zone.sources[zone.sources.Length - 1] = source;
			zone.volumes[zone.sources.Length - 1] = num;
			if ((Object)(object)activeAmbience.activeZone != (Object)null)
			{
				activeAmbience.TransitionToZone(activeAmbience.activeZone, 0.1f);
			}
		}
	}
}
