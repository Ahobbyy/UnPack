using System;
using System.Text;
using UnityEditor;
using UnityEngine;

public class pg_AboutWindow : EditorWindow
{
	[Serializable]
	public struct AdvertisementThumb
	{
		public Texture2D image;

		public string url;

		public string about;

		public GUIContent guiContent;

		public AdvertisementThumb(string imagePath, string url, string about)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			guiContent = new GUIContent("", about);
			image = (Texture2D)AssetDatabase.LoadAssetAtPath(imagePath, typeof(Texture2D));
			guiContent.set_image((Texture)(object)image);
			this.url = url;
			this.about = about;
		}
	}

	private const string ABOUT_ROOT = "Assets/ProCore/ProGrids/About";

	[SerializeField]
	public static AdvertisementThumb[] advertisements = new AdvertisementThumb[7]
	{
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/ProBuilder_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/probuilder/", "Build and Texture Geometry In-Editor"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/ProGrids_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/progrids/", "True Grids and Grid-Snapping"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/ProGroups_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/progroups/", "Hide, Freeze, Group, & Organize"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/Prototype_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/prototype/", "Design and Build With Zero Lag"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/QuickBrush_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/quickbrush/", "Quickly Add Detail Geometry"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/QuickDecals_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/quickdecals/", "Add Dirt, Splatters, Posters, etc"),
		new AdvertisementThumb("Assets/ProCore/ProGrids/About/Images/QuickEdit_AssetStore_Icon_96px.png", "http://www.protoolsforunity3d.com/quickedit/", "Edit Imported Meshes!")
	};

	private string AboutEntryPath = "";

	private string ProductName = "";

	private string ProductVersion = "";

	private string ChangelogPath = "";

	private string BannerPath = "Assets/ProCore/ProGrids/About/Images/Banner.png";

	private const int AD_HEIGHT = 96;

	private Texture2D banner;

	private string changelog = "";

	private Color LinkColor = new Color(0f, 0.682f, 0.937f, 1f);

	private GUIStyle boldTextStyle;

	private GUIStyle headerTextStyle;

	private GUIStyle linkTextStyle;

	private GUIStyle advertisementStyle;

	private Vector2 scroll = Vector2.get_zero();

	private Vector2 adScroll = Vector2.get_zero();

	private const string RemoveBraketsRegex = "(\\<.*?\\>)";

	public static bool Init(string aboutEntryPath, bool fromMenu)
	{
		if (!GetField(aboutEntryPath, "version: ", out var value) || !GetField(aboutEntryPath, "identifier: ", out var value2))
		{
			return false;
		}
		if (fromMenu || EditorPrefs.GetString(value2) != value)
		{
			if (!GetField(aboutEntryPath, "name: ", out var value3) || !value3.Contains("ProGrids"))
			{
				return false;
			}
			pg_AboutWindow obj = (pg_AboutWindow)(object)EditorWindow.GetWindow(typeof(pg_AboutWindow), true, value3, true);
			obj.SetAboutEntryPath(aboutEntryPath);
			((EditorWindow)obj).ShowUtility();
			EditorPrefs.SetString(value2, value);
			return true;
		}
		return false;
	}

	public void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		banner = (Texture2D)AssetDatabase.LoadAssetAtPath(BannerPath, typeof(Texture2D));
		((EditorWindow)this).set_minSize(new Vector2((float)(((Texture)banner).get_width() + 12), (float)(((Texture)banner).get_height() * 7)));
		((EditorWindow)this).set_maxSize(new Vector2((float)(((Texture)banner).get_width() + 12), (float)(((Texture)banner).get_height() * 7)));
	}

	public void SetAboutEntryPath(string path)
	{
		AboutEntryPath = path;
		PopulateDataFields(AboutEntryPath);
	}

	private void OnGUI()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		headerTextStyle = (GUIStyle)(((object)headerTextStyle) ?? ((object)new GUIStyle(EditorStyles.get_boldLabel())));
		headerTextStyle.set_fontSize(16);
		linkTextStyle = (GUIStyle)(((object)linkTextStyle) ?? ((object)new GUIStyle(GUI.get_skin().get_label())));
		linkTextStyle.get_normal().set_textColor(LinkColor);
		linkTextStyle.set_alignment((TextAnchor)3);
		boldTextStyle = (GUIStyle)(((object)boldTextStyle) ?? ((object)new GUIStyle(GUI.get_skin().get_label())));
		boldTextStyle.set_fontStyle((FontStyle)1);
		boldTextStyle.set_alignment((TextAnchor)3);
		advertisementStyle = (GUIStyle)(((object)advertisementStyle) ?? ((object)new GUIStyle(GUI.get_skin().get_button())));
		advertisementStyle.get_normal().set_background((Texture2D)null);
		if ((Object)(object)banner != (Object)null)
		{
			GUILayout.Label((Texture)(object)banner, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}
		GUILayout.Label("Thank you for purchasing " + ProductName + ". Your support allows us to keep developing this and future tools for everyone.", EditorStyles.get_wordWrappedLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(2f);
		GUILayout.Label("Read these quick \"ProTips\" before starting:", headerTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("1) ", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(16f),
			GUILayout.MaxWidth(16f)
		});
		GUILayout.Label("Register", boldTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(58f),
			GUILayout.MaxWidth(58f)
		});
		GUILayout.Label("for instant email updates, send your invoice # to", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(284f),
			GUILayout.MaxWidth(284f)
		});
		if (GUILayout.Button("contact@procore3d.com", linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(142f),
			GUILayout.MaxWidth(142f)
		}))
		{
			Application.OpenURL("mailto:contact@procore3d.com?subject=Sign me up for the Beta!");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("2) ", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(16f),
			GUILayout.MaxWidth(16f)
		});
		GUILayout.Label("Report bugs", boldTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(82f),
			GUILayout.MaxWidth(82f)
		});
		GUILayout.Label("to the ProCore Forum at", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(144f),
			GUILayout.MaxWidth(144f)
		});
		if (GUILayout.Button("www.procore3d.com/forum", linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(162f),
			GUILayout.MaxWidth(162f)
		}))
		{
			Application.OpenURL("http://www.procore3d.com/forum");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("3) ", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(16f),
			GUILayout.MaxWidth(16f)
		});
		GUILayout.Label("Customize!", boldTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(74f),
			GUILayout.MaxWidth(74f)
		});
		GUILayout.Label("Click on \"Edit > Preferences\" then \"" + ProductName + "\"", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(276f),
			GUILayout.MaxWidth(276f)
		});
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("4) ", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(16f),
			GUILayout.MaxWidth(16f)
		});
		GUILayout.Label("Documentation", boldTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(102f),
			GUILayout.MaxWidth(102f)
		});
		GUILayout.Label("Tutorials, & more info:", (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(132f),
			GUILayout.MaxWidth(132f)
		});
		if (GUILayout.Button("www.procore3d.com/" + ProductName.ToLower(), linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinWidth(190f),
			GUILayout.MaxWidth(190f)
		}))
		{
			Application.OpenURL("http://www.procore3d.com/" + ProductName.ToLower());
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(50f) });
		GUILayout.Label("Links:", boldTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		linkTextStyle.set_fontStyle((FontStyle)2);
		linkTextStyle.set_alignment((TextAnchor)4);
		if (GUILayout.Button("procore3d.com", linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Application.OpenURL("http://www.procore3d.com");
		}
		if (GUILayout.Button("facebook", linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Application.OpenURL("http://www.facebook.com/probuilder3d");
		}
		if (GUILayout.Button("twitter", linkTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Application.OpenURL("http://www.twitter.com/probuilder3d");
		}
		linkTextStyle.set_fontStyle((FontStyle)0);
		GUILayout.EndHorizontal();
		GUILayout.Space(4f);
		HorizontalLine();
		scroll = EditorGUILayout.BeginScrollView(scroll, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label(ProductName + "  |  version: " + ProductVersion, EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("\n" + changelog, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.EndScrollView();
		HorizontalLine();
		GUILayout.Label("More ProCore Products", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		int num = ((advertisements.Length * 96 > Screen.get_width()) ? 22 : 6);
		adScroll = EditorGUILayout.BeginScrollView(adScroll, false, false, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinHeight((float)(96 + num)),
			GUILayout.MaxHeight((float)(96 + num))
		});
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		AdvertisementThumb[] array = advertisements;
		for (int i = 0; i < array.Length; i++)
		{
			AdvertisementThumb advertisementThumb = array[i];
			if (!advertisementThumb.url.ToLower().Contains(ProductName.ToLower()) && GUILayout.Button(advertisementThumb.guiContent, advertisementStyle, (GUILayoutOption[])(object)new GUILayoutOption[4]
			{
				GUILayout.MinWidth(96f),
				GUILayout.MaxWidth(96f),
				GUILayout.MinHeight(96f),
				GUILayout.MaxHeight(96f)
			}))
			{
				Application.OpenURL(advertisementThumb.url);
			}
		}
		GUILayout.EndHorizontal();
		EditorGUILayout.EndScrollView();
	}

	private void HorizontalLine()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Rect lastRect = GUILayoutUtility.GetLastRect();
		Color backgroundColor = GUI.get_backgroundColor();
		GUI.set_backgroundColor(Color.get_black());
		GUI.Box(new Rect(0f, ((Rect)(ref lastRect)).get_y() + ((Rect)(ref lastRect)).get_height() + 2f, (float)Screen.get_width(), 2f), "");
		GUI.set_backgroundColor(backgroundColor);
		GUILayout.Space(6f);
	}

	private void PopulateDataFields(string entryPath)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		TextAsset val = (TextAsset)AssetDatabase.LoadAssetAtPath(entryPath, typeof(TextAsset));
		ProductName = "";
		ProductVersion = "";
		ChangelogPath = "";
		if ((Object)(object)val != (Object)null)
		{
			string[] array = val.get_text().Split('\n');
			foreach (string text in array)
			{
				if (text.StartsWith("name:"))
				{
					ProductName = text.Replace("name: ", "").Trim();
				}
				else if (text.StartsWith("version:"))
				{
					ProductVersion = text.Replace("version: ", "").Trim();
				}
				else if (text.StartsWith("changelog:"))
				{
					ChangelogPath = text.Replace("changelog: ", "").Trim();
				}
			}
		}
		TextAsset val2 = (TextAsset)AssetDatabase.LoadAssetAtPath(ChangelogPath, typeof(TextAsset));
		if (Object.op_Implicit((Object)(object)val2))
		{
			string[] array2 = val2.get_text().Split(new string[1] { "--" }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder stringBuilder = new StringBuilder();
			string[] array3 = array2[0].Trim().Split('\n');
			for (int j = 2; j < array3.Length; j++)
			{
				stringBuilder.AppendLine(array3[j]);
			}
			changelog = stringBuilder.ToString();
		}
	}

	private static bool GetField(string path, string field, out string value)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		TextAsset val = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
		value = "";
		if (!Object.op_Implicit((Object)(object)val))
		{
			return false;
		}
		string[] array = val.get_text().Split('\n');
		foreach (string text in array)
		{
			if (text.Contains(field))
			{
				value = text.Replace(field, "").Trim();
				return true;
			}
		}
		return false;
	}

	public pg_AboutWindow()
		: this()
	{
	}//IL_0057: Unknown result type (might be due to invalid IL or missing references)
	//IL_005c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0062: Unknown result type (might be due to invalid IL or missing references)
	//IL_0067: Unknown result type (might be due to invalid IL or missing references)
	//IL_006d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0072: Unknown result type (might be due to invalid IL or missing references)

}
