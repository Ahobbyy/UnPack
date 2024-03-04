using HumanAPI;
using UnityEngine;

public class HoseSignal : Node
{
	public NodeOutput startConnected;

	public NodeOutput endConnected;

	public NodeOutput bothConnected;

	public HosePipe hosePipe;

	private void Start()
	{
		if ((Object)(object)hosePipe == (Object)null)
		{
			hosePipe = ((Component)this).GetComponent<HosePipe>();
		}
	}

	private void Update()
	{
		if ((Object)(object)hosePipe != (Object)null)
		{
			if ((Object)(object)hosePipe.startPlug != (Object)null && (Object)(object)hosePipe.startPlug.connectedSocket != (Object)null)
			{
				startConnected.SetValue(1f);
			}
			else
			{
				startConnected.SetValue(0f);
			}
			if ((Object)(object)hosePipe.endPlug != (Object)null && (Object)(object)hosePipe.endPlug.connectedSocket != (Object)null)
			{
				endConnected.SetValue(1f);
			}
			else
			{
				endConnected.SetValue(0f);
			}
			bothConnected.SetValue((startConnected.value >= 0.5f && endConnected.value >= 0.5f) ? 1f : 0f);
		}
	}
}
