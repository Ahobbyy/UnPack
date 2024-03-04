using UnityEngine;

public class LockCursorInEditor : MonoBehaviour
{
	public bool LockCursor = true;

	private void Start()
	{
		Cursor.set_lockState((CursorLockMode)1);
		Cursor.set_visible(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown("l"))
		{
			Cursor.set_visible(!Cursor.get_visible());
			Cursor.set_lockState((CursorLockMode)((!Cursor.get_visible()) ? 1 : 0));
		}
	}

	public LockCursorInEditor()
		: this()
	{
	}
}
