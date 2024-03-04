using UnityEditor;
using UnityEngine;

public static class eUL_MainMenu
{
	private static GameObject CreateGameObject(string name)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		GameObject val = new GameObject(name);
		Object activeObject = Selection.get_activeObject();
		GameObject val2 = (GameObject)(object)((activeObject is GameObject) ? activeObject : null);
		if ((Object)(object)val2 != (Object)null)
		{
			val.get_transform().SetParent(val2.get_transform(), false);
		}
		return val;
	}

	[MenuItem("GameObject/UPGEN Lighting/Fast Light", false, 10)]
	public static void CreateFastLight()
	{
		CreateGameObject("Fast Light").AddComponent<UL_FastLight>();
	}

	[MenuItem("GameObject/UPGEN Lighting/Manager", false, 10)]
	public static void CreateManager()
	{
		CreateGameObject("UPGEN Lighting Manager").AddComponent<UL_Manager>();
	}
}
