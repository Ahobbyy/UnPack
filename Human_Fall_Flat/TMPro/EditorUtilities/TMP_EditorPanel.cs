using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TextMeshPro))]
	[CanEditMultipleObjects]
	public class TMP_EditorPanel : Editor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct m_foldout
		{
			public static bool textInput = true;

			public static bool fontSettings = true;

			public static bool extraSettings = false;

			public static bool shadowSetting = false;

			public static bool materialEditor = true;
		}

		private static int m_eventID;

		private static string[] uiStateLabel = new string[2] { "\t- <i>Click to expand</i> -", "\t- <i>Click to collapse</i> -" };

		private const string k_UndoRedo = "UndoRedoPerformed";

		private GUIStyle toggleStyle;

		public int selAlignGrid_A;

		public int selAlignGrid_B;

		private SerializedProperty text_prop;

		private SerializedProperty isRightToLeft_prop;

		private string m_RTLText;

		private SerializedProperty fontAsset_prop;

		private SerializedProperty fontSharedMaterial_prop;

		private Material[] m_materialPresets;

		private string[] m_materialPresetNames;

		private int m_materialPresetSelectionIndex;

		private bool m_isPresetListDirty;

		private SerializedProperty fontStyle_prop;

		private SerializedProperty fontColor_prop;

		private SerializedProperty enableVertexGradient_prop;

		private SerializedProperty fontColorGradient_prop;

		private SerializedProperty fontColorGradientPreset_prop;

		private SerializedProperty overrideHtmlColor_prop;

		private SerializedProperty fontSize_prop;

		private SerializedProperty fontSizeBase_prop;

		private SerializedProperty autoSizing_prop;

		private SerializedProperty fontSizeMin_prop;

		private SerializedProperty fontSizeMax_prop;

		private SerializedProperty lineSpacingMax_prop;

		private SerializedProperty charWidthMaxAdj_prop;

		private SerializedProperty characterSpacing_prop;

		private SerializedProperty wordSpacing_prop;

		private SerializedProperty lineSpacing_prop;

		private SerializedProperty paragraphSpacing_prop;

		private SerializedProperty textAlignment_prop;

		private SerializedProperty horizontalMapping_prop;

		private SerializedProperty verticalMapping_prop;

		private SerializedProperty uvLineOffset_prop;

		private SerializedProperty enableWordWrapping_prop;

		private SerializedProperty wordWrappingRatios_prop;

		private SerializedProperty textOverflowMode_prop;

		private SerializedProperty pageToDisplay_prop;

		private SerializedProperty linkedTextComponent_prop;

		private SerializedProperty isLinkedTextComponent_prop;

		private SerializedProperty enableKerning_prop;

		private SerializedProperty inputSource_prop;

		private SerializedProperty havePropertiesChanged_prop;

		private SerializedProperty isInputPasingRequired_prop;

		private SerializedProperty isRichText_prop;

		private SerializedProperty hasFontAssetChanged_prop;

		private SerializedProperty enableExtraPadding_prop;

		private SerializedProperty checkPaddingRequired_prop;

		private SerializedProperty enableEscapeCharacterParsing_prop;

		private SerializedProperty useMaxVisibleDescender_prop;

		private SerializedProperty isVolumetricText_prop;

		private SerializedProperty geometrySortingOrder_prop;

		private SerializedProperty spriteAsset_prop;

		private SerializedProperty isOrthographic_prop;

		private bool havePropertiesChanged;

		private TextMeshPro m_textComponent;

		private RectTransform m_rectTransform;

		private Material m_targetMaterial;

		private SerializedProperty margin_prop;

		private Vector3[] m_rectCorners = (Vector3[])(object)new Vector3[4];

		private Vector3[] handlePoints = (Vector3[])(object)new Vector3[4];

		public void OnEnable()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Combine((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedo));
			text_prop = ((Editor)this).get_serializedObject().FindProperty("m_text");
			isRightToLeft_prop = ((Editor)this).get_serializedObject().FindProperty("m_isRightToLeft");
			fontAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontAsset");
			fontSharedMaterial_prop = ((Editor)this).get_serializedObject().FindProperty("m_sharedMaterial");
			fontStyle_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontStyle");
			fontSize_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontSize");
			fontSizeBase_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontSizeBase");
			autoSizing_prop = ((Editor)this).get_serializedObject().FindProperty("m_enableAutoSizing");
			fontSizeMin_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontSizeMin");
			fontSizeMax_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontSizeMax");
			lineSpacingMax_prop = ((Editor)this).get_serializedObject().FindProperty("m_lineSpacingMax");
			charWidthMaxAdj_prop = ((Editor)this).get_serializedObject().FindProperty("m_charWidthMaxAdj");
			fontColor_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontColor");
			enableVertexGradient_prop = ((Editor)this).get_serializedObject().FindProperty("m_enableVertexGradient");
			fontColorGradient_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontColorGradient");
			fontColorGradientPreset_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontColorGradientPreset");
			overrideHtmlColor_prop = ((Editor)this).get_serializedObject().FindProperty("m_overrideHtmlColors");
			characterSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("m_characterSpacing");
			wordSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("m_wordSpacing");
			lineSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("m_lineSpacing");
			paragraphSpacing_prop = ((Editor)this).get_serializedObject().FindProperty("m_paragraphSpacing");
			textAlignment_prop = ((Editor)this).get_serializedObject().FindProperty("m_textAlignment");
			horizontalMapping_prop = ((Editor)this).get_serializedObject().FindProperty("m_horizontalMapping");
			verticalMapping_prop = ((Editor)this).get_serializedObject().FindProperty("m_verticalMapping");
			uvLineOffset_prop = ((Editor)this).get_serializedObject().FindProperty("m_uvLineOffset");
			enableWordWrapping_prop = ((Editor)this).get_serializedObject().FindProperty("m_enableWordWrapping");
			wordWrappingRatios_prop = ((Editor)this).get_serializedObject().FindProperty("m_wordWrappingRatios");
			textOverflowMode_prop = ((Editor)this).get_serializedObject().FindProperty("m_overflowMode");
			pageToDisplay_prop = ((Editor)this).get_serializedObject().FindProperty("m_pageToDisplay");
			linkedTextComponent_prop = ((Editor)this).get_serializedObject().FindProperty("m_linkedTextComponent");
			isLinkedTextComponent_prop = ((Editor)this).get_serializedObject().FindProperty("m_isLinkedTextComponent");
			enableKerning_prop = ((Editor)this).get_serializedObject().FindProperty("m_enableKerning");
			isOrthographic_prop = ((Editor)this).get_serializedObject().FindProperty("m_isOrthographic");
			havePropertiesChanged_prop = ((Editor)this).get_serializedObject().FindProperty("m_havePropertiesChanged");
			inputSource_prop = ((Editor)this).get_serializedObject().FindProperty("m_inputSource");
			isInputPasingRequired_prop = ((Editor)this).get_serializedObject().FindProperty("m_isInputParsingRequired");
			enableExtraPadding_prop = ((Editor)this).get_serializedObject().FindProperty("m_enableExtraPadding");
			isRichText_prop = ((Editor)this).get_serializedObject().FindProperty("m_isRichText");
			checkPaddingRequired_prop = ((Editor)this).get_serializedObject().FindProperty("checkPaddingRequired");
			enableEscapeCharacterParsing_prop = ((Editor)this).get_serializedObject().FindProperty("m_parseCtrlCharacters");
			useMaxVisibleDescender_prop = ((Editor)this).get_serializedObject().FindProperty("m_useMaxVisibleDescender");
			isVolumetricText_prop = ((Editor)this).get_serializedObject().FindProperty("m_isVolumetricText");
			geometrySortingOrder_prop = ((Editor)this).get_serializedObject().FindProperty("m_geometrySortingOrder");
			spriteAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_spriteAsset");
			margin_prop = ((Editor)this).get_serializedObject().FindProperty("m_margin");
			hasFontAssetChanged_prop = ((Editor)this).get_serializedObject().FindProperty("m_hasFontAssetChanged");
			TMP_UIStyleManager.GetUIStyles();
			m_textComponent = ((Editor)this).get_target() as TextMeshPro;
			m_rectTransform = m_textComponent.rectTransform;
			m_targetMaterial = m_textComponent.fontSharedMaterial;
			if (m_foldout.materialEditor)
			{
				InternalEditorUtility.SetIsInspectorExpanded((Object)(object)m_targetMaterial, true);
			}
			m_materialPresetNames = GetMaterialPresets();
		}

		public void OnDisable()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Remove((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedo));
		}

		public override void OnInspectorGUI()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0410: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_044c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0671: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Expected O, but got Unknown
			//IL_0688: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Expected O, but got Unknown
			//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e9: Expected O, but got Unknown
			//IL_071e: Unknown result type (might be due to invalid IL or missing references)
			//IL_072e: Expected O, but got Unknown
			//IL_075f: Unknown result type (might be due to invalid IL or missing references)
			//IL_076f: Expected O, but got Unknown
			//IL_0785: Unknown result type (might be due to invalid IL or missing references)
			//IL_0795: Expected O, but got Unknown
			//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bb: Expected O, but got Unknown
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_07e1: Expected O, but got Unknown
			//IL_0803: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Expected O, but got Unknown
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			//IL_0830: Expected O, but got Unknown
			//IL_0842: Unknown result type (might be due to invalid IL or missing references)
			//IL_0852: Expected O, but got Unknown
			//IL_0864: Unknown result type (might be due to invalid IL or missing references)
			//IL_0874: Expected O, but got Unknown
			//IL_0886: Unknown result type (might be due to invalid IL or missing references)
			//IL_0896: Expected O, but got Unknown
			//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0908: Expected O, but got Unknown
			//IL_094d: Unknown result type (might be due to invalid IL or missing references)
			//IL_095d: Expected O, but got Unknown
			//IL_09b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_09da: Unknown result type (might be due to invalid IL or missing references)
			//IL_09f7: Expected O, but got Unknown
			//IL_0a46: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a63: Expected O, but got Unknown
			//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acf: Expected O, but got Unknown
			//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b02: Expected O, but got Unknown
			//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bae: Expected O, but got Unknown
			//IL_0bba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bd7: Expected O, but got Unknown
			//IL_0be3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c00: Expected O, but got Unknown
			//IL_0c0c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c29: Expected O, but got Unknown
			//IL_0c54: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c59: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6b: Expected O, but got Unknown
			//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c7b: Expected O, but got Unknown
			//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c8b: Expected O, but got Unknown
			//IL_0cd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0def: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e23: Expected O, but got Unknown
			//IL_0e1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f45: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_100a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1061: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_10b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e2: Expected O, but got Unknown
			//IL_10dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_1110: Unknown result type (might be due to invalid IL or missing references)
			//IL_1150: Unknown result type (might be due to invalid IL or missing references)
			//IL_118e: Unknown result type (might be due to invalid IL or missing references)
			//IL_11ab: Expected O, but got Unknown
			//IL_11c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_11d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_11e6: Expected O, but got Unknown
			//IL_1205: Unknown result type (might be due to invalid IL or missing references)
			//IL_1215: Expected O, but got Unknown
			//IL_12ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_12cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_13c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d6: Expected O, but got Unknown
			//IL_13ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_140d: Expected O, but got Unknown
			//IL_1419: Unknown result type (might be due to invalid IL or missing references)
			//IL_1429: Expected O, but got Unknown
			//IL_1435: Unknown result type (might be due to invalid IL or missing references)
			//IL_1446: Unknown result type (might be due to invalid IL or missing references)
			//IL_1456: Expected O, but got Unknown
			//IL_1462: Unknown result type (might be due to invalid IL or missing references)
			//IL_1472: Expected O, but got Unknown
			//IL_1488: Unknown result type (might be due to invalid IL or missing references)
			//IL_1499: Expected O, but got Unknown
			//IL_14b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_14c8: Expected O, but got Unknown
			if (toggleStyle == null)
			{
				toggleStyle = new GUIStyle(GUI.get_skin().get_label());
				toggleStyle.set_fontSize(12);
				toggleStyle.get_normal().set_textColor(TMP_UIStyleManager.Section_Label.get_normal().get_textColor());
				toggleStyle.set_richText(true);
			}
			((Editor)this).get_serializedObject().Update();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 25f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUIUtility.set_labelWidth(130f);
			float labelWidth = 130f;
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			((Rect)(ref controlRect)).set_y(((Rect)(ref controlRect)).get_y() + 2f);
			GUI.Label(controlRect, "<b>TEXT INPUT BOX</b>" + (m_foldout.textInput ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label);
			if (GUI.Button(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width() - 150f, ((Rect)(ref controlRect)).get_height()), GUIContent.none, GUI.get_skin().get_label()))
			{
				m_foldout.textInput = !m_foldout.textInput;
			}
			GUI.Label(new Rect(((Rect)(ref controlRect)).get_width() - 125f, ((Rect)(ref controlRect)).get_y() + 4f, 125f, 24f), "<i>Enable RTL Editor</i>", toggleStyle);
			isRightToLeft_prop.set_boolValue(EditorGUI.Toggle(new Rect(((Rect)(ref controlRect)).get_width() - 10f, ((Rect)(ref controlRect)).get_y() + 3f, 20f, 24f), GUIContent.none, isRightToLeft_prop.get_boolValue()));
			if (m_foldout.textInput)
			{
				if (isLinkedTextComponent_prop.get_boolValue())
				{
					EditorGUILayout.HelpBox("The Text Input Box is disabled due to this text component being linked to another.", (MessageType)1);
				}
				else
				{
					EditorGUI.BeginChangeCheck();
					text_prop.set_stringValue(EditorGUILayout.TextArea(text_prop.get_stringValue(), TMP_UIStyleManager.TextAreaBoxEditor, (GUILayoutOption[])(object)new GUILayoutOption[2]
					{
						GUILayout.Height(125f),
						GUILayout.ExpandWidth(true)
					}));
					if (EditorGUI.EndChangeCheck() || (isRightToLeft_prop.get_boolValue() && (m_RTLText == null || m_RTLText == string.Empty)))
					{
						inputSource_prop.set_enumValueIndex(0);
						isInputPasingRequired_prop.set_boolValue(true);
						havePropertiesChanged = true;
						if (isRightToLeft_prop.get_boolValue())
						{
							m_RTLText = string.Empty;
							string stringValue = text_prop.get_stringValue();
							for (int i = 0; i < stringValue.Length; i++)
							{
								m_RTLText += stringValue[stringValue.Length - i - 1];
							}
						}
					}
					if (isRightToLeft_prop.get_boolValue())
					{
						EditorGUI.BeginChangeCheck();
						m_RTLText = EditorGUILayout.TextArea(m_RTLText, TMP_UIStyleManager.TextAreaBoxEditor, (GUILayoutOption[])(object)new GUILayoutOption[2]
						{
							GUILayout.Height(125f),
							GUILayout.ExpandWidth(true)
						});
						if (EditorGUI.EndChangeCheck())
						{
							string text = string.Empty;
							for (int j = 0; j < m_RTLText.Length; j++)
							{
								text += m_RTLText[m_RTLText.Length - j - 1];
							}
							text_prop.set_stringValue(text);
						}
					}
				}
			}
			if (GUILayout.Button("<b>FONT SETTINGS</b>" + (m_foldout.fontSettings ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.fontSettings = !m_foldout.fontSettings;
			}
			if (m_isPresetListDirty)
			{
				m_materialPresetNames = GetMaterialPresets();
			}
			if (m_foldout.fontSettings)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(fontAsset_prop, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
					hasFontAssetChanged_prop.set_boolValue(true);
					m_isPresetListDirty = true;
					m_materialPresetSelectionIndex = 0;
				}
				if (m_materialPresetNames != null)
				{
					EditorGUI.BeginChangeCheck();
					controlRect = EditorGUILayout.GetControlRect(false, 18f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					float fixedHeight = EditorStyles.get_popup().get_fixedHeight();
					EditorStyles.get_popup().set_fixedHeight(((Rect)(ref controlRect)).get_height());
					int fontSize = EditorStyles.get_popup().get_fontSize();
					EditorStyles.get_popup().set_fontSize(11);
					m_materialPresetSelectionIndex = EditorGUI.Popup(controlRect, "Material Preset", m_materialPresetSelectionIndex, m_materialPresetNames);
					if (EditorGUI.EndChangeCheck())
					{
						fontSharedMaterial_prop.set_objectReferenceValue((Object)(object)m_materialPresets[m_materialPresetSelectionIndex]);
						havePropertiesChanged = true;
					}
					if (m_materialPresetSelectionIndex < m_materialPresetNames.Length && (Object)(object)m_targetMaterial != (Object)(object)m_materialPresets[m_materialPresetSelectionIndex] && !havePropertiesChanged)
					{
						m_isPresetListDirty = true;
					}
					EditorStyles.get_popup().set_fixedHeight(fixedHeight);
					EditorStyles.get_popup().set_fontSize(fontSize);
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PrefixLabel("Font Style");
				int intValue = fontStyle_prop.get_intValue();
				int num = (GUILayout.Toggle((intValue & 1) == 1, "B", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 1 : 0);
				int num2 = (GUILayout.Toggle((intValue & 2) == 2, "I", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 2 : 0);
				int num3 = (GUILayout.Toggle((intValue & 4) == 4, "U", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 4 : 0);
				int num4 = (GUILayout.Toggle((intValue & 0x40) == 64, "S", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 64 : 0);
				int num5 = (GUILayout.Toggle((intValue & 8) == 8, "ab", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 8 : 0);
				int num6 = (GUILayout.Toggle((intValue & 0x10) == 16, "AB", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 16 : 0);
				int num7 = (GUILayout.Toggle((intValue & 0x20) == 32, "SC", GUI.get_skin().get_button(), (GUILayoutOption[])(object)new GUILayoutOption[0]) ? 32 : 0);
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					fontStyle_prop.set_intValue(num + num2 + num3 + num5 + num6 + num7 + num4);
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(fontColor_prop, new GUIContent("Color (Vertex)"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(enableVertexGradient_prop, new GUIContent("Color Gradient"), (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.MinWidth(140f),
					GUILayout.MaxWidth(200f)
				});
				EditorGUIUtility.set_labelWidth(95f);
				EditorGUILayout.PropertyField(overrideHtmlColor_prop, new GUIContent("Override Tags"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUIUtility.set_labelWidth(labelWidth);
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				if (enableVertexGradient_prop.get_boolValue())
				{
					EditorGUILayout.PropertyField(fontColorGradientPreset_prop, new GUIContent("Gradient (Preset)"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
					if (fontColorGradientPreset_prop.get_objectReferenceValue() == (Object)null)
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(fontColorGradient_prop.FindPropertyRelative("topLeft"), new GUIContent("Top Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(fontColorGradient_prop.FindPropertyRelative("topRight"), new GUIContent("Top Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(fontColorGradient_prop.FindPropertyRelative("bottomLeft"), new GUIContent("Bottom Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(fontColorGradient_prop.FindPropertyRelative("bottomRight"), new GUIContent("Bottom Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							havePropertiesChanged = true;
						}
					}
					else
					{
						SerializedObject val = new SerializedObject(fontColorGradientPreset_prop.get_objectReferenceValue());
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(val.FindProperty("topLeft"), new GUIContent("Top Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(val.FindProperty("topRight"), new GUIContent("Top Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(val.FindProperty("bottomLeft"), new GUIContent("Bottom Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(val.FindProperty("bottomRight"), new GUIContent("Bottom Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck())
						{
							val.ApplyModifiedProperties();
							havePropertiesChanged = true;
							TMPro_EventManager.ON_COLOR_GRAIDENT_PROPERTY_CHANGED(fontColorGradientPreset_prop.get_objectReferenceValue() as TMP_ColorGradient);
						}
					}
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(fontSize_prop, new GUIContent("Font Size"), (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.MinWidth(168f),
					GUILayout.MaxWidth(200f)
				});
				EditorGUIUtility.set_fieldWidth(fieldWidth);
				if (EditorGUI.EndChangeCheck())
				{
					fontSizeBase_prop.set_floatValue(fontSize_prop.get_floatValue());
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				EditorGUIUtility.set_labelWidth(70f);
				EditorGUILayout.PropertyField(autoSizing_prop, new GUIContent("Auto Size"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUIUtility.set_labelWidth(labelWidth);
				if (EditorGUI.EndChangeCheck())
				{
					if (!autoSizing_prop.get_boolValue())
					{
						fontSize_prop.set_floatValue(fontSizeBase_prop.get_floatValue());
					}
					havePropertiesChanged = true;
				}
				if (autoSizing_prop.get_boolValue())
				{
					EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
					EditorGUILayout.PrefixLabel("Auto Size Options");
					EditorGUIUtility.set_labelWidth(24f);
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(fontSizeMin_prop, new GUIContent("Min"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(46f) });
					if (EditorGUI.EndChangeCheck())
					{
						fontSizeMin_prop.set_floatValue(Mathf.Min(fontSizeMin_prop.get_floatValue(), fontSizeMax_prop.get_floatValue()));
						havePropertiesChanged = true;
					}
					EditorGUIUtility.set_labelWidth(27f);
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(fontSizeMax_prop, new GUIContent("Max"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(49f) });
					if (EditorGUI.EndChangeCheck())
					{
						fontSizeMax_prop.set_floatValue(Mathf.Max(fontSizeMin_prop.get_floatValue(), fontSizeMax_prop.get_floatValue()));
						havePropertiesChanged = true;
					}
					EditorGUI.BeginChangeCheck();
					EditorGUIUtility.set_labelWidth(36f);
					EditorGUILayout.PropertyField(charWidthMaxAdj_prop, new GUIContent("WD%"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(58f) });
					EditorGUIUtility.set_labelWidth(28f);
					EditorGUILayout.PropertyField(lineSpacingMax_prop, new GUIContent("Line"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(50f) });
					EditorGUIUtility.set_labelWidth(labelWidth);
					EditorGUILayout.EndHorizontal();
					if (EditorGUI.EndChangeCheck())
					{
						charWidthMaxAdj_prop.set_floatValue(Mathf.Clamp(charWidthMaxAdj_prop.get_floatValue(), 0f, 50f));
						lineSpacingMax_prop.set_floatValue(Mathf.Min(0f, lineSpacingMax_prop.get_floatValue()));
						havePropertiesChanged = true;
					}
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PrefixLabel("Spacing Options");
				EditorGUIUtility.set_labelWidth(35f);
				EditorGUILayout.PropertyField(characterSpacing_prop, new GUIContent("Char"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(50f) });
				EditorGUILayout.PropertyField(wordSpacing_prop, new GUIContent("Word"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(50f) });
				EditorGUILayout.PropertyField(lineSpacing_prop, new GUIContent("Line"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(50f) });
				EditorGUILayout.PropertyField(paragraphSpacing_prop, new GUIContent(" Par."), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(50f) });
				EditorGUIUtility.set_labelWidth(labelWidth);
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				controlRect = EditorGUILayout.GetControlRect(false, 19f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUIStyle val2 = new GUIStyle(GUI.get_skin().get_button());
				val2.set_margin(new RectOffset(1, 1, 1, 1));
				val2.set_padding(new RectOffset(1, 1, 1, 0));
				selAlignGrid_A = TMP_EditorUtility.GetHorizontalAlignmentGridValue(textAlignment_prop.get_intValue());
				selAlignGrid_B = TMP_EditorUtility.GetVerticalAlignmentGridValue(textAlignment_prop.get_intValue());
				GUI.Label(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y() + 2f, 100f, ((Rect)(ref controlRect)).get_height()), "Alignment");
				float num8 = EditorGUIUtility.get_labelWidth() + 15f;
				selAlignGrid_A = GUI.SelectionGrid(new Rect(num8, ((Rect)(ref controlRect)).get_y(), 138f, ((Rect)(ref controlRect)).get_height()), selAlignGrid_A, TMP_UIStyleManager.alignContent_A, 6, val2);
				selAlignGrid_B = GUI.SelectionGrid(new Rect(num8 + 138f + 20f, ((Rect)(ref controlRect)).get_y(), 138f, ((Rect)(ref controlRect)).get_height()), selAlignGrid_B, TMP_UIStyleManager.alignContent_B, 6, val2);
				if (EditorGUI.EndChangeCheck())
				{
					int intValue2 = (1 << selAlignGrid_A) | (256 << selAlignGrid_B);
					textAlignment_prop.set_intValue(intValue2);
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				if ((textAlignment_prop.get_intValue() & 8) == 8 || (textAlignment_prop.get_intValue() & 0x10) == 16)
				{
					DrawPropertySlider("Wrap Mix (W <-> C)", wordWrappingRatios_prop);
				}
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				controlRect = EditorGUILayout.GetControlRect(false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.PrefixLabel(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), 130f, ((Rect)(ref controlRect)).get_height()), new GUIContent("Wrapping & Overflow"));
				((Rect)(ref controlRect)).set_width((((Rect)(ref controlRect)).get_width() - 130f) / 2f);
				((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + 130f);
				int num9 = EditorGUI.Popup(controlRect, enableWordWrapping_prop.get_boolValue() ? 1 : 0, new string[2] { "Disabled", "Enabled" });
				if (EditorGUI.EndChangeCheck())
				{
					enableWordWrapping_prop.set_boolValue((num9 == 1) ? true : false);
					havePropertiesChanged = true;
					isInputPasingRequired_prop.set_boolValue(true);
				}
				EditorGUI.BeginChangeCheck();
				TMP_Text tMP_Text = linkedTextComponent_prop.get_objectReferenceValue() as TMP_Text;
				if (textOverflowMode_prop.get_enumValueIndex() == 6)
				{
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + (((Rect)(ref controlRect)).get_width() + 5f));
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 3f);
					EditorGUI.PropertyField(controlRect, textOverflowMode_prop, GUIContent.none);
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() * 2f - 5f);
					EditorGUI.PropertyField(controlRect, linkedTextComponent_prop, GUIContent.none);
					if (GUI.get_changed())
					{
						TMP_Text tMP_Text2 = linkedTextComponent_prop.get_objectReferenceValue() as TMP_Text;
						if (Object.op_Implicit((Object)(object)tMP_Text2))
						{
							m_textComponent.linkedTextComponent = tMP_Text2;
						}
					}
				}
				else if (textOverflowMode_prop.get_enumValueIndex() == 5)
				{
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + (((Rect)(ref controlRect)).get_width() + 5f));
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() / 2f);
					EditorGUI.PropertyField(controlRect, textOverflowMode_prop, GUIContent.none);
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() - 5f);
					EditorGUI.PropertyField(controlRect, pageToDisplay_prop, GUIContent.none);
					if (Object.op_Implicit((Object)(object)tMP_Text))
					{
						m_textComponent.linkedTextComponent = null;
					}
				}
				else
				{
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + (((Rect)(ref controlRect)).get_width() + 5f));
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() - 5f);
					EditorGUI.PropertyField(controlRect, textOverflowMode_prop, GUIContent.none);
					if (Object.op_Implicit((Object)(object)tMP_Text))
					{
						m_textComponent.linkedTextComponent = null;
					}
				}
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
					isInputPasingRequired_prop.set_boolValue(true);
				}
				EditorGUI.BeginChangeCheck();
				controlRect = EditorGUILayout.GetControlRect(false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.PrefixLabel(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), 130f, ((Rect)(ref controlRect)).get_height()), new GUIContent("UV Mapping Options"));
				((Rect)(ref controlRect)).set_width((((Rect)(ref controlRect)).get_width() - 130f) / 2f);
				((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + 130f);
				EditorGUI.PropertyField(controlRect, horizontalMapping_prop, GUIContent.none);
				((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + (((Rect)(ref controlRect)).get_width() + 5f));
				((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() - 5f);
				EditorGUI.PropertyField(controlRect, verticalMapping_prop, GUIContent.none);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				if (horizontalMapping_prop.get_enumValueIndex() > 0)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(uvLineOffset_prop, new GUIContent("UV Line Offset"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(70f) });
					if (EditorGUI.EndChangeCheck())
					{
						havePropertiesChanged = true;
					}
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(enableKerning_prop, new GUIContent("Enable Kerning?"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(enableExtraPadding_prop, new GUIContent("Extra Padding?"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
					checkPaddingRequired_prop.set_boolValue(true);
				}
				EditorGUILayout.EndHorizontal();
			}
			if (GUILayout.Button("<b>EXTRA SETTINGS</b>" + (m_foldout.extraSettings ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.extraSettings = !m_foldout.extraSettings;
			}
			if (m_foldout.extraSettings)
			{
				EditorGUI.set_indentLevel(0);
				EditorGUI.BeginChangeCheck();
				DrawMaginProperty(margin_prop, "Margins");
				if (EditorGUI.EndChangeCheck())
				{
					m_textComponent.margin = margin_prop.get_vector4Value();
					havePropertiesChanged = true;
				}
				GUILayout.Space(10f);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PrefixLabel("Sorting Layer");
				EditorGUI.BeginChangeCheck();
				float labelWidth2 = EditorGUIUtility.get_labelWidth();
				float fieldWidth2 = EditorGUIUtility.get_fieldWidth();
				string[] sortingLayerNames = SortingLayerHelper.sortingLayerNames;
				string sortingLayerNameFromID = SortingLayerHelper.GetSortingLayerNameFromID(m_textComponent.sortingLayerID);
				int num10 = Array.IndexOf(sortingLayerNames, sortingLayerNameFromID);
				EditorGUIUtility.set_fieldWidth(0f);
				int num11 = EditorGUILayout.Popup(string.Empty, num10, sortingLayerNames, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(80f) });
				if (num11 != num10)
				{
					m_textComponent.sortingLayerID = SortingLayerHelper.GetSortingLayerIDForIndex(num11);
				}
				EditorGUIUtility.set_labelWidth(40f);
				EditorGUIUtility.set_fieldWidth(80f);
				int num12 = EditorGUILayout.IntField("Order", m_textComponent.sortingOrder, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (num12 != m_textComponent.sortingOrder)
				{
					m_textComponent.sortingOrder = num12;
				}
				EditorGUILayout.EndHorizontal();
				EditorGUIUtility.set_labelWidth(labelWidth2);
				EditorGUIUtility.set_fieldWidth(fieldWidth2);
				EditorGUILayout.PropertyField(geometrySortingOrder_prop, new GUIContent("Geometry Sorting"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.BeginChangeCheck();
				EditorGUIUtility.set_labelWidth(150f);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(isOrthographic_prop, new GUIContent("Orthographic Mode?"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(isRichText_prop, new GUIContent("Enable Rich Text?"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(enableEscapeCharacterParsing_prop, new GUIContent("Parse Escape Characters"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(useMaxVisibleDescender_prop, new GUIContent("Use Visible Descender"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.PropertyField(spriteAsset_prop, new GUIContent("Sprite Asset", "The Sprite Asset used when NOT specifically referencing one using <sprite=\"Sprite Asset Name\"."), true, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(isVolumetricText_prop, new GUIContent("Enabled Volumetric Setup"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
					m_textComponent.textInfo.ResetVertexLayout(isVolumetricText_prop.get_boolValue());
				}
				EditorGUIUtility.set_labelWidth(135f);
				GUILayout.Space(10f);
			}
			if (havePropertiesChanged)
			{
				havePropertiesChanged_prop.set_boolValue(true);
				havePropertiesChanged = false;
			}
			EditorGUILayout.Space();
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		public void OnSceneGUI()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Expected O, but got Unknown
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Expected O, but got Unknown
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0304: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032a: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_032e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0353: Unknown result type (might be due to invalid IL or missing references)
			//IL_0366: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Expected O, but got Unknown
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_041d: Unknown result type (might be due to invalid IL or missing references)
			if (!IsMixSelectionTypes())
			{
				m_rectTransform.GetWorldCorners(m_rectCorners);
				Vector4 margin = m_textComponent.margin;
				Vector3 lossyScale = ((Transform)m_rectTransform).get_lossyScale();
				handlePoints[0] = m_rectCorners[0] + ((Transform)m_rectTransform).TransformDirection(new Vector3(margin.x * lossyScale.x, margin.w * lossyScale.y, 0f));
				handlePoints[1] = m_rectCorners[1] + ((Transform)m_rectTransform).TransformDirection(new Vector3(margin.x * lossyScale.x, (0f - margin.y) * lossyScale.y, 0f));
				handlePoints[2] = m_rectCorners[2] + ((Transform)m_rectTransform).TransformDirection(new Vector3((0f - margin.z) * lossyScale.x, (0f - margin.y) * lossyScale.y, 0f));
				handlePoints[3] = m_rectCorners[3] + ((Transform)m_rectTransform).TransformDirection(new Vector3((0f - margin.z) * lossyScale.x, margin.w * lossyScale.y, 0f));
				Handles.set_color(Color.get_yellow());
				Handles.DrawSolidRectangleWithOutline(handlePoints, Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)0)), Color32.op_Implicit(new Color32(byte.MaxValue, byte.MaxValue, (byte)0, byte.MaxValue)));
				Vector3 val = (handlePoints[0] + handlePoints[1]) * 0.5f;
				Vector3 val2 = Handles.FreeMoveHandle(val, Quaternion.get_identity(), HandleUtility.GetHandleSize(((Transform)m_rectTransform).get_position()) * 0.05f, Vector3.get_zero(), new CapFunction(Handles.DotHandleCap));
				bool flag = false;
				if (val != val2)
				{
					float num = val.x - val2.x;
					margin.x += (0f - num) / lossyScale.x;
					flag = true;
				}
				Vector3 val3 = (handlePoints[1] + handlePoints[2]) * 0.5f;
				Vector3 val4 = Handles.FreeMoveHandle(val3, Quaternion.get_identity(), HandleUtility.GetHandleSize(((Transform)m_rectTransform).get_position()) * 0.05f, Vector3.get_zero(), new CapFunction(Handles.DotHandleCap));
				if (val3 != val4)
				{
					float num2 = val3.y - val4.y;
					margin.y += num2 / lossyScale.y;
					flag = true;
				}
				Vector3 val5 = (handlePoints[2] + handlePoints[3]) * 0.5f;
				Vector3 val6 = Handles.FreeMoveHandle(val5, Quaternion.get_identity(), HandleUtility.GetHandleSize(((Transform)m_rectTransform).get_position()) * 0.05f, Vector3.get_zero(), new CapFunction(Handles.DotHandleCap));
				if (val5 != val6)
				{
					float num3 = val5.x - val6.x;
					margin.z += num3 / lossyScale.x;
					flag = true;
				}
				Vector3 val7 = (handlePoints[3] + handlePoints[0]) * 0.5f;
				Vector3 val8 = Handles.FreeMoveHandle(val7, Quaternion.get_identity(), HandleUtility.GetHandleSize(((Transform)m_rectTransform).get_position()) * 0.05f, Vector3.get_zero(), new CapFunction(Handles.DotHandleCap));
				if (val7 != val8)
				{
					float num4 = val7.y - val8.y;
					margin.w += (0f - num4) / lossyScale.y;
					flag = true;
				}
				if (flag)
				{
					Undo.RecordObjects((Object[])(object)new Object[2]
					{
						(Object)m_rectTransform,
						(Object)m_textComponent
					}, "Margin Changes");
					m_textComponent.margin = margin;
					EditorUtility.SetDirty(((Editor)this).get_target());
				}
			}
		}

		private bool IsMixSelectionTypes()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Object[] gameObjects = (Object[])(object)Selection.get_gameObjects();
			Object[] array = gameObjects;
			if (array.Length > 1)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if ((Object)(object)((GameObject)array[i]).GetComponent<TextMeshPro>() == (Object)null)
					{
						return true;
					}
				}
			}
			return false;
		}

		private string[] GetMaterialPresets()
		{
			TMP_FontAsset tMP_FontAsset = fontAsset_prop.get_objectReferenceValue() as TMP_FontAsset;
			if ((Object)(object)tMP_FontAsset == (Object)null)
			{
				return null;
			}
			m_materialPresets = TMP_EditorUtility.FindMaterialReferences(tMP_FontAsset);
			m_materialPresetNames = new string[m_materialPresets.Length];
			for (int i = 0; i < m_materialPresetNames.Length; i++)
			{
				m_materialPresetNames[i] = ((Object)m_materialPresets[i]).get_name();
				if (((Object)m_targetMaterial).GetInstanceID() == ((Object)m_materialPresets[i]).GetInstanceID())
				{
					m_materialPresetSelectionIndex = i;
				}
			}
			m_isPresetListDirty = false;
			return m_materialPresetNames;
		}

		private void DrawMaginProperty(SerializedProperty property, string label)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 36f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width(), 18f);
			float num = ((Rect)(ref controlRect)).get_width() + 3f;
			((Rect)(ref val)).set_width(labelWidth);
			GUI.Label(val, label);
			Vector4 vector4Value = property.get_vector4Value();
			float num2 = (num - labelWidth) / 4f;
			((Rect)(ref val)).set_width(num2 - 5f);
			((Rect)(ref val)).set_x(labelWidth + 15f);
			GUI.Label(val, "Left");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Top");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Right");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Bottom");
			((Rect)(ref val)).set_y(((Rect)(ref val)).get_y() + 18f);
			((Rect)(ref val)).set_x(labelWidth + 15f);
			vector4Value.x = EditorGUI.FloatField(val, GUIContent.none, vector4Value.x);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			vector4Value.y = EditorGUI.FloatField(val, GUIContent.none, vector4Value.y);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			vector4Value.z = EditorGUI.FloatField(val, GUIContent.none, vector4Value.z);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			vector4Value.w = EditorGUI.FloatField(val, GUIContent.none, vector4Value.w);
			property.set_vector4Value(vector4Value);
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
		}

		private void DrawPropertySlider(string label, SerializedProperty property)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 17f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUIContent val = (GUIContent)((label == "") ? ((object)GUIContent.none) : ((object)new GUIContent(label)));
			EditorGUI.Slider(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width(), ((Rect)(ref controlRect)).get_height()), property, 0f, 1f, val);
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
		}

		private void DrawDoubleEnumPopup(SerializedProperty property1, SerializedProperty property2, string label)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 17f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), EditorGUIUtility.get_labelWidth(), ((Rect)(ref controlRect)).get_height());
			EditorGUI.PrefixLabel(val, new GUIContent(label));
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + ((Rect)(ref val)).get_width());
			((Rect)(ref val)).set_width((((Rect)(ref controlRect)).get_width() - ((Rect)(ref val)).get_x()) / 2f + 5f);
			EditorGUI.PropertyField(val, property1, GUIContent.none);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (((Rect)(ref val)).get_width() + 5f));
			EditorGUI.PropertyField(val, property2, GUIContent.none);
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
		}

		private void DrawPropertyBlock(string[] labels, SerializedProperty[] properties)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Expected O, but got Unknown
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 17f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.Label(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), labelWidth, ((Rect)(ref controlRect)).get_height()), labels[0]);
			((Rect)(ref controlRect)).set_x(labelWidth + 15f);
			((Rect)(ref controlRect)).set_width((((Rect)(ref controlRect)).get_width() + 20f - ((Rect)(ref controlRect)).get_x()) / (float)labels.Length);
			for (int i = 0; i < labels.Length; i++)
			{
				if (i == 0)
				{
					EditorGUIUtility.set_labelWidth(20f);
					int enabled;
					if (properties[i] != fontSize_prop || !autoSizing_prop.get_boolValue())
					{
						enabled = 1;
						GUI.set_enabled((byte)enabled != 0);
					}
					else
					{
						enabled = 0;
						GUI.set_enabled((byte)enabled != 0);
					}
					GUI.set_enabled((byte)enabled != 0);
					EditorGUI.PropertyField(new Rect(((Rect)(ref controlRect)).get_x() - 20f, ((Rect)(ref controlRect)).get_y(), 80f, ((Rect)(ref controlRect)).get_height()), properties[i], new GUIContent("  "));
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
					GUI.set_enabled(true);
				}
				else
				{
					EditorGUIUtility.set_labelWidth(GUI.get_skin().get_textArea().CalcSize(new GUIContent(labels[i]))
						.x);
						EditorGUI.PropertyField(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width() - 5f, ((Rect)(ref controlRect)).get_height()), properties[i], new GUIContent(labels[i]));
						((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + ((Rect)(ref controlRect)).get_width());
					}
				}
				EditorGUIUtility.set_labelWidth(labelWidth);
				EditorGUIUtility.set_fieldWidth(fieldWidth);
			}

			private void OnUndoRedo()
			{
				int currentGroup = Undo.GetCurrentGroup();
				int eventID = m_eventID;
				if (currentGroup != eventID)
				{
					for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
					{
						TMPro_EventManager.ON_TEXTMESHPRO_PROPERTY_CHANGED(isChanged: true, ((Editor)this).get_targets()[i] as TextMeshPro);
						m_eventID = currentGroup;
					}
				}
			}

			public TMP_EditorPanel()
				: this()
			{
			}
		}
	}
