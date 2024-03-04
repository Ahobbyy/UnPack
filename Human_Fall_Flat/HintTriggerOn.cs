using UnityEngine;

public class HintTriggerOn : MonoBehaviour
{
	public Hint hint;

	public Collider acceptedCollider;

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)acceptedCollider == (Object)null || (Object)(object)other == (Object)(object)acceptedCollider)
		{
			hint.TriggerHint();
		}
	}

	public HintTriggerOn()
		: this()
	{
	}
}
