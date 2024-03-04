using HumanAPI;
using UnityEngine;

public class NodeMigrateSignal : Node
{
	public SignalBase input;

	public NodeOutput value;

	protected override void OnEnable()
	{
		base.OnEnable();
		value.SetValue(input.value);
		if ((Object)(object)input != (Object)null)
		{
			input.onValueChanged += SignalChanged;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		if ((Object)(object)input != (Object)null)
		{
			input.onValueChanged -= SignalChanged;
		}
	}

	private void SignalChanged(float val)
	{
		value.SetValue(val);
	}
}
