using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace UnityEditor.Rendering.PostProcessing
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(PostProcessLayer))]
	public sealed class PostProcessLayerEditor : BaseEditor<PostProcessLayer>
	{
		private enum ExportMode
		{
			FullFrame,
			DisablePost,
			BreakBeforeColorGradingLinear,
			BreakBeforeColorGradingLog
		}

		private SerializedProperty m_StopNaNPropagation;

		private SerializedProperty m_VolumeTrigger;

		private SerializedProperty m_VolumeLayer;

		private SerializedProperty m_AntialiasingMode;

		private SerializedProperty m_TaaJitterSpread;

		private SerializedProperty m_TaaSharpness;

		private SerializedProperty m_TaaStationaryBlending;

		private SerializedProperty m_TaaMotionBlending;

		private SerializedProperty m_SmaaQuality;

		private SerializedProperty m_FxaaFastMode;

		private SerializedProperty m_FxaaKeepAlpha;

		private SerializedProperty m_FogEnabled;

		private SerializedProperty m_FogExcludeSkybox;

		private SerializedProperty m_ShowToolkit;

		private SerializedProperty m_ShowCustomSorter;

		private Dictionary<PostProcessEvent, ReorderableList> m_CustomLists;

		private static GUIContent[] s_AntialiasingMethodNames = (GUIContent[])(object)new GUIContent[4]
		{
			new GUIContent("No Anti-aliasing"),
			new GUIContent("Fast Approximate Anti-aliasing (FXAA)"),
			new GUIContent("Subpixel Morphological Anti-aliasing (SMAA)"),
			new GUIContent("Temporal Anti-aliasing (TAA)")
		};

		private void OnEnable()
		{
			m_StopNaNPropagation = FindProperty((PostProcessLayer x) => x.stopNaNPropagation);
			m_VolumeTrigger = FindProperty((PostProcessLayer x) => x.volumeTrigger);
			m_VolumeLayer = FindProperty((PostProcessLayer x) => x.volumeLayer);
			m_AntialiasingMode = FindProperty((PostProcessLayer x) => x.antialiasingMode);
			m_TaaJitterSpread = FindProperty((PostProcessLayer x) => x.temporalAntialiasing.jitterSpread);
			m_TaaSharpness = FindProperty((PostProcessLayer x) => x.temporalAntialiasing.sharpness);
			m_TaaStationaryBlending = FindProperty((PostProcessLayer x) => x.temporalAntialiasing.stationaryBlending);
			m_TaaMotionBlending = FindProperty((PostProcessLayer x) => x.temporalAntialiasing.motionBlending);
			m_SmaaQuality = FindProperty((PostProcessLayer x) => x.subpixelMorphologicalAntialiasing.quality);
			m_FxaaFastMode = FindProperty((PostProcessLayer x) => x.fastApproximateAntialiasing.fastMode);
			m_FxaaKeepAlpha = FindProperty((PostProcessLayer x) => x.fastApproximateAntialiasing.keepAlpha);
			m_FogEnabled = FindProperty((PostProcessLayer x) => x.fog.enabled);
			m_FogExcludeSkybox = FindProperty((PostProcessLayer x) => x.fog.excludeSkybox);
			m_ShowToolkit = ((Editor)this).get_serializedObject().FindProperty("m_ShowToolkit");
			m_ShowCustomSorter = ((Editor)this).get_serializedObject().FindProperty("m_ShowCustomSorter");
		}

		private void OnDisable()
		{
			m_CustomLists = null;
		}

		public override void OnInspectorGUI()
		{
			((Editor)this).get_serializedObject().Update();
			Camera component = ((Component)base.m_Target).GetComponent<Camera>();
			DoVolumeBlending();
			DoAntialiasing();
			DoFog(component);
			EditorGUILayout.PropertyField(m_StopNaNPropagation, EditorUtilities.GetContent("Stop NaN Propagation|Automatically replaces NaN/Inf in shaders by a black pixel to avoid breaking some effects. This will slightly affect performances and should only be used if you experience NaN issues that you can't fix. Has no effect on GLES2 platforms."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.Space();
			DoToolkit();
			DoCustomEffectSorter();
			EditorUtilities.DrawSplitter();
			EditorGUILayout.Space();
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		private void DoVolumeBlending()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Expected O, but got Unknown
			EditorGUILayout.LabelField(EditorUtilities.GetContent("Volume blending"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			float num = (float)EditorGUI.get_indentLevel() * 15f;
			Rect rect = GUILayoutUtility.GetRect(1f, EditorGUIUtility.get_singleLineHeight());
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), EditorGUIUtility.get_labelWidth() - num, ((Rect)(ref rect)).get_height());
			Rect val2 = default(Rect);
			((Rect)(ref val2))._002Ector(((Rect)(ref val)).get_xMax(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width() - ((Rect)(ref val)).get_width() - 60f, ((Rect)(ref rect)).get_height());
			Rect val3 = new Rect(((Rect)(ref val2)).get_xMax(), ((Rect)(ref rect)).get_y(), 60f, ((Rect)(ref rect)).get_height());
			EditorGUI.PrefixLabel(val, EditorUtilities.GetContent("Trigger|A transform that will act as a trigger for volume blending."));
			m_VolumeTrigger.set_objectReferenceValue((Object)(Transform)EditorGUI.ObjectField(val2, m_VolumeTrigger.get_objectReferenceValue(), typeof(Transform), true));
			if (GUI.Button(val3, EditorUtilities.GetContent("This|Assigns the current GameObject as a trigger."), EditorStyles.get_miniButton()))
			{
				m_VolumeTrigger.set_objectReferenceValue((Object)(object)((Component)base.m_Target).get_transform());
			}
			if (m_VolumeTrigger.get_objectReferenceValue() == (Object)null)
			{
				EditorGUILayout.HelpBox("No trigger has been set, the camera will only be affected by global volumes.", (MessageType)1);
			}
			EditorGUILayout.PropertyField(m_VolumeLayer, EditorUtilities.GetContent("Layer|This camera will only be affected by volumes in the selected scene-layers."), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			int intValue = m_VolumeLayer.get_intValue();
			if (intValue == 0)
			{
				EditorGUILayout.HelpBox("No layer has been set, the trigger will never be affected by volumes.", (MessageType)2);
			}
			else if (intValue == -1 || ((uint)intValue & (true ? 1u : 0u)) != 0)
			{
				EditorGUILayout.HelpBox("Do not use \"Everything\" or \"Default\" as a layer mask as it will slow down the volume blending process! Put post-processing volumes in their own dedicated layer for best performances.", (MessageType)2);
			}
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
			EditorGUILayout.Space();
		}

		private void DoAntialiasing()
		{
			EditorGUILayout.LabelField(EditorUtilities.GetContent("Anti-aliasing"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
			m_AntialiasingMode.set_intValue(EditorGUILayout.Popup(EditorUtilities.GetContent("Mode|The anti-aliasing method to use. FXAA is fast but low quality. SMAA works well for non-HDR scenes. TAA is a bit slower but higher quality and works well with HDR."), m_AntialiasingMode.get_intValue(), s_AntialiasingMethodNames, (GUILayoutOption[])(object)new GUILayoutOption[0]));
			if (m_AntialiasingMode.get_intValue() == 3)
			{
				EditorGUILayout.PropertyField(m_TaaJitterSpread, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_TaaStationaryBlending, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_TaaMotionBlending, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_TaaSharpness, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			else if (m_AntialiasingMode.get_intValue() == 2)
			{
				if (RuntimeUtilities.isSinglePassStereoSelected)
				{
					EditorGUILayout.HelpBox("SMAA doesn't work with Single-pass stereo rendering.", (MessageType)2);
				}
				EditorGUILayout.PropertyField(m_SmaaQuality, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (m_SmaaQuality.get_intValue() != 0 && EditorUtilities.isTargetingConsolesOrMobiles)
				{
					EditorGUILayout.HelpBox("For performance reasons it is recommended to use Low Quality on mobile and console platforms.", (MessageType)2);
				}
			}
			else if (m_AntialiasingMode.get_intValue() == 1)
			{
				EditorGUILayout.PropertyField(m_FxaaFastMode, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(m_FxaaKeepAlpha, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (!m_FxaaFastMode.get_boolValue() && EditorUtilities.isTargetingConsolesOrMobiles)
				{
					EditorGUILayout.HelpBox("For performance reasons it is recommended to use Fast Mode on mobile and console platforms.", (MessageType)2);
				}
			}
			EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
			EditorGUILayout.Space();
		}

		private void DoFog(Camera camera)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			if (!((Object)(object)camera == (Object)null) && (int)camera.get_actualRenderingPath() == 3)
			{
				EditorGUILayout.LabelField(EditorUtilities.GetContent("Deferred Fog"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				EditorGUILayout.PropertyField(m_FogEnabled, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (m_FogEnabled.get_boolValue())
				{
					EditorGUILayout.PropertyField(m_FogExcludeSkybox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					EditorGUILayout.HelpBox("This adds fog compatibility to the deferred rendering path; actual fog settings should be set in the Lighting panel.", (MessageType)1);
				}
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				EditorGUILayout.Space();
			}
		}

		private void DoToolkit()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Expected O, but got Unknown
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			EditorUtilities.DrawSplitter();
			m_ShowToolkit.set_boolValue(EditorUtilities.DrawHeader("Toolkit", m_ShowToolkit.get_boolValue()));
			if (!m_ShowToolkit.get_boolValue())
			{
				return;
			}
			GUILayout.Space(2f);
			if (GUILayout.Button(EditorUtilities.GetContent("Export frame to EXR..."), EditorStyles.get_miniButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				GenericMenu val = new GenericMenu();
				val.AddItem(EditorUtilities.GetContent("Full Frame (as displayed)"), false, (MenuFunction)delegate
				{
					ExportFrameToExr(ExportMode.FullFrame);
				});
				val.AddItem(EditorUtilities.GetContent("Disable post-processing"), false, (MenuFunction)delegate
				{
					ExportFrameToExr(ExportMode.DisablePost);
				});
				val.AddItem(EditorUtilities.GetContent("Break before Color Grading (Linear)"), false, (MenuFunction)delegate
				{
					ExportFrameToExr(ExportMode.BreakBeforeColorGradingLinear);
				});
				val.AddItem(EditorUtilities.GetContent("Break before Color Grading (Log)"), false, (MenuFunction)delegate
				{
					ExportFrameToExr(ExportMode.BreakBeforeColorGradingLog);
				});
				val.ShowAsContext();
			}
			if (GUILayout.Button(EditorUtilities.GetContent("Select all layer volumes|Selects all the volumes that will influence this layer."), EditorStyles.get_miniButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Object[] array = (from x in RuntimeUtilities.GetAllSceneObjects<PostProcessVolume>()
					where (m_VolumeLayer.get_intValue() & (1 << ((Component)x).get_gameObject().get_layer())) != 0
					select ((Component)x).get_gameObject()).Cast<Object>().ToArray();
				if (array.Length != 0)
				{
					Selection.set_objects(array);
				}
			}
			if (GUILayout.Button(EditorUtilities.GetContent("Select all active volumes|Selects all volumes currently affecting the layer."), EditorStyles.get_miniButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				List<PostProcessVolume> list = new List<PostProcessVolume>();
				PostProcessManager.instance.GetActiveVolumes(base.m_Target, list);
				if (list.Count > 0)
				{
					Selection.set_objects(list.Select((PostProcessVolume x) => ((Component)x).get_gameObject()).Cast<Object>().ToArray());
				}
			}
			GUILayout.Space(3f);
		}

		private void DoCustomEffectSorter()
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Expected O, but got Unknown
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Expected O, but got Unknown
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Expected O, but got Unknown
			EditorUtilities.DrawSplitter();
			m_ShowCustomSorter.set_boolValue(EditorUtilities.DrawHeader("Custom Effect Sorting", m_ShowCustomSorter.get_boolValue()));
			if (!m_ShowCustomSorter.get_boolValue())
			{
				return;
			}
			bool flag = false;
			if (m_CustomLists == null)
			{
				if (base.m_Target.sortedBundles == null)
				{
					Scene scene = ((Component)base.m_Target).get_gameObject().get_scene();
					flag = string.IsNullOrEmpty(((Scene)(ref scene)).get_name());
					if (!flag)
					{
						((Editor)this).Repaint();
					}
				}
				else
				{
					m_CustomLists = new Dictionary<PostProcessEvent, ReorderableList>();
					foreach (PostProcessEvent item in Enum.GetValues(typeof(PostProcessEvent)).Cast<PostProcessEvent>())
					{
						List<PostProcessLayer.SerializedBundleRef> list2 = base.m_Target.sortedBundles[item];
						string listName = ObjectNames.NicifyVariableName(item.ToString());
						ReorderableList list = new ReorderableList((IList)list2, typeof(PostProcessLayer.SerializedBundleRef), true, true, false, false);
						list.drawHeaderCallback = (HeaderCallbackDelegate)delegate(Rect rect)
						{
							//IL_0000: Unknown result type (might be due to invalid IL or missing references)
							EditorGUI.LabelField(rect, listName);
						};
						list.drawElementCallback = (ElementCallbackDelegate)delegate(Rect rect, int index, bool isActive, bool isFocused)
						{
							//IL_0017: Unknown result type (might be due to invalid IL or missing references)
							PostProcessLayer.SerializedBundleRef serializedBundleRef = (PostProcessLayer.SerializedBundleRef)list.get_list()[index];
							EditorGUI.LabelField(rect, serializedBundleRef.bundle.attribute.menuItem);
						};
						list.onReorderCallback = (ReorderCallbackDelegate)delegate
						{
							EditorUtility.SetDirty((Object)(object)base.m_Target);
						};
						m_CustomLists.Add(item, list);
					}
				}
			}
			GUILayout.Space(5f);
			if (flag)
			{
				EditorGUILayout.HelpBox("Not supported in prefabs.", (MessageType)1);
				GUILayout.Space(3f);
				return;
			}
			bool flag2 = false;
			if (m_CustomLists != null)
			{
				foreach (KeyValuePair<PostProcessEvent, ReorderableList> customList in m_CustomLists)
				{
					ReorderableList value = customList.Value;
					if (value.get_count() != 0)
					{
						value.DoLayoutList();
						flag2 = true;
					}
				}
			}
			if (!flag2)
			{
				EditorGUILayout.HelpBox("No custom effect loaded.", (MessageType)1);
				GUILayout.Space(3f);
			}
		}

		private void ExportFrameToExr(ExportMode mode)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			string text = EditorUtility.SaveFilePanel("Export EXR...", "", "Frame", "exr");
			if (!string.IsNullOrEmpty(text))
			{
				EditorUtility.DisplayProgressBar("Export EXR", "Rendering...", 0f);
				Camera component = ((Component)base.m_Target).GetComponent<Camera>();
				int pixelWidth = component.get_pixelWidth();
				int pixelHeight = component.get_pixelHeight();
				Texture2D val = new Texture2D(pixelWidth, pixelHeight, (TextureFormat)20, false, true);
				RenderTexture val2 = RenderTexture.GetTemporary(pixelWidth, pixelHeight, 24, (RenderTextureFormat)11, (RenderTextureReadWrite)1);
				RenderTexture active = RenderTexture.get_active();
				RenderTexture targetTexture = component.get_targetTexture();
				bool enabled = ((Behaviour)base.m_Target).get_enabled();
				bool breakBeforeColorGrading = base.m_Target.breakBeforeColorGrading;
				switch (mode)
				{
				case ExportMode.DisablePost:
					((Behaviour)base.m_Target).set_enabled(false);
					break;
				case ExportMode.BreakBeforeColorGradingLinear:
				case ExportMode.BreakBeforeColorGradingLog:
					base.m_Target.breakBeforeColorGrading = true;
					break;
				}
				component.set_targetTexture(val2);
				component.Render();
				component.set_targetTexture(targetTexture);
				EditorUtility.DisplayProgressBar("Export EXR", "Reading...", 0.25f);
				((Behaviour)base.m_Target).set_enabled(enabled);
				base.m_Target.breakBeforeColorGrading = breakBeforeColorGrading;
				if (mode == ExportMode.BreakBeforeColorGradingLog)
				{
					Material val3 = new Material(Shader.Find("Hidden/PostProcessing/Editor/ConvertToLog"));
					RenderTexture temporary = RenderTexture.GetTemporary(pixelWidth, pixelHeight, 0, (RenderTextureFormat)11, (RenderTextureReadWrite)1);
					Graphics.Blit((Texture)(object)val2, temporary, val3, 0);
					RenderTexture.ReleaseTemporary(val2);
					Object.DestroyImmediate((Object)(object)val3);
					val2 = temporary;
				}
				RenderTexture.set_active(val2);
				val.ReadPixels(new Rect(0f, 0f, (float)pixelWidth, (float)pixelHeight), 0, 0);
				val.Apply();
				RenderTexture.set_active(active);
				EditorUtility.DisplayProgressBar("Export EXR", "Encoding...", 0.5f);
				byte[] bytes = ImageConversion.EncodeToEXR(val, (EXRFlags)3);
				EditorUtility.DisplayProgressBar("Export EXR", "Saving...", 0.75f);
				File.WriteAllBytes(text, bytes);
				EditorUtility.ClearProgressBar();
				AssetDatabase.Refresh();
				RenderTexture.ReleaseTemporary(val2);
				Object.DestroyImmediate((Object)(object)val);
			}
		}
	}
}
