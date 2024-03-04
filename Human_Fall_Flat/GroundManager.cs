using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
	public List<GameObject> groundObjects = new List<GameObject>();

	private List<Rigidbody> groundRigids = new List<Rigidbody>();

	private static List<GroundManager> all = new List<GroundManager>();

	private static Dictionary<GroundVehicle, Vector3> vehicleStartPositions = new Dictionary<GroundVehicle, Vector3>();

	private static Dictionary<FloatingMesh, Vector3> shipStartPositions = new Dictionary<FloatingMesh, Vector3>();

	private List<GameObject> removedObjects = new List<GameObject>();

	public float surfaceAngle;

	public bool onGround => groundObjects.Count > 0;

	public Vector3 groudSpeed
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			Vector3 zero = Vector3.get_zero();
			for (int i = 0; i < groundRigids.Count; i++)
			{
				Rigidbody val = groundRigids[i];
				if ((Object)(object)val != (Object)null)
				{
					Vector3 velocity = val.get_velocity();
					if (Mathf.Abs(zero.x) < Mathf.Abs(velocity.x))
					{
						zero.x = velocity.x;
					}
					if (Mathf.Abs(zero.y) < Mathf.Abs(velocity.y))
					{
						zero.y = velocity.y;
					}
					if (Mathf.Abs(zero.z) < Mathf.Abs(velocity.z))
					{
						zero.z = velocity.z;
					}
				}
			}
			return zero;
		}
	}

	private void OnEnable()
	{
		all.Add(this);
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	public void PostFixedUpdate()
	{
		for (int i = 0; i < removedObjects.Count; i++)
		{
			if (shipStartPositions.Count > 0)
			{
				CheckDriveEnd(removedObjects[i], shipStartPositions);
			}
			if (vehicleStartPositions.Count > 0)
			{
				CheckDriveEnd(removedObjects[i], vehicleStartPositions);
			}
		}
		List<GameObject> list = removedObjects;
		removedObjects = groundObjects;
		groundObjects = list;
		groundObjects.Clear();
		groundRigids.Clear();
	}

	public void ObjectEnter(GameObject groundObject)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (!groundObjects.Contains(groundObject))
		{
			removedObjects.Remove(groundObject);
			groundObjects.Add(groundObject);
			groundRigids.Add(groundObject.GetComponentInParent<Rigidbody>());
			FloatingMesh componentInParent = groundObject.GetComponentInParent<FloatingMesh>();
			if ((Object)(object)componentInParent != (Object)null && !shipStartPositions.ContainsKey(componentInParent))
			{
				shipStartPositions[componentInParent] = groundObject.get_transform().get_position();
			}
			GroundVehicle componentInParent2 = groundObject.GetComponentInParent<GroundVehicle>();
			if ((Object)(object)componentInParent2 != (Object)null && !vehicleStartPositions.ContainsKey(componentInParent2))
			{
				vehicleStartPositions[componentInParent2] = groundObject.get_transform().get_position();
			}
		}
	}

	private void CheckDriveUpdate<T>(Dictionary<T, Vector3> startPositions) where T : MonoBehaviour
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < groundObjects.Count; i++)
		{
			GameObject val = groundObjects[i];
			if ((Object)(object)val == (Object)null)
			{
				continue;
			}
			T componentInParent = val.GetComponentInParent<T>();
			if (!((Object)(object)componentInParent != (Object)null) || !startPositions.ContainsKey(componentInParent))
			{
				continue;
			}
			Vector2 val2 = (startPositions[componentInParent] - val.get_transform().get_position()).To2D();
			float magnitude = ((Vector2)(ref val2)).get_magnitude();
			if (magnitude > 5f)
			{
				if (typeof(T) == typeof(FloatingMesh))
				{
					StatsAndAchievements.AddShip(magnitude);
				}
				else
				{
					StatsAndAchievements.AddDrive(magnitude);
				}
				startPositions[componentInParent] = val.get_transform().get_position();
			}
		}
	}

	public void Update()
	{
		if (shipStartPositions.Count > 0)
		{
			CheckDriveUpdate(shipStartPositions);
		}
		if (vehicleStartPositions.Count > 0)
		{
			CheckDriveUpdate(vehicleStartPositions);
		}
	}

	private static void CheckDriveEnd<T>(GameObject groundObject, Dictionary<T, Vector3> startPositions) where T : MonoBehaviour
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		T componentInParent = groundObject.GetComponentInParent<T>();
		if (!((Object)(object)componentInParent != (Object)null) || !startPositions.ContainsKey(componentInParent))
		{
			return;
		}
		bool flag = true;
		for (int i = 0; i < all.Count; i++)
		{
			for (int j = 0; j < all[i].groundObjects.Count; j++)
			{
				GameObject val = all[i].groundObjects[j];
				if (!((Object)(object)val == (Object)null) && val.get_transform().IsChildOf(((Component)(object)componentInParent).get_transform()))
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		Vector2 val2 = (startPositions[componentInParent] - groundObject.get_transform().get_position()).To2D();
		float magnitude = ((Vector2)(ref val2)).get_magnitude();
		if (magnitude > 0f)
		{
			if (typeof(T) == typeof(FloatingMesh))
			{
				StatsAndAchievements.AddShip(magnitude);
			}
			else
			{
				StatsAndAchievements.AddDrive(magnitude);
			}
		}
		startPositions.Remove(componentInParent);
	}

	public static bool IsStandingAny(GameObject item)
	{
		for (int i = 0; i < all.Count; i++)
		{
			if (all[i].IsStanding(item))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsStanding(GameObject item)
	{
		for (int num = groundObjects.Count - 1; num >= 0; num--)
		{
			GameObject val = groundObjects[num];
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

	public void DistributeForce(Vector3 force, Vector3 pos)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < groundRigids.Count; i++)
		{
			Rigidbody val = groundRigids[i];
			if ((Object)(object)val != (Object)null)
			{
				val.SafeAddForceAtPosition(Vector3.ClampMagnitude(force / (float)groundRigids.Count, val.get_mass() / Time.get_fixedDeltaTime() * 10f), pos, (ForceMode)0);
			}
		}
	}

	internal static void ResetOnSceneUnload()
	{
		foreach (GroundManager item in all)
		{
			item.groundRigids.Clear();
			item.groundObjects.Clear();
		}
		vehicleStartPositions.Clear();
		shipStartPositions.Clear();
	}

	internal void Reset()
	{
		GameObject[] array = groundObjects.ToArray();
		groundObjects.Clear();
		groundRigids.Clear();
		for (int i = 0; i < array.Length; i++)
		{
			GameObject val = array[0];
			if ((Object)(object)val != (Object)null)
			{
				CheckDriveEnd(val, shipStartPositions);
				CheckDriveEnd(val, vehicleStartPositions);
			}
		}
	}

	public void ReportSurfaceAngle(float surfaceAngle)
	{
		this.surfaceAngle = Mathf.Min(surfaceAngle, this.surfaceAngle);
	}

	internal void DecaySurfaceAngle()
	{
		surfaceAngle = Mathf.Min(90f, surfaceAngle + 90f * Time.get_fixedDeltaTime());
	}

	public GroundManager()
		: this()
	{
	}
}
