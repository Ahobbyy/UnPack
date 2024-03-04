using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	[CustomEditor(typeof(Sound2))]
	[CanEditMultipleObjects]
	public class SoundEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			((Editor)this).DrawDefaultInspector();
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("AmbienceClose", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.AmbienceClose);
				}
			}
			if (GUILayout.Button("AmbienceMedium", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.AmbienceMedium);
				}
			}
			if (GUILayout.Button("AmbienceFar", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.AmbienceFar);
				}
			}
			GUILayout.EndVertical();
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("EffectsClose", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.EffectClose);
				}
			}
			if (GUILayout.Button("EffectMedium", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.EffectMedium);
				}
			}
			if (GUILayout.Button("EffectFar", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] targets = ((Editor)this).get_targets();
				for (int i = 0; i < targets.Length; i++)
				{
					((Sound2)(object)targets[i]).ApplyPreset(BusType.EffectFar);
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		public SoundEditor()
			: this()
		{
		}
	}
}
