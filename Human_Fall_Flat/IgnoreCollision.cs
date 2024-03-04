using UnityEngine;

public class IgnoreCollision : MonoBehaviour
{
	[Tooltip("List of the colliders to ignore")]
	public Collider[] collidersToIgnore;

	[Tooltip("Whether the check is recursive or not")]
	public bool recursive = true;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void Start()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Started "));
		}
		if (recursive)
		{
			Collider[] componentsInChildren = ((Component)this).GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Ignore(componentsInChildren[i]);
			}
		}
		else
		{
			Collider component = ((Component)this).GetComponent<Collider>();
			Ignore(component);
		}
	}

	private void Ignore(Collider collider)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Ignoring "));
		}
		if (collidersToIgnore == null && !((Object)(object)collider != (Object)null))
		{
			return;
		}
		for (int i = 0; i < collidersToIgnore.Length; i++)
		{
			if ((Object)(object)collider != (Object)null)
			{
				if ((Object)(object)collidersToIgnore[i] == (Object)null)
				{
					Debug.LogWarning((object)"Colliders to ignore has null objects in list", (Object)(object)this);
				}
				else
				{
					Physics.IgnoreCollision(collider, collidersToIgnore[i], true);
				}
			}
		}
	}

	public static void Ignore(Transform t1, Transform t2)
	{
		Collider[] componentsInChildren = ((Component)t1).GetComponentsInChildren<Collider>();
		Collider[] componentsInChildren2 = ((Component)t2).GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Physics.IgnoreCollision(componentsInChildren[i], componentsInChildren2[j], true);
			}
		}
	}

	public static void Unignore(Transform t1, Transform t2)
	{
		Collider[] componentsInChildren = ((Component)t1).GetComponentsInChildren<Collider>();
		Collider[] componentsInChildren2 = ((Component)t2).GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				Physics.IgnoreCollision(componentsInChildren[i], componentsInChildren2[j], false);
			}
		}
	}

	public IgnoreCollision()
		: this()
	{
	}
}
