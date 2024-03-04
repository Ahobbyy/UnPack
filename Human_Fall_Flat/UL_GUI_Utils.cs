using UnityEngine;

public static class UL_GUI_Utils
{
	public static void Text(string text)
	{
		GUILayout.Label(text, GUI.get_skin().get_box(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
	}

	public static float Slider(string text, float value, float min, float max)
	{
		GUILayout.BeginHorizontal(GUI.get_skin().get_box(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("<b>" + text + "</b>", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
		GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(6f);
		value = GUILayout.HorizontalSlider(value, min, max, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		return value;
	}
}
