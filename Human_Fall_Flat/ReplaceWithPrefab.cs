using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplaceWithPrefab : EditorWindow
{
	public GameObject Prefab;

	public GameObject[] ObjectsToReplace;

	public List<GameObject> TempObjects = new List<GameObject>();

	public bool KeepOriginalNames = true;

	public bool EditMode;

	public bool ApplyRotation = true;

	public bool ApplyScale = true;

	public bool KeepPlaceInHeirarchy;

	[MenuItem("Tools/ReplaceWithPrefab")]
	private static void Init()
	{
		((EditorWindow)(ReplaceWithPrefab)(object)EditorWindow.GetWindow(typeof(ReplaceWithPrefab))).Show();
	}

	private void OnSelectionChange()
	{
		GetSelection();
		((EditorWindow)this).Repaint();
	}

	private void CheckParent()
	{
	}

	private void OnGUI()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		EditMode = GUILayout.Toggle(EditMode, "Edit", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUI.get_changed())
		{
			if (EditMode)
			{
				GetSelection();
			}
			else
			{
				ResetPreview();
			}
		}
		KeepOriginalNames = GUILayout.Toggle(KeepOriginalNames, "Keep names", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		ApplyRotation = GUILayout.Toggle(ApplyRotation, "Apply rotation", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		ApplyScale = GUILayout.Toggle(ApplyScale, "Apply scale", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		KeepPlaceInHeirarchy = GUILayout.Toggle(KeepPlaceInHeirarchy, "Keep Place In Heirarchy", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(5f);
		if (EditMode)
		{
			ResetPreview();
			GUI.set_color(Color.get_yellow());
			if ((Object)(object)Prefab != (Object)null)
			{
				GUILayout.Label("Prefab: ", (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Label(((Object)Prefab).get_name(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			else
			{
				GUILayout.Label("No prefab selected", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
			GUI.set_color(Color.get_white());
			GUILayout.Space(5f);
			GUILayout.BeginScrollView(default(Vector2), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GameObject[] objectsToReplace = ObjectsToReplace;
			foreach (GameObject val in objectsToReplace)
			{
				GUILayout.Label(((Object)val).get_name(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if ((Object)(object)Prefab != (Object)null)
				{
					GameObject val2 = (GameObject)PrefabUtility.InstantiatePrefab((Object)(object)Prefab);
					val2.get_transform().SetParent(val.get_transform().get_parent(), true);
					val2.get_transform().set_localPosition(val.get_transform().get_localPosition());
					if (ApplyRotation)
					{
						val2.get_transform().set_localRotation(val.get_transform().get_localRotation());
					}
					if (ApplyScale)
					{
						val2.get_transform().set_localScale(val.get_transform().get_localScale());
					}
					TempObjects.Add(val2);
					if (KeepOriginalNames)
					{
						((Object)val2.get_transform()).set_name(((Object)val.get_transform()).get_name());
					}
					val.SetActive(false);
					if (KeepPlaceInHeirarchy)
					{
						val2.get_transform().SetSiblingIndex(val.get_transform().GetSiblingIndex());
					}
				}
			}
			GUILayout.EndScrollView();
			GUILayout.Space(5f);
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Apply", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				objectsToReplace = ObjectsToReplace;
				for (int i = 0; i < objectsToReplace.Length; i++)
				{
					Object.DestroyImmediate((Object)(object)objectsToReplace[i]);
				}
				EditMode = false;
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
			if (GUILayout.Button("Cancel", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				ResetPreview();
				EditMode = false;
			}
			GUILayout.EndHorizontal();
		}
		else
		{
			ObjectsToReplace = (GameObject[])(object)new GameObject[0];
			TempObjects.Clear();
			Prefab = null;
		}
	}

	private void OnDestroy()
	{
		ResetPreview();
	}

	private void GetSelection()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		if (EditMode && (Object)(object)Selection.get_activeGameObject() != (Object)null)
		{
			if ((int)PrefabUtility.GetPrefabType((Object)(object)Selection.get_activeGameObject()) == 1)
			{
				Prefab = Selection.get_activeGameObject();
				return;
			}
			ResetPreview();
			ObjectsToReplace = Selection.get_gameObjects();
		}
	}

	private void ResetPreview()
	{
		if (TempObjects != null)
		{
			foreach (GameObject tempObject in TempObjects)
			{
				Object.DestroyImmediate((Object)(object)tempObject);
			}
		}
		GameObject[] objectsToReplace = ObjectsToReplace;
		for (int i = 0; i < objectsToReplace.Length; i++)
		{
			objectsToReplace[i].SetActive(true);
		}
		TempObjects.Clear();
	}

	public ReplaceWithPrefab()
		: this()
	{
	}
}
