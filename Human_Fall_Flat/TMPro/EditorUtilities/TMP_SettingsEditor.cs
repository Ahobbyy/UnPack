using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_Settings))]
	public class TMP_SettingsEditor : Editor
	{
		private SerializedProperty prop_FontAsset;

		private SerializedProperty prop_DefaultFontAssetPath;

		private SerializedProperty prop_DefaultFontSize;

		private SerializedProperty prop_DefaultAutoSizeMinRatio;

		private SerializedProperty prop_DefaultAutoSizeMaxRatio;

		private SerializedProperty prop_DefaultTextMeshProTextContainerSize;

		private SerializedProperty prop_DefaultTextMeshProUITextContainerSize;

		private SerializedProperty prop_AutoSizeTextContainer;

		private SerializedProperty prop_SpriteAsset;

		private SerializedProperty prop_SpriteAssetPath;

		private SerializedProperty prop_EnableEmojiSupport;

		private SerializedProperty prop_StyleSheet;

		private ReorderableList m_list;

		private SerializedProperty prop_ColorGradientPresetsPath;

		private SerializedProperty prop_matchMaterialPreset;

		private SerializedProperty prop_WordWrapping;

		private SerializedProperty prop_Kerning;

		private SerializedProperty prop_ExtraPadding;

		private SerializedProperty prop_TintAllSprites;

		private SerializedProperty prop_ParseEscapeCharacters;

		private SerializedProperty prop_MissingGlyphCharacter;

		private SerializedProperty prop_WarningsDisabled;

		private SerializedProperty prop_LeadingCharacters;

		private SerializedProperty prop_FollowingCharacters;

		public void OnEnable()
		{
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Expected O, but got Unknown
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Expected O, but got Unknown
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Expected O, but got Unknown
			prop_FontAsset = ((Editor)this).get_serializedObject().FindProperty("m_defaultFontAsset");
			prop_DefaultFontAssetPath = ((Editor)this).get_serializedObject().FindProperty("m_defaultFontAssetPath");
			prop_DefaultFontSize = ((Editor)this).get_serializedObject().FindProperty("m_defaultFontSize");
			prop_DefaultAutoSizeMinRatio = ((Editor)this).get_serializedObject().FindProperty("m_defaultAutoSizeMinRatio");
			prop_DefaultAutoSizeMaxRatio = ((Editor)this).get_serializedObject().FindProperty("m_defaultAutoSizeMaxRatio");
			prop_DefaultTextMeshProTextContainerSize = ((Editor)this).get_serializedObject().FindProperty("m_defaultTextMeshProTextContainerSize");
			prop_DefaultTextMeshProUITextContainerSize = ((Editor)this).get_serializedObject().FindProperty("m_defaultTextMeshProUITextContainerSize");
			prop_AutoSizeTextContainer = ((Editor)this).get_serializedObject().FindProperty("m_autoSizeTextContainer");
			prop_SpriteAsset = ((Editor)this).get_serializedObject().FindProperty("m_defaultSpriteAsset");
			prop_SpriteAssetPath = ((Editor)this).get_serializedObject().FindProperty("m_defaultSpriteAssetPath");
			prop_EnableEmojiSupport = ((Editor)this).get_serializedObject().FindProperty("m_enableEmojiSupport");
			prop_StyleSheet = ((Editor)this).get_serializedObject().FindProperty("m_defaultStyleSheet");
			prop_ColorGradientPresetsPath = ((Editor)this).get_serializedObject().FindProperty("m_defaultColorGradientPresetsPath");
			m_list = new ReorderableList(((Editor)this).get_serializedObject(), ((Editor)this).get_serializedObject().FindProperty("m_fallbackFontAssets"), true, true, true, true);
			m_list.drawElementCallback = (ElementCallbackDelegate)delegate(Rect rect, int index, bool isActive, bool isFocused)
			{
				//IL_003f: Unknown result type (might be due to invalid IL or missing references)
				SerializedProperty arrayElementAtIndex = m_list.get_serializedProperty().GetArrayElementAtIndex(index);
				((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + 2f);
				EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), EditorGUIUtility.get_singleLineHeight()), arrayElementAtIndex, GUIContent.none);
			};
			ReorderableList list = m_list;
			object obj = _003C_003Ec._003C_003E9__24_1;
			if (obj == null)
			{
				HeaderCallbackDelegate val = delegate(Rect rect)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					EditorGUI.LabelField(rect, "<b>Fallback Font Asset List</b>", TMP_UIStyleManager.Label);
				};
				obj = (object)val;
				_003C_003Ec._003C_003E9__24_1 = val;
			}
			list.drawHeaderCallback = (HeaderCallbackDelegate)obj;
			prop_matchMaterialPreset = ((Editor)this).get_serializedObject().FindProperty("m_matchMaterialPreset");
			prop_WordWrapping = ((Editor)this).get_serializedObject().FindProperty("m_enableWordWrapping");
			prop_Kerning = ((Editor)this).get_serializedObject().FindProperty("m_enableKerning");
			prop_ExtraPadding = ((Editor)this).get_serializedObject().FindProperty("m_enableExtraPadding");
			prop_TintAllSprites = ((Editor)this).get_serializedObject().FindProperty("m_enableTintAllSprites");
			prop_ParseEscapeCharacters = ((Editor)this).get_serializedObject().FindProperty("m_enableParseEscapeCharacters");
			prop_MissingGlyphCharacter = ((Editor)this).get_serializedObject().FindProperty("m_missingGlyphCharacter");
			prop_WarningsDisabled = ((Editor)this).get_serializedObject().FindProperty("m_warningsDisabled");
			prop_LeadingCharacters = ((Editor)this).get_serializedObject().FindProperty("m_leadingCharacters");
			prop_FollowingCharacters = ((Editor)this).get_serializedObject().FindProperty("m_followingCharacters");
			TMP_UIStyleManager.GetUIStyles();
		}

		public override void OnInspectorGUI()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Expected O, but got Unknown
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Expected O, but got Unknown
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Expected O, but got Unknown
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Expected O, but got Unknown
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Expected O, but got Unknown
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Expected O, but got Unknown
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Expected O, but got Unknown
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Expected O, but got Unknown
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ca: Expected O, but got Unknown
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Expected O, but got Unknown
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0391: Expected O, but got Unknown
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03db: Expected O, but got Unknown
			//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0452: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Expected O, but got Unknown
			//IL_048d: Unknown result type (might be due to invalid IL or missing references)
			//IL_049d: Expected O, but got Unknown
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Expected O, but got Unknown
			//IL_0566: Unknown result type (might be due to invalid IL or missing references)
			((Editor)this).get_serializedObject().Update();
			GUILayout.Label("<b>TEXTMESH PRO - SETTINGS</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(0);
			EditorGUIUtility.set_labelWidth(135f);
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Default Font Asset</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Select the Font Asset that will be assigned by default to newly created text objects when no Font Asset is specified.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(prop_FontAsset, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.Label("The relative path to a Resources folder where the Font Assets and Material Presets are located.\nExample \"Fonts & Materials/\"", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_DefaultFontAssetPath, new GUIContent("Path:        Resources/"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Fallback Font Assets</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Select the Font Assets that will be searched to locate and replace missing characters from a given Font Asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(5f);
			m_list.DoLayoutList();
			GUILayout.Label("<b>Fallback Material Settings</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_matchMaterialPreset, new GUIContent("Match Material Presets"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>New Text Object Default Settings</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Default settings used by all new text objects.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			EditorGUI.BeginChangeCheck();
			GUILayout.Label("<b>Text Container Default Settings</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUIUtility.set_labelWidth(150f);
			EditorGUILayout.PropertyField(prop_DefaultTextMeshProTextContainerSize, new GUIContent("TextMeshPro"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_DefaultTextMeshProUITextContainerSize, new GUIContent("TextMeshPro UI"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_AutoSizeTextContainer, new GUIContent("Auto Size Text Container", "Set the size of the text container to match the text."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.Label("<b>Text Component Default Settings</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUIUtility.set_labelWidth(150f);
			EditorGUILayout.PropertyField(prop_DefaultFontSize, new GUIContent("Default Font Size"), (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.MinWidth(200f),
				GUILayout.MaxWidth(200f)
			});
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel(new GUIContent("Text Auto Size Ratios"));
			EditorGUIUtility.set_labelWidth(35f);
			EditorGUILayout.PropertyField(prop_DefaultAutoSizeMinRatio, new GUIContent("Min:"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_DefaultAutoSizeMaxRatio, new GUIContent("Max:"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUIUtility.set_labelWidth(150f);
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_WordWrapping, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_Kerning, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_ExtraPadding, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_TintAllSprites, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_ParseEscapeCharacters, new GUIContent("Parse Escape Sequence"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUIUtility.set_fieldWidth(10f);
			EditorGUILayout.PropertyField(prop_MissingGlyphCharacter, new GUIContent("Missing Glyph Repl."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUIUtility.set_labelWidth(135f);
			GUILayout.Space(10f);
			GUILayout.Label("<b>Disable warnings for missing glyphs on text objects.</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_WarningsDisabled, new GUIContent("Disable warnings"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Default Sprite Asset</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Select the Sprite Asset that will be assigned by default when using the <sprite> tag when no Sprite Asset is specified.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(prop_SpriteAsset, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			EditorGUILayout.PropertyField(prop_EnableEmojiSupport, new GUIContent("Enable Emoji Support", "Enables Emoji support for Touch Screen Keyboards on target devices."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			GUILayout.Label("The relative path to a Resources folder where the Sprite Assets are located.\nExample \"Sprite Assets/\"", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_SpriteAssetPath, new GUIContent("Path:        Resources/"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Default Style Sheet</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Select the Style Sheet that will be used for all text objects in this project.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(prop_StyleSheet, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Color Gradient Presets</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("The relative path to a Resources folder where the Color Gradient Presets are located.\nExample \"Color Gradient Presets/\"", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_ColorGradientPresetsPath, new GUIContent("Path:        Resources/"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>Line Breaking Resources for Asian languages</b>", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Select the text assets that contain the Leading and Following characters which define the rules for line breaking with Asian languages.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(5f);
			EditorGUILayout.PropertyField(prop_LeadingCharacters, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(prop_FollowingCharacters, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			if (((Editor)this).get_serializedObject().ApplyModifiedProperties())
			{
				EditorUtility.SetDirty(((Editor)this).get_target());
				TMPro_EventManager.ON_TMP_SETTINGS_CHANGED();
			}
		}

		public TMP_SettingsEditor()
			: this()
		{
		}
	}
}
