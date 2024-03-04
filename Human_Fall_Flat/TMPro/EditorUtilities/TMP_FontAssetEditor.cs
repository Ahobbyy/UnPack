using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_FontAsset))]
	public class TMP_FontAssetEditor : Editor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct UI_PanelState
		{
			public static bool fontInfoPanel = true;

			public static bool fontWeightPanel = true;

			public static bool fallbackFontAssetPanel = true;

			public static bool glyphInfoPanel = false;

			public static bool kerningInfoPanel = false;
		}

		private struct Warning
		{
			public bool isEnabled;

			public double expirationTime;
		}

		private int m_GlyphPage;

		private int m_KerningPage;

		private int m_selectedElement = -1;

		private string m_dstGlyphID;

		private const string k_placeholderUnicodeHex = "<i>Unicode Hex ID</i>";

		private string m_unicodeHexLabel = "<i>Unicode Hex ID</i>";

		private Warning m_AddGlyphWarning;

		private string m_searchPattern;

		private List<int> m_searchList;

		private bool m_isSearchDirty;

		private const string k_UndoRedo = "UndoRedoPerformed";

		private SerializedProperty font_atlas_prop;

		private SerializedProperty font_material_prop;

		private SerializedProperty fontWeights_prop;

		private ReorderableList m_list;

		private SerializedProperty font_normalStyle_prop;

		private SerializedProperty font_normalSpacing_prop;

		private SerializedProperty font_boldStyle_prop;

		private SerializedProperty font_boldSpacing_prop;

		private SerializedProperty font_italicStyle_prop;

		private SerializedProperty font_tabSize_prop;

		private SerializedProperty m_fontInfo_prop;

		private SerializedProperty m_glyphInfoList_prop;

		private SerializedProperty m_kerningInfo_prop;

		private KerningTable m_kerningTable;

		private SerializedProperty m_kerningPair_prop;

		private TMP_FontAsset m_fontAsset;

		private Material[] m_materialPresets;

		private bool isAssetDirty;

		private int errorCode;

		private DateTime timeStamp;

		private string[] uiStateLabel = new string[2] { "<i>(Click to expand)</i>", "<i>(Click to collapse)</i>" };

		public void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Expected O, but got Unknown
			font_atlas_prop = ((Editor)this).get_serializedObject().FindProperty("atlas");
			font_material_prop = ((Editor)this).get_serializedObject().FindProperty("material");
			fontWeights_prop = ((Editor)this).get_serializedObject().FindProperty("fontWeights");
			m_list = new ReorderableList(((Editor)this).get_serializedObject(), ((Editor)this).get_serializedObject().FindProperty("fallbackFontAssets"), true, true, true, true);
			m_list.drawElementCallback = (ElementCallbackDelegate)delegate(Rect rect, int index, bool isActive, bool isFocused)
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				SerializedProperty arrayElementAtIndex = m_list.get_serializedProperty().GetArrayElementAtIndex(index);
				((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + 2f);
				EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), EditorGUIUtility.get_singleLineHeight()), arrayElementAtIndex, GUIContent.none);
			};
			ReorderableList list = m_list;
			object obj = _003C_003Ec._003C_003E9__34_1;
			if (obj == null)
			{
				HeaderCallbackDelegate val = delegate(Rect rect)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					EditorGUI.LabelField(rect, "<b>Fallback Font Asset List</b>", TMP_UIStyleManager.Label);
				};
				obj = (object)val;
				_003C_003Ec._003C_003E9__34_1 = val;
			}
			list.drawHeaderCallback = (HeaderCallbackDelegate)obj;
			font_normalStyle_prop = ((Editor)this).get_serializedObject().FindProperty("normalStyle");
			font_normalSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("normalSpacingOffset");
			font_boldStyle_prop = ((Editor)this).get_serializedObject().FindProperty("boldStyle");
			font_boldSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("boldSpacing");
			font_italicStyle_prop = ((Editor)this).get_serializedObject().FindProperty("italicStyle");
			font_tabSize_prop = ((Editor)this).get_serializedObject().FindProperty("tabSize");
			m_fontInfo_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontInfo");
			m_glyphInfoList_prop = ((Editor)this).get_serializedObject().FindProperty("m_glyphInfoList");
			m_kerningInfo_prop = ((Editor)this).get_serializedObject().FindProperty("m_kerningInfo");
			m_kerningPair_prop = ((Editor)this).get_serializedObject().FindProperty("m_kerningPair");
			m_fontAsset = ((Editor)this).get_target() as TMP_FontAsset;
			m_kerningTable = m_fontAsset.kerningInfo;
			m_materialPresets = TMP_EditorUtility.FindMaterialReferences(m_fontAsset);
			TMP_UIStyleManager.GetUIStyles();
			m_searchList = new List<int>();
		}

		public override void OnInspectorGUI()
		{
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Expected O, but got Unknown
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Expected O, but got Unknown
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Expected O, but got Unknown
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Expected O, but got Unknown
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Expected O, but got Unknown
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Expected O, but got Unknown
			//IL_0431: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Expected O, but got Unknown
			//IL_045c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a8: Expected O, but got Unknown
			//IL_0529: Unknown result type (might be due to invalid IL or missing references)
			//IL_0546: Expected O, but got Unknown
			//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Expected O, but got Unknown
			//IL_0634: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Expected O, but got Unknown
			//IL_0690: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b1: Expected O, but got Unknown
			//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06ec: Expected O, but got Unknown
			//IL_0765: Unknown result type (might be due to invalid IL or missing references)
			//IL_082d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0839: Unknown result type (might be due to invalid IL or missing references)
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0952: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_09dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a62: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aea: Expected O, but got Unknown
			//IL_0b70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b9e: Expected O, but got Unknown
			//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dcf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ebb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
			//IL_118b: Unknown result type (might be due to invalid IL or missing references)
			Event current = Event.get_current();
			((Editor)this).get_serializedObject().Update();
			GUILayout.Label("<b>TextMesh Pro! Font Asset</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Face Info", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(1);
			GUI.set_enabled(false);
			EditorGUIUtility.set_labelWidth(150f);
			float labelWidth = 150f;
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Name"), new GUIContent("Font Source"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("PointSize"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(true);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Scale"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("LineHeight"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Ascender"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("CapHeight"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Baseline"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Descender"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Underline"), new GUIContent("Underline Offset"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("strikethrough"), new GUIContent("Strikethrough Offset"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("SuperscriptOffset"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("SubscriptOffset"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			SerializedProperty val = m_fontInfo_prop.FindPropertyRelative("SubSize");
			EditorGUILayout.PropertyField(val, new GUIContent("Super / Subscript Size"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			val.set_floatValue(Mathf.Clamp(val.get_floatValue(), 0.25f, 1f));
			GUI.set_enabled(false);
			EditorGUI.set_indentLevel(1);
			GUILayout.Space(18f);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("Padding"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("AtlasWidth"), new GUIContent("Width"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_fontInfo_prop.FindPropertyRelative("AtlasHeight"), new GUIContent("Height"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(true);
			EditorGUI.set_indentLevel(0);
			GUILayout.Space(20f);
			GUILayout.Label("Font Sub-Assets", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(false);
			EditorGUI.set_indentLevel(1);
			EditorGUILayout.PropertyField(font_atlas_prop, new GUIContent("Font Atlas:"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(font_material_prop, new GUIContent("Font Material:"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(true);
			string commandName = Event.get_current().get_commandName();
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("Font Weights\t" + (UI_PanelState.fontWeightPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.fontWeightPanel = !UI_PanelState.fontWeightPanel;
			}
			if (UI_PanelState.fontWeightPanel)
			{
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(0);
				GUILayout.Label("Select the Font Assets that will be used for the following font weights.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("<b>Font Weight</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(117f) });
				GUILayout.Label("<b>Normal Style</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("<b>Italic Style</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.PropertyField(fontWeights_prop.GetArrayElementAtIndex(4), new GUIContent("400 - Regular"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(fontWeights_prop.GetArrayElementAtIndex(7), new GUIContent("700 - Bold"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndVertical();
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Settings used to simulate a typeface when no font asset is available.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(5f);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(font_normalStyle_prop, new GUIContent("Normal Weight"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				font_normalStyle_prop.set_floatValue(Mathf.Clamp(font_normalStyle_prop.get_floatValue(), -3f, 3f));
				if (GUI.get_changed() || commandName == "UndoRedoPerformed")
				{
					GUI.set_changed(false);
					for (int i = 0; i < m_materialPresets.Length; i++)
					{
						m_materialPresets[i].SetFloat("_WeightNormal", font_normalStyle_prop.get_floatValue());
					}
				}
				EditorGUILayout.PropertyField(font_boldStyle_prop, new GUIContent("Bold Weight"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(100f) });
				font_boldStyle_prop.set_floatValue(Mathf.Clamp(font_boldStyle_prop.get_floatValue(), -3f, 3f));
				if (GUI.get_changed() || commandName == "UndoRedoPerformed")
				{
					GUI.set_changed(false);
					for (int j = 0; j < m_materialPresets.Length; j++)
					{
						m_materialPresets[j].SetFloat("_WeightBold", font_boldStyle_prop.get_floatValue());
					}
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(font_normalSpacing_prop, new GUIContent("Spacing Offset"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				font_normalSpacing_prop.set_floatValue(Mathf.Clamp(font_normalSpacing_prop.get_floatValue(), -100f, 100f));
				if (GUI.get_changed() || commandName == "UndoRedoPerformed")
				{
					GUI.set_changed(false);
				}
				EditorGUILayout.PropertyField(font_boldSpacing_prop, new GUIContent("Bold Spacing"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				font_boldSpacing_prop.set_floatValue(Mathf.Clamp(font_boldSpacing_prop.get_floatValue(), 0f, 100f));
				if (GUI.get_changed() || commandName == "UndoRedoPerformed")
				{
					GUI.set_changed(false);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(font_italicStyle_prop, new GUIContent("Italic Style: "), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				font_italicStyle_prop.set_intValue(Mathf.Clamp(font_italicStyle_prop.get_intValue(), 15, 60));
				EditorGUILayout.PropertyField(font_tabSize_prop, new GUIContent("Tab Multiple: "), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			}
			GUILayout.Space(5f);
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("Fallback Font Assets\t" + (UI_PanelState.fallbackFontAssetPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.fallbackFontAssetPanel = !UI_PanelState.fallbackFontAssetPanel;
			}
			if (UI_PanelState.fallbackFontAssetPanel)
			{
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(0);
				GUILayout.Label("Select the Font Assets that will be searched and used as fallback when characters are missing from this font asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				m_list.DoLayoutList();
				EditorGUILayout.EndVertical();
			}
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
			GUILayout.Space(5f);
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("Glyph Info\t" + (UI_PanelState.glyphInfoPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.glyphInfoPanel = !UI_PanelState.glyphInfoPanel;
			}
			if (UI_PanelState.glyphInfoPanel)
			{
				int num = m_glyphInfoList_prop.get_arraySize();
				int num2 = 15;
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUIUtility.set_labelWidth(110f);
				EditorGUI.BeginChangeCheck();
				string text = EditorGUILayout.TextField("Glyph Search", m_searchPattern, GUIStyle.op_Implicit("SearchTextField"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck() || m_isSearchDirty)
				{
					if (!string.IsNullOrEmpty(text))
					{
						m_searchPattern = text;
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
					Rect val2 = default(Rect);
					Rect val3 = default(Rect);
					for (int k = num2 * m_GlyphPage; k < num && k < num2 * (m_GlyphPage + 1); k++)
					{
						Rect rect = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						int num3 = k;
						if (!string.IsNullOrEmpty(m_searchPattern))
						{
							num3 = m_searchList[k];
						}
						SerializedProperty arrayElementAtIndex = m_glyphInfoList_prop.GetArrayElementAtIndex(num3);
						EditorGUI.BeginDisabledGroup(k != m_selectedElement);
						EditorGUILayout.BeginVertical(TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(arrayElementAtIndex, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.EndVertical();
						EditorGUI.EndDisabledGroup();
						Rect rect2 = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
						((Rect)(ref val2))._002Ector(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect2)).get_width(), ((Rect)(ref rect2)).get_y() - ((Rect)(ref rect)).get_y());
						if (DoSelectionCheck(val2))
						{
							m_selectedElement = k;
							m_AddGlyphWarning.isEnabled = false;
							m_unicodeHexLabel = "<i>Unicode Hex ID</i>";
							GUIUtility.set_keyboardControl(0);
						}
						if (m_selectedElement != k)
						{
							continue;
						}
						TMP_EditorUtility.DrawBox(val2, 2f, Color32.op_Implicit(new Color32((byte)40, (byte)192, byte.MaxValue, byte.MaxValue)));
						Rect controlRect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.get_singleLineHeight() * 1f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						float num4 = ((Rect)(ref controlRect)).get_width() * 0.6f / 3f;
						((Rect)(ref val3))._002Ector(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width() * 0.4f, ((Rect)(ref controlRect)).get_y(), num4, ((Rect)(ref controlRect)).get_height());
						GUI.set_enabled(!string.IsNullOrEmpty(m_dstGlyphID));
						if (GUI.Button(val3, new GUIContent("Copy to")))
						{
							GUIUtility.set_keyboardControl(0);
							int dstGlyphID = TMP_TextUtilities.StringToInt(m_dstGlyphID);
							if (!AddNewGlyph(num3, dstGlyphID))
							{
								m_AddGlyphWarning.isEnabled = true;
								m_AddGlyphWarning.expirationTime = EditorApplication.get_timeSinceStartup() + 1.0;
							}
							m_dstGlyphID = string.Empty;
							m_isSearchDirty = true;
							TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(isChanged: true, m_fontAsset);
						}
						GUI.set_enabled(true);
						((Rect)(ref val3)).set_x(((Rect)(ref val3)).get_x() + num4);
						GUI.SetNextControlName("GlyphID_Input");
						m_dstGlyphID = EditorGUI.TextField(val3, m_dstGlyphID);
						EditorGUI.LabelField(val3, new GUIContent(m_unicodeHexLabel, "The Unicode (Hex) ID of the duplicated Glyph"), TMP_UIStyleManager.Label);
						if (GUI.GetNameOfFocusedControl() == "GlyphID_Input")
						{
							m_unicodeHexLabel = string.Empty;
							char character = Event.get_current().get_character();
							if ((character < '0' || character > '9') && (character < 'a' || character > 'f') && (character < 'A' || character > 'F'))
							{
								Event.get_current().set_character('\0');
							}
						}
						else
						{
							m_unicodeHexLabel = "<i>Unicode Hex ID</i>";
						}
						((Rect)(ref val3)).set_x(((Rect)(ref val3)).get_x() + num4);
						if (GUI.Button(val3, "Remove"))
						{
							GUIUtility.set_keyboardControl(0);
							RemoveGlyphFromList(num3);
							m_selectedElement = -1;
							m_isSearchDirty = true;
							TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(isChanged: true, m_fontAsset);
							return;
						}
						if (m_AddGlyphWarning.isEnabled && EditorApplication.get_timeSinceStartup() < m_AddGlyphWarning.expirationTime)
						{
							EditorGUILayout.HelpBox("The Destination Glyph ID already exists", (MessageType)2);
						}
					}
				}
				DisplayGlyphPageNavigation(num, num2);
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Kerning Table Info\t" + (UI_PanelState.kerningInfoPanel ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				UI_PanelState.kerningInfoPanel = !UI_PanelState.kerningInfoPanel;
			}
			if (UI_PanelState.kerningInfoPanel)
			{
				SerializedProperty val4 = m_kerningInfo_prop.FindPropertyRelative("kerningPairs");
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Left Char", TMP_UIStyleManager.TMP_GUISkin.get_label(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Right Char", TMP_UIStyleManager.TMP_GUISkin.get_label(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Offset Value", TMP_UIStyleManager.TMP_GUISkin.get_label(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label(GUIContent.none, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) });
				EditorGUILayout.EndHorizontal();
				GUILayout.BeginVertical(TMP_UIStyleManager.TMP_GUISkin.get_label(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				int arraySize = val4.get_arraySize();
				int num5 = 25;
				Rect val5;
				if (arraySize > 0)
				{
					for (int l = num5 * m_KerningPage; l < arraySize && l < num5 * (m_KerningPage + 1); l++)
					{
						SerializedProperty arrayElementAtIndex2 = val4.GetArrayElementAtIndex(l);
						val5 = EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUI.PropertyField(new Rect(((Rect)(ref val5)).get_x(), ((Rect)(ref val5)).get_y(), ((Rect)(ref val5)).get_width() - 20f, ((Rect)(ref val5)).get_height()), arrayElementAtIndex2, GUIContent.none);
						if (GUILayout.Button("-", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) }))
						{
							m_kerningTable.RemoveKerningPair(l);
							m_fontAsset.ReadFontDefinition();
							((Editor)this).get_serializedObject().Update();
							isAssetDirty = true;
							break;
						}
						EditorGUILayout.EndHorizontal();
					}
				}
				Rect controlRect2 = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				((Rect)(ref controlRect2)).set_width(((Rect)(ref controlRect2)).get_width() / 3f);
				int num6 = ((!current.get_shift()) ? 1 : 10);
				if (m_KerningPage > 0)
				{
					GUI.set_enabled(true);
				}
				else
				{
					GUI.set_enabled(false);
				}
				if (GUI.Button(controlRect2, "Previous Page"))
				{
					m_KerningPage -= num6;
				}
				GUI.set_enabled(true);
				((Rect)(ref controlRect2)).set_x(((Rect)(ref controlRect2)).get_x() + ((Rect)(ref controlRect2)).get_width());
				GUI.Label(controlRect2, string.Concat(str3: ((int)((float)arraySize / (float)num5 + 0.999f)).ToString(), str0: "Page ", str1: (m_KerningPage + 1).ToString(), str2: " / "), GUI.get_skin().get_button());
				((Rect)(ref controlRect2)).set_x(((Rect)(ref controlRect2)).get_x() + ((Rect)(ref controlRect2)).get_width());
				if (num5 * (m_GlyphPage + 1) < arraySize)
				{
					GUI.set_enabled(true);
				}
				else
				{
					GUI.set_enabled(false);
				}
				if (GUI.Button(controlRect2, "Next Page"))
				{
					m_KerningPage += num6;
				}
				m_KerningPage = Mathf.Clamp(m_KerningPage, 0, arraySize / num5);
				GUILayout.EndVertical();
				GUILayout.Space(10f);
				GUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				val5 = EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.PropertyField(new Rect(((Rect)(ref val5)).get_x(), ((Rect)(ref val5)).get_y(), ((Rect)(ref val5)).get_width() - 20f, ((Rect)(ref val5)).get_height()), m_kerningPair_prop);
				GUILayout.Label(GUIContent.none, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(19f) });
				EditorGUILayout.EndHorizontal();
				GUILayout.Space(5f);
				if (GUILayout.Button("Add New Kerning Pair", (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					int intValue = m_kerningPair_prop.FindPropertyRelative("AscII_Left").get_intValue();
					int intValue2 = m_kerningPair_prop.FindPropertyRelative("AscII_Right").get_intValue();
					float floatValue = m_kerningPair_prop.FindPropertyRelative("XadvanceOffset").get_floatValue();
					errorCode = m_kerningTable.AddKerningPair(intValue, intValue2, floatValue);
					if (errorCode != -1)
					{
						m_kerningTable.SortKerningPairs();
						m_fontAsset.ReadFontDefinition();
						((Editor)this).get_serializedObject().Update();
						isAssetDirty = true;
					}
					else
					{
						timeStamp = DateTime.Now.AddSeconds(5.0);
					}
				}
				if (errorCode == -1)
				{
					GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
					GUILayout.FlexibleSpace();
					GUILayout.Label("Kerning Pair already <color=#ffff00>exists!</color>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal();
					if (DateTime.Now > timeStamp)
					{
						errorCode = 0;
					}
				}
				GUILayout.EndVertical();
			}
			if (((Editor)this).get_serializedObject().ApplyModifiedProperties() || commandName == "UndoRedoPerformed" || isAssetDirty)
			{
				TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(isChanged: true, m_fontAsset);
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
			GUI.set_enabled(m_GlyphPage > 0);
			if (GUI.Button(controlRect, "Previous Page"))
			{
				m_GlyphPage -= num;
			}
			GUIStyle val = new GUIStyle(GUI.get_skin().get_button());
			val.get_normal().set_background((Texture2D)null);
			GUIStyle val2 = val;
			GUI.set_enabled(true);
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			GUI.Button(controlRect, string.Concat(str3: ((int)((float)arraySize / (float)itemsPerPage + 0.999f)).ToString(), str0: "Page ", str1: (m_GlyphPage + 1).ToString(), str2: " / "), val2);
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
			GUI.set_enabled(itemsPerPage * (m_GlyphPage + 1) < arraySize);
			if (GUI.Button(controlRect, "Next Page"))
			{
				m_GlyphPage += num;
			}
			m_GlyphPage = Mathf.Clamp(m_GlyphPage, 0, arraySize / itemsPerPage);
			GUI.set_enabled(true);
		}

		private bool AddNewGlyph(int srcIndex, int dstGlyphID)
		{
			if (m_fontAsset.characterDictionary.ContainsKey(dstGlyphID))
			{
				return false;
			}
			SerializedProperty glyphInfoList_prop = m_glyphInfoList_prop;
			glyphInfoList_prop.set_arraySize(glyphInfoList_prop.get_arraySize() + 1);
			SerializedProperty arrayElementAtIndex = m_glyphInfoList_prop.GetArrayElementAtIndex(srcIndex);
			int num = m_glyphInfoList_prop.get_arraySize() - 1;
			SerializedProperty target = m_glyphInfoList_prop.GetArrayElementAtIndex(num);
			CopySerializedProperty(arrayElementAtIndex, ref target);
			target.FindPropertyRelative("id").set_intValue(dstGlyphID);
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
			m_fontAsset.SortGlyphs();
			m_fontAsset.ReadFontDefinition();
			return true;
		}

		private void RemoveGlyphFromList(int index)
		{
			if (index <= m_glyphInfoList_prop.get_arraySize())
			{
				m_glyphInfoList_prop.DeleteArrayElementAtIndex(index);
				((Editor)this).get_serializedObject().ApplyModifiedProperties();
				m_fontAsset.ReadFontDefinition();
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

		private void CopySerializedProperty(SerializedProperty source, ref SerializedProperty target)
		{
			target.FindPropertyRelative("id").set_intValue(source.FindPropertyRelative("id").get_intValue());
			target.FindPropertyRelative("x").set_floatValue(source.FindPropertyRelative("x").get_floatValue());
			target.FindPropertyRelative("y").set_floatValue(source.FindPropertyRelative("y").get_floatValue());
			target.FindPropertyRelative("width").set_floatValue(source.FindPropertyRelative("width").get_floatValue());
			target.FindPropertyRelative("height").set_floatValue(source.FindPropertyRelative("height").get_floatValue());
			target.FindPropertyRelative("xOffset").set_floatValue(source.FindPropertyRelative("xOffset").get_floatValue());
			target.FindPropertyRelative("yOffset").set_floatValue(source.FindPropertyRelative("yOffset").get_floatValue());
			target.FindPropertyRelative("xAdvance").set_floatValue(source.FindPropertyRelative("xAdvance").get_floatValue());
			target.FindPropertyRelative("scale").set_floatValue(source.FindPropertyRelative("scale").get_floatValue());
		}

		private void SearchGlyphTable(string searchPattern, ref List<int> searchResults)
		{
			if (searchResults == null)
			{
				searchResults = new List<int>();
			}
			searchResults.Clear();
			int arraySize = m_glyphInfoList_prop.get_arraySize();
			for (int i = 0; i < arraySize; i++)
			{
				int intValue = m_glyphInfoList_prop.GetArrayElementAtIndex(i).FindPropertyRelative("id").get_intValue();
				if (searchPattern.Length == 1 && intValue == searchPattern[0])
				{
					searchResults.Add(i);
				}
				if (intValue.ToString().Contains(searchPattern))
				{
					searchResults.Add(i);
				}
				if (intValue.ToString("x").Contains(searchPattern))
				{
					searchResults.Add(i);
				}
				if (intValue.ToString("X").Contains(searchPattern))
				{
					searchResults.Add(i);
				}
			}
		}

		public TMP_FontAssetEditor()
			: this()
		{
		}
	}
}
