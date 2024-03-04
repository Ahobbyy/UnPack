using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Checkpoint))]
public class CheckpointEditor : Editor
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)checkpoint).get_transform().get_position() + new Vector3(0f, 50f, 0f);
		Vector3 val2 = ((Component)checkpoint).get_transform().get_position() + new Vector3(0f, -50f, 0f);
		Gizmos.DrawLine(val, val2);
		RaycastHit val3 = default(RaycastHit);
		if (Physics.Raycast(val, Vector3.get_down(), ref val3, float.PositiveInfinity, -5, (QueryTriggerInteraction)1))
		{
			Gizmos.set_color(Color.get_red());
			Gizmos.DrawSphere(((RaycastHit)(ref val3)).get_point(), 0.5f);
		}
	}

	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		Checkpoint checkpoint = (Checkpoint)(object)((Editor)this).get_target();
		if (GUILayout.Button("Set as debug spawn point", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			BuiltinLevel builtinLevel = Object.FindObjectOfType<BuiltinLevel>();
			if (Object.op_Implicit((Object)(object)builtinLevel))
			{
				builtinLevel.debugSpawnPoint = ((Component)checkpoint).get_transform();
			}
		}
	}

	public CheckpointEditor()
		: this()
	{
	}
}
