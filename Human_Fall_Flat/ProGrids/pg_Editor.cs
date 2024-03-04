using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	public class pg_Editor : ScriptableObject, ISerializationCallbackReceiver
	{
		private static pg_Editor _instance;

		private Color oldColor;

		[SerializeField]
		private bool snapEnabled = true;

		[SerializeField]
		private SnapUnit snapUnit;

		private float snapValue = 1f;

		private float t_snapValue = 1f;

		private bool drawGrid = true;

		private bool drawAngles;

		public float angleValue = 45f;

		private bool gridRepaint = true;

		public bool predictiveGrid = true;

		private bool _snapAsGroup = true;

		private bool _scaleSnapEnabled;

		private KeyCode m_IncreaseGridSizeShortcut = (KeyCode)61;

		private KeyCode m_DecreaseGridSizeShortcut = (KeyCode)45;

		private KeyCode m_NudgePerspectiveBackwardShortcut = (KeyCode)91;

		private KeyCode m_NudgePerspectiveForwardShortcut = (KeyCode)93;

		private KeyCode m_NudgePerspectiveResetShortcut = (KeyCode)48;

		private KeyCode m_CyclePerspectiveShortcut = (KeyCode)92;

		private bool lockGrid;

		private Axis renderPlane = Axis.Y;

		private const int VERSION = 22;

		private const int WINDOW_HEIGHT = 240;

		private const int DEFAULT_SNAP_MULTIPLIER = 2048;

		private const int MAX_LINES = 150;

		public static float alphaBump;

		private const int BUTTON_SIZE = 46;

		private Texture2D icon_extendoClose;

		private Texture2D icon_extendoOpen;

		[SerializeField]
		private pg_ToggleContent gc_SnapToGrid = new pg_ToggleContent("Snap", "", "Snaps all selected objects to grid.");

		[SerializeField]
		private pg_ToggleContent gc_GridEnabled = new pg_ToggleContent("Hide", "Show", "Toggles drawing of guide lines on or off.  Note that object snapping is not affected by this setting.");

		[SerializeField]
		private pg_ToggleContent gc_SnapEnabled = new pg_ToggleContent("On", "Off", "Toggles snapping on or off.");

		[SerializeField]
		private pg_ToggleContent gc_LockGrid = new pg_ToggleContent("Lock", "Unlck", "Lock the perspective grid center in place.");

		[SerializeField]
		private pg_ToggleContent gc_AngleEnabled = new pg_ToggleContent("> On", "> Off", "If on, ProGrids will draw angled line guides.  Angle is settable in degrees.");

		[SerializeField]
		private pg_ToggleContent gc_RenderPlaneX = new pg_ToggleContent("X", "X", "Renders a grid on the X plane.");

		[SerializeField]
		private pg_ToggleContent gc_RenderPlaneY = new pg_ToggleContent("Y", "Y", "Renders a grid on the Y plane.");

		[SerializeField]
		private pg_ToggleContent gc_RenderPlaneZ = new pg_ToggleContent("Z", "Z", "Renders a grid on the Z plane.");

		[SerializeField]
		private pg_ToggleContent gc_RenderPerspectiveGrid = new pg_ToggleContent("Full", "Plane", "Renders a 3d grid in perspective mode.");

		[SerializeField]
		private GUIContent gc_ExtendMenu = new GUIContent("", "Show or hide the scene view menu.");

		[SerializeField]
		private GUIContent gc_SnapIncrement = new GUIContent("", "Set the snap increment.");

		public Color gridColorX;

		public Color gridColorY;

		public Color gridColorZ;

		public Color gridColorX_primary;

		public Color gridColorY_primary;

		public Color gridColorZ_primary;

		private GUISkin sixBySevenSkin;

		private GUIStyle gridButtonStyle = new GUIStyle();

		private GUIStyle extendoStyle = new GUIStyle();

		private GUIStyle gridButtonStyleBlank = new GUIStyle();

		private GUIStyle backgroundStyle = new GUIStyle();

		private bool guiInitialized;

		private const int MENU_EXTENDED = 8;

		private const int PAD = 3;

		private Rect r = new Rect(8f, 8f, 42f, 16f);

		private Rect backgroundRect = new Rect(0f, 0f, 0f, 0f);

		private Rect extendoButtonRect = new Rect(0f, 0f, 0f, 0f);

		private bool menuOpen = true;

		private float menuStart = 8f;

		private const float MENU_SPEED = 500f;

		private float deltaTime;

		private float lastTime;

		private const float FADE_SPEED = 2.5f;

		private float backgroundFade = 1f;

		private bool mouseOverMenu;

		private Color menuBackgroundColor = new Color(0f, 0f, 0f, 0.5f);

		private Color extendoNormalColor = new Color(0.9f, 0.9f, 0.9f, 0.7f);

		private Color extendoHoverColor = new Color(0f, 1f, 0.4f, 1f);

		private bool extendoButtonHovering;

		private bool menuIsOrtho;

		private Transform lastTransform;

		private const string AXIS_CONSTRAINT_KEY = "s";

		private const string TEMP_DISABLE_KEY = "d";

		private bool toggleAxisConstraint;

		private bool toggleTempSnap;

		private Vector3 lastPosition = Vector3.get_zero();

		private Vector3 lastScale = Vector3.get_one();

		private Vector3 pivot = Vector3.get_zero();

		private Vector3 lastPivot = Vector3.get_zero();

		private Vector3 camDir = Vector3.get_zero();

		private Vector3 prevCamDir = Vector3.get_zero();

		private float lastDistance;

		public float offset;

		private bool firstMove = true;

		private bool prevOrtho;

		private float planeGridDrawDistance;

		private GameObject go;

		private int PRIMARY_COLOR_INCREMENT = 10;

		private Color previousColor;

		[SerializeField]
		private static List<Action<float>> pushToGridListeners = new List<Action<float>>();

		[SerializeField]
		private static List<Action<bool>> toolbarEventSubscribers = new List<Action<bool>>();

		public static pg_Editor instance
		{
			get
			{
				if ((Object)(object)_instance == (Object)null)
				{
					pg_Editor[] array = Resources.FindObjectsOfTypeAll<pg_Editor>();
					if (array != null && array.Length != 0)
					{
						_instance = array[0];
						for (int i = 1; i < array.Length; i++)
						{
							Object.DestroyImmediate((Object)(object)array[i]);
						}
					}
				}
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		private bool useAxisConstraints
		{
			get
			{
				return EditorPrefs.GetBool("pgUseAxisConstraints");
			}
			set
			{
				EditorPrefs.SetBool("pgUseAxisConstraints", value);
			}
		}

		public bool snapAsGroup
		{
			get
			{
				if (!EditorPrefs.HasKey("pg_SnapAsGroup"))
				{
					return true;
				}
				return EditorPrefs.GetBool("pg_SnapAsGroup");
			}
			set
			{
				_snapAsGroup = value;
				EditorPrefs.SetBool("pg_SnapAsGroup", _snapAsGroup);
			}
		}

		public bool fullGrid { get; private set; }

		public bool ScaleSnapEnabled
		{
			get
			{
				if (!EditorPrefs.HasKey("pg_SnapOnScale"))
				{
					return false;
				}
				return EditorPrefs.GetBool("pg_SnapOnScale");
			}
			set
			{
				_scaleSnapEnabled = value;
				EditorPrefs.SetBool("pg_SnapOnScale", _scaleSnapEnabled);
			}
		}

		private int MENU_HIDDEN
		{
			get
			{
				if (!menuIsOrtho)
				{
					return -173;
				}
				return -192;
			}
		}

		public bool ortho { get; private set; }

		public void LoadPreferences()
		{
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_0347: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039a: Unknown result type (might be due to invalid IL or missing references)
			if ((EditorPrefs.HasKey("pg_Version") ? EditorPrefs.GetInt("pg_Version") : 0) != 22)
			{
				EditorPrefs.SetInt("pg_Version", 22);
				pg_Preferences.ResetPrefs();
			}
			if (EditorPrefs.HasKey("pgSnapEnabled"))
			{
				snapEnabled = EditorPrefs.GetBool("pgSnapEnabled");
			}
			menuOpen = EditorPrefs.GetBool("pgProGridsIsExtended", true);
			SetSnapValue(EditorPrefs.HasKey("pg_GridUnit") ? ((SnapUnit)EditorPrefs.GetInt("pg_GridUnit")) : SnapUnit.Meter, EditorPrefs.HasKey("pgSnapValue") ? EditorPrefs.GetFloat("pgSnapValue") : 1f, EditorPrefs.HasKey("pgSnapMultiplier") ? EditorPrefs.GetInt("pgSnapMultiplier") : 2048);
			m_IncreaseGridSizeShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::IncreaseGridSize") ? EditorPrefs.GetInt("pg_Editor::IncreaseGridSize") : 61);
			m_DecreaseGridSizeShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::DecreaseGridSize") ? EditorPrefs.GetInt("pg_Editor::DecreaseGridSize") : 45);
			m_NudgePerspectiveBackwardShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveBackward") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveBackward") : 91);
			m_NudgePerspectiveForwardShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveForward") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveForward") : 93);
			m_NudgePerspectiveResetShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveReset") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveReset") : 48);
			m_CyclePerspectiveShortcut = (KeyCode)(EditorPrefs.HasKey("pg_Editor::CyclePerspective") ? EditorPrefs.GetInt("pg_Editor::CyclePerspective") : 92);
			lockGrid = EditorPrefs.GetBool("pg_LockGrid");
			if (lockGrid && EditorPrefs.HasKey("pg_LockedGridPivot"))
			{
				string[] array = EditorPrefs.GetString("pg_LockedGridPivot").Replace("(", "").Replace(")", "")
					.Split(',');
				if (float.TryParse(array[0], out var result) && float.TryParse(array[1], out var result2) && float.TryParse(array[2], out var result3))
				{
					pivot.x = result;
					pivot.y = result2;
					pivot.z = result3;
				}
			}
			fullGrid = EditorPrefs.GetBool("pg_PerspGrid");
			renderPlane = (EditorPrefs.HasKey("pg_GridAxis") ? ((Axis)EditorPrefs.GetInt("pg_GridAxis")) : Axis.Y);
			alphaBump = (EditorPrefs.HasKey("pg_alphaBump") ? EditorPrefs.GetFloat("pg_alphaBump") : pg_Preferences.ALPHA_BUMP);
			gridColorX = (EditorPrefs.HasKey("gridColorX") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorX")) : pg_Preferences.GRID_COLOR_X);
			gridColorX_primary = new Color(gridColorX.r, gridColorX.g, gridColorX.b, gridColorX.a + alphaBump);
			gridColorY = (EditorPrefs.HasKey("gridColorY") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorY")) : pg_Preferences.GRID_COLOR_Y);
			gridColorY_primary = new Color(gridColorY.r, gridColorY.g, gridColorY.b, gridColorY.a + alphaBump);
			gridColorZ = (EditorPrefs.HasKey("gridColorZ") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorZ")) : pg_Preferences.GRID_COLOR_Z);
			gridColorZ_primary = new Color(gridColorZ.r, gridColorZ.g, gridColorZ.b, gridColorZ.a + alphaBump);
			drawGrid = (EditorPrefs.HasKey("showgrid") ? EditorPrefs.GetBool("showgrid") : pg_Preferences.SHOW_GRID);
			predictiveGrid = !EditorPrefs.HasKey("pg_PredictiveGrid") || EditorPrefs.GetBool("pg_PredictiveGrid");
			_snapAsGroup = snapAsGroup;
			_scaleSnapEnabled = ScaleSnapEnabled;
		}

		[MenuItem("Tools/ProGrids/About", false, 0)]
		public static void MenuAboutProGrids()
		{
			pg_AboutWindow.Init("Assets/ProCore/ProGrids/About/pc_AboutEntry_ProGrids.txt", fromMenu: true);
		}

		[MenuItem("Tools/ProGrids/ProGrids Window", false, 15)]
		public static void InitProGrids()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Expected O, but got Unknown
			if ((Object)(object)instance == (Object)null)
			{
				EditorPrefs.SetBool("pgProGridsIsEnabled", true);
				instance = ScriptableObject.CreateInstance<pg_Editor>();
				((Object)instance).set_hideFlags((HideFlags)52);
				EditorApplication.delayCall = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.delayCall, (Delegate)new CallbackFunction(instance.Initialize));
			}
			else
			{
				CloseProGrids();
			}
			SceneView.RepaintAll();
		}

		[MenuItem("Tools/ProGrids/Close ProGrids", true, 200)]
		public static bool VerifyCloseProGrids()
		{
			if (!((Object)(object)instance != (Object)null))
			{
				return Resources.FindObjectsOfTypeAll<pg_Editor>().Length != 0;
			}
			return true;
		}

		[MenuItem("Tools/ProGrids/Close ProGrids")]
		public static void CloseProGrids()
		{
			pg_Editor[] array = Resources.FindObjectsOfTypeAll<pg_Editor>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Close();
			}
		}

		[MenuItem("Tools/ProGrids/Cycle SceneView Projection", false, 101)]
		public static void CyclePerspective()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)instance == (Object)null)
			{
				return;
			}
			SceneView lastActiveSceneView = SceneView.get_lastActiveSceneView();
			if (!((Object)(object)lastActiveSceneView == (Object)null))
			{
				int num = EditorPrefs.GetInt("pgLastOrthoToggledRotation");
				switch (num)
				{
				case 0:
					lastActiveSceneView.set_orthographic(true);
					lastActiveSceneView.LookAt(lastActiveSceneView.get_pivot(), Quaternion.Euler(Vector3.get_zero()));
					num++;
					break;
				case 1:
					lastActiveSceneView.set_orthographic(true);
					lastActiveSceneView.LookAt(lastActiveSceneView.get_pivot(), Quaternion.Euler(Vector3.get_up() * -90f));
					num++;
					break;
				case 2:
					lastActiveSceneView.set_orthographic(true);
					lastActiveSceneView.LookAt(lastActiveSceneView.get_pivot(), Quaternion.Euler(Vector3.get_right() * 90f));
					num++;
					break;
				case 3:
					lastActiveSceneView.set_orthographic(false);
					lastActiveSceneView.LookAt(lastActiveSceneView.get_pivot(), new Quaternion(-0.1f, 0.9f, -0.2f, -0.4f));
					num = 0;
					break;
				}
				EditorPrefs.SetInt("pgLastOrthoToggledRotation", num);
			}
		}

		[MenuItem("Tools/ProGrids/Cycle SceneView Projection", true, 101)]
		[MenuItem("Tools/ProGrids/Increase Grid Size", true, 203)]
		[MenuItem("Tools/ProGrids/Decrease Grid Size", true, 202)]
		public static bool VerifyGridSizeAdjustment()
		{
			return (Object)(object)instance != (Object)null;
		}

		[MenuItem("Tools/ProGrids/Decrease Grid Size", false, 202)]
		public static void DecreaseGridSize()
		{
			if (!((Object)(object)instance == (Object)null))
			{
				int num = (EditorPrefs.HasKey("pgSnapMultiplier") ? EditorPrefs.GetInt("pgSnapMultiplier") : 2048);
				float val = (EditorPrefs.HasKey("pgSnapValue") ? EditorPrefs.GetFloat("pgSnapValue") : 1f);
				if (num > 1)
				{
					num /= 2;
				}
				instance.SetSnapValue(instance.snapUnit, val, num);
				SceneView.RepaintAll();
			}
		}

		[MenuItem("Tools/ProGrids/Increase Grid Size", false, 203)]
		public static void IncreaseGridSize()
		{
			if (!((Object)(object)instance == (Object)null))
			{
				int num = (EditorPrefs.HasKey("pgSnapMultiplier") ? EditorPrefs.GetInt("pgSnapMultiplier") : 2048);
				float val = (EditorPrefs.HasKey("pgSnapValue") ? EditorPrefs.GetFloat("pgSnapValue") : 1f);
				if (num < 1073741823)
				{
					num *= 2;
				}
				instance.SetSnapValue(instance.snapUnit, val, num);
				SceneView.RepaintAll();
			}
		}

		[MenuItem("Tools/ProGrids/Nudge Perspective Backward", true, 304)]
		[MenuItem("Tools/ProGrids/Nudge Perspective Forward", true, 305)]
		[MenuItem("Tools/ProGrids/Reset Perspective Nudge", true, 306)]
		public static bool VerifyMenuNudgePerspective()
		{
			if ((Object)(object)instance != (Object)null && !instance.fullGrid && !instance.ortho)
			{
				return instance.lockGrid;
			}
			return false;
		}

		[MenuItem("Tools/ProGrids/Nudge Perspective Backward", false, 304)]
		public static void MenuNudgePerspectiveBackward()
		{
			if (instance.lockGrid)
			{
				instance.offset -= instance.snapValue;
				instance.gridRepaint = true;
				SceneView.RepaintAll();
			}
		}

		[MenuItem("Tools/ProGrids/Nudge Perspective Forward", false, 305)]
		public static void MenuNudgePerspectiveForward()
		{
			if (instance.lockGrid)
			{
				instance.offset += instance.snapValue;
				instance.gridRepaint = true;
				SceneView.RepaintAll();
			}
		}

		[MenuItem("Tools/ProGrids/Reset Perspective Nudge", false, 306)]
		public static void MenuNudgePerspectiveReset()
		{
			if (instance.lockGrid)
			{
				instance.offset = 0f;
				instance.gridRepaint = true;
				SceneView.RepaintAll();
			}
		}

		public static void ForceRepaint()
		{
			if ((Object)(object)instance != (Object)null)
			{
				instance.gridRepaint = true;
				SceneView.RepaintAll();
			}
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Expected O, but got Unknown
			instance = this;
			SceneView.onSceneGUIDelegate = (OnSceneFunc)Delegate.Combine((Delegate)(object)SceneView.onSceneGUIDelegate, (Delegate)new OnSceneFunc(OnSceneGUI));
			EditorApplication.update = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
			EditorApplication.hierarchyWindowChanged = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.hierarchyWindowChanged, (Delegate)new CallbackFunction(HierarchyWindowChanged));
		}

		private void OnEnable()
		{
			instance.LoadGUIResources();
			Selection.selectionChanged = (Action)Delegate.Combine(Selection.selectionChanged, new Action(OnSelectionChange));
		}

		public void Initialize()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Expected O, but got Unknown
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Expected O, but got Unknown
			SceneView.onSceneGUIDelegate = (OnSceneFunc)Delegate.Remove((Delegate)(object)SceneView.onSceneGUIDelegate, (Delegate)new OnSceneFunc(OnSceneGUI));
			EditorApplication.update = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
			EditorApplication.hierarchyWindowChanged = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.hierarchyWindowChanged, (Delegate)new CallbackFunction(HierarchyWindowChanged));
			SceneView.onSceneGUIDelegate = (OnSceneFunc)Delegate.Combine((Delegate)(object)SceneView.onSceneGUIDelegate, (Delegate)new OnSceneFunc(OnSceneGUI));
			EditorApplication.update = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
			EditorApplication.hierarchyWindowChanged = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.hierarchyWindowChanged, (Delegate)new CallbackFunction(HierarchyWindowChanged));
			LoadGUIResources();
			LoadPreferences();
			instance = this;
			pg_GridRenderer.Init();
			SetMenuIsExtended(menuOpen);
			lastTime = Time.get_realtimeSinceStartup();
			menuOpen = !menuOpen;
			ToggleMenuVisibility();
			if (drawGrid)
			{
				pg_Util.SetUnityGridEnabled(isEnabled: false);
			}
			gridRepaint = true;
			RepaintSceneView();
		}

		private void OnDestroy()
		{
			Close(isBeingDestroyed: true);
		}

		public void Close()
		{
			EditorPrefs.SetBool("pgProGridsIsEnabled", false);
			Object.DestroyImmediate((Object)(object)this);
			Selection.selectionChanged = (Action)Delegate.Remove(Selection.selectionChanged, new Action(OnSelectionChange));
		}

		public void Close(bool isBeingDestroyed)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			pg_GridRenderer.Destroy();
			SceneView.onSceneGUIDelegate = (OnSceneFunc)Delegate.Remove((Delegate)(object)SceneView.onSceneGUIDelegate, (Delegate)new OnSceneFunc(OnSceneGUI));
			EditorApplication.update = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
			EditorApplication.hierarchyWindowChanged = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.hierarchyWindowChanged, (Delegate)new CallbackFunction(HierarchyWindowChanged));
			instance = null;
			foreach (Action<bool> toolbarEventSubscriber in toolbarEventSubscribers)
			{
				toolbarEventSubscriber(obj: false);
			}
			pg_Util.SetUnityGridEnabled(isEnabled: true);
			SceneView.RepaintAll();
		}

		private void LoadGUIResources()
		{
			if ((Object)(object)gc_GridEnabled.image_on == (Object)null)
			{
				gc_GridEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_Vis_On.png");
			}
			if ((Object)(object)gc_GridEnabled.image_off == (Object)null)
			{
				gc_GridEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_Vis_Off.png");
			}
			if ((Object)(object)gc_SnapEnabled.image_on == (Object)null)
			{
				gc_SnapEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_Snap_On.png");
			}
			if ((Object)(object)gc_SnapEnabled.image_off == (Object)null)
			{
				gc_SnapEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_Snap_Off.png");
			}
			if ((Object)(object)gc_SnapToGrid.image_on == (Object)null)
			{
				gc_SnapToGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PushToGrid_Normal.png");
			}
			if ((Object)(object)gc_LockGrid.image_on == (Object)null)
			{
				gc_LockGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Lock_On.png");
			}
			if ((Object)(object)gc_LockGrid.image_off == (Object)null)
			{
				gc_LockGrid.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Lock_Off.png");
			}
			if ((Object)(object)gc_AngleEnabled.image_on == (Object)null)
			{
				gc_AngleEnabled.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_AngleVis_On.png");
			}
			if ((Object)(object)gc_AngleEnabled.image_off == (Object)null)
			{
				gc_AngleEnabled.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_AngleVis_Off.png");
			}
			if ((Object)(object)gc_RenderPlaneX.image_on == (Object)null)
			{
				gc_RenderPlaneX.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_X_On.png");
			}
			if ((Object)(object)gc_RenderPlaneX.image_off == (Object)null)
			{
				gc_RenderPlaneX.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_X_Off.png");
			}
			if ((Object)(object)gc_RenderPlaneY.image_on == (Object)null)
			{
				gc_RenderPlaneY.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Y_On.png");
			}
			if ((Object)(object)gc_RenderPlaneY.image_off == (Object)null)
			{
				gc_RenderPlaneY.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Y_Off.png");
			}
			if ((Object)(object)gc_RenderPlaneZ.image_on == (Object)null)
			{
				gc_RenderPlaneZ.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Z_On.png");
			}
			if ((Object)(object)gc_RenderPlaneZ.image_off == (Object)null)
			{
				gc_RenderPlaneZ.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_Z_Off.png");
			}
			if ((Object)(object)gc_RenderPerspectiveGrid.image_on == (Object)null)
			{
				gc_RenderPerspectiveGrid.image_on = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_3D_On.png");
			}
			if ((Object)(object)gc_RenderPerspectiveGrid.image_off == (Object)null)
			{
				gc_RenderPerspectiveGrid.image_off = pg_IconUtility.LoadIcon("ProGrids2_GUI_PGrid_3D_Off.png");
			}
			if ((Object)(object)icon_extendoOpen == (Object)null)
			{
				icon_extendoOpen = pg_IconUtility.LoadIcon("ProGrids2_MenuExtendo_Open.png");
			}
			if ((Object)(object)icon_extendoClose == (Object)null)
			{
				icon_extendoClose = pg_IconUtility.LoadIcon("ProGrids2_MenuExtendo_Close.png");
			}
		}

		public float GetSnapIncrement()
		{
			return t_snapValue;
		}

		public void SetSnapIncrement(float inc)
		{
			SetSnapValue(snapUnit, Mathf.Max(inc, 0.001f), 2048);
		}

		private void RepaintSceneView()
		{
			SceneView.RepaintAll();
		}

		private void Update()
		{
			deltaTime = Time.get_realtimeSinceStartup() - lastTime;
			lastTime = Time.get_realtimeSinceStartup();
			if ((menuOpen && menuStart < 8f) || (!menuOpen && menuStart > (float)MENU_HIDDEN))
			{
				menuStart += deltaTime * 500f * (menuOpen ? 1f : (-1f));
				menuStart = Mathf.Clamp(menuStart, (float)MENU_HIDDEN, 8f);
				RepaintSceneView();
			}
			float a = menuBackgroundColor.a;
			backgroundFade = ((mouseOverMenu || !menuOpen) ? 2.5f : (-2.5f));
			menuBackgroundColor.a = Mathf.Clamp(menuBackgroundColor.a + backgroundFade * deltaTime, 0f, 0.5f);
			extendoNormalColor.a = menuBackgroundColor.a;
			extendoHoverColor.a = menuBackgroundColor.a / 0.5f;
			if (!Mathf.Approximately(menuBackgroundColor.a, a))
			{
				RepaintSceneView();
			}
		}

		private void DrawSceneGUI()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Expected O, but got Unknown
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Expected O, but got Unknown
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_047d: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0538: Unknown result type (might be due to invalid IL or missing references)
			//IL_0597: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0710: Unknown result type (might be due to invalid IL or missing references)
			//IL_074a: Unknown result type (might be due to invalid IL or missing references)
			GUI.set_backgroundColor(menuBackgroundColor);
			((Rect)(ref backgroundRect)).set_x(((Rect)(ref r)).get_x() - 4f);
			((Rect)(ref backgroundRect)).set_y(0f);
			((Rect)(ref backgroundRect)).set_width(((Rect)(ref r)).get_width() + 8f);
			((Rect)(ref backgroundRect)).set_height(((Rect)(ref r)).get_y() + ((Rect)(ref r)).get_height() + 3f);
			GUI.Box(backgroundRect, "", backgroundStyle);
			ref Rect reference = ref backgroundRect;
			((Rect)(ref reference)).set_width(((Rect)(ref reference)).get_width() + 32f);
			ref Rect reference2 = ref backgroundRect;
			((Rect)(ref reference2)).set_height(((Rect)(ref reference2)).get_height() + 32f);
			GUI.set_backgroundColor(Color.get_white());
			if (!guiInitialized)
			{
				extendoStyle.get_normal().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
				extendoStyle.get_hover().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
				guiInitialized = true;
				backgroundStyle.get_normal().set_background(EditorGUIUtility.get_whiteTexture());
				Texture2D val = pg_IconUtility.LoadIcon("ProGrids2_Button_Normal.png");
				Texture2D background = pg_IconUtility.LoadIcon("ProGrids2_Button_Hover.png");
				if ((Object)(object)val == (Object)null)
				{
					gridButtonStyleBlank = new GUIStyle(GUIStyle.op_Implicit("button"));
				}
				else
				{
					gridButtonStyleBlank.get_normal().set_background(val);
					gridButtonStyleBlank.get_hover().set_background(background);
					gridButtonStyleBlank.get_normal().set_textColor(((Object)(object)val != (Object)null) ? Color.get_white() : Color.get_black());
					gridButtonStyleBlank.get_hover().set_textColor(new Color(0.7f, 0.7f, 0.7f, 1f));
				}
				gridButtonStyleBlank.set_padding(new RectOffset(1, 2, 1, 2));
				gridButtonStyleBlank.set_alignment((TextAnchor)4);
			}
			((Rect)(ref r)).set_y(menuStart);
			gc_SnapIncrement.set_text(t_snapValue.ToString("#.####"));
			if (GUI.Button(r, gc_SnapIncrement, gridButtonStyleBlank))
			{
				pg_ParameterWindow window = EditorWindow.GetWindow<pg_ParameterWindow>(true, "ProGrids Settings", true);
				Rect position = ((EditorWindow)SceneView.get_lastActiveSceneView()).get_position();
				window.editor = this;
				((EditorWindow)window).set_position(new Rect(((Rect)(ref position)).get_x() + ((Rect)(ref r)).get_x() + ((Rect)(ref r)).get_width() + 3f, ((Rect)(ref position)).get_y() + ((Rect)(ref r)).get_y() + 24f, 256f, 174f));
			}
			ref Rect reference3 = ref r;
			((Rect)(ref reference3)).set_y(((Rect)(ref reference3)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_GridEnabled, drawGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SetGridEnabled(!drawGrid);
			}
			ref Rect reference4 = ref r;
			((Rect)(ref reference4)).set_y(((Rect)(ref reference4)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_SnapEnabled, snapEnabled, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SetSnapEnabled(!snapEnabled);
			}
			ref Rect reference5 = ref r;
			((Rect)(ref reference5)).set_y(((Rect)(ref reference5)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_SnapToGrid, enabled: true, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SnapToGrid(Selection.get_transforms());
			}
			ref Rect reference6 = ref r;
			((Rect)(ref reference6)).set_y(((Rect)(ref reference6)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_LockGrid, lockGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				lockGrid = !lockGrid;
				EditorPrefs.SetBool("pg_LockGrid", lockGrid);
				EditorPrefs.SetString("pg_LockedGridPivot", ((object)(Vector3)(ref pivot)).ToString());
				if (!lockGrid)
				{
					offset = 0f;
				}
				gridRepaint = true;
				RepaintSceneView();
			}
			if (menuIsOrtho)
			{
				ref Rect reference7 = ref r;
				((Rect)(ref reference7)).set_y(((Rect)(ref reference7)).get_y() + (((Rect)(ref r)).get_height() + 3f));
				if (pg_ToggleContent.ToggleButton(r, gc_AngleEnabled, drawAngles, gridButtonStyle, EditorStyles.get_miniButton()))
				{
					SetDrawAngles(!drawAngles);
				}
			}
			ref Rect reference8 = ref r;
			((Rect)(ref reference8)).set_y(((Rect)(ref reference8)).get_y() + (((Rect)(ref r)).get_height() + 3f + 4f));
			if (pg_ToggleContent.ToggleButton(r, gc_RenderPlaneX, (renderPlane & Axis.X) == Axis.X && !fullGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SetRenderPlane(Axis.X);
			}
			ref Rect reference9 = ref r;
			((Rect)(ref reference9)).set_y(((Rect)(ref reference9)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_RenderPlaneY, (renderPlane & Axis.Y) == Axis.Y && !fullGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SetRenderPlane(Axis.Y);
			}
			ref Rect reference10 = ref r;
			((Rect)(ref reference10)).set_y(((Rect)(ref reference10)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_RenderPlaneZ, (renderPlane & Axis.Z) == Axis.Z && !fullGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				SetRenderPlane(Axis.Z);
			}
			ref Rect reference11 = ref r;
			((Rect)(ref reference11)).set_y(((Rect)(ref reference11)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			if (pg_ToggleContent.ToggleButton(r, gc_RenderPerspectiveGrid, fullGrid, gridButtonStyle, EditorStyles.get_miniButton()))
			{
				fullGrid = !fullGrid;
				gridRepaint = true;
				EditorPrefs.SetBool("pg_PerspGrid", fullGrid);
				RepaintSceneView();
			}
			ref Rect reference12 = ref r;
			((Rect)(ref reference12)).set_y(((Rect)(ref reference12)).get_y() + (((Rect)(ref r)).get_height() + 3f));
			((Rect)(ref extendoButtonRect)).set_x(((Rect)(ref r)).get_x());
			((Rect)(ref extendoButtonRect)).set_y(((Rect)(ref r)).get_y());
			((Rect)(ref extendoButtonRect)).set_width(((Rect)(ref r)).get_width());
			((Rect)(ref extendoButtonRect)).set_height(((Rect)(ref r)).get_height());
			GUI.set_backgroundColor(extendoButtonHovering ? extendoHoverColor : extendoNormalColor);
			gc_ExtendMenu.set_text((!((Object)(object)icon_extendoOpen == (Object)null)) ? "" : (menuOpen ? "Close" : "Open"));
			if (GUI.Button(r, gc_ExtendMenu, Object.op_Implicit((Object)(object)icon_extendoOpen) ? extendoStyle : gridButtonStyleBlank))
			{
				ToggleMenuVisibility();
				extendoButtonHovering = false;
			}
			GUI.set_backgroundColor(Color.get_white());
		}

		private void ToggleMenuVisibility()
		{
			menuOpen = !menuOpen;
			EditorPrefs.SetBool("pgProGridsIsExtended", menuOpen);
			extendoStyle.get_normal().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
			extendoStyle.get_hover().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
			foreach (Action<bool> toolbarEventSubscriber in toolbarEventSubscribers)
			{
				toolbarEventSubscriber(menuOpen);
			}
			RepaintSceneView();
		}

		private void SetMenuIsExtended(bool isExtended)
		{
			menuOpen = isExtended;
			menuIsOrtho = ortho;
			menuStart = (menuOpen ? 8 : MENU_HIDDEN);
			menuBackgroundColor.a = 0f;
			extendoNormalColor.a = menuBackgroundColor.a;
			extendoHoverColor.a = menuBackgroundColor.a / 0.5f;
			extendoStyle.get_normal().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
			extendoStyle.get_hover().set_background(menuOpen ? icon_extendoClose : icon_extendoOpen);
			foreach (Action<bool> toolbarEventSubscriber in toolbarEventSubscribers)
			{
				toolbarEventSubscriber(menuOpen);
			}
			EditorPrefs.SetBool("pgProGridsIsExtended", menuOpen);
		}

		private void OpenProGridsPopup()
		{
			if (EditorUtility.DisplayDialog("Upgrade to ProGrids", "Enables all kinds of super-cool features, like different snap values, more units of measurement, and angles.", "Upgrade", "Cancel"))
			{
				Application.OpenURL("http://u3d.as/content/six-by-seven-studio/pro-grids/3ov");
			}
		}

		public void OnSceneGUI(SceneView scnview)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Invalid comparison between Unknown and I4
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Invalid comparison between Unknown and I4
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Invalid comparison between Unknown and I4
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Invalid comparison between Unknown and I4
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Invalid comparison between Unknown and I4
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Invalid comparison between Unknown and I4
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Invalid comparison between Unknown and I4
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0354: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0364: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0372: Unknown result type (might be due to invalid IL or missing references)
			//IL_037d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_043b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0469: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0485: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0503: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_0513: Unknown result type (might be due to invalid IL or missing references)
			//IL_0518: Unknown result type (might be due to invalid IL or missing references)
			//IL_051d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_052e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0550: Unknown result type (might be due to invalid IL or missing references)
			//IL_0555: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_0565: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Unknown result type (might be due to invalid IL or missing references)
			//IL_056f: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_059d: Unknown result type (might be due to invalid IL or missing references)
			//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_060b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0610: Unknown result type (might be due to invalid IL or missing references)
			//IL_061b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0620: Unknown result type (might be due to invalid IL or missing references)
			//IL_0626: Unknown result type (might be due to invalid IL or missing references)
			//IL_062c: Invalid comparison between Unknown and I4
			//IL_066a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0670: Unknown result type (might be due to invalid IL or missing references)
			//IL_0694: Unknown result type (might be due to invalid IL or missing references)
			//IL_0699: Unknown result type (might be due to invalid IL or missing references)
			//IL_069b: Unknown result type (might be due to invalid IL or missing references)
			//IL_069e: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0703: Unknown result type (might be due to invalid IL or missing references)
			//IL_0724: Unknown result type (might be due to invalid IL or missing references)
			//IL_0759: Unknown result type (might be due to invalid IL or missing references)
			//IL_077d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0788: Unknown result type (might be due to invalid IL or missing references)
			//IL_0791: Unknown result type (might be due to invalid IL or missing references)
			//IL_0796: Unknown result type (might be due to invalid IL or missing references)
			//IL_079e: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07be: Unknown result type (might be due to invalid IL or missing references)
			//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0800: Unknown result type (might be due to invalid IL or missing references)
			//IL_0805: Unknown result type (might be due to invalid IL or missing references)
			//IL_080a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0819: Unknown result type (might be due to invalid IL or missing references)
			//IL_081b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0855: Unknown result type (might be due to invalid IL or missing references)
			//IL_085a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0862: Unknown result type (might be due to invalid IL or missing references)
			//IL_0881: Unknown result type (might be due to invalid IL or missing references)
			//IL_0886: Unknown result type (might be due to invalid IL or missing references)
			bool flag = (Object)(object)scnview == (Object)(object)SceneView.get_lastActiveSceneView();
			if (flag)
			{
				Handles.BeginGUI();
				DrawSceneGUI();
				Handles.EndGUI();
			}
			if (EditorApplication.get_isPlayingOrWillChangePlaymode())
			{
				return;
			}
			Event current = Event.get_current();
			if (flag && (int)current.get_type() == 2)
			{
				bool flag2 = extendoButtonHovering;
				extendoButtonHovering = ((Rect)(ref extendoButtonRect)).Contains(current.get_mousePosition());
				if (extendoButtonHovering != flag2)
				{
					RepaintSceneView();
				}
				mouseOverMenu = ((Rect)(ref backgroundRect)).Contains(current.get_mousePosition());
			}
			if (((object)current).Equals((object)Event.KeyboardEvent("s")))
			{
				toggleAxisConstraint = true;
			}
			if (((object)current).Equals((object)Event.KeyboardEvent("d")))
			{
				toggleTempSnap = true;
			}
			if (current.get_isKey())
			{
				toggleAxisConstraint = false;
				toggleTempSnap = false;
				bool flag3 = true;
				if (current.get_keyCode() == m_IncreaseGridSizeShortcut)
				{
					if ((int)current.get_type() == 5)
					{
						IncreaseGridSize();
					}
				}
				else if (current.get_keyCode() == m_DecreaseGridSizeShortcut)
				{
					if ((int)current.get_type() == 5)
					{
						DecreaseGridSize();
					}
				}
				else if (current.get_keyCode() == m_NudgePerspectiveBackwardShortcut)
				{
					if ((int)current.get_type() == 5 && VerifyMenuNudgePerspective())
					{
						MenuNudgePerspectiveBackward();
					}
				}
				else if (current.get_keyCode() == m_NudgePerspectiveForwardShortcut)
				{
					if ((int)current.get_type() == 5 && VerifyMenuNudgePerspective())
					{
						MenuNudgePerspectiveForward();
					}
				}
				else if (current.get_keyCode() == m_NudgePerspectiveResetShortcut)
				{
					if ((int)current.get_type() == 5 && VerifyMenuNudgePerspective())
					{
						MenuNudgePerspectiveReset();
					}
				}
				else if (current.get_keyCode() == m_CyclePerspectiveShortcut)
				{
					if ((int)current.get_type() == 5)
					{
						CyclePerspective();
					}
				}
				else
				{
					flag3 = false;
				}
				if (flag3)
				{
					current.Use();
				}
			}
			Camera current2 = Camera.get_current();
			if ((Object)(object)current2 == (Object)null)
			{
				return;
			}
			int num;
			if (current2.get_orthographic())
			{
				Quaternion rotation = scnview.get_rotation();
				Vector3 eulerAngles = ((Quaternion)(ref rotation)).get_eulerAngles();
				num = (IsRounded(((Vector3)(ref eulerAngles)).get_normalized()) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			ortho = (byte)num != 0;
			camDir = pg_Util.CeilFloor(pivot - ((Component)current2).get_transform().get_position());
			if ((ortho && !prevOrtho) || ortho != menuIsOrtho)
			{
				OnSceneBecameOrtho(flag);
			}
			if (!ortho && prevOrtho)
			{
				OnSceneBecamePersp(flag);
			}
			prevOrtho = ortho;
			float num2 = Vector3.Distance(((Component)current2).get_transform().get_position(), lastPivot);
			if (fullGrid)
			{
				pivot = ((lockGrid || (Object)(object)Selection.get_activeTransform() == (Object)null) ? pivot : Selection.get_activeTransform().get_position());
			}
			else
			{
				Vector3 v = pivot;
				Ray val = default(Ray);
				((Ray)(ref val))._002Ector(((Component)current2).get_transform().get_position(), ((Component)current2).get_transform().get_forward());
				Plane val2 = default(Plane);
				((Plane)(ref val2))._002Ector(Vector3.get_up(), pivot);
				if ((lockGrid && !current2.InFrustum(pivot)) || !lockGrid || (Object)(object)scnview != (Object)(object)SceneView.get_lastActiveSceneView())
				{
					float num3 = default(float);
					v = ((!((Plane)(ref val2)).Raycast(val, ref num3)) ? ((Ray)(ref val)).GetPoint(Mathf.Min(current2.get_farClipPlane() / 2f, planeGridDrawDistance / 2f)) : ((Ray)(ref val)).GetPoint(Mathf.Min(num3, planeGridDrawDistance / 2f)));
				}
				if (lockGrid)
				{
					pivot = pg_Enum.InverseAxisMask(v, renderPlane) + pg_Enum.AxisMask(pivot, renderPlane);
				}
				else
				{
					pivot = (((Object)(object)Selection.get_activeTransform() == (Object)null) ? pivot : Selection.get_activeTransform().get_position());
					if ((Object)(object)Selection.get_activeTransform() == (Object)null || !current2.InFrustum(pivot))
					{
						pivot = pg_Enum.InverseAxisMask(v, renderPlane) + pg_Enum.AxisMask(((Object)(object)Selection.get_activeTransform() == (Object)null) ? pivot : Selection.get_activeTransform().get_position(), renderPlane);
					}
				}
			}
			if (drawGrid)
			{
				if (ortho)
				{
					DrawGridOrthographic(current2);
				}
				else if (gridRepaint || pivot != lastPivot || Mathf.Abs(num2 - lastDistance) > lastDistance / 2f || camDir != prevCamDir)
				{
					prevCamDir = camDir;
					gridRepaint = false;
					lastPivot = pivot;
					lastDistance = num2;
					if (fullGrid)
					{
						pg_GridRenderer.DrawGridPerspective(current2, pivot, snapValue, (Color[])(object)new Color[3] { gridColorX, gridColorY, gridColorZ }, alphaBump);
					}
					else
					{
						if ((renderPlane & Axis.X) == Axis.X)
						{
							planeGridDrawDistance = pg_GridRenderer.DrawPlane(current2, pivot + Vector3.get_right() * offset, Vector3.get_up(), Vector3.get_forward(), snapValue, gridColorX, alphaBump);
						}
						if ((renderPlane & Axis.Y) == Axis.Y)
						{
							planeGridDrawDistance = pg_GridRenderer.DrawPlane(current2, pivot + Vector3.get_up() * offset, Vector3.get_right(), Vector3.get_forward(), snapValue, gridColorY, alphaBump);
						}
						if ((renderPlane & Axis.Z) == Axis.Z)
						{
							planeGridDrawDistance = pg_GridRenderer.DrawPlane(current2, pivot + Vector3.get_forward() * offset, Vector3.get_up(), Vector3.get_right(), snapValue, gridColorZ, alphaBump);
						}
					}
				}
			}
			if (!Selection.get_transforms().Contains(lastTransform) && Object.op_Implicit((Object)(object)Selection.get_activeTransform()))
			{
				lastTransform = Selection.get_activeTransform();
				lastPosition = Selection.get_activeTransform().get_position();
				lastScale = Selection.get_activeTransform().get_localScale();
			}
			if ((int)current.get_type() == 1)
			{
				firstMove = true;
			}
			if (!snapEnabled || GUIUtility.get_hotControl() < 1 || !Object.op_Implicit((Object)(object)Selection.get_activeTransform()) || !pg_Util.SnapIsEnabled(Selection.get_activeTransform()))
			{
				return;
			}
			Transform[] transforms;
			if (!FuzzyEquals(lastTransform.get_position(), lastPosition))
			{
				Transform val3 = lastTransform;
				if (!toggleTempSnap)
				{
					Vector3 position = val3.get_position();
					Vector3 mask = position - lastPosition;
					bool flag4 = (toggleAxisConstraint ? (!useAxisConstraints) : useAxisConstraints);
					if (flag4)
					{
						val3.set_position(pg_Util.SnapValue(position, mask, snapValue));
					}
					else
					{
						val3.set_position(pg_Util.SnapValue(position, snapValue));
					}
					Vector3 movement = val3.get_position() - position;
					if (predictiveGrid && firstMove && !fullGrid)
					{
						firstMove = false;
						Axis axis = pg_Util.CalcDragAxis(movement, scnview.get_camera());
						if (axis != 0 && axis != renderPlane)
						{
							SetRenderPlane(axis);
						}
					}
					if (_snapAsGroup)
					{
						OffsetTransforms(Selection.get_transforms(), val3, movement);
					}
					else
					{
						transforms = Selection.get_transforms();
						foreach (Transform val4 in transforms)
						{
							val4.set_position(flag4 ? pg_Util.SnapValue(val4.get_position(), mask, snapValue) : pg_Util.SnapValue(val4.get_position(), snapValue));
						}
					}
				}
				lastPosition = val3.get_position();
			}
			if (FuzzyEquals(lastTransform.get_localScale(), lastScale) || !_scaleSnapEnabled || toggleTempSnap)
			{
				return;
			}
			Vector3 val5 = lastTransform.get_localScale() - lastScale;
			if (predictiveGrid)
			{
				Axis axis2 = pg_Util.CalcDragAxis(Selection.get_activeTransform().TransformDirection(val5), scnview.get_camera());
				if (axis2 != 0 && axis2 != renderPlane)
				{
					SetRenderPlane(axis2);
				}
			}
			transforms = Selection.get_transforms();
			foreach (Transform obj in transforms)
			{
				obj.set_localScale(pg_Util.SnapValue(obj.get_localScale(), val5, snapValue));
			}
			lastScale = lastTransform.get_localScale();
		}

		private void OnSelectionChange()
		{
			pg_Util.ClearSnapEnabledCache();
		}

		private void OnSceneBecameOrtho(bool isCurrentView)
		{
			pg_GridRenderer.Destroy();
			if (isCurrentView && ortho != menuIsOrtho)
			{
				SetMenuIsExtended(menuOpen);
			}
		}

		private void OnSceneBecamePersp(bool isCurrentView)
		{
			if (isCurrentView && ortho != menuIsOrtho)
			{
				SetMenuIsExtended(menuOpen);
			}
		}

		private void DrawGridOrthographic(Camera cam)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = ((Component)Camera.get_current()).get_transform().TransformDirection(Vector3.get_forward());
			Axis axis = AxisWithVector(((Vector3)(ref val)).get_normalized());
			if (drawGrid)
			{
				switch (axis)
				{
				case Axis.X:
				case Axis.NegX:
					DrawGridOrthographic(cam, axis, gridColorX_primary, gridColorX);
					break;
				case Axis.Y:
				case Axis.NegY:
					DrawGridOrthographic(cam, axis, gridColorY_primary, gridColorY);
					break;
				case Axis.Z:
				case Axis.NegZ:
					DrawGridOrthographic(cam, axis, gridColorZ_primary, gridColorZ);
					break;
				}
			}
		}

		private void DrawGridOrthographic(Camera cam, Axis camAxis, Color primaryColor, Color secondaryColor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_029f: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0307: Unknown result type (might be due to invalid IL or missing references)
			//IL_030c: Unknown result type (might be due to invalid IL or missing references)
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_0376: Unknown result type (might be due to invalid IL or missing references)
			//IL_0377: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038d: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0400: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0405: Unknown result type (might be due to invalid IL or missing references)
			//IL_040a: Unknown result type (might be due to invalid IL or missing references)
			//IL_040f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_0415: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_0427: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0436: Unknown result type (might be due to invalid IL or missing references)
			previousColor = Handles.get_color();
			Handles.set_color(primaryColor);
			Vector3 val = pg_Util.SnapToFloor(cam.ScreenToWorldPoint(Vector2.op_Implicit(Vector2.get_zero())), snapValue);
			Vector3 val2 = pg_Util.SnapToFloor(cam.ScreenToWorldPoint(Vector2.op_Implicit(new Vector2((float)cam.get_pixelWidth(), 0f))), snapValue);
			Vector3 val3 = pg_Util.SnapToFloor(cam.ScreenToWorldPoint(Vector2.op_Implicit(new Vector2(0f, (float)cam.get_pixelHeight()))), snapValue);
			Vector3 val4 = pg_Util.SnapToFloor(cam.ScreenToWorldPoint(Vector2.op_Implicit(new Vector2((float)cam.get_pixelWidth(), (float)cam.get_pixelHeight()))), snapValue);
			Vector3 val5 = VectorWithAxis(camAxis);
			float num = Vector3.Distance(val, val2);
			float num2 = Vector3.Distance(val2, val4);
			val += val5 * 10f;
			val4 += val5 * 10f;
			val2 += val5 * 10f;
			val3 += val5 * 10f;
			Vector3 right = ((Component)cam).get_transform().get_right();
			Vector3 up = ((Component)cam).get_transform().get_up();
			float num3 = snapValue;
			int num4 = (int)Mathf.Ceil(num / num3) + 2;
			float num5 = 2f;
			while (num4 > 150)
			{
				num3 *= num5;
				num4 = (int)Mathf.Ceil(num / num3) + 2;
				num5 += 1f;
			}
			Vector3 val6 = ((right.Sum() > 0f) ? pg_Util.SnapToFloor(val, right, num3 * (float)PRIMARY_COLOR_INCREMENT) : pg_Util.SnapToCeil(val, right, num3 * (float)PRIMARY_COLOR_INCREMENT));
			Vector3 val7 = val6 - up * (num2 + num3 * 2f);
			Vector3 val8 = val6 + up * (num2 + num3 * 2f);
			num4 += PRIMARY_COLOR_INCREMENT;
			Vector3.get_zero();
			Vector3 zero = Vector3.get_zero();
			for (int i = -1; i < num4; i++)
			{
				Vector3 val9 = val7 + (float)i * (right * num3);
				zero = val8 + (float)i * (right * num3);
				Handles.set_color((i % PRIMARY_COLOR_INCREMENT == 0) ? primaryColor : secondaryColor);
				Handles.DrawLine(val9, zero);
			}
			num4 = (int)Mathf.Ceil(num2 / num3) + 2;
			num5 = 2f;
			while (num4 > 150)
			{
				num3 *= num5;
				num4 = (int)Mathf.Ceil(num2 / num3) + 2;
				num5 += 1f;
			}
			Vector3 val10 = ((up.Sum() > 0f) ? pg_Util.SnapToCeil(val3, up, num3 * (float)PRIMARY_COLOR_INCREMENT) : pg_Util.SnapToFloor(val3, up, num3 * (float)PRIMARY_COLOR_INCREMENT));
			val7 = val10 - right * (num + num3 * 2f);
			val8 = val10 + right * (num + num3 * 2f);
			num4 += PRIMARY_COLOR_INCREMENT;
			for (int j = -1; j < num4; j++)
			{
				Vector3 val11 = val7 + (float)j * (-up * num3);
				zero = val8 + (float)j * (-up * num3);
				Handles.set_color((j % PRIMARY_COLOR_INCREMENT == 0) ? primaryColor : secondaryColor);
				Handles.DrawLine(val11, zero);
			}
			if (drawAngles)
			{
				Vector3 val12 = pg_Util.SnapValue((val4 + val) / 2f, snapValue);
				float num6 = ((num > num2) ? num : num2);
				float num7 = Mathf.Tan((float)Math.PI / 180f * angleValue) * num6;
				Vector3 val13 = ((Component)cam).get_transform().get_up() * num7;
				Vector3 val14 = ((Component)cam).get_transform().get_right() * num6;
				Vector3 val15 = val12 - (val13 + val14);
				Vector3 val16 = val12 + (val13 + val14);
				Vector3 val17 = val12 + (val14 - val13);
				Vector3 val18 = val12 + (val13 - val14);
				Handles.set_color(primaryColor);
				Handles.DrawLine(val15, val16);
				Handles.DrawLine(val18, val17);
			}
			Handles.set_color(previousColor);
		}

		public SnapUnit SnapUnitWithString(string str)
		{
			foreach (SnapUnit value in Enum.GetValues(typeof(SnapUnit)))
			{
				if (value.ToString() == str)
				{
					return value;
				}
			}
			return SnapUnit.Meter;
		}

		public Axis AxisWithVector(Vector3 val)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val2 = default(Vector3);
			((Vector3)(ref val2))._002Ector(Mathf.Abs(val.x), Mathf.Abs(val.y), Mathf.Abs(val.z));
			if (val2.x > val2.y && val2.x > val2.z)
			{
				if (val.x > 0f)
				{
					return Axis.X;
				}
				return Axis.NegX;
			}
			if (val2.y > val2.x && val2.y > val2.z)
			{
				if (val.y > 0f)
				{
					return Axis.Y;
				}
				return Axis.NegY;
			}
			if (val.z > 0f)
			{
				return Axis.Z;
			}
			return Axis.NegZ;
		}

		public Vector3 VectorWithAxis(Axis axis)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			return (Vector3)(axis switch
			{
				Axis.X => Vector3.get_right(), 
				Axis.Y => Vector3.get_up(), 
				Axis.Z => Vector3.get_forward(), 
				Axis.NegX => -Vector3.get_right(), 
				Axis.NegY => -Vector3.get_up(), 
				Axis.NegZ => -Vector3.get_forward(), 
				_ => Vector3.get_forward(), 
			});
		}

		public bool IsRounded(Vector3 v)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if (!Mathf.Approximately(v.x, 1f) && !Mathf.Approximately(v.y, 1f) && !Mathf.Approximately(v.z, 1f))
			{
				return v == Vector3.get_zero();
			}
			return true;
		}

		public Vector3 RoundAxis(Vector3 v)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return VectorWithAxis(AxisWithVector(v));
		}

		private static bool FuzzyEquals(Vector3 lhs, Vector3 rhs)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (Mathf.Abs(lhs.x - rhs.x) < 0.001f && Mathf.Abs(lhs.y - rhs.y) < 0.001f)
			{
				return Mathf.Abs(lhs.z - rhs.z) < 0.001f;
			}
			return false;
		}

		public void OffsetTransforms(Transform[] trsfrms, Transform ignore, Vector3 offset)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			foreach (Transform val in trsfrms)
			{
				if ((Object)(object)val != (Object)(object)ignore)
				{
					val.set_position(val.get_position() + offset);
				}
			}
		}

		private void HierarchyWindowChanged()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)Selection.get_activeTransform() != (Object)null)
			{
				lastPosition = Selection.get_activeTransform().get_position();
			}
		}

		public void SetSnapEnabled(bool enable)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			EditorPrefs.SetBool("pgSnapEnabled", enable);
			if (Object.op_Implicit((Object)(object)Selection.get_activeTransform()))
			{
				lastTransform = Selection.get_activeTransform();
				lastPosition = Selection.get_activeTransform().get_position();
			}
			snapEnabled = enable;
			gridRepaint = true;
			RepaintSceneView();
		}

		public void SetSnapValue(SnapUnit su, float val, int multiplier)
		{
			int num = Mathf.Min(Mathf.Max(1, multiplier), int.MaxValue);
			float num2 = (float)num / 2048f;
			snapValue = pg_Enum.SnapUnitValue(su) * val * num2;
			RepaintSceneView();
			EditorPrefs.SetInt("pg_GridUnit", (int)su);
			EditorPrefs.SetFloat("pgSnapValue", val);
			EditorPrefs.SetInt("pgSnapMultiplier", num);
			t_snapValue = val * num2;
			snapUnit = su;
			switch (su)
			{
			case SnapUnit.Inch:
				PRIMARY_COLOR_INCREMENT = 12;
				break;
			case SnapUnit.Foot:
				PRIMARY_COLOR_INCREMENT = 3;
				break;
			default:
				PRIMARY_COLOR_INCREMENT = 10;
				break;
			}
			if (EditorPrefs.GetBool("pg_SyncUnitySnap", true))
			{
				EditorPrefs.SetFloat("MoveSnapX", snapValue);
				EditorPrefs.SetFloat("MoveSnapY", snapValue);
				EditorPrefs.SetFloat("MoveSnapZ", snapValue);
				if (EditorPrefs.GetBool("pg_SnapOnScale", true))
				{
					EditorPrefs.SetFloat("ScaleSnap", snapValue);
				}
				Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SnapSettings");
				if (type != null)
				{
					FieldInfo field = type.GetField("s_Initialized", BindingFlags.Static | BindingFlags.NonPublic);
					if (field != null)
					{
						field.SetValue(null, false);
						EditorWindow val2 = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault((EditorWindow x) => ((object)x).ToString().Contains("SnapSettings"));
						if ((Object)(object)val2 != (Object)null)
						{
							val2.Repaint();
						}
					}
				}
			}
			gridRepaint = true;
		}

		public void SetRenderPlane(Axis axis)
		{
			offset = 0f;
			fullGrid = false;
			renderPlane = axis;
			EditorPrefs.SetBool("pg_PerspGrid", fullGrid);
			EditorPrefs.SetInt("pg_GridAxis", (int)renderPlane);
			gridRepaint = true;
			RepaintSceneView();
		}

		public void SetGridEnabled(bool enable)
		{
			drawGrid = enable;
			if (!drawGrid)
			{
				pg_GridRenderer.Destroy();
			}
			else
			{
				pg_Util.SetUnityGridEnabled(isEnabled: false);
			}
			EditorPrefs.SetBool("showgrid", enable);
			gridRepaint = true;
			RepaintSceneView();
		}

		public void SetDrawAngles(bool enable)
		{
			drawAngles = enable;
			gridRepaint = true;
			RepaintSceneView();
		}

		private void SnapToGrid(Transform[] transforms)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			Undo.RecordObjects((Object[])(object)transforms, "Snap to Grid");
			foreach (Transform obj in transforms)
			{
				obj.set_position(pg_Util.SnapValue(obj.get_position(), snapValue));
			}
			gridRepaint = true;
			PushToGrid(snapValue);
		}

		internal bool GetUseAxisConstraints()
		{
			if (!toggleAxisConstraint)
			{
				return useAxisConstraints;
			}
			return !useAxisConstraints;
		}

		internal float GetSnapValue()
		{
			return snapValue;
		}

		internal bool GetSnapEnabled()
		{
			if (!toggleTempSnap)
			{
				return snapEnabled;
			}
			return !snapEnabled;
		}

		public static bool UseAxisConstraints()
		{
			if (!((Object)(object)instance != (Object)null))
			{
				return false;
			}
			return instance.GetUseAxisConstraints();
		}

		public static float SnapValue()
		{
			if (!((Object)(object)instance != (Object)null))
			{
				return 0f;
			}
			return instance.GetSnapValue();
		}

		public static bool SnapEnabled()
		{
			if (!((Object)(object)instance == (Object)null))
			{
				return instance.GetSnapEnabled();
			}
			return false;
		}

		public static void AddPushToGridListener(Action<float> listener)
		{
			pushToGridListeners.Add(listener);
		}

		public static void RemovePushToGridListener(Action<float> listener)
		{
			pushToGridListeners.Remove(listener);
		}

		public static void AddToolbarEventSubscriber(Action<bool> listener)
		{
			toolbarEventSubscribers.Add(listener);
		}

		public static void RemoveToolbarEventSubscriber(Action<bool> listener)
		{
			toolbarEventSubscribers.Remove(listener);
		}

		public static bool SceneToolbarActive()
		{
			return (Object)(object)instance != (Object)null;
		}

		private void PushToGrid(float snapValue)
		{
			foreach (Action<float> pushToGridListener in pushToGridListeners)
			{
				pushToGridListener(snapValue);
			}
		}

		public static void OnHandleMove(Vector3 worldDirection)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)instance != (Object)null)
			{
				instance.OnHandleMove_Internal(worldDirection);
			}
		}

		private void OnHandleMove_Internal(Vector3 worldDirection)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if (predictiveGrid && firstMove && !fullGrid)
			{
				firstMove = false;
				Axis axis = pg_Util.CalcDragAxis(worldDirection, SceneView.get_lastActiveSceneView().get_camera());
				if (axis != 0 && axis != renderPlane)
				{
					SetRenderPlane(axis);
				}
			}
		}

		public pg_Editor()
			: this()
		{
		}//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Expected O, but got Unknown
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Expected O, but got Unknown
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)

	}
}
