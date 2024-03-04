using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ProBuilder2.EditorCommon
{
	public class pb_ScenePostProcessor
	{
		[PostProcessScene]
		public static void OnPostprocessScene()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Expected O, but got Unknown
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Invalid comparison between Unknown and I4
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Invalid comparison between Unknown and I4
			Material val = (Material)Resources.Load("Materials/InvisibleFace");
			Object[] array = Object.FindObjectsOfType(typeof(pb_Object));
			for (int i = 0; i < array.Length; i++)
			{
				pb_Object val2 = (pb_Object)array[i];
				if ((Object)(object)((Component)val2).GetComponent<MeshRenderer>() == (Object)null || !((Renderer)((Component)val2).GetComponent<MeshRenderer>()).get_sharedMaterials().Any((Material x) => (Object)(object)x != (Object)null && ((Object)x).get_name().Contains("NoDraw")))
				{
					continue;
				}
				Material[] sharedMaterials = ((Renderer)((Component)val2).GetComponent<MeshRenderer>()).get_sharedMaterials();
				for (int j = 0; j < sharedMaterials.Length; j++)
				{
					if (((Object)sharedMaterials[j]).get_name().Contains("NoDraw"))
					{
						sharedMaterials[j] = val;
					}
				}
				((Renderer)((Component)val2).GetComponent<MeshRenderer>()).set_sharedMaterials(sharedMaterials);
			}
			if (EditorApplication.get_isPlayingOrWillChangePlaymode())
			{
				return;
			}
			array = Object.FindObjectsOfType(typeof(pb_Object));
			for (int i = 0; i < array.Length; i++)
			{
				pb_Object val3 = (pb_Object)array[i];
				GameObject gameObject = ((Component)val3).get_gameObject();
				pb_Entity component = ((Component)val3).get_gameObject().GetComponent<pb_Entity>();
				if (!((Object)(object)component == (Object)null))
				{
					if ((int)component.get_entityType() == 3 || (int)component.get_entityType() == 2)
					{
						((Renderer)gameObject.GetComponent<MeshRenderer>()).set_enabled(false);
					}
					if ((Object)(object)val3.get_msh() != (Object)null)
					{
						((Object)val3.get_msh()).set_hideFlags((HideFlags)0);
					}
					if (!pb_PreferencesInternal.GetBool("pbStripProBuilderOnBuild"))
					{
						break;
					}
					val3.dontDestroyMeshOnDelete = true;
					Object.DestroyImmediate((Object)(object)val3);
					Object.DestroyImmediate((Object)(object)gameObject.GetComponent<pb_Entity>());
				}
			}
		}
	}
}
