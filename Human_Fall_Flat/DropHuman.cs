using UnityEditor;
using UnityEngine;

internal static class DropHuman
{
	[MenuItem("Human/Drop Human")]
	public static void DoDrop()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		if (Application.get_isPlaying() && (Object)(object)SceneView.get_lastActiveSceneView() != (Object)null && (Object)(object)SceneView.get_lastActiveSceneView().get_camera() != (Object)null)
		{
			Transform transform = ((Component)SceneView.get_lastActiveSceneView().get_camera()).get_transform();
			((Component)Human.instance).get_gameObject().get_transform().set_position(transform.TransformPoint(new Vector3(0f, 0f, 2f)));
		}
	}
}
