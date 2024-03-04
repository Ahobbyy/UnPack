using System.IO;
using HumanAPI;
using UnityEditor;
using UnityEngine;

public class ImportRagdollModel
{
	[MenuItem("Assets/Import Ragdoll/Head", true)]
	[MenuItem("Assets/Import Ragdoll/Upper", true)]
	[MenuItem("Assets/Import Ragdoll/UpperBody", true)]
	[MenuItem("Assets/Import Ragdoll/Lower", true)]
	[MenuItem("Assets/Import Ragdoll/Full", true)]
	public static bool ImportRagdollValidate()
	{
		AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(Selection.get_activeObject()));
		if ((Object)(object)((atPath is ModelImporter) ? atPath : null) != (Object)null)
		{
			return true;
		}
		return false;
	}

	[MenuItem("Assets/Import Ragdoll/Head")]
	public static void ImportRagdollHead()
	{
		ImportRagdoll(WorkshopItemType.ModelHead);
	}

	[MenuItem("Assets/Import Ragdoll/UpperBody")]
	public static void ImportRagdollUpperBody()
	{
		ImportRagdoll(WorkshopItemType.ModelUpperBody);
	}

	[MenuItem("Assets/Import Ragdoll/UpperBodyBones")]
	public static void ImportRagdollUpperBodyBones()
	{
		ImportRagdoll(WorkshopItemType.ModelUpperBody, bones: true);
	}

	[MenuItem("Assets/Import Ragdoll/LowerBody")]
	public static void ImportRagdollLowerBody()
	{
		ImportRagdoll(WorkshopItemType.ModelLowerBody);
	}

	[MenuItem("Assets/Import Ragdoll/Full")]
	public static void ImportRagdollFull()
	{
		ImportRagdoll(WorkshopItemType.ModelFull);
	}

	public static void ImportRagdoll(WorkshopItemType part, bool bones = false)
	{
		Object activeObject = Selection.get_activeObject();
		GameObject val = (GameObject)(object)((activeObject is GameObject) ? activeObject : null);
		string assetPath = AssetDatabase.GetAssetPath(Selection.get_activeObject());
		string text = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath) + "Color") + ".png";
		string text2 = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath) + "Mask") + ".png";
		string text3 = Path.ChangeExtension(assetPath, ".prefab");
		string text4 = Path.ChangeExtension(assetPath, ".asset");
		Texture2D val2 = AssetDatabase.LoadAssetAtPath<Texture2D>(text);
		Texture2D val3 = AssetDatabase.LoadAssetAtPath<Texture2D>(text2);
		if ((Object)(object)val2 == (Object)null)
		{
			text = Path.Combine("Assets/SkinTextures", Path.GetFileNameWithoutExtension(assetPath) + "Color") + ".png";
			val2 = AssetDatabase.LoadAssetAtPath<Texture2D>(text);
			if ((Object)(object)val2 == (Object)null)
			{
				Debug.LogErrorFormat("Missing Color texture {0}", new object[1] { text });
				return;
			}
		}
		if ((Object)(object)val3 == (Object)null)
		{
			text2 = Path.Combine("Assets/SkinTextures", Path.GetFileNameWithoutExtension(assetPath) + "Mask") + ".png";
			val3 = AssetDatabase.LoadAssetAtPath<Texture2D>(text2);
			if ((Object)(object)val3 == (Object)null)
			{
				Debug.LogErrorFormat("Missing Mask texture {0}", new object[1] { text2 });
				return;
			}
		}
		if ((Object)(object)Object.FindObjectOfType<PhotoRoom>() == (Object)null)
		{
			Debug.LogError((object)"Load PhotoRoom scene to import models!");
			return;
		}
		GameObject val4 = Object.Instantiate<GameObject>(val);
		((Object)val4).set_name(((Object)val).get_name());
		RagdollModel ragdollModel = val4.AddComponent<RagdollModel>();
		ragdollModel.ragdollPart = part;
		ragdollModel.mask1 = true;
		ragdollModel.mask2 = true;
		ragdollModel.mask3 = true;
		ragdollModel.baseTexture = val2;
		ragdollModel.maskTexture = val3;
		SetTexture(ragdollModel, val2);
		MeshRenderer[] componentsInChildren = val4.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer val5 in componentsInChildren)
		{
			if (((Object)val5).get_name().Contains("Clip"))
			{
				((Component)val5).get_gameObject().AddComponent<RigClipVolume>();
				((Renderer)val5).set_enabled(false);
			}
		}
		if (bones)
		{
			ragdollModel.rigType = RagdollModel.Rigging.BoneMap;
			ragdollModel.partHead = val4.get_transform().FindRecursive("Head");
			ragdollModel.partChest = val4.get_transform().FindRecursive("Head");
			ragdollModel.partWaist = val4.get_transform().FindRecursive("Waist");
			ragdollModel.partHips = val4.get_transform().FindRecursive("Hips");
			ragdollModel.partLeftArm = val4.get_transform().FindRecursive("LeftArm");
			ragdollModel.partLeftForearm = val4.get_transform().FindRecursive("LeftForearm");
			ragdollModel.partLeftHand = val4.get_transform().FindRecursive("LeftHand");
			ragdollModel.partLeftThigh = val4.get_transform().FindRecursive("LeftThigh");
			ragdollModel.partLeftLeg = val4.get_transform().FindRecursive("LeftLeg");
			ragdollModel.partLeftFoot = val4.get_transform().FindRecursive("LeftFoot");
			ragdollModel.partRightArm = val4.get_transform().FindRecursive("RightArm");
			ragdollModel.partRightForearm = val4.get_transform().FindRecursive("RightForearm");
			ragdollModel.partRightHand = val4.get_transform().FindRecursive("RightHand");
			ragdollModel.partRightThigh = val4.get_transform().FindRecursive("RightThigh");
			ragdollModel.partRightLeg = val4.get_transform().FindRecursive("RightLeg");
			ragdollModel.partRightFoot = val4.get_transform().FindRecursive("RightFoot");
		}
		GameObject val6 = PrefabUtility.CreatePrefab(text3, val4);
		WorkshopRagdollModel workshopRagdollModel = AssetDatabase.LoadAssetAtPath<WorkshopRagdollModel>(text4);
		if ((Object)(object)workshopRagdollModel == (Object)null)
		{
			workshopRagdollModel = new WorkshopRagdollModel();
			AssetDatabase.CreateAsset((Object)(object)workshopRagdollModel, text4);
			workshopRagdollModel = AssetDatabase.LoadAssetAtPath<WorkshopRagdollModel>(text4);
		}
		((Object)workshopRagdollModel).set_name(((Object)ragdollModel).get_name());
		workshopRagdollModel.title = ((Object)ragdollModel).get_name();
		workshopRagdollModel.model = val6.GetComponent<RagdollModel>();
		WorkshopRagdollModelEditor.CaptureThumbnail(workshopRagdollModel);
		Object.DestroyImmediate((Object)(object)val4);
		workshopRagdollModel.model.maskTexture = (workshopRagdollModel.model.baseTexture = null);
		SetTexture(workshopRagdollModel.model, null);
	}

	private static void SetTexture(RagdollModel ragdolModel, Texture2D texture)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)((Component)ragdolModel).GetComponentInChildren<SkinnedMeshRenderer>() != (Object)null)
		{
			((Renderer)((Component)ragdolModel).GetComponentInChildren<SkinnedMeshRenderer>()).get_sharedMaterial().set_mainTexture((Texture)(object)texture);
			((Renderer)((Component)ragdolModel).GetComponentInChildren<SkinnedMeshRenderer>()).get_sharedMaterial().set_color(Color.get_white());
			((Renderer)((Component)ragdolModel).GetComponentInChildren<SkinnedMeshRenderer>()).get_sharedMaterial().SetFloat("_Glossiness", 0.1f);
		}
		else if ((Object)(object)((Component)ragdolModel).GetComponentInChildren<MeshRenderer>() != (Object)null)
		{
			((Renderer)((Component)ragdolModel).GetComponentInChildren<MeshRenderer>()).get_sharedMaterial().set_mainTexture((Texture)(object)texture);
			((Renderer)((Component)ragdolModel).GetComponentInChildren<MeshRenderer>()).get_sharedMaterial().set_color(Color.get_white());
			((Renderer)((Component)ragdolModel).GetComponentInChildren<MeshRenderer>()).get_sharedMaterial().SetFloat("_Glossiness", 0.1f);
		}
	}
}
