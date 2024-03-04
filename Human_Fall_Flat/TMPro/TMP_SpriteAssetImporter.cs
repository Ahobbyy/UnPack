using System;
using System.Collections.Generic;
using System.IO;
using TMPro.EditorUtilities;
using TMPro.SpriteAssetUtilities;
using UnityEditor;
using UnityEngine;

namespace TMPro
{
	public class TMP_SpriteAssetImporter : EditorWindow
	{
		private Texture2D m_SpriteAtlas;

		private SpriteAssetImportFormats m_SpriteDataFormat = SpriteAssetImportFormats.TexturePacker;

		private TextAsset m_JsonFile;

		private string m_CreationFeedback;

		private TMP_SpriteAsset m_SpriteAsset;

		private List<TMP_Sprite> m_SpriteInfoList = new List<TMP_Sprite>();

		[MenuItem("Window/TextMeshPro/Sprite Importer")]
		public static void ShowFontAtlasCreatorWindow()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			TMP_SpriteAssetImporter window = EditorWindow.GetWindow<TMP_SpriteAssetImporter>();
			((EditorWindow)window).set_titleContent(new GUIContent("Sprite Importer"));
			((EditorWindow)window).Focus();
		}

		private void OnEnable()
		{
			SetEditorWindowSize();
			TMP_UIStyleManager.GetUIStyles();
		}

		public void OnGUI()
		{
			DrawEditorPanel();
		}

		private void DrawEditorPanel()
		{
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("<b>TMP Sprite Importer</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Import Settings", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(150f) });
			GUILayout.BeginVertical(TMP_UIStyleManager.TextureAreaBox, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			ref TextAsset jsonFile = ref m_JsonFile;
			Object obj = EditorGUILayout.ObjectField("Sprite Data Source", (Object)(object)m_JsonFile, typeof(TextAsset), false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			jsonFile = (TextAsset)(object)((obj is TextAsset) ? obj : null);
			m_SpriteDataFormat = (SpriteAssetImportFormats)(object)EditorGUILayout.EnumPopup("Import Format", (Enum)m_SpriteDataFormat, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			ref Texture2D spriteAtlas = ref m_SpriteAtlas;
			Object obj2 = EditorGUILayout.ObjectField("Sprite Texture Atlas", (Object)(object)m_SpriteAtlas, typeof(Texture2D), false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			spriteAtlas = (Texture2D)(object)((obj2 is Texture2D) ? obj2 : null);
			if (EditorGUI.EndChangeCheck())
			{
				m_CreationFeedback = string.Empty;
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Create Sprite Asset", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_CreationFeedback = string.Empty;
				if (m_SpriteDataFormat == SpriteAssetImportFormats.TexturePacker)
				{
					TexturePacker.SpriteDataObject spriteDataObject = JsonUtility.FromJson<TexturePacker.SpriteDataObject>(m_JsonFile.get_text());
					if (spriteDataObject != null && spriteDataObject.frames != null && spriteDataObject.frames.Count > 0)
					{
						int count = spriteDataObject.frames.Count;
						m_CreationFeedback = "<b>Import Results</b>\n--------------------\n";
						m_CreationFeedback = m_CreationFeedback + "<color=#C0ffff><b>" + count + "</b></color> Sprites were imported from file.";
						m_SpriteInfoList = CreateSpriteInfoList(spriteDataObject);
					}
				}
			}
			GUILayout.Space(5f);
			GUILayout.BeginVertical(TMP_UIStyleManager.TextAreaBoxWindow, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(60f) });
			EditorGUILayout.LabelField(m_CreationFeedback, TMP_UIStyleManager.Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.Space(5f);
			GUI.set_enabled((m_SpriteInfoList != null) ? true : false);
			if (GUILayout.Button("Save Sprite Asset", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				string empty = string.Empty;
				empty = EditorUtility.SaveFilePanel("Save Sprite Asset File", new FileInfo(AssetDatabase.GetAssetPath((Object)(object)m_JsonFile)).DirectoryName, ((Object)m_JsonFile).get_name(), "asset");
				if (empty.Length == 0)
				{
					return;
				}
				SaveSpriteAsset(empty);
			}
			GUILayout.EndVertical();
			GUILayout.EndVertical();
		}

		private List<TMP_Sprite> CreateSpriteInfoList(TexturePacker.SpriteDataObject spriteDataObject)
		{
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			List<TexturePacker.SpriteData> frames = spriteDataObject.frames;
			List<TMP_Sprite> list = new List<TMP_Sprite>();
			for (int i = 0; i < frames.Count; i++)
			{
				TMP_Sprite tMP_Sprite = new TMP_Sprite();
				tMP_Sprite.id = i;
				tMP_Sprite.name = Path.GetFileNameWithoutExtension(frames[i].filename);
				tMP_Sprite.hashCode = TMP_TextUtilities.GetSimpleHashCode(tMP_Sprite.name);
				int num = 0;
				int num2 = tMP_Sprite.name.IndexOf('-');
				num = (tMP_Sprite.unicode = ((num2 == -1) ? TMP_TextUtilities.StringToInt(tMP_Sprite.name) : TMP_TextUtilities.StringToInt(tMP_Sprite.name.Substring(num2 + 1))));
				tMP_Sprite.x = frames[i].frame.x;
				tMP_Sprite.y = (float)((Texture)m_SpriteAtlas).get_height() - (frames[i].frame.y + frames[i].frame.h);
				tMP_Sprite.width = frames[i].frame.w;
				tMP_Sprite.height = frames[i].frame.h;
				tMP_Sprite.pivot = frames[i].pivot;
				tMP_Sprite.xAdvance = tMP_Sprite.width;
				tMP_Sprite.scale = 1f;
				tMP_Sprite.xOffset = 0f - tMP_Sprite.width * tMP_Sprite.pivot.x;
				tMP_Sprite.yOffset = tMP_Sprite.height - tMP_Sprite.height * tMP_Sprite.pivot.y;
				list.Add(tMP_Sprite);
			}
			return list;
		}

		private void SaveSpriteAsset(string filePath)
		{
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
			m_SpriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
			AssetDatabase.CreateAsset((Object)(object)m_SpriteAsset, text + ".asset");
			m_SpriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(((Object)m_SpriteAsset).get_name());
			m_SpriteAsset.spriteSheet = (Texture)(object)m_SpriteAtlas;
			m_SpriteAsset.spriteInfoList = m_SpriteInfoList;
			AddDefaultMaterial(m_SpriteAsset);
		}

		private static void AddDefaultMaterial(TMP_SpriteAsset spriteAsset)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			Material val = new Material(Shader.Find("TextMeshPro/Sprite"));
			val.SetTexture(ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);
			spriteAsset.material = val;
			((Object)val).set_hideFlags((HideFlags)1);
			AssetDatabase.AddObjectToAsset((Object)(object)val, (Object)(object)spriteAsset);
		}

		private void SetEditorWindowSize()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			Vector2 minSize = ((EditorWindow)this).get_minSize();
			((EditorWindow)this).set_minSize(new Vector2(Mathf.Max(230f, minSize.x), Mathf.Max(300f, minSize.y)));
		}

		public TMP_SpriteAssetImporter()
			: this()
		{
		}
	}
}
