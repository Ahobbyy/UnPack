using UnityEngine;

public class FantasticCenterOfMass : MonoBehaviour
{
	[SerializeField]
	private Transform COMObject;

	private void Start()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Rigidbody>()))
		{
			((Component)this).GetComponent<Rigidbody>().set_centerOfMass(COMObject.get_localPosition());
		}
	}

	public FantasticCenterOfMass()
		: this()
	{
	}
}
