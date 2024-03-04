using HumanAPI;
using UnityEngine;

public class SignalRelease : Node
{
	public NodeInput release;

	[Tooltip("The object to be force released when input >= 0.5")]
	public GameObject toRelease;

	private float prevRelease;

	private bool ReleaseThisFrame
	{
		get
		{
			if (release.value >= 0.5f)
			{
				return prevRelease < 0.5f;
			}
			return false;
		}
	}

	private void OnValidate()
	{
		if ((Object)(object)toRelease == (Object)null)
		{
			toRelease = ((Component)this).get_gameObject();
		}
	}

	public override void Process()
	{
		if (ReleaseThisFrame)
		{
			GrabManager.Release(toRelease);
		}
		prevRelease = release.value;
	}
}
