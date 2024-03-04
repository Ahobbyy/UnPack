using HumanAPI;
using UnityEngine;

public class SignalGravity : Node
{
	[Tooltip("Default gravity is (X:0, Y:-9.81, Z:0")]
	public NodeInput gravityX;

	[Tooltip("Default gravity is (X:0, Y:-9.81, Z:0")]
	public NodeInput gravityY;

	[Tooltip("Default gravity is (X:0, Y:-9.81, Z:0")]
	public NodeInput gravityZ;

	public NodeInput setDefault;

	private float prevX;

	private float prevY;

	private float prevZ;

	private float prevSetDefault;

	private Vector3 defaultGravity = new Vector3(0f, -9.81f, 0f);

	public override string Title => $"Gravity: {Physics.get_gravity()}";

	private bool ResetThisFrame
	{
		get
		{
			if (setDefault.value >= 0.5f)
			{
				return prevSetDefault < 0.5f;
			}
			return false;
		}
	}

	private bool valueChanged
	{
		get
		{
			if (prevX == gravityX.value && prevY == gravityY.value)
			{
				return prevZ != gravityZ.value;
			}
			return true;
		}
	}

	public override void Process()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (valueChanged || ResetThisFrame)
		{
			Physics.set_gravity((Vector3)(ResetThisFrame ? defaultGravity : new Vector3(gravityX.value, gravityY.value, gravityZ.value)));
		}
		prevSetDefault = setDefault.value;
		prevX = gravityX.value;
		prevY = gravityY.value;
		prevZ = gravityZ.value;
	}
}
