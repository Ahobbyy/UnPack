using HumanAPI;
using UnityEngine;

public class StartupExperienceGeometry : MonoBehaviour
{
	[Tooltip("Var to store the left half of the door")]
	public Rigidbody left;

	[Tooltip("Var to store the right half of thr door")]
	public Rigidbody right;

	[Tooltip("Var to store the sound the door should make when opened")]
	public Sound2 doorRelease;

	[Tooltip("Var to store an Audio source for the door opening")]
	public AudioSource doorReleaseSample;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	public void ReleaseDoor()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Release the doors "));
		}
		Vector3 position = Human.all[0].ragdoll.partWaist.transform.get_position();
		position.y = ((Component)this).get_transform().get_position().y;
		((Component)this).get_transform().set_position(position);
		left.set_isKinematic(false);
		right.set_isKinematic(false);
		doorRelease.Play();
		((Component)doorRelease).get_transform().SetParent((Transform)null, false);
		Object.DontDestroyOnLoad((Object)(object)doorRelease);
		Object.Destroy((Object)(object)doorRelease, 5f);
	}

	public StartupExperienceGeometry()
		: this()
	{
	}
}
