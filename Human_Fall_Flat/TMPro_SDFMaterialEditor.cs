using System;
using System.Linq;
using System.Runtime.InteropServices;
using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

public class TMPro_SDFMaterialEditor : MaterialEditor
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	private struct m_foldout
	{
		public static bool editorPanel = true;

		public static bool face = true;

		public static bool outline = true;

		public static bool underlay = false;

		public static bool bevel = false;

		public static bool light = false;

		public static bool bump = false;

		public static bool env = false;

		public static bool glow = false;

		public static bool debug = false;
	}

	private enum FoldoutType
	{
		face,
		outline,
		underlay,
		bevel,
		light,
		bump,
		env,
		glow,
		debug
	}

	private enum WarningTypes
	{
		None,
		ShaderMismatch,
		FontAtlasMismatch
	}

	private enum ShaderTypes
	{
		None,
		Bitmap,
		SDF
	}

	private enum Bevel_Types
	{
		OuterBevel,
		InnerBevel
	}

	private enum Underlay_Types
	{
		Normal,
		Inner
	}

	private string m_warningMsg;

	private WarningTypes m_warning;

	private static int m_eventID;

	private TMP_Text m_textComponent;

	private MaterialProperty m_faceColor;

	private MaterialProperty m_faceTex;

	private MaterialProperty m_faceUVSpeedX;

	private MaterialProperty m_faceUVSpeedY;

	private MaterialProperty m_faceDilate;

	private MaterialProperty m_faceShininess;

	private MaterialProperty m_outlineColor;

	private MaterialProperty m_outlineTex;

	private MaterialProperty m_outlineUVSpeedX;

	private MaterialProperty m_outlineUVSpeedY;

	private MaterialProperty m_outlineThickness;

	private MaterialProperty m_outlineSoftness;

	private MaterialProperty m_outlineShininess;

	private MaterialProperty m_bevel;

	private MaterialProperty m_bevelOffset;

	private MaterialProperty m_bevelWidth;

	private MaterialProperty m_bevelClamp;

	private MaterialProperty m_bevelRoundness;

	private MaterialProperty m_underlayColor;

	private MaterialProperty m_underlayOffsetX;

	private MaterialProperty m_underlayOffsetY;

	private MaterialProperty m_underlayDilate;

	private MaterialProperty m_underlaySoftness;

	private MaterialProperty m_lightAngle;

	private MaterialProperty m_specularColor;

	private MaterialProperty m_specularPower;

	private MaterialProperty m_reflectivity;

	private MaterialProperty m_diffuse;

	private MaterialProperty m_ambientLight;

	private MaterialProperty m_bumpMap;

	private MaterialProperty m_bumpFace;

	private MaterialProperty m_bumpOutline;

	private MaterialProperty m_reflectFaceColor;

	private MaterialProperty m_reflectOutlineColor;

	private MaterialProperty m_reflectTex;

	private MaterialProperty m_reflectRotation;

	private MaterialProperty m_specColor;

	private MaterialProperty m_glowColor;

	private MaterialProperty m_glowInner;

	private MaterialProperty m_glowOffset;

	private MaterialProperty m_glowPower;

	private MaterialProperty m_glowOuter;

	private MaterialProperty m_mainTex;

	private MaterialProperty m_texSampleWidth;

	private MaterialProperty m_texSampleHeight;

	private MaterialProperty m_gradientScale;

	private MaterialProperty m_scaleX;

	private MaterialProperty m_scaleY;

	private MaterialProperty m_PerspectiveFilter;

	private MaterialProperty m_vertexOffsetX;

	private MaterialProperty m_vertexOffsetY;

	private MaterialProperty m_maskCoord;

	private MaterialProperty m_clipRect;

	private MaterialProperty m_maskSoftnessX;

	private MaterialProperty m_maskSoftnessY;

	private MaterialProperty m_maskTex;

	private MaterialProperty m_maskTexInverse;

	private MaterialProperty m_maskTexEdgeColor;

	private MaterialProperty m_maskTexEdgeSoftness;

	private MaterialProperty m_maskTexWipeControl;

	private MaterialProperty m_stencilID;

	private MaterialProperty m_stencilOp;

	private MaterialProperty m_stencilComp;

	private MaterialProperty m_stencilReadMask;

	private MaterialProperty m_stencilWriteMask;

	private MaterialProperty m_shaderFlags;

	private MaterialProperty m_scaleRatio_A;

	private MaterialProperty m_scaleRatio_B;

	private MaterialProperty m_scaleRatio_C;

	private string[] m_bevelOptions = new string[3] { "Outer Bevel", "Inner Bevel", "--" };

	private int m_bevelSelection;

	private MaskingTypes m_mask;

	private Underlay_Types m_underlaySelection;

	private string[] m_Keywords;

	private bool isOutlineEnabled;

	private bool isRatiosEnabled;

	private bool isBevelEnabled;

	private bool isGlowEnabled;

	private bool isUnderlayEnabled;

	private bool havePropertiesChanged;

	private Rect m_inspectorStartRegion;

	private Rect m_inspectorEndRegion;

	public override void OnEnable()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		((MaterialEditor)this).OnEnable();
		TMP_UIStyleManager.GetUIStyles();
		ShaderUtilities.GetShaderPropertyIDs();
		Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Combine((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedo));
		if ((Object)(object)Selection.get_activeGameObject() != (Object)null)
		{
			m_textComponent = Selection.get_activeGameObject().GetComponent<TMP_Text>();
		}
	}

	public override void OnDisable()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		((MaterialEditor)this).OnDisable();
		Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Remove((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedo));
	}

	protected override void OnHeaderGUI()
	{
		EditorGUI.BeginChangeCheck();
		((MaterialEditor)this).OnHeaderGUI();
		if (EditorGUI.EndChangeCheck())
		{
			m_foldout.editorPanel = InternalEditorUtility.GetIsInspectorExpanded(((Editor)this).get_target());
		}
		GUI.get_skin().GetStyle("HelpBox").set_richText(true);
		if (m_warning == WarningTypes.FontAtlasMismatch)
		{
			EditorGUILayout.HelpBox(m_warningMsg, (MessageType)2);
		}
	}

	public override void OnInspectorGUI()
	{
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0614: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba1: Unknown result type (might be due to invalid IL or missing references)
		if (!m_foldout.editorPanel)
		{
			return;
		}
		Object target = ((Editor)this).get_target();
		Material val = (Material)(object)((target is Material) ? target : null);
		if (((Editor)this).get_targets().Length > 1)
		{
			for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
			{
				Object obj = ((Editor)this).get_targets()[i];
				Material val2 = (Material)(object)((obj is Material) ? obj : null);
				if ((Object)(object)val.get_shader() != (Object)(object)val2.get_shader())
				{
					return;
				}
			}
		}
		ReadMaterialProperties();
		if (!val.HasProperty(ShaderUtilities.ID_GradientScale))
		{
			m_warning = WarningTypes.ShaderMismatch;
			m_warningMsg = "The selected Shader is not compatible with the currently selected Font Asset type.";
			EditorGUILayout.HelpBox(m_warningMsg, (MessageType)2);
			return;
		}
		m_Keywords = val.get_shaderKeywords();
		isOutlineEnabled = m_Keywords.Contains("OUTLINE_ON");
		isBevelEnabled = m_Keywords.Contains("BEVEL_ON");
		isGlowEnabled = m_Keywords.Contains("GLOW_ON");
		isRatiosEnabled = !m_Keywords.Contains("RATIOS_OFF");
		if (m_Keywords.Contains("UNDERLAY_ON"))
		{
			isUnderlayEnabled = true;
			m_underlaySelection = Underlay_Types.Normal;
		}
		else if (m_Keywords.Contains("UNDERLAY_INNER"))
		{
			isUnderlayEnabled = true;
			m_underlaySelection = Underlay_Types.Inner;
		}
		else
		{
			isUnderlayEnabled = false;
		}
		if (m_Keywords.Contains("MASK_HARD"))
		{
			m_mask = MaskingTypes.MaskHard;
		}
		else if (m_Keywords.Contains("MASK_SOFT"))
		{
			m_mask = MaskingTypes.MaskSoft;
		}
		else
		{
			m_mask = MaskingTypes.MaskOff;
		}
		if (m_shaderFlags.get_hasMixedValue())
		{
			m_bevelSelection = 2;
		}
		else
		{
			m_bevelSelection = (int)m_shaderFlags.get_floatValue() & 1;
		}
		m_inspectorStartRegion = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
		EditorGUIUtility.set_labelWidth(130f);
		EditorGUIUtility.set_fieldWidth(50f);
		EditorGUI.set_indentLevel(0);
		if (GUILayout.Button("<b>Face</b> - <i>Settings</i> -", TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			m_foldout.face = !m_foldout.face;
		}
		if (m_foldout.face)
		{
			EditorGUI.BeginChangeCheck();
			EditorGUI.set_indentLevel(1);
			((MaterialEditor)this).ColorProperty(m_faceColor, "Color");
			if (val.HasProperty("_FaceTex"))
			{
				DrawTextureProperty(m_faceTex, "Texture");
				DrawUVProperty((MaterialProperty[])(object)new MaterialProperty[2] { m_faceUVSpeedX, m_faceUVSpeedY }, "UV Speed");
			}
			DrawRangeProperty(m_outlineSoftness, "Softness");
			DrawRangeProperty(m_faceDilate, "Dilate");
			if (val.HasProperty("_FaceShininess"))
			{
				DrawRangeProperty(m_faceShininess, "Gloss");
			}
			if (EditorGUI.EndChangeCheck())
			{
				havePropertiesChanged = true;
			}
		}
		if (val.HasProperty("_OutlineColor"))
		{
			if (val.HasProperty("_Bevel"))
			{
				if (GUILayout.Button("<b>Outline</b> - <i>Settings</i> -", TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					m_foldout.outline = !m_foldout.outline;
				}
			}
			else
			{
				isOutlineEnabled = DrawTogglePanel(FoldoutType.outline, "<b>Outline</b> - <i>Settings</i> -", isOutlineEnabled, "OUTLINE_ON");
			}
			EditorGUI.set_indentLevel(0);
			if (m_foldout.outline)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				((MaterialEditor)this).ColorProperty(m_outlineColor, "Color");
				if (val.HasProperty("_OutlineTex"))
				{
					DrawTextureProperty(m_outlineTex, "Texture");
					DrawUVProperty((MaterialProperty[])(object)new MaterialProperty[2] { m_outlineUVSpeedX, m_outlineUVSpeedY }, "UV Speed");
				}
				DrawRangeProperty(m_outlineThickness, "Thickness");
				if (val.HasProperty("_OutlineShininess"))
				{
					DrawRangeProperty(m_outlineShininess, "Gloss");
				}
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_UnderlayColor"))
		{
			string keyword = ((m_underlaySelection == Underlay_Types.Normal) ? "UNDERLAY_ON" : "UNDERLAY_INNER");
			isUnderlayEnabled = DrawTogglePanel(FoldoutType.underlay, "<b>Underlay</b> - <i>Settings</i> -", isUnderlayEnabled, keyword);
			if (m_foldout.underlay)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				m_underlaySelection = (Underlay_Types)(object)EditorGUILayout.EnumPopup("Underlay Type", (Enum)m_underlaySelection, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (GUI.get_changed())
				{
					SetUnderlayKeywords();
				}
				((MaterialEditor)this).ColorProperty(m_underlayColor, "Color");
				DrawRangeProperty(m_underlayOffsetX, "OffsetX");
				DrawRangeProperty(m_underlayOffsetY, "OffsetY");
				DrawRangeProperty(m_underlayDilate, "Dilate");
				DrawRangeProperty(m_underlaySoftness, "Softness");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_Bevel"))
		{
			isBevelEnabled = DrawTogglePanel(FoldoutType.bevel, "<b>Bevel</b> - <i>Settings</i> -", isBevelEnabled, "BEVEL_ON");
			if (m_foldout.bevel)
			{
				EditorGUI.set_indentLevel(1);
				GUI.set_changed(false);
				m_bevelSelection = EditorGUILayout.Popup("Type", m_bevelSelection, m_bevelOptions, (GUILayoutOption[])(object)new GUILayoutOption[0]) & 1;
				if (GUI.get_changed())
				{
					havePropertiesChanged = true;
					m_shaderFlags.set_floatValue((float)m_bevelSelection);
				}
				EditorGUI.BeginChangeCheck();
				DrawRangeProperty(m_bevel, "Amount");
				DrawRangeProperty(m_bevelOffset, "Offset");
				DrawRangeProperty(m_bevelWidth, "Width");
				DrawRangeProperty(m_bevelRoundness, "Roundness");
				DrawRangeProperty(m_bevelClamp, "Clamp");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_SpecularColor") || val.HasProperty("_SpecColor"))
		{
			isBevelEnabled = DrawTogglePanel(FoldoutType.light, "<b>Lighting</b> - <i>Settings</i> -", isBevelEnabled, "BEVEL_ON");
			if (m_foldout.light)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				if (val.HasProperty("_LightAngle"))
				{
					DrawRangeProperty(m_lightAngle, "Light Angle");
					((MaterialEditor)this).ColorProperty(m_specularColor, "Specular Color");
					DrawRangeProperty(m_specularPower, "Specular Power");
					DrawRangeProperty(m_reflectivity, "Reflectivity Power");
					DrawRangeProperty(m_diffuse, "Diffuse Shadow");
					DrawRangeProperty(m_ambientLight, "Ambient Shadow");
				}
				else
				{
					((MaterialEditor)this).ColorProperty(m_specColor, "Specular Color");
				}
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_BumpMap"))
		{
			isBevelEnabled = DrawTogglePanel(FoldoutType.bump, "<b>BumpMap</b> - <i>Settings</i> -", isBevelEnabled, "BEVEL_ON");
			if (m_foldout.bump)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				DrawTextureProperty(m_bumpMap, "Texture");
				DrawRangeProperty(m_bumpFace, "Face");
				DrawRangeProperty(m_bumpOutline, "Outline");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_Cube"))
		{
			isBevelEnabled = DrawTogglePanel(FoldoutType.env, "<b>EnvMap</b> - <i>Settings</i> -", isBevelEnabled, "BEVEL_ON");
			if (m_foldout.env)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				((MaterialEditor)this).ColorProperty(m_reflectFaceColor, "Face Color");
				((MaterialEditor)this).ColorProperty(m_reflectOutlineColor, "Outline Color");
				DrawTextureProperty(m_reflectTex, "Texture");
				if (val.HasProperty("_Cube"))
				{
					DrawVectorProperty(m_reflectRotation, "EnvMap Rotation");
				}
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_GlowColor"))
		{
			isGlowEnabled = DrawTogglePanel(FoldoutType.glow, "<b>Glow</b> - <i>Settings</i> -", isGlowEnabled, "GLOW_ON");
			if (m_foldout.glow)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUI.set_indentLevel(1);
				((MaterialEditor)this).ColorProperty(m_glowColor, "Color");
				DrawRangeProperty(m_glowOffset, "Offset");
				DrawRangeProperty(m_glowInner, "Inner");
				DrawRangeProperty(m_glowOuter, "Outer");
				DrawRangeProperty(m_glowPower, "Power");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		if (val.HasProperty("_GradientScale"))
		{
			EditorGUI.set_indentLevel(0);
			if (GUILayout.Button("<b>Debug</b> - <i>Settings</i> -", TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.debug = !m_foldout.debug;
			}
			if (m_foldout.debug)
			{
				EditorGUI.set_indentLevel(1);
				EditorGUI.BeginChangeCheck();
				DrawTextureProperty(m_mainTex, "Font Atlas");
				DrawFloatProperty(m_gradientScale, "Gradient Scale");
				DrawFloatProperty(m_texSampleWidth, "Texture Width");
				DrawFloatProperty(m_texSampleHeight, "Texture Height");
				GUILayout.Space(20f);
				DrawFloatProperty(m_scaleX, "Scale X");
				DrawFloatProperty(m_scaleY, "Scale Y");
				DrawRangeProperty(m_PerspectiveFilter, "Perspective Filter");
				GUILayout.Space(20f);
				DrawFloatProperty(m_vertexOffsetX, "Offset X");
				DrawFloatProperty(m_vertexOffsetY, "Offset Y");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				if (val.HasProperty("_ClipRect"))
				{
					GUILayout.Space(15f);
					m_mask = (MaskingTypes)(object)EditorGUILayout.EnumPopup("Mask", (Enum)m_mask, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					if (GUI.get_changed())
					{
						havePropertiesChanged = true;
						SetMaskKeywords(m_mask);
					}
					if (m_mask != 0)
					{
						EditorGUI.BeginChangeCheck();
						Draw2DBoundsProperty(m_maskCoord, "Mask Bounds");
						DrawFloatProperty(m_maskSoftnessX, "Softness X");
						DrawFloatProperty(m_maskSoftnessY, "Softness Y");
						if (val.HasProperty("_MaskEdgeColor"))
						{
							DrawTextureProperty(m_maskTex, "Mask Texture");
							bool flag = ((m_maskTexInverse.get_floatValue() != 0f) ? true : false);
							flag = EditorGUILayout.Toggle("Inverse Mask", flag, (GUILayoutOption[])(object)new GUILayoutOption[0]);
							((MaterialEditor)this).ColorProperty(m_maskTexEdgeColor, "Edge Color");
							((MaterialEditor)this).RangeProperty(m_maskTexEdgeSoftness, "Edge Softness");
							((MaterialEditor)this).RangeProperty(m_maskTexWipeControl, "Wipe Position");
							if (EditorGUI.EndChangeCheck())
							{
								m_maskTexInverse.set_floatValue((float)(flag ? 1 : 0));
								havePropertiesChanged = true;
							}
						}
					}
					GUILayout.Space(15f);
				}
				EditorGUI.BeginChangeCheck();
				Draw2DRectBoundsProperty(m_clipRect, "_ClipRect");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
				GUILayout.Space(20f);
				if (val.HasProperty("_Stencil"))
				{
					((MaterialEditor)this).FloatProperty(m_stencilID, "Stencil ID");
					((MaterialEditor)this).FloatProperty(m_stencilComp, "Stencil Comp");
				}
				GUILayout.Space(20f);
				GUI.set_changed(false);
				isRatiosEnabled = EditorGUILayout.Toggle("Enable Ratios?", isRatiosEnabled, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (GUI.get_changed())
				{
					SetKeyword(!isRatiosEnabled, "RATIOS_OFF");
				}
				EditorGUI.BeginChangeCheck();
				DrawFloatProperty(m_scaleRatio_A, "Scale Ratio A");
				DrawFloatProperty(m_scaleRatio_B, "Scale Ratio B");
				DrawFloatProperty(m_scaleRatio_C, "Scale Ratio C");
				if (EditorGUI.EndChangeCheck())
				{
					havePropertiesChanged = true;
				}
			}
		}
		m_inspectorEndRegion = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
		DragAndDropGUI();
		if (havePropertiesChanged)
		{
			havePropertiesChanged = false;
			((MaterialEditor)this).PropertiesChanged();
			EditorUtility.SetDirty(((Editor)this).get_target());
			Object target2 = ((Editor)this).get_target();
			TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, (Material)(object)((target2 is Material) ? target2 : null));
		}
	}

	private void DragAndDropGUI()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Invalid comparison between Unknown and I4
		Event current = Event.get_current();
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref m_inspectorStartRegion)).get_x(), ((Rect)(ref m_inspectorStartRegion)).get_y(), ((Rect)(ref m_inspectorEndRegion)).get_width(), ((Rect)(ref m_inspectorEndRegion)).get_y() - ((Rect)(ref m_inspectorStartRegion)).get_y());
		EventType type = current.get_type();
		if (type - 9 > 1 || !((Rect)(ref val)).Contains(current.get_mousePosition()))
		{
			return;
		}
		DragAndDrop.set_visualMode((DragAndDropVisualMode)4);
		if ((int)current.get_type() == 10)
		{
			DragAndDrop.AcceptDrag();
			Object target = ((Editor)this).get_target();
			Material val2 = (Material)(object)((target is Material) ? target : null);
			Texture texture = val2.GetTexture(ShaderUtilities.ID_MainTex);
			Object obj = DragAndDrop.get_objectReferences()[0];
			Material val3 = (Material)(object)((obj is Material) ? obj : null);
			Texture texture2 = val3.GetTexture(ShaderUtilities.ID_MainTex);
			TMP_FontAsset tMP_FontAsset = null;
			if ((Object)(object)val3 == (Object)null || (Object)(object)val3 == (Object)(object)val2 || (Object)(object)val3 == (Object)null || (Object)(object)texture2 == (Object)null)
			{
				return;
			}
			if (((Object)texture2).GetInstanceID() != ((Object)texture).GetInstanceID())
			{
				tMP_FontAsset = TMP_EditorUtility.FindMatchingFontAsset(val3);
				if ((Object)(object)tMP_FontAsset == (Object)null)
				{
					return;
				}
			}
			GameObject[] gameObjects = Selection.get_gameObjects();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				if ((Object)(object)tMP_FontAsset != (Object)null)
				{
					TMP_Text component = gameObjects[i].GetComponent<TMP_Text>();
					if ((Object)(object)component != (Object)null)
					{
						Undo.RecordObject((Object)(object)component, "Font Asset Change");
						component.font = tMP_FontAsset;
					}
				}
				TMPro_EventManager.ON_DRAG_AND_DROP_MATERIAL_CHANGED(gameObjects[i], val2, val3);
				EditorUtility.SetDirty((Object)(object)gameObjects[i]);
			}
		}
		current.Use();
	}

	private void OnUndoRedo()
	{
		int currentGroup = Undo.GetCurrentGroup();
		int eventID = m_eventID;
		if (currentGroup != eventID)
		{
			Object target = ((Editor)this).get_target();
			TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, (Material)(object)((target is Material) ? target : null));
			m_eventID = currentGroup;
		}
	}

	private UndoPropertyModification[] OnUndoRedoEvent(UndoPropertyModification[] modifications)
	{
		return modifications;
	}

	private bool DrawTogglePanel(FoldoutType type, string label, bool toggle, string keyword)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		EditorGUI.set_indentLevel(0);
		Rect controlRect = EditorGUILayout.GetControlRect(false, 22f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.Label(controlRect, GUIContent.none, TMP_UIStyleManager.Group_Label);
		if (GUI.Button(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), 250f, ((Rect)(ref controlRect)).get_height()), label, TMP_UIStyleManager.Group_Label_Left))
		{
			switch (type)
			{
			case FoldoutType.outline:
				m_foldout.outline = !m_foldout.outline;
				break;
			case FoldoutType.underlay:
				m_foldout.underlay = !m_foldout.underlay;
				break;
			case FoldoutType.bevel:
				m_foldout.bevel = !m_foldout.bevel;
				break;
			case FoldoutType.light:
				m_foldout.light = !m_foldout.light;
				break;
			case FoldoutType.bump:
				m_foldout.bump = !m_foldout.bump;
				break;
			case FoldoutType.env:
				m_foldout.env = !m_foldout.env;
				break;
			case FoldoutType.glow:
				m_foldout.glow = !m_foldout.glow;
				break;
			}
		}
		EditorGUIUtility.set_labelWidth(70f);
		EditorGUI.BeginChangeCheck();
		Object target = ((Editor)this).get_target();
		if (!((Material)((target is Material) ? target : null)).HasProperty("_FaceShininess") || keyword != "BEVEL_ON")
		{
			toggle = EditorGUI.Toggle(new Rect(((Rect)(ref controlRect)).get_width() - 90f, ((Rect)(ref controlRect)).get_y() + 3f, 90f, 22f), new GUIContent("Enable ->"), toggle);
			if (EditorGUI.EndChangeCheck())
			{
				SetKeyword(toggle, keyword);
				havePropertiesChanged = true;
			}
		}
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
		return toggle;
	}

	private void DrawUVProperty(MaterialProperty[] properties, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = new Rect(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width() - 55f, 20f);
		Rect val2 = default(Rect);
		((Rect)(ref val2))._002Ector(130f, ((Rect)(ref controlRect)).get_y(), 80f, 18f);
		GUI.Label(val, label);
		EditorGUIUtility.set_labelWidth(35f);
		((MaterialEditor)this).FloatProperty(val2, properties[0], "X");
		EditorGUIUtility.set_labelWidth(35f);
		((MaterialEditor)this).FloatProperty(new Rect(((Rect)(ref val2)).get_x() + 70f, ((Rect)(ref val2)).get_y(), ((Rect)(ref val2)).get_width(), ((Rect)(ref val2)).get_height()), properties[1], "Y");
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawSliderProperty(MaterialProperty property, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width() - 55f, 20f);
		Rect val2 = default(Rect);
		((Rect)(ref val2))._002Ector(((Rect)(ref controlRect)).get_width() - 46f, ((Rect)(ref controlRect)).get_y(), 60f, 18f);
		((MaterialEditor)this).RangeProperty(val, property, label);
		EditorGUIUtility.set_labelWidth(10f);
		((MaterialEditor)this).FloatProperty(new Rect(val2), property, string.Empty);
		if (!property.get_hasMixedValue())
		{
			property.set_floatValue(Mathf.Round(property.get_floatValue() * 1000f) / 1000f);
		}
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawTextureProperty(MaterialProperty property, string label)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		EditorGUIUtility.set_fieldWidth(70f);
		Rect controlRect = EditorGUILayout.GetControlRect(false, 75f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.Label(new Rect(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y() + 5f, 100f, ((Rect)(ref controlRect)).get_height()), label);
		((MaterialEditor)this).TextureProperty(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y() + 5f, 200f, ((Rect)(ref controlRect)).get_height()), property, string.Empty, false);
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawFloatProperty(MaterialProperty property, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), 225f, 18f);
		((MaterialEditor)this).FloatProperty(val, property, label);
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawRangeProperty(MaterialProperty property, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 16f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y(), ((Rect)(ref controlRect)).get_width(), ((Rect)(ref controlRect)).get_height());
		GUI.Label(val, label);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + 100f);
		((Rect)(ref val)).set_width(((Rect)(ref val)).get_width() - 115f);
		((MaterialEditor)this).RangeProperty(val, property, string.Empty);
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawVectorProperty(MaterialProperty property, string label)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		EditorGUIUtility.set_labelWidth(160f);
		Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = new Rect(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width() - 120f, 18f);
		Rect val2 = default(Rect);
		((Rect)(ref val2))._002Ector(175f, ((Rect)(ref controlRect)).get_y() - 14f, ((Rect)(ref controlRect)).get_width() - 160f, 18f);
		GUI.Label(val, label);
		((MaterialEditor)this).VectorProperty(val2, property, "");
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void DrawVectorProperty(MaterialProperty property, string label, int floatCount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		EditorGUIUtility.set_labelWidth(160f);
		Rect controlRect = EditorGUILayout.GetControlRect(false, 20f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = new Rect(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width() - 120f, 18f);
		Rect val2 = default(Rect);
		((Rect)(ref val2))._002Ector(175f, ((Rect)(ref controlRect)).get_y() - 14f, ((Rect)(ref controlRect)).get_width() - 160f, 18f);
		GUI.Label(val, label);
		((MaterialEditor)this).VectorProperty(val2, property, "");
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void Draw2DBoundsProperty(MaterialProperty property, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 22f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width() - 15f, 18f);
		GUI.Label(val, label);
		EditorGUIUtility.set_labelWidth(30f);
		float num = (((Rect)(ref val)).get_width() - 15f) / 5f;
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (labelWidth - 30f));
		Vector4 vectorValue = property.get_vectorValue();
		((Rect)(ref val)).set_width(num);
		vectorValue.x = EditorGUI.FloatField(val, "X", vectorValue.x);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (num - 14f));
		vectorValue.y = EditorGUI.FloatField(val, "Y", vectorValue.y);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (num - 14f));
		vectorValue.z = EditorGUI.FloatField(val, "W", vectorValue.z);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (num - 14f));
		vectorValue.w = EditorGUI.FloatField(val, "H", vectorValue.w);
		((Rect)(ref val)).set_x(((Rect)(ref controlRect)).get_width() - 11f);
		((Rect)(ref val)).set_width(25f);
		property.set_vectorValue(vectorValue);
		if (GUI.Button(val, "X"))
		{
			Renderer component = Selection.get_activeGameObject().GetComponent<Renderer>();
			if ((Object)(object)component != (Object)null)
			{
				Bounds bounds = component.get_bounds();
				float num2 = Mathf.Round(((Bounds)(ref bounds)).get_extents().x * 1000f) / 1000f;
				bounds = component.get_bounds();
				property.set_vectorValue(new Vector4(0f, 0f, num2, Mathf.Round(((Bounds)(ref bounds)).get_extents().y * 1000f) / 1000f));
			}
		}
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void Draw2DRectBoundsProperty(MaterialProperty property, string label)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		Rect controlRect = EditorGUILayout.GetControlRect(false, 22f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Rect val = default(Rect);
		((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x() + 15f, ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width() - 15f, 18f);
		GUI.Label(val, label);
		float num = (((Rect)(ref val)).get_width() - ((Rect)(ref val)).get_x() - 30f) / 4f;
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (labelWidth - 30f));
		Vector4 vectorValue = property.get_vectorValue();
		EditorGUIUtility.set_labelWidth(40f);
		((Rect)(ref val)).set_width(num + 8f);
		vectorValue.x = EditorGUI.FloatField(val, "BL", vectorValue.x);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num);
		((Rect)(ref val)).set_width(num - 18f);
		vectorValue.y = EditorGUI.FloatField(val, "", vectorValue.y);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (num - 24f));
		((Rect)(ref val)).set_width(num + 8f);
		vectorValue.z = EditorGUI.FloatField(val, "TR", vectorValue.z);
		((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num);
		((Rect)(ref val)).set_width(num - 18f);
		vectorValue.w = EditorGUI.FloatField(val, "", vectorValue.w);
		property.set_vectorValue(vectorValue);
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
	}

	private void SetKeyword(bool state, string keyword)
	{
		Undo.RecordObjects(((Editor)this).get_targets(), "Keyword State Change");
		for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
		{
			Object obj = ((Editor)this).get_targets()[i];
			Material val = (Material)(object)((obj is Material) ? obj : null);
			if (state)
			{
				if (!(keyword == "UNDERLAY_ON"))
				{
					if (keyword == "UNDERLAY_INNER")
					{
						val.EnableKeyword("UNDERLAY_INNER");
						val.DisableKeyword("UNDERLAY_ON");
					}
					else
					{
						val.EnableKeyword(keyword);
					}
				}
				else
				{
					val.EnableKeyword("UNDERLAY_ON");
					val.DisableKeyword("UNDERLAY_INNER");
				}
			}
			else if (!(keyword == "UNDERLAY_ON"))
			{
				if (keyword == "UNDERLAY_INNER")
				{
					val.DisableKeyword("UNDERLAY_INNER");
					val.DisableKeyword("UNDERLAY_ON");
				}
				else
				{
					val.DisableKeyword(keyword);
				}
			}
			else
			{
				val.DisableKeyword("UNDERLAY_ON");
				val.DisableKeyword("UNDERLAY_INNER");
			}
		}
	}

	private void SetUnderlayKeywords()
	{
		for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
		{
			Object obj = ((Editor)this).get_targets()[i];
			Material val = (Material)(object)((obj is Material) ? obj : null);
			if (m_underlaySelection == Underlay_Types.Normal)
			{
				val.EnableKeyword("UNDERLAY_ON");
				val.DisableKeyword("UNDERLAY_INNER");
			}
			else if (m_underlaySelection == Underlay_Types.Inner)
			{
				val.EnableKeyword("UNDERLAY_INNER");
				val.DisableKeyword("UNDERLAY_ON");
			}
		}
	}

	private void SetMaskID(MaskingTypes id)
	{
		for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
		{
			Object obj = ((Editor)this).get_targets()[i];
			Material val = (Material)(object)((obj is Material) ? obj : null);
			switch (id)
			{
			case MaskingTypes.MaskHard:
				val.SetFloat("_MaskID", (float)id);
				break;
			case MaskingTypes.MaskSoft:
				val.SetFloat("_MaskID", (float)id);
				break;
			case MaskingTypes.MaskOff:
				val.SetFloat("_MaskID", (float)id);
				break;
			}
		}
	}

	private void SetMaskKeywords(MaskingTypes mask)
	{
		for (int i = 0; i < ((Editor)this).get_targets().Length; i++)
		{
			Object obj = ((Editor)this).get_targets()[i];
			Material val = (Material)(object)((obj is Material) ? obj : null);
			switch (mask)
			{
			case MaskingTypes.MaskHard:
				val.EnableKeyword("MASK_HARD");
				val.DisableKeyword("MASK_SOFT");
				break;
			case MaskingTypes.MaskSoft:
				val.EnableKeyword("MASK_SOFT");
				val.DisableKeyword("MASK_HARD");
				break;
			case MaskingTypes.MaskOff:
				val.DisableKeyword("MASK_HARD");
				val.DisableKeyword("MASK_SOFT");
				break;
			}
		}
	}

	private void ReadMaterialProperties()
	{
		Object[] targets = ((Editor)this).get_targets();
		m_faceColor = MaterialEditor.GetMaterialProperty(targets, "_FaceColor");
		m_faceTex = MaterialEditor.GetMaterialProperty(targets, "_FaceTex");
		m_faceUVSpeedX = MaterialEditor.GetMaterialProperty(targets, "_FaceUVSpeedX");
		m_faceUVSpeedY = MaterialEditor.GetMaterialProperty(targets, "_FaceUVSpeedY");
		m_faceDilate = MaterialEditor.GetMaterialProperty(targets, "_FaceDilate");
		m_faceShininess = MaterialEditor.GetMaterialProperty(targets, "_FaceShininess");
		m_outlineColor = MaterialEditor.GetMaterialProperty(targets, "_OutlineColor");
		m_outlineThickness = MaterialEditor.GetMaterialProperty(targets, "_OutlineWidth");
		m_outlineSoftness = MaterialEditor.GetMaterialProperty(targets, "_OutlineSoftness");
		m_outlineTex = MaterialEditor.GetMaterialProperty(targets, "_OutlineTex");
		m_outlineUVSpeedX = MaterialEditor.GetMaterialProperty(targets, "_OutlineUVSpeedX");
		m_outlineUVSpeedY = MaterialEditor.GetMaterialProperty(targets, "_OutlineUVSpeedY");
		m_outlineShininess = MaterialEditor.GetMaterialProperty(targets, "_OutlineShininess");
		m_underlayColor = MaterialEditor.GetMaterialProperty(targets, "_UnderlayColor");
		m_underlayOffsetX = MaterialEditor.GetMaterialProperty(targets, "_UnderlayOffsetX");
		m_underlayOffsetY = MaterialEditor.GetMaterialProperty(targets, "_UnderlayOffsetY");
		m_underlayDilate = MaterialEditor.GetMaterialProperty(targets, "_UnderlayDilate");
		m_underlaySoftness = MaterialEditor.GetMaterialProperty(targets, "_UnderlaySoftness");
		m_bumpMap = MaterialEditor.GetMaterialProperty(targets, "_BumpMap");
		m_bumpFace = MaterialEditor.GetMaterialProperty(targets, "_BumpFace");
		m_bumpOutline = MaterialEditor.GetMaterialProperty(targets, "_BumpOutline");
		m_bevel = MaterialEditor.GetMaterialProperty(targets, "_Bevel");
		m_bevelOffset = MaterialEditor.GetMaterialProperty(targets, "_BevelOffset");
		m_bevelWidth = MaterialEditor.GetMaterialProperty(targets, "_BevelWidth");
		m_bevelClamp = MaterialEditor.GetMaterialProperty(targets, "_BevelClamp");
		m_bevelRoundness = MaterialEditor.GetMaterialProperty(targets, "_BevelRoundness");
		m_specColor = MaterialEditor.GetMaterialProperty(targets, "_SpecColor");
		m_lightAngle = MaterialEditor.GetMaterialProperty(targets, "_LightAngle");
		m_specularColor = MaterialEditor.GetMaterialProperty(targets, "_SpecularColor");
		m_specularPower = MaterialEditor.GetMaterialProperty(targets, "_SpecularPower");
		m_reflectivity = MaterialEditor.GetMaterialProperty(targets, "_Reflectivity");
		m_diffuse = MaterialEditor.GetMaterialProperty(targets, "_Diffuse");
		m_ambientLight = MaterialEditor.GetMaterialProperty(targets, "_Ambient");
		m_glowColor = MaterialEditor.GetMaterialProperty(targets, "_GlowColor");
		m_glowOffset = MaterialEditor.GetMaterialProperty(targets, "_GlowOffset");
		m_glowInner = MaterialEditor.GetMaterialProperty(targets, "_GlowInner");
		m_glowOuter = MaterialEditor.GetMaterialProperty(targets, "_GlowOuter");
		m_glowPower = MaterialEditor.GetMaterialProperty(targets, "_GlowPower");
		m_reflectFaceColor = MaterialEditor.GetMaterialProperty(targets, "_ReflectFaceColor");
		m_reflectOutlineColor = MaterialEditor.GetMaterialProperty(targets, "_ReflectOutlineColor");
		m_reflectTex = MaterialEditor.GetMaterialProperty(targets, "_Cube");
		m_reflectRotation = MaterialEditor.GetMaterialProperty(targets, "_EnvMatrixRotation");
		m_mainTex = MaterialEditor.GetMaterialProperty(targets, "_MainTex");
		m_texSampleWidth = MaterialEditor.GetMaterialProperty(targets, "_TextureWidth");
		m_texSampleHeight = MaterialEditor.GetMaterialProperty(targets, "_TextureHeight");
		m_gradientScale = MaterialEditor.GetMaterialProperty(targets, "_GradientScale");
		m_PerspectiveFilter = MaterialEditor.GetMaterialProperty(targets, "_PerspectiveFilter");
		m_scaleX = MaterialEditor.GetMaterialProperty(targets, "_ScaleX");
		m_scaleY = MaterialEditor.GetMaterialProperty(targets, "_ScaleY");
		m_vertexOffsetX = MaterialEditor.GetMaterialProperty(targets, "_VertexOffsetX");
		m_vertexOffsetY = MaterialEditor.GetMaterialProperty(targets, "_VertexOffsetY");
		m_maskTex = MaterialEditor.GetMaterialProperty(targets, "_MaskTex");
		m_maskCoord = MaterialEditor.GetMaterialProperty(targets, "_MaskCoord");
		m_clipRect = MaterialEditor.GetMaterialProperty(targets, "_ClipRect");
		m_maskSoftnessX = MaterialEditor.GetMaterialProperty(targets, "_MaskSoftnessX");
		m_maskSoftnessY = MaterialEditor.GetMaterialProperty(targets, "_MaskSoftnessY");
		m_maskTexInverse = MaterialEditor.GetMaterialProperty(targets, "_MaskInverse");
		m_maskTexEdgeColor = MaterialEditor.GetMaterialProperty(targets, "_MaskEdgeColor");
		m_maskTexEdgeSoftness = MaterialEditor.GetMaterialProperty(targets, "_MaskEdgeSoftness");
		m_maskTexWipeControl = MaterialEditor.GetMaterialProperty(targets, "_MaskWipeControl");
		m_stencilID = MaterialEditor.GetMaterialProperty(targets, "_Stencil");
		m_stencilComp = MaterialEditor.GetMaterialProperty(targets, "_StencilComp");
		m_stencilOp = MaterialEditor.GetMaterialProperty(targets, "_StencilOp");
		m_stencilReadMask = MaterialEditor.GetMaterialProperty(targets, "_StencilReadMask");
		m_stencilWriteMask = MaterialEditor.GetMaterialProperty(targets, "_StencilWriteMask");
		m_shaderFlags = MaterialEditor.GetMaterialProperty(targets, "_ShaderFlags");
		m_scaleRatio_A = MaterialEditor.GetMaterialProperty(targets, "_ScaleRatioA");
		m_scaleRatio_B = MaterialEditor.GetMaterialProperty(targets, "_ScaleRatioB");
		m_scaleRatio_C = MaterialEditor.GetMaterialProperty(targets, "_ScaleRatioC");
	}

	public TMPro_SDFMaterialEditor()
		: this()
	{
	}
}
