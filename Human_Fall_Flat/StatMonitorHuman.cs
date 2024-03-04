using UnityEngine;

public class StatMonitorHuman : MonoBehaviour
{
	private Human human;

	private Vector3 oldPos;

	private Vector3 climbStartPos;

	private Vector3 walkStartPos;

	private HumanState oldState;

	private bool oldOnGround;

	private void Awake()
	{
		human = ((Component)this).GetComponent<Human>();
	}

	private void Update()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)human == (Object)null)
		{
			return;
		}
		Vector3 position = ((Component)human).get_transform().get_position();
		HumanState state = human.state;
		_ = human.onGround;
		if (oldState != HumanState.Walk && state == HumanState.Walk)
		{
			walkStartPos = position;
		}
		if (oldState == HumanState.Walk)
		{
			Vector3 val = (position - walkStartPos).ZeroY();
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			if (state != HumanState.Walk)
			{
				if (magnitude > 0.5f)
				{
					StatsAndAchievements.AddTravel(human, magnitude);
				}
			}
			else if (magnitude > 10f)
			{
				StatsAndAchievements.AddTravel(human, magnitude);
				walkStartPos = position;
			}
		}
		if (oldState == HumanState.Climb && (state == HumanState.Jump || state == HumanState.Idle || state == HumanState.Walk))
		{
			float num = position.y - climbStartPos.y;
			if (num > 0f)
			{
				StatsAndAchievements.AddClimb(human, num);
			}
		}
		if (state == HumanState.Climb && oldState != HumanState.Jump && oldState != HumanState.Climb)
		{
			climbStartPos = oldPos;
		}
		else if (state == HumanState.Jump && oldState != HumanState.Jump)
		{
			climbStartPos = oldPos;
		}
		else if (state == HumanState.Climb && position.y - climbStartPos.y < 0f)
		{
			climbStartPos = oldPos;
		}
		if (state == HumanState.Jump && oldState != HumanState.Jump)
		{
			StatsAndAchievements.IncreaseJumpCount(human);
		}
		oldState = state;
		oldPos = position;
	}

	public StatMonitorHuman()
		: this()
	{
	}
}
