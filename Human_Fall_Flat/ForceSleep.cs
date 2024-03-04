using UnityEngine;

public class ForceSleep : MonoBehaviour
{
	public float sleepSpeed = 0.05f;

	public int sleepFrames = 30;

	private Vector3[] positions;

	private Rigidbody[] bodies;

	private int frames;

	private void Awake()
	{
		bodies = ((Component)this).GetComponentsInChildren<Rigidbody>();
		positions = (Vector3[])(object)new Vector3[bodies.Length];
	}

	private void FixedUpdate()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		float num = sleepSpeed * sleepSpeed * Time.get_fixedDeltaTime() * Time.get_fixedDeltaTime();
		for (int i = 0; i < bodies.Length; i++)
		{
			Vector3 position = bodies[i].get_position();
			if (!flag)
			{
				Vector3 val = position - positions[i];
				if (((Vector3)(ref val)).get_sqrMagnitude() > num)
				{
					flag = true;
				}
			}
			positions[i] = position;
		}
		if (flag)
		{
			frames = 0;
			return;
		}
		frames++;
		if (frames < sleepFrames)
		{
			return;
		}
		for (int j = 0; j < bodies.Length; j++)
		{
			if (!bodies[j].IsSleeping())
			{
				bodies[j].Sleep();
			}
		}
	}

	public ForceSleep()
		: this()
	{
	}
}
