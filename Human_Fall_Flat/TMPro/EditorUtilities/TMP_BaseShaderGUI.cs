using System;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public abstract class TMP_BaseShaderGUI : ShaderGUI
	{
		protected class ShaderFeature
		{
			public string undoLabel;

			public GUIContent label;

			public GUIContent[] keywordLabels;

			public string[] keywords;

			private int state;

			public bool Active => state >= 0;

			public int State => state;

			public void ReadState(Material material)
			{
				for (int i = 0; i < keywords.Length; i++)
				{
					if (material.IsKeywordEnabled(keywords[i]))
					{
						state = i;
						return;
					}
				}
				state = -1;
			}

			public void SetActive(bool active, Material material)
			{
				state = ((!active) ? (-1) : 0);
				SetStateKeywords(material);
			}

			public void DoPopup(MaterialEditor editor, Material material)
			{
				EditorGUI.BeginChangeCheck();
				int num = EditorGUILayout.Popup(label, state + 1, keywordLabels, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (EditorGUI.EndChangeCheck())
				{
					state = num - 1;
					editor.RegisterPropertyChangeUndo(undoLabel);
					SetStateKeywords(material);
				}
			}

			private void SetStateKeywords(Material material)
			{
				for (int i = 0; i < keywords.Length; i++)
				{
					if (i == state)
					{
						material.EnableKeyword(keywords[i]);
					}
					else
					{
						material.DisableKeyword(keywords[i]);
					}
				}
			}
		}

		protected class MaterialPanel
		{
			private string key;

			private string label;

			private bool expanded;

			public bool Expanded => expanded;

			public string Label => label;

			public MaterialPanel(string name, bool expandedByDefault)
			{
				label = "<b>" + name + "</b> - <i>Settings</i> -";
				key = "TexMeshPro.material." + name + ".expanded";
				expanded = EditorPrefs.GetBool(key, expandedByDefault);
			}

			public void ToggleExpanded()
			{
				expanded = !expanded;
				EditorPrefs.SetBool(key, expanded);
			}
		}

		private static GUIContent tempLabel;

		private static int undoRedoCount;

		private static int lastSeenUndoRedoCount;

		private static float[][] tempFloats;

		protected static GUIContent[] xywhVectorLabels;

		protected static GUIContent[] lbrtVectorLabels;

		private bool isNewGUI = true;

		private float dragAndDropMinY;

		protected MaterialEditor editor;

		protected Material material;

		protected MaterialProperty[] properties;

		static TMP_BaseShaderGUI()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Expected O, but got Unknown
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Expected O, but got Unknown
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Expected O, but got Unknown
			tempLabel = new GUIContent();
			tempFloats = new float[5][]
			{
				null,
				new float[1],
				new float[2],
				new float[3],
				new float[4]
			};
			xywhVectorLabels = (GUIContent[])(object)new GUIContent[4]
			{
				new GUIContent("X"),
				new GUIContent("Y"),
				new GUIContent("W", "Width"),
				new GUIContent("H", "Height")
			};
			lbrtVectorLabels = (GUIContent[])(object)new GUIContent[4]
			{
				new GUIContent("L", "Left"),
				new GUIContent("B", "Bottom"),
				new GUIContent("R", "Right"),
				new GUIContent("T", "Top")
			};
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Combine((Delegate)(object)Undo.undoRedoPerformed, (Delegate)(UndoRedoCallback)delegate
			{
				undoRedoCount++;
			});
		}

		private void PrepareGUI()
		{
			isNewGUI = false;
			TMP_UIStyleManager.GetUIStyles();
			ShaderUtilities.GetShaderPropertyIDs();
			if (lastSeenUndoRedoCount != undoRedoCount)
			{
				TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, material);
			}
			lastSeenUndoRedoCount = undoRedoCount;
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			editor = materialEditor;
			ref Material val = ref material;
			Object target = ((Editor)materialEditor).get_target();
			val = (Material)(object)((target is Material) ? target : null);
			this.properties = properties;
			if (isNewGUI)
			{
				PrepareGUI();
			}
			EditorGUIUtility.set_labelWidth(130f);
			EditorGUIUtility.set_fieldWidth(50f);
			DoDragAndDropBegin();
			EditorGUI.BeginChangeCheck();
			DoGUI();
			if (EditorGUI.EndChangeCheck())
			{
				TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, material);
			}
			DoDragAndDropEnd();
		}

		protected abstract void DoGUI();

		protected bool DoPanelHeader(MaterialPanel panel)
		{
			if (GUILayout.Button(panel.Label, TMP_UIStyleManager.Group_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				panel.ToggleExpanded();
			}
			return panel.Expanded;
		}

		protected bool DoPanelHeader(MaterialPanel panel, ShaderFeature feature, bool readState = true)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			Rect controlRect = EditorGUILayout.GetControlRect(false, 22f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.Label(controlRect, GUIContent.none, TMP_UIStyleManager.Group_Label);
			if (GUI.Button(new Rect(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y(), 250f, ((Rect)(ref controlRect)).get_height()), panel.Label, TMP_UIStyleManager.Group_Label_Left))
			{
				panel.ToggleExpanded();
			}
			if (readState)
			{
				feature.ReadState(material);
			}
			EditorGUI.BeginChangeCheck();
			float labelWidth = EditorGUIUtility.get_labelWidth();
			EditorGUIUtility.set_labelWidth(70f);
			bool active = EditorGUI.Toggle(new Rect(((Rect)(ref controlRect)).get_width() - 90f, ((Rect)(ref controlRect)).get_y() + 3f, 90f, 22f), new GUIContent("Enable ->"), feature.Active);
			EditorGUIUtility.set_labelWidth(labelWidth);
			if (EditorGUI.EndChangeCheck())
			{
				editor.RegisterPropertyChangeUndo(feature.undoLabel);
				feature.SetActive(active, material);
			}
			return panel.Expanded;
		}

		private MaterialProperty BeginProperty(string name)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = ShaderGUI.FindProperty(name, properties);
			EditorGUI.BeginChangeCheck();
			EditorGUI.set_showMixedValue(val.get_hasMixedValue());
			editor.BeginAnimatedCheck(Rect.get_zero(), val);
			return val;
		}

		private bool EndProperty()
		{
			editor.EndAnimatedCheck();
			EditorGUI.set_showMixedValue(false);
			return EditorGUI.EndChangeCheck();
		}

		protected void DoPopup(string name, string label, GUIContent[] options)
		{
			MaterialProperty val = BeginProperty(name);
			tempLabel.set_text(label);
			int num = EditorGUILayout.Popup(tempLabel, (int)val.get_floatValue(), options, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EndProperty())
			{
				val.set_floatValue((float)num);
			}
		}

		protected void DoCubeMap(string name, string label)
		{
			DoTexture(name, label, typeof(Cubemap));
		}

		protected void DoTexture2D(string name, string label, bool withTilingOffset = false, string[] speedNames = null)
		{
			DoTexture(name, label, typeof(Texture2D), withTilingOffset, speedNames);
		}

		private void DoTexture(string name, string label, Type type, bool withTilingOffset = false, string[] speedNames = null)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			Rect controlRect = EditorGUILayout.GetControlRect(true, 60f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			float width = ((Rect)(ref controlRect)).get_width();
			((Rect)(ref controlRect)).set_width(EditorGUIUtility.get_labelWidth() + 60f);
			tempLabel.set_text(label);
			Object val2 = EditorGUI.ObjectField(controlRect, tempLabel, (Object)(object)val.get_textureValue(), type, false);
			if (EndProperty())
			{
				val.set_textureValue((Texture)(object)((val2 is Texture) ? val2 : null));
			}
			((Rect)(ref controlRect)).set_x(((Rect)(ref controlRect)).get_x() + (((Rect)(ref controlRect)).get_width() + 4f));
			((Rect)(ref controlRect)).set_width(width - ((Rect)(ref controlRect)).get_width() - 4f);
			((Rect)(ref controlRect)).set_height(EditorGUIUtility.get_singleLineHeight());
			if (withTilingOffset)
			{
				DoTilingOffset(controlRect, val);
				((Rect)(ref controlRect)).set_y(((Rect)(ref controlRect)).get_y() + (((Rect)(ref controlRect)).get_height() + 2f) * 2f);
			}
			if (speedNames != null)
			{
				DoUVSpeed(controlRect, speedNames);
			}
		}

		private void DoTilingOffset(Rect rect, MaterialProperty property)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			int indentLevel = EditorGUI.get_indentLevel();
			EditorGUI.set_indentLevel(0);
			EditorGUIUtility.set_labelWidth(40f);
			Vector4 textureScaleAndOffset = property.get_textureScaleAndOffset();
			bool flag = false;
			float[] array = tempFloats[2];
			tempLabel.set_text("Tiling");
			Rect val = EditorGUI.PrefixLabel(rect, tempLabel);
			array[0] = textureScaleAndOffset.x;
			array[1] = textureScaleAndOffset.y;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(val, xywhVectorLabels, array);
			if (EndProperty())
			{
				textureScaleAndOffset.x = array[0];
				textureScaleAndOffset.y = array[1];
				flag = true;
			}
			((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + (((Rect)(ref rect)).get_height() + 2f));
			tempLabel.set_text("Offset");
			Rect val2 = EditorGUI.PrefixLabel(rect, tempLabel);
			array[0] = textureScaleAndOffset.z;
			array[1] = textureScaleAndOffset.w;
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(val2, xywhVectorLabels, array);
			if (EndProperty())
			{
				textureScaleAndOffset.z = array[0];
				textureScaleAndOffset.w = array[1];
				flag = true;
			}
			if (flag)
			{
				property.set_textureScaleAndOffset(textureScaleAndOffset);
			}
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUI.set_indentLevel(indentLevel);
		}

		protected void DoUVSpeed(Rect rect, string[] names)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			int indentLevel = EditorGUI.get_indentLevel();
			EditorGUI.set_indentLevel(0);
			EditorGUIUtility.set_labelWidth(40f);
			tempLabel.set_text("Speed");
			rect = EditorGUI.PrefixLabel(rect, tempLabel);
			EditorGUIUtility.set_labelWidth(13f);
			((Rect)(ref rect)).set_width(((Rect)(ref rect)).get_width() * 0.5f - 1f);
			DoFloat(rect, names[0], "X");
			((Rect)(ref rect)).set_x(((Rect)(ref rect)).get_x() + (((Rect)(ref rect)).get_width() + 2f));
			DoFloat(rect, names[1], "Y");
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUI.set_indentLevel(indentLevel);
		}

		protected void DoToggle(string name, string label)
		{
			MaterialProperty val = BeginProperty(name);
			tempLabel.set_text(label);
			bool flag = EditorGUILayout.Toggle(tempLabel, val.get_floatValue() == 1f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EndProperty())
			{
				val.set_floatValue(flag ? 1f : 0f);
			}
		}

		protected void DoFloat(string name, string label)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			Rect controlRect = EditorGUILayout.GetControlRect((GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Rect)(ref controlRect)).set_width(225f);
			tempLabel.set_text(label);
			float floatValue = EditorGUI.FloatField(controlRect, tempLabel, val.get_floatValue());
			if (EndProperty())
			{
				val.set_floatValue(floatValue);
			}
		}

		protected void DoColor(string name, string label)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			tempLabel.set_text(label);
			Color colorValue = EditorGUI.ColorField(EditorGUILayout.GetControlRect((GUILayoutOption[])(object)new GUILayoutOption[0]), tempLabel, val.get_colorValue());
			if (EndProperty())
			{
				val.set_colorValue(colorValue);
			}
		}

		private void DoFloat(Rect rect, string name, string label)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			tempLabel.set_text(label);
			float floatValue = EditorGUI.FloatField(rect, tempLabel, val.get_floatValue());
			if (EndProperty())
			{
				val.set_floatValue(floatValue);
			}
		}

		protected void DoSlider(string name, string label)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			Vector2 rangeLimits = val.get_rangeLimits();
			tempLabel.set_text(label);
			float floatValue = EditorGUI.Slider(EditorGUILayout.GetControlRect((GUILayoutOption[])(object)new GUILayoutOption[0]), tempLabel, val.get_floatValue(), rangeLimits.x, rangeLimits.y);
			if (EndProperty())
			{
				val.set_floatValue(floatValue);
			}
		}

		protected void DoVector3(string name, string label)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			tempLabel.set_text(label);
			Vector4 vectorValue = Vector4.op_Implicit(EditorGUILayout.Vector3Field(tempLabel, Vector4.op_Implicit(val.get_vectorValue()), (GUILayoutOption[])(object)new GUILayoutOption[0]));
			if (EndProperty())
			{
				val.set_vectorValue(vectorValue);
			}
		}

		protected void DoVector(string name, string label, GUIContent[] subLabels)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			MaterialProperty val = BeginProperty(name);
			Rect controlRect = EditorGUILayout.GetControlRect((GUILayoutOption[])(object)new GUILayoutOption[0]);
			tempLabel.set_text(label);
			controlRect = EditorGUI.PrefixLabel(controlRect, tempLabel);
			Vector4 vectorValue = val.get_vectorValue();
			float[] array = tempFloats[subLabels.Length];
			for (int i = 0; i < subLabels.Length; i++)
			{
				array[i] = ((Vector4)(ref vectorValue)).get_Item(i);
			}
			EditorGUI.MultiFloatField(controlRect, subLabels, array);
			if (EndProperty())
			{
				for (int j = 0; j < subLabels.Length; j++)
				{
					((Vector4)(ref vectorValue)).set_Item(j, array[j]);
				}
				val.set_vectorValue(vectorValue);
			}
		}

		protected void DoEmptyLine()
		{
			GUILayout.Space(EditorGUIUtility.get_singleLineHeight());
		}

		private void DoDragAndDropBegin()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
			dragAndDropMinY = ((Rect)(ref rect)).get_y();
		}

		private void DoDragAndDropEnd()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Invalid comparison between Unknown and I4
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Invalid comparison between Unknown and I4
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			Rect rect = GUILayoutUtility.GetRect(0f, 0f, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.ExpandWidth(true) });
			Event current = Event.get_current();
			if ((int)current.get_type() == 9)
			{
				DragAndDrop.set_visualMode((DragAndDropVisualMode)4);
				current.Use();
			}
			else
			{
				if ((int)current.get_type() != 10)
				{
					return;
				}
				Rect val = Rect.MinMaxRect(((Rect)(ref rect)).get_xMin(), dragAndDropMinY, ((Rect)(ref rect)).get_xMax(), ((Rect)(ref rect)).get_yMax());
				if (((Rect)(ref val)).Contains(current.get_mousePosition()))
				{
					DragAndDrop.AcceptDrag();
					current.Use();
					Object obj = DragAndDrop.get_objectReferences()[0];
					Material val2 = (Material)(object)((obj is Material) ? obj : null);
					if (Object.op_Implicit((Object)(object)val2) && (Object)(object)val2 != (Object)(object)material)
					{
						PerformDrop(val2);
					}
				}
			}
		}

		private void PerformDrop(Material droppedMaterial)
		{
			Texture texture = droppedMaterial.GetTexture(ShaderUtilities.ID_MainTex);
			if (!Object.op_Implicit((Object)(object)texture))
			{
				return;
			}
			Texture texture2 = material.GetTexture(ShaderUtilities.ID_MainTex);
			TMP_FontAsset tMP_FontAsset = null;
			if ((Object)(object)texture != (Object)(object)texture2)
			{
				tMP_FontAsset = TMP_EditorUtility.FindMatchingFontAsset(droppedMaterial);
				if (!Object.op_Implicit((Object)(object)tMP_FontAsset))
				{
					return;
				}
			}
			GameObject[] gameObjects = Selection.get_gameObjects();
			foreach (GameObject val in gameObjects)
			{
				if (Object.op_Implicit((Object)(object)tMP_FontAsset))
				{
					TMP_Text component = val.GetComponent<TMP_Text>();
					if (Object.op_Implicit((Object)(object)component))
					{
						Undo.RecordObject((Object)(object)component, "Font Asset Change");
						component.font = tMP_FontAsset;
					}
				}
				TMPro_EventManager.ON_DRAG_AND_DROP_MATERIAL_CHANGED(val, material, droppedMaterial);
				EditorUtility.SetDirty((Object)(object)val);
			}
		}

		protected TMP_BaseShaderGUI()
			: this()
		{
		}
	}
}
