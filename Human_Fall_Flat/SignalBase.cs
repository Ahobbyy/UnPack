using System;
using HumanAPI;
using UnityEngine;

public class SignalBase : MonoBehaviour, IReset, IPostReset
{
	public float value;

	private float originalValue;

	public SignalBase sourceSignal;

	public bool boolValue => value > 0.5f;

	public event Action<float> onValueChanged;

	protected virtual void OnEnable()
	{
		if ((Object)(object)sourceSignal != (Object)null)
		{
			sourceSignal.onValueChanged += SignalChanged;
		}
		originalValue = value;
	}

	protected virtual void OnDisable()
	{
		if ((Object)(object)sourceSignal != (Object)null)
		{
			sourceSignal.onValueChanged -= SignalChanged;
		}
	}

	public virtual void ResetState(int checkpoint, int subObjectives)
	{
		if ((Object)(object)sourceSignal == (Object)null)
		{
			SetValue(originalValue);
		}
	}

	public virtual void PostResetState(int checkpoint)
	{
	}

	public void ResetSignal()
	{
		ResetState(0, 0);
		PostResetState(0);
	}

	private void SignalChanged(float val)
	{
		InternalSetValue(val);
	}

	public void SetValue(float value)
	{
		if ((Object)(object)sourceSignal != (Object)null)
		{
			sourceSignal.SetValue(value);
		}
		else
		{
			InternalSetValue(value);
		}
	}

	public void SetValue(bool value)
	{
		SetValue(value ? 1 : 0);
	}

	public void InternalSetValue(float value)
	{
		if (this.value != value)
		{
			this.value = value;
			if (this.onValueChanged != null)
			{
				this.onValueChanged(value);
			}
		}
	}

	public SignalBase()
		: this()
	{
	}
}
