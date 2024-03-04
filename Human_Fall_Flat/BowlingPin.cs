using Multiplayer;
using UnityEngine;

public class BowlingPin : MonoBehaviour
{
	public Transform spawnPosition;

	public Collider uprightCollider;

	public Collider uprightSensor;

	private Rigidbody rigidBody;

	private NetBody netBody;

	private bool respawning;

	private float respawningTimer;

	private const float respawnDuration = 0.5f;

	private void Start()
	{
		rigidBody = ((Component)this).GetComponent<Rigidbody>();
		netBody = ((Component)this).GetComponent<NetBody>();
	}

	public bool IsInPlace()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = uprightCollider.get_bounds();
		return ((Bounds)(ref bounds)).Intersects(uprightSensor.get_bounds());
	}

	public void ResetPosition()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		rigidBody.set_isKinematic(true);
		((Component)this).get_transform().set_position(spawnPosition.get_position());
		((Component)this).get_transform().set_rotation(Quaternion.get_identity());
		respawning = true;
		respawningTimer = 0f;
	}

	public void Hide()
	{
		netBody.SetVisible(visible: false);
	}

	public void Show()
	{
		netBody.SetVisible(visible: true);
		ResetPosition();
	}

	private void Update()
	{
		if (respawning)
		{
			respawningTimer += Time.get_deltaTime();
			if (respawningTimer >= 0.5f)
			{
				rigidBody.set_isKinematic(false);
				respawning = false;
			}
		}
	}

	public BowlingPin()
		: this()
	{
	}
}
