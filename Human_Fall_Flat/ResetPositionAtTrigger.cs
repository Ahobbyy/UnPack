using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ResetPositionAtTrigger : MonoBehaviour
{
	public Transform postionAtReset;

	public bool isKinematicAfterReset = true;

	private Rigidbody rb;

	public void Awake()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
		rb.set_isKinematic(false);
	}

	public void ResetPostion()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		rb.set_isKinematic(isKinematicAfterReset);
		((Component)this).get_transform().set_position(postionAtReset.get_position());
	}

	public ResetPositionAtTrigger()
		: this()
	{
	}
}
