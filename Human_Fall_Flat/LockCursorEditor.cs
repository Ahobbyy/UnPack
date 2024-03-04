using UnityEngine;

public class LockCursorEditor : MonoBehaviour
{
	[SerializeField]
	private bool LockCursor = true;

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
			if (Cursor.get_visible())
			{
				Cursor.set_lockState((CursorLockMode)0);
			}
			else
			{
				Cursor.set_lockState((CursorLockMode)1);
			}
		}
	}

	public LockCursorEditor()
		: this()
	{
	}
}
