using UnityEditor;
using UnityEngine;

public class PauseOnLag : MonoBehaviour
{
	private void Update()
	{
		if (Time.get_deltaTime() > 0.2f)
		{
			EditorApplication.set_isPaused(true);
		}
	}

	public PauseOnLag()
		: this()
	{
	}
}
