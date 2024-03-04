using HumanAPI;
using UnityEngine;

[AddNodeMenuItem]
public class SignalCheatKeyCode : Node
{
	public string cheatName;

	public NodeInput input;

	public NodeOutput output;

	public KeyCode cheatKey = (KeyCode)120;

	public float cheatValue;

	public bool enableCheat;

	public override string Title => "Cheat: " + cheatName + (enableCheat ? " (On)" : " (Off)");

	public override void Process()
	{
		if (enableCheat)
		{
			output.SetValue(input.value);
		}
		else
		{
			output.SetValue(input.value);
		}
	}

	private void Update()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (enableCheat && Input.GetKeyDown(cheatKey))
		{
			output.SetValue(cheatValue);
			enableCheat = false;
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
