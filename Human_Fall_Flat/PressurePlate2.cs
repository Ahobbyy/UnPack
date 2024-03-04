using UnityEngine;

public class PressurePlate2 : SignalBase
{
	public Transform sensor;

	public float pressTreshold = 0.06f;

	public float releaseTreshold = 0.12f;

	public float holdState = 1f;

	private float timer;

	private void Start()
	{
		Rigidbody component = ((Component)sensor).GetComponent<Rigidbody>();
		component.set_sleepThreshold(component.get_sleepThreshold() * 0.1f);
	}

	private void Update()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		timer -= Time.get_deltaTime();
		if (base.boolValue && sensor.get_localPosition().y > releaseTreshold && timer <= 0f)
		{
			SetValue(0f);
			timer = holdState;
		}
		if (!base.boolValue && sensor.get_localPosition().y < pressTreshold && timer <= 0f)
		{
			SetValue(1f);
			timer = holdState;
		}
	}
}
