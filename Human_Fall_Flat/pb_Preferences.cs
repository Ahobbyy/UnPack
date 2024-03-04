using System;
using System.Linq;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class pb_Preferences : Editor
{
	private static bool prefsLoaded = false;

	private static Color pbDefaultFaceColor;

	private static Color pbDefaultEdgeColor;

	private static Color pbDefaultSelectedVertexColor;

	private static Color pbDefaultVertexColor;

	private static bool defaultOpenInDockableWindow;

	private static Material pbDefaultMaterial;

	private static Vector2 settingsScroll = Vector2.get_zero();

	private static bool pbShowEditorNotifications;

	private static bool pbForceConvex = false;

	private static bool pbDragCheckLimit = false;

	private static bool pbForceVertexPivot = true;

	private static bool pbForceGridPivot = true;

	private static bool pbPerimeterEdgeBridgeOnly;

	private static bool pbPBOSelectionOnly;

	private static bool pbCloseShapeWindow = false;

	private static bool pbUVEditorFloating = true;

	private static bool pbStripProBuilderOnBuild = true;

	private static bool pbDisableAutoUV2Generation = false;

	private static bool pbShowSceneInfo = false;

	private static bool pbUniqueModeShortcuts = false;

	private static bool pbIconGUI = false;

	private static bool pbShiftOnlyTooltips = false;

	private static bool pbDrawAxisLines = true;

	private static bool pbMeshesAreAssets = false;

	private static bool pbElementSelectIsHamFisted = false;

	private static bool pbDragSelectWholeElement = false;

	private static bool pbEnableExperimental = false;

	private static bool showMissingLightmapUvWarning = false;

	private static ShadowCastingMode pbShadowCastingMode = (ShadowCastingMode)1;

	private static ColliderType defaultColliderType = (ColliderType)1;

	private static SceneToolbarLocation pbToolbarLocation = (SceneToolbarLocation)0;

	private static EntityType pbDefaultEntity = (EntityType)0;

	private static float pbUVGridSnapValue;

	private static float pbVertexHandleSize;

	private static pb_Shortcut[] defaultShortcuts;

	private static int shortcutIndex = 0;

	private static Rect selectBox = new Rect(0f, 214f, 183f, 156f);

	private static Rect shortcutEditRect = new Rect(190f, 191f, 178f, 300f);

	private static Vector2 shortcutScroll = Vector2.get_zero();

	private static int CELL_HEIGHT = 20;

	[PreferenceItem("ProBuilder")]
	private static void PreferencesGUI()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Expected O, but got Unknown
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Invalid comparison between Unknown and I4
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Expected O, but got Unknown
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Expected O, but got Unknown
		if (!prefsLoaded)
		{
			LoadPrefs();
			prefsLoaded = true;
		}
		settingsScroll = EditorGUILayout.BeginScrollView(settingsScroll, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MinHeight(180f),
			GUILayout.MaxHeight(180f)
		});
		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button("Reset All Preferences", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			ResetToDefaults();
		}
		GUILayout.Label("General Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbStripProBuilderOnBuild = EditorGUILayout.Toggle(new GUIContent("Strip PB Scripts on Build", "If true, when building an executable all ProBuilder scripts will be stripped from your built product."), pbStripProBuilderOnBuild, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDisableAutoUV2Generation = EditorGUILayout.Toggle(new GUIContent("Disable Auto UV2 Generation", "Disables automatic generation of UV2 channel.  If Unity is sluggish when working with large ProBuilder objects, disabling UV2 generation will improve performance.  Use `Actions/Generate UV2` or `Actions/Generate Scene UV2` to build lightmap UVs prior to baking."), pbDisableAutoUV2Generation, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbShowSceneInfo = EditorGUILayout.Toggle(new GUIContent("Show Scene Info", "Displays the selected object vertex and triangle counts in the scene view."), pbShowSceneInfo, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbShowEditorNotifications = EditorGUILayout.Toggle("Show Editor Notifications", pbShowEditorNotifications, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Toolbar Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbIconGUI = EditorGUILayout.Toggle(new GUIContent("Use Icon GUI", "Toggles the ProBuilder window interface between text and icon versions."), pbIconGUI, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbShiftOnlyTooltips = EditorGUILayout.Toggle(new GUIContent("Shift Key Tooltips", "Tooltips will only show when the Shift key is held"), pbShiftOnlyTooltips, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbToolbarLocation = (SceneToolbarLocation)(object)EditorGUILayout.EnumPopup("Toolbar Location", (Enum)(object)pbToolbarLocation, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbUniqueModeShortcuts = EditorGUILayout.Toggle(new GUIContent("Unique Mode Shortcuts", "When off, the G key toggles between Object and Element modes and H enumerates the element modes.  If on, G, H, J, and K are shortcuts to Object, Vertex, Edge, and Face modes respectively."), pbUniqueModeShortcuts, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		defaultOpenInDockableWindow = EditorGUILayout.Toggle("Open in Dockable Window", defaultOpenInDockableWindow, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Defaults", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDefaultMaterial = (Material)EditorGUILayout.ObjectField("Default Material", (Object)(object)pbDefaultMaterial, typeof(Material), false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PrefixLabel("Default Entity");
		pbDefaultEntity = (EntityType)(object)EditorGUILayout.EnumPopup((Enum)(object)pbDefaultEntity, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PrefixLabel("Default Collider");
		defaultColliderType = (ColliderType)(object)EditorGUILayout.EnumPopup((Enum)(object)defaultColliderType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		if ((int)defaultColliderType == 2)
		{
			pbForceConvex = EditorGUILayout.Toggle("Force Convex Mesh Collider", pbForceConvex, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PrefixLabel("Shadow Casting Mode");
		pbShadowCastingMode = (ShadowCastingMode)(object)EditorGUILayout.EnumPopup((Enum)(object)pbShadowCastingMode, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		GUILayout.Label("Misc. Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDragCheckLimit = EditorGUILayout.Toggle(new GUIContent("Limit Drag Check to Selection", "If true, when drag selecting faces, only currently selected pb-Objects will be tested for matching faces.  If false, all pb_Objects in the scene will be checked.  The latter may be slower in large scenes."), pbDragCheckLimit, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbPBOSelectionOnly = EditorGUILayout.Toggle(new GUIContent("Only PBO are Selectable", "If true, you will not be able to select non probuilder objects in Geometry and Texture mode"), pbPBOSelectionOnly, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbCloseShapeWindow = EditorGUILayout.Toggle(new GUIContent("Close shape window after building", "If true the shape window will close after hitting the build button"), pbCloseShapeWindow, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDrawAxisLines = EditorGUILayout.Toggle(new GUIContent("Dimension Overlay Lines", "When the Dimensions Overlay is on, this toggle shows or hides the axis lines."), pbDrawAxisLines, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		showMissingLightmapUvWarning = EditorGUILayout.Toggle("Show Missing Lightmap UVs Warning", showMissingLightmapUvWarning, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(4f);
		GUILayout.Label("Geometry Editing Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbElementSelectIsHamFisted = !EditorGUILayout.Toggle(new GUIContent("Precise Element Selection", "When enabled you will be able to select object faces when in Vertex of Edge mode by clicking the center of a face.  When disabled, edge and vertex selection will always be restricted to the nearest element."), !pbElementSelectIsHamFisted, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDragSelectWholeElement = EditorGUILayout.Toggle("Precise Drag Select", pbDragSelectWholeElement, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDefaultFaceColor = EditorGUILayout.ColorField("Selected Face Color", pbDefaultFaceColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDefaultEdgeColor = EditorGUILayout.ColorField("Edge Wireframe Color", pbDefaultEdgeColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDefaultVertexColor = EditorGUILayout.ColorField("Vertex Color", pbDefaultVertexColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbDefaultSelectedVertexColor = EditorGUILayout.ColorField("Selected Vertex Color", pbDefaultSelectedVertexColor, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbVertexHandleSize = EditorGUILayout.Slider("Vertex Handle Size", pbVertexHandleSize, 0f, 3f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbForceVertexPivot = EditorGUILayout.Toggle(new GUIContent("Force Pivot to Vertex Point", "If true, new objects will automatically have their pivot point set to a vertex instead of the center."), pbForceVertexPivot, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbForceGridPivot = EditorGUILayout.Toggle(new GUIContent("Force Pivot to Grid", "If true, newly instantiated pb_Objects will be snapped to the nearest point on grid.  If ProGrids is present, the snap value will be used, otherwise decimals are simply rounded to whole numbers."), pbForceGridPivot, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbPerimeterEdgeBridgeOnly = EditorGUILayout.Toggle(new GUIContent("Bridge Perimeter Edges Only", "If true, only edges on the perimeters of an object may be bridged.  If false, you may bridge any between any two edges you like."), pbPerimeterEdgeBridgeOnly, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(4f);
		GUILayout.Label("Experimental", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbEnableExperimental = EditorGUILayout.Toggle(new GUIContent("Experimental Features", "Enables some experimental new features that we're trying out.  These may be incomplete or buggy, so please exercise caution when making use of this functionality!"), pbEnableExperimental, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbMeshesAreAssets = EditorGUILayout.Toggle(new GUIContent("Meshes Are Assets", "Experimental!  Instead of storing mesh data in the scene, this toggle creates a Mesh cache in the Project that ProBuilder will use."), pbMeshesAreAssets, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(4f);
		GUILayout.Label("UV Editing Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbUVGridSnapValue = EditorGUILayout.FloatField("UV Snap Increment", pbUVGridSnapValue, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		pbUVGridSnapValue = Mathf.Clamp(pbUVGridSnapValue, 0.015625f, 2f);
		pbUVEditorFloating = EditorGUILayout.Toggle(new GUIContent("Editor window floating", "If true UV   Editor window will open as a floating window"), pbUVEditorFloating, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.EndScrollView();
		GUILayout.Space(4f);
		GUILayout.Label("Shortcut Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		ShortcutSelectPanel();
		ShortcutEditPanel();
		if (EditorGUI.EndChangeCheck())
		{
			SetPrefs();
		}
	}

	public static void ResetToDefaults()
	{
		if (EditorUtility.DisplayDialog("Delete ProBuilder editor preferences?", "Are you sure you want to delete all existing ProBuilder preferences?\n\nThis action cannot be undone.", "Yes", "No"))
		{
			pb_PreferencesInternal.DeleteKey("pbDefaultFaceColor");
			pb_PreferencesInternal.DeleteKey("pbDefaultEditLevel");
			pb_PreferencesInternal.DeleteKey("pbDefaultSelectionMode");
			pb_PreferencesInternal.DeleteKey("pbHandleAlignment");
			pb_PreferencesInternal.DeleteKey("pbVertexColorTool");
			pb_PreferencesInternal.DeleteKey("pbToolbarLocation");
			pb_PreferencesInternal.DeleteKey("pbDefaultEntity");
			pb_PreferencesInternal.DeleteKey("pbDefaultFaceColor");
			pb_PreferencesInternal.DeleteKey("pbDefaultEdgeColor");
			pb_PreferencesInternal.DeleteKey("pbDefaultSelectedVertexColor");
			pb_PreferencesInternal.DeleteKey("pbDefaultVertexColor");
			pb_PreferencesInternal.DeleteKey("pbDefaultOpenInDockableWindow");
			pb_PreferencesInternal.DeleteKey("pbEditorPrefVersion");
			pb_PreferencesInternal.DeleteKey("pbEditorShortcutsVersion");
			pb_PreferencesInternal.DeleteKey("pbDefaultCollider");
			pb_PreferencesInternal.DeleteKey("pbForceConvex");
			pb_PreferencesInternal.DeleteKey("pbVertexColorPrefs");
			pb_PreferencesInternal.DeleteKey("pbShowEditorNotifications");
			pb_PreferencesInternal.DeleteKey("pbDragCheckLimit");
			pb_PreferencesInternal.DeleteKey("pbForceVertexPivot");
			pb_PreferencesInternal.DeleteKey("pbForceGridPivot");
			pb_PreferencesInternal.DeleteKey("pbManifoldEdgeExtrusion");
			pb_PreferencesInternal.DeleteKey("pbPerimeterEdgeBridgeOnly");
			pb_PreferencesInternal.DeleteKey("pbPBOSelectionOnly");
			pb_PreferencesInternal.DeleteKey("pbCloseShapeWindow");
			pb_PreferencesInternal.DeleteKey("pbUVEditorFloating");
			pb_PreferencesInternal.DeleteKey("pbUVMaterialPreview");
			pb_PreferencesInternal.DeleteKey("pbShowSceneToolbar");
			pb_PreferencesInternal.DeleteKey("pbNormalizeUVsOnPlanarProjection");
			pb_PreferencesInternal.DeleteKey("pbStripProBuilderOnBuild");
			pb_PreferencesInternal.DeleteKey("pbDisableAutoUV2Generation");
			pb_PreferencesInternal.DeleteKey("pbShowSceneInfo");
			pb_PreferencesInternal.DeleteKey("pbEnableBackfaceSelection");
			pb_PreferencesInternal.DeleteKey("pbVertexPaletteDockable");
			pb_PreferencesInternal.DeleteKey("pbExtrudeAsGroup");
			pb_PreferencesInternal.DeleteKey("pbUniqueModeShortcuts");
			pb_PreferencesInternal.DeleteKey("pbMaterialEditorFloating");
			pb_PreferencesInternal.DeleteKey("pbShapeWindowFloating");
			pb_PreferencesInternal.DeleteKey("pbIconGUI");
			pb_PreferencesInternal.DeleteKey("pbShiftOnlyTooltips");
			pb_PreferencesInternal.DeleteKey("pbDrawAxisLines");
			pb_PreferencesInternal.DeleteKey("pbCollapseVertexToFirst");
			pb_PreferencesInternal.DeleteKey("pbMeshesAreAssets");
			pb_PreferencesInternal.DeleteKey("pbElementSelectIsHamFisted");
			pb_PreferencesInternal.DeleteKey("pbDragSelectWholeElement");
			pb_PreferencesInternal.DeleteKey("pbEnableExperimental");
			pb_PreferencesInternal.DeleteKey("pbFillHoleSelectsEntirePath");
			pb_PreferencesInternal.DeleteKey("pbDetachToNewObject");
			pb_PreferencesInternal.DeleteKey("pbPreserveFaces");
			pb_PreferencesInternal.DeleteKey("pbVertexHandleSize");
			pb_PreferencesInternal.DeleteKey("pbUVGridSnapValue");
			pb_PreferencesInternal.DeleteKey("pbUVWeldDistance");
			pb_PreferencesInternal.DeleteKey("pbWeldDistance");
			pb_PreferencesInternal.DeleteKey("pbExtrudeDistance");
			pb_PreferencesInternal.DeleteKey("pbBevelAmount");
			pb_PreferencesInternal.DeleteKey("pbEdgeSubdivisions");
			pb_PreferencesInternal.DeleteKey("pbDefaultShortcuts");
			pb_PreferencesInternal.DeleteKey("pbDefaultMaterial");
			pb_PreferencesInternal.DeleteKey("pbGrowSelectionUsingAngle");
			pb_PreferencesInternal.DeleteKey("pbGrowSelectionAngle");
			pb_PreferencesInternal.DeleteKey("pbGrowSelectionAngleIterative");
			pb_PreferencesInternal.DeleteKey("pbShowDetail");
			pb_PreferencesInternal.DeleteKey("pbShowOccluder");
			pb_PreferencesInternal.DeleteKey("pbShowMover");
			pb_PreferencesInternal.DeleteKey("pbShowCollider");
			pb_PreferencesInternal.DeleteKey("pbShowTrigger");
			pb_PreferencesInternal.DeleteKey("pbShowNoDraw");
			pb_PreferencesInternal.DeleteKey("pb_Lightmapping::showMissingLightmapUvWarning");
			pb_PreferencesInternal.DeleteKey("pbShadowCastingMode");
		}
		LoadPrefs();
	}

	private static void ShortcutSelectPanel()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.Space(4f);
		GUI.set_contentColor(Color.get_white());
		GUI.Box(selectBox, "");
		GUIStyle none = GUIStyle.get_none();
		if (EditorGUIUtility.get_isProSkin())
		{
			none.get_normal().set_textColor(new Color(1f, 1f, 1f, 0.8f));
		}
		none.set_alignment((TextAnchor)3);
		none.set_contentOffset(new Vector2(4f, 0f));
		shortcutScroll = EditorGUILayout.BeginScrollView(shortcutScroll, false, true, (GUILayoutOption[])(object)new GUILayoutOption[2]
		{
			GUILayout.MaxWidth(183f),
			GUILayout.MaxHeight(156f)
		});
		for (int i = 1; i < defaultShortcuts.Length; i++)
		{
			if (i == shortcutIndex)
			{
				GUI.set_backgroundColor(new Color(0.23f, 0.49f, 0.89f, 1f));
				none.get_normal().set_background(EditorGUIUtility.get_whiteTexture());
				Color textColor = none.get_normal().get_textColor();
				none.get_normal().set_textColor(Color.get_white());
				GUILayout.Box(defaultShortcuts[i].action, none, (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.MinHeight((float)CELL_HEIGHT),
					GUILayout.MaxHeight((float)CELL_HEIGHT)
				});
				none.get_normal().set_background((Texture2D)null);
				none.get_normal().set_textColor(textColor);
				GUI.set_backgroundColor(Color.get_white());
			}
			else if (GUILayout.Button(defaultShortcuts[i].action, none, (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.MinHeight((float)CELL_HEIGHT),
				GUILayout.MaxHeight((float)CELL_HEIGHT)
			}))
			{
				shortcutIndex = i;
			}
		}
		EditorGUILayout.EndScrollView();
	}

	private static void ShortcutEditPanel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginArea(shortcutEditRect);
		GUILayout.Label("Key", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		KeyCode key = defaultShortcuts[shortcutIndex].key;
		key = (KeyCode)(object)EditorGUILayout.EnumPopup((Enum)(object)key, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		defaultShortcuts[shortcutIndex].key = key;
		GUILayout.Label("Modifiers", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EventModifiers eventModifiers = defaultShortcuts[shortcutIndex].eventModifiers;
		defaultShortcuts[shortcutIndex].eventModifiers = (EventModifiers)(object)EditorGUILayout.EnumFlagsField((Enum)(object)eventModifiers, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Description", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label(defaultShortcuts[shortcutIndex].description, EditorStyles.get_wordWrappedLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndArea();
	}

	private static void LoadPrefs()
	{
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		pbStripProBuilderOnBuild = pb_PreferencesInternal.GetBool("pbStripProBuilderOnBuild");
		pbDisableAutoUV2Generation = pb_PreferencesInternal.GetBool("pbDisableAutoUV2Generation");
		pbShowSceneInfo = pb_PreferencesInternal.GetBool("pbShowSceneInfo");
		defaultOpenInDockableWindow = pb_PreferencesInternal.GetBool("pbDefaultOpenInDockableWindow");
		pbDragCheckLimit = pb_PreferencesInternal.GetBool("pbDragCheckLimit");
		pbForceConvex = pb_PreferencesInternal.GetBool("pbForceConvex");
		pbForceGridPivot = pb_PreferencesInternal.GetBool("pbForceGridPivot");
		pbForceVertexPivot = pb_PreferencesInternal.GetBool("pbForceVertexPivot");
		pbPerimeterEdgeBridgeOnly = pb_PreferencesInternal.GetBool("pbPerimeterEdgeBridgeOnly");
		pbPBOSelectionOnly = pb_PreferencesInternal.GetBool("pbPBOSelectionOnly");
		pbCloseShapeWindow = pb_PreferencesInternal.GetBool("pbCloseShapeWindow");
		pbUVEditorFloating = pb_PreferencesInternal.GetBool("pbUVEditorFloating");
		pbShowEditorNotifications = pb_PreferencesInternal.GetBool("pbShowEditorNotifications");
		pbUniqueModeShortcuts = pb_PreferencesInternal.GetBool("pbUniqueModeShortcuts");
		pbIconGUI = pb_PreferencesInternal.GetBool("pbIconGUI");
		pbShiftOnlyTooltips = pb_PreferencesInternal.GetBool("pbShiftOnlyTooltips");
		pbDrawAxisLines = pb_PreferencesInternal.GetBool("pbDrawAxisLines");
		pbMeshesAreAssets = pb_PreferencesInternal.GetBool("pbMeshesAreAssets");
		pbElementSelectIsHamFisted = pb_PreferencesInternal.GetBool("pbElementSelectIsHamFisted");
		pbDragSelectWholeElement = pb_PreferencesInternal.GetBool("pbDragSelectWholeElement");
		pbEnableExperimental = pb_PreferencesInternal.GetBool("pbEnableExperimental");
		showMissingLightmapUvWarning = pb_PreferencesInternal.GetBool("pb_Lightmapping::showMissingLightmapUvWarning", false);
		pbDefaultFaceColor = pb_PreferencesInternal.GetColor("pbDefaultFaceColor");
		pbDefaultEdgeColor = pb_PreferencesInternal.GetColor("pbDefaultEdgeColor");
		pbDefaultSelectedVertexColor = pb_PreferencesInternal.GetColor("pbDefaultSelectedVertexColor");
		pbDefaultVertexColor = pb_PreferencesInternal.GetColor("pbDefaultVertexColor");
		pbUVGridSnapValue = pb_PreferencesInternal.GetFloat("pbUVGridSnapValue");
		pbVertexHandleSize = pb_PreferencesInternal.GetFloat("pbVertexHandleSize");
		defaultColliderType = pb_PreferencesInternal.GetEnum<ColliderType>("pbDefaultCollider");
		pbToolbarLocation = pb_PreferencesInternal.GetEnum<SceneToolbarLocation>("pbToolbarLocation");
		pbDefaultEntity = pb_PreferencesInternal.GetEnum<EntityType>("pbDefaultEntity");
		pbShadowCastingMode = pb_PreferencesInternal.GetEnum<ShadowCastingMode>("pbShadowCastingMode");
		pbDefaultMaterial = pb_PreferencesInternal.GetMaterial("pbDefaultMaterial");
		defaultShortcuts = pb_PreferencesInternal.GetShortcuts().ToArray();
	}

	public static void SetPrefs()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected I4, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected I4, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Expected I4, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected I4, but got Unknown
		pb_PreferencesInternal.SetBool("pbStripProBuilderOnBuild", pbStripProBuilderOnBuild, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetBool("pbDisableAutoUV2Generation", pbDisableAutoUV2Generation, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetBool("pbShowSceneInfo", pbShowSceneInfo, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetInt("pbToolbarLocation", (int)pbToolbarLocation, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetInt("pbDefaultEntity", (int)pbDefaultEntity, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetColor("pbDefaultFaceColor", pbDefaultFaceColor, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetColor("pbDefaultEdgeColor", pbDefaultEdgeColor, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetColor("pbDefaultSelectedVertexColor", pbDefaultSelectedVertexColor, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetColor("pbDefaultVertexColor", pbDefaultVertexColor, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetString("pbDefaultShortcuts", pb_Shortcut.ShortcutsToString(defaultShortcuts), (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetMaterial("pbDefaultMaterial", pbDefaultMaterial, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetInt("pbDefaultCollider", (int)defaultColliderType, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetInt("pbShadowCastingMode", (int)pbShadowCastingMode, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetBool("pbDefaultOpenInDockableWindow", defaultOpenInDockableWindow, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbShowEditorNotifications", pbShowEditorNotifications, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbForceConvex", pbForceConvex, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetBool("pbDragCheckLimit", pbDragCheckLimit, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbForceVertexPivot", pbForceVertexPivot, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbForceGridPivot", pbForceGridPivot, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbPerimeterEdgeBridgeOnly", pbPerimeterEdgeBridgeOnly, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbPBOSelectionOnly", pbPBOSelectionOnly, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbCloseShapeWindow", pbCloseShapeWindow, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbUVEditorFloating", pbUVEditorFloating, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbUniqueModeShortcuts", pbUniqueModeShortcuts, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbIconGUI", pbIconGUI, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbShiftOnlyTooltips", pbShiftOnlyTooltips, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbDrawAxisLines", pbDrawAxisLines, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbMeshesAreAssets", pbMeshesAreAssets, (pb_PreferenceLocation)0);
		pb_PreferencesInternal.SetBool("pbElementSelectIsHamFisted", pbElementSelectIsHamFisted, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbDragSelectWholeElement", pbDragSelectWholeElement, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pbEnableExperimental", pbEnableExperimental, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetBool("pb_Lightmapping::showMissingLightmapUvWarning", showMissingLightmapUvWarning, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetFloat("pbVertexHandleSize", pbVertexHandleSize, (pb_PreferenceLocation)1);
		pb_PreferencesInternal.SetFloat("pbUVGridSnapValue", pbUVGridSnapValue, (pb_PreferenceLocation)1);
		if ((Object)(object)pb_Editor.get_instance() != (Object)null)
		{
			pb_Editor.get_instance().OnEnable();
		}
		SceneView.RepaintAll();
	}

	public pb_Preferences()
		: this()
	{
	}
}
