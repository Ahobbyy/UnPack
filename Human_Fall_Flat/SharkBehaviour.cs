using System.Collections;
using HumanAPI;
using UnityEngine;

public class SharkBehaviour : Node
{
	public NodeInput enraged;

	public Rigidbody[] m_Tail;

	public Rigidbody[] front;

	public float flopStrength = 1000f;

	public float flopCooldownMin = 1f;

	public float flopCooldownMax = 4f;

	private SharkState m_State = SharkState.Ground;

	private IEnumerator currentState;

	private Rigidbody body;

	public WaterSensor sensor;

	private float swimSpeed = 10f;

	public float multiplier = 10f;

	public float maxTailAngle = 30f;

	private FloatingMesh1[] parts;

	private float flopCooldown;

	private Vector3 startingDir;

	private void Start()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		body = ((Component)this).GetComponent<Rigidbody>();
		ChangeSharkState(m_State);
		if ((Object)(object)sensor == (Object)null)
		{
			sensor = ((Component)this).GetComponentInChildren<WaterSensor>();
		}
		parts = ((Component)this).GetComponentsInChildren<FloatingMesh1>();
		startingDir = ((Component)m_Tail[m_Tail.Length - 1]).get_transform().get_forward();
	}

	private void FixedUpdate()
	{
		SharkState sharkState = ((!((Object)(object)sensor.waterBody != (Object)null)) ? SharkState.Ground : SharkState.Water);
		if (m_State != sharkState)
		{
			ChangeSharkState(sharkState);
		}
		if (currentState != null)
		{
			currentState.MoveNext();
		}
		ApplyForces();
	}

	private void ApplyForces()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.get_zero();
		Vector3.get_zero();
		Vector3.Angle(((Component)m_Tail[m_Tail.Length - 1]).get_transform().get_forward(), startingDir);
		switch (m_State)
		{
		case SharkState.Ground:
			if (!(flopCooldown > Time.get_time()))
			{
				flopCooldown = Time.get_time() + Random.Range(flopCooldownMin, flopCooldownMax);
			}
			break;
		case SharkState.Water:
			zero += ((Component)sensor).get_transform().get_forward() * swimSpeed;
			break;
		}
	}

	private void ChangeSharkState(SharkState state)
	{
		switch (state)
		{
		case SharkState.Water:
			currentState = SwimState();
			break;
		case SharkState.Ground:
			currentState = FlopState();
			break;
		}
		m_State = state;
	}

	private IEnumerator SwimState()
	{
		int dir = 1;
		int tailFrames = 0;
		while (true)
		{
			float num = (float)dir * (enraged.value * 3f + 0.3f) * flopStrength;
			if (tailFrames < 100)
			{
				tailFrames++;
			}
			else
			{
				dir = -dir;
				tailFrames = 0;
			}
			Vector3 val = num * ((Component)this).get_transform().get_up();
			Rigidbody[] tail = m_Tail;
			for (int i = 0; i < tail.Length; i++)
			{
				tail[i].AddTorque(val, (ForceMode)0);
			}
			if (enraged.value > 0f)
			{
				((Component)this).get_transform().InverseTransformVector(front[0].get_velocity());
				front[0].AddForce(-((Component)front[0]).get_transform().get_up() * enraged.value * multiplier, (ForceMode)1);
			}
			yield return (object)new WaitForFixedUpdate();
		}
	}

	private IEnumerator FlopState()
	{
		flopCooldown = Time.get_time() + flopCooldownMax;
		while (Time.get_time() < flopCooldown)
		{
			yield return (object)new WaitForEndOfFrame();
		}
		while (true)
		{
			float num = flopStrength * (float)(Random.Range(0, 2) * 2 - 1);
			Vector3 dir2 = num * ((Component)this).get_transform().get_up();
			for (int j = 0; j < 20; j++)
			{
				Rigidbody[] array = front;
				for (int k = 0; k < array.Length; k++)
				{
					array[k].AddTorque(-dir2, (ForceMode)1);
				}
				array = m_Tail;
				for (int k = 0; k < array.Length; k++)
				{
					array[k].AddTorque(dir2, (ForceMode)1);
				}
				yield return (object)new WaitForFixedUpdate();
			}
			dir2 *= -0.5f;
			for (int j = 0; j < 20; j++)
			{
				Rigidbody[] array = front;
				for (int k = 0; k < array.Length; k++)
				{
					array[k].AddTorque(-dir2, (ForceMode)1);
				}
				array = m_Tail;
				for (int k = 0; k < array.Length; k++)
				{
					array[k].AddTorque(dir2, (ForceMode)1);
				}
				yield return (object)new WaitForFixedUpdate();
			}
			flopCooldown = Time.get_time() + Random.Range(flopCooldownMin, flopCooldownMax);
			while (Time.get_time() < flopCooldown)
			{
				yield return (object)new WaitForEndOfFrame();
			}
		}
	}
}
