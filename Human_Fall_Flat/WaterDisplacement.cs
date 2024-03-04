using UnityEngine;

public class WaterDisplacement : MonoBehaviour
{
	private float scaleTarget = 1f;

	[SerializeField]
	private Transform waterLevel;

	[SerializeField]
	private float displacementMultiplier = 1f;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		scaleTarget = ((Component)this).get_transform().get_localScale().y;
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Lerp(((Component)this).get_transform().get_localScale().y, scaleTarget, 2.5f * Time.get_deltaTime());
		((Component)this).get_transform().set_localScale(new Vector3(((Component)this).get_transform().get_localScale().x, num, ((Component)this).get_transform().get_localScale().z));
		waterLevel.set_position(new Vector3(waterLevel.get_position().x, ((Component)this).get_transform().get_position().y + ((Component)this).get_transform().get_localScale().y, waterLevel.get_position().z));
	}

	private void OnTriggerEnter(Collider collision)
	{
		Debug.Log((object)"Water Triggered");
		if (Object.op_Implicit((Object)(object)((Component)collision).get_gameObject().GetComponent<VolumeFinder>()))
		{
			float volume = ((Component)collision).get_gameObject().GetComponent<VolumeFinder>().volume;
			AddVolume(volume);
		}
	}

	private void OnTriggerExit(Collider collision)
	{
		Debug.Log((object)"Water Triggered");
		if (Object.op_Implicit((Object)(object)((Component)collision).get_gameObject().GetComponent<VolumeFinder>()))
		{
			float volume = ((Component)collision).get_gameObject().GetComponent<VolumeFinder>().volume;
			AddVolume(0f - volume);
		}
	}

	private void AddVolume(float volume2Add)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		scaleTarget = displacementMultiplier * volume2Add / ((Component)this).get_transform().get_localScale().x / ((Component)this).get_transform().get_localScale().z + ((Component)this).get_transform().get_localScale().y;
		Debug.Log((object)("Water rose to " + scaleTarget));
	}

	public WaterDisplacement()
		: this()
	{
	}
}
