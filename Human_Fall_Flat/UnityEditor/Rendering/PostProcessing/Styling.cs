using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public static class Styling
	{
		public static readonly GUIStyle smallTickbox;

		public static readonly GUIStyle miniLabelButton;

		private static readonly Color splitterDark;

		private static readonly Color splitterLight;

		private static readonly Texture2D paneOptionsIconDark;

		private static readonly Texture2D paneOptionsIconLight;

		public static readonly GUIStyle headerLabel;

		private static readonly Color headerBackgroundDark;

		private static readonly Color headerBackgroundLight;

		public static readonly GUIStyle wheelLabel;

		public static readonly GUIStyle wheelThumb;

		public static readonly Vector2 wheelThumbSize;

		public static readonly GUIStyle preLabel;

		public static Color splitter
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (!EditorGUIUtility.get_isProSkin())
				{
					return splitterLight;
				}
				return splitterDark;
			}
		}

		public static Texture2D paneOptionsIcon
		{
			get
			{
				if (!EditorGUIUtility.get_isProSkin())
				{
					return paneOptionsIconLight;
				}
				return paneOptionsIconDark;
			}
		}

		public static Color headerBackground
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				if (!EditorGUIUtility.get_isProSkin())
				{
					return headerBackgroundLight;
				}
				return headerBackgroundDark;
			}
		}

		static Styling()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Expected O, but got Unknown
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Expected O, but got Unknown
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Expected O, but got Unknown
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Expected O, but got Unknown
			smallTickbox = new GUIStyle(GUIStyle.op_Implicit("ShurikenToggle"));
			miniLabelButton = new GUIStyle(EditorStyles.get_miniLabel());
			GUIStyle obj = miniLabelButton;
			GUIStyleState val = new GUIStyleState();
			val.set_background(RuntimeUtilities.transparentTexture);
			val.set_scaledBackgrounds((Texture2D[])null);
			val.set_textColor(Color.get_grey());
			obj.set_normal(val);
			GUIStyleState val2 = new GUIStyleState();
			val2.set_background(RuntimeUtilities.transparentTexture);
			val2.set_scaledBackgrounds((Texture2D[])null);
			val2.set_textColor(Color.get_white());
			GUIStyleState val3 = val2;
			miniLabelButton.set_active(val3);
			miniLabelButton.set_onNormal(val3);
			miniLabelButton.set_onActive(val3);
			splitterDark = new Color(0.12f, 0.12f, 0.12f, 1.333f);
			splitterLight = new Color(0.6f, 0.6f, 0.6f, 1.333f);
			headerBackgroundDark = new Color(0.1f, 0.1f, 0.1f, 0.2f);
			headerBackgroundLight = new Color(1f, 1f, 1f, 0.2f);
			paneOptionsIconDark = (Texture2D)EditorGUIUtility.Load("Builtin Skins/DarkSkin/Images/pane options.png");
			paneOptionsIconLight = (Texture2D)EditorGUIUtility.Load("Builtin Skins/LightSkin/Images/pane options.png");
			headerLabel = new GUIStyle(EditorStyles.get_miniLabel());
			wheelThumb = new GUIStyle(GUIStyle.op_Implicit("ColorPicker2DThumb"));
			wheelThumbSize = new Vector2((!Mathf.Approximately(wheelThumb.get_fixedWidth(), 0f)) ? wheelThumb.get_fixedWidth() : ((float)wheelThumb.get_padding().get_horizontal()), (!Mathf.Approximately(wheelThumb.get_fixedHeight(), 0f)) ? wheelThumb.get_fixedHeight() : ((float)wheelThumb.get_padding().get_vertical()));
			wheelLabel = new GUIStyle(EditorStyles.get_miniLabel());
			preLabel = new GUIStyle(GUIStyle.op_Implicit("ShurikenLabel"));
		}
	}
}
