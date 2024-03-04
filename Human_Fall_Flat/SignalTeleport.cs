using HumanAPI;
using UnityEngine;

public class SignalTeleport : Node
{
	public NodeInput triggerTeleport;

	public Transform objectToTeleport;

	public Transform targetTransform;

	private float prevValue;

	private Rigidbody body;

	private Vector3 InitialPosition;

	private Quaternion InitialRotation;

	private int InitialChildren;

	private Vector3[] ChildPosition;

	private Quaternion[] ChildRotation;

	private bool doTeleport;

	private int telePhase;

	private void Start()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		body = ((Component)objectToTeleport).get_gameObject().GetComponent<Rigidbody>();
		if ((Object)(object)body != (Object)null)
		{
			InitialPosition = body.get_position();
			InitialRotation = body.get_rotation();
		}
		else
		{
			InitialPosition = ((Component)objectToTeleport).get_transform().get_position();
			InitialRotation = ((Component)objectToTeleport).get_transform().get_rotation();
		}
		InitialChildren = objectToTeleport.get_childCount();
		ChildPosition = (Vector3[])(object)new Vector3[InitialChildren];
		ChildRotation = (Quaternion[])(object)new Quaternion[InitialChildren];
		for (int i = 0; i < InitialChildren; i++)
		{
			Transform child = ((Component)objectToTeleport).get_transform().GetChild(i);
			Rigidbody component = ((Component)child).get_gameObject().GetComponent<Rigidbody>();
			if ((Object)(object)component != (Object)null)
			{
				ChildPosition[i] = component.get_position();
				ChildRotation[i] = component.get_rotation();
			}
			else
			{
				ChildPosition[i] = child.get_position();
				ChildRotation[i] = child.get_rotation();
			}
		}
	}

	private void FixedUpdate()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		if (!doTeleport)
		{
			return;
		}
		if (telePhase == 0)
		{
			if ((Object)(object)body != (Object)null)
			{
				body.MovePosition(InitialPosition);
				body.MoveRotation(InitialRotation);
			}
			((Component)objectToTeleport).get_transform().set_position(InitialPosition);
			((Component)objectToTeleport).get_transform().set_rotation(InitialRotation);
			for (int i = 0; i < InitialChildren; i++)
			{
				Transform child = ((Component)objectToTeleport).get_transform().GetChild(i);
				Rigidbody component = ((Component)child).get_gameObject().GetComponent<Rigidbody>();
				if ((Object)(object)component != (Object)null)
				{
					component.MovePosition(ChildPosition[i]);
					component.MoveRotation(ChildRotation[i]);
				}
				child.set_position(ChildPosition[i]);
				child.set_rotation(ChildRotation[i]);
			}
			telePhase++;
		}
		else
		{
			if ((Object)(object)body != (Object)null)
			{
				body.MovePosition(targetTransform.get_position());
				body.MoveRotation(targetTransform.get_rotation());
			}
			else
			{
				((Component)objectToTeleport).get_transform().set_position(targetTransform.get_position());
				((Component)objectToTeleport).get_transform().set_rotation(targetTransform.get_rotation());
			}
			doTeleport = false;
		}
	}

	public override void Process()
	{
		if (triggerTeleport.value >= 0.5f && prevValue < 0.5f)
		{
			doTeleport = true;
			telePhase = 0;
		}
		prevValue = triggerTeleport.value;
	}
}
