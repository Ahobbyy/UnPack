using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagneticPoint))]
public class CMagneticPointEditor : Editor
{
	[DrawGizmo(/*Could not decode attribute arguments.*/)]
	public static void DrawGizmo(MagneticPoint magnetic, GizmoType gizmoType)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		if (magnetic.magnetism == 0f)
		{
			Gizmos.set_color(Color.get_white());
		}
		else if (magnetic.magnetism > 0f)
		{
			Gizmos.set_color(Color.get_red());
		}
		else
		{
			Gizmos.set_color(Color.get_cyan());
		}
		if (!((Behaviour)magnetic).get_isActiveAndEnabled())
		{
			Gizmos.set_color(new Color(0.3f, 0.3f, 0.3f, 0.5f));
		}
		Vector3 val = ((Component)magnetic).get_transform().TransformPoint(magnetic.magneticPointOffset);
		Gizmos.DrawSphere(val, 0.2f);
		Gizmos.DrawWireSphere(val, magnetic.range);
		if (magnetic.magnetActive)
		{
			Gizmos.set_color(Color.get_white());
		}
		Gizmos.DrawLine(val, val + ((Component)magnetic).get_transform().get_forward().Rotate(((Component)magnetic).get_transform().get_up(), magnetic.angle / 2f));
		Gizmos.DrawLine(val, val + ((Component)magnetic).get_transform().get_forward().Rotate(((Component)magnetic).get_transform().get_up(), (0f - magnetic.angle) / 2f));
	}

	public CMagneticPointEditor()
		: this()
	{
	}
}
