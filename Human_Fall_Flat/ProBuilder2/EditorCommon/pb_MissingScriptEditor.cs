using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProBuilder2.Actions;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.EditorCommon
{
	[CustomEditor(typeof(MonoBehaviour))]
	public class pb_MissingScriptEditor : Editor
	{
		private static bool applyDummyScript = true;

		private static float index = 0f;

		private static float total;

		private static bool doFix = false;

		private static List<GameObject> unfixable = new List<GameObject>();

		private static MonoScript _mono_pb;

		private static MonoScript _mono_pe;

		private static MonoScript _mono_dummy;

		private List<string> PB_OBJECT_SCRIPT_PROPERTIES = new List<string> { "_sharedIndices", "_vertices", "_uv", "_sharedIndicesUV", "_quads" };

		private List<string> PB_ENTITY_SCRIPT_PROPERTIES = new List<string> { "pb", "userSetDimensions", "_entityType", "forceConvex" };

		private static bool skipEvent = false;

		public MonoScript pb_monoscript
		{
			get
			{
				if ((Object)(object)_mono_pb == (Object)null)
				{
					LoadMonoScript();
				}
				return _mono_pb;
			}
		}

		public MonoScript pe_monoscript
		{
			get
			{
				if ((Object)(object)_mono_pe == (Object)null)
				{
					LoadMonoScript();
				}
				return _mono_pe;
			}
		}

		public MonoScript dummy_monoscript
		{
			get
			{
				if ((Object)(object)_mono_dummy == (Object)null)
				{
					LoadMonoScript();
				}
				return _mono_dummy;
			}
		}

		private static void LoadMonoScript()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			GameObject val = new GameObject();
			pb_Object val2 = val.AddComponent<pb_Object>();
			pb_Entity val3 = val.GetComponent<pb_Entity>();
			if ((Object)(object)val3 == (Object)null)
			{
				val3 = val.AddComponent<pb_Entity>();
			}
			pb_DummyScript obj = val.AddComponent<pb_DummyScript>();
			_mono_pb = MonoScript.FromMonoBehaviour((MonoBehaviour)(object)val2);
			_mono_pe = MonoScript.FromMonoBehaviour((MonoBehaviour)(object)val3);
			_mono_dummy = MonoScript.FromMonoBehaviour((MonoBehaviour)(object)obj);
			Object.DestroyImmediate((Object)(object)val);
		}

		[MenuItem("Tools/ProBuilder/Repair/Repair Missing Script References")]
		public static void MenuRepairMissingScriptReferences()
		{
			FixAllScriptReferencesInScene();
		}

		private static void FixAllScriptReferencesInScene()
		{
			EditorApplication.ExecuteMenuItem("Window/Inspector");
			Object[] array = (from x in Resources.FindObjectsOfTypeAll(typeof(GameObject))
				where ((GameObject)x).GetComponents<Component>().Any((Component n) => (Object)(object)n == (Object)null)
				select x).ToArray();
			total = array.Length;
			unfixable.Clear();
			if (total > 1f)
			{
				Undo.RecordObjects(array, "Fix missing script references");
				index = 0f;
				doFix = true;
				Next();
			}
			else
			{
				if (applyDummyScript)
				{
					DeleteDummyScripts();
				}
				EditorUtility.DisplayDialog("Success", "No missing ProBuilder script references found.", "Okay");
			}
		}

		private static void Next()
		{
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Expected O, but got Unknown
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Invalid comparison between Unknown and I4
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Invalid comparison between Unknown and I4
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected O, but got Unknown
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			if (EditorUtility.DisplayCancelableProgressBar("Repair ProBuilder Script References", "Fixing " + (int)Mathf.Floor(index + 1f) + " out of " + total + " objects in scene.", index / total))
			{
				flag = true;
				doFix = false;
			}
			if (!flag)
			{
				Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));
				for (int i = 0; i < array.Length; i++)
				{
					GameObject val = (GameObject)array[i];
					if (!val.GetComponents<Component>().Any((Component x) => (Object)(object)x == (Object)null) || unfixable.Contains(val))
					{
						continue;
					}
					if ((int)PrefabUtility.GetPrefabType((Object)(object)val) == 3 || (int)PrefabUtility.GetPrefabType((Object)(object)val) == 1)
					{
						GameObject val2 = (GameObject)PrefabUtility.GetPrefabParent((Object)(object)val);
						if (Object.op_Implicit((Object)(object)val2) && (Object.op_Implicit((Object)(object)val2.GetComponent<pb_Object>()) || Object.op_Implicit((Object)(object)val2.GetComponent<pb_Entity>())))
						{
							unfixable.Add(val);
							continue;
						}
					}
					if ((int)((Object)val).get_hideFlags() != 0)
					{
						unfixable.Add(val);
						continue;
					}
					Selection.set_activeObject((Object)(object)val);
					return;
				}
			}
			pb_Object[] array2 = (pb_Object[])(object)Resources.FindObjectsOfTypeAll(typeof(pb_Object));
			for (int j = 0; j < array2.Length; j++)
			{
				EditorUtility.DisplayProgressBar("Checking ProBuilder Meshes", "Refresh " + (j + 1) + " out of " + total + " objects in scene.", (float)j / (float)array2.Length);
				try
				{
					array2[j].ToMesh();
					array2[j].Refresh((RefreshMask)255);
					pb_EditorMeshUtility.Optimize(array2[j], false);
				}
				catch (Exception ex)
				{
					Debug.LogWarning((object)("Failed reconstituting " + ((Object)array2[j]).get_name() + ".  Proceeding with upgrade anyways.  Usually this means a prefab is already fixed, and just needs to be instantiated to take effect.\n" + ex.ToString()));
				}
			}
			EditorUtility.ClearProgressBar();
			if (applyDummyScript)
			{
				DeleteDummyScripts();
			}
			EditorUtility.DisplayDialog("Success", "Successfully repaired " + total + " ProBuilder objects.", "Okay");
			if (!pb_EditorSceneUtility.SaveCurrentSceneIfUserWantsTo())
			{
				Debug.LogWarning((object)"Repaired script references will be lost on exit if this scene is not saved!");
			}
			doFix = false;
			skipEvent = true;
		}

		public override void OnInspectorGUI()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Invalid comparison between Unknown and I4
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Invalid comparison between Unknown and I4
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			if (skipEvent && (int)Event.get_current().get_type() == 7)
			{
				skipEvent = false;
				return;
			}
			SerializedProperty val = ((Editor)this).get_serializedObject().FindProperty("m_Script");
			if (val == null || val.get_objectReferenceValue() != (Object)null)
			{
				if (doFix)
				{
					if ((int)Event.get_current().get_type() == 7)
					{
						Next();
					}
				}
				else
				{
					((Editor)this).OnInspectorGUI();
				}
				return;
			}
			int num = 0;
			int num2 = 0;
			SerializedProperty iterator = ((Editor)this).get_serializedObject().GetIterator();
			iterator.Next(true);
			while (iterator.Next(true))
			{
				if (PB_OBJECT_SCRIPT_PROPERTIES.Contains(iterator.get_name()))
				{
					num++;
				}
				if (PB_ENTITY_SCRIPT_PROPERTIES.Contains(iterator.get_name()))
				{
					num2++;
				}
			}
			if (num >= 3 || num2 >= 3)
			{
				EditorGUILayout.HelpBox("Missing Script Reference\n\nProBuilder can automatically fix this missing reference.  To fix all references in the scene, click \"Fix All in Scene\".  To fix just this one, click \"Reconnect\".", (MessageType)2);
				GUI.set_backgroundColor(Color.get_green());
				if (!doFix && GUILayout.Button("Fix All in Scene", (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					FixAllScriptReferencesInScene();
					return;
				}
				GUI.set_backgroundColor(Color.get_cyan());
				if ((doFix && (int)Event.get_current().get_type() == 7) || GUILayout.Button("Reconnect", (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					if (num >= 3)
					{
						index += 1f;
					}
					else if (doFix && (Object)(object)((Component)((Editor)this).get_target()).get_gameObject().GetComponent<pb_Object>() == (Object)null)
					{
						return;
					}
					if (!doFix)
					{
						Undo.RegisterCompleteObjectUndo(((Editor)this).get_target(), "Fix missing reference.");
					}
					val.set_objectReferenceValue((Object)(object)((num >= 3) ? pb_monoscript : pe_monoscript));
					val.get_serializedObject().ApplyModifiedProperties();
					val = ((Editor)this).get_serializedObject().FindProperty("m_Script");
					val.get_serializedObject().Update();
					if (doFix)
					{
						Next();
					}
					GUIUtility.ExitGUI();
				}
				GUI.set_backgroundColor(Color.get_white());
			}
			else if (doFix)
			{
				if (applyDummyScript)
				{
					index += 0.5f;
					val.set_objectReferenceValue((Object)(object)dummy_monoscript);
					val.get_serializedObject().ApplyModifiedProperties();
					val = ((Editor)this).get_serializedObject().FindProperty("m_Script");
					val.get_serializedObject().Update();
				}
				else
				{
					unfixable.Add(((Component)((Editor)this).get_target()).get_gameObject());
				}
				Next();
				GUIUtility.ExitGUI();
			}
			else
			{
				((Editor)this).OnInspectorGUI();
			}
		}

		private static void DeleteDummyScripts()
		{
			pb_DummyScript[] source = (pb_DummyScript[])(object)Resources.FindObjectsOfTypeAll(typeof(pb_DummyScript));
			source = source.Where((pb_DummyScript x) => (int)((Object)x).get_hideFlags() == 0).ToArray();
			if (source.Length == 0)
			{
				return;
			}
			switch (EditorUtility.DisplayDialogComplex("Found Unrepairable Objects", "Repair script found " + source.Length + " missing components that could not be repaired.  Would you like to delete those components now, or attempt to rebuild (ProBuilderize) them?", "Delete", "Cancel", "ProBuilderize"))
			{
			case 2:
				ProBuilderize.DoProBuilderize(from x in Array.ConvertAll((from x in Resources.FindObjectsOfTypeAll(typeof(GameObject))
						where !((object)x).Equals((object)null) && x is GameObject && ((GameObject)x).GetComponents<pb_DummyScript>().Length == 2 && (Object)(object)((GameObject)x).GetComponent<MeshRenderer>() != (Object)null && (Object)(object)((GameObject)x).GetComponent<MeshFilter>() != (Object)null && (Object)(object)((GameObject)x).GetComponent<MeshFilter>().get_sharedMesh() != (Object)null
						select x).ToArray().Distinct().ToArray(), (Converter<Object, GameObject>)((Object x) => (GameObject)x))
					select x.GetComponent<MeshFilter>(), Settings.get_Default());
				break;
			case 1:
				return;
			}
			Object[] array = (Object[])(object)source.Select((pb_DummyScript x) => ((Component)x).get_gameObject()).ToArray();
			Undo.RecordObjects(array, "Delete Broken Scripts");
			for (int i = 0; i < source.Length; i++)
			{
				Object.DestroyImmediate((Object)(object)source[i]);
			}
		}

		private static string SerializedObjectToString(SerializedObject serializedObject)
		{
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Invalid comparison between Unknown and I4
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			StringBuilder stringBuilder = new StringBuilder();
			if (serializedObject == null)
			{
				stringBuilder.Append("NULL");
				return stringBuilder.ToString();
			}
			SerializedProperty iterator = serializedObject.GetIterator();
			iterator.Next(true);
			while (iterator.Next(true))
			{
				string text = "";
				for (int i = 0; i < iterator.get_depth(); i++)
				{
					text += "\t";
				}
				stringBuilder.AppendLine(text + iterator.get_name() + (((int)iterator.get_propertyType() == 5 && iterator.get_type().Contains("Component") && iterator.get_objectReferenceValue() == (Object)null) ? " -> NULL" : ""));
				text += "  - ";
				string[] obj = new string[8]
				{
					text,
					"Type: (",
					iterator.get_type(),
					" / ",
					null,
					null,
					null,
					null
				};
				SerializedPropertyType propertyType = iterator.get_propertyType();
				obj[4] = ((object)(SerializedPropertyType)(ref propertyType)).ToString();
				obj[5] = " /  / ";
				obj[6] = iterator.get_name();
				obj[7] = ")";
				stringBuilder.AppendLine(string.Concat(obj));
				stringBuilder.AppendLine(text + iterator.get_propertyPath());
				stringBuilder.AppendLine(text + "Value: " + SerializedPropertyValue(iterator));
			}
			return stringBuilder.ToString();
		}

		private static string SerializedPropertyValue(SerializedProperty sp)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected I4, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			SerializedPropertyType propertyType = sp.get_propertyType();
			switch ((int)propertyType)
			{
			case 0:
				return sp.get_intValue().ToString();
			case 1:
				return sp.get_boolValue().ToString();
			case 2:
				return sp.get_floatValue().ToString();
			case 3:
				return sp.get_stringValue().ToString();
			case 4:
			{
				Color colorValue = sp.get_colorValue();
				return ((object)(Color)(ref colorValue)).ToString();
			}
			case 5:
				if (!(sp.get_objectReferenceValue() == (Object)null))
				{
					return sp.get_objectReferenceValue().get_name();
				}
				return "null";
			case 6:
				return sp.get_intValue().ToString();
			case 7:
				return sp.get_enumValueIndex().ToString();
			case 8:
			{
				Vector2 vector2Value = sp.get_vector2Value();
				return ((object)(Vector2)(ref vector2Value)).ToString();
			}
			case 9:
			{
				Vector3 vector3Value = sp.get_vector3Value();
				return ((object)(Vector3)(ref vector3Value)).ToString();
			}
			case 11:
			{
				Rect rectValue = sp.get_rectValue();
				return ((object)(Rect)(ref rectValue)).ToString();
			}
			case 12:
				return sp.get_intValue().ToString();
			case 13:
				return "Character";
			case 14:
				return ((object)sp.get_animationCurveValue()).ToString();
			case 15:
			{
				Bounds boundsValue = sp.get_boundsValue();
				return ((object)(Bounds)(ref boundsValue)).ToString();
			}
			case 16:
				return "Gradient";
			default:
				return "Unknown type";
			}
		}

		public pb_MissingScriptEditor()
			: this()
		{
		}
	}
}
