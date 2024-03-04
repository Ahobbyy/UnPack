using System;
using HumanAPI;
using Multiplayer;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class CaptureSceneSaving
{
	static CaptureSceneSaving()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		EditorSceneManager.add_sceneSaving(new SceneSavingCallback(OnSceneSaving));
		EditorApplication.add_playModeStateChanged((Action<PlayModeStateChange>)PlayModeChanged);
	}

	public static void SetupDebugLevel()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		BuiltinLevel builtinLevel = Object.FindObjectOfType<BuiltinLevel>();
		if ((Object)(object)builtinLevel != (Object)null)
		{
			GameObject val = (builtinLevel.gamePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Game.prefab"));
			GameObject val2 = (builtinLevel.resourcePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Resources.prefab"));
			EditorUtility.SetDirty((Object)(object)builtinLevel);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}

	public static void SetupDebugRemove()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		BuiltinLevel builtinLevel = Object.FindObjectOfType<BuiltinLevel>();
		if ((Object)(object)builtinLevel != (Object)null)
		{
			builtinLevel.gamePrefab = null;
			builtinLevel.resourcePrefab = null;
			EditorUtility.SetDirty((Object)(object)builtinLevel);
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
		}
	}

	private static void OnSceneSaving(Scene scene, string path)
	{
		BuiltinLevel builtinLevel = Object.FindObjectOfType<BuiltinLevel>();
		if ((Object)(object)builtinLevel != (Object)null)
		{
			if ((Object)(object)builtinLevel.gamePrefab != (Object)null || (Object)(object)builtinLevel.resourcePrefab != (Object)null)
			{
				SetupDebugRemove();
			}
			if (!NetScenePostporcess.CheckNetIds(report: false))
			{
				NetScenePostporcess.AssignIds();
			}
			AudioSourceAssignMixer.ProcessLevel();
		}
	}

	private static BuiltinLevel GetLevel()
	{
		return Object.FindObjectOfType<BuiltinLevel>();
	}

	private static void PlayModeChanged(PlayModeStateChange state)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		if (!((Object)(object)GetLevel() != (Object)null))
		{
			return;
		}
		if ((int)state != 0)
		{
			if ((int)state == 1)
			{
				SetupDebugLevel();
			}
		}
		else
		{
			SetupDebugRemove();
		}
	}
}
