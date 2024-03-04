using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(ColorGrading))]
	public sealed class ColorGradingEditor : PostProcessEffectEditor<ColorGrading>
	{
		private SerializedParameterOverride m_GradingMode;

		private static GUIContent[] s_Curves = (GUIContent[])(object)new GUIContent[8]
		{
			new GUIContent("Master"),
			new GUIContent("Red"),
			new GUIContent("Green"),
			new GUIContent("Blue"),
			new GUIContent("Hue Vs Hue"),
			new GUIContent("Hue Vs Sat"),
			new GUIContent("Sat Vs Sat"),
			new GUIContent("Lum Vs Sat")
		};

		private SerializedParameterOverride m_ExternalLut;

		private SerializedParameterOverride m_Tonemapper;

		private SerializedParameterOverride m_ToneCurveToeStrength;

		private SerializedParameterOverride m_ToneCurveToeLength;

		private SerializedParameterOverride m_ToneCurveShoulderStrength;

		private SerializedParameterOverride m_ToneCurveShoulderLength;

		private SerializedParameterOverride m_ToneCurveShoulderAngle;

		private SerializedParameterOverride m_ToneCurveGamma;

		private SerializedParameterOverride m_LdrLut;

		private SerializedParameterOverride m_LdrLutContribution;

		private SerializedParameterOverride m_Temperature;

		private SerializedParameterOverride m_Tint;

		private SerializedParameterOverride m_ColorFilter;

		private SerializedParameterOverride m_HueShift;

		private SerializedParameterOverride m_Saturation;

		private SerializedParameterOverride m_Brightness;

		private SerializedParameterOverride m_PostExposure;

		private SerializedParameterOverride m_Contrast;

		private SerializedParameterOverride m_MixerRedOutRedIn;

		private SerializedParameterOverride m_MixerRedOutGreenIn;

		private SerializedParameterOverride m_MixerRedOutBlueIn;

		private SerializedParameterOverride m_MixerGreenOutRedIn;

		private SerializedParameterOverride m_MixerGreenOutGreenIn;

		private SerializedParameterOverride m_MixerGreenOutBlueIn;

		private SerializedParameterOverride m_MixerBlueOutRedIn;

		private SerializedParameterOverride m_MixerBlueOutGreenIn;

		private SerializedParameterOverride m_MixerBlueOutBlueIn;

		private SerializedParameterOverride m_Lift;

		private SerializedParameterOverride m_Gamma;

		private SerializedParameterOverride m_Gain;

		private SerializedParameterOverride m_MasterCurve;

		private SerializedParameterOverride m_RedCurve;

		private SerializedParameterOverride m_GreenCurve;

		private SerializedParameterOverride m_BlueCurve;

		private SerializedParameterOverride m_HueVsHueCurve;

		private SerializedParameterOverride m_HueVsSatCurve;

		private SerializedParameterOverride m_SatVsSatCurve;

		private SerializedParameterOverride m_LumVsSatCurve;

		private SerializedProperty m_RawMasterCurve;

		private SerializedProperty m_RawRedCurve;

		private SerializedProperty m_RawGreenCurve;

		private SerializedProperty m_RawBlueCurve;

		private SerializedProperty m_RawHueVsHueCurve;

		private SerializedProperty m_RawHueVsSatCurve;

		private SerializedProperty m_RawSatVsSatCurve;

		private SerializedProperty m_RawLumVsSatCurve;

		private CurveEditor m_CurveEditor;

		private Dictionary<SerializedProperty, Color> m_CurveDict;

		private const int k_CustomToneCurveResolution = 48;

		private const float k_CustomToneCurveRangeY = 1.025f;

		private readonly Vector3[] m_RectVertices = (Vector3[])(object)new Vector3[4];

		private readonly Vector3[] m_LineVertices = (Vector3[])(object)new Vector3[2];

		private readonly Vector3[] m_CurveVertices = (Vector3[])(object)new Vector3[48];

		private Rect m_CustomToneCurveRect;

		private readonly HableCurve m_HableCurve = new HableCurve();

		private static Material s_MaterialGrid;

		public override void OnEnable()
		{
			//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d26: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d48: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dae: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df2: Unknown result type (might be due to invalid IL or missing references)
			m_GradingMode = FindParameterOverride((ColorGrading x) => x.gradingMode);
			m_ExternalLut = FindParameterOverride((ColorGrading x) => x.externalLut);
			m_Tonemapper = FindParameterOverride((ColorGrading x) => x.tonemapper);
			m_ToneCurveToeStrength = FindParameterOverride((ColorGrading x) => x.toneCurveToeStrength);
			m_ToneCurveToeLength = FindParameterOverride((ColorGrading x) => x.toneCurveToeLength);
			m_ToneCurveShoulderStrength = FindParameterOverride((ColorGrading x) => x.toneCurveShoulderStrength);
			m_ToneCurveShoulderLength = FindParameterOverride((ColorGrading x) => x.toneCurveShoulderLength);
			m_ToneCurveShoulderAngle = FindParameterOverride((ColorGrading x) => x.toneCurveShoulderAngle);
			m_ToneCurveGamma = FindParameterOverride((ColorGrading x) => x.toneCurveGamma);
			m_LdrLut = FindParameterOverride((ColorGrading x) => x.ldrLut);
			m_LdrLutContribution = FindParameterOverride((ColorGrading x) => x.ldrLutContribution);
			m_Temperature = FindParameterOverride((ColorGrading x) => x.temperature);
			m_Tint = FindParameterOverride((ColorGrading x) => x.tint);
			m_ColorFilter = FindParameterOverride((ColorGrading x) => x.colorFilter);
			m_HueShift = FindParameterOverride((ColorGrading x) => x.hueShift);
			m_Saturation = FindParameterOverride((ColorGrading x) => x.saturation);
			m_Brightness = FindParameterOverride((ColorGrading x) => x.brightness);
			m_PostExposure = FindParameterOverride((ColorGrading x) => x.postExposure);
			m_Contrast = FindParameterOverride((ColorGrading x) => x.contrast);
			m_MixerRedOutRedIn = FindParameterOverride((ColorGrading x) => x.mixerRedOutRedIn);
			m_MixerRedOutGreenIn = FindParameterOverride((ColorGrading x) => x.mixerRedOutGreenIn);
			m_MixerRedOutBlueIn = FindParameterOverride((ColorGrading x) => x.mixerRedOutBlueIn);
			m_MixerGreenOutRedIn = FindParameterOverride((ColorGrading x) => x.mixerGreenOutRedIn);
			m_MixerGreenOutGreenIn = FindParameterOverride((ColorGrading x) => x.mixerGreenOutGreenIn);
			m_MixerGreenOutBlueIn = FindParameterOverride((ColorGrading x) => x.mixerGreenOutBlueIn);
			m_MixerBlueOutRedIn = FindParameterOverride((ColorGrading x) => x.mixerBlueOutRedIn);
			m_MixerBlueOutGreenIn = FindParameterOverride((ColorGrading x) => x.mixerBlueOutGreenIn);
			m_MixerBlueOutBlueIn = FindParameterOverride((ColorGrading x) => x.mixerBlueOutBlueIn);
			m_Lift = FindParameterOverride((ColorGrading x) => x.lift);
			m_Gamma = FindParameterOverride((ColorGrading x) => x.gamma);
			m_Gain = FindParameterOverride((ColorGrading x) => x.gain);
			m_MasterCurve = FindParameterOverride((ColorGrading x) => x.masterCurve);
			m_RedCurve = FindParameterOverride((ColorGrading x) => x.redCurve);
			m_GreenCurve = FindParameterOverride((ColorGrading x) => x.greenCurve);
			m_BlueCurve = FindParameterOverride((ColorGrading x) => x.blueCurve);
			m_HueVsHueCurve = FindParameterOverride((ColorGrading x) => x.hueVsHueCurve);
			m_HueVsSatCurve = FindParameterOverride((ColorGrading x) => x.hueVsSatCurve);
			m_SatVsSatCurve = FindParameterOverride((ColorGrading x) => x.satVsSatCurve);
			m_LumVsSatCurve = FindParameterOverride((ColorGrading x) => x.lumVsSatCurve);
			m_RawMasterCurve = FindProperty((ColorGrading x) => x.masterCurve.value.curve);
			m_RawRedCurve = FindProperty((ColorGrading x) => x.redCurve.value.curve);
			m_RawGreenCurve = FindProperty((ColorGrading x) => x.greenCurve.value.curve);
			m_RawBlueCurve = FindProperty((ColorGrading x) => x.blueCurve.value.curve);
			m_RawHueVsHueCurve = FindProperty((ColorGrading x) => x.hueVsHueCurve.value.curve);
			m_RawHueVsSatCurve = FindProperty((ColorGrading x) => x.hueVsSatCurve.value.curve);
			m_RawSatVsSatCurve = FindProperty((ColorGrading x) => x.satVsSatCurve.value.curve);
			m_RawLumVsSatCurve = FindProperty((ColorGrading x) => x.lumVsSatCurve.value.curve);
			m_CurveEditor = new CurveEditor();
			m_CurveDict = new Dictionary<SerializedProperty, Color>();
			SetupCurve(m_RawMasterCurve, new Color(1f, 1f, 1f), 2u, loop: false);
			SetupCurve(m_RawRedCurve, new Color(1f, 0f, 0f), 2u, loop: false);
			SetupCurve(m_RawGreenCurve, new Color(0f, 1f, 0f), 2u, loop: false);
			SetupCurve(m_RawBlueCurve, new Color(0f, 0.5f, 1f), 2u, loop: false);
			SetupCurve(m_RawHueVsHueCurve, new Color(1f, 1f, 1f), 0u, loop: true);
			SetupCurve(m_RawHueVsSatCurve, new Color(1f, 1f, 1f), 0u, loop: true);
			SetupCurve(m_RawSatVsSatCurve, new Color(1f, 1f, 1f), 0u, loop: false);
			SetupCurve(m_RawLumVsSatCurve, new Color(1f, 1f, 1f), 0u, loop: false);
		}

		public override void OnInspectorGUI()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			PropertyField(m_GradingMode);
			GradingMode intValue = (GradingMode)m_GradingMode.value.get_intValue();
			if (intValue != 0 && (int)QualitySettings.get_activeColorSpace() == 0)
			{
				EditorGUILayout.HelpBox("ColorSpace in project settings is set to Gamma, HDR color grading won't look correct. Switch to Linear or use LDR color grading mode instead.", (MessageType)2);
			}
			if (m_GradingMode.overrideState.get_boolValue() && intValue == GradingMode.External && (!SystemInfo.get_supports3DRenderTextures() || !SystemInfo.get_supportsComputeShaders()))
			{
				EditorGUILayout.HelpBox("HDR color grading requires compute shader & 3D render texture support.", (MessageType)2);
			}
			switch (intValue)
			{
			case GradingMode.LowDefinitionRange:
				DoStandardModeGUI(hdr: false);
				break;
			case GradingMode.HighDefinitionRange:
				DoStandardModeGUI(hdr: true);
				break;
			case GradingMode.External:
				DoExternalModeGUI();
				break;
			}
			EditorGUILayout.Space();
		}

		private void SetupCurve(SerializedProperty prop, Color color, uint minPointCount, bool loop)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			CurveEditor.CurveState defaultState = CurveEditor.CurveState.defaultState;
			defaultState.color = color;
			defaultState.visible = false;
			defaultState.minPointCount = minPointCount;
			defaultState.onlyShowHandlesOnSelection = true;
			defaultState.zeroKeyConstantValue = 0.5f;
			defaultState.loopInBounds = loop;
			m_CurveEditor.Add(prop, defaultState);
			m_CurveDict.Add(prop, color);
		}

		private void DoExternalModeGUI()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Expected O, but got Unknown
			PropertyField(m_ExternalLut);
			Object objectReferenceValue = m_ExternalLut.value.get_objectReferenceValue();
			if (!(objectReferenceValue != (Object)null))
			{
				return;
			}
			if (((object)objectReferenceValue).GetType() == typeof(Texture3D))
			{
				Texture3D val = (Texture3D)objectReferenceValue;
				if (((Texture)val).get_width() == ((Texture)val).get_height() && ((Texture)val).get_height() == val.get_depth())
				{
					return;
				}
			}
			else if (((object)objectReferenceValue).GetType() == typeof(RenderTexture))
			{
				RenderTexture val2 = (RenderTexture)objectReferenceValue;
				if (((Texture)val2).get_width() == ((Texture)val2).get_height() && ((Texture)val2).get_height() == val2.get_volumeDepth())
				{
					return;
				}
			}
			EditorGUILayout.HelpBox("Custom LUTs have to be log-encoded 3D textures or 3D render textures with cube format.", (MessageType)2);
		}

		private void DoStandardModeGUI(bool hdr)
		{
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Expected O, but got Unknown
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Expected O, but got Unknown
			if (!hdr)
			{
				PropertyField(m_LdrLut);
				PropertyField(m_LdrLutContribution);
				Texture value = (base.target as ColorGrading).ldrLut.value;
				CheckLutImportSettings(value);
			}
			if (hdr)
			{
				EditorGUILayout.Space();
				EditorUtilities.DrawHeaderLabel("Tonemapping");
				PropertyField(m_Tonemapper);
				if (m_Tonemapper.value.get_intValue() == 3)
				{
					DrawCustomToneCurve();
					PropertyField(m_ToneCurveToeStrength);
					PropertyField(m_ToneCurveToeLength);
					PropertyField(m_ToneCurveShoulderStrength);
					PropertyField(m_ToneCurveShoulderLength);
					PropertyField(m_ToneCurveShoulderAngle);
					PropertyField(m_ToneCurveGamma);
				}
			}
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("White Balance");
			PropertyField(m_Temperature);
			PropertyField(m_Tint);
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("Tone");
			if (hdr)
			{
				PropertyField(m_PostExposure);
			}
			PropertyField(m_ColorFilter);
			PropertyField(m_HueShift);
			PropertyField(m_Saturation);
			if (!hdr)
			{
				PropertyField(m_Brightness);
			}
			PropertyField(m_Contrast);
			EditorGUILayout.Space();
			int num = GlobalSettings.currentChannelMixer;
			HorizontalScope val = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				EditorGUILayout.PrefixLabel("Channel Mixer", GUIStyle.get_none(), Styling.headerLabel);
				EditorGUI.BeginChangeCheck();
				HorizontalScope val2 = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
				try
				{
					GUILayoutUtility.GetRect(9f, 18f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(false) });
					if (GUILayout.Toggle(num == 0, EditorUtilities.GetContent("Red|Red output channel."), EditorStyles.get_miniButtonLeft(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
					{
						num = 0;
					}
					if (GUILayout.Toggle(num == 1, EditorUtilities.GetContent("Green|Green output channel."), EditorStyles.get_miniButtonMid(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
					{
						num = 1;
					}
					if (GUILayout.Toggle(num == 2, EditorUtilities.GetContent("Blue|Blue output channel."), EditorStyles.get_miniButtonRight(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
					{
						num = 2;
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				if (EditorGUI.EndChangeCheck())
				{
					GUI.FocusControl((string)null);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			GlobalSettings.currentChannelMixer = num;
			switch (num)
			{
			case 0:
				PropertyField(m_MixerRedOutRedIn);
				PropertyField(m_MixerRedOutGreenIn);
				PropertyField(m_MixerRedOutBlueIn);
				break;
			case 1:
				PropertyField(m_MixerGreenOutRedIn);
				PropertyField(m_MixerGreenOutGreenIn);
				PropertyField(m_MixerGreenOutBlueIn);
				break;
			default:
				PropertyField(m_MixerBlueOutRedIn);
				PropertyField(m_MixerBlueOutGreenIn);
				PropertyField(m_MixerBlueOutBlueIn);
				break;
			}
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("Trackballs");
			val = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				PropertyField(m_Lift);
				GUILayout.Space(4f);
				PropertyField(m_Gamma);
				GUILayout.Space(4f);
				PropertyField(m_Gain);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("Grading Curves");
			DoCurvesGUI(hdr);
		}

		private void CheckLutImportSettings(Texture lut)
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Invalid comparison between Unknown and I4
			if (!((Object)(object)lut != (Object)null))
			{
				return;
			}
			TextureImporter importer = default(TextureImporter);
			ref TextureImporter val = ref importer;
			AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object)(object)lut));
			val = (TextureImporter)(object)((atPath is TextureImporter) ? atPath : null);
			if ((Object)(object)importer != (Object)null && (importer.get_anisoLevel() != 0 || importer.get_mipmapEnabled() || importer.get_sRGBTexture() || (int)importer.get_textureCompression() != 0 || (int)importer.get_wrapMode() != 1))
			{
				EditorUtilities.DrawFixMeBox("Invalid LUT import settings.", delegate
				{
					SetLutImportSettings(importer);
				});
			}
		}

		private void SetLutImportSettings(TextureImporter importer)
		{
			importer.set_textureType((TextureImporterType)0);
			importer.set_mipmapEnabled(false);
			importer.set_anisoLevel(0);
			importer.set_sRGBTexture(false);
			importer.set_npotScale((TextureImporterNPOTScale)0);
			importer.set_textureCompression((TextureImporterCompression)0);
			importer.set_alphaSource((TextureImporterAlphaSource)0);
			importer.set_wrapMode((TextureWrapMode)1);
			((AssetImporter)importer).SaveAndReimport();
			AssetDatabase.Refresh();
		}

		private void DrawCustomToneCurve()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Invalid comparison between Unknown and I4
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_026c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			HorizontalScope val = new HorizontalScope((GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				GUILayout.Space((float)EditorGUI.get_indentLevel() * 15f);
				m_CustomToneCurveRect = GUILayoutUtility.GetRect(128f, 80f);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if ((int)Event.get_current().get_type() != 7)
			{
				return;
			}
			float floatValue = m_ToneCurveToeStrength.value.get_floatValue();
			float floatValue2 = m_ToneCurveToeLength.value.get_floatValue();
			float floatValue3 = m_ToneCurveShoulderStrength.value.get_floatValue();
			float floatValue4 = m_ToneCurveShoulderLength.value.get_floatValue();
			float floatValue5 = m_ToneCurveShoulderAngle.value.get_floatValue();
			float floatValue6 = m_ToneCurveGamma.value.get_floatValue();
			m_HableCurve.Init(floatValue, floatValue2, floatValue3, floatValue4, floatValue5, floatValue6);
			float whitePoint = m_HableCurve.whitePoint;
			m_RectVertices[0] = PointInRect(0f, 0f, whitePoint);
			m_RectVertices[1] = PointInRect(whitePoint, 0f, whitePoint);
			m_RectVertices[2] = PointInRect(whitePoint, 1.025f, whitePoint);
			m_RectVertices[3] = PointInRect(0f, 1.025f, whitePoint);
			Handles.DrawSolidRectangleWithOutline(m_RectVertices, Color.get_white() * 0.1f, Color.get_white() * 0.4f);
			if (whitePoint < ((Rect)(ref m_CustomToneCurveRect)).get_width() / 3f)
			{
				int num = Mathf.CeilToInt(whitePoint);
				for (int i = 1; i < num; i++)
				{
					DrawLine(i, 0f, i, 1.025f, 0.4f, whitePoint);
				}
			}
			Handles.Label(Vector2.op_Implicit(((Rect)(ref m_CustomToneCurveRect)).get_position() + Vector2.get_right()), "Custom Tone Curve", EditorStyles.get_miniLabel());
			int num2 = 0;
			while (num2 < 48)
			{
				float x = whitePoint * (float)num2 / 47f;
				float num3 = m_HableCurve.Eval(x);
				if (num3 < 1.025f)
				{
					m_CurveVertices[num2++] = PointInRect(x, num3, whitePoint);
					continue;
				}
				if (num2 > 1)
				{
					Vector3 val2 = m_CurveVertices[num2 - 2];
					Vector3 val3 = m_CurveVertices[num2 - 1];
					float num4 = (((Rect)(ref m_CustomToneCurveRect)).get_y() - val2.y) / (val3.y - val2.y);
					m_CurveVertices[num2 - 1] = val2 + (val3 - val2) * num4;
				}
				break;
			}
			if (num2 > 1)
			{
				Handles.set_color(Color.get_white() * 0.9f);
				Handles.DrawAAPolyLine(2f, num2, m_CurveVertices);
			}
		}

		private void DrawLine(float x1, float y1, float x2, float y2, float grayscale, float rangeX)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			m_LineVertices[0] = PointInRect(x1, y1, rangeX);
			m_LineVertices[1] = PointInRect(x2, y2, rangeX);
			Handles.set_color(Color.get_white() * grayscale);
			Handles.DrawAAPolyLine(2f, m_LineVertices);
		}

		private Vector3 PointInRect(float x, float y, float rangeX)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			x = Mathf.Lerp(((Rect)(ref m_CustomToneCurveRect)).get_x(), ((Rect)(ref m_CustomToneCurveRect)).get_xMax(), x / rangeX);
			y = Mathf.Lerp(((Rect)(ref m_CustomToneCurveRect)).get_yMax(), ((Rect)(ref m_CustomToneCurveRect)).get_y(), y / 1.025f);
			return new Vector3(x, y, 0f);
		}

		private void ResetVisibleCurves()
		{
			foreach (KeyValuePair<SerializedProperty, Color> item in m_CurveDict)
			{
				CurveEditor.CurveState curveState = m_CurveEditor.GetCurveState(item.Key);
				curveState.visible = false;
				m_CurveEditor.SetCurveState(item.Key, curveState);
			}
		}

		private void SetCurveVisible(SerializedProperty rawProp, SerializedProperty overrideProp)
		{
			CurveEditor.CurveState curveState = m_CurveEditor.GetCurveState(rawProp);
			curveState.visible = true;
			curveState.editable = overrideProp.get_boolValue();
			m_CurveEditor.SetCurveState(rawProp, curveState);
		}

		private void CurveOverrideToggle(SerializedProperty overrideProp)
		{
			overrideProp.set_boolValue(GUILayout.Toggle(overrideProp.get_boolValue(), EditorUtilities.GetContent("Override"), EditorStyles.get_toolbarButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]));
		}

		private void DoCurvesGUI(bool hdr)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Expected O, but got Unknown
			//IL_0301: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Expected O, but got Unknown
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Expected O, but got Unknown
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032f: Expected O, but got Unknown
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Expected O, but got Unknown
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_036a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0370: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0383: Invalid comparison between Unknown and I4
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			//IL_039d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0426: Unknown result type (might be due to invalid IL or missing references)
			//IL_047e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0483: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0490: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0512: Unknown result type (might be due to invalid IL or missing references)
			//IL_0517: Unknown result type (might be due to invalid IL or missing references)
			//IL_051f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0524: Unknown result type (might be due to invalid IL or missing references)
			//IL_0536: Unknown result type (might be due to invalid IL or missing references)
			//IL_053b: Unknown result type (might be due to invalid IL or missing references)
			//IL_053d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0542: Unknown result type (might be due to invalid IL or missing references)
			//IL_055b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0562: Unknown result type (might be due to invalid IL or missing references)
			//IL_0567: Unknown result type (might be due to invalid IL or missing references)
			//IL_0583: Unknown result type (might be due to invalid IL or missing references)
			//IL_059c: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Invalid comparison between Unknown and I4
			//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0606: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_061e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0623: Unknown result type (might be due to invalid IL or missing references)
			//IL_063b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0640: Unknown result type (might be due to invalid IL or missing references)
			//IL_0653: Unknown result type (might be due to invalid IL or missing references)
			//IL_0658: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Unknown result type (might be due to invalid IL or missing references)
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_068e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0693: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0714: Unknown result type (might be due to invalid IL or missing references)
			//IL_0719: Unknown result type (might be due to invalid IL or missing references)
			//IL_071b: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			ResetVisibleCurves();
			DisabledGroupScope val = new DisabledGroupScope(base.serializedObject.get_isEditingMultipleObjects());
			try
			{
				int num = 0;
				SerializedProperty curve = null;
				HorizontalScope val2 = new HorizontalScope(EditorStyles.get_toolbar(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				try
				{
					num = DoCurveSelectionPopup(GlobalSettings.currentCurve, hdr);
					num = Mathf.Clamp(num, hdr ? 4 : 0, 7);
					EditorGUILayout.Space();
					switch (num)
					{
					case 0:
						CurveOverrideToggle(m_MasterCurve.overrideState);
						SetCurveVisible(m_RawMasterCurve, m_MasterCurve.overrideState);
						curve = m_RawMasterCurve;
						break;
					case 1:
						CurveOverrideToggle(m_RedCurve.overrideState);
						SetCurveVisible(m_RawRedCurve, m_RedCurve.overrideState);
						curve = m_RawRedCurve;
						break;
					case 2:
						CurveOverrideToggle(m_GreenCurve.overrideState);
						SetCurveVisible(m_RawGreenCurve, m_GreenCurve.overrideState);
						curve = m_RawGreenCurve;
						break;
					case 3:
						CurveOverrideToggle(m_BlueCurve.overrideState);
						SetCurveVisible(m_RawBlueCurve, m_BlueCurve.overrideState);
						curve = m_RawBlueCurve;
						break;
					case 4:
						CurveOverrideToggle(m_HueVsHueCurve.overrideState);
						SetCurveVisible(m_RawHueVsHueCurve, m_HueVsHueCurve.overrideState);
						curve = m_RawHueVsHueCurve;
						break;
					case 5:
						CurveOverrideToggle(m_HueVsSatCurve.overrideState);
						SetCurveVisible(m_RawHueVsSatCurve, m_HueVsSatCurve.overrideState);
						curve = m_RawHueVsSatCurve;
						break;
					case 6:
						CurveOverrideToggle(m_SatVsSatCurve.overrideState);
						SetCurveVisible(m_RawSatVsSatCurve, m_SatVsSatCurve.overrideState);
						curve = m_RawSatVsSatCurve;
						break;
					case 7:
						CurveOverrideToggle(m_LumVsSatCurve.overrideState);
						SetCurveVisible(m_RawLumVsSatCurve, m_LumVsSatCurve.overrideState);
						curve = m_RawLumVsSatCurve;
						break;
					}
					GUILayout.FlexibleSpace();
					if (GUILayout.Button("Reset", EditorStyles.get_toolbarButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
					{
						switch (num)
						{
						case 0:
							m_RawMasterCurve.set_animationCurveValue(AnimationCurve.Linear(0f, 0f, 1f, 1f));
							break;
						case 1:
							m_RawRedCurve.set_animationCurveValue(AnimationCurve.Linear(0f, 0f, 1f, 1f));
							break;
						case 2:
							m_RawGreenCurve.set_animationCurveValue(AnimationCurve.Linear(0f, 0f, 1f, 1f));
							break;
						case 3:
							m_RawBlueCurve.set_animationCurveValue(AnimationCurve.Linear(0f, 0f, 1f, 1f));
							break;
						case 4:
							m_RawHueVsHueCurve.set_animationCurveValue(new AnimationCurve());
							break;
						case 5:
							m_RawHueVsSatCurve.set_animationCurveValue(new AnimationCurve());
							break;
						case 6:
							m_RawSatVsSatCurve.set_animationCurveValue(new AnimationCurve());
							break;
						case 7:
							m_RawLumVsSatCurve.set_animationCurveValue(new AnimationCurve());
							break;
						}
					}
					GlobalSettings.currentCurve = num;
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
				CurveEditor.Settings settings = m_CurveEditor.settings;
				Rect aspectRect = GUILayoutUtility.GetAspectRect(2f);
				Rect val3 = settings.padding.Remove(aspectRect);
				if ((int)Event.get_current().get_type() == 7)
				{
					EditorGUI.DrawRect(aspectRect, new Color(0.15f, 0.15f, 0.15f, 1f));
					switch (num)
					{
					case 4:
					case 5:
						DrawBackgroundTexture(val3, 0);
						break;
					case 6:
					case 7:
						DrawBackgroundTexture(val3, 1);
						break;
					}
					Handles.set_color(Color.get_white() * (GUI.get_enabled() ? 1f : 0.5f));
					Handles.DrawSolidRectangleWithOutline(val3, Color.get_clear(), new Color(0.8f, 0.8f, 0.8f, 0.5f));
					Handles.set_color(new Color(1f, 1f, 1f, 0.05f));
					int num2 = (int)Mathf.Sqrt(((Rect)(ref val3)).get_width());
					int num3 = (int)((float)num2 / (((Rect)(ref val3)).get_width() / ((Rect)(ref val3)).get_height()));
					int num4 = Mathf.FloorToInt(((Rect)(ref val3)).get_width() / (float)num2);
					int num5 = (int)((Rect)(ref val3)).get_width() % num2 / 2;
					for (int i = 1; i < num2; i++)
					{
						Vector2 val4 = (float)i * Vector2.get_right() * (float)num4;
						val4.x += num5;
						Handles.DrawLine(Vector2.op_Implicit(((Rect)(ref val3)).get_position() + val4), Vector2.op_Implicit(new Vector2(((Rect)(ref val3)).get_x(), ((Rect)(ref val3)).get_yMax() - 1f) + val4));
					}
					num4 = Mathf.FloorToInt(((Rect)(ref val3)).get_height() / (float)num3);
					num5 = (int)((Rect)(ref val3)).get_height() % num3 / 2;
					for (int j = 1; j < num3; j++)
					{
						Vector2 val5 = (float)j * Vector2.get_up() * (float)num4;
						val5.y += num5;
						Handles.DrawLine(Vector2.op_Implicit(((Rect)(ref val3)).get_position() + val5), Vector2.op_Implicit(new Vector2(((Rect)(ref val3)).get_xMax() - 1f, ((Rect)(ref val3)).get_y()) + val5));
					}
				}
				if (m_CurveEditor.OnGUI(aspectRect))
				{
					Repaint();
					GUI.set_changed(true);
				}
				if ((int)Event.get_current().get_type() == 7)
				{
					Handles.set_color(Color.get_black());
					Handles.DrawLine(Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_x(), ((Rect)(ref aspectRect)).get_y() - 18f)), Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_xMax(), ((Rect)(ref aspectRect)).get_y() - 18f)));
					Handles.DrawLine(Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_x(), ((Rect)(ref aspectRect)).get_y() - 19f)), Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_x(), ((Rect)(ref aspectRect)).get_yMax())));
					Handles.DrawLine(Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_x(), ((Rect)(ref aspectRect)).get_yMax())), Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_xMax(), ((Rect)(ref aspectRect)).get_yMax())));
					Handles.DrawLine(Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_xMax(), ((Rect)(ref aspectRect)).get_yMax())), Vector2.op_Implicit(new Vector2(((Rect)(ref aspectRect)).get_xMax(), ((Rect)(ref aspectRect)).get_y() - 18f)));
					string text = (m_CurveEditor.GetCurveState(curve).editable ? string.Empty : "(Not Overriding)\n");
					CurveEditor.Selection selection = m_CurveEditor.GetSelection();
					Rect val6 = val3;
					((Rect)(ref val6)).set_x(((Rect)(ref val6)).get_x() + 5f);
					((Rect)(ref val6)).set_width(100f);
					((Rect)(ref val6)).set_height(30f);
					if (selection.curve != null && selection.keyframeIndex > -1)
					{
						Keyframe value = selection.keyframe.Value;
						GUI.Label(val6, string.Format("{0}\n{1}", ((Keyframe)(ref value)).get_time().ToString("F3"), ((Keyframe)(ref value)).get_value().ToString("F3")), Styling.preLabel);
					}
					else
					{
						GUI.Label(val6, text, Styling.preLabel);
					}
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		private void DrawBackgroundTexture(Rect rect, int pass)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)s_MaterialGrid == (Object)null)
			{
				Material val = new Material(Shader.Find("Hidden/PostProcessing/Editor/CurveGrid"));
				((Object)val).set_hideFlags((HideFlags)61);
				s_MaterialGrid = val;
			}
			float pixelsPerPoint = EditorGUIUtility.get_pixelsPerPoint();
			RenderTexture active = RenderTexture.get_active();
			RenderTexture temporary = RenderTexture.GetTemporary(Mathf.CeilToInt(((Rect)(ref rect)).get_width() * pixelsPerPoint), Mathf.CeilToInt(((Rect)(ref rect)).get_height() * pixelsPerPoint), 0, (RenderTextureFormat)0, (RenderTextureReadWrite)1);
			s_MaterialGrid.SetFloat("_DisabledState", GUI.get_enabled() ? 1f : 0.5f);
			s_MaterialGrid.SetFloat("_PixelScaling", EditorGUIUtility.get_pixelsPerPoint());
			Graphics.Blit((Texture)null, temporary, s_MaterialGrid, pass);
			RenderTexture.set_active(active);
			GUI.DrawTexture(rect, (Texture)(object)temporary);
			RenderTexture.ReleaseTemporary(temporary);
		}

		private int DoCurveSelectionPopup(int id, bool hdr)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			GUILayout.Label(s_Curves[id], EditorStyles.get_toolbarPopup(), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(150f) });
			Rect lastRect = GUILayoutUtility.GetLastRect();
			Event current2 = Event.get_current();
			if ((int)current2.get_type() == 0 && current2.get_button() == 0 && ((Rect)(ref lastRect)).Contains(current2.get_mousePosition()))
			{
				GenericMenu val = new GenericMenu();
				for (int i = 0; i < s_Curves.Length; i++)
				{
					if (i == 4)
					{
						val.AddSeparator("");
					}
					if (hdr && i < 4)
					{
						val.AddDisabledItem(s_Curves[i]);
						continue;
					}
					int current = i;
					val.AddItem(s_Curves[i], current == id, (MenuFunction)delegate
					{
						GlobalSettings.currentCurve = current;
					});
				}
				val.DropDown(new Rect(((Rect)(ref lastRect)).get_xMin(), ((Rect)(ref lastRect)).get_yMax(), 1f, 1f));
			}
			return id;
		}
	}
}
