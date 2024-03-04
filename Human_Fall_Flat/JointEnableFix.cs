using UnityEngine;

public class JointEnableFix : MonoBehaviour
{
	private Quaternion initialLocalRotation;

	private Vector3 initialLocalPosition;

	private Quaternion localRotationOnDisable;

	private Vector3 localPositionOnDisable;

	private bool hasDisabled;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		initialLocalRotation = ((Component)this).get_transform().get_localRotation();
		initialLocalPosition = ((Component)this).get_transform().get_localPosition();
	}

	private void OnDisable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		localRotationOnDisable = ((Component)this).get_transform().get_localRotation();
		((Component)this).get_transform().set_localRotation(initialLocalRotation);
		localPositionOnDisable = ((Component)this).get_transform().get_localPosition();
		((Component)this).get_transform().set_localPosition(initialLocalPosition);
		hasDisabled = true;
	}

	private void OnEnable()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (hasDisabled)
		{
			hasDisabled = false;
			((Component)this).get_transform().set_localRotation(localRotationOnDisable);
			((Component)this).get_transform().set_localPosition(localPositionOnDisable);
		}
	}

	public JointEnableFix()
		: this()
	{
	}
}
