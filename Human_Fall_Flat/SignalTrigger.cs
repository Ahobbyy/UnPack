using HumanAPI;
using UnityEngine;

public class SignalTrigger : Node
{
	public NodeOutput output;

	public void OnTriggerEnter(Collider other)
	{
		if (!(((Component)other).get_tag() != "Player"))
		{
			output.SetValue(1f);
		}
	}
}
