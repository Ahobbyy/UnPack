using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SplineComponent))]
public class SplineComponentEditor : Editor
{
	private int hotIndex = -1;

	private int removeIndex = -1;

	public override void OnInspectorGUI()
	{
		EditorGUILayout.HelpBox("Hold Shift and click to append and insert curve points. Backspace to delete points.", (MessageType)1);
		SplineComponent splineComponent = ((Editor)this).get_target() as SplineComponent;
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		bool flag = GUILayout.Toggle(splineComponent.closed, "Closed", GUIStyle.op_Implicit("button"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (splineComponent.closed != flag)
		{
			splineComponent.closed = flag;
			splineComponent.ResetIndex();
		}
		if (GUILayout.Button("Flatten Y Axis", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Undo.RecordObject(((Editor)this).get_target(), "Flatten Y Axis");
			splineComponent.Flatten(splineComponent.points);
			splineComponent.Flatten(splineComponent.points);
		}
		if (GUILayout.Button("Center around Origin", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Undo.RecordObject(((Editor)this).get_target(), "Center around Origin");
			splineComponent.CenterAroundOrigin(splineComponent.points);
			splineComponent.ResetIndex();
		}
		if (GUILayout.Button("Reverse", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Undo.RecordObject(((Editor)this).get_target(), "Reverse");
			splineComponent.Reverse(splineComponent.points);
			splineComponent.ResetIndex();
		}
		GUILayout.EndHorizontal();
	}

	[DrawGizmo(/*Could not decode attribute arguments.*/)]
	private static void DrawGizmosLoRes(SplineComponent spline, GizmoType gizmoType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_white());
		DrawGizmo(spline, 64);
	}

	[DrawGizmo(/*Could not decode attribute arguments.*/)]
	private static void DrawGizmosHiRes(SplineComponent spline, GizmoType gizmoType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_white());
		DrawGizmo(spline, 1024);
	}

	private static void DrawGizmo(SplineComponent spline, int stepCount)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (spline.points.Count > 0)
		{
			float num = 0f;
			Vector3 val = spline.GetNonUniformPoint(0f);
			float num2 = 1f / (float)stepCount;
			do
			{
				num += num2;
				Vector3 nonUniformPoint = spline.GetNonUniformPoint(num);
				Gizmos.DrawLine(val, nonUniformPoint);
				val = nonUniformPoint;
			}
			while (num + num2 <= 1f);
		}
	}

	private void OnSceneGUI()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		SplineComponent splineComponent = ((Editor)this).get_target() as SplineComponent;
		Event.get_current();
		GUIUtility.GetControlID((FocusType)2);
		Event.get_current().get_mousePosition();
		Vector3 val = SceneView.get_currentDrawingSceneView().get_camera().ScreenToViewportPoint(Vector2.op_Implicit(Event.get_current().get_mousePosition()));
		if (val.x < 0f || val.x > 1f || val.y < 0f || val.y > 1f)
		{
			return;
		}
		SerializedProperty val2 = ((Editor)this).get_serializedObject().FindProperty("points");
		if (Event.get_current().get_shift())
		{
			if (splineComponent.closed)
			{
				ShowClosestPointOnClosedSpline(val2);
			}
			else
			{
				ShowClosestPointOnOpenSpline(val2);
			}
		}
		for (int i = 0; i < splineComponent.points.Count; i++)
		{
			SerializedProperty arrayElementAtIndex = val2.GetArrayElementAtIndex(i);
			Vector3 vector3Value = arrayElementAtIndex.get_vector3Value();
			Vector3 val3 = ((Component)splineComponent).get_transform().TransformPoint(vector3Value);
			if (hotIndex == i)
			{
				Vector3 val4 = Handles.PositionHandle(val3, ((int)Tools.get_pivotRotation() == 1) ? Quaternion.get_identity() : ((Component)splineComponent).get_transform().get_rotation());
				Vector3 val5 = ((Component)splineComponent).get_transform().InverseTransformDirection(val4 - val3);
				if (((Vector3)(ref val5)).get_sqrMagnitude() > 0f)
				{
					arrayElementAtIndex.set_vector3Value(vector3Value + val5);
					splineComponent.ResetIndex();
				}
				HandleCommands(val3);
			}
			Handles.set_color(((i == 0) | (i == splineComponent.points.Count - 1)) ? Color.get_red() : Color.get_white());
			float num = HandleUtility.GetHandleSize(val3) * 0.1f;
			if (Handles.Button(val3, Quaternion.get_identity(), num, num, new CapFunction(Handles.SphereHandleCap)))
			{
				hotIndex = i;
			}
			if (!(((Component)SceneView.get_currentDrawingSceneView().get_camera()).get_transform().InverseTransformPoint(val3).z < 0f))
			{
				Handles.Label(val3, i.ToString());
			}
		}
		if (removeIndex >= 0 && val2.get_arraySize() > 4)
		{
			val2.DeleteArrayElementAtIndex(removeIndex);
			splineComponent.ResetIndex();
		}
		removeIndex = -1;
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	private void HandleCommands(Vector3 wp)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Invalid comparison between Unknown and I4
		if ((int)Event.get_current().get_type() == 14 && Event.get_current().get_commandName() == "FrameSelected")
		{
			SceneView.get_currentDrawingSceneView().Frame(new Bounds(wp, Vector3.get_one() * 10f), false);
			Event.get_current().Use();
		}
		if ((int)Event.get_current().get_type() == 4 && (int)Event.get_current().get_keyCode() == 8)
		{
			removeIndex = hotIndex;
			Event.get_current().Use();
		}
	}

	private void ShowClosestPointOnClosedSpline(SerializedProperty points)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		SplineComponent splineComponent = ((Editor)this).get_target() as SplineComponent;
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(((Component)splineComponent).get_transform().get_up(), ((Component)splineComponent).get_transform().get_position());
		Ray val2 = HandleUtility.GUIPointToWorldRay(Event.get_current().get_mousePosition());
		float num = default(float);
		if (((Plane)(ref val)).Raycast(val2, ref num))
		{
			Vector3 val3 = ((Ray)(ref val2)).get_origin() + ((Ray)(ref val2)).get_direction() * num;
			Handles.DrawWireDisc(val3, ((Component)splineComponent).get_transform().get_up(), 5f);
			float num2 = SearchForClosestPoint(Event.get_current().get_mousePosition());
			Vector3 nonUniformPoint = splineComponent.GetNonUniformPoint(num2);
			Handles.DrawLine(val3, nonUniformPoint);
			if ((int)Event.get_current().get_type() == 0 && Event.get_current().get_button() == 0 && Event.get_current().get_shift())
			{
				int num3 = (Mathf.FloorToInt(num2 * (float)splineComponent.points.Count) + 2) % splineComponent.points.Count;
				points.InsertArrayElementAtIndex(num3);
				points.GetArrayElementAtIndex(num3).set_vector3Value(((Component)splineComponent).get_transform().InverseTransformPoint(nonUniformPoint));
				((Editor)this).get_serializedObject().ApplyModifiedProperties();
				hotIndex = num3;
			}
		}
	}

