using UnityEngine;

public class CounterweightStretch : MonoBehaviour
{
	public Transform target;

	private void Start()
	{
	}

	private void Update()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		float num = (((Component)target).get_transform().get_position().y - ((Component)this).get_transform().get_position().y) / 2f;
		((Component)this).get_transform().set_localScale(new Vector3(1f, num, 1f));
	}

	public CounterweightStretch()
		: this()
	{
	}
}
