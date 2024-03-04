using UnityEngine;

public class SignalAnimate : MonoBehaviour
{
	public SignalBase triggerSignal;

	private bool isOn;

	public string animationName;

	private void OnEnable()
	{
		isOn = triggerSignal.boolValue;
		triggerSignal.onValueChanged += SignalChanged;
	}

	private void OnDisable()
	{
		triggerSignal.onValueChanged -= SignalChanged;
	}

	private void SignalChanged(float val)
	{
		if (triggerSignal.boolValue != isOn)
		{
			isOn = triggerSignal.boolValue;
			if (isOn)
			{
				((Component)this).GetComponent<Animator>().Play(animationName);
			}
		}
	}

	public SignalAnimate()
		: this()
	{
	}
}
