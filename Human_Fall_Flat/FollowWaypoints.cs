using HumanAPI;
using Multiplayer;
using UnityEngine;

public class FollowWaypoints : Node
{
	[Tooltip("Array of the locations to move through")]
	public Transform[] waypoints;

	[Tooltip("Whether this thing is powered or not")]
	public NodeInput power;

	[Tooltip("Whether this thing is getting and input or not")]
	public NodeInput input;

	[Tooltip("Whether this thing has finished a loop through the waypoints")]
	public NodeOutput finished;

	[Tooltip("Whether or not to start again once complete")]
	public bool loop = true;

	[Tooltip("The speed at which the thing moves through the waypoints")]
	public float speed = 1f;

	[Tooltip("The current location for this thing in terms of waypoints")]
	public int curPoint = -1;

	private void Awake()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (curPoint != -1)
		{
			return;
		}
		Vector3 val = waypoints[0].get_position() - ((Component)this).get_transform().get_position();
		float num = ((Vector3)(ref val)).get_magnitude();
		for (int i = 0; i < waypoints.Length; i++)
		{
			val = waypoints[i].get_position() - ((Component)this).get_transform().get_position();
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			if (magnitude < num)
			{
				num = magnitude;
				curPoint = i;
			}
		}
	}

	private void FixedUpdate()
	{
		if (!ReplayRecorder.isPlaying && !NetGame.isClient && power.value != 0f && input.value != 0f)
		{
			Advance(input.value * speed * Time.get_fixedDeltaTime());
		}
	}

	private void Advance(float delta)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		int num = curPoint + 1;
		if (num == waypoints.Length && loop)
		{
			num = 0;
			finished.SetValue(0f);
		}
		((Component)this).get_transform().set_position(Vector3.MoveTowards(((Component)this).get_transform().get_position(), waypoints[num].get_position(), delta));
		((Component)this).get_transform().set_forward(Vector3.MoveTowards(((Component)this).get_transform().get_forward(), waypoints[num].get_forward(), delta));
		Vector3 val = ((Component)this).get_transform().get_position() - waypoints[num].get_position();
		if (((Vector3)(ref val)).get_magnitude() < 0.1f)
		{
			curPoint = num;
		}
	}
}
