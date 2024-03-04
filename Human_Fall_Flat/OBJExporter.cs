using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class OBJExporter : ScriptableWizard
{
	public bool onlySelectedObjects;

	public bool applyPosition = true;

	public bool applyRotation = true;

	public bool applyScale = true;

	public bool generateMaterials = true;

	public bool exportTextures = true;

	public bool splitObjects = true;

	public bool autoMarkTexReadable;

	public bool objNameAddIdNum;

	private string versionString = "v2.0";

	private string lastExportFolder;

	private bool StaticBatchingEnabled()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		PlayerSettings[] array = Resources.FindObjectsOfTypeAll<PlayerSettings>();
		if (array == null)
		{
			return false;
		}
		Object[] array2 = (Object[])(object)array;
		SerializedProperty val = new SerializedObject(array2).FindProperty("m_BuildTargetBatching");
		for (int i = 0; i < val.get_arraySize(); i++)
		{
			SerializedProperty arrayElementAtIndex = val.GetArrayElementAtIndex(i);
			if (arrayElementAtIndex == null)
			{
				continue;
			}
			IEnumerator enumerator = arrayElementAtIndex.GetEnumerator();
			if (enumerator == null)
			{
				continue;
			}
			while (enumerator.MoveNext())
			{
				SerializedProperty val2 = (SerializedProperty)enumerator.Current;
				if (val2 != null && val2.get_name() == "m_StaticBatching")
				{
					return val2.get_boolValue();
				}
			}
		}
		return false;
	}

	private void OnWizardUpdate()
	{
		((ScriptableWizard)this).set_helpString("Aaro4130's OBJ Exporter " + versionString);
	}

	private Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return angle * (point - pivot) + pivot;
	}

	private Vector3 MultiplyVec3s(Vector3 v1, Vector3 v2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
	}

	private void OnWizardCreate()
	{
		if (StaticBatchingEnabled() && Application.get_isPlaying())
		{
			EditorUtility.DisplayDialog("Error", "Static batching is enabled. This will cause the export file to look like a mess, as well as be a large filesize. Disable this option, and restart the player, before continuing.", "OK");
		}
		else if (!autoMarkTexReadable || EditorUtility.DisplayDialogComplex("Warning", "This will convert all textures to Advanced type with the read/write option set. This is not reversible and will permanently affect your project. Continue?", "Yes", "No", "Cancel") <= 0)
		{
			string @string = EditorPrefs.GetString("a4_OBJExport_lastPath", "");
			string string2 = EditorPrefs.GetString("a4_OBJExport_lastFile", "unityexport.obj");
			string text = EditorUtility.SaveFilePanel("Export OBJ", @string, string2, "obj");
			if (text.Length > 0)
			{
				FileInfo fileInfo = new FileInfo(text);
				EditorPrefs.SetString("a4_OBJExport_lastFile", fileInfo.Name);
				EditorPrefs.SetString("a4_OBJExport_lastPath", fileInfo.Directory.FullName);
				Export(text);
			}
		}
	}

	private void Export(string exportPath)
	{
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		FileInfo fileInfo = new FileInfo(exportPath);
		lastExportFolder = fileInfo.Directory.FullName;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(exportPath);
		EditorUtility.DisplayProgressBar("Exporting OBJ", "Please wait.. Starting export.", 0f);
		MeshFilter[] array;
		if (onlySelectedObjects)
		{
			List<MeshFilter> list = new List<MeshFilter>();
			GameObject[] gameObjects = Selection.get_gameObjects();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				MeshFilter component = gameObjects[i].GetComponent<MeshFilter>();
				if ((Object)(object)component != (Object)null)
				{
					list.Add(component);
				}
			}
			array = list.ToArray();
		}
		else
		{
			array = Object.FindObjectsOfType(typeof(MeshFilter)) as MeshFilter[];
		}
		if (Application.get_isPlaying())
		{
			MeshFilter[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				MeshRenderer component2 = ((Component)array2[i]).get_gameObject().GetComponent<MeshRenderer>();
				if ((Object)(object)component2 != (Object)null && ((Renderer)component2).get_isPartOfStaticBatch())
				{
					EditorUtility.ClearProgressBar();
					EditorUtility.DisplayDialog("Error", "Static batched object detected. Static batching is not compatible with this exporter. Please disable it before starting the player.", "OK");
					return;
				}
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = new StringBuilder();
		stringBuilder.AppendLine("# Export of " + Application.get_loadedLevelName());
		stringBuilder.AppendLine("# from Aaro4130 OBJ Exporter " + versionString);
		if (generateMaterials)
		{
			stringBuilder.AppendLine("mtllib " + fileNameWithoutExtension + ".mtl");
		}
		float num = array.Length + 1;
		int num2 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			string name = ((Object)((Component)array[j]).get_gameObject()).get_name();
			float num3 = (float)(j + 1) / num;
			EditorUtility.DisplayProgressBar("Exporting objects... (" + Mathf.Round(num3 * 100f) + "%)", "Exporting object " + name, num3);
			MeshFilter val = array[j];
			MeshRenderer component3 = ((Component)array[j]).get_gameObject().GetComponent<MeshRenderer>();
			if (splitObjects)
			{
				string text = name;
				if (objNameAddIdNum)
				{
					text = text + "_" + j;
				}
				stringBuilder.AppendLine("g " + text);
			}
			if ((Object)(object)component3 != (Object)null && generateMaterials)
			{
				Material[] sharedMaterials = ((Renderer)component3).get_sharedMaterials();
				foreach (Material val2 in sharedMaterials)
				{
					if (!dictionary.ContainsKey(((Object)val2).get_name()))
					{
						dictionary[((Object)val2).get_name()] = true;
						stringBuilder2.Append(MaterialToString(val2));
						stringBuilder2.AppendLine();
					}
				}
			}
			Mesh sharedMesh = val.get_sharedMesh();
			int num4 = (int)Mathf.Clamp(((Component)val).get_gameObject().get_transform().get_lossyScale()
				.x * ((Component)val).get_gameObject().get_transform().get_lossyScale()
				.z, -1f, 1f);
			Vector3[] vertices = sharedMesh.get_vertices();
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 val3 = vertices[i];
				if (applyScale)
				{
					val3 = MultiplyVec3s(val3, ((Component)val).get_gameObject().get_transform().get_lossyScale());
				}
				if (applyRotation)
				{
					val3 = RotateAroundPoint(val3, Vector3.get_zero(), ((Component)val).get_gameObject().get_transform().get_rotation());
				}
				if (applyPosition)
				{
					val3 += ((Component)val).get_gameObject().get_transform().get_position();
				}
				val3.x *= -1f;
				stringBuilder.AppendLine("v " + val3.x + " " + val3.y + " " + val3.z);
			}
			vertices = sharedMesh.get_normals();
			for (int i = 0; i < vertices.Length; i++)
			{
				Vector3 val4 = vertices[i];
				if (applyScale)
				{
					Vector3 v = val4;
					Vector3 lossyScale = ((Component)val).get_gameObject().get_transform().get_lossyScale();
					val4 = MultiplyVec3s(v, ((Vector3)(ref lossyScale)).get_normalized());
				}
				if (applyRotation)
				{
					val4 = RotateAroundPoint(val4, Vector3.get_zero(), ((Component)val).get_gameObject().get_transform().get_rotation());
				}
				val4.x *= -1f;
				stringBuilder.AppendLine("vn " + val4.x + " " + val4.y + " " + val4.z);
			}
			Vector2[] uv = sharedMesh.get_uv();
			foreach (Vector2 val5 in uv)
			{
				float x = val5.x;
				string text2 = x.ToString();
				x = val5.y;
				stringBuilder.AppendLine("vt " + text2 + " " + x);
			}
			for (int l = 0; l < sharedMesh.get_subMeshCount(); l++)
			{
				if ((Object)(object)component3 != (Object)null && l < ((Renderer)component3).get_sharedMaterials().Length)
				{
					string name2 = ((Object)((Renderer)component3).get_sharedMaterials()[l]).get_name();
					stringBuilder.AppendLine("usemtl " + name2);
				}
				else
				{
					stringBuilder.AppendLine("usemtl " + name + "_sm" + l);
				}
				int[] triangles = sharedMesh.GetTriangles(l);
				for (int m = 0; m < triangles.Length; m += 3)
				{
					int index = triangles[m] + 1 + num2;
					int index2 = triangles[m + 1] + 1 + num2;
					int index3 = triangles[m + 2] + 1 + num2;
					if (num4 < 0)
					{
						stringBuilder.AppendLine("f " + ConstructOBJString(index) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index3));
					}
					else
					{
						stringBuilder.AppendLine("f " + ConstructOBJString(index3) + " " + ConstructOBJString(index2) + " " + ConstructOBJString(index));
					}
				}
			}
			num2 += sharedMesh.get_vertices().Length;
		}
		File.WriteAllText(exportPath, stringBuilder.ToString());
		if (generateMaterials)
		{
			File.WriteAllText(fileInfo.Directory.FullName + "\\" + fileNameWithoutExtension + ".mtl", stringBuilder2.ToString());
		}
		EditorUtility.ClearProgressBar();
	}

	private string TryExportTexture(string propertyName, Material m)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		if (m.HasProperty(propertyName))
		{
			Texture texture = m.GetTexture(propertyName);
			if ((Object)(object)texture != (Object)null)
			{
				return ExportTexture((Texture2D)texture);
			}
		}
		return "false";
	}

	private string ExportTexture(Texture2D t)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			if (autoMarkTexReadable)
			{
				string assetPath = AssetDatabase.GetAssetPath((Object)(object)t);
				AssetImporter atPath = AssetImporter.GetAtPath(assetPath);
				TextureImporter val = (TextureImporter)(object)((atPath is TextureImporter) ? atPath : null);
				if ((Object)(object)val != (Object)null)
				{
					val.set_textureType((TextureImporterType)0);
					if (!val.get_isReadable())
					{
						val.set_isReadable(true);
						AssetDatabase.ImportAsset(assetPath);
						AssetDatabase.Refresh();
					}
				}
			}
			string text = lastExportFolder + "\\" + ((Object)t).get_name() + ".png";
			Texture2D val2 = new Texture2D(((Texture)t).get_width(), ((Texture)t).get_height(), (TextureFormat)5, false);
			val2.SetPixels(t.GetPixels());
			File.WriteAllBytes(text, ImageConversion.EncodeToPNG(val2));
			return text;
		}
		catch (Exception)
		{
			Debug.Log((object)("Could not export texture : " + ((Object)t).get_name() + ". is it readable?"));
			return "null";
		}
	}

	private string ConstructOBJString(int index)
	{
		string text = index.ToString();
		return text + "/" + text + "/" + text;
	}

	private string MaterialToString(Material m)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("newmtl " + ((Object)m).get_name());
		if (m.HasProperty("_Color"))
		{
			stringBuilder.AppendLine("Kd " + m.get_color().r + " " + m.get_color().g + " " + m.get_color().b);
			if (m.get_color().a < 1f)
			{
				stringBuilder.AppendLine("Tr " + (1f - m.get_color().a));
				stringBuilder.AppendLine("d " + m.get_color().a);
			}
		}
		if (m.HasProperty("_SpecColor"))
		{
			Color color = m.GetColor("_SpecColor");
			stringBuilder.AppendLine("Ks " + color.r + " " + color.g + " " + color.b);
		}
		if (exportTextures)
		{
			string text = TryExportTexture("_MainTex", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Kd " + text);
			}
			text = TryExportTexture("_SpecMap", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Ks " + text);
			}
			text = TryExportTexture("_BumpMap", m);
			if (text != "false")
			{
				stringBuilder.AppendLine("map_Bump " + text);
			}
		}
		stringBuilder.AppendLine("illum 2");
		return stringBuilder.ToString();
	}

	[MenuItem("File/Export/Wavefront OBJ")]
	private static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Export OBJ", typeof(OBJExporter), "Export");
	}

	public OBJExporter()
		: this()
	{
	}
}
