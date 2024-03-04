using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public class TMPro_FontAssetCreatorWindow : EditorWindow
	{
		private enum FontPackingModes
		{
			Fast = 0,
			Optimum = 4
		}

		private enum PreviewSelectionTypes
		{
			PreviewFont,
			PreviewTexture,
			PreviewDistanceField
		}

		private string[] FontSizingOptions = new string[2] { "Auto Sizing", "Custom Size" };

		private int FontSizingOption_Selection;

		private string[] FontResolutionLabels = new string[10] { "16", "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192" };

		private int[] FontAtlasResolutions = new int[10] { 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

		private string[] FontCharacterSets = new string[9] { "ASCII", "Extended ASCII", "ASCII Lowercase", "ASCII Uppercase", "Numbers + Symbols", "Custom Range", "Unicode Range (Hex)", "Custom Characters", "Characters from File" };

		private FontPackingModes m_fontPackingSelection;

		private int font_CharacterSet_Selection;

		private PreviewSelectionTypes previewSelection;

		private string characterSequence = "";

		private string output_feedback = "";

		private string output_name_label = "Font: ";

		private string output_size_label = "Pt. Size: ";

		private string output_count_label = "Characters packed: ";

		private int m_character_Count;

		private Vector2 output_ScrollPosition;

		private Color[] Output;

		private bool isDistanceMapReady;

		private bool isRepaintNeeded;

		private Rect progressRect;

		public static float ProgressPercentage;

		private float m_renderingProgress;

		private bool isRenderingDone;

		private bool isProcessing;

		private Object font_TTF;

		private TMP_FontAsset m_fontAssetSelection;

		private TextAsset characterList;

		private int font_size;

		private int font_padding = 5;

		private FaceStyles font_style;

		private float font_style_mod = 2f;

		private RenderModes font_renderMode = RenderModes.DistanceField16;

		private int font_atlas_width = 512;

		private int font_atlas_height = 512;

		private int font_scaledownFactor = 1;

		private FT_FaceInfo m_font_faceInfo;

		private FT_GlyphInfo[] m_font_glyphInfo;

		private byte[] m_texture_buffer;

		private Texture2D m_font_Atlas;

		private Texture2D m_destination_Atlas;

		private bool includeKerningPairs;

		private int[] m_kerningSet;

		private EditorWindow m_editorWindow;

		private Vector2 m_previewWindow_Size = new Vector2(768f, 768f);

		private Rect m_UI_Panel_Size;

		public string m_saveOverridePath;

		[MenuItem("Window/TextMeshPro/Font Asset Creator")]
		public static void ShowFontAtlasCreatorWindow()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			TMPro_FontAssetCreatorWindow window = EditorWindow.GetWindow<TMPro_FontAssetCreatorWindow>();
			((EditorWindow)window).set_titleContent(new GUIContent("Asset Creator"));
			window.m_saveOverridePath = null;
			((EditorWindow)window).Focus();
		}

		public void OnEnable()
		{
			m_editorWindow = (EditorWindow)(object)this;
			UpdateEditorWindowSize(768f, 768f);
			TMP_UIStyleManager.GetUIStyles();
			ShaderUtilities.GetShaderPropertyIDs();
			TMPro_EventManager.COMPUTE_DT_EVENT.Add(ON_COMPUTE_DT_EVENT);
		}

		public void OnDisable()
		{
			TMPro_EventManager.COMPUTE_DT_EVENT.Remove(ON_COMPUTE_DT_EVENT);
			if (TMPro_FontPlugin.Initialize_FontEngine() == 99)
			{
				TMPro_FontPlugin.Destroy_FontEngine();
			}
			if ((Object)(object)m_destination_Atlas != (Object)null && !EditorUtility.IsPersistent((Object)(object)m_destination_Atlas))
			{
				Object.DestroyImmediate((Object)(object)m_destination_Atlas);
			}
			if ((Object)(object)m_font_Atlas != (Object)null && !EditorUtility.IsPersistent((Object)(object)m_font_Atlas))
			{
				Object.DestroyImmediate((Object)(object)m_font_Atlas);
			}
			string fullPath = Path.GetFullPath("Assets/..");
			if (File.Exists(fullPath + "/Assets/Glyph Report.txt"))
			{
				File.Delete(fullPath + "/Assets/Glyph Report.txt");
				File.Delete(fullPath + "/Assets/Glyph Report.txt.meta");
				AssetDatabase.Refresh();
			}
			Resources.UnloadUnusedAssets();
		}

		public void OnGUI()
		{
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(310f) });
			DrawControls();
			DrawPreview();
			GUILayout.EndHorizontal();
		}

		public void ON_COMPUTE_DT_EVENT(object Sender, Compute_DT_EventArgs e)
		{
			if (e.EventType == Compute_DistanceTransform_EventTypes.Completed)
			{
				Output = e.Colors;
				isProcessing = false;
				isDistanceMapReady = true;
			}
			else if (e.EventType == Compute_DistanceTransform_EventTypes.Processing)
			{
				ProgressPercentage = e.ProgressPercentage;
				isRepaintNeeded = true;
			}
		}

		public void Update()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			if (isDistanceMapReady)
			{
				if ((Object)(object)m_font_Atlas != (Object)null)
				{
					m_destination_Atlas = new Texture2D(((Texture)m_font_Atlas).get_width() / font_scaledownFactor, ((Texture)m_font_Atlas).get_height() / font_scaledownFactor, (TextureFormat)1, false, true);
					m_destination_Atlas.SetPixels(Output);
					m_destination_Atlas.Apply(false, true);
				}
				isDistanceMapReady = false;
				((EditorWindow)this).Repaint();
			}
			if (isRepaintNeeded)
			{
				isRepaintNeeded = false;
				((EditorWindow)this).Repaint();
			}
			if (isProcessing)
			{
				m_renderingProgress = TMPro_FontPlugin.Check_RenderProgress();
				isRepaintNeeded = true;
			}
			if (isRenderingDone)
			{
				isProcessing = false;
				isRenderingDone = false;
				UpdateRenderFeedbackWindow();
				CreateFontTexture();
			}
		}

		private int[] ParseNumberSequence(string sequence)
		{
			List<int> list = new List<int>();
			string[] array = sequence.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('-');
				if (array2.Length == 1)
				{
					try
					{
						list.Add(int.Parse(array2[0]));
					}
					catch
					{
						Debug.Log((object)"No characters selected or invalid format.");
					}
				}
				else
				{
					for (int j = int.Parse(array2[0]); j < int.Parse(array2[1]) + 1; j++)
					{
						list.Add(j);
					}
				}
			}
			return list.ToArray();
		}

		private int[] ParseHexNumberSequence(string sequence)
		{
			List<int> list = new List<int>();
			string[] array = sequence.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('-');
				if (array2.Length == 1)
				{
					try
					{
						list.Add(int.Parse(array2[0], NumberStyles.AllowHexSpecifier));
					}
					catch
					{
						Debug.Log((object)"No characters selected or invalid format.");
					}
				}
				else
				{
					for (int j = int.Parse(array2[0], NumberStyles.AllowHexSpecifier); j < int.Parse(array2[1], NumberStyles.AllowHexSpecifier) + 1; j++)
					{
						list.Add(j);
					}
				}
			}
			return list.ToArray();
		}

		private void DrawControls()
		{
			//IL_0385: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b96: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
			//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0df4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfa: Invalid comparison between Unknown and I4
			//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dfe: Unknown result type (might be due to invalid IL or missing references)
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>TextMeshPro - Font Asset Creator</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(300f) });
			GUILayout.Label("Font Settings", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(300f) });
			GUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(300f) });
			EditorGUIUtility.set_labelWidth(120f);
			EditorGUIUtility.set_fieldWidth(160f);
			EditorGUI.BeginChangeCheck();
			ref Object val = ref font_TTF;
			Object obj = EditorGUILayout.ObjectField("Font Source", font_TTF, typeof(Font), false, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			val = ((obj is Font) ? obj : null);
			EditorGUI.EndChangeCheck();
			if (FontSizingOption_Selection == 0)
			{
				FontSizingOption_Selection = EditorGUILayout.Popup("Font Size", FontSizingOption_Selection, FontSizingOptions, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			}
			else
			{
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUIUtility.set_fieldWidth(40f);
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
				FontSizingOption_Selection = EditorGUILayout.Popup("Font Size", FontSizingOption_Selection, FontSizingOptions, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(225f) });
				font_size = EditorGUILayout.IntField(font_size, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
			}
			EditorGUIUtility.set_labelWidth(120f);
			EditorGUIUtility.set_fieldWidth(160f);
			font_padding = EditorGUILayout.IntField("Font Padding", font_padding, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			font_padding = (int)Mathf.Clamp((float)font_padding, 0f, 64f);
			m_fontPackingSelection = (FontPackingModes)(object)EditorGUILayout.EnumPopup("Packing Method", (Enum)m_fontPackingSelection, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(225f) });
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			GUI.set_changed(false);
			EditorGUIUtility.set_labelWidth(120f);
			EditorGUIUtility.set_fieldWidth(40f);
			GUILayout.Label("Atlas Resolution:", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(116f) });
			font_atlas_width = EditorGUILayout.IntPopup(font_atlas_width, FontResolutionLabels, FontAtlasResolutions, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			font_atlas_height = EditorGUILayout.IntPopup(font_atlas_height, FontResolutionLabels, FontAtlasResolutions, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();
			bool flag = false;
			font_CharacterSet_Selection = EditorGUILayout.Popup("Character Set", font_CharacterSet_Selection, FontCharacterSets, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			if (EditorGUI.EndChangeCheck())
			{
				characterSequence = "";
				flag = true;
			}
			switch (font_CharacterSet_Selection)
			{
			case 0:
				characterSequence = "32 - 126, 160, 8203, 8230, 9633";
				break;
			case 1:
				characterSequence = "32 - 126, 160 - 255, 8192 - 8303, 8364, 8482, 9633";
				break;
			case 2:
				characterSequence = "32 - 64, 91 - 126, 160";
				break;
			case 3:
				characterSequence = "32 - 96, 123 - 126, 160";
				break;
			case 4:
				characterSequence = "32 - 64, 91 - 96, 123 - 126, 160";
				break;
			case 5:
			{
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Enter a sequence of decimal values to define the characters to be included in the font asset or retrieve one from another font asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				EditorGUI.BeginChangeCheck();
				m_fontAssetSelection = EditorGUILayout.ObjectField("Select Font Asset", (Object)(object)m_fontAssetSelection, typeof(TMP_FontAsset), false, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) }) as TMP_FontAsset;
				if ((EditorGUI.EndChangeCheck() || flag) && (Object)(object)m_fontAssetSelection != (Object)null)
				{
					characterSequence = TMP_EditorUtility.GetDecimalCharacterSequence(TMP_FontAsset.GetCharactersArray(m_fontAssetSelection));
				}
				EditorGUIUtility.set_labelWidth(120f);
				char character = Event.get_current().get_character();
				if ((character < '0' || character > '9') && (character < ',' || character > '-'))
				{
					Event.get_current().set_character('\0');
				}
				GUILayout.Label("Character Sequence (Decimal)", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				characterSequence = EditorGUILayout.TextArea(characterSequence, TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(120f),
					GUILayout.MaxWidth(290f)
				});
				EditorGUILayout.EndVertical();
				break;
			}
			case 6:
			{
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Enter a sequence of Unicode (hex) values to define the characters to be included in the font asset or retrieve one from another font asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				EditorGUI.BeginChangeCheck();
				m_fontAssetSelection = EditorGUILayout.ObjectField("Select Font Asset", (Object)(object)m_fontAssetSelection, typeof(TMP_FontAsset), false, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) }) as TMP_FontAsset;
				if ((EditorGUI.EndChangeCheck() || flag) && (Object)(object)m_fontAssetSelection != (Object)null)
				{
					characterSequence = TMP_EditorUtility.GetUnicodeCharacterSequence(TMP_FontAsset.GetCharactersArray(m_fontAssetSelection));
				}
				EditorGUIUtility.set_labelWidth(120f);
				char character = Event.get_current().get_character();
				if ((character < '0' || character > '9') && (character < 'a' || character > 'f') && (character < 'A' || character > 'F') && (character < ',' || character > '-'))
				{
					Event.get_current().set_character('\0');
				}
				GUILayout.Label("Character Sequence (Hex)", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				characterSequence = EditorGUILayout.TextArea(characterSequence, TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(120f),
					GUILayout.MaxWidth(290f)
				});
				EditorGUILayout.EndVertical();
				break;
			}
			case 7:
				EditorGUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label("Type the characters to be included in the font asset or retrieve them from another font asset.", TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Space(10f);
				EditorGUI.BeginChangeCheck();
				m_fontAssetSelection = EditorGUILayout.ObjectField("Select Font Asset", (Object)(object)m_fontAssetSelection, typeof(TMP_FontAsset), false, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) }) as TMP_FontAsset;
				if ((EditorGUI.EndChangeCheck() || flag) && (Object)(object)m_fontAssetSelection != (Object)null)
				{
					characterSequence = TMP_FontAsset.GetCharacters(m_fontAssetSelection);
				}
				EditorGUIUtility.set_labelWidth(120f);
				EditorGUI.set_indentLevel(0);
				GUILayout.Label("Custom Character List", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				characterSequence = EditorGUILayout.TextArea(characterSequence, TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Height(120f),
					GUILayout.MaxWidth(290f)
				});
				EditorGUILayout.EndVertical();
				break;
			case 8:
			{
				ref TextAsset val2 = ref characterList;
				Object obj2 = EditorGUILayout.ObjectField("Character File", (Object)(object)characterList, typeof(TextAsset), false, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
				val2 = (TextAsset)(object)((obj2 is TextAsset) ? obj2 : null);
				if ((Object)(object)characterList != (Object)null)
				{
					characterSequence = characterList.get_text();
				}
				break;
			}
			}
			EditorGUIUtility.set_labelWidth(120f);
			EditorGUIUtility.set_fieldWidth(40f);
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			font_style = (FaceStyles)(object)EditorGUILayout.EnumPopup("Font Style:", (Enum)font_style, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(225f) });
			font_style_mod = EditorGUILayout.IntField((int)font_style_mod, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.EndHorizontal();
			EditorGUI.BeginChangeCheck();
			font_renderMode = (RenderModes)(object)EditorGUILayout.EnumPopup("Font Render Mode:", (Enum)font_renderMode, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) });
			EditorGUI.EndChangeCheck();
			includeKerningPairs = EditorGUILayout.Toggle("Get Kerning Pairs?", includeKerningPairs, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(290f) });
			EditorGUIUtility.set_labelWidth(120f);
			EditorGUIUtility.set_fieldWidth(160f);
			GUILayout.Space(20f);
			GUI.set_enabled((!(font_TTF == (Object)null) && !isProcessing) ? true : false);
			if (GUILayout.Button("Generate Font Atlas", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) }) && characterSequence.Length != 0 && GUI.get_enabled() && font_TTF != (Object)null)
			{
				int error_Code = TMPro_FontPlugin.Initialize_FontEngine();
				if (error_Code != 0)
				{
					if (error_Code == 99)
					{
						error_Code = 0;
					}
					else
					{
						Debug.Log((object)("Error Code: " + error_Code + "  occurred while Initializing the FreeType Library."));
					}
				}
				string assetPath = AssetDatabase.GetAssetPath(font_TTF);
				if (error_Code == 0)
				{
					error_Code = TMPro_FontPlugin.Load_TrueType_Font(assetPath);
					if (error_Code != 0)
					{
						if (error_Code == 99)
						{
							error_Code = 0;
						}
						else
						{
							Debug.Log((object)("Error Code: " + error_Code + "  occurred while Loading the font."));
						}
					}
				}
				if (error_Code == 0)
				{
					if (FontSizingOption_Selection == 0)
					{
						font_size = 72;
					}
					error_Code = TMPro_FontPlugin.FT_Size_Font(font_size);
					if (error_Code != 0)
					{
						Debug.Log((object)("Error Code: " + error_Code + "  occurred while Sizing the font."));
					}
				}
				if (error_Code == 0)
				{
					int[] character_Set = null;
					if (font_CharacterSet_Selection == 7 || font_CharacterSet_Selection == 8)
					{
						List<int> list = new List<int>();
						int i;
						for (i = 0; i < characterSequence.Length; i++)
						{
							if (list.FindIndex((int item) => item == characterSequence[i]) == -1)
							{
								list.Add(characterSequence[i]);
							}
						}
						character_Set = list.ToArray();
					}
					else if (font_CharacterSet_Selection == 6)
					{
						character_Set = ParseHexNumberSequence(characterSequence);
					}
					else
					{
						character_Set = ParseNumberSequence(characterSequence);
					}
					m_character_Count = character_Set.Length;
					m_texture_buffer = new byte[font_atlas_width * font_atlas_height];
					m_font_faceInfo = default(FT_FaceInfo);
					m_font_glyphInfo = new FT_GlyphInfo[m_character_Count];
					int padding = font_padding;
					bool autoSizing = ((FontSizingOption_Selection == 0) ? true : false);
					float strokeSize = font_style_mod;
					if (font_renderMode == RenderModes.DistanceField16)
					{
						strokeSize = font_style_mod * 16f;
					}
					if (font_renderMode == RenderModes.DistanceField32)
					{
						strokeSize = font_style_mod * 32f;
					}
					isProcessing = true;
					ThreadPool.QueueUserWorkItem(delegate
					{
						isRenderingDone = false;
						error_Code = TMPro_FontPlugin.Render_Characters(m_texture_buffer, font_atlas_width, font_atlas_height, padding, character_Set, m_character_Count, font_style, strokeSize, autoSizing, font_renderMode, (int)m_fontPackingSelection, ref m_font_faceInfo, m_font_glyphInfo);
						isRenderingDone = true;
					});
					previewSelection = PreviewSelectionTypes.PreviewFont;
				}
			}
			GUILayout.Space(1f);
			progressRect = GUILayoutUtility.GetRect(288f, 20f, TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(288f),
				GUILayout.Height(20f)
			});
			GUI.BeginGroup(progressRect);
			GUI.DrawTextureWithTexCoords(new Rect(2f, 0f, 288f, 20f), (Texture)(object)TMP_UIStyleManager.progressTexture, new Rect(1f - m_renderingProgress, 0f, 1f, 1f));
			GUI.EndGroup();
			GUISkin skin = GUI.get_skin();
			GUI.set_skin(TMP_UIStyleManager.TMP_GUISkin);
			GUILayout.Space(5f);
			GUILayout.BeginVertical(TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			output_ScrollPosition = EditorGUILayout.BeginScrollView(output_ScrollPosition, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(145f) });
			EditorGUILayout.LabelField(output_feedback, TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUI.set_skin(skin);
			GUILayout.Space(10f);
			GUI.set_enabled(((Object)(object)m_font_Atlas != (Object)null) ? true : false);
			if (GUILayout.Button("Save TextMeshPro Font Asset", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(290f) }) && GUI.get_enabled())
			{
				string empty = string.Empty;
				if (font_renderMode < RenderModes.DistanceField16)
				{
					if (m_saveOverridePath != null)
					{
						string directoryName = Path.GetDirectoryName(m_saveOverridePath);
						string fileName = Path.GetFileName(m_saveOverridePath);
						empty = EditorUtility.SaveFilePanel("Save TextMesh Pro! Font Asset File", directoryName, fileName, "asset");
					}
					else
					{
						empty = EditorUtility.SaveFilePanel("Save TextMesh Pro! Font Asset File", new FileInfo(AssetDatabase.GetAssetPath(font_TTF)).DirectoryName, font_TTF.get_name(), "asset");
					}
					if (empty.Length == 0)
					{
						return;
					}
					Save_Normal_FontAsset(empty);
				}
				else if (font_renderMode >= RenderModes.DistanceField16)
				{
					if (m_saveOverridePath != null && m_saveOverridePath.Length != 0)
					{
						string directoryName2 = Path.GetDirectoryName(m_saveOverridePath);
						string fileName2 = Path.GetFileName(m_saveOverridePath);
						empty = EditorUtility.SaveFilePanel("Save TextMesh Pro! Font Asset File", directoryName2, fileName2, "asset");
					}
					else
					{
						empty = EditorUtility.SaveFilePanel("Save TextMesh Pro! Font Asset File", new FileInfo(AssetDatabase.GetAssetPath(font_TTF)).DirectoryName, font_TTF.get_name() + " SDF", "asset");
					}
					if (empty.Length == 0)
					{
						return;
					}
					Save_SDF_FontAsset(empty);
				}
			}
			GUI.set_enabled(true);
			GUILayout.Space(5f);
			GUILayout.EndVertical();
			GUILayout.Space(25f);
			Rect controlRect = EditorGUILayout.GetControlRect(false, 5f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if ((int)Event.get_current().get_type() == 7)
			{
				m_UI_Panel_Size = controlRect;
			}
			GUILayout.EndVertical();
		}

		private void UpdateRenderFeedbackWindow()
		{
			font_size = m_font_faceInfo.pointSize;
			string empty = string.Empty;
			string text = ((m_font_faceInfo.characterCount == m_character_Count) ? "<color=#C0ffff>" : "<color=#ffff00>");
			string text2 = "<color=#C0ffff>";
			empty = output_name_label + "<b>" + text2 + m_font_faceInfo.name + "</color></b>";
			empty = ((empty.Length <= 60) ? (empty + "  " + output_size_label + "<b>" + text2 + m_font_faceInfo.pointSize + "</color></b>") : (empty + "\n" + output_size_label + "<b>" + text2 + m_font_faceInfo.pointSize + "</color></b>"));
			empty = empty + "\n" + output_count_label + "<b>" + text + m_font_faceInfo.characterCount + "/" + m_character_Count + "</color></b>";
			empty += "\n\n<color=#ffff00><b>Missing Characters</b></color>";
			empty = (output_feedback = empty + "\n----------------------------------------");
			for (int i = 0; i < m_character_Count; i++)
			{
				if (m_font_glyphInfo[i].x == -1f)
				{
					empty = empty + "\nID: <color=#C0ffff>" + m_font_glyphInfo[i].id + "\t</color>Hex: <color=#C0ffff>" + m_font_glyphInfo[i].id.ToString("X") + "\t</color>Char [<color=#C0ffff>" + (char)m_font_glyphInfo[i].id + "</color>]";
					if (empty.Length < 16300)
					{
						output_feedback = empty;
					}
				}
			}
			if (empty.Length > 16300)
			{
				output_feedback += "\n\n<color=#ffff00>Report truncated.</color>\n<color=#c0ffff>See</color> \"TextMesh Pro\\Glyph Report.txt\"";
			}
			string fullPath = Path.GetFullPath("Assets/..");
			File.WriteAllText(contents: Regex.Replace(empty, "<[^>]*>", string.Empty), path: fullPath + "/Assets/Glyph Report.txt");
			AssetDatabase.Refresh();
		}

		private void CreateFontTexture()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			m_font_Atlas = new Texture2D(font_atlas_width, font_atlas_height, (TextureFormat)1, false, true);
			Color32[] array = (Color32[])(object)new Color32[font_atlas_width * font_atlas_height];
			for (int i = 0; i < font_atlas_width * font_atlas_height; i++)
			{
				byte b = m_texture_buffer[i];
				array[i] = new Color32(b, b, b, b);
			}
			if (font_renderMode == RenderModes.RasterHinted)
			{
				((Texture)m_font_Atlas).set_filterMode((FilterMode)0);
			}
			m_font_Atlas.SetPixels32(array, 0);
			m_font_Atlas.Apply(false, true);
			UpdateEditorWindowSize(((Texture)m_font_Atlas).get_width(), ((Texture)m_font_Atlas).get_height());
		}

		private void Save_Normal_FontAsset(string filePath)
		{
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			filePath = filePath.Substring(0, filePath.Length - 6);
			string dataPath = Application.get_dataPath();
			if (filePath.IndexOf(dataPath, StringComparison.InvariantCultureIgnoreCase) == -1)
			{
				Debug.LogError((object)("You're saving the font asset in a directory outside of this project folder. This is not supported. Please select a directory under \"" + dataPath + "\""));
				return;
			}
			string path = filePath.Substring(dataPath.Length - 6);
			string directoryName = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string text = directoryName + "/" + fileNameWithoutExtension;
			TMP_FontAsset tMP_FontAsset = AssetDatabase.LoadAssetAtPath(text + ".asset", typeof(TMP_FontAsset)) as TMP_FontAsset;
			if ((Object)(object)tMP_FontAsset == (Object)null)
			{
				tMP_FontAsset = ScriptableObject.CreateInstance<TMP_FontAsset>();
				AssetDatabase.CreateAsset((Object)(object)tMP_FontAsset, text + ".asset");
				tMP_FontAsset.fontAssetType = TMP_FontAsset.FontAssetTypes.Bitmap;
				FaceInfo faceInfo = GetFaceInfo(m_font_faceInfo, 1);
				tMP_FontAsset.AddFaceInfo(faceInfo);
				TMP_Glyph[] glyphInfo = GetGlyphInfo(m_font_glyphInfo, 1);
				tMP_FontAsset.AddGlyphInfo(glyphInfo);
				if (includeKerningPairs)
				{
					string assetPath = AssetDatabase.GetAssetPath(font_TTF);
					KerningTable kerningTable = GetKerningTable(assetPath, (int)faceInfo.PointSize);
					tMP_FontAsset.AddKerningInfo(kerningTable);
				}
				tMP_FontAsset.atlas = m_font_Atlas;
				((Object)m_font_Atlas).set_name(fileNameWithoutExtension + " Atlas");
				AssetDatabase.AddObjectToAsset((Object)(object)m_font_Atlas, (Object)(object)tMP_FontAsset);
				Material val = new Material(Shader.Find("TextMeshPro/Bitmap"));
				((Object)val).set_name(fileNameWithoutExtension + " Material");
				val.SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)m_font_Atlas);
				tMP_FontAsset.material = val;
				AssetDatabase.AddObjectToAsset((Object)(object)val, (Object)(object)tMP_FontAsset);
			}
			else
			{
				Material[] array = TMP_EditorUtility.FindMaterialReferences(tMP_FontAsset);
				Object.DestroyImmediate((Object)(object)tMP_FontAsset.atlas, true);
				tMP_FontAsset.fontAssetType = TMP_FontAsset.FontAssetTypes.Bitmap;
				FaceInfo faceInfo2 = GetFaceInfo(m_font_faceInfo, 1);
				tMP_FontAsset.AddFaceInfo(faceInfo2);
				TMP_Glyph[] glyphInfo2 = GetGlyphInfo(m_font_glyphInfo, 1);
				tMP_FontAsset.AddGlyphInfo(glyphInfo2);
				if (includeKerningPairs)
				{
					string assetPath2 = AssetDatabase.GetAssetPath(font_TTF);
					KerningTable kerningTable2 = GetKerningTable(assetPath2, (int)faceInfo2.PointSize);
					tMP_FontAsset.AddKerningInfo(kerningTable2);
				}
				tMP_FontAsset.atlas = m_font_Atlas;
				((Object)m_font_Atlas).set_name(fileNameWithoutExtension + " Atlas");
				((Object)m_font_Atlas).set_hideFlags((HideFlags)0);
				((Object)tMP_FontAsset.material).set_hideFlags((HideFlags)0);
				AssetDatabase.AddObjectToAsset((Object)(object)m_font_Atlas, (Object)(object)tMP_FontAsset);
				tMP_FontAsset.material.SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)tMP_FontAsset.atlas);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)m_font_Atlas);
				}
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Object)(object)tMP_FontAsset));
			tMP_FontAsset.ReadFontDefinition();
			AssetDatabase.Refresh();
			m_font_Atlas = null;
			TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(isChanged: true, tMP_FontAsset);
		}

		private void Save_SDF_FontAsset(string filePath)
		{
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Expected O, but got Unknown
			filePath = filePath.Substring(0, filePath.Length - 6);
			string dataPath = Application.get_dataPath();
			if (filePath.IndexOf(dataPath, StringComparison.InvariantCultureIgnoreCase) == -1)
			{
				Debug.LogError((object)("You're saving the font asset in a directory outside of this project folder. This is not supported. Please select a directory under \"" + dataPath + "\""));
				return;
			}
			string path = filePath.Substring(dataPath.Length - 6);
			string directoryName = Path.GetDirectoryName(path);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
			string text = directoryName + "/" + fileNameWithoutExtension;
			TMP_FontAsset tMP_FontAsset = AssetDatabase.LoadAssetAtPath(text + ".asset", typeof(TMP_FontAsset)) as TMP_FontAsset;
			if ((Object)(object)tMP_FontAsset == (Object)null)
			{
				tMP_FontAsset = ScriptableObject.CreateInstance<TMP_FontAsset>();
				AssetDatabase.CreateAsset((Object)(object)tMP_FontAsset, text + ".asset");
				tMP_FontAsset.fontAssetType = TMP_FontAsset.FontAssetTypes.SDF;
				int scaleFactor = ((font_renderMode >= RenderModes.DistanceField16) ? 1 : font_scaledownFactor);
				FaceInfo faceInfo = GetFaceInfo(m_font_faceInfo, scaleFactor);
				tMP_FontAsset.AddFaceInfo(faceInfo);
				TMP_Glyph[] glyphInfo = GetGlyphInfo(m_font_glyphInfo, scaleFactor);
				tMP_FontAsset.AddGlyphInfo(glyphInfo);
				if (includeKerningPairs)
				{
					string assetPath = AssetDatabase.GetAssetPath(font_TTF);
					KerningTable kerningTable = GetKerningTable(assetPath, (int)faceInfo.PointSize);
					tMP_FontAsset.AddKerningInfo(kerningTable);
				}
				tMP_FontAsset.atlas = m_font_Atlas;
				((Object)m_font_Atlas).set_name(fileNameWithoutExtension + " Atlas");
				AssetDatabase.AddObjectToAsset((Object)(object)m_font_Atlas, (Object)(object)tMP_FontAsset);
				Material val = new Material(Shader.Find("TextMeshPro/Distance Field"));
				((Object)val).set_name(fileNameWithoutExtension + " Material");
				val.SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)m_font_Atlas);
				val.SetFloat(ShaderUtilities.ID_TextureWidth, (float)((Texture)m_font_Atlas).get_width());
				val.SetFloat(ShaderUtilities.ID_TextureHeight, (float)((Texture)m_font_Atlas).get_height());
				int num = font_padding + 1;
				val.SetFloat(ShaderUtilities.ID_GradientScale, (float)num);
				val.SetFloat(ShaderUtilities.ID_WeightNormal, tMP_FontAsset.normalStyle);
				val.SetFloat(ShaderUtilities.ID_WeightBold, tMP_FontAsset.boldStyle);
				tMP_FontAsset.material = val;
				AssetDatabase.AddObjectToAsset((Object)(object)val, (Object)(object)tMP_FontAsset);
			}
			else
			{
				Material[] array = TMP_EditorUtility.FindMaterialReferences(tMP_FontAsset);
				Object.DestroyImmediate((Object)(object)tMP_FontAsset.atlas, true);
				tMP_FontAsset.fontAssetType = TMP_FontAsset.FontAssetTypes.SDF;
				int scaleFactor2 = ((font_renderMode >= RenderModes.DistanceField16) ? 1 : font_scaledownFactor);
				FaceInfo faceInfo2 = GetFaceInfo(m_font_faceInfo, scaleFactor2);
				tMP_FontAsset.AddFaceInfo(faceInfo2);
				TMP_Glyph[] glyphInfo2 = GetGlyphInfo(m_font_glyphInfo, scaleFactor2);
				tMP_FontAsset.AddGlyphInfo(glyphInfo2);
				if (includeKerningPairs)
				{
					string assetPath2 = AssetDatabase.GetAssetPath(font_TTF);
					KerningTable kerningTable2 = GetKerningTable(assetPath2, (int)faceInfo2.PointSize);
					tMP_FontAsset.AddKerningInfo(kerningTable2);
				}
				tMP_FontAsset.atlas = m_font_Atlas;
				((Object)m_font_Atlas).set_name(fileNameWithoutExtension + " Atlas");
				((Object)m_font_Atlas).set_hideFlags((HideFlags)0);
				((Object)tMP_FontAsset.material).set_hideFlags((HideFlags)0);
				AssetDatabase.AddObjectToAsset((Object)(object)m_font_Atlas, (Object)(object)tMP_FontAsset);
				tMP_FontAsset.material.SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)tMP_FontAsset.atlas);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetTexture(ShaderUtilities.ID_MainTex, (Texture)(object)m_font_Atlas);
					array[i].SetFloat(ShaderUtilities.ID_TextureWidth, (float)((Texture)m_font_Atlas).get_width());
					array[i].SetFloat(ShaderUtilities.ID_TextureHeight, (float)((Texture)m_font_Atlas).get_height());
					int num2 = font_padding + 1;
					array[i].SetFloat(ShaderUtilities.ID_GradientScale, (float)num2);
					array[i].SetFloat(ShaderUtilities.ID_WeightNormal, tMP_FontAsset.normalStyle);
					array[i].SetFloat(ShaderUtilities.ID_WeightBold, tMP_FontAsset.boldStyle);
				}
			}
			tMP_FontAsset.fontCreationSettings = SaveFontCreationSettings();
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Object)(object)tMP_FontAsset));
			tMP_FontAsset.ReadFontDefinition();
			AssetDatabase.Refresh();
			m_font_Atlas = null;
			TMPro_EventManager.ON_FONT_PROPERTY_CHANGED(isChanged: true, tMP_FontAsset);
		}

		private FontCreationSetting SaveFontCreationSettings()
		{
			FontCreationSetting result = default(FontCreationSetting);
			result.fontSourcePath = AssetDatabase.GetAssetPath(font_TTF);
			result.fontSizingMode = FontSizingOption_Selection;
			result.fontSize = font_size;
			result.fontPadding = font_padding;
			result.fontPackingMode = (int)m_fontPackingSelection;
			result.fontAtlasWidth = font_atlas_width;
			result.fontAtlasHeight = font_atlas_height;
			result.fontCharacterSet = font_CharacterSet_Selection;
			result.fontStyle = (int)font_style;
			result.fontStlyeModifier = font_style_mod;
			result.fontRenderMode = (int)font_renderMode;
			result.fontKerning = includeKerningPairs;
			return result;
		}

		public void LoadFontCreationSettings(FontCreationSetting settings, string newCharList = null)
		{
			if (settings.fontSourcePath != null)
			{
				font_TTF = (Object)(object)AssetDatabase.LoadAssetAtPath<Font>(settings.fontSourcePath);
			}
			else
			{
				font_TTF = null;
			}
			FontSizingOption_Selection = settings.fontSizingMode;
			font_size = settings.fontSize;
			font_padding = settings.fontPadding;
			m_fontPackingSelection = (FontPackingModes)settings.fontPackingMode;
			font_atlas_width = settings.fontAtlasWidth;
			font_atlas_height = settings.fontAtlasHeight;
			font_CharacterSet_Selection = settings.fontCharacterSet;
			font_style = (FaceStyles)settings.fontStyle;
			font_style_mod = settings.fontStlyeModifier;
			font_renderMode = (RenderModes)settings.fontRenderMode;
			includeKerningPairs = settings.fontKerning;
			if (newCharList != null)
			{
				characterSequence = newCharList;
			}
		}

		private void UpdateEditorWindowSize(float width, float height)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			m_previewWindow_Size = new Vector2(768f, 768f);
			if (width > height)
			{
				m_previewWindow_Size = new Vector2(768f, height / (width / 768f));
			}
			else if (height > width)
			{
				m_previewWindow_Size = new Vector2(width / (height / 768f), 768f);
			}
			m_editorWindow.set_minSize(new Vector2(m_previewWindow_Size.x + 330f, Mathf.Max(((Rect)(ref m_UI_Panel_Size)).get_y() + 20f, m_previewWindow_Size.y + 20f)));
			m_editorWindow.set_maxSize(m_editorWindow.get_minSize() + new Vector2(0.25f, 0f));
		}

		private void DrawPreview()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			GUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Rect rect = GUILayoutUtility.GetRect(m_previewWindow_Size.x, m_previewWindow_Size.y, TMP_UIStyleManager.Section_Label);
			if ((Object)(object)m_destination_Atlas != (Object)null && previewSelection == PreviewSelectionTypes.PreviewDistanceField)
			{
				EditorGUI.DrawTextureAlpha(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), m_previewWindow_Size.x, m_previewWindow_Size.y), (Texture)(object)m_destination_Atlas, (ScaleMode)2);
			}
			else if ((Object)(object)m_font_Atlas != (Object)null && previewSelection == PreviewSelectionTypes.PreviewFont)
			{
				EditorGUI.DrawTextureAlpha(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), m_previewWindow_Size.x, m_previewWindow_Size.y), (Texture)(object)m_font_Atlas, (ScaleMode)2);
			}
			GUILayout.EndVertical();
		}

		private FaceInfo GetFaceInfo(FT_FaceInfo ft_face, int scaleFactor)
		{
			FaceInfo faceInfo = new FaceInfo();
			faceInfo.Name = ft_face.name;
			faceInfo.PointSize = (float)ft_face.pointSize / (float)scaleFactor;
			faceInfo.Padding = ft_face.padding / scaleFactor;
			faceInfo.LineHeight = ft_face.lineHeight / (float)scaleFactor;
			faceInfo.CapHeight = 0f;
			faceInfo.Baseline = 0f;
			faceInfo.Ascender = ft_face.ascender / (float)scaleFactor;
			faceInfo.Descender = ft_face.descender / (float)scaleFactor;
			faceInfo.CenterLine = ft_face.centerLine / (float)scaleFactor;
			faceInfo.Underline = ft_face.underline / (float)scaleFactor;
			faceInfo.UnderlineThickness = ((ft_face.underlineThickness == 0f) ? 5f : (ft_face.underlineThickness / (float)scaleFactor));
			faceInfo.strikethrough = (faceInfo.Ascender + faceInfo.Descender) / 2.75f;
			faceInfo.strikethroughThickness = faceInfo.UnderlineThickness;
			faceInfo.SuperscriptOffset = faceInfo.Ascender;
			faceInfo.SubscriptOffset = faceInfo.Underline;
			faceInfo.SubSize = 0.5f;
			faceInfo.AtlasWidth = ft_face.atlasWidth / scaleFactor;
			faceInfo.AtlasHeight = ft_face.atlasHeight / scaleFactor;
			return faceInfo;
		}

		private TMP_Glyph[] GetGlyphInfo(FT_GlyphInfo[] ft_glyphs, int scaleFactor)
		{
			List<TMP_Glyph> list = new List<TMP_Glyph>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < ft_glyphs.Length; i++)
			{
				TMP_Glyph tMP_Glyph = new TMP_Glyph();
				tMP_Glyph.id = ft_glyphs[i].id;
				tMP_Glyph.x = ft_glyphs[i].x / (float)scaleFactor;
				tMP_Glyph.y = ft_glyphs[i].y / (float)scaleFactor;
				tMP_Glyph.width = ft_glyphs[i].width / (float)scaleFactor;
				tMP_Glyph.height = ft_glyphs[i].height / (float)scaleFactor;
				tMP_Glyph.xOffset = ft_glyphs[i].xOffset / (float)scaleFactor;
				tMP_Glyph.yOffset = ft_glyphs[i].yOffset / (float)scaleFactor;
				tMP_Glyph.xAdvance = ft_glyphs[i].xAdvance / (float)scaleFactor;
				if (tMP_Glyph.x != -1f)
				{
					list.Add(tMP_Glyph);
					list2.Add(tMP_Glyph.id);
				}
			}
			m_kerningSet = list2.ToArray();
			return list.ToArray();
		}

		public KerningTable GetKerningTable(string fontFilePath, int pointSize)
		{
			KerningTable kerningTable = new KerningTable();
			kerningTable.kerningPairs = new List<KerningPair>();
			FT_KerningPair[] array = new FT_KerningPair[7500];
			int num = TMPro_FontPlugin.FT_GetKerningPairs(fontFilePath, m_kerningSet, m_kerningSet.Length, array);
			for (int i = 0; i < num; i++)
			{
				KerningPair kp = new KerningPair(array[i].ascII_Left, array[i].ascII_Right, array[i].xAdvanceOffset * (float)pointSize);
				if (kerningTable.kerningPairs.FindIndex((KerningPair item) => item.AscII_Left == kp.AscII_Left && item.AscII_Right == kp.AscII_Right) == -1)
				{
					kerningTable.kerningPairs.Add(kp);
				}
				else if (!TMP_Settings.warningsDisabled)
				{
					Debug.LogWarning((object)("Kerning Key for [" + kp.AscII_Left + "] and [" + kp.AscII_Right + "] is a duplicate."));
				}
			}
			return kerningTable;
		}

		private string[] UpdateShaderList(RenderModes mode, out Shader[] shaders)
		{
			string text = "t:Shader TMP_";
			text = ((mode != RenderModes.DistanceField16 && mode != RenderModes.DistanceField32) ? (text + " Bitmap") : (text + " SDF"));
			string[] array = AssetDatabase.FindAssets(text);
			string[] array2 = new string[array.Length];
			shaders = (Shader[])(object)new Shader[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Shader val = AssetDatabase.LoadAssetAtPath<Shader>(AssetDatabase.GUIDToAssetPath(array[i]));
				shaders[i] = val;
				string text2 = ((Object)val).get_name().Replace("TextMeshPro/", "");
				text2 = (array2[i] = text2.Replace("Mobile/", "Mobile - "));
			}
			return array2;
		}

		public TMPro_FontAssetCreatorWindow()
			: this()
		{
		}//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)

	}
}
