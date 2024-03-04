using UnityEngine;

public class Spark : MonoBehaviour
{
	public Color coldColor = Color.get_red();

	public Color hotColor = Color.get_yellow();

	public Color emitColor = Color.get_yellow();

	public float emitIntensity = 2f;

	private Material material;

	private Light light;

	private float lifetime = 1f;

	private void Awake()
	{
		MeshRenderer component = ((Component)this).GetComponent<MeshRenderer>();
		material = ((Renderer)component).get_material();
		((Renderer)component).set_sharedMaterial(material);
		light = ((Component)this).GetComponent<Light>();
	}

	private void LateUpdate()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		lifetime -= Time.get_deltaTime();
		if (lifetime <= 0.2f)
		{
			((Component)this).get_gameObject().SetActive(false);
			SparkPool.instance.Return(this);
			return;
		}
		float num = lifetime * emitIntensity;
		Color color = Color.Lerp(coldColor, hotColor, lifetime);
		material.SetColor("_EmissionColor", emitColor * num);
		material.set_color(color);
		light.set_color(emitColor * num);
	}

	internal void Ignite(Vector3 pos, Vector3 speed)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_gameObject().SetActive(true);
		((Component)this).get_transform().set_position(pos);
		((Component)this).GetComponent<Rigidbody>().set_velocity(speed);
		((Component)this).GetComponent<Rigidbody>().set_angularVelocity(Random.get_insideUnitSphere() * 360f);
		lifetime = Random.Range(0.8f, 1.2f);
	}

	public Spark()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_0017: Unknown result type (might be due to invalid IL or missing references)
	//IL_001c: Unknown result type (might be due to invalid IL or missing references)

}
