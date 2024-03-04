using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yondernauts.LayerManager
{
	public class LayerManager : EditorWindow
	{
		private enum ManagerState
		{
			Editing,
			Confirmation,
			Processing,
			Complete
		}

		private const string instructions = "Instructions:\n\n- Rename and reorganise layers as desired using the reorderable list below.\n- Applying the modifications will update the physics settings; change the layers on all game objects in all scenes and all prefabs; change any LayerMask serialized properties on game objects and scriptable objects.\n- Once the manager has finished processing files, you will be able to save out a layer map which you can use to transform layer filters and masks from the old layout to the new layout.\n";

		private ManagerState m_State = ManagerState.Complete;

		private Vector2 m_EditScroll = Vector2.get_zero();

		private bool m_SkipRepaint;

		private LayerManagerData m_Data;

		private ReorderableList m_LayerList;

		private string[] m_AssetPaths;

		private int m_CurrentAssetPath = -1;

		private int[] m_IndexSwaps;

		private int[] m_IndexSwapsRedirected;

		private string[] m_FixedLayers;

		private int m_SceneCount;

		private int m_PrefabCount;

		private int m_ObjectCount;

		private int m_ComponentCount;

		private int m_AssetCount;

		private int m_LayerMaskCount;

		private bool m_PhysicsMatrixCompleted;

		private bool m_Physics2DMatrixCompleted;

		private string m_CompletionReport = string.Empty;

		private uint[] m_PhysicsMasks;

		private uint[] m_Physics2DMasks;

		private List<string> m_Errors = new List<string>();

		private string[] scenes = new string[28]
		{
			"CurveStartScene.unity", "Startup.unity", "Intro.unity", "Push.unity", "Carry.unity", "Climb.unity", "Break.unity", "Siege.unity", "River.unity", "Power.unity",
			"Aztec.unity", "Credits.unity", "Empty.unity", "Lobby.unity", "Customization.unity", "Xmas.unity", "Zodiac.unity", "Halloween.unity", "WorkshopLobby.unity", "Steam_merged.unity",
			"Ice_merged.unity", "Intro_Reprise.unity", "City.unity", "Forest.unity", "Golf.unity", "Factory.unity", "Thermal.unity", "Lab.unity"
		};

		private float lineHeight => EditorGUIUtility.get_singleLineHeight();

		private float lineSpacing => EditorGUIUtility.get_standardVerticalSpacing();

		public bool dirty
		{
			get
			{
				return m_Data.dirty;
			}
			private set
			{
				m_Data.dirty = value;
			}
		}

		public bool valid => m_Data.valid;

		[MenuItem("Tools/Yondernauts/Layer Manager")]
		private static void ShowEdtor()
		{
			LayerManager obj = (LayerManager)(object)EditorWindow.GetWindow(typeof(LayerManager));
			obj.ResetData();
			((EditorWindow)obj).Show();
		}

		private void Initialise()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Expected O, but got Unknown
			if ((Object)(object)m_Data == (Object)null)
			{
				m_Data = ScriptableObject.CreateInstance<LayerManagerData>();
				m_Data.Initialise();
			}
			m_FixedLayers = new string[8];
			for (int i = 0; i < 8; i++)
			{
				m_FixedLayers[i] = LayerMask.LayerToName(i);
			}
			m_LayerList = new ReorderableList(m_Data.serializedObject, m_Data.layerMapProperty, true, true, false, false);
			m_LayerList.drawHeaderCallback = new HeaderCallbackDelegate(DrawLayerMapHeader);
			m_LayerList.drawElementCallback = new ElementCallbackDelegate(DrawLayerMapElement);
			m_LayerList.elementHeight = lineHeight * 2f + lineSpacing * 3f;
			m_LayerList.onReorderCallback = new ReorderCallbackDelegate(OnLayerMapReorder);
			m_State = ManagerState.Editing;
			m_SceneCount = 0;
			m_PrefabCount = 0;
			m_ObjectCount = 0;
			m_ComponentCount = 0;
			m_AssetCount = 0;
			m_LayerMaskCount = 0;
			m_PhysicsMatrixCompleted = false;
			m_Physics2DMatrixCompleted = false;
			m_Errors.Clear();
			m_CompletionReport = string.Empty;
		}

		private void OnEnable()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			((EditorWindow)this).set_titleContent(EditorGUIUtility.IconContent("HorizontalSplit"));
			((EditorWindow)this).get_titleContent().set_text("Layer Mgr");
			((EditorWindow)this).set_minSize(new Vector2(400f, 320f));
			Initialise();
			((EditorWindow)this).set_autoRepaintOnSceneChange(true);
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Combine((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndo));
		}

		private void OnDestroy()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			while (m_AssetPaths != null)
			{
				IncrementLayerModifications();
			}
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Remove((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndo));
		}

		private void OnUndo()
		{
			((EditorWindow)this).Repaint();
		}

		private void OnQuit()
		{
			((EditorWindow)this).Close();
		}

		private void OnGUI()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Invalid comparison between Unknown and I4
			if (m_SkipRepaint && (int)Event.get_current().get_type() == 7)
			{
				m_SkipRepaint = false;
				return;
			}
			switch (m_State)
			{
			case ManagerState.Editing:
				OnEditingGUI();
				break;
			case ManagerState.Confirmation:
				OnConfirmationGUI();
				break;
			case ManagerState.Processing:
				OnProcessingGUI();
				break;
			case ManagerState.Complete:
				OnCompleteGUI();
				break;
			}
		}

		private void OnEditingGUI()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Invalid comparison between Unknown and I4
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			if (m_LayerList == null || m_LayerList.get_serializedProperty() == null)
			{
				ResetData();
				if ((int)Event.get_current().get_type() == 8)
				{
					m_SkipRepaint = true;
				}
				return;
			}
			m_EditScroll = EditorGUILayout.BeginScrollView(m_EditScroll, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.HelpBox("Instructions:\n\n- Rename and reorganise layers as desired using the reorderable list below.\n- Applying the modifications will update the physics settings; change the layers on all game objects in all scenes and all prefabs; change any LayerMask serialized properties on game objects and scriptable objects.\n- Once the manager has finished processing files, you will be able to save out a layer map which you can use to transform layer filters and masks from the old layout to the new layout.\n", (MessageType)1);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			float num = ((Rect)(ref lastRect)).get_height() + 8f;
			float num2 = m_LayerList.elementHeight * 25f;
			Rect controlRect = EditorGUILayout.GetControlRect(false, num2 + num + 52f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Rect)(ref controlRect)).set_x(8f);
			((Rect)(ref controlRect)).set_y(num);
			((Rect)(ref controlRect)).set_height(num2);
			((Rect)(ref controlRect)).set_width(((Rect)(ref controlRect)).get_width() - 6f);
			m_LayerList.DoList(controlRect);
			((Rect)(ref controlRect)).set_y(((Rect)(ref controlRect)).get_y() + ((Rect)(ref controlRect)).get_height());
			((Rect)(ref controlRect)).set_height(lineHeight + 8f);
			GUI.set_enabled(dirty && valid);
			if (GUI.Button(controlRect, "Apply Layer Modifications"))
			{
				m_State = ManagerState.Confirmation;
			}
			GUI.set_enabled(true);
			((Rect)(ref controlRect)).set_y(((Rect)(ref controlRect)).get_y() + (lineHeight + 12f));
			if (GUI.Button(controlRect, "Reset Layer Moditications"))
			{
				ResetData();
			}
			EditorGUILayout.EndScrollView();
			m_Data.ApplyModifiedProperties();
		}

		private void OnConfirmationGUI()
		{
			EditorGUILayout.HelpBox("Warning: This process is not reversible and modifies a lot of files.\n\nMake sure all scenes are saved (including the open scene) and you have an up to date backup in case anything gose wrong.", (MessageType)2);
			if (GUILayout.Button("Yes, I have a backup", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				ApplyLayerModifications();
			}
			if (GUILayout.Button("No, I'm not ready yet", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_State = ManagerState.Editing;
			}
		}

		private void OnProcessingGUI()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Invalid comparison between Unknown and I4
			EditorGUILayout.HelpBox("Processing layer modifications. Do not close this window until completed.", (MessageType)1);
			Rect lastRect = GUILayoutUtility.GetLastRect();
			float y = ((Rect)(ref lastRect)).get_height() + 8f;
			Rect position = ((EditorWindow)this).get_position();
			((Rect)(ref position)).set_y(y);
			((Rect)(ref position)).set_height(EditorGUIUtility.get_singleLineHeight());
			((Rect)(ref position)).set_x(4f);
			((Rect)(ref position)).set_width(((Rect)(ref position)).get_width() - 8f);
			EditorGUI.ProgressBar(position, (float)m_CurrentAssetPath / (float)m_AssetPaths.Length, "Progress");
			if ((int)Event.get_current().get_type() == 7)
			{
				IncrementLayerModifications();
				((EditorWindow)this).Repaint();
			}
		}

		private void OnCompleteGUI()
		{
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			//IL_00a0: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Expected O, but got Unknown
			//IL_00bd: Expected O, but got Unknown
			EditorGUILayout.HelpBox(m_CompletionReport, (MessageType)1);
			EditorGUILayout.Space();
			if (GUILayout.Button("Close the Layer Manager", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				((EditorWindow)this).Close();
			}
			if (GUILayout.Button("Keep making changes", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				ResetData();
			}
			if (GUILayout.Button("Save Layer Map", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				CreateMap();
			}
			if (m_Errors.Count == 0)
			{
				GUI.set_enabled(false);
			}
			if (GUILayout.Button("Handle Errors", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				GenericMenu val = new GenericMenu();
				val.AddItem(new GUIContent("Output To Console"), false, (MenuFunction)delegate
				{
					Debug.LogError((object)BuildErrorReport(url: false));
				});
				val.AddItem(new GUIContent("Email Support"), false, (MenuFunction)delegate
				{
					Application.OpenURL("mailto:support@yondernauts.games?subject=Layer%20Manager&body=" + BuildErrorReport(url: true));
				});
				val.ShowAsContext();
			}
			if (m_Errors.Count == 0)
			{
				GUI.set_enabled(true);
			}
		}

		private string BuildErrorReport(bool url)
		{
			StringBuilder stringBuilder = new StringBuilder("Layer Manager failed with the following errors:");
			foreach (string error in m_Errors)
			{
				stringBuilder.Append(" - ").AppendLine(error);
			}
			if (url)
			{
				return Uri.EscapeDataString(stringBuilder.ToString());
			}
			return stringBuilder.ToString();
		}

		private void DrawLayerMapHeader(Rect rect)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			EditorGUI.LabelField(rect, "Modified Layer Map");
		}

		private void DrawLayerMapElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Expected O, but got Unknown
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Expected O, but got Unknown
			//IL_015d: Expected O, but got Unknown
			//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Expected O, but got Unknown
			//IL_01b9: Expected O, but got Unknown
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Expected O, but got Unknown
			//IL_024a: Expected O, but got Unknown
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Expected O, but got Unknown
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0338: Unknown result type (might be due to invalid IL or missing references)
			LayerManagerData.SerializedLayerMapEntry entry = m_Data.GetEntryFromIndex(index);
			((Rect)(ref rect)).set_height(lineHeight);
			((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + lineSpacing);
			EditorGUI.LabelField(rect, new GUIContent($"Modified [{index + 8:D2}]"), EditorStyles.get_boldLabel());
			Color backgroundColor = GUI.get_backgroundColor();
			if (!entry.valid)
			{
				GUI.set_backgroundColor(Color.get_red());
			}
			Rect val = rect;
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + 120f);
			((Rect)(ref val)).set_width(((Rect)(ref val)).get_width() - 204f);
			string text = EditorGUI.TextField(val, entry.name);
			if (entry.name != text)
			{
				entry.name = text;
				dirty = true;
			}
			GUI.set_backgroundColor(backgroundColor);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + (((Rect)(ref val)).get_width() + 4f));
			((Rect)(ref val)).set_width(80f);
			GUI.set_enabled(!string.IsNullOrEmpty(entry.oldName));
			if (EditorGUI.DropdownButton(val, new GUIContent("Redirect"), (FocusType)2))
			{
				GenericMenu val2 = new GenericMenu();
				val2.AddItem(new GUIContent("None"), false, (MenuFunction)delegate
				{
					entry.redirect = -1;
					dirty = true;
				});
				val2.AddSeparator("");
				for (int i = 0; i < 8; i++)
				{
					if (!string.IsNullOrEmpty(m_FixedLayers[i]))
					{
						int id = i;
						val2.AddItem(new GUIContent(m_FixedLayers[i]), false, (MenuFunction)delegate
						{
							entry.redirect = id;
							dirty = true;
						});
					}
				}
				val2.AddSeparator("");
				LayerManagerData.SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
				for (int j = 0; j < allEntries.Length; j++)
				{
					if (j == index)
					{
						continue;
					}
					LayerManagerData.SerializedLayerMapEntry targetEntry = allEntries[j];
					if (targetEntry.redirect == -1 && !string.IsNullOrEmpty(targetEntry.name))
					{
						val2.AddItem(new GUIContent(targetEntry.name), false, (MenuFunction)delegate
						{
							entry.redirect = targetEntry.oldIndex;
							dirty = true;
						});
					}
				}
				val2.ShowAsContext();
			}
			GUI.set_enabled(true);
			((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + (lineHeight + lineSpacing));
			EditorGUI.LabelField(rect, new GUIContent($"Original [{entry.oldIndex:D2}]"));
			Rect val3 = rect;
			((Rect)(ref val3)).set_x(((Rect)(ref val3)).get_x() + 120f);
			((Rect)(ref val3)).set_width(((Rect)(ref val3)).get_width() - 204f);
			EditorGUI.LabelField(val3, entry.oldName);
			((Rect)(ref val3)).set_x(((Rect)(ref val3)).get_x() + (((Rect)(ref val3)).get_width() + 4f));
			((Rect)(ref val3)).set_width(80f);
			int redirect = entry.redirect;
			if (redirect == -1)
			{
				EditorGUI.LabelField(val3, "No Redirect");
			}
			else if (redirect < 8)
			{
				EditorGUI.LabelField(val3, m_FixedLayers[redirect]);
			}
			else
			{
				EditorGUI.LabelField(val3, m_Data.GetEntryFromOldIndex(redirect).name);
			}
		}

		private void OnLayerMapReorder(ReorderableList list)
		{
			dirty = true;
			m_Data.RebuildSerializedEntries();
		}

		private void ResetData()
		{
			if ((Object)(object)m_Data != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_Data);
				m_Data = null;
			}
			Initialise();
		}

		private void ApplyLayerModifications()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			GetLayerCollisionMatrix();
			Get2DLayerCollisionMatrix();
			Object val = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0];
			if (val == (Object)null)
			{
				m_State = ManagerState.Complete;
				m_CompletionReport = "Failed to process layer modifcations. Asset not found: ProjectSettings/TagManager.asset";
				return;
			}
			SerializedObject val2 = new SerializedObject(val);
			SerializedProperty val3 = val2.FindProperty("layers");
			if (val3 == null)
			{
				m_State = ManagerState.Complete;
				m_CompletionReport = "Failed to process layer modifcations. No layers property found in tag manager asset.";
				return;
			}
			LayerManagerData.SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
			try
			{
				for (int i = 0; i < 24; i++)
				{
					val3.GetArrayElementAtIndex(i + 8).set_stringValue(allEntries[i].name);
				}
				val2.ApplyModifiedPropertiesWithoutUndo();
			}
			catch (Exception ex)
			{
				m_State = ManagerState.Complete;
				m_CompletionReport = "Failed to process layer modifcations. Exception when updating layer settings: " + ex.Message;
				return;
			}
			m_IndexSwaps = new int[32];
			m_IndexSwapsRedirected = new int[32];
			for (int j = 0; j < 8; j++)
			{
				m_IndexSwaps[j] = j;
				m_IndexSwapsRedirected[j] = j;
			}
			for (int k = 0; k < 24; k++)
			{
				m_IndexSwaps[allEntries[k].oldIndex] = k + 8;
			}
			for (int l = 0; l < 24; l++)
			{
				if (allEntries[l].redirect == -1)
				{
					m_IndexSwapsRedirected[allEntries[l].oldIndex] = l + 8;
				}
				else
				{
					m_IndexSwapsRedirected[allEntries[l].oldIndex] = TransformLayer(allEntries[l].redirect, redirected: false);
				}
			}
			ProcessLayerCollisionMatrix();
			Process2DLayerCollisionMatrix();
			m_AssetPaths = AssetDatabase.GetAllAssetPaths();
			m_CurrentAssetPath = 0;
			m_State = ManagerState.Processing;
		}

		private void IncrementLayerModifications()
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			if (m_AssetPaths == null)
			{
				return;
			}
			if (m_CurrentAssetPath >= m_AssetPaths.Length)
			{
				m_AssetPaths = null;
				m_CurrentAssetPath = 0;
				return;
			}
			string text = m_AssetPaths[m_CurrentAssetPath];
			try
			{
				if (text.StartsWith("Assets/"))
				{
					if (text.EndsWith(".prefab"))
					{
						try
						{
							int objectCount = m_ObjectCount;
							int componentCount = m_ComponentCount;
							GameObject val = (GameObject)AssetDatabase.LoadMainAssetAtPath(text);
							if ((Object)(object)val != (Object)null)
							{
								ProcessGameObject(val, inScene: false);
								if (m_ObjectCount > objectCount || m_ComponentCount > componentCount)
								{
									m_PrefabCount++;
								}
							}
						}
						catch (Exception ex)
						{
							m_Errors.Add($"Encountered error processing prefab: \"{text}\", message: {ex.Message}");
						}
					}
					else if (text.EndsWith(".asset"))
					{
						try
						{
							Object[] array = AssetDatabase.LoadAllAssetsAtPath(text);
							if (array != null)
							{
								Object[] array2 = array;
								foreach (Object val2 in array2)
								{
									if (!(val2 == (Object)null))
									{
										SerializedObject so = new SerializedObject(val2);
										if (ProcessSerializedObject(so))
										{
											m_AssetCount++;
										}
									}
								}
							}
						}
						catch (Exception ex2)
						{
							m_Errors.Add($"Encountered error processing Scriptable Object: \"{text}\", message: {ex2.Message}");
						}
					}
					else if (WorkingScene(text))
					{
						try
						{
							int objectCount2 = m_ObjectCount;
							int componentCount2 = m_ComponentCount;
							Scene val3 = EditorSceneManager.OpenScene(text);
							GameObject[] rootGameObjects = ((Scene)(ref val3)).GetRootGameObjects();
							foreach (GameObject val4 in rootGameObjects)
							{
								if ((Object)(object)val4 != (Object)null)
								{
									ProcessGameObject(val4, inScene: true);
								}
							}
							if (m_ObjectCount > objectCount2 || m_ComponentCount > componentCount2)
							{
								m_SceneCount++;
								if (!EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), ""))
								{
									Debug.LogWarning((object)("Failed to save scene: " + text));
								}
							}
						}
						catch (Exception ex3)
						{
							m_Errors.Add($"Encountered error processing scene: \"{text}\", message: {ex3.Message}");
						}
					}
				}
			}
			catch (Exception ex4)
			{
				m_Errors.Add($"Encountered error processing asset: \"{text}\", message: {ex4.Message}");
			}
			m_CurrentAssetPath++;
			if (m_CurrentAssetPath >= m_AssetPaths.Length)
			{
				m_AssetPaths = null;
				m_CurrentAssetPath = 0;
				AssetDatabase.SaveAssets();
				m_State = ManagerState.Complete;
				BuildReport();
			}
		}

		private bool WorkingScene(string path)
		{
			string[] array = scenes;
			foreach (string value in array)
			{
				if (path.EndsWith(value))
				{
					return true;
				}
			}
			return false;
		}

		private void ProcessGameObject(GameObject go, bool inScene)
		{
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Expected O, but got Unknown
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Expected O, but got Unknown
			try
			{
				Transform transform = go.get_transform();
				int childCount = transform.get_childCount();
				for (int i = 0; i < childCount; i++)
				{
					try
					{
						Transform child = transform.GetChild(i);
						if (!((Object)(object)child == (Object)null))
						{
							ProcessGameObject(((Component)child).get_gameObject(), inScene);
						}
					}
					catch (Exception ex)
					{
						m_Errors.Add($"Encountered error processing GameObject child: \"{((Object)go).get_name()}\", child index: {i}, in scene: {inScene}, message: {ex.Message}");
					}
				}
				SerializedObject val = new SerializedObject((Object)(object)go);
				SerializedProperty val2 = val.FindProperty("m_Layer");
				int intValue = val2.get_intValue();
				int num = TransformLayer(intValue, redirected: true);
				if (num != intValue)
				{
					val2.set_intValue(num);
					val.ApplyModifiedPropertiesWithoutUndo();
					m_ObjectCount++;
				}
				Component[] components = go.GetComponents<Component>();
				for (int j = 0; j < components.Length; j++)
				{
					try
					{
						if (!((Object)(object)components[j] == (Object)null) && ProcessSerializedObject(new SerializedObject((Object)(object)components[j])))
						{
							m_ComponentCount++;
						}
					}
					catch (Exception ex2)
					{
						m_Errors.Add($"Encountered error processing component on GameObject: \"{((Object)go).get_name()}\", component index: {j}, in scene: {inScene}, message: {ex2.Message}");
					}
				}
			}
			catch (Exception ex3)
			{
				m_Errors.Add($"Encountered error processing GameObject: \"{((Object)go).get_name()}\", message: {ex3.Message}");
			}
		}

		private bool ProcessSerializedObject(SerializedObject so)
		{
			try
			{
				int layerMaskCount = m_LayerMaskCount;
				SerializedProperty iterator = so.GetIterator();
				if (iterator != null)
				{
					bool flag = false;
					while (!flag)
					{
						if (iterator.get_type() == "LayerMask")
						{
							int intValue = iterator.get_intValue();
							int num = TransformMask(intValue);
							if (intValue != num)
							{
								iterator.set_intValue(num);
								m_LayerMaskCount++;
							}
						}
						flag = !iterator.Next(true);
					}
				}
				if (m_LayerMaskCount > layerMaskCount)
				{
					so.ApplyModifiedPropertiesWithoutUndo();
					return true;
				}
			}
			catch (Exception ex)
			{
				m_Errors.Add($"Encountered error processing SerializedObject: \"{so.get_targetObject().get_name()}\", message: {ex.Message}");
			}
			return false;
		}

		private int TransformLayer(int old, bool redirected)
		{
			if (redirected)
			{
				return m_IndexSwapsRedirected[old];
			}
			return m_IndexSwaps[old];
		}

		private int TransformMask(int old)
		{
			int num = 0;
			for (int i = 0; i < 32; i++)
			{
				int num2 = (old >> i) & 1;
				num |= num2 << TransformLayer(i, redirected: true);
			}
			return num;
		}

		private uint TransformMatrix(uint old)
		{
			uint num = 0u;
			for (int i = 0; i < 32; i++)
			{
				uint num2 = (old >> i) & 1u;
				num |= num2 << TransformLayer(i, redirected: false);
			}
			return num;
		}

		private void CreateMap()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			LayerMap layerMap = ScriptableObject.CreateInstance<LayerMap>();
			SerializedObject val = new SerializedObject((Object)(object)layerMap);
			SerializedProperty val2 = val.FindProperty("m_Map");
			val2.set_arraySize(32);
			for (int i = 0; i < 32; i++)
			{
				val2.GetArrayElementAtIndex(i).set_intValue(m_IndexSwapsRedirected[i]);
			}
			val.ApplyModifiedPropertiesWithoutUndo();
			AssetDatabase.CreateAsset((Object)(object)layerMap, AssetDatabase.GenerateUniqueAssetPath("Assets/LayerMap.asset"));
			AssetDatabase.SaveAssets();
		}

		private void GetLayerCollisionMatrix()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			m_PhysicsMasks = null;
			try
			{
				Object val = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/DynamicsManager.asset")[0];
				if (val == (Object)null)
				{
					m_Errors.Add("Failed to read physics layer collision matrix. Asset file was missing or null");
					m_PhysicsMasks = null;
					return;
				}
				SerializedObject val2 = new SerializedObject(val);
				if (val2 == null)
				{
					return;
				}
				SerializedProperty val3 = val2.FindProperty("m_LayerCollisionMatrix");
				if (val3 == null)
				{
					return;
				}
				m_PhysicsMasks = new uint[32];
				for (int i = 0; i < 32; i++)
				{
					m_PhysicsMasks[i] = (uint)val3.GetArrayElementAtIndex(i).get_longValue();
				}
				for (int j = 0; j < 32; j++)
				{
					if (string.IsNullOrEmpty(LayerMask.LayerToName(j)))
					{
						m_PhysicsMasks[j] = uint.MaxValue;
					}
				}
				for (int k = 0; k < 32; k++)
				{
					for (int l = 0; l < 32; l++)
					{
						if (k != l)
						{
							uint num = (m_PhysicsMasks[l] >> k) & 1u;
							m_PhysicsMasks[k] |= num << l;
						}
					}
				}
			}
			catch (Exception ex)
			{
				m_Errors.Add("Failed to read physics layer collision matrix. Exception when updating settings: " + ex.Message);
				m_PhysicsMasks = null;
			}
		}

		private void ProcessLayerCollisionMatrix()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			if (m_PhysicsMasks == null)
			{
				return;
			}
			try
			{
				SerializedObject val = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/DynamicsManager.asset")[0]);
				if (val == null)
				{
					m_Errors.Add("Failed to process physics layer collision matrix. Asset not found: ProjectSettings/DynamicsManager.asset");
					return;
				}
				SerializedProperty val2 = val.FindProperty("m_LayerCollisionMatrix");
				if (val2 == null)
				{
					m_Errors.Add("Failed to process physics layer collision matrix. Matrix property not found in dynamics manager asset.");
					return;
				}
				for (int i = 0; i < 32; i++)
				{
					uint old = m_PhysicsMasks[i];
					uint num = TransformMatrix(old);
					val2.GetArrayElementAtIndex(TransformLayer(i, redirected: false)).set_longValue((long)num);
				}
				val.ApplyModifiedPropertiesWithoutUndo();
				m_PhysicsMatrixCompleted = true;
			}
			catch (Exception ex)
			{
				m_Errors.Add("Failed to process physics layer collision matrix. Exception when updating settings: " + ex.Message);
			}
		}

		private void Get2DLayerCollisionMatrix()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			m_Physics2DMasks = null;
			try
			{
				SerializedObject val = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Physics2DSettings.asset")[0]);
				if (val == null)
				{
					return;
				}
				SerializedProperty val2 = val.FindProperty("m_LayerCollisionMatrix");
				if (val2 == null)
				{
					return;
				}
				m_Physics2DMasks = new uint[32];
				for (int i = 0; i < 32; i++)
				{
					m_Physics2DMasks[i] = (uint)val2.GetArrayElementAtIndex(i).get_longValue();
				}
				for (int j = 0; j < 32; j++)
				{
					if (string.IsNullOrEmpty(LayerMask.LayerToName(j)))
					{
						m_Physics2DMasks[j] = uint.MaxValue;
					}
				}
				for (int k = 0; k < 32; k++)
				{
					for (int l = 0; l < 32; l++)
					{
						if (k != l)
						{
							uint num = (m_Physics2DMasks[l] >> k) & 1u;
							m_Physics2DMasks[k] |= num << l;
						}
					}
				}
			}
			catch (Exception ex)
			{
				m_Errors.Add("Failed to read physics 2D layer collision matrix. Exception when updating settings: " + ex.Message);
				m_Physics2DMasks = null;
			}
		}

		private void Process2DLayerCollisionMatrix()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			if (m_Physics2DMasks == null)
			{
				return;
			}
			try
			{
				Object val = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Physics2DSettings.asset")[0];
				if (val == (Object)null)
				{
					m_Errors.Add("Failed to process physics 2D layer collision matrix. Asset not found: ProjectSettings/Physics2DSettings.asset");
					return;
				}
				SerializedObject val2 = new SerializedObject(val);
				SerializedProperty val3 = val2.FindProperty("m_LayerCollisionMatrix");
				if (val3 == null)
				{
					m_Errors.Add("Failed to process physics 2D layer collision matrix. Matrix property not found in dynamics manager asset.");
					return;
				}
				for (int i = 0; i < 32; i++)
				{
					uint old = m_Physics2DMasks[i];
					uint num = TransformMatrix(old);
					val3.GetArrayElementAtIndex(TransformLayer(i, redirected: false)).set_longValue((long)num);
				}
				val2.ApplyModifiedPropertiesWithoutUndo();
				m_Physics2DMatrixCompleted = true;
			}
			catch (Exception ex)
			{
				m_Errors.Add("Failed to process physics 2D layer collision matrix. Exception when updating settings: " + ex.Message);
			}
		}

		private void BuildReport()
		{
			m_CompletionReport = $"Layer Modification Completed\n\n- Modified tags and layers settings.\n- {GetCollisionMatrixReport()}\n- {Get2DCollisionMatrixReport()}\n- {GetObjectsReport()}\n- {GetMasksReport()}\n- Errors encountered: {m_Errors.Count}.";
		}

		private string GetCollisionMatrixReport()
		{
			if (m_PhysicsMatrixCompleted)
			{
				return "Physics layer collision matrix modifications succeeded.";
			}
			return "Physics layer collision matrix modifications failed with errors.";
		}

		private string Get2DCollisionMatrixReport()
		{
			if (m_Physics2DMatrixCompleted)
			{
				return "Physics 2D layer collision matrix modifications succeeded.";
			}
			return "Physics 2D layer collision matrix modifications failed with errors.";
		}

		private string GetObjectsReport()
		{
			if (m_SceneCount > 0 && m_PrefabCount > 0)
			{
				return $"Modified layer property for {m_ObjectCount} GameObjects across {m_SceneCount} scenes and {m_PrefabCount} prefabs.";
			}
			if (m_SceneCount > 0)
			{
				return $"Modified layer property for {m_ObjectCount} GameObjects across {m_SceneCount} scenes.";
			}
			if (m_PrefabCount > 0)
			{
				return $"Modified layer property for {m_ObjectCount} GameObjects across {m_PrefabCount} prefabs.";
			}
			if (m_ObjectCount > 0)
			{
				return $"Modified layer property for {m_ObjectCount} GameObjects.";
			}
			return "No GameObject layers affected by changes.";
		}

		private string GetMasksReport()
		{
			if (m_ComponentCount > 0 && m_AssetCount > 0)
			{
				return $"Modified {m_LayerMaskCount} LayerMask properties on {m_ComponentCount} components and {m_AssetCount} scriptable object assets.";
			}
			if (m_ComponentCount > 0)
			{
				return $"Modified {m_LayerMaskCount} LayerMask properties on {m_ComponentCount} components.";
			}
			if (m_AssetCount > 0)
			{
				return $"Modified {m_LayerMaskCount} LayerMask properties on {m_AssetCount} scriptable object assets.";
			}
			if (m_LayerMaskCount > 0)
			{
				return $"Modified {m_LayerMaskCount} LayerMask properties.";
			}
			return "No LayerMask properties found on components or scriptable object assets.";
		}

		public LayerManager()
			: this()
		{
		}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)

	}
}
