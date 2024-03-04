using UnityEngine;

public class PartsExplode : MonoBehaviour
{
	public Rigidbody[] bodiesToExplode;

	public void Explode()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (bodiesToExplode.Length != 0)
		{
			Rigidbody[] array = bodiesToExplode;
			foreach (Rigidbody val in array)
			{
				val.set_isKinematic(false);
				val.AddForceAtPosition(new Vector3(0f, 0.05f, -0.05f), ((Component)val).get_transform().get_position());
			}
		}
	}

	public PartsExplode()
		: this()
	{
	}
}
