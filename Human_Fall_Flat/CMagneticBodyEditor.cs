using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagneticBody))]
public class CMagneticBodyEditor : Editor
{
	[DrawGizmo(/*Could not decode attribute arguments.*/)]
	private static void DrawGizmo(MagneticBody magneticBody, GizmoType gizmoType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		MagneticPoint[] componentsInChildren = ((Component)magneticBody).GetComponentsInChildren<MagneticPoint>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			CMagneticPointEditor.DrawGizmo(componentsInChildren[i], gizmoType);
		}
	}

	public CMagneticBodyEditor()
		: this()
	{
	}
}
