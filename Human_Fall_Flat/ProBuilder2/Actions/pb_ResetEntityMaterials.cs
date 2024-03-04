using System.Collections;
using System.Linq;
using ProBuilder2.Common;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_ResetEntityMaterials : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Repair Entity Materials", false, 600)]
		public static void MenuRefreshMeshReferences()
		{
			RepairEntityMaterials();
		}

		private static void RepairEntityMaterials()
		{
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Invalid comparison between Unknown and I4
			IEnumerable enumerable = from x in Object.FindObjectsOfType(typeof(pb_Entity))
				where (int)((pb_Entity)x).get_entityType() == 3 || (int)((pb_Entity)x).get_entityType() == 2
				select x;
			Material colliderMaterial = pb_Constant.get_ColliderMaterial();
			Material triggerMaterial = pb_Constant.get_TriggerMaterial();
			if ((Object)(object)colliderMaterial == (Object)null)
			{
				Debug.LogError((object)"ProBuilder cannot find Collider material!  Make sure the Collider material asset is in \"Assets/ProCore/ProBuilder/Resources/Material\" folder.");
				return;
			}
			if ((Object)(object)triggerMaterial == (Object)null)
			{
				Debug.LogError((object)"ProBuilder cannot find Trigger material!  Make sure the Trigger material asset is in \"Assets/ProCore/ProBuilder/Resources/Material\" folder.");
				return;
			}
			foreach (pb_Entity item in enumerable)
			{
				pb_Entity val = item;
				((Renderer)(((Component)((Component)val).get_transform()).GetComponent<MeshRenderer>() ?? ((Component)val).get_gameObject().AddComponent<MeshRenderer>())).set_sharedMaterials((Material[])(object)new Material[1] { ((int)val.get_entityType() == 3) ? colliderMaterial : triggerMaterial });
			}
			EditorUtility.DisplayDialog("Repair Entity Materials", "Successfully reset special entity materials in scene.", "Okay");
		}

		public pb_ResetEntityMaterials()
			: this()
		{
		}
	}
}
