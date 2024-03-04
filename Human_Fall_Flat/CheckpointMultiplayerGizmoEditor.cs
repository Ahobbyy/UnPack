using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CheckpointMultiplayerGizmo))]
public class CheckpointMultiplayerGizmoEditor : Editor
{
	[DrawGizmo(/*Could not decode attribute arguments.*/)]
	private static void DrawGizmo(Checkpoint checkpoint, GizmoType gizmoType)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)checkpoint).get_transform().get_position() + new Vector3(0f, 50f, 0f);
		Vector3 val2 = ((Component)checkpoint).get_transform().get_position() + new Vector3(0f, -50f, 0f);
		Gizmos.DrawLine(val, val2);
		RaycastHit val3 = default(RaycastHit);
		if (Physics.Raycast(val, Vector3.get_down(), ref val3, float.PositiveInfinity, -5, (QueryTriggerInteraction)1))
		{
			Gizmos.set_color(Color.get_red());
			Gizmos.DrawSphere(((RaycastHit)(ref val3)).get_point(), 0.5f);
			Gizmos.set_color(new Color(0f, 0f, 1f, 0.5f));
			float num = (checkpoint.tightSpawn ? 0.5f : 2f);
			for (int i = 0; i < 8; i++)
			{
				Gizmos.DrawCube(((RaycastHit)(ref val3)).get_point() + Vector3.get_left() * (float)(i % 3) * num + Vector3.get_back() * (float)(i / 3) * num, new Vector3(0.2f, 1f, 0.2f));
			}
		}
	}

	public CheckpointMultiplayerGizmoEditor()
		: this()
	{
	}
}
