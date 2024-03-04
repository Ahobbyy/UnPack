using UnityEngine;

[RequireComponent(typeof(ConstantForce))]
public class TriggerConstantForce : MonoBehaviour
{
	[Tooltip("The force that is being applied when the player is holding 2 umbrellas's at the same time.")]
	public float ForceValueDouble = 1000f;

	private ConstantForce cf;

	private float originalForceValue;

	private GameObject otherUmbrella;

	private void OnTriggerStay(Collider other)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		if (!Object.op_Implicit((Object)(object)otherUmbrella))
		{
			if (!Object.op_Implicit((Object)(object)cf))
			{
				cf = ((Component)this).GetComponent<ConstantForce>();
			}
			if (originalForceValue == 0f || originalForceValue != cf.get_force().y)
			{
				originalForceValue = cf.get_force().y;
			}
			ConstantForce component = ((Component)other).get_gameObject().GetComponent<ConstantForce>();
			if (Object.op_Implicit((Object)(object)component) && ((Behaviour)component).get_isActiveAndEnabled() && originalForceValue > 700f)
			{
				otherUmbrella = ((Component)other).get_gameObject();
				cf.set_force(new Vector3(0f, ForceValueDouble, 0f));
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)((Component)other).get_gameObject() == (Object)(object)otherUmbrella)
		{
			cf.set_force(new Vector3(0f, originalForceValue, 0f));
			otherUmbrella = null;
		}
	}

	public TriggerConstantForce()
		: this()
	{
	}
}
