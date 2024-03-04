using System.Collections.Generic;
using HumanAPI;
using UnityEditor;
using UnityEngine;

namespace PrefabSampler
{
	public static class PrefabSamplerTool
	{
		[MenuItem("GameObject/Make Prefab", false, 0)]
		private static void ContextClick()
		{
			SamplePrefab(Selection.get_gameObjects());
		}

		public static void SamplePrefab(GameObject[] objectsToSample)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			if (objectsToSample == null || objectsToSample.Length == 0)
			{
				return;
			}
			string uniqueName = GetUniqueName<GameObject>(((Object)Selection.get_gameObjects()[0]).get_name() + PrefabSamplerConfigData.EditorData.AppendName, "prefab", PrefabSamplerConfigData.EditorData.DestinationFolder);
			string text = PrefabSamplerConfigData.EditorData.DestinationFolder + "/" + uniqueName + ".prefab";
			List<GameObject> list = new List<GameObject>(objectsToSample);
			Vector3 val = Vector3.get_zero();
			if (PrefabSamplerConfigData.EditorData.IncludeNodeGraphObjects)
			{
				List<GameObject> nodeGraphObjects = new List<GameObject>();
				AddNodeGraphRelatedNodes(ref nodeGraphObjects, list);
				foreach (GameObject item in nodeGraphObjects)
				{
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
			}
			List<GameObject> list2 = new List<GameObject>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				GameObject val2 = Object.Instantiate<GameObject>(list[i], list[i].get_transform().get_position(), list[i].get_transform().get_rotation());
				((Object)val2).set_name(((Object)list[i]).get_name());
				val += val2.get_transform().get_position();
				list2.Add(val2);
			}
			val /= (float)list.Count;
			GameObject val3 = (GameObject)((list2.Count > 1) ? ((object)new GameObject(uniqueName)) : ((object)list2[0]));
			((Object)val3).set_name(uniqueName);
			val3.get_transform().set_position(val);
			if (list2.Count > 1)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					list2[j].get_transform().set_parent(val3.get_transform());
				}
			}
			if (PrefabSamplerConfigData.EditorData.FixMeshPosition)
			{
				AdjustChildren(val3.get_transform(), uniqueName, topLevel: true);
			}
			if (PrefabSamplerConfigData.EditorData.AllignWithChildren && (Object)(object)val3.GetComponent<Collider>() == (Object)null && (Object)(object)val3.GetComponent<MeshRenderer>() == (Object)null)
			{
				val = Vector3.get_zero();
				for (int k = 0; k < val3.get_transform().get_childCount(); k++)
				{
					val += val3.get_transform().GetChild(k).get_position();
				}
				val /= (float)val3.get_transform().get_childCount();
				Vector3 val4 = val3.get_transform().InverseTransformVector(val3.get_transform().get_position() - val);
				for (int l = 0; l < val3.get_transform().get_childCount(); l++)
				{
					Transform child = val3.get_transform().GetChild(l);
					child.set_localPosition(child.get_localPosition() + val4);
				}
			}
			PrefabUtility.CreatePrefab(text, val3);
			Object.DestroyImmediate((Object)(object)val3);
		}

		private static void AdjustChildren(Transform objTransform, string prefabName, bool topLevel)
		{
			for (int i = 0; i < objTransform.get_childCount(); i++)
			{
				AdjustChildren(objTransform.GetChild(i), prefabName, topLevel: false);
			}
			Mesh val = null;
			Mesh val2 = null;
			MeshFilter component = ((Component)objTransform).GetComponent<MeshFilter>();
			MeshCollider component2 = ((Component)objTransform).GetComponent<MeshCollider>();
			if (!((Object)(object)component != (Object)null))
			{
				return;
			}
			val = (((Object)(object)component.get_sharedMesh() != (Object)null) ? component.get_sharedMesh() : component.get_mesh());
			val2 = (((Object)(object)component2 != (Object)null && (Object)(object)val != (Object)(object)component2.get_sharedMesh()) ? component2.get_sharedMesh() : null);
			Mesh val3 = Object.Instantiate<Mesh>(val);
			Mesh val4 = (((Object)(object)val2 != (Object)null) ? Object.Instantiate<Mesh>(val2) : null);
			MeshRotateAndScale(val3, val4, objTransform);
			string text = PrefabSamplerConfigData.EditorData.DestinationFolder + "/Meshes";
			if (!AssetDatabase.IsValidFolder(text))
			{
				AssetDatabase.CreateFolder(PrefabSamplerConfigData.EditorData.DestinationFolder, "Meshes");
			}
			string uniqueName = GetUniqueName<Mesh>(((Object)objTransform).get_name(), "asset", text);
			AssetDatabase.CreateAsset((Object)(object)val3, text + "/" + uniqueName + ".asset");
			((Component)objTransform).GetComponent<MeshFilter>().set_sharedMesh(val3);
			if ((Object)(object)component2 != (Object)null && (Object)(object)component2.get_sharedMesh() == (Object)(object)val)
			{
				component2.set_sharedMesh(val3);
			}
			if ((Object)(object)val4 != (Object)null)
			{
				string text2 = text + "/MeshColliders";
				if (!AssetDatabase.IsValidFolder(text2))
				{
					AssetDatabase.CreateFolder(text, "MeshColliders");
				}
				AssetDatabase.CreateAsset((Object)(object)val3, text2 + "/" + uniqueName + ".asset");
				((Component)objTransform).GetComponent<MeshCollider>().set_sharedMesh(val4);
			}
		}

		private static void MeshRotateAndScale(Mesh mesh, Mesh meshCollider, Transform prefabTransform)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_021d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			Bounds bounds = mesh.get_bounds();
			Matrix4x4 localToWorldMatrix = prefabTransform.get_localToWorldMatrix();
			Vector3 val = prefabTransform.InverseTransformVector(Vector3.get_up());
			Vector3 val2 = (Vector3)(PrefabSamplerConfigData.EditorData.PivotType switch
			{
				MeshPivotType.Centre => ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(((Bounds)(ref bounds)).get_center()), 
				MeshPivotType.Top => ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(((Bounds)(ref bounds)).get_center() + Vector3.Project(((Bounds)(ref bounds)).get_extents(), val)), 
				MeshPivotType.Bottom => ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(((Bounds)(ref bounds)).get_center() - Vector3.Project(((Bounds)(ref bounds)).get_extents(), val)), 
				_ => ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(Vector3.get_zero()), 
			});
			Vector3 val3 = val2 - prefabTransform.get_position();
			for (int i = 0; i < prefabTransform.get_childCount(); i++)
			{
				Transform child = prefabTransform.GetChild(i);
				child.set_position(child.get_position() - val3);
			}
			prefabTransform.set_position(val2);
			if (PrefabSamplerConfigData.EditorData.FixScale)
			{
				prefabTransform.set_localScale(Vector3.get_one());
			}
			Vector3[] vertices = mesh.get_vertices();
			for (int j = 0; j < mesh.get_vertexCount(); j++)
			{
				Vector3 val4 = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(vertices[j]);
				vertices[j] = prefabTransform.InverseTransformPoint(val4);
			}
			BoxCollider component = ((Component)prefabTransform).GetComponent<BoxCollider>();
			if ((Object)(object)component != (Object)null)
			{
				Vector3 val5 = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(component.get_center());
				component.set_center(prefabTransform.InverseTransformPoint(val5));
			}
			SphereCollider component2 = ((Component)prefabTransform).GetComponent<SphereCollider>();
			if ((Object)(object)component2 != (Object)null)
			{
				Vector3 val6 = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(component2.get_center());
				component2.set_center(prefabTransform.InverseTransformPoint(val6));
			}
			CapsuleCollider component3 = ((Component)prefabTransform).GetComponent<CapsuleCollider>();
			if ((Object)(object)component3 != (Object)null)
			{
				Vector3 val7 = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(component3.get_center());
				component3.set_center(prefabTransform.InverseTransformPoint(val7));
			}
			mesh.set_vertices(vertices);
			mesh.RecalculateNormals();
			mesh.RecalculateTangents();
			mesh.RecalculateBounds();
			mesh.UploadMeshData(false);
			if ((Object)(object)meshCollider != (Object)null)
			{
				Vector3[] vertices2 = meshCollider.get_vertices();
				for (int k = 0; k < meshCollider.get_vertexCount(); k++)
				{
					Vector3 val8 = ((Matrix4x4)(ref localToWorldMatrix)).MultiplyPoint(vertices2[k]);
					vertices2[k] = prefabTransform.InverseTransformPoint(val8);
				}
				meshCollider.set_vertices(vertices2);
				meshCollider.RecalculateNormals();
				meshCollider.RecalculateTangents();
				meshCollider.RecalculateBounds();
				meshCollider.UploadMeshData(false);
			}
		}

		private static void AddNodeGraphRelatedNodes(ref List<GameObject> nodeGraphObjects, IEnumerable<GameObject> objectsToPrefabs)
		{
			foreach (GameObject objectsToPrefab in objectsToPrefabs)
			{
				if (!((Object)(object)objectsToPrefab.GetComponent<Node>() != (Object)null))
				{
					continue;
				}
				Transform parent = objectsToPrefab.get_transform().get_parent();
				if (!((Object)(object)parent != (Object)null) || !((Object)(object)((Component)parent).GetComponentInParent<Node>() != (Object)null))
				{
					continue;
				}
				for (int i = 0; i < parent.get_childCount(); i++)
				{
					if (Object.op_Implicit((Object)(object)((Component)parent.GetChild(i)).GetComponent<Node>()))
					{
						nodeGraphObjects.Add(((Component)parent.GetChild(i)).get_gameObject());
					}
				}
			}
		}

		private static string GetUniqueName<T>(string assetName, string extension, string location)
		{
			string text = assetName;
			if (!(AssetDatabase.LoadAssetAtPath(location + "/" + assetName + "." + extension, typeof(T)) == (Object)null))
			{
				int num = 1;
				bool flag = false;
				while (!flag)
				{
					text = assetName + num;
					if (AssetDatabase.LoadAssetAtPath(location + "/" + text + "." + extension, typeof(T)) == (Object)null)
					{
						flag = true;
					}
					else
					{
						num++;
					}
				}
			}
			return text;
		}
	}
}
