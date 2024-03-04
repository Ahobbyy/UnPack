using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.EditorCommon
{
	public class pb_AboutWindow : EditorWindow
	{
		private const string PACKAGE_NAME = "ProBuilder";

		private static string aboutRoot = "Assets/ProCore/ProBuilder/About";

		private GUIContent gc_Learn = new GUIContent("Learn ProBuilder", "Documentation");

		private GUIContent gc_Forum = new GUIContent("Support Forum", "ProCore Support Forum");

		private GUIContent gc_Contact = new GUIContent("Contact Us", "Send us an email!");

		private GUIContent gc_Banner = new GUIContent("", "ProBuilder Quick-Start Video Tutorials");

		private const string VIDEO_URL = "http://bit.ly/pbstarter";

		private const string LEARN_URL = "http://procore3d.com/docs/probuilder";

		private const string SUPPORT_URL = "http://www.procore3d.com/forum/";

		private const string CONTACT_EMAIL = "http://www.procore3d.com/about/";

		private const float BANNER_WIDTH = 480f;

		private const float BANNER_HEIGHT = 270f;

		internal const string FONT_REGULAR = "Asap-Regular.otf";

		internal const string FONT_MEDIUM = "Asap-Medium.otf";

		public static readonly Color font_white = HexToColor(13553358u);

		public static readonly Color font_black = HexToColor(5526612u);

		public static readonly Color font_blue_normal = HexToColor(43759u);

		public static readonly Color font_blue_hover = HexToColor(35823u);

		private string productName = "ProBuilder";

		private pb_AboutEntry about;

		private string changelogRichText = "";

		internal static GUIStyle bannerStyle;

		internal static GUIStyle header1Style;

		internal static GUIStyle versionInfoStyle;

		internal static GUIStyle linkStyle;

		internal static GUIStyle separatorStyle;

		internal static GUIStyle changelogStyle;

		internal static GUIStyle changelogTextStyle;

		private Vector2 scroll = Vector2.get_zero();

		internal static string AboutRoot
		{
			get
			{
				if (Directory.Exists(aboutRoot))
				{
					return aboutRoot;
				}
				aboutRoot = pb_FileUtil.FindFolder("ProBuilder/About", false);
				if (aboutRoot.EndsWith("/"))
				{
					aboutRoot = aboutRoot.Remove(aboutRoot.LastIndexOf("/"), 1);
				}
				return aboutRoot;
			}
		}

		public static bool Init(bool fromMenu)
		{
			if (!pb_VersionUtil.GetAboutEntry(out var pb_AboutEntry2))
			{
				Debug.LogWarning((object)"Couldn't find pb_AboutEntry_ProBuilder.txt");
				return false;
			}
			if (fromMenu || pb_PreferencesInternal.GetString(pb_AboutEntry2.identifier) != pb_AboutEntry2.version)
			{
				pb_AboutWindow obj = (pb_AboutWindow)(object)EditorWindow.GetWindow(typeof(pb_AboutWindow), true, pb_AboutEntry2.name, true);
				((EditorWindow)obj).ShowUtility();
				obj.SetAbout(pb_AboutEntry2);
				pb_PreferencesInternal.SetString(pb_AboutEntry2.identifier, pb_AboutEntry2.version, (pb_PreferenceLocation)1);
				return true;
			}
			return false;
		}

		private static Color HexToColor(uint x)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			return new Color((float)((x >> 16) & 0xFFu) / 255f, (float)((x >> 8) & 0xFFu) / 255f, (float)(x & 0xFFu) / 255f, 1f);
		}

		public static void InitGuiStyles()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0067: Expected O, but got Unknown
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Expected O, but got Unknown
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Expected O, but got Unknown
			//IL_00d6: Expected O, but got Unknown
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Expected O, but got Unknown
			//IL_013e: Expected O, but got Unknown
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Expected O, but got Unknown
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Expected O, but got Unknown
			//IL_020f: Expected O, but got Unknown
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Expected O, but got Unknown
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Expected O, but got Unknown
			//IL_027e: Expected O, but got Unknown
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0283: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Expected O, but got Unknown
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Expected O, but got Unknown
			//IL_02f9: Expected O, but got Unknown
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Expected O, but got Unknown
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			//IL_0339: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035c: Expected O, but got Unknown
			//IL_035c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Expected O, but got Unknown
			GUIStyle val = new GUIStyle();
			val.set_margin(new RectOffset(12, 12, 12, 12));
			GUIStyleState val2 = new GUIStyleState();
			val2.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>($"{AboutRoot}/Images/Banner_Normal.png"));
			val.set_normal(val2);
			GUIStyleState val3 = new GUIStyleState();
			val3.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>($"{AboutRoot}/Images/Banner_Hover.png"));
			val.set_hover(val3);
			bannerStyle = val;
			GUIStyle val4 = new GUIStyle();
			val4.set_margin(new RectOffset(10, 10, 10, 10));
			val4.set_alignment((TextAnchor)4);
			val4.set_fontSize(24);
			val4.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Medium.otf")));
			GUIStyleState val5 = new GUIStyleState();
			val5.set_textColor(EditorGUIUtility.get_isProSkin() ? font_white : font_black);
			val4.set_normal(val5);
			header1Style = val4;
			GUIStyle val6 = new GUIStyle();
			val6.set_margin(new RectOffset(10, 10, 10, 10));
			val6.set_fontSize(14);
			val6.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Regular.otf")));
			GUIStyleState val7 = new GUIStyleState();
			val7.set_textColor(EditorGUIUtility.get_isProSkin() ? font_white : font_black);
			val6.set_normal(val7);
			versionInfoStyle = val6;
			GUIStyle val8 = new GUIStyle();
			val8.set_margin(new RectOffset(10, 10, 10, 10));
			val8.set_alignment((TextAnchor)4);
			val8.set_fontSize(16);
			val8.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Regular.otf")));
			GUIStyleState val9 = new GUIStyleState();
			val9.set_textColor(font_blue_normal);
			val9.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>(string.Format("{0}/Images/ScrollBackground_{1}.png", AboutRoot, EditorGUIUtility.get_isProSkin() ? "Pro" : "Light")));
			val8.set_normal(val9);
			GUIStyleState val10 = new GUIStyleState();
			val10.set_textColor(font_blue_hover);
			val10.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>(string.Format("{0}/Images/ScrollBackground_{1}.png", AboutRoot, EditorGUIUtility.get_isProSkin() ? "Pro" : "Light")));
			val8.set_hover(val10);
			linkStyle = val8;
			GUIStyle val11 = new GUIStyle();
			val11.set_margin(new RectOffset(10, 10, 10, 10));
			val11.set_alignment((TextAnchor)4);
			val11.set_fontSize(16);
			val11.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Regular.otf")));
			GUIStyleState val12 = new GUIStyleState();
			val12.set_textColor(EditorGUIUtility.get_isProSkin() ? font_white : font_black);
			val11.set_normal(val12);
			separatorStyle = val11;
			GUIStyle val13 = new GUIStyle();
			val13.set_margin(new RectOffset(10, 10, 10, 10));
			val13.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Regular.otf")));
			val13.set_richText(true);
			GUIStyleState val14 = new GUIStyleState();
			val14.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>(string.Format("{0}/Images/ScrollBackground_{1}.png", AboutRoot, EditorGUIUtility.get_isProSkin() ? "Pro" : "Light")));
			val13.set_normal(val14);
			changelogStyle = val13;
			GUIStyle val15 = new GUIStyle();
			val15.set_margin(new RectOffset(10, 10, 10, 10));
			val15.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", AboutRoot, "Asap-Regular.otf")));
			val15.set_fontSize(14);
			GUIStyleState val16 = new GUIStyleState();
			val16.set_textColor(EditorGUIUtility.get_isProSkin() ? font_white : font_black);
			val15.set_normal(val16);
			val15.set_richText(true);
			val15.set_wordWrap(true);
			changelogTextStyle = val15;
		}

		public void OnEnable()
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			InitGuiStyles();
			if ((Object)(object)bannerStyle.get_normal().get_background() == (Object)null)
			{
				Debug.LogWarning((object)"Could not load About window resources");
				((EditorWindow)this).Close();
				return;
			}
			bannerStyle.set_fixedWidth(480f);
			bannerStyle.set_fixedHeight(270f);
			((EditorWindow)this).set_wantsMouseMove(true);
			((EditorWindow)this).set_minSize(new Vector2(504f, 675f));
			((EditorWindow)this).set_maxSize(new Vector2(504f, 675f));
			if (!productName.Contains("Basic"))
			{
				productName = "ProBuilder Advanced";
			}
		}

		private void SetAbout(pb_AboutEntry about)
		{
			this.about = about;
			if (!File.Exists(about.changelogPath))
			{
				about.changelogPath = pb_FileUtil.FindFile("ProBuilder/About/changelog.txt");
			}
			if (File.Exists(about.changelogPath))
			{
				string text = File.ReadAllText(about.changelogPath);
				if (!string.IsNullOrEmpty(text))
				{
					pb_VersionUtil.FormatChangelog(text, out var _, out changelogRichText);
				}
			}
		}

		internal static T LoadAssetAtPath<T>(string InPath) where T : Object
		{
			return (T)(object)AssetDatabase.LoadAssetAtPath(InPath, typeof(T));
		}

		private void OnGUI()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			Vector2 mousePosition = Event.get_current().get_mousePosition();
			if (GUILayout.Button(gc_Banner, bannerStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Application.OpenURL("http://bit.ly/pbstarter");
			}
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if (((Rect)(ref lastRect)).Contains(mousePosition))
			{
				((EditorWindow)this).Repaint();
			}
			GUILayout.BeginVertical(changelogStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label(productName, header1Style, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(gc_Learn, linkStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Application.OpenURL("http://procore3d.com/docs/probuilder");
			}
			GUILayout.Label("|", separatorStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button(gc_Forum, linkStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Application.OpenURL("http://www.procore3d.com/forum/");
			}
			GUILayout.Label("|", separatorStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button(gc_Contact, linkStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Application.OpenURL("http://www.procore3d.com/about/");
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			lastRect = GUILayoutUtility.GetLastRect();
			if (((Rect)(ref lastRect)).Contains(mousePosition))
			{
				((EditorWindow)this).Repaint();
			}
			GUILayout.EndVertical();
			scroll = EditorGUILayout.BeginScrollView(scroll, changelogStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label($"Version: {about.version}", versionInfoStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("\n" + changelogRichText, changelogTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
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

		public pb_AboutWindow()
			: this()
		{
		}//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)

	}
}
