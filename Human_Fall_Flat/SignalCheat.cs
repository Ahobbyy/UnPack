using HumanAPI;
using UnityEngine;

[AddNodeMenuItem]
public class SignalCheat : Node
{
	public string cheatName;

	public NodeInput input;

	public NodeOutput output;

	public float cheatValue;

	public bool enableCheat;

	public override string Title => "Cheat: " + cheatName + (enableCheat ? " (On)" : " (Off)");

	public override void Process()
	{
		if (enableCheat)
		{
			output.SetValue(cheatValue);
		}
		else
		{
			output.SetValue(input.value);
		}
	}

	public static bool AnyCheatsEnabled()
	{
		SignalCheat[] array = Object.FindObjectsOfType<SignalCheat>();
		foreach (SignalCheat signalCheat in array)
		{
			if (signalCheat.enableCheat)
			{
				Debug.Log((object)("Cheat " + ((Object)signalCheat).get_name() + " enabled."), (Object)(object)signalCheat);
				return true;
			}
		}
		return false;
	}
}
