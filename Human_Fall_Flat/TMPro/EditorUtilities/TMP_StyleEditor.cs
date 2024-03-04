using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_StyleSheet))]
	[CanEditMultipleObjects]
	public class TMP_StyleEditor : Editor
	{
		private SerializedProperty m_styleList_prop;

		private int m_selectedElement = -1;

		private Rect m_selectionRect;

		private int m_page;

		private void OnEnable()
		{
			TMP_UIStyleManager.GetUIStyles();
			m_styleList_prop = ((Editor)this).get_serializedObject().FindProperty("m_StyleList");
		}

		public override void OnInspectorGUI()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			((Editor)this).get_serializedObject().Update();
			GUILayout.Label("<b>TextMeshPro - Style Sheet</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			int arraySize = m_styleList_prop.get_arraySize();
			int num = (Screen.get_height() - 178) / 111;
			if (arraySize > 0)
			{
				Rect selectionArea = default(Rect);
				for (int i = num * m_page; i < arraySize && i < num * (m_page + 1); i++)
				{
					if (m_selectedElement == i)
					{
						EditorGUI.DrawRect(m_selectionRect, Color32.op_Implicit(new Color32((byte)40, (byte)192, byte.MaxValue, byte.MaxValue)));
					}
					Rect rect = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
					EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					SerializedProperty arrayElementAtIndex = m_styleList_prop.GetArrayElementAtIndex(i);
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(arrayElementAtIndex, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					EditorGUILayout.EndVertical();
					EditorGUI.EndChangeCheck();
					Rect rect2 = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
					((Rect)(ref selectionArea))._002Ector(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect2)).get_width(), ((Rect)(ref rect2)).get_y() - ((Rect)(ref rect)).get_y());
					if (DoSelectionCheck(selectionArea))
					{
						m_selectedElement = i;
						m_selectionRect = new Rect(((Rect)(ref selectionArea)).get_x() - 2f, ((Rect)(ref selectionArea)).get_y() + 2f, ((Rect)(ref selectionArea)).get_width() + 4f, ((Rect)(ref selectionArea)).get_height() - 4f);
						((Editor)this).Repaint();
					}
				}
			}
			int num2 = ((!current.get_shift()) ? 1 : 10);
			GUILayout.Space(-3f);
			Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 6f);
			if (num == 0)
			{
				return;
			}
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width() * 4f);
			if (GUI.Button(controlRect, "+"))
			{
				SerializedProperty styleList_prop = m_styleList_prop;
				styleList_prop.set_arraySize(styleList_prop.get_arraySize() + 1);
				((Editor)this).get_serializedObject().ApplyModifiedProperties();
				TMP_StyleSheet.RefreshStyles();
			}
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			if (m_selectedElement == -1)
			{
				GUI.set_enabled(false);
			}
			if (GUI.Button(controlRect, "-"))
			{
				if (m_selectedElement != -1)
				{
					m_styleList_prop.DeleteArrayElementAtIndex(m_selectedElement);
				}
				m_selectedElement = -1;
				((Editor)this).get_serializedObject().ApplyModifiedProperties();
				TMP_StyleSheet.RefreshStyles();
			}
			GUILayout.Space(5f);
			controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 3f);
			if (m_page > 0)
			{
				GUI.set_enabled(true);
			}
			else
			{
				GUI.set_enabled(false);
			}
			if (GUI.Button(controlRect, "Previous"))
			{
				m_page -= num2;
			}
			GUI.set_enabled(true);
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			GUI.Label(controlRect, string.Concat(str3: ((int)((float)arraySize / (float)num + 0.999f)).ToString(), str0: "Page ", str1: (m_page + 1).ToString(), str2: " / "), GUI.get_skin().get_button());
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			if (num * (m_page + 1) < arraySize)
			{
				GUI.set_enabled(true);
			}
			else
			{
				GUI.set_enabled(false);
			}
			if (GUI.Button(controlRect, "Next"))
			{
				m_page += num2;
			}
			m_page = Mathf.Clamp(m_page, 0, arraySize / num);
			if (((Editor)this).get_serializedObject().ApplyModifiedProperties())
			{
				TMPro_EventManager.ON_TEXT_STYLE_PROPERTY_CHANGED(isChanged: true);
			}
			GUI.set_enabled(true);
			if ((int)current.get_type() == 0 && current.get_button() == 0)
			{
				m_selectedElement = -1;
			}
		}

		private bool DoSelectionCheck(Rect selectionArea)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			if ((int)current.get_type() == 0 && ((Rect)(ref selectionArea)).Contains(current.get_mousePosition()) && current.get_button() == 0)
			{
				current.Use();
				return true;
			}
			return false;
		}

		public TMP_StyleEditor()
			: this()
		{
		}
	}
}
