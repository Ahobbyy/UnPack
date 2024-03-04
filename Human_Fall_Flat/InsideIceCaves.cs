using System.Collections.Generic;
using UnityEngine;

public class InsideIceCaves : MonoBehaviour
{
	private const int kMaxSnowballs = 4;

	public static InsideIceCaves instance;

	private static List<SnowBallGrowth> snowballsInside = new List<SnowBallGrowth>(4);

	private void Awake()
	{
		if ((Object)(object)instance == (Object)null)
		{
			instance = this;
		}
	}

	public bool SnowballInsideCave(SnowBallGrowth snowball)
	{
		return snowballsInside.Contains(snowball);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.get_isTrigger())
		{
			SnowBallGrowth component = ((Component)other).get_gameObject().GetComponent<SnowBallGrowth>();
			if ((Object)(object)component != (Object)null)
			{
				snowballsInside.Add(component);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.get_isTrigger())
		{
			SnowBallGrowth component = ((Component)other).get_gameObject().GetComponent<SnowBallGrowth>();
			if ((Object)(object)component != (Object)null)
			{
				snowballsInside.Remove(component);
			}
		}
	}

	public InsideIceCaves()
		: this()
	{
	}
}
