using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

namespace TMPro.EditorUtilities
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(TMP_InputField), true)]
	public class TMP_InputFieldEditor : SelectableEditor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct m_foldout
		{
			public static bool textInput = true;

			public static bool fontSettings = true;

			public static bool extraSettings = true;
		}

		private static string[] uiStateLabel = new string[2] { "\t- <i>Click to expand</i> -", "\t- <i>Click to collapse</i> -" };

		private SerializedProperty m_TextViewport;

		private SerializedProperty m_TextComponent;

		private SerializedProperty m_Text;

		private SerializedProperty m_ContentType;

		private SerializedProperty m_LineType;

		private SerializedProperty m_InputType;

		private SerializedProperty m_CharacterValidation;

		private SerializedProperty m_InputValidator;

		private SerializedProperty m_RegexValue;

		private SerializedProperty m_KeyboardType;

		private SerializedProperty m_CharacterLimit;

		private SerializedProperty m_CaretBlinkRate;

		private SerializedProperty m_CaretWidth;

		private SerializedProperty m_CaretColor;

		private SerializedProperty m_CustomCaretColor;

		private SerializedProperty m_SelectionColor;

		private SerializedProperty m_HideMobileInput;

		private SerializedProperty m_Placeholder;

		private SerializedProperty m_VerticalScrollbar;

		private SerializedProperty m_ScrollbarScrollSensitivity;

		private SerializedProperty m_OnValueChanged;

		private SerializedProperty m_OnEndEdit;

		private SerializedProperty m_OnSelect;

		private SerializedProperty m_OnDeselect;

		private SerializedProperty m_ReadOnly;

		private SerializedProperty m_RichText;

		private SerializedProperty m_RichTextEditingAllowed;

		private SerializedProperty m_ResetOnDeActivation;

		private SerializedProperty m_RestoreOriginalTextOnEscape;

		private SerializedProperty m_OnFocusSelectAll;

		private SerializedProperty m_GlobalPointSize;

		private SerializedProperty m_GlobalFontAsset;

		private AnimBool m_CustomColor;

		protected override void OnEnable()
		{
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Expected O, but got Unknown
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Expected O, but got Unknown
			((SelectableEditor)this).OnEnable();
			m_TextViewport = ((Editor)this).get_serializedObject().FindProperty("m_TextViewport");
			m_TextComponent = ((Editor)this).get_serializedObject().FindProperty("m_TextComponent");
			m_Text = ((Editor)this).get_serializedObject().FindProperty("m_Text");
			m_ContentType = ((Editor)this).get_serializedObject().FindProperty("m_ContentType");
			m_LineType = ((Editor)this).get_serializedObject().FindProperty("m_LineType");
			m_InputType = ((Editor)this).get_serializedObject().FindProperty("m_InputType");
			m_CharacterValidation = ((Editor)this).get_serializedObject().FindProperty("m_CharacterValidation");
			m_InputValidator = ((Editor)this).get_serializedObject().FindProperty("m_InputValidator");
			m_RegexValue = ((Editor)this).get_serializedObject().FindProperty("m_RegexValue");
			m_KeyboardType = ((Editor)this).get_serializedObject().FindProperty("m_KeyboardType");
			m_CharacterLimit = ((Editor)this).get_serializedObject().FindProperty("m_CharacterLimit");
			m_CaretBlinkRate = ((Editor)this).get_serializedObject().FindProperty("m_CaretBlinkRate");
			m_CaretWidth = ((Editor)this).get_serializedObject().FindProperty("m_CaretWidth");
			m_CaretColor = ((Editor)this).get_serializedObject().FindProperty("m_CaretColor");
			m_CustomCaretColor = ((Editor)this).get_serializedObject().FindProperty("m_CustomCaretColor");
			m_SelectionColor = ((Editor)this).get_serializedObject().FindProperty("m_SelectionColor");
			m_HideMobileInput = ((Editor)this).get_serializedObject().FindProperty("m_HideMobileInput");
			m_Placeholder = ((Editor)this).get_serializedObject().FindProperty("m_Placeholder");
			m_VerticalScrollbar = ((Editor)this).get_serializedObject().FindProperty("m_VerticalScrollbar");
			m_ScrollbarScrollSensitivity = ((Editor)this).get_serializedObject().FindProperty("m_ScrollSensitivity");
			m_OnValueChanged = ((Editor)this).get_serializedObject().FindProperty("m_OnValueChanged");
			m_OnEndEdit = ((Editor)this).get_serializedObject().FindProperty("m_OnEndEdit");
			m_OnSelect = ((Editor)this).get_serializedObject().FindProperty("m_OnSelect");
			m_OnDeselect = ((Editor)this).get_serializedObject().FindProperty("m_OnDeselect");
			m_ReadOnly = ((Editor)this).get_serializedObject().FindProperty("m_ReadOnly");
			m_RichText = ((Editor)this).get_serializedObject().FindProperty("m_RichText");
			m_RichTextEditingAllowed = ((Editor)this).get_serializedObject().FindProperty("m_isRichTextEditingAllowed");
			m_ResetOnDeActivation = ((Editor)this).get_serializedObject().FindProperty("m_ResetOnDeActivation");
			m_RestoreOriginalTextOnEscape = ((Editor)this).get_serializedObject().FindProperty("m_RestoreOriginalTextOnEscape");
			m_OnFocusSelectAll = ((Editor)this).get_serializedObject().FindProperty("m_OnFocusSelectAll");
			m_GlobalPointSize = ((Editor)this).get_serializedObject().FindProperty("m_GlobalPointSize");
			m_GlobalFontAsset = ((Editor)this).get_serializedObject().FindProperty("m_GlobalFontAsset");
			m_CustomColor = new AnimBool(m_CustomCaretColor.get_boolValue());
			((BaseAnimValue<bool>)(object)m_CustomColor).valueChanged.AddListener(new UnityAction(((Editor)this).Repaint));
			TMP_UIStyleManager.GetUIStyles();
		}

		protected override void OnDisable()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			((SelectableEditor)this).OnDisable();
			((BaseAnimValue<bool>)(object)m_CustomColor).valueChanged.RemoveListener(new UnityAction(((Editor)this).Repaint));
		}

		public override void OnInspectorGUI()
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Expected O, but got Unknown
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Expected O, but got Unknown
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04df: Expected O, but got Unknown
			//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Expected O, but got Unknown
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0521: Expected O, but got Unknown
			//IL_054c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_0589: Expected O, but got Unknown
			((Editor)this).get_serializedObject().Update();
			((SelectableEditor)this).OnInspectorGUI();
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(m_TextViewport, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_TextComponent, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			TextMeshProUGUI textMeshProUGUI = null;
			if (m_TextComponent != null && m_TextComponent.get_objectReferenceValue() != (Object)null)
			{
				textMeshProUGUI = m_TextComponent.get_objectReferenceValue() as TextMeshProUGUI;
			}
			EditorGUI.BeginDisabledGroup(m_TextComponent == null || m_TextComponent.get_objectReferenceValue() == (Object)null);
			Rect controlRect = EditorGUILayout.GetControlRect(false, 25f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUIUtility.set_labelWidth(130f);
			((Rect)(ref controlRect)).set_y(((Rect)(ref controlRect)).get_y() + 2f);
			GUI.Label(controlRect, "<b>TEXT INPUT BOX</b>" + (m_foldout.textInput ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label);
			if (GUI.Button(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width() - 150f, ((Rect)(ref controlRect)).get_height()), GUIContent.none, GUI.get_skin().get_label()))
			{
				m_foldout.textInput = !m_foldout.textInput;
			}
			if (m_foldout.textInput)
			{
				EditorGUI.BeginChangeCheck();
				m_Text.set_stringValue(EditorGUILayout.TextArea(m_Text.get_stringValue(), TMP_UIStyleManager.TextAreaBoxEditor, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(125f),
					GUILayout.ExpandWidth(true)
				}));
			}
			if (GUILayout.Button("<b>INPUT FIELD SETTINGS</b>" + (m_foldout.fontSettings ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.fontSettings = !m_foldout.fontSettings;
			}
			if (m_foldout.fontSettings)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_GlobalFontAsset, new GUIContent("Font Asset", "Set the Font Asset for both Placeholder and Input Field text object."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					(((Editor)this).get_target() as TMP_InputField).SetGlobalFontAsset(m_GlobalFontAsset.get_objectReferenceValue() as TMP_FontAsset);
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_GlobalPointSize, new GUIContent("Point Size", "Set the point size of both Placeholder and Input Field text object."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					(((Editor)this).get_target() as TMP_InputField).SetGlobalPointSize(m_GlobalPointSize.get_floatValue());
				}
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_CharacterLimit, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(m_ContentType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (!m_ContentType.get_hasMultipleDifferentValues())
				{
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
					if (m_ContentType.get_enumValueIndex() == 0 || m_ContentType.get_enumValueIndex() == 1 || m_ContentType.get_enumValueIndex() == 9)
					{
						EditorGUI.BeginChangeCheck();
						EditorGUILayout.PropertyField(m_LineType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						if (EditorGUI.EndChangeCheck() && (Object)(object)textMeshProUGUI != (Object)null)
						{
							if (m_LineType.get_enumValueIndex() == 0)
							{
								textMeshProUGUI.enableWordWrapping = false;
							}
							else
							{
								textMeshProUGUI.enableWordWrapping = true;
							}
						}
					}
					if (m_ContentType.get_enumValueIndex() == 9)
					{
						EditorGUILayout.PropertyField(m_InputType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(m_KeyboardType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						EditorGUILayout.PropertyField(m_CharacterValidation, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						if (m_CharacterValidation.get_enumValueIndex() == 6)
						{
							EditorGUILayout.PropertyField(m_RegexValue, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						}
						else if (m_CharacterValidation.get_enumValueIndex() == 8)
						{
							EditorGUILayout.PropertyField(m_InputValidator, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						}
					}
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				}
				EditorGUILayout.Space();
				EditorGUILayout.PropertyField(m_Placeholder, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_VerticalScrollbar, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (m_VerticalScrollbar.get_objectReferenceValue() != (Object)null)
				{
					EditorGUILayout.PropertyField(m_ScrollbarScrollSensitivity, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
				EditorGUILayout.PropertyField(m_CaretBlinkRate, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_CaretWidth, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_CustomCaretColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				((BaseAnimValue<bool>)(object)m_CustomColor).set_target(m_CustomCaretColor.get_boolValue());
				if (EditorGUILayout.BeginFadeGroup(m_CustomColor.get_faded()))
				{
					EditorGUILayout.PropertyField(m_CaretColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
				EditorGUILayout.EndFadeGroup();
				EditorGUILayout.PropertyField(m_SelectionColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			if (GUILayout.Button("<b>CONTROL SETTINGS</b>" + (m_foldout.extraSettings ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.extraSettings = !m_foldout.extraSettings;
			}
			if (m_foldout.extraSettings)
			{
				EditorGUILayout.PropertyField(m_OnFocusSelectAll, new GUIContent("OnFocus - Select All", "Should all the text be selected when the Input Field is selected."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_ResetOnDeActivation, new GUIContent("Reset On DeActivation", "Should the Text and Caret position be reset when Input Field is DeActivated."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_RestoreOriginalTextOnEscape, new GUIContent("Restore On ESC Key", "Should the original text be restored when pressing ESC."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_HideMobileInput, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_ReadOnly, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_RichText, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUIUtility.set_labelWidth(140f);
				EditorGUILayout.PropertyField(m_RichTextEditingAllowed, new GUIContent("Allow Rich Text Editing"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUIUtility.set_labelWidth(130f);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(m_OnValueChanged, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_OnEndEdit, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_OnSelect, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_OnDeselect, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.EndDisabledGroup();
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		public TMP_InputFieldEditor()
			: this()
		{
		}
	}
}
