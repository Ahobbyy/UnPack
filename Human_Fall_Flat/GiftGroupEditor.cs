using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GiftGroup))]
public class GiftGroupEditor : Editor
{
	private GiftModel[] models;

	private List<float> modelProbabilityMap = new List<float>();

	private float modelProbabilitySum;

	public override void OnInspectorGUI()
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("RandomizeModels", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			models = ((Component)(((Editor)this).get_target() as GiftGroup)).get_gameObject().GetComponentsInChildren<GiftModel>(true);
			modelProbabilityMap.Clear();
			modelProbabilitySum = 0f;
			for (int i = 0; i < models.Length; i++)
			{
				modelProbabilitySum += models[i].probability;
				modelProbabilityMap.Add(modelProbabilitySum);
			}
			Gift[] componentsInChildren = ((Component)(((Editor)this).get_target() as GiftGroup)).get_gameObject().GetComponentsInChildren<Gift>(true);
			foreach (Gift obj in componentsInChildren)
			{
				GiftModel randomModel = GetRandomModel();
				((Component)obj).get_gameObject().GetComponent<MeshCollider>().set_sharedMesh(((Component)randomModel).get_gameObject().GetComponent<MeshCollider>().get_sharedMesh());
				((Component)obj).get_gameObject().GetComponent<MeshFilter>().set_sharedMesh(((Component)randomModel).get_gameObject().GetComponent<MeshFilter>().get_sharedMesh());
				((Renderer)((Component)obj).get_gameObject().GetComponent<MeshRenderer>()).set_sharedMaterial(((Renderer)((Component)randomModel).get_gameObject().GetComponent<MeshRenderer>()).get_sharedMaterial());
				float num = Random.Range(0.8f, 1.3f);
				((Component)obj).get_transform().set_localScale(new Vector3(num, num, 1f / num) * Random.Range(0.7f, 1f));
				((Component)obj).get_transform().set_localRotation(Quaternion.Euler(-90f, (float)Random.Range(-180, 180), 0f));
			}
		}
	}

	public static void RandomizeGift(GiftGroup group, Gift gift)
	{
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		GiftModel[] componentsInChildren = ((Component)group).get_gameObject().GetComponentsInChildren<GiftModel>(true);
		List<float> list = new List<float>();
		float num = 0f;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			num += componentsInChildren[i].probability;
			list.Add(num);
		}
		((Component)group).get_gameObject().GetComponentsInChildren<Gift>(true);
		GiftModel giftModel = null;
		float num2 = Random.Range(0f, num);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (num2 < list[j])
			{
				giftModel = componentsInChildren[j];
				break;
			}
		}
		if ((Object)(object)giftModel == (Object)null)
		{
			giftModel = componentsInChildren[0];
		}
		((Component)gift).get_gameObject().GetComponent<MeshCollider>().set_sharedMesh(((Component)giftModel).get_gameObject().GetComponent<MeshCollider>().get_sharedMesh());
		((Component)gift).get_gameObject().GetComponent<MeshFilter>().set_sharedMesh(((Component)giftModel).get_gameObject().GetComponent<MeshFilter>().get_sharedMesh());
		((Renderer)((Component)gift).get_gameObject().GetComponent<MeshRenderer>()).set_sharedMaterial(((Renderer)((Component)giftModel).get_gameObject().GetComponent<MeshRenderer>()).get_sharedMaterial());
		float num3 = Random.Range(0.8f, 1.3f);
		((Component)gift).get_transform().set_localScale(new Vector3(num3, num3, 1f / num3) * Random.Range(0.7f, 1f));
		((Component)gift).get_transform().set_localRotation(Quaternion.Euler(-90f, (float)Random.Range(-180, 180), 0f));
	}

	public GiftModel GetRandomModel()
	{
		float num = Random.Range(0f, modelProbabilitySum);
		for (int i = 0; i < models.Length; i++)
		{
			if (num < modelProbabilityMap[i])
			{
				return models[i];
			}
		}
		return models[0];
	}

	public GiftGroupEditor()
		: this()
	{
	}
}
