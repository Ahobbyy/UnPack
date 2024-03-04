using HumanAPI;
using UnityEngine;

public class BreakOnSignal : MonoBehaviour, IPostReset
{
	public Sound2 sound;

	public float treshold;

	public Joint jointToBreak;

	public SignalBase source;

	public GameObject jointHolder;

	private void Awake()
	{
		if ((Object)(object)jointHolder == (Object)null)
		{
			jointHolder = ((Component)jointToBreak).get_gameObject();
		}
		if (jointHolder.GetComponents<Joint>().Length > 1)
		{
			Debug.LogError((object)"BreakOnSignal has multiple jooints", (Object)(object)this);
		}
	}

	private void OnEnable()
	{
		source.onValueChanged += SignalChanged;
	}

	private void OnDisable()
	{
		source.onValueChanged -= SignalChanged;
	}

	private void SignalChanged(float value)
	{
		if (((Behaviour)this).get_enabled() && value > treshold)
		{
			CGShift component = ((Component)jointToBreak).GetComponent<CGShift>();
			if ((Object)(object)component != (Object)null)
			{
				component.ResetCG();
			}
			Object.Destroy((Object)(object)jointToBreak);
			((Behaviour)this).set_enabled(false);
			if ((Object)(object)sound != (Object)null)
			{
				sound.PlayOneShot();
			}
		}
	}

	public void PostResetState(int checkpoint)
	{
		((Behaviour)this).set_enabled(true);
		jointToBreak = jointHolder.GetComponent<Joint>();
	}

	public BreakOnSignal()
		: this()
	{
	}
}
