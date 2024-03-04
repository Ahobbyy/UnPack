using UnityEngine;

public class HintTriggerOff : MonoBehaviour
{
	public Hint hint;

	public Collider acceptedCollider;

	public bool requireEnter;

	public void OnTriggerEnter(Collider other)
	{
		if ((!requireEnter || hint.wasActivated) && ((Object)(object)acceptedCollider == (Object)null || (Object)(object)other == (Object)(object)acceptedCollider))
		{
			hint.StopHint();
		}
	}

	public HintTriggerOff()
		: this()
	{
	}
}
