using System.Collections.Generic;
using UnityEngine;

public class SparkPool : MonoBehaviour
{
	public static SparkPool instance;

	public GameObject template;

	public int minItems;

	public int maxItems = 50;

	private Queue<GameObject> available = new Queue<GameObject>();

	private void OnEnable()
	{
		if ((Object)(object)instance != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).get_gameObject());
			return;
		}
		instance = this;
		for (int i = 1; i < minItems; i++)
		{
			GameObject val = Object.Instantiate<GameObject>(template);
			val.get_transform().SetParent(((Component)this).get_transform(), false);
			available.Enqueue(val);
		}
	}

	public void Emit(Vector3 pos, float speedMult = 3f)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Spark component;
		if (available.Count > 0)
		{
			component = available.Dequeue().GetComponent<Spark>();
		}
		else
		{
			GameObject obj = Object.Instantiate<GameObject>(template);
			obj.get_transform().SetParent(((Component)this).get_transform(), false);
			component = obj.GetComponent<Spark>();
		}
		Vector3 speed = Random.get_insideUnitSphere() * speedMult;
		component.Ignite(pos + Random.get_insideUnitSphere() * 0.1f, speed);
	}

	internal void Return(Spark spark)
	{
		available.Enqueue(((Component)spark).get_gameObject());
	}

	public void Emit(int count, Vector3 position, float speedMult = 3f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < count; i++)
		{
			Emit(position, speedMult);
		}
	}

	public SparkPool()
		: this()
	{
	}
}
