using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MovingAudioSource : MonoBehaviour
{
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private float maxVol = 1f;

	private Vector3 lastPositionFrame;

	private float distanceSinceLastFrame;

	private float targetVol;

	private void Start()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		audioSource.set_pitch(Random.Range(0.95f, 1.05f));
		audioSource.set_time(Random.Range(0f, 1f));
		lastPositionFrame = ((Component)this).get_transform().get_position();
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		distanceSinceLastFrame = Vector3.Distance(((Component)this).get_transform().get_position(), lastPositionFrame);
		if (distanceSinceLastFrame > 0.1f)
		{
			targetVol = Mathf.Clamp(distanceSinceLastFrame * 2f, 0f, maxVol);
		}
		else
		{
			targetVol = 0f;
		}
		lastPositionFrame = ((Component)this).get_transform().get_position();
		if (audioSource.get_volume() > targetVol)
		{
			audioSource.set_volume(audioSource.get_volume() - (audioSource.get_volume() - targetVol) * 0.2f);
		}
		else
		{
			audioSource.set_volume(audioSource.get_volume() + (targetVol - audioSource.get_volume()) * 0.2f);
		}
	}

	public MovingAudioSource()
		: this()
	{
	}
}
