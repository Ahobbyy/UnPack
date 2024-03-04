using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

public class pb_MenuItems : EditorWindow
{
	private const string DOCUMENTATION_URL = "http://procore3d.github.io/probuilder2/";

	private static pb_Editor editor => pb_Editor.get_instance();

	private static pb_Object[] selection => pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms());

	[MenuItem("Tools/ProBuilder/About", false, 0)]
	public static void MenuInitAbout()
	{
		pb_AboutWindow.Init(fromMenu: true);
	}

	[MenuItem("Tools/ProBuilder/Documentation", false, 0)]
	public static void MenuInitDocumentation()
	{
		Application.OpenURL("http://procore3d.github.io/probuilder2/");
	}

	[MenuItem("Tools/ProBuilder/ProBuilder Window", false, 100)]
	public static void OpenEditorWindow()
	{
		pb_Editor.MenuOpenWindow();
	}

	[MenuItem("Tools/ProBuilder/Geometry/Extrude %e", true)]
	private static bool MenuVerifyExtrude()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		pb_Editor instance = pb_Editor.get_instance();
		if ((Object)(object)instance != (Object)null && (int)instance.get_editLevel() == 1 && selection != null && selection.Length != 0)
		{
			if (!selection.Any((pb_Object x) => x.get_SelectedEdgeCount() > 0))
			{
				return selection.Any((pb_Object x) => x.get_SelectedFaces().Length != 0);
			}
			return true;
		}
		return false;
	}

	[MenuItem("Tools/ProBuilder/Geometry/Extrude %e", false, 203)]
	private static void MenuDoExtrude()
	{
		pb_Menu_Commands.MenuExtrude(selection, false);
	}

	[MenuItem("Tools/ProBuilder/Selection/Select Loop &l", true, 200)]
	[MenuItem("Tools/ProBuilder/Selection/Select Ring &r", true, 200)]
	private static bool MenuVerifyRingLoop()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Invalid comparison between Unknown and I4
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Invalid comparison between Unknown and I4
		if ((Object)(object)editor == (Object)null || (int)editor.get_editLevel() != 1)
		{
			return false;
		}
		if ((int)editor.get_selectionMode() == 1)
		{
			return pb_Selection.Top().Any((pb_Object x) => x.get_SelectedEdgeCount() > 0);
		}
		if ((int)editor.get_selectionMode() == 2)
		{
			return pb_Selection.Top().Any((pb_Object x) => x.get_SelectedFaceCount() > 0);
		}
		return false;
	}

	[MenuItem("Tools/ProBuilder/Selection/Select Loop &l", false, 200)]
	private static void MenuSelectLoop()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		SelectMode selectionMode = editor.get_selectionMode();
		if ((int)selectionMode != 1)
		{
			if ((int)selectionMode == 2)
			{
				pb_Menu_Commands.MenuLoopFaces(selection);
			}
		}
		else
		{
			pb_Menu_Commands.MenuLoopSelection(selection);
		}
	}

	[MenuItem("Tools/ProBuilder/Selection/Select Ring &r", false, 200)]
	private static void MenuSelectRing()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Invalid comparison between Unknown and I4
		SelectMode selectionMode = editor.get_selectionMode();
		if ((int)selectionMode != 1)
		{
			if ((int)selectionMode == 2)
			{
				pb_Menu_Commands.MenuRingFaces(selection);
			}
		}
		else
		{
			pb_Menu_Commands.MenuRingSelection(selection);
		}
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 1 &#1", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 2 &#2", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 3 &#3", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 4 &#4", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 5 &#5", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 6 &#6", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 7 &#7", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 8 &#8", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 9 &#9", true, 400)]
	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 0 &#0", true, 400)]
	public static bool VerifyApplyVertexColor()
	{
		if ((Object)(object)pb_Editor.get_instance() != (Object)null)
		{
			return pb_Editor.get_instance().get_selectedVertexCount() > 0;
		}
		return false;
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 1 &#1", false, 400)]
	public static void MenuSetVertexColorPreset1()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(1);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 2 &#2", false, 400)]
	public static void MenuSetVertexColorPreset2()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(2);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 3 &#3", false, 400)]
	public static void MenuSetVertexColorPreset3()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(3);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 4 &#4", false, 400)]
	public static void MenuSetVertexColorPreset4()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(4);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 5 &#5", false, 400)]
	public static void MenuSetVertexColorPreset5()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(5);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 6 &#6", false, 400)]
	public static void MenuSetVertexColorPreset6()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(6);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 7 &#7", false, 400)]
	public static void MenuSetVertexColorPreset7()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(7);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 8 &#8", false, 400)]
	public static void MenuSetVertexColorPreset8()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(8);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 9 &#9", false, 400)]
	public static void MenuSetVertexColorPreset9()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(9);
	}

	[MenuItem("Tools/ProBuilder/Vertex Colors/Set Selected Faces to Preset 0 &#0", false, 400)]
	public static void MenuSetVertexColorPreset0()
	{
		pb_Vertex_Color_Toolbar.SetFaceColors(0);
	}

	public pb_MenuItems()
		: this()
	{
	}
}
