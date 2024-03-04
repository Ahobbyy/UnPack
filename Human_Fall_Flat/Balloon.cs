using HumanAPI;
using Multiplayer;
using UnityEngine;

public class Balloon : Node
{
	private Renderer rend;

	private Light light;

	private float startingLightIntensity;

	private Color startingEmision;

	public Color myColor = Color.get_white();

	public NodeInput r;

	public NodeInput g;

	public NodeInput b;

	public float explodeThreshold = 0.5f;

	public float respawnTime = 1f;

	public float explodeTime = 0.5f;

	private NetBody body;

	private void Awake()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		rend = ((Component)this).GetComponent<Renderer>();
		light = ((Component)this).GetComponent<Light>();
		startingEmision = rend.get_material().GetColor("_EmissionColor");
	}

	public override void Process()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		base.Process();
		ConsumeLight(new Color(r.value, g.value, b.value));
	}

	private void ConsumeLight(Color c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		Color val = myColor - c;
		if (val.r < 0f - explodeThreshold || val.g < 0f - explodeThreshold || val.b < 0f - explodeThreshold)
		{
			Explode();
		}
		else
		{
			UpdateLight(val);
		}
	}

	private void UpdateLight(Color dif)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		float num = dif.r + dif.g + dif.b;
		float num2 = myColor.r + myColor.g + myColor.b;
		float num3 = num / num2;
		rend.get_material().SetColor("_EmissionColor", Color.Lerp(Color.get_black(), startingEmision, num3));
		light.set_intensity(Mathf.Lerp(0f, startingLightIntensity, num3));
	}

	private void Explode()
	{
		((MonoBehaviour)this).Invoke("Respawn", respawnTime);
		((Component)this).get_gameObject().SetActive(false);
	}

	private void Respawn()
	{
		((Component)this).get_gameObject().SetActive(true);
		body.Respawn();
	}
}
