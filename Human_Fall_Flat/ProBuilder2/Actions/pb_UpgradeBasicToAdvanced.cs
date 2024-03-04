using System.Linq;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_UpgradeBasicToAdvanced : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Upgrade Scene to Advanced", false, 610)]
		public static void MenuUpgradeSceneAdvanced()
		{
			if (EditorUtility.DisplayDialog("Upgrade Scene to Advanced", "This utility sets the materials on every ProBuilder object in the scene.  Continue?", "Okay", "Cancel"))
			{
				DoUpgrade((pb_Object[])(object)Resources.FindObjectsOfTypeAll(typeof(pb_Object)));
				EditorUtility.DisplayDialog("Upgrade ProBuilder Objects", "Successfully upgraded all ProBuilder objects in scene.\n\nIf any of the objects in the scene were prefabs you'll need to 'Apply' changes.", "Okay");
			}
		}

		[MenuItem("Tools/ProBuilder/Repair/Upgrade Selection to Advanced", false, 610)]
		public static void MenuUpgradeSelectionAdvanced()
		{
			if (EditorUtility.DisplayDialog("Upgrade Selection to Advanced", "This utility sets the materials on every selected ProBuilder object.  Continue?", "Okay", "Cancel"))
			{
				DoUpgrade(Selection.get_gameObjects().SelectMany((GameObject x) => x.GetComponentsInChildren<pb_Object>()).ToArray());
				EditorUtility.DisplayDialog("Upgrade ProBuilder Objects", "Successfully upgraded all ProBuilder objects in selection", "Okay");
			}
		}

		private static void DoUpgrade(pb_Object[] all)
		{
			bool flag = all != null && all.Length > 8;
			for (int i = 0; i < all.Length; i++)
			{
				pb_Object val = all[i];
				if (flag)
				{
					EditorUtility.DisplayProgressBar("Applying Materials", "Setting pb_Object " + all[i].get_id() + ".", (float)i / (float)all.Length);
				}
				val.SetFaceMaterial(val.get_faces(), ((Renderer)((Component)val).get_gameObject().GetComponent<MeshRenderer>()).get_sharedMaterial());
				val.ToMesh();
				val.Refresh((RefreshMask)255);
				pb_EditorMeshUtility.Optimize(val, false);
			}
			if (flag)
			{
				EditorUtility.ClearProgressBar();
			}
		}

		public pb_UpgradeBasicToAdvanced()
			: this()
		{
		}
	}
}
