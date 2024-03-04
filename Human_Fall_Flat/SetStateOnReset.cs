using Multiplayer;
using UnityEngine;

public class SetStateOnReset : MonoBehaviour, IReset
{
	public bool activeOnReset;

	public void ResetState(int checkpoint, int subObjectives)
	{
		NetBody component = ((Component)this).GetComponent<NetBody>();
		if (Object.op_Implicit((Object)(object)component))
		{
			component.SetVisible(activeOnReset);
		}
		else
		{
			((Component)this).get_gameObject().SetActive(activeOnReset);
		}
	}

	public SetStateOnReset()
		: this()
	{
	}
}
