using System.Collections.Generic;
using UnityEngine;

public class GrabManager : MonoBehaviour
{
	public List<GameObject> grabbedObjects = new List<GameObject>();

	private static Dictionary<GameObject, Vector3> grabStartPositions = new Dictionary<GameObject, Vector3>();

	private static List<GrabManager> all = new List<GrabManager>();

	private Human human;

	public Human Human => human;

	public bool hasGrabbed => grabbedObjects.Count > 0;

	private void OnEnable()
	{
		all.Add(this);
		human = ((Component)this).GetComponentInParent<Human>();
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	public void ObjectGrabbed(GameObject grabObject)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		for (int i = 0; i < all.Count; i++)
		{
			flag &= !all[i].grabbedObjects.Contains(grabObject);
		}
		grabbedObjects.Add(grabObject);
		if (flag)
		{
			grabObject.GetComponentInParent<IGrabbable>()?.OnGrab();
			grabObject.GetComponentInParent<IGrabbableWithInfo>()?.OnGrab(this);
			grabStartPositions[grabObject] = grabObject.get_transform().get_position();
			Human componentInParent = grabObject.GetComponentInParent<Human>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				componentInParent.grabbedByHuman = human;
			}
		}
	}

	private void CheckCarryEnd(GameObject grabObject)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		for (int i = 0; i < all.Count; i++)
		{
			flag &= !all[i].grabbedObjects.Contains(grabObject);
		}
		if (!flag)
		{
			return;
		}
		grabObject.GetComponentInParent<IGrabbable>()?.OnRelease();
		grabObject.GetComponentInParent<IGrabbableWithInfo>()?.OnRelease(this);
		if ((Object)(object)grabObject != (Object)null && grabStartPositions.ContainsKey(grabObject))
		{
			Vector2 val = (grabStartPositions[grabObject] - grabObject.get_transform().get_position()).To2D();
			float magnitude = ((Vector2)(ref val)).get_magnitude();
			if (magnitude > 0.1f)
			{
				StatsAndAchievements.AddCarry(human, magnitude);
			}
		}
		grabStartPositions.Remove(grabObject);
		if (!CheatCodes.throwCheat)
		{
			Human componentInParent = grabObject.GetComponentInParent<Human>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				componentInParent.grabbedByHuman = null;
			}
		}
	}

	public void ObjectReleased(GameObject grabObject)
	{
		grabbedObjects.Remove(grabObject);
		CheckCarryEnd(grabObject);
	}

	public static void Release(GameObject item, float delay = 0f)
	{
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human.all[i].ReleaseGrab(item, delay);
		}
	}

	public static bool IsGrabbedAny(GameObject item)
	{
		for (int i = 0; i < all.Count; i++)
		{
			if (all[i].IsGrabbed(item))
			{
				return true;
			}
		}
		return false;
	}

	public static Human GrabbedBy(GameObject item)
	{
		for (int i = 0; i < all.Count; i++)
		{
			if (all[i].IsGrabbed(item))
			{
				return all[i].human;
			}
		}
		return null;
	}

	public bool IsGrabbed(GameObject item)
	{
		for (int num = grabbedObjects.Count - 1; num >= 0; num--)
		{
			GameObject val = grabbedObjects[num];
			if (!((Object)(object)val == (Object)null))
			{
				Transform val2 = val.get_transform();
				while ((Object)(object)val2 != (Object)null)
				{
					if ((Object)(object)((Component)val2).get_gameObject() == (Object)(object)item)
					{
						return true;
					}
					val2 = val2.get_parent();
				}
			}
		}
		return false;
	}

	public void DistributeForce(Vector3 force)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < grabbedObjects.Count; i++)
		{
			Rigidbody componentInParent = grabbedObjects[i].GetComponentInParent<Rigidbody>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				componentInParent.SafeAddForce(force / (float)grabbedObjects.Count, (ForceMode)0);
			}
		}
	}

	internal void Reset()
	{
		GameObject[] array = grabbedObjects.ToArray();
		grabbedObjects.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			GameObject val = array[0];
			if ((Object)(object)val != (Object)null)
			{
				CheckCarryEnd(val);
			}
		}
	}

	public GrabManager()
		: this()
	{
	}
}
