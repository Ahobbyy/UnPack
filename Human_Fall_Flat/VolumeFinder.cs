using UnityEngine;

public class VolumeFinder : MonoBehaviour
{
	private enum VolumeType
	{
		Custom,
		Box,
		Sphere
	}

	[SerializeField]
	public float volume;

	[SerializeField]
	private VolumeType shapeType;

	private void Start()
	{
		if (shapeType != 0)
		{
			CalculateVolume();
		}
	}

	public void CalculateVolume()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (shapeType == VolumeType.Sphere)
		{
			volume = 4.18879032f * Mathf.Pow(((Component)this).get_transform().get_localScale().x * 0.5f, 3f);
		}
		if (shapeType == VolumeType.Box)
		{
			volume = ((Component)this).get_transform().get_localScale().x * ((Component)this).get_transform().get_localScale().y * ((Component)this).get_transform().get_localScale().z;
		}
	}

	public VolumeFinder()
		: this()
	{
	}
}
