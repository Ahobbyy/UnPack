using UnityEngine;

public class FantasticWind : MonoBehaviour
{
	[SerializeField]
	private Rigidbody[] blownObjects;

	[SerializeField]
	private int percentChance;

	[SerializeField]
	private Vector3 direction;

	[SerializeField]
	private int frameSkip = 10;

	[SerializeField]
	private float minForce;

	[SerializeField]
	private float maxForce;

	private int frame;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if (frame == frameSkip)
		{
			frame = 0;
			Rigidbody[] array = blownObjects;
			foreach (Rigidbody val in array)
			{
				if (Random.Range(0, 100) < percentChance)
				{
					float num = Random.Range(minForce, maxForce);
					val.AddForce(Vector3.Normalize(direction) * num);
				}
			}
		}
		else
		{
			frame++;
		}
	}

	public FantasticWind()
		: this()
	{
	}
}
