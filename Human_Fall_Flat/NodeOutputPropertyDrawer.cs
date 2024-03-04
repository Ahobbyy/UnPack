using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NodeOutput))]
public class NodeOutputPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		label = EditorGUI.BeginProperty(position, label, property);
		GUIContent obj = label;
		obj.set_text(obj.get_text() + " >");
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID((FocusType)2), label);
		int indentLevel = EditorGUI.get_indentLevel();
		EditorGUI.set_indentLevel(0);
		EditorGUI.PropertyField(new Rect(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width(), ((Rect)(ref position)).get_height()), property.FindPropertyRelative("initialValue"), GUIContent.none);
		EditorGUI.set_indentLevel(indentLevel);
		EditorGUI.EndProperty();
	}

	public NodeOutputPropertyDrawer()
		: this()
	{
	}
}
