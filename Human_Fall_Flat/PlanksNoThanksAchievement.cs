using Multiplayer;
using UnityEngine;

public class PlanksNoThanksAchievement : MonoBehaviour, IReset
{
	public static PlanksNoThanksAchievement instance;

	public float plankMoveThreshold;

	public float plankAngleThreshold;

	public NetBody[] plankNetBodies;

	private bool isCancelled;

	private bool hasCheckedCheckpointStart;

	public static bool AchievementValid
	{
		get
		{
			if ((Object)(object)instance != (Object)null)
			{
				return !instance.isCancelled;
			}
			return false;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void OnDestroy()
	{
		instance = null;
	}

	public void Update()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (NetGame.isClient || isCancelled)
		{
			return;
		}
		NetBody[] array = plankNetBodies;
		foreach (NetBody netBody in array)
		{
			if ((Object)(object)netBody != (Object)null && (Object)(object)netBody.RigidBody != (Object)null)
			{
				NetBody netBody2 = netBody;
				Rigidbody rigidBody = netBody.RigidBody;
				if (Vector3.Distance(rigidBody.get_position(), netBody2.startPos) >= plankMoveThreshold || Quaternion.Angle(rigidBody.get_rotation(), netBody2.startRot) >= plankAngleThreshold)
				{
					isCancelled = true;
					break;
				}
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (!hasCheckedCheckpointStart || checkpoint == 0)
		{
			hasCheckedCheckpointStart = true;
			isCancelled = checkpoint != 0;
		}
	}

	public PlanksNoThanksAchievement()
		: this()
	{
	}
}
