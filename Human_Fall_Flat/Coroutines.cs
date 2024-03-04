using System.Collections;
using UnityEngine;

public class Coroutines : MonoBehaviour
{
	private static Coroutines instance;

	private static void EnsureInstance()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		if ((Object)(object)instance == (Object)null)
		{
			GameObject val = new GameObject("Coroutines");
			instance = val.AddComponent<Coroutines>();
			Object.DontDestroyOnLoad((Object)val);
		}
	}

	public static Coroutine StartGlobalCoroutine(IEnumerator enumerator)
	{
		EnsureInstance();
		return ((MonoBehaviour)instance).StartCoroutine(enumerator);
	}

	public static void StopGlobalCoroutine(Coroutine coroutine)
	{
		if ((Object)(object)instance != (Object)null)
		{
			((MonoBehaviour)instance).StopCoroutine(coroutine);
		}
	}

	public Coroutines()
		: this()
	{
	}
}
