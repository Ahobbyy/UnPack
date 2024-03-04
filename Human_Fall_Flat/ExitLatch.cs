using System.Collections;
using HumanAPI;
using UnityEngine;

public class ExitLatch : Node, IReset
{
	public NodeInput input;

	public Rigidbody left;

	public Rigidbody right;

	public Rigidbody portcullis;

	public AudioSource doorReleaseSample;

	public Sound2 doorReleaseSound;

	public override void Process()
	{
		base.Process();
		if (input.value > 0.5f)
		{
			((MonoBehaviour)this).StartCoroutine(TheEnd());
		}
	}

	private IEnumerator TheEnd()
	{
		portcullis.set_isKinematic(false);
		portcullis.WakeUp();
		yield return (object)new WaitForSeconds(2f);
		left.set_isKinematic(false);
		right.set_isKinematic(false);
		left.WakeUp();
		right.WakeUp();
		if ((Object)(object)doorReleaseSound != (Object)null)
		{
			doorReleaseSound.Play();
		}
		else
		{
			doorReleaseSample.Play();
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		((MonoBehaviour)this).StopAllCoroutines();
		left.set_isKinematic(true);
		right.set_isKinematic(true);
		portcullis.set_isKinematic(true);
	}
}
