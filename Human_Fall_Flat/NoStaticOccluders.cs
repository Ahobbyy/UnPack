using UnityEditor;
using UnityEngine;

public class NoStaticOccluders
{
	private const string kWasOccluderTag = "WasOccluder";

	private const string kUntaggedTag = "Untagged";

	public static void Init()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		GameObject[] array = Object.FindObjectsOfType<GameObject>();
		foreach (GameObject val in array)
		{
			if (GameObjectUtility.AreStaticEditorFlagsSet(val, (StaticEditorFlags)2))
			{
				StaticEditorFlags staticEditorFlags = GameObjectUtility.GetStaticEditorFlags(val);
				staticEditorFlags = (StaticEditorFlags)(staticEditorFlags & -3);
				GameObjectUtility.SetStaticEditorFlags(val, staticEditorFlags);
				if (!string.IsNullOrEmpty(val.get_tag()) && !val.get_tag().Equals("Untagged"))
				{
					Debug.Log((object)("Tagged: " + val.get_tag() + " " + (object)val), (Object)(object)val);
				}
				else
				{
					val.set_tag("WasOccluder");
				}
				EditorUtility.SetDirty((Object)(object)val);
			}
		}
	}
}
