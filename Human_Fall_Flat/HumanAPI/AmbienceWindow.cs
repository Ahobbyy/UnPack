using System;
using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	public class AmbienceWindow : EditorWindow
	{
		private Ambience activeAmbience;

		private AmbienceSource[] sources;

		private AmbienceZone[] zones;

		private const int sliderWidth = 100;

		private const int sliderHeight = 20;

		private GUIStyle style;

		private Vector2 scrollPos;

		internal static void Init(Ambience ambience)
		{
			AmbienceWindow obj = EditorWindow.GetWindow(typeof(AmbienceWindow), false, "Ambience") as AmbienceWindow;
			obj.activeAmbience = ambience;
			obj.OnEnable();
		}

		private void OnEnable()
		{
			if ((Object)(object)activeAmbience != (Object)null)
			{
				Rebuild(activeAmbience);
			}
		}

		private void Rebuild(Ambience activeAmbience)
		{
			sources = ((Component)activeAmbience).get_gameObject().GetComponentsInChildren<AmbienceSource>();
			zones = ((Component)activeAmbience).get_gameObject().GetComponentsInChildren<AmbienceZone>();
		}

		private void OnGUI()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)activeAmbience == (Object)null)
			{
				return;
			}
			style = new GUIStyle(GUI.get_skin().GetStyle("label"));
			style.set_fontSize(10);
			style.get_normal().set_textColor(Color.get_white());
			style.set_alignment((TextAnchor)4);
			scrollPos = GUILayout.BeginScrollView(scrollPos, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			for (int i = 0; i < sources.Length; i++)
			{
				int num = (i + 1) * 100;
				if (EditorApplication.get_isPlaying() && sources[i].baseVolume > 0f)
				{
					EditorGUI.DrawRect(new Rect((float)num, 0f, 100f, 20f), new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				GUI.Label(new Rect((float)num, 0f, 100f, 20f), ((Object)sources[i]).get_name());
			}
			for (int j = 0; j < zones.Length; j++)
			{
				int num2 = (j + 1) * 20;
				if (EditorApplication.get_isPlaying() && (Object)(object)activeAmbience.activeZone == (Object)(object)zones[j])
				{
					EditorGUI.DrawRect(new Rect(0f, (float)num2, 100f, 20f), new Color(0.5f, 0.25f, 0.25f, 1f));
				}
				GUI.Label(new Rect(0f, (float)num2, 100f, 20f), ((Object)zones[j]).get_name());
				for (int k = 0; k < sources.Length; k++)
				{
					int x = (k + 1) * 100;
					DrawSlider(x, num2, zones[j], sources[k]);
				}
			}
			EditorGUILayout.EndScrollView();
			if (EditorApplication.get_isPlaying())
			{
				((EditorWindow)this).Repaint();
			}
		}

		private string FormatNumber(float num, int len = 5)
		{
			if (float.IsInfinity(num))
			{
				return "-Inf";
			}
			return num.ToString("0.0");
		}

		private void DrawSlider(int x, int y, AmbienceZone zone, AmbienceSource source)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			float level = zone.GetLevel(source);
			float num = Sliders.DrawHorizontalSlider(new Rect((float)x, (float)y, 100f, 20f), 0f, 1f, level, SliderType.Volume);
			if (level == num)
			{
				return;
			}
			Undo.RecordObject((Object)(object)zone, "changelevel");
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
						if (EditorApplication.get_isPlaying() && (Object)(object)activeAmbience.activeZone != (Object)null)
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
			if (EditorApplication.get_isPlaying() && (Object)(object)activeAmbience.activeZone != (Object)null)
			{
				activeAmbience.TransitionToZone(activeAmbience.activeZone, 0.1f);
			}
		}

		public AmbienceWindow()
			: this()
		{
		}
	}
}
