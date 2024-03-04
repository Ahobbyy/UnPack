using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_SpriteAsset))]
	public class TMP_SpriteAssetEditor : Editor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct UI_PanelState
		{
			public static bool spriteAssetInfoPanel = true;

			public static bool fallbackSpriteAssetPanel = true;

			public static bool spriteInfoPanel = false;
		}

		private int m_moveToIndex;

		private int m_selectedElement = -1;

		private int m_page;

		private const string k_UndoRedo = "UndoRedoPerformed";

		private string m_searchPattern;

		private List<int> m_searchList;

		private bool m_isSearchDirty;

		private SerializedProperty m_spriteAtlas_prop;

		private SerializedProperty m_material_prop;

		private SerializedProperty m_spriteInfoList_prop;

		private ReorderableList m_fallbackSpriteAssetList;

		private bool isAssetDirty;

		private string[] uiStateLabel = new string[2] { "<i>(Click to expand)</i>", "<i>(Click to collapse)</i>" };

		private float m_xOffset;

		private float m_yOffset;

		private float m_xAdvance;

		private float m_scale;

		public void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			m_spriteAtlas_prop = ((Editor)this).get_serializedObject().FindProperty("spriteSheet");
			m_material_prop = ((Editor)this).get_serializedObject().FindProperty("material");
			m_spriteInfoList_prop = ((Editor)this).get_serializedObject().FindProperty("spriteInfoList");
			m_fallbackSpriteAssetList = new ReorderableList(((Editor)this).get_serializedObject(), ((Editor)this).get_serializedObject().FindProperty("fallbackSpriteAssets"), true, true, true, true);
			m_fallbackSpriteAssetList.drawElementCallback = (ElementCallbackDelegate)delegate(Rect rect, int index, bool isActive, bool isFocused)
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				SerializedProperty arrayElementAtIndex = m_fallbackSpriteAssetList.get_serializedProperty().GetArrayElementAtIndex(index);
				((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + 2f);
				EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), EditorGUIUtility.get_singleLineHeight()), arrayElementAtIndex, GUIContent.none);
			};
			ReorderableList fallbackSpriteAssetList = m_fallbackSpriteAssetList;
			object obj = _003C_003Ec._003C_003E9__18_1;
			if (obj == null)
			{
				HeaderCallbackDelegate val = delegate(Rect rect)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					EditorGUI.LabelField(rect, "<b>Fallback Sprite Asset List</b>", TMP_UIStyleManager.Label);
				};
				obj = (object)val;
				_003C_003Ec._003C_003E9__18_1 = val;
			}
			fallbackSpriteAssetList.drawHeaderCallback = (HeaderCallbackDelegate)obj;
			TMP_UIStyleManager.GetUIStyles();
		}

		public override void OnInspectorGUI()
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Expected O, but got Unknown
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_038c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0432: Unknown result type (might be due to invalid IL or missing references)
			//IL_0437: Unknown result type (might be due to invalid IL or missing references)
			//IL_0453: Unknown result type (might be due to invalid IL or missing references)
			//IL_0458: Unknown result type (might be due to invalid IL or missing references)
			//IL_047c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_055e: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0717: Unknown result type (might be due to invalid IL or missing references)
			//IL_0721: Unknown result type (might be due to invalid IL or missing references)
			//IL_0731: Expected O, but got Unknown
			//IL_077f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0789: Unknown result type (might be due to invalid IL or missing references)
			//IL_0799: Expected O, but got Unknown
			//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Expected O, but got Unknown
			//IL_084f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0859: Unknown result type (might be due to invalid IL or missing references)
			//IL_0869: Expected O, but got Unknown
			//IL_08cb: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			string commandName = current.get_commandName();
			((Editor)this).get_serializedObject().Update();
			EditorGUIUtility.set_labelWidth(135f);
			GUILayout.Label("<b>TextMeshPro - Sprite Asset</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Sprite Info", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(1);
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(m_spriteAtlas_prop, new GUIContent("Sprite Atlas"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				Object objectReferenceValue = m_spriteAtlas_prop.get_objectReferenceValue();
				Texture2D val = (Texture2D)(object)((objectReferenceValue is Texture2D) ? objectReferenceValue : null);
				if ((Object)(object)val != (Object)null)
				{
					Object objectReferenceValue2 = m_material_prop.get_objectReferenceValue();
					Material val2 = (Material)(object)((objectReferenceValue2 is Material) ? objectReferenceValue2 : null);
					if ((Object)(object)val2 != (Object)null)
					{
						val2.set_mainTexture((Texture)(object)val);
					}
				}
			}
			GUI.set_enabled(true);
			EditorGUILayout.PropertyField(m_material_prop, new GUIContent("Default Material"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("Fallback Sprite Assets\t" + (UI_PanelState.fallbackSpriteAssetPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.fallbackSpriteAssetPanel = !UI_PanelState.fallbackSpriteAssetPanel;
			}
			if (UI_PanelState.fallbackSpriteAssetPanel)
			{
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(0);
				GUILayout.Label("Select the Sprite Assets that will be searched and used as fallback when a given sprite is missing from this sprite asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				m_fallbackSpriteAssetList.DoLayoutList();
				EditorGUILayout.EndVertical();
			}
			GUI.set_enabled(true);
			GUILayout.Space(10f);
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("Sprite List\t\t" + (UI_PanelState.spriteInfoPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.spriteInfoPanel = !UI_PanelState.spriteInfoPanel;
			}
			if (UI_PanelState.spriteInfoPanel)
			{
				int num = m_spriteInfoList_prop.get_arraySize();
				int num2 = 10;
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUIUtility.set_labelWidth(110f);
				EditorGUI.BeginChangeCheck();
				string text = EditorGUILayout.TextField("Sprite Search", m_searchPattern, GUIStyle.op_Implicit("SearchTextField"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck() || m_isSearchDirty)
				{
					if (!string.IsNullOrEmpty(text))
					{
						m_searchPattern = text.ToLower(CultureInfo.InvariantCulture).Trim();
						SearchGlyphTable(m_searchPattern, ref m_searchList);
					}
					m_isSearchDirty = false;
				}
				string text2 = (string.IsNullOrEmpty(m_searchPattern) ? "SearchCancelButtonEmpty" : "SearchCancelButton");
				if (GUILayout.Button(GUIContent.none, GUIStyle.op_Implicit(text2), (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					GUIUtility.set_keyboardControl(0);
					m_searchPattern = string.Empty;
				}
				EditorGUILayout.EndHorizontal();
				if (!string.IsNullOrEmpty(m_searchPattern))
				{
					num = m_searchList.Count;
				}
				DisplayGlyphPageNavigation(num, num2);
				EditorGUILayout.EndVertical();
				if (num > 0)
				{
					Rect val3 = default(Rect);
					for (int i = num2 * m_page; i < num && i < num2 * (m_page + 1); i++)
					{
						Rect rect = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						int num3 = i;
						if (!string.IsNullOrEmpty(m_searchPattern))
						{
							num3 = m_searchList[i];
						}
						SerializedProperty arrayElementAtIndex = m_spriteInfoList_prop.GetArrayElementAtIndex(num3);
						EditorGUI.BeginDisabledGroup(i != m_selectedElement);
						EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(75f) });
						EditorGUILayout.PropertyField(arrayElementAtIndex, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.EndVertical();
						EditorGUI.EndDisabledGroup();
						Rect rect2 = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						((Rect)(ref val3))._002Ector(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect2)).get_width(), ((Rect)(ref rect2)).get_y() - ((Rect)(ref rect)).get_y());
						if (DoSelectionCheck(val3))
						{
							m_selectedElement = i;
							GUIUtility.set_keyboardControl(0);
						}
						if (m_selectedElement == i)
						{
							TMP_EditorUtility.DrawBox(val3, 2f, Color32.op_Implicit(new Color32((byte)40, (byte)192, byte.MaxValue, byte.MaxValue)));
							Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.get_singleLineHeight() * 1f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
							((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 8f);
							bool enabled = GUI.get_enabled();
							if (i == 0)
							{
								GUI.set_enabled(false);
							}
							if (GUI.Button(controlRect, "Up"))
							{
								SwapSpriteElement(i, i - 1);
							}
							GUI.set_enabled(enabled);
							((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
							if (i == num - 1)
							{
								GUI.set_enabled(false);
							}
							if (GUI.Button(controlRect, "Down"))
							{
								SwapSpriteElement(i, i + 1);
							}
							GUI.set_enabled(enabled);
							((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width() * 2f);
							m_moveToIndex = EditorGUI.IntField(controlRect, m_moveToIndex);
							((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() - ((Rect)(ref controlRect)).get_width());
							if (GUI.Button(controlRect, "Goto"))
							{
								MoveSpriteElement(i, m_moveToIndex);
							}
							GUI.set_enabled(enabled);
							((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width() * 4f);
							if (GUI.Button(controlRect, "+"))
							{
								SerializedProperty spriteInfoList_prop = m_spriteInfoList_prop;
								spriteInfoList_prop.set_arraySize(spriteInfoList_prop.get_arraySize() + 1);
								int num4 = m_spriteInfoList_prop.get_arraySize() - 1;
								SerializedProperty target = m_spriteInfoList_prop.GetArrayElementAtIndex(num4);
								CopySerializedProperty(m_spriteInfoList_prop.GetArrayElementAtIndex(num3), ref target);
								target.FindPropertyRelative("id").set_intValue(num4);
								((Editor)this).get_serializedObject().ApplyModifiedProperties();
								m_isSearchDirty = true;
							}
							((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
							if (m_selectedElement == -1)
							{
								GUI.set_enabled(false);
							}
							if (GUI.Button(controlRect, "-"))
							{
								m_spriteInfoList_prop.DeleteArrayElementAtIndex(num3);
								m_selectedElement = -1;
								((Editor)this).get_serializedObject().ApplyModifiedProperties();
								m_isSearchDirty = true;
								return;
							}
						}
					}
				}
				DisplayGlyphPageNavigation(num, num2);
				EditorGUIUtility.set_labelWidth(40f);
				EditorGUIUtility.set_fieldWidth(20f);
				GUILayout.Space(5f);
				GUI.set_enabled(true);
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				Rect controlRect2 = EditorGUILayout.GetControlRect(false, 40f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				float num5 = (((Rect)(ref controlRect2)).get_width() - 75f) / 4f;
				EditorGUI.LabelField(controlRect2, "Global Offsets & Scale", EditorStyles.get_boldLabel());
				((Rect)(ref controlRect2)).set_x(((Rect)(ref controlRect2)).get_x() + 70f);
				bool changed = GUI.get_changed();
				GUI.set_changed(false);
				m_xOffset = EditorGUI.FloatField(new Rect(((Rect)(ref controlRect2)).get_x() + 5f + num5 * 0f, ((Rect)(ref controlRect2)).get_y() + 20f, num5 - 5f, 18f), new GUIContent("OX:"), m_xOffset);
				if (GUI.get_changed())
				{
					UpdateGlobalProperty("xOffset", m_xOffset);
				}
				m_yOffset = EditorGUI.FloatField(new Rect(((Rect)(ref controlRect2)).get_x() + 5f + num5 * 1f, ((Rect)(ref controlRect2)).get_y() + 20f, num5 - 5f, 18f), new GUIContent("OY:"), m_yOffset);
				if (GUI.get_changed())
				{
					UpdateGlobalProperty("yOffset", m_yOffset);
				}
				m_xAdvance = EditorGUI.FloatField(new Rect(((Rect)(ref controlRect2)).get_x() + 5f + num5 * 2f, ((Rect)(ref controlRect2)).get_y() + 20f, num5 - 5f, 18f), new GUIContent("ADV."), m_xAdvance);
				if (GUI.get_changed())
				{
					UpdateGlobalProperty("xAdvance", m_xAdvance);
				}
				m_scale = EditorGUI.FloatField(new Rect(((Rect)(ref controlRect2)).get_x() + 5f + num5 * 3f, ((Rect)(ref controlRect2)).get_y() + 20f, num5 - 5f, 18f), new GUIContent("SF."), m_scale);
				if (GUI.get_changed())
				{
					UpdateGlobalProperty("scale", m_scale);
				}
				EditorGUILayout.EndVertical();
				GUI.set_changed(changed);
			}
			if (((Editor)this).get_serializedObject().ApplyModifiedProperties() || commandName == "UndoRedoPerformed" || isAssetDirty)
			{
				isAssetDirty = false;
				EditorUtility.SetDirty(((Editor)this).get_target());
			}
			GUI.set_enabled(true);
			if ((int)current.get_type() == 0 && current.get_button() == 0)
			{
				m_selectedElement = -1;
			}
		}

		private void DisplayGlyphPageNavigation(int arraySize, int itemsPerPage)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 3f);
			int num = ((!Event.get_current().get_shift()) ? 1 : 10);
			GUI.set_enabled(m_page > 0);
			if (GUI.Button(controlRect, "Previous Page"))
			{
				m_page -= num;
			}
			GUIStyle val = new GUIStyle(GUI.get_skin().get_button());
			val.get_normal().set_background((Texture2D)null);
			GUIStyle val2 = val;
			GUI.set_enabled(true);
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			GUI.Button(controlRect, string.Concat(str3: ((int)((float)arraySize / (float)itemsPerPage + 0.999f)).ToString(), str0: "Page ", str1: (m_page + 1).ToString(), str2: " / "), val2);
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			GUI.set_enabled(itemsPerPage * (m_page + 1) < arraySize);
			if (GUI.Button(controlRect, "Next Page"))
			{
				m_page += num;
			}
			m_page = Mathf.Clamp(m_page, 0, arraySize / itemsPerPage);
			GUI.set_enabled(true);
		}

		private void UpdateGlobalProperty(string property, float value)
		{
			int arraySize = m_spriteInfoList_prop.get_arraySize();
			for (int i = 0; i < arraySize; i++)
			{
				m_spriteInfoList_prop.GetArrayElementAtIndex(i).FindPropertyRelative(property).set_floatValue(value);
			}
			GUI.set_changed(false);
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

		private void SwapSpriteElement(int selectedIndex, int newIndex)
		{
			m_spriteInfoList_prop.MoveArrayElement(selectedIndex, newIndex);
			m_spriteInfoList_prop.GetArrayElementAtIndex(selectedIndex).FindPropertyRelative("id").set_intValue(selectedIndex);
			m_spriteInfoList_prop.GetArrayElementAtIndex(newIndex).FindPropertyRelative("id").set_intValue(newIndex);
			m_selectedElement = newIndex;
			m_isSearchDirty = true;
		}

		private void MoveSpriteElement(int selectedIndex, int newIndex)
		{
			m_spriteInfoList_prop.MoveArrayElement(selectedIndex, newIndex);
			for (int i = 0; i < m_spriteInfoList_prop.get_arraySize(); i++)
			{
				m_spriteInfoList_prop.GetArrayElementAtIndex(i).FindPropertyRelative("id").set_intValue(i);
			}
			m_selectedElement = newIndex;
			m_isSearchDirty = true;
		}

		private void CopySerializedProperty(SerializedProperty source, ref SerializedProperty target)
		{
			target.FindPropertyRelative("name").set_stringValue(source.FindPropertyRelative("name").get_stringValue());
			target.FindPropertyRelative("hashCode").set_intValue(source.FindPropertyRelative("hashCode").get_intValue());
			target.FindPropertyRelative("x").set_floatValue(source.FindPropertyRelative("x").get_floatValue());
			target.FindPropertyRelative("y").set_floatValue(source.FindPropertyRelative("y").get_floatValue());
			target.FindPropertyRelative("width").set_floatValue(source.FindPropertyRelative("width").get_floatValue());
			target.FindPropertyRelative("height").set_floatValue(source.FindPropertyRelative("height").get_floatValue());
			target.FindPropertyRelative("xOffset").set_floatValue(source.FindPropertyRelative("xOffset").get_floatValue());
			target.FindPropertyRelative("yOffset").set_floatValue(source.FindPropertyRelative("yOffset").get_floatValue());
			target.FindPropertyRelative("xAdvance").set_floatValue(source.FindPropertyRelative("xAdvance").get_floatValue());
			target.FindPropertyRelative("scale").set_floatValue(source.FindPropertyRelative("scale").get_floatValue());
			target.FindPropertyRelative("sprite").set_objectReferenceValue(source.FindPropertyRelative("sprite").get_objectReferenceValue());
		}

		private void SearchGlyphTable(string searchPattern, ref List<int> searchResults)
		{
			if (searchResults == null)
			{
				searchResults = new List<int>();
			}
			searchResults.Clear();
			int arraySize = m_spriteInfoList_prop.get_arraySize();
			for (int i = 0; i < arraySize; i++)
			{
				SerializedProperty arrayElementAtIndex = m_spriteInfoList_prop.GetArrayElementAtIndex(i);
				if (arrayElementAtIndex.FindPropertyRelative("id").get_intValue().ToString()
					.Contains(searchPattern))
				{
					searchResults.Add(i);
				}
				if (arrayElementAtIndex.FindPropertyRelative("name").get_stringValue().ToLower(CultureInfo.InvariantCulture)
					.Trim()
					.Contains(searchPattern))
				{
					searchResults.Add(i);
				}
			}
		}

		public TMP_SpriteAssetEditor()
			: this()
		{
		}
	}
}
