using Multiplayer;
using UnityEngine;

public class CreditsGround : MonoBehaviour, IReset
{
	public Rigidbody left;

	public Rigidbody right;

	public AudioSource doorReleaseSample;

	private Vector3 oldPosL;

	private Vector3 oldPosR;

	private Quaternion oldRotL;

	private Quaternion oldRotR;

	public bool isOpen { get; protected set; }

	public void OnCollisionEnter(Collision collision)
	{
		if (!NetGame.isClient)
		{
			ChangeState(newState: true);
		}
	}

	public void ResetState(int cp, int subObjective)
	{
		ChangeState(newState: false);
	}

	public void ChangeState(bool newState)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (isOpen != newState)
		{
			isOpen = newState;
			if (newState)
			{
				oldPosL = left.get_position();
				oldPosR = right.get_position();
				oldRotL = left.get_rotation();
				oldRotR = right.get_rotation();
				left.set_isKinematic(false);
				right.set_isKinematic(false);
				((Component)left).GetComponent<NetBody>().isKinematic = false;
				((Component)right).GetComponent<NetBody>().isKinematic = false;
				doorReleaseSample.Play();
			}
			else
			{
				left.set_isKinematic(true);
				right.set_isKinematic(true);
				((Component)left).GetComponent<NetBody>().isKinematic = true;
				((Component)right).GetComponent<NetBody>().isKinematic = true;
			}
		}
	}

	public CreditsGround()
		: this()
	{
	}
}
