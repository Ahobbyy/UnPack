using UnityEngine;

public class MagicLock : MonoBehaviour
{
	[Tooltip("The Magic key we need to open the lock")]
	public MagicKey magicKey;

	[Tooltip("The game object to diable when the lock is broken")]
	public GameObject objectToDisable;

	[Tooltip("Forced used by the magnet")]
	public float attractionForce;

	private bool magnetActive;

	private Rigidbody keyBody;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void Start()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Started "));
		}
		keyBody = ((Component)magicKey).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (magnetActive && magicKey.isPoweredUp)
		{
			Vector3 val = ((Component)this).get_transform().get_position() - ((Component)magicKey).get_transform().get_position();
			val = ((Vector3)(ref val)).get_normalized() * attractionForce;
			keyBody.AddForce(val);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)((Component)other).get_gameObject() == (Object)(object)((Component)magicKey).get_gameObject())
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Magic Key Entered range "));
			}
			magnetActive = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((Object)(object)((Component)other).get_gameObject() == (Object)(object)((Component)magicKey).get_gameObject())
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Magic Key Lef Range "));
			}
			magnetActive = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((Object)(object)collision.get_gameObject() == (Object)(object)((Component)magicKey).get_gameObject() && magicKey.isPoweredUp)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Magic key powered and unlocked me "));
			}
			objectToDisable.SetActive(false);
		}
	}

	public MagicLock()
		: this()
	{
	}
}
