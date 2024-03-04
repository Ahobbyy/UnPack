using UnityEditor;
using UnityEngine;

public class ReplaceMaterialsInChildren : EditorWindow
{
	private Material _baseMaterial;

	private int _counter;

	private Material _replaceMaterial;

	private GameObject[] _selection;

	private GUIStyle _style;

	private string _text;

	[MenuItem("Lab42/Replace Materials In Children")]
	private static void Create()
	{
		((EditorWindow)(ReplaceMaterialsInChildren)(object)EditorWindow.GetWindow(typeof(ReplaceMaterialsInChildren))).Show();
	}

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_style = new GUIStyle();
		_style.set_wordWrap(true);
		_text = "Select one or more objects, if base material is found it will be replaced by replace material";
	}

	private void OnGUI()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		EditorGUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
		_baseMaterial = (Material)EditorGUILayout.ObjectField("Base Material", (Object)(object)_baseMaterial, typeof(Material), false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		_replaceMaterial = (Material)EditorGUILayout.ObjectField("Replace Material", (Object)(object)_replaceMaterial, typeof(Material), false, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUILayout.Button("Replace Materials", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			if (!Object.op_Implicit((Object)(object)_baseMaterial))
			{
				_text = "No base material selected, please fill base material slot";
				return;
			}
			if (!Object.op_Implicit((Object)(object)_replaceMaterial))
			{
				_text = "No replace material selected, please fill replace material slot";
				return;
			}
			_selection = Selection.get_gameObjects();
			if (_selection.Length != 0)
			{
				_counter = 0;
				GameObject[] selection = _selection;
				foreach (GameObject val in selection)
				{
					SwitchMaterials(val);
					SwitchChildMaterials(val.get_transform());
				}
				_text = _counter + " materials replaced";
			}
			else
			{
				_text = "No object selected, please select one or more objects that should have their materials replaced.";
			}
		}
		EditorGUILayout.LabelField(_text, _style, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.EndVertical();
	}

	private void SwitchMaterials(GameObject go)
	{
		if (!Object.op_Implicit((Object)(object)go.GetComponent<MeshRenderer>()))
		{
			return;
		}
		Material[] sharedMaterials = ((Renderer)go.GetComponent<MeshRenderer>()).get_sharedMaterials();
		if (sharedMaterials == null || sharedMaterials.Length == 0)
		{
			return;
		}
		for (int i = 0; i < sharedMaterials.Length; i++)
		{
			if ((Object)(object)sharedMaterials[i] != (Object)null && ((Object)sharedMaterials[i]).get_name().Equals(((Object)_baseMaterial).get_name()))
			{
				sharedMaterials[i] = _replaceMaterial;
				_counter++;
			}
		}
		((Renderer)go.GetComponent<MeshRenderer>()).set_sharedMaterials(sharedMaterials);
	}

	private void SwitchChildMaterials(Transform root)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		foreach (Transform item in root)
		{
			Transform val = item;
			SwitchMaterials(((Component)val).get_gameObject());
			SwitchChildMaterials(val);
		}
	}

	public ReplaceMaterialsInChildren()
		: this()
	{
	}
}
