using System;
using System.Collections.Generic;
using System.IO;
using HumanAPI;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(WallVolume))]
public class WallVolumeEditor : Editor
{
	private enum Tool
	{
		None,
		Add,
		Remove
	}

	private Tool activeTool;

	private WallVolume volume;

	private MeshFilter volFilter;

	private MeshRenderer volRenderer;

	private MeshCollider volCollider;

	private int height = 1;

	private int brushX = 1;

	private int brushY = 1;

	private int brushZ = 1;

	private bool dirty;

	private WallVolumeFace[] faces;

	private Mesh mesh;

	private WallVolumeOrientaion lockOrientation;

	private int lockX;

	private int lockY;

	private int lockZ;

	private int lastX = int.MinValue;

	private int lastY = int.MinValue;

	private int lastZ = int.MinValue;

	private int controlID;

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("BakeLight", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			SaveToAsset();
			Lightmapping.BakeAsync();
		}
	}

	private void OnEnable()
	{
		activeTool = Tool.None;
		volume = ((Editor)this).get_target() as WallVolume;
		volRenderer = ((Component)volume).GetComponent<MeshRenderer>();
		volFilter = ((Component)volume).GetComponent<MeshFilter>();
		volCollider = ((Component)volume).GetComponent<MeshCollider>();
		if (volume.faces.Count == 0)
		{
			if ((Object)(object)((Renderer)volRenderer).get_sharedMaterial() == (Object)null)
			{
				((Renderer)volRenderer).set_sharedMaterial(AssetDatabase.GetBuiltinExtraResource<Material>("Default-Diffuse.mat"));
			}
			((Editor)this).get_serializedObject().Update();
			volume.AddVoxel(0, 0, 0, 4, 1, 4);
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
			SaveToAsset();
		}
	}

	private void OnDisable()
	{
		HideGizmo();
		SetTool(Tool.None);
		volume = null;
		volRenderer = null;
		volFilter = null;
		volCollider = null;
		if (dirty)
		{
			dirty = false;
			SaveToAsset();
		}
	}

	private void SaveToAsset()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		WallVolume wallVolume = ((Editor)this).get_target() as WallVolume;
		Object.DestroyImmediate((Object)(object)volCollider.get_sharedMesh(), true);
		if ((Object)(object)mesh == (Object)null)
		{
			mesh = new Mesh();
			((Object)mesh).set_name(((Object)wallVolume).get_name());
		}
		wallVolume.FillMesh(mesh, forceExact: true);
		UnwrapParam val = default(UnwrapParam);
		UnwrapParam.SetDefaults(ref val);
		Unwrapping.GenerateSecondaryUVSet(mesh, val);
		mesh.set_uv(mesh.get_uv2());
		Scene activeScene = SceneManager.GetActiveScene();
		string text = "Assets/WallMesh/" + ((Scene)(ref activeScene)).get_name() + "/";
		Directory.CreateDirectory(text);
		string text2 = text + ((Object)wallVolume).get_name() + ((Object)wallVolume).GetInstanceID() + ".asset";
		AssetDatabase.CreateAsset((Object)(object)mesh, text2);
		volFilter.set_sharedMesh(mesh);
		volCollider.set_sharedMesh(mesh);
		mesh = null;
	}

	private void BrushButtonXZ(string label, int x, int z)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		GUI.set_color(Color.get_white());
		GUI.set_backgroundColor((brushX == x && brushZ == z) ? Color.get_red() : Color.get_grey());
		if (GUILayout.Button(label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			brushX = x;
			brushZ = z;
		}
	}

	private void BrushButtonY(string label, int y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		GUI.set_color(Color.get_white());
		GUI.set_backgroundColor((height == y) ? Color.get_red() : Color.get_grey());
		if (GUILayout.Button(label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			height = y;
		}
	}

	private void OnSceneGUI()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Invalid comparison between Unknown and I4
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Invalid comparison between Unknown and I4
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Invalid comparison between Unknown and I4
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Invalid comparison between Unknown and I4
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected I4, but got Unknown
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		Handles.BeginGUI();
		GUILayout.BeginArea(new Rect(20f, 20f, 350f, 140f));
		Rect val = EditorGUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.set_color(Color.get_black());
		GUI.Box(val, GUIContent.none);
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.set_color(Color.get_black());
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Brush Height", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		BrushButtonY("2", 4);
		BrushButtonY("1", 2);
		BrushButtonY(".5", 1);
		GUILayout.EndVertical();
		GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUI.set_color(Color.get_black());
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.Label("Tool", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUI.set_color(Color.get_white());
		GUI.set_backgroundColor((activeTool == Tool.None) ? Color.get_red() : Color.get_grey());
		if (GUILayout.Button("None", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			SetTool(Tool.None);
		}
		GUI.set_backgroundColor((activeTool == Tool.Add) ? Color.get_red() : Color.get_grey());
		if (GUILayout.Button("Add (A)", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			SetTool(Tool.Add);
		}
		GUI.set_backgroundColor((activeTool == Tool.Remove) ? Color.get_red() : Color.get_grey());
		if (GUILayout.Button("Remove (S)", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			SetTool(Tool.Remove);
		}
		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		GUILayout.EndArea();
		if ((int)Event.get_current().get_type() == 5 && (int)Event.get_current().get_keyCode() == 97)
		{
			SetTool(Tool.Add, toggle: true);
		}
		if ((int)Event.get_current().get_type() == 5 && (int)Event.get_current().get_keyCode() == 115)
		{
			SetTool(Tool.Remove, toggle: true);
		}
		Handles.EndGUI();
		if (activeTool == Tool.None)
		{
			return;
		}
		Event current = Event.get_current();
		controlID = GUIUtility.GetControlID(((object)this).GetHashCode(), (FocusType)2);
		EventType type = current.get_type();
		switch ((int)type)
		{
		case 0:
			if (current.get_button() == 0 && (int)current.get_modifiers() == 0 && Raycast(volume.faces, out lockX, out lockY, out lockZ, out lockOrientation))
			{
				faces = volume.faces.ToArray();
				if ((Object)(object)mesh == (Object)null)
				{
					mesh = new Mesh();
					((Object)mesh).set_name(((Object)volume).get_name());
					mesh.MarkDynamic();
				}
				Paint(lockX, lockY, lockZ, lockOrientation);
				current.Use();
				((Renderer)volRenderer).set_realtimeLightmapIndex(-1);
				HideGizmo();
			}
			break;
		case 3:
			if (current.get_button() == 0 && (int)current.get_modifiers() == 0 && (Object)(object)mesh != (Object)null)
			{
				if (Raycast(faces, out var x, out var y, out var z, out var orientation))
				{
					Paint(x, y, z, orientation);
				}
				current.Use();
			}
			break;
		case 1:
			if (current.get_button() == 0 && (int)current.get_modifiers() == 0 && (Object)(object)mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)volCollider.get_sharedMesh(), true);
				volCollider.set_sharedMesh(mesh);
				mesh = null;
				faces = null;
				lastX = int.MinValue;
				lastY = int.MinValue;
				lastZ = int.MinValue;
				current.Use();
				ShowGizmo();
			}
			break;
		case 2:
			if (activeTool == Tool.None || (Object)(object)mesh != (Object)null)
			{
				HideGizmo();
			}
			else
			{
				ShowGizmo();
			}
			HandleUtility.Repaint();
			break;
		case 8:
			HandleUtility.AddDefaultControl(controlID);
			break;
		case 4:
		case 5:
		case 6:
		case 7:
			break;
		}
	}

	private void SetTool(Tool tool, bool toggle = false)
	{
		if (toggle)
		{
			if (activeTool == tool)
			{
				activeTool = Tool.None;
			}
			else
			{
				activeTool = tool;
			}
		}
		else
		{
			activeTool = tool;
		}
		if (activeTool == Tool.None)
		{
			EditorUtility.SetSelectedWireframeHidden((Renderer)(object)volRenderer, false);
			HideGizmo();
		}
		else
		{
			EditorUtility.SetSelectedWireframeHidden((Renderer)(object)volRenderer, true);
			ShowGizmo();
		}
	}

	private bool Raycast(IList<WallVolumeFace> faces, out int x, out int y, out int z, out WallVolumeOrientaion orientation)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit val = default(RaycastHit);
		if (Physics.Raycast(HandleUtility.GUIPointToWorldRay(Event.get_current().get_mousePosition()), ref val) && (Object)(object)((RaycastHit)(ref val)).get_collider() == (Object)(object)volCollider)
		{
			Vector3 val2 = ((Component)volume).get_transform().InverseTransformPoint(((RaycastHit)(ref val)).get_point());
			Vector3 val3 = ((Component)volume).get_transform().InverseTransformVector(((RaycastHit)(ref val)).get_normal());
			val2 /= volume.gridSize;
			if (val3.x > 0.8f)
			{
				orientation = WallVolumeOrientaion.Right;
			}
			else if (val3.x < -0.8f)
			{
				orientation = WallVolumeOrientaion.Left;
			}
			else if (val3.y > 0.8f)
			{
				orientation = WallVolumeOrientaion.Up;
			}
			else if (val3.y < -0.8f)
			{
				orientation = WallVolumeOrientaion.Down;
			}
			else if (val3.z > 0.8f)
			{
				orientation = WallVolumeOrientaion.Forward;
			}
			else
			{
				if (!(val3.z < -0.8f))
				{
					throw new InvalidOperationException();
				}
				orientation = WallVolumeOrientaion.Back;
			}
			brushX = (brushY = (brushZ = 1));
			switch (orientation)
			{
			case WallVolumeOrientaion.Up:
				brushY = height;
				val2.x -= 0.5f;
				val2.z -= 0.5f;
				if (activeTool == Tool.Remove)
				{
					val2.y -= height;
				}
				break;
			case WallVolumeOrientaion.Down:
				brushY = height;
				val2.x -= 0.5f;
				val2.z -= 0.5f;
				if (activeTool == Tool.Add)
				{
					val2.y -= height;
				}
				break;
			case WallVolumeOrientaion.Right:
				brushX = height;
				val2.y -= 0.5f;
				val2.z -= 0.5f;
				if (activeTool == Tool.Remove)
				{
					val2.x -= height;
				}
				break;
			case WallVolumeOrientaion.Left:
				brushX = height;
				val2.y -= 0.5f;
				val2.z -= 0.5f;
				if (activeTool == Tool.Add)
				{
					val2.x -= height;
				}
				break;
			case WallVolumeOrientaion.Forward:
				brushZ = height;
				val2.x -= 0.5f;
				val2.y -= 0.5f;
				if (activeTool == Tool.Remove)
				{
					val2.z -= height;
				}
				break;
			case WallVolumeOrientaion.Back:
				brushZ = height;
				val2.x -= 0.5f;
				val2.y -= 0.5f;
				if (activeTool == Tool.Add)
				{
					val2.z -= height;
				}
				break;
			}
			x = Mathf.RoundToInt(val2.x);
			y = Mathf.RoundToInt(val2.y);
			z = Mathf.RoundToInt(val2.z);
			return true;
		}
		x = (y = (z = 0));
		orientation = WallVolumeOrientaion.Back;
		return false;
	}

	private void ShowGizmo()
	{
		if (Raycast(faces, out var x, out var y, out var z, out var _))
		{
			ShowGizmo(x, y, z);
		}
		else
		{
			HideGizmo();
		}
	}

	private void ShowGizmo(int x, int y, int z)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		volume.gizmoOffset = ((activeTool == Tool.Add) ? (-Vector3.get_one() * 0.005f) : (Vector3.get_one() * 0.005f));
		volume.gizmoStart = new Vector3((float)x, (float)y, (float)z) * volume.gridSize;
		volume.gizmoSize = new Vector3((float)brushX, (float)brushY, (float)brushZ) * volume.gridSize;
		volume.gizmoColor = ((activeTool == Tool.Add) ? new Color(1f, 1f, 0f, 0.7f) : new Color(1f, 0f, 0f, 0.7f));
	}

	private void HideGizmo()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		volume.gizmoSize = Vector3.get_zero();
	}

	private void Paint(int x, int y, int z, WallVolumeOrientaion orientation)
	{
		dirty = true;
		if (lockOrientation == orientation && (((lockOrientation == WallVolumeOrientaion.Up || lockOrientation == WallVolumeOrientaion.Down) && lockY == y) || ((lockOrientation == WallVolumeOrientaion.Left || lockOrientation == WallVolumeOrientaion.Right) && lockX == x) || ((lockOrientation == WallVolumeOrientaion.Forward || lockOrientation == WallVolumeOrientaion.Back) && lockZ == z)) && (lastX != x || lastY != y || lastZ != z))
		{
			lastX = x;
			lastY = y;
			lastZ = z;
			if (activeTool == Tool.Add)
			{
				volume.AddVoxel(x, y, z, brushX, brushY, brushZ);
			}
			else
			{
				volume.RemoveVoxel(x, y, z, brushX, brushY, brushZ);
			}
			volume.FillMesh(mesh, forceExact: false);
			volFilter.set_sharedMesh(mesh);
		}
	}

	public WallVolumeEditor()
		: this()
	{
	}
}
