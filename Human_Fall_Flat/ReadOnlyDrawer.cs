using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return EditorGUI.GetPropertyHeight(property, label, true);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		GUI.set_enabled(false);
		EditorGUI.PropertyField(position, property, label, true);
		GUI.set_enabled(true);
	}

	public ReadOnlyDrawer()
		: this()
	{
	}
}
