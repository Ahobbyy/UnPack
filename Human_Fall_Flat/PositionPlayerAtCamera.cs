using UnityEditor;
using UnityEngine;

public class PositionPlayerAtCamera : EditorWindow
{
	private GameObject player;

	[MenuItem("Lab42Tools/Position Player At Camera")]
	private static void Init()
	{
		EditorWindow.GetWindow(typeof(PositionPlayerAtCamera)).Show();
	}

	private void OnGUI()
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (GUILayout.Button("Position Player Here", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			player = GameObject.Find("Player(Clone)");
			if (!Object.op_Implicit((Object)(object)player))
			{
				Debug.LogError((object)"No Player object found in scene");
			}
			else
			{
				((Component)player.get_transform().Find("Ball")).get_transform().set_position(((Component)SceneView.get_lastActiveSceneView().get_camera()).get_transform().get_position());
			}
		}
	}

	public PositionPlayerAtCamera()
		: this()
	{
	}
}
