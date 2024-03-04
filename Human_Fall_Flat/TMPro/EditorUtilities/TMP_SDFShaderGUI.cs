using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public class TMP_SDFShaderGUI : TMP_BaseShaderGUI
	{
		private static MaterialPanel facePanel;

		private static MaterialPanel outlinePanel;

		private static MaterialPanel underlayPanel;

		private static MaterialPanel bevelPanel;

		private static MaterialPanel lightingPanel;

		private static MaterialPanel bumpMapPanel;

		private static MaterialPanel envMapPanel;

		private static MaterialPanel glowPanel;

		private static MaterialPanel debugPanel;

		private static ShaderFeature outlineFeature;

		private static ShaderFeature underlayFeature;

		private static ShaderFeature bevelFeature;

		private static ShaderFeature glowFeature;

		private static ShaderFeature maskFeature;

		private static string[] faceUVSpeedNames;

		private static string[] outlineUVSpeedNames;

		private static GUIContent[] bevelTypeLabels;

		static TMP_SDFShaderGUI()
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Expected O, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Expected O, but got Unknown
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Expected O, but got Unknown
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Expected O, but got Unknown
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Expected O, but got Unknown
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Expected O, but got Unknown
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_024d: Expected O, but got Unknown
			faceUVSpeedNames = new string[2] { "_FaceUVSpeedX", "_FaceUVSpeedY" };
			outlineUVSpeedNames = new string[2] { "_OutlineUVSpeedX", "_OutlineUVSpeedY" };
			bevelTypeLabels = (GUIContent[])(object)new GUIContent[2]
			{
				new GUIContent("Outer Bevel"),
				new GUIContent("Inner Bevel")
			};
			facePanel = new MaterialPanel("Face", expandedByDefault: true);
			outlinePanel = new MaterialPanel("Outline", expandedByDefault: true);
			underlayPanel = new MaterialPanel("Underlay", expandedByDefault: false);
			bevelPanel = new MaterialPanel("Bevel", expandedByDefault: false);
			lightingPanel = new MaterialPanel("Lighting", expandedByDefault: false);
			bumpMapPanel = new MaterialPanel("BumpMap", expandedByDefault: false);
			envMapPanel = new MaterialPanel("EnvMap", expandedByDefault: false);
			glowPanel = new MaterialPanel("Glow", expandedByDefault: false);
			debugPanel = new MaterialPanel("Debug", expandedByDefault: false);
			outlineFeature = new ShaderFeature
			{
				undoLabel = "Outline",
				keywords = new string[1] { "OUTLINE_ON" }
			};
			underlayFeature = new ShaderFeature
			{
				undoLabel = "Underlay",
				keywords = new string[2] { "UNDERLAY_ON", "UNDERLAY_INNER" },
				label = new GUIContent("Underlay Type"),
				keywordLabels = (GUIContent[])(object)new GUIContent[3]
				{
					new GUIContent("None"),
					new GUIContent("Normal"),
					new GUIContent("Inner")
				}
			};
			bevelFeature = new ShaderFeature
			{
				undoLabel = "Bevel",
				keywords = new string[1] { "BEVEL_ON" }
			};
			glowFeature = new ShaderFeature
			{
				undoLabel = "Glow",
				keywords = new string[1] { "GLOW_ON" }
			};
			maskFeature = new ShaderFeature
			{
				undoLabel = "Mask",
				keywords = new string[2] { "MASK_HARD", "MASK_SOFT" },
				label = new GUIContent("Mask"),
				keywordLabels = (GUIContent[])(object)new GUIContent[3]
				{
					new GUIContent("Mask Off"),
					new GUIContent("Mask Hard"),
					new GUIContent("Mask Soft")
				}
			};
		}

		protected override void DoGUI()
		{
			if (DoPanelHeader(facePanel))
			{
				DoFacePanel();
			}
			if (material.HasProperty(ShaderUtilities.ID_OutlineTex) ? DoPanelHeader(outlinePanel) : DoPanelHeader(outlinePanel, outlineFeature))
			{
				DoOutlinePanel();
			}
			if (material.HasProperty(ShaderUtilities.ID_UnderlayColor) && DoPanelHeader(underlayPanel, underlayFeature))
			{
				DoUnderlayPanel();
			}
			if (material.HasProperty("_SpecularColor"))
			{
				if (DoPanelHeader(bevelPanel, bevelFeature))
				{
					DoBevelPanel();
				}
				if (DoPanelHeader(lightingPanel, bevelFeature, readState: false))
				{
					DoLocalLightingPanel();
				}
				if (DoPanelHeader(bumpMapPanel, bevelFeature, readState: false))
				{
					DoBumpMapPanel();
				}
				if (DoPanelHeader(envMapPanel, bevelFeature, readState: false))
				{
					DoEnvMapPanel();
				}
			}
			else if (material.HasProperty("_SpecColor"))
			{
				if (DoPanelHeader(bevelPanel))
				{
					DoBevelPanel();
				}
				if (DoPanelHeader(lightingPanel))
				{
					DoSurfaceLightingPanel();
				}
				if (DoPanelHeader(bumpMapPanel))
				{
					DoBumpMapPanel();
				}
				if (DoPanelHeader(envMapPanel))
				{
					DoEnvMapPanel();
				}
			}
			if (material.HasProperty(ShaderUtilities.ID_GlowColor) && DoPanelHeader(glowPanel, glowFeature))
			{
				DoGlowPanel();
			}
			if (DoPanelHeader(debugPanel))
			{
				DoDebugPanel();
			}
		}

		private void DoFacePanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoColor("_FaceColor", "Color");
			if (material.HasProperty(ShaderUtilities.ID_FaceTex))
			{
				if (material.HasProperty("_FaceUVSpeedX"))
				{
					DoTexture2D("_FaceTex", "Texture", withTilingOffset: true, faceUVSpeedNames);
				}
				else
				{
					DoTexture2D("_FaceTex", "Texture", withTilingOffset: true);
				}
			}
			DoSlider("_OutlineSoftness", "Softness");
			DoSlider("_FaceDilate", "Dilate");
			if (material.HasProperty(ShaderUtilities.ID_Shininess))
			{
				DoSlider("_FaceShininess", "Gloss");
			}
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoOutlinePanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoColor("_OutlineColor", "Color");
			if (material.HasProperty(ShaderUtilities.ID_OutlineTex))
			{
				if (material.HasProperty("_OutlineUVSpeedX"))
				{
					DoTexture2D("_OutlineTex", "Texture", withTilingOffset: true, outlineUVSpeedNames);
				}
				else
				{
					DoTexture2D("_OutlineTex", "Texture", withTilingOffset: true);
				}
			}
			DoSlider("_OutlineWidth", "Thickness");
			if (material.HasProperty("_OutlineShininess"))
			{
				DoSlider("_OutlineShininess", "Gloss");
			}
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoUnderlayPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			underlayFeature.DoPopup(editor, material);
			DoColor("_UnderlayColor", "Color");
			DoSlider("_UnderlayOffsetX", "Offset X");
			DoSlider("_UnderlayOffsetY", "Offset Y");
			DoSlider("_UnderlayDilate", "Dilate");
			DoSlider("_UnderlaySoftness", "Softness");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoBevelPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoPopup("_ShaderFlags", "Type", bevelTypeLabels);
			DoSlider("_Bevel", "Amount");
			DoSlider("_BevelOffset", "Offset");
			DoSlider("_BevelWidth", "Width");
			DoSlider("_BevelRoundness", "Roundness");
			DoSlider("_BevelClamp", "Clamp");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoLocalLightingPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoSlider("_LightAngle", "Light Angle");
			DoColor("_SpecularColor", "Specular Color");
			DoSlider("_SpecularPower", "Specular Power");
			DoSlider("_Reflectivity", "Reflectivity Power");
			DoSlider("_Diffuse", "Diffuse Shadow");
			DoSlider("_Ambient", "Ambient Shadow");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoSurfaceLightingPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoColor("_SpecColor", "Specular Color");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoBumpMapPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoTexture2D("_BumpMap", "Texture");
			DoSlider("_BumpFace", "Face");
			DoSlider("_BumpOutline", "Outline");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoEnvMapPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoColor("_ReflectFaceColor", "Face Color");
			DoColor("_ReflectOutlineColor", "Outline Color");
			DoCubeMap("_Cube", "Texture");
			DoVector3("_EnvMatrixRotation", "EnvMap Rotation");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoGlowPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoColor("_GlowColor", "Color");
			DoSlider("_GlowOffset", "Offset");
			DoSlider("_GlowInner", "Inner");
			DoSlider("_GlowOuter", "Outer");
			DoSlider("_GlowPower", "Power");
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoDebugPanel()
		{
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			DoTexture2D("_MainTex", "Font Atlas");
			DoFloat("_GradientScale", "Gradient Scale");
			DoFloat("_TextureWidth", "Texture Width");
			DoFloat("_TextureHeight", "Texture Height");
			DoEmptyLine();
			DoFloat("_ScaleX", "Scale X");
			DoFloat("_ScaleY", "Scale Y");
			DoSlider("_PerspectiveFilter", "Perspective Filter");
			DoEmptyLine();
			DoFloat("_VertexOffsetX", "Offset X");
			DoFloat("_VertexOffsetY", "Offset Y");
			if (material.HasProperty(ShaderUtilities.ID_MaskCoord))
			{
				DoEmptyLine();
				maskFeature.ReadState(material);
				maskFeature.DoPopup(editor, material);
				if (maskFeature.Active)
				{
					DoMaskSubgroup();
				}
				DoEmptyLine();
				DoVector("_ClipRect", "Clip Rect", TMP_BaseShaderGUI.lbrtVectorLabels);
			}
			else if (material.HasProperty("_MaskTex"))
			{
				DoMaskTexSubgroup();
			}
			else if (material.HasProperty(ShaderUtilities.ID_MaskSoftnessX))
			{
				DoEmptyLine();
				DoFloat("_MaskSoftnessX", "Softness X");
				DoFloat("_MaskSoftnessY", "Softness Y");
				DoVector("_ClipRect", "Clip Rect", TMP_BaseShaderGUI.lbrtVectorLabels);
			}
			if (material.HasProperty(ShaderUtilities.ID_StencilID))
			{
				DoEmptyLine();
				DoFloat("_Stencil", "Stencil ID");
				DoFloat("_StencilComp", "Stencil Comp");
			}
			DoEmptyLine();
			EditorGUI.BeginChangeCheck();
			bool flag = EditorGUILayout.Toggle("Use Ratios?", !material.IsKeywordEnabled("RATIOS_OFF"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				editor.RegisterPropertyChangeUndo("Use Ratios");
				if (flag)
				{
					material.DisableKeyword("RATIOS_OFF");
				}
				else
				{
					material.EnableKeyword("RATIOS_OFF");
				}
			}
			EditorGUI.BeginDisabledGroup(true);
			DoFloat("_ScaleRatioA", "Scale Ratio A");
			DoFloat("_ScaleRatioB", "Scale Ratio B");
			DoFloat("_ScaleRatioC", "Scale Ratio C");
			EditorGUI.EndDisabledGroup();
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
		}

		private void DoMaskSubgroup()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			DoVector("_MaskCoord", "Mask Bounds", TMP_BaseShaderGUI.xywhVectorLabels);
			if ((Object)(object)Selection.get_activeGameObject() != (Object)null)
			{
				Renderer component = Selection.get_activeGameObject().GetComponent<Renderer>();
				if ((Object)(object)component != (Object)null)
				{
					Rect controlRect = EditorGUILayout.GetControlRect((GUILayoutOption[])(object)new GUILayoutOption[0]);
					((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + EditorGUIUtility.get_labelWidth());
					((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() - EditorGUIUtility.get_labelWidth());
					if (GUI.Button(controlRect, "Match Renderer Bounds"))
					{
						MaterialProperty obj = ShaderGUI.FindProperty("_MaskCoord", properties);
						Bounds bounds = component.get_bounds();
						float num = Mathf.Round(((Bounds)(ref bounds)).get_extents().x * 1000f) / 1000f;
						bounds = component.get_bounds();
						obj.set_vectorValue(new Vector4(0f, 0f, num, Mathf.Round(((Bounds)(ref bounds)).get_extents().y * 1000f) / 1000f));
					}
				}
			}
			if (maskFeature.State == 1)
			{
				DoFloat("_MaskSoftnessX", "Softness X");
				DoFloat("_MaskSoftnessY", "Softness Y");
			}
		}

		private void DoMaskTexSubgroup()
		{
			DoEmptyLine();
			DoTexture2D("_MaskTex", "Mask Texture");
			DoToggle("_MaskInverse", "Inverse Mask");
			DoColor("_MaskEdgeColor", "Edge Color");
			DoSlider("_MaskEdgeSoftness", "Edge Softness");
			DoSlider("_MaskWipeControl", "Wipe Position");
			DoFloat("_MaskSoftnessX", "Softness X");
			DoFloat("_MaskSoftnessY", "Softness Y");
			DoVector("_ClipRect", "Clip Rect", TMP_BaseShaderGUI.lbrtVectorLabels);
		}
	}
}
