using UnityEditor;
using UnityEngine;

namespace ProBuilder2.EditorCommon
{
	internal class pb_UpdateAvailable : EditorWindow
	{
		[SerializeField]
		private pb_VersionInfo m_NewVersion;

		[SerializeField]
		private string m_NewChangelog;

		private Vector2 scroll = Vector2.get_zero();

		private GUIContent gc_DownloadUpdate = new GUIContent("", "Open Asset Store Download Page");

		private static GUIStyle downloadImageStyle;

		private static GUIStyle updateHeader;

		private bool checkForProBuilderUpdates
		{
			get
			{
				return pb_PreferencesInternal.GetBool("pbCheckForProBuilderUpdates", true);
			}
			set
			{
				pb_PreferencesInternal.SetBool("pbCheckForProBuilderUpdates", value, (pb_PreferenceLocation)0);
			}
		}

		public static void Init(pb_VersionInfo newVersion, string changelog)
		{
			pb_UpdateAvailable window = EditorWindow.GetWindow<pb_UpdateAvailable>(true, "ProBuilder Update Available", true);
			window.m_NewVersion = newVersion;
			window.m_NewChangelog = changelog;
		}

		private void OnEnable()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Expected O, but got Unknown
			//IL_011b: Expected O, but got Unknown
			pb_AboutWindow.InitGuiStyles();
			((EditorWindow)this).set_wantsMouseMove(true);
			((EditorWindow)this).set_minSize(new Vector2(400f, 350f));
			GUIStyle val = new GUIStyle();
			val.set_margin(new RectOffset(10, 10, 10, 10));
			val.set_fixedWidth(154f);
			val.set_fixedHeight(85f);
			GUIStyleState val2 = new GUIStyleState();
			val2.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>($"{pb_AboutWindow.AboutRoot}/Images/DownloadPB_Normal.png"));
			val.set_normal(val2);
			GUIStyleState val3 = new GUIStyleState();
			val3.set_background(pb_AboutWindow.LoadAssetAtPath<Texture2D>($"{pb_AboutWindow.AboutRoot}/Images/DownloadPB_Hover.png"));
			val.set_hover(val3);
			downloadImageStyle = val;
			GUIStyle val4 = new GUIStyle();
			val4.set_margin(new RectOffset(0, 0, 0, 0));
			val4.set_alignment((TextAnchor)4);
			val4.set_fixedHeight(85f);
			val4.set_fontSize(24);
			val4.set_wordWrap(true);
			val4.set_font(pb_AboutWindow.LoadAssetAtPath<Font>(string.Format("{0}/Font/{1}", pb_AboutWindow.AboutRoot, "Asap-Medium.otf")));
			GUIStyleState val5 = new GUIStyleState();
			val5.set_textColor(EditorGUIUtility.get_isProSkin() ? pb_AboutWindow.font_white : pb_AboutWindow.font_black);
			val4.set_normal(val5);
			updateHeader = val4;
		}

		private void OnGUI()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button(gc_DownloadUpdate, downloadImageStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Application.OpenURL("http://u3d.as/30b");
			}
			Rect lastRect = GUILayoutUtility.GetLastRect();
			if (((Rect)(ref lastRect)).Contains(Event.get_current().get_mousePosition()))
			{
				((EditorWindow)this).Repaint();
			}
			GUILayout.BeginVertical(pb_AboutWindow.changelogStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("ProBuilder Update\nAvailable", updateHeader, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			scroll = EditorGUILayout.BeginScrollView(scroll, pb_AboutWindow.changelogStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label($"Version: {m_NewVersion.text}", pb_AboutWindow.versionInfoStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("\n" + m_NewChangelog, pb_AboutWindow.changelogTextStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndScrollView();
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			checkForProBuilderUpdates = EditorGUILayout.Toggle("Show Update Notifications", checkForProBuilderUpdates, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(4f);
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
		}

		public pb_UpdateAvailable()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown

	}
}
