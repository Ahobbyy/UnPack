using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class TMP_UIStyleManager
	{
		public static GUISkin TMP_GUISkin;

		public static GUIStyle Label;

		public static GUIStyle Group_Label;

		public static GUIStyle Group_Label_Left;

		public static GUIStyle TextAreaBoxEditor;

		public static GUIStyle TextAreaBoxWindow;

		public static GUIStyle TextureAreaBox;

		public static GUIStyle Section_Label;

		public static GUIStyle SquareAreaBox85G;

		public static Texture2D alignLeft;

		public static Texture2D alignCenter;

		public static Texture2D alignRight;

		public static Texture2D alignJustified;

		public static Texture2D alignFlush;

		public static Texture2D alignGeoCenter;

		public static Texture2D alignTop;

		public static Texture2D alignMiddle;

		public static Texture2D alignBottom;

		public static Texture2D alignBaseline;

		public static Texture2D alignMidline;

		public static Texture2D alignCapline;

		public static Texture2D progressTexture;

		public static Texture2D selectionBox;

		public static GUIContent[] alignContent_A;

		public static GUIContent[] alignContent_B;

		public static void GetUIStyles()
		{
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Expected O, but got Unknown
			//IL_050f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0515: Expected O, but got Unknown
			//IL_0521: Unknown result type (might be due to invalid IL or missing references)
			//IL_0527: Expected O, but got Unknown
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0539: Expected O, but got Unknown
			//IL_0545: Unknown result type (might be due to invalid IL or missing references)
			//IL_054b: Expected O, but got Unknown
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_055d: Expected O, but got Unknown
			//IL_0574: Unknown result type (might be due to invalid IL or missing references)
			//IL_057a: Expected O, but got Unknown
			//IL_0586: Unknown result type (might be due to invalid IL or missing references)
			//IL_058c: Expected O, but got Unknown
			//IL_0598: Unknown result type (might be due to invalid IL or missing references)
			//IL_059e: Expected O, but got Unknown
			//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b0: Expected O, but got Unknown
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c2: Expected O, but got Unknown
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d4: Expected O, but got Unknown
			if (!((Object)(object)TMP_GUISkin != (Object)null))
			{
				string packageRelativePath = TMP_EditorUtility.packageRelativePath;
				if (EditorGUIUtility.get_isProSkin())
				{
					Object obj = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/TMPro_DarkSkin.guiskin", typeof(GUISkin));
					TMP_GUISkin = (GUISkin)(object)((obj is GUISkin) ? obj : null);
					Object obj2 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignLeft.psd", typeof(Texture2D));
					alignLeft = (Texture2D)(object)((obj2 is Texture2D) ? obj2 : null);
					Object obj3 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCenter.psd", typeof(Texture2D));
					alignCenter = (Texture2D)(object)((obj3 is Texture2D) ? obj3 : null);
					Object obj4 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignRight.psd", typeof(Texture2D));
					alignRight = (Texture2D)(object)((obj4 is Texture2D) ? obj4 : null);
					Object obj5 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignJustified.psd", typeof(Texture2D));
					alignJustified = (Texture2D)(object)((obj5 is Texture2D) ? obj5 : null);
					Object obj6 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignFlush.psd", typeof(Texture2D));
					alignFlush = (Texture2D)(object)((obj6 is Texture2D) ? obj6 : null);
					Object obj7 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCenterGeo.psd", typeof(Texture2D));
					alignGeoCenter = (Texture2D)(object)((obj7 is Texture2D) ? obj7 : null);
					Object obj8 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignTop.psd", typeof(Texture2D));
					alignTop = (Texture2D)(object)((obj8 is Texture2D) ? obj8 : null);
					Object obj9 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignMiddle.psd", typeof(Texture2D));
					alignMiddle = (Texture2D)(object)((obj9 is Texture2D) ? obj9 : null);
					Object obj10 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignBottom.psd", typeof(Texture2D));
					alignBottom = (Texture2D)(object)((obj10 is Texture2D) ? obj10 : null);
					Object obj11 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignBaseLine.psd", typeof(Texture2D));
					alignBaseline = (Texture2D)(object)((obj11 is Texture2D) ? obj11 : null);
					Object obj12 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignMidLine.psd", typeof(Texture2D));
					alignMidline = (Texture2D)(object)((obj12 is Texture2D) ? obj12 : null);
					Object obj13 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCapLine.psd", typeof(Texture2D));
					alignCapline = (Texture2D)(object)((obj13 is Texture2D) ? obj13 : null);
					Object obj14 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/Progress Bar.psd", typeof(Texture2D));
					progressTexture = (Texture2D)(object)((obj14 is Texture2D) ? obj14 : null);
					Object obj15 = EditorGUIUtility.Load("IN thumbnailshadow On@2x");
					selectionBox = (Texture2D)(object)((obj15 is Texture2D) ? obj15 : null);
				}
				else
				{
					Object obj16 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/TMPro_LightSkin.guiskin", typeof(GUISkin));
					TMP_GUISkin = (GUISkin)(object)((obj16 is GUISkin) ? obj16 : null);
					Object obj17 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignLeft_Light.psd", typeof(Texture2D));
					alignLeft = (Texture2D)(object)((obj17 is Texture2D) ? obj17 : null);
					Object obj18 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCenter_Light.psd", typeof(Texture2D));
					alignCenter = (Texture2D)(object)((obj18 is Texture2D) ? obj18 : null);
					Object obj19 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignRight_Light.psd", typeof(Texture2D));
					alignRight = (Texture2D)(object)((obj19 is Texture2D) ? obj19 : null);
					Object obj20 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignJustified_Light.psd", typeof(Texture2D));
					alignJustified = (Texture2D)(object)((obj20 is Texture2D) ? obj20 : null);
					Object obj21 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignFlush_Light.psd", typeof(Texture2D));
					alignFlush = (Texture2D)(object)((obj21 is Texture2D) ? obj21 : null);
					Object obj22 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCenterGeo_Light.psd", typeof(Texture2D));
					alignGeoCenter = (Texture2D)(object)((obj22 is Texture2D) ? obj22 : null);
					Object obj23 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignTop_Light.psd", typeof(Texture2D));
					alignTop = (Texture2D)(object)((obj23 is Texture2D) ? obj23 : null);
					Object obj24 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignMiddle_Light.psd", typeof(Texture2D));
					alignMiddle = (Texture2D)(object)((obj24 is Texture2D) ? obj24 : null);
					Object obj25 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignBottom_Light.psd", typeof(Texture2D));
					alignBottom = (Texture2D)(object)((obj25 is Texture2D) ? obj25 : null);
					Object obj26 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignBaseLine_Light.psd", typeof(Texture2D));
					alignBaseline = (Texture2D)(object)((obj26 is Texture2D) ? obj26 : null);
					Object obj27 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignMidLine_Light.psd", typeof(Texture2D));
					alignMidline = (Texture2D)(object)((obj27 is Texture2D) ? obj27 : null);
					Object obj28 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/btn_AlignCapLine_Light.psd", typeof(Texture2D));
					alignCapline = (Texture2D)(object)((obj28 is Texture2D) ? obj28 : null);
					Object obj29 = AssetDatabase.LoadAssetAtPath(packageRelativePath + "/Editor Resources/Textures/Progress Bar (Light).psd", typeof(Texture2D));
					progressTexture = (Texture2D)(object)((obj29 is Texture2D) ? obj29 : null);
					Object obj30 = EditorGUIUtility.Load("IN thumbnailshadow On@2x");
					selectionBox = (Texture2D)(object)((obj30 is Texture2D) ? obj30 : null);
				}
				if ((Object)(object)TMP_GUISkin != (Object)null)
				{
					Label = TMP_GUISkin.FindStyle("Label");
					Section_Label = TMP_GUISkin.FindStyle("Section Label");
					Group_Label = TMP_GUISkin.FindStyle("Group Label");
					Group_Label_Left = TMP_GUISkin.FindStyle("Group Label - Left Half");
					TextAreaBoxEditor = TMP_GUISkin.FindStyle("Text Area Box (Editor)");
					TextAreaBoxWindow = TMP_GUISkin.FindStyle("Text Area Box (Window)");
					TextureAreaBox = TMP_GUISkin.FindStyle("Texture Area Box");
					SquareAreaBox85G = TMP_GUISkin.FindStyle("Square Area Box (85 Grey)");
					alignContent_A = (GUIContent[])(object)new GUIContent[6]
					{
						new GUIContent((Texture)(object)alignLeft, "Left"),
						new GUIContent((Texture)(object)alignCenter, "Center"),
						new GUIContent((Texture)(object)alignRight, "Right"),
						new GUIContent((Texture)(object)alignJustified, "Justified"),
						new GUIContent((Texture)(object)alignFlush, "Flush"),
						new GUIContent((Texture)(object)alignGeoCenter, "Geometry Center")
					};
					alignContent_B = (GUIContent[])(object)new GUIContent[6]
					{
						new GUIContent((Texture)(object)alignTop, "Top"),
						new GUIContent((Texture)(object)alignMiddle, "Middle"),
						new GUIContent((Texture)(object)alignBottom, "Bottom"),
						new GUIContent((Texture)(object)alignBaseline, "Baseline"),
						new GUIContent((Texture)(object)alignMidline, "Midline"),
						new GUIContent((Texture)(object)alignCapline, "Capline")
					};
				}
			}
		}
	}
}
