using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	internal static class GlobalSettings
	{
		private static class Keys
		{
			internal const string trackballSensitivity = "PostProcessing.Trackball.Sensitivity";

			internal const string volumeGizmoColor = "PostProcessing.Volume.GizmoColor";

			internal const string currentChannelMixer = "PostProcessing.ChannelMixer.CurrentChannel";

			internal const string currentCurve = "PostProcessing.Curve.Current";
		}

		private static bool m_Loaded;

		private static float m_TrackballSensitivity;

		private static Color m_VolumeGizmoColor;

		private static int m_CurrentChannelMixer;

		private static int m_CurrentCurve;

		internal static float trackballSensitivity
		{
			get
			{
				return m_TrackballSensitivity;
			}
			set
			{
				TrySave(ref m_TrackballSensitivity, value, "PostProcessing.Trackball.Sensitivity");
			}
		}

		internal static Color volumeGizmoColor
		{
			get
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				return m_VolumeGizmoColor;
			}
			set
			{
				//IL_0005: Unknown result type (might be due to invalid IL or missing references)
				TrySave(ref m_VolumeGizmoColor, value, "PostProcessing.Volume.GizmoColor");
			}
		}

		internal static int currentChannelMixer
		{
			get
			{
				return m_CurrentChannelMixer;
			}
			set
			{
				TrySave(ref m_CurrentChannelMixer, value, "PostProcessing.ChannelMixer.CurrentChannel");
			}
		}

		internal static int currentCurve
		{
			get
			{
				return m_CurrentCurve;
			}
			set
			{
				TrySave(ref m_CurrentCurve, value, "PostProcessing.Curve.Current");
			}
		}

		static GlobalSettings()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			m_Loaded = false;
			m_TrackballSensitivity = 0.2f;
			m_VolumeGizmoColor = new Color(0.2f, 0.8f, 0.1f, 0.5f);
			m_CurrentChannelMixer = 0;
			m_CurrentCurve = 0;
			Load();
		}

		[PreferenceItem("Post-processing")]
		private static void PreferenceGUI()
		{
			OpenGUI();
		}

		private static void OpenGUI()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			if (!m_Loaded)
			{
				Load();
			}
			EditorGUILayout.Space();
			trackballSensitivity = EditorGUILayout.Slider("Trackballs Sensitivity", trackballSensitivity, 0.05f, 1f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			volumeGizmoColor = EditorGUILayout.ColorField("Volume Gizmo Color", volumeGizmoColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}

		private static void Load()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			m_TrackballSensitivity = EditorPrefs.GetFloat("PostProcessing.Trackball.Sensitivity", 0.2f);
			m_VolumeGizmoColor = GetColor("PostProcessing.Volume.GizmoColor", new Color(0.2f, 0.8f, 0.1f, 0.5f));
			m_CurrentChannelMixer = EditorPrefs.GetInt("PostProcessing.ChannelMixer.CurrentChannel", 0);
			m_CurrentCurve = EditorPrefs.GetInt("PostProcessing.Curve.Current", 0);
			m_Loaded = true;
		}

		private static Color GetColor(string key, Color defaultValue)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			return ColorUtilities.ToRGBA((uint)EditorPrefs.GetInt(key, (int)ColorUtilities.ToHex(defaultValue)));
		}

		private static void TrySave<T>(ref T field, T newValue, string key)
		{
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			if (!field.Equals(newValue))
			{
				if (typeof(T) == typeof(float))
				{
					EditorPrefs.SetFloat(key, (float)(object)newValue);
				}
				else if (typeof(T) == typeof(int))
				{
					EditorPrefs.SetInt(key, (int)(object)newValue);
				}
				else if (typeof(T) == typeof(bool))
				{
					EditorPrefs.SetBool(key, (bool)(object)newValue);
				}
				else if (typeof(T) == typeof(string))
				{
					EditorPrefs.SetString(key, (string)(object)newValue);
				}
				else if (typeof(T) == typeof(Color))
				{
					EditorPrefs.SetInt(key, (int)ColorUtilities.ToHex((Color)(object)newValue));
				}
				field = newValue;
			}
		}
	}
}
