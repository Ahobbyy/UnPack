using UnityEngine;

public class StoneDoor : MonoBehaviour, IReset
{
	[Tooltip("Reference to the thing barring the door")]
	public GameObject block;

	[Tooltip("A Container for a bunch of phs boulders")]
	public GameObject stoneContainer;

	[Tooltip("Scaffoding the player uses to get to the door")]
	public GameObject scaffold;

	[Tooltip("How far the bar can be moved before the doors open with force")]
	public float distTreshold;

	[Tooltip("The velocity to add to the boulders when the door is opened")]
	public Vector3 spitVelocity;

	private Vector3 startPos;

	private Rigidbody[] stones;

	private Rigidbody[] scaffoldPieces;

	[Tooltip("Use this in order to see the activity within the script")]
	public bool showDebug;

	private void Start()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Start "));
		}
		startPos = block.get_transform().get_position();
		if ((Object)(object)stoneContainer != (Object)null)
		{
			stones = stoneContainer.GetComponentsInChildren<Rigidbody>();
		}
		if ((Object)(object)scaffold != (Object)null)
		{
			scaffoldPieces = scaffold.GetComponentsInChildren<Rigidbody>();
		}
	}

	private void FixedUpdate()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = block.get_transform().get_position() - startPos;
		if (!(((Vector3)(ref val)).get_magnitude() > distTreshold))
		{
			return;
		}
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Doors should burst open "));
		}
		if ((Object)(object)stoneContainer != (Object)null)
		{
			SetKinematic(kinematic: false);
			for (int i = 0; i < stones.Length; i++)
			{
				stones[i].SafeAddForce(spitVelocity, (ForceMode)2);
			}
			((Behaviour)this).set_enabled(false);
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_BREAK_SURPRISE);
		}
	}

	private void SetKinematic(bool kinematic)
	{
		((Behaviour)this).set_enabled(kinematic);
		if (stones != null)
		{
			for (int i = 0; i < stones.Length; i++)
			{
				stones[i].set_isKinematic(kinematic);
			}
		}
		if (scaffoldPieces != null)
		{
			for (int j = 0; j < scaffoldPieces.Length; j++)
			{
				scaffoldPieces[j].set_isKinematic(kinematic);
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Reset State for Checkpoints "));
		}
		SetKinematic(kinematic: true);
	}

	public StoneDoor()
		: this()
	{
	}
}
