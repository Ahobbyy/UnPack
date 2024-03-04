using System.Collections;
using UnityEngine;

public class LinearVelocityFader : MonoBehaviour
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

	private float previousVelocity;

	public bool debug;

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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		Vector3 velocity;
		if (soundEnabled)
		{
			if (debug)
			{
				string text = previousVelocity.ToString();
				velocity = rigidbody.get_velocity();
				Debug.Log((object)("<color=green> previousVelocity=</color>" + text + " currentVelocity=" + ((Vector3)(ref velocity)).get_magnitude()));
			}
			if (previousVelocity > magnitudeThresholdForTail)
			{
				velocity = rigidbody.get_velocity();
				if (((Vector3)(ref velocity)).get_magnitude() < 0.0001f)
				{
					if (debug)
					{
						Debug.Log((object)"<color=green> Triggered Tail Sound </color>");
					}
					TriggerTailSound();
					previousVelocity = 0f;
				}
			}
			AudioSource obj = loopAudioSource;
			velocity = rigidbody.get_velocity();
			obj.set_volume(Mathf.Clamp(((Vector3)(ref velocity)).get_magnitude() * volumeMultiplier, 0f, 1f));
		}
		velocity = rigidbody.get_velocity();
		previousVelocity = ((Vector3)(ref velocity)).get_magnitude();
	}

	private void TriggerTailSound()
	{
		if ((Object)(object)tailAudioSource != (Object)null)
		{
			tailAudioSource.Play();
		}
	}

	public LinearVelocityFader()
		: this()
	{
	}
}