	private void ShowClosestPointOnOpenSpline(SerializedProperty points)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		SplineComponent splineComponent = ((Editor)this).get_target() as SplineComponent;
		Plane val = default(Plane);
		((Plane)(ref val))._002Ector(((Component)splineComponent).get_transform().get_up(), ((Component)splineComponent).get_transform().get_position());
		Ray val2 = HandleUtility.GUIPointToWorldRay(Event.get_current().get_mousePosition());
		float num = default(float);
		if (!((Plane)(ref val)).Raycast(val2, ref num))
		{
			return;
		}
		Vector3 val3 = ((Ray)(ref val2)).get_origin() + ((Ray)(ref val2)).get_direction() * num;
		float handleSize = HandleUtility.GetHandleSize(val3);
		Handles.DrawWireDisc(val3, ((Component)splineComponent).get_transform().get_up(), handleSize);
		float num2 = SearchForClosestPoint(Event.get_current().get_mousePosition());
		Vector3 val4 = val3 - splineComponent.GetNonUniformPoint(0f);
		if (((Vector3)(ref val4)).get_sqrMagnitude() < 25f)
		{
			num2 = 0f;
		}
		val4 = val3 - splineComponent.GetNonUniformPoint(1f);
		if (((Vector3)(ref val4)).get_sqrMagnitude() < 25f)
		{
			num2 = 1f;
		}
		Vector3 nonUniformPoint = splineComponent.GetNonUniformPoint(num2);
		bool flag = Mathf.Approximately(num2, 0f) || Mathf.Approximately(num2, 1f);
		Handles.set_color(flag ? Color.get_red() : Color.get_white());
		Handles.DrawLine(val3, nonUniformPoint);
		Handles.set_color(Color.get_white());
		int num3 = 1 + Mathf.FloorToInt(num2 * (float)(splineComponent.points.Count - 3));
		if ((int)Event.get_current().get_type() != 0 || Event.get_current().get_button() != 0 || !Event.get_current().get_shift())
		{
			return;
		}
		if (flag)
		{
			if (num3 == splineComponent.points.Count - 2)
			{
				num3++;
			}
			points.InsertArrayElementAtIndex(num3);
			points.GetArrayElementAtIndex(num3).set_vector3Value(((Component)splineComponent).get_transform().InverseTransformPoint(val3));
			hotIndex = num3;
		}
		else
		{
			num3++;
			points.InsertArrayElementAtIndex(num3);
			points.GetArrayElementAtIndex(num3).set_vector3Value(((Component)splineComponent).get_transform().InverseTransformPoint(nonUniformPoint));
			hotIndex = num3;
		}
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	private float SearchForClosestPoint(Vector2 screenPoint, float A = 0f, float B = 1f, float steps = 1000f)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		SplineComponent splineComponent = ((Editor)this).get_target() as SplineComponent;
		float num = float.MaxValue;
		float num2 = (B - A) / steps;
		float num3 = A;
		for (int i = 0; (float)i <= steps; i++)
		{
			Vector2 val = HandleUtility.WorldToGUIPoint(splineComponent.GetNonUniformPoint((float)i * num2));
			Vector2 val2 = screenPoint - val;
			float sqrMagnitude = ((Vector2)(ref val2)).get_sqrMagnitude();
			if (sqrMagnitude < num)
			{
				num3 = i;
				num = sqrMagnitude;
			}
		}
		return num3 * num2;
	}

	public SplineComponentEditor()
		: this()
	{
	}
}
