using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class Wire : Rope, ICircuitComponent
	{
		public PowerPlug startPlug;

		public PowerPlug endPlug;

		public float current { get; set; }

		public CircuitConnector forwardConnector => startPlug;

		public CircuitConnector reverseConnector => endPlug;

		public bool isOpen => false;

		public float CalculateVoltage(float I)
		{
			return 0f;
		}

		public void RunCurrent(float I)
		{
			current = I;
		}

		public void StopCurrent()
		{
			current = 0f;
		}

		public override Vector3[] GetHandlePositions()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> list = new List<Vector3>();
			if ((Object)(object)startPlug != (Object)null)
			{
				list.Add(startPlug.ropeFixPoint.get_position());
				list.Add(startPlug.ropeFixPoint.get_position() - 2f * startPlug.alignmentTransform.get_forward());
			}
			for (int i = 0; i < handles.Length; i++)
			{
				list.Add(handles[i].get_position());
			}
			if ((Object)(object)endPlug != (Object)null)
			{
				list.Add(endPlug.ropeFixPoint.get_position() - 2f * endPlug.alignmentTransform.get_forward());
				list.Add(endPlug.ropeFixPoint.get_position());
			}
			return list.ToArray();
		}

		public override void OnEnable()
		{
			if ((Object)(object)startPlug != (Object)null)
			{
				startBody = ((Component)startPlug).GetComponent<Rigidbody>();
				fixStart = (fixStartDir = true);
				startPlug.parent = this;
				startPlug.isForward = true;
			}
			if ((Object)(object)endPlug != (Object)null)
			{
				endBody = ((Component)endPlug).GetComponent<Rigidbody>();
				fixEnd = (fixEndDir = true);
				endPlug.parent = this;
				endPlug.isForward = false;
			}
			base.OnEnable();
			if ((Object)(object)startPlug != (Object)null)
			{
				startPlug.grablist = (GameObject[])(object)new GameObject[3]
				{
					((Component)startPlug).get_gameObject(),
					((Component)bones[0]).get_gameObject(),
					((Component)bones[1]).get_gameObject()
				};
			}
			if ((Object)(object)endPlug != (Object)null)
			{
				endPlug.grablist = (GameObject[])(object)new GameObject[3]
				{
					((Component)endPlug).get_gameObject(),
					((Component)bones[bones.Length - 1]).get_gameObject(),
					((Component)bones[bones.Length - 2]).get_gameObject()
				};
			}
		}
	}
}
