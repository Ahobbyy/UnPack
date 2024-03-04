using UnityEngine;

public class CreditsLetter : MonoBehaviour
{
	public float width = 1f;

	public char character = 'a';

	public void OnCollisionEnter(Collision collision)
	{
		((Component)this).GetComponent<Rigidbody>().set_isKinematic(false);
	}

	internal void Attach(CreditsBlock creditsBlock, Vector3 offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)this).get_transform();
		transform.set_localPosition(transform.get_localPosition() + offset);
		((Component)this).get_transform().set_localRotation(Quaternion.Euler(-90f, 180f, 0f));
		((Component)this).GetComponent<Rigidbody>().set_isKinematic(true);
		((Component)this).get_gameObject().SetActive(true);
	}

	public CreditsLetter()
		: this()
	{
	}
}
