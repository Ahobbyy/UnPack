using System.Linq;
using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SteamNode))]
public class SteamNodeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		((Editor)this).DrawDefaultInspector();
		SteamNode steamNode = (SteamNode)(object)((Editor)this).get_target();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUILayout.Button("Attach", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			PipePort[] array = FindAllPorts(steamNode);
			Collider[] componentsInChildren = ((Component)steamNode).get_gameObject().GetComponentsInChildren<Collider>();
			PipePort[] array2 = array;
			foreach (PipePort pipePort in array2)
			{
				Collider[] array3 = Physics.OverlapSphere(((Component)pipePort).get_transform().get_position(), 1f);
				foreach (Collider val in array3)
				{
					PipePort pipePort2 = FindClosestPort(((Component)steamNode).get_transform().get_position(), ((Component)val).get_transform());
					if (pipePort.CanConnect(pipePort2) && !componentsInChildren.Contains(val))
					{
						if (Application.get_isPlaying())
						{
							pipePort.ConnectPipe(pipePort2);
							break;
						}
						EditorGUI.BeginChangeCheck();
						pipePort.startPipe = pipePort2;
						EditorUtility.SetDirty((Object)(object)pipePort);
						EditorGUI.EndChangeCheck();
						break;
					}
				}
			}
		}
		if (GUILayout.Button("Detach", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			PipePort[] array2 = FindAllPorts(steamNode);
			foreach (PipePort pipePort3 in array2)
			{
				if (Application.get_isPlaying())
				{
					if ((Object)(object)pipePort3.connectedPort != (Object)null)
					{
						pipePort3.DisconnectPipe();
					}
				}
				else
				{
					pipePort3.startPipe = null;
					EditorUtility.SetDirty((Object)(object)pipePort3);
				}
			}
		}
		GUILayout.EndHorizontal();
	}

	private PipePort FindClosestPort(Vector3 position, Transform parent)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		PipePort[] componentsInChildren = ((Component)parent).GetComponentsInChildren<PipePort>();
		if (componentsInChildren.Length == 0)
		{
			return null;
		}
		PipePort pipePort = componentsInChildren[0];
		PipePort[] array = componentsInChildren;
		foreach (PipePort pipePort2 in array)
		{
			if (Vector3.Distance(((Component)pipePort2).get_transform().get_position(), position) < Vector3.Distance(((Component)pipePort).get_transform().get_position(), position))
			{
				pipePort = pipePort2;
			}
		}
		return pipePort;
	}

	private PipePort[] FindAllPorts(SteamNode node)
	{
		return (from pipe in ((Component)node).get_gameObject().GetComponentsInChildren<PipePort>()
			where pipe.connectable
			select pipe).ToArray();
	}

	public SteamNodeEditor()
		: this()
	{
	}
}
