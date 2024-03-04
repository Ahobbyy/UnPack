using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_StripProBuilderScripts : Editor
	{
		[MenuItem("Tools/ProBuilder/Actions/Strip All ProBuilder Scripts in Scene")]
		public static void StripAllScenes()
		{
			if (EditorUtility.DisplayDialog("Strip ProBuilder Scripts", "This will remove all ProBuilder scripts in the scene.  You will no longer be able to edit these objects.  There is no undo, please exercise caution!\n\nAre you sure you want to do this?", "Okay", "Cancel"))
			{
				Strip((pb_Object[])(object)Resources.FindObjectsOfTypeAll(typeof(pb_Object)));
			}
		}

		[MenuItem("Tools/ProBuilder/Actions/Strip ProBuilder Scripts in Selection", true, 0)]
		public static bool VerifyStripSelection()
		{
			return pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()).Length != 0;
		}

		[MenuItem("Tools/ProBuilder/Actions/Strip ProBuilder Scripts in Selection")]
		public static void StripAllSelected()
		{
			if (!EditorUtility.DisplayDialog("Strip ProBuilder Scripts", "This will remove all ProBuilder scripts on the selected objects.  You will no longer be able to edit these objects.  There is no undo, please exercise caution!\n\nAre you sure you want to do this?", "Okay", "Cancel"))
			{
				return;
			}
			Transform[] transforms = Selection.get_transforms();
			for (int i = 0; i < transforms.Length; i++)
			{
				pb_Object[] componentsInChildren = ((Component)transforms[i]).GetComponentsInChildren<pb_Object>(true);
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					DoStrip(componentsInChildren[j]);
				}
			}
		}

		public static void Strip(pb_Object[] all)
		{
			for (int i = 0; i < all.Length && !EditorUtility.DisplayCancelableProgressBar("Stripping ProBuilder Scripts", "Working over " + all[i].get_id() + ".", (float)i / (float)all.Length); i++)
			{
				DoStrip(all[i]);
			}
			EditorUtility.ClearProgressBar();
			EditorUtility.DisplayDialog("Strip ProBuilder Scripts", "Successfully stripped out all ProBuilder components.", "Okay");
			if (Object.op_Implicit((Object)(object)pb_Editor.get_instance()))
			{
				pb_Editor.get_instance().UpdateSelection(true);
			}
		}

		public static void DoStrip(pb_Object pb)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Invalid comparison between Unknown and I4
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				GameObject gameObject = ((Component)pb).get_gameObject();
				Renderer component = gameObject.GetComponent<Renderer>();
				if ((Object)(object)component != (Object)null)
				{
					pb_EditorUtility.SetSelectionRenderState(component, pb_EditorUtility.GetSelectionRenderState());
				}
				if ((int)PrefabUtility.GetPrefabType((Object)(object)gameObject) == 1)
				{
					return;
				}
				pb_EditorUtility.VerifyMesh(pb);
				if ((Object)(object)pb.get_msh() == (Object)null)
				{
					Object.DestroyImmediate((Object)(object)pb);
					if (Object.op_Implicit((Object)(object)gameObject.GetComponent<pb_Entity>()))
					{
						Object.DestroyImmediate((Object)(object)gameObject.GetComponent<pb_Entity>());
					}
					return;
				}
				string text = default(string);
				Mesh val = default(Mesh);
				if (pb_PreferencesInternal.GetBool("pbMeshesAreAssets") && pb_EditorMeshUtility.GetCachedMesh(pb, ref text, ref val))
				{
					pb.dontDestroyMeshOnDelete = true;
					Object.DestroyImmediate((Object)(object)pb);
					if (Object.op_Implicit((Object)(object)gameObject.GetComponent<pb_Entity>()))
					{
						Object.DestroyImmediate((Object)(object)gameObject.GetComponent<pb_Entity>());
					}
					return;
				}
				Mesh sharedMesh = pb_MeshUtility.DeepCopy(pb.get_msh());
				Object.DestroyImmediate((Object)(object)pb);
				if (Object.op_Implicit((Object)(object)gameObject.GetComponent<pb_Entity>()))
				{
					Object.DestroyImmediate((Object)(object)gameObject.GetComponent<pb_Entity>());
				}
				gameObject.GetComponent<MeshFilter>().set_sharedMesh(sharedMesh);
				if (Object.op_Implicit((Object)(object)gameObject.GetComponent<MeshCollider>()))
				{
					gameObject.GetComponent<MeshCollider>().set_sharedMesh(sharedMesh);
				}
			}
			catch
			{
			}
		}

		public pb_StripProBuilderScripts()
			: this()
		{
		}
	}
}
