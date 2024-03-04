using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public static class VolumeFactory
	{
		[MenuItem("GameObject/3D Object/Post-process Volume")]
		private static void CreateVolume()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = new GameObject("Post-process Volume");
			BoxCollider obj = val.AddComponent<BoxCollider>();
			obj.set_size(Vector3.get_one());
			((Collider)obj).set_isTrigger(true);
			val.AddComponent<PostProcessVolume>();
			Object[] objects = (Object[])(object)new GameObject[1] { val };
			Selection.set_objects(objects);
			EditorApplication.ExecuteMenuItem("GameObject/Move To View");
		}
	}
}
