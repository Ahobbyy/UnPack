using System.Text;
using UnityEditor;
using UnityEngine;

namespace Multiplayer
{
	[CustomEditor(typeof(NetBody))]
	[CanEditMultipleObjects]
	public class NetBodyEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			((Editor)this).DrawDefaultInspector();
			NetBody netBody = ((Editor)this).get_target() as NetBody;
			netBody.CalculatePrecision(out var posMeter, out var rotDeg, out var rotMeter);
			GUILayout.Label($"pos: {posMeter}m\r\nrot: {rotDeg}deg\r\nrot: {rotMeter} m\r\nox:{netBody.startPos}\r\nor:{((Quaternion)(ref netBody.startRot)).get_eulerAngles()}\r\ncol:{netBody.collectedRot}\r\napp:{netBody.appliedRot}\r\neulcol:{netBody.collectedEuler}\r\neulapp:{netBody.appliedEuler}", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Apply Suggestion", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < 360; i++)
				{
					NetQuaternion netQuaternion = NetQuaternion.Quantize(Quaternion.AngleAxis((float)i, Vector3.get_forward()), 24);
					stringBuilder.AppendFormat("{0}\t{1}\t{2}\r\n", netQuaternion.x, netQuaternion.y, netQuaternion.z);
				}
				Debug.Log((object)stringBuilder);
			}
		}

		public NetBodyEditor()
			: this()
		{
		}
	}
}
