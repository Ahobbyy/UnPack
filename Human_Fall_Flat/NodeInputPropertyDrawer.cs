using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NodeInput))]
public class NodeInputPropertyDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		label = EditorGUI.BeginProperty(position, label, property);
		label.set_text("> " + label.get_text());
		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID((FocusType)2), label);
		int indentLevel = EditorGUI.get_indentLevel();
		EditorGUI.set_indentLevel(0);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width(), ((Rect)(ref position)).get_height());
		if (property.FindPropertyRelative("connectedNode").get_objectReferenceValue() == (Object)null)
		{
			EditorGUI.PropertyField(val, property.FindPropertyRelative("initialValue"), GUIContent.none);
		}
		else
		{
			EditorGUI.LabelField(val, string.Format("{0}.{1}", property.FindPropertyRelative("connectedNode").get_objectReferenceValue().get_name(), property.FindPropertyRelative("connectedSocket").get_stringValue()));
		}
		EditorGUI.set_indentLevel(indentLevel);
		EditorGUI.EndProperty();
	}

	public NodeInputPropertyDrawer()
		: this()
	{
	}
}
