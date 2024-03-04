using UnityEngine;

public class HideObject : MonoBehaviour
{
	public Vector3 Translation;

	public void Hide()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
		if ((Object)(object)component != (Object)null)
		{
			component.set_isKinematic(true);
		}
		((Component)this).get_gameObject().SetActive(false);
		Transform transform = ((Component)this).get_transform();
		transform.set_localPosition(transform.get_localPosition() + Translation);
	}

	public void Show()
	{
		((Component)this).get_gameObject().SetActive(true);
	}

	public HideObject()
		: this()
	{
	}
}
