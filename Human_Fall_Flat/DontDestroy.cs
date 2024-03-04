using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	public void OnEnable()
	{
		Object.DontDestroyOnLoad((Object)(object)((Component)this).get_gameObject());
	}

	public DontDestroy()
		: this()
	{
	}
}
