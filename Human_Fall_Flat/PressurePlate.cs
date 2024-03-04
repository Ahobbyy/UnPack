using UnityEngine;

public class PressurePlate : MonoBehaviour
{
	public Transform sensor;

	public float pressTreshold = 0.06f;

	public float releaseTreshold = 0.12f;

	public bool isPressed;

	public float holdState = 1f;

	private float timer;

	private void Update()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		timer -= Time.get_deltaTime();
		if (isPressed && sensor.get_localPosition().y > releaseTreshold && timer <= 0f)
		{
			isPressed = false;
			timer = holdState;
		}
		if (!isPressed && sensor.get_localPosition().y < pressTreshold && timer <= 0f)
		{
			isPressed = true;
			timer = holdState;
		}
	}

	public PressurePlate()
		: this()
	{
	}
}
