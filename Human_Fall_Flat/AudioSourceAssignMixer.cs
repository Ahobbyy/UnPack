using HumanAPI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public static class AudioSourceAssignMixer
{
	private static AudioMixerGroup GetDefaultAudioMixer()
	{
		AudioMixerGroup[] array = AssetDatabase.LoadAssetAtPath<AudioMixer>("Assets/Audio/Mixers/MasterMixer.mixer").FindMatchingGroups("Effects");
		if (array.Length == 0)
		{
			Debug.LogError((object)"Can't find effect mixer group");
			return null;
		}
		return array[0];
	}

	public static void ProcessLevel()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Object.FindObjectOfType<BuiltinLevel>() != (Object)null)
		{
			AudioMixerGroup defaultAudioMixer = GetDefaultAudioMixer();
			if (!((Object)(object)defaultAudioMixer == (Object)null) && ProcessAudioSources(Object.FindObjectsOfType<AudioSource>(), defaultAudioMixer))
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	private static bool ProcessAudioSources(AudioSource[] audioSources, AudioMixerGroup effectsMixer, string prefabPath = "")
	{
		bool result = false;
		if (audioSources.Length != 0)
		{
			foreach (AudioSource val in audioSources)
			{
				if ((Object)(object)val.get_outputAudioMixerGroup() == (Object)null)
				{
					val.set_outputAudioMixerGroup(effectsMixer);
					Debug.Log((object)("null mixer: " + ((Object)val).get_name() + " " + prefabPath), (Object)(object)val);
					EditorUtility.SetDirty((Object)(object)((Component)val).get_gameObject());
					result = true;
				}
			}
		}
		return result;
	}

	private static void ProcessPrefabs()
	{
		string[] array = AssetDatabase.FindAssets("t:Prefab");
		AudioMixerGroup defaultAudioMixer = GetDefaultAudioMixer();
		if ((Object)(object)defaultAudioMixer == (Object)null)
		{
			return;
		}
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string text = AssetDatabase.GUIDToAssetPath(array2[i]);
			GameObject val = AssetDatabase.LoadAssetAtPath<GameObject>(text);
			if ((Object)(object)val != (Object)null)
			{
				ProcessAudioSources(val.GetComponentsInChildren<AudioSource>(), defaultAudioMixer, text);
			}
		}
		AssetDatabase.SaveAssets();
	}
}
