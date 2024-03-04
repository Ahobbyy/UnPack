using Multiplayer;
using UnityEngine;

public sealed class ChainRespawn : MonoBehaviour
{
	private const float kMinimumTimeBetweenCallbacks = 2f;

	private NetBody[] chainBodies;

	private float lastCallback = -1f;

	private void Awake()
	{
		chainBodies = ((Component)this).GetComponentsInChildren<NetBody>();
	}

	public void OnChainRespawn()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		float unscaledTime = Time.get_unscaledTime();
		if (unscaledTime - lastCallback > 2f)
		{
			lastCallback = unscaledTime;
			NetBody[] array = chainBodies;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Respawn(Vector3.get_zero());
			}
		}
	}

	public ChainRespawn()
		: this()
	{
	}
}
