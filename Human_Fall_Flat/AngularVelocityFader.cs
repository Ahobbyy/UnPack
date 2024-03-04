using System.Collections;
using UnityEngine;

public class AngularVelocityFader : MonoBehaviour
{
	[SerializeField]
	private AudioSource loopAudioSource;

	[SerializeField]
	private AudioSource tailAudioSource;

	[SerializeField]
	private Rigidbody rigidbody;

	[SerializeField]
	private float volumeMultiplier = 0.6f;

	[SerializeField]
	private float magnitudeThresholdForTail = 0.1f;

	[SerializeField]
	private float enableDelay = 3f;

	private bool soundEnabled;

	private float angularVelocityMagnitude;

	private void Awake()
	{
		loopAudioSource.set_loop(true);
		loopAudioSource.set_volume(0f);
		loopAudioSource.set_playOnAwake(true);
	}

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(EnableVelocityBasedSound());
	}

	private IEnumerator EnableVelocityBasedSound()
	{
		yield return (object)new WaitForSeconds(enableDelay);
		soundEnabled = true;
	}

	private void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Vector3 angularVelocity;
		if (soundEnabled)
		{
			if (angularVelocityMagnitude > magnitudeThresholdForTail)
			{
				angularVelocity = rigidbody.get_angularVelocity();
				if (((Vector3)(ref angularVelocity)).get_magnitude() < 0.01f)
				{
					TriggerTailSound();
					angularVelocityMagnitude = 0f;
				}
			}
			AudioSource obj = loopAudioSource;
			angularVelocity = rigidbody.get_angularVelocity();
			obj.set_volume(Mathf.Clamp(((Vector3)(ref angularVelocity)).get_magnitude(), 0f, 1f) * volumeMultiplier);
		}
		angularVelocity = rigidbody.get_angularVelocity();
		angularVelocityMagnitude = ((Vector3)(ref angularVelocity)).get_magnitude();
	}

	private void TriggerTailSound()
	{
		if ((Object)(object)tailAudioSource != (Object)null)
		{
			tailAudioSource.Play();
		}
	}

	public AngularVelocityFader()
		: this()
	{
	}
}
