using UnityEngine;

public class WaterLevelUpdater : MonoBehaviour
{
	[SerializeField]
	private float liftSpeed = 10f;

	[SerializeField]
	private bool debug;

	private Vector3 targetPosition;

	public float maxHeight = 10f;

	private float startingHeight;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		targetPosition = ((Component)this).get_transform().get_position();
		startingHeight = ((Component)this).get_transform().get_position().y;
	}

	private void OnTriggerEnter(Collider other)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		WaterItemVolume component = ((Component)other).GetComponent<WaterItemVolume>();
		if ((Object)(object)component != (Object)null && !component.hasBeenDrown)
		{
			component.hasBeenDrown = true;
			targetPosition += Vector3.get_up() * component.Volume;
			if (targetPosition.y > startingHeight + maxHeight)
			{
				targetPosition.y = startingHeight + maxHeight;
			}
		}
	}

	private void Update()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_position(Vector3.Lerp(((Component)this).get_transform().get_position(), targetPosition, Time.get_deltaTime() * liftSpeed));
	}

	public WaterLevelUpdater()
		: this()
	{
	}
}
