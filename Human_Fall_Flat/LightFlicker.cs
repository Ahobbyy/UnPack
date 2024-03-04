using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
	[SerializeField]
	private float speed1;

	[SerializeField]
	private float speed2;

	[SerializeField]
	private float speed3;

	[SerializeField]
	private float minIntesity;

	[SerializeField]
	private float maxIntesity;

	[SerializeField]
	private float minRange;

	[SerializeField]
	private float maxRange;

	private Light light;

	private void Start()
	{
		light = ((Component)this).GetComponent<Light>();
	}

	private void Update()
	{
		float num = Mathf.Sin(Time.get_time() * speed1);
		float num2 = Mathf.Sin(Time.get_time() * speed2);
		float num3 = Mathf.Sin(Time.get_time() * speed3);
		float intensity = Mathf.Lerp(minIntesity, maxIntesity, (num + num2 + num3) * 0.333f);
		float intensity2 = Mathf.Lerp(minIntesity, maxIntesity, (num + num2 + num3) * 0.333f);
		light.set_intensity(intensity);
		light.set_intensity(intensity2);
	}

	public LightFlicker()
		: this()
	{
	}
}
