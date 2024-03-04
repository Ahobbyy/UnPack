using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	public sealed class EffectListEditor
	{
		private Editor m_BaseEditor;

		private SerializedObject m_SerializedObject;

		private SerializedProperty m_SettingsProperty;

		private Dictionary<Type, Type> m_EditorTypes;

		private List<PostProcessEffectBaseEditor> m_Editors;

		public PostProcessProfile asset { get; private set; }

		public EffectListEditor(Editor editor)
		{
			Assert.IsNotNull<Editor>(editor);
			m_BaseEditor = editor;
		}

		public void Init(PostProcessProfile asset, SerializedObject serializedObject)
		{
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Expected O, but got Unknown
			Assert.IsNotNull<PostProcessProfile>(asset);
			Assert.IsNotNull<SerializedObject>(serializedObject);
			this.asset = asset;
			m_SerializedObject = serializedObject;
			m_SettingsProperty = serializedObject.FindProperty("settings");
			Assert.IsNotNull<SerializedProperty>(m_SettingsProperty);
			m_EditorTypes = new Dictionary<Type, Type>();
			m_Editors = new List<PostProcessEffectBaseEditor>();
			foreach (Type item in from t in RuntimeUtilities.GetAllAssemblyTypes()
				where t.IsSubclassOf(typeof(PostProcessEffectBaseEditor)) && t.IsDefined(typeof(PostProcessEditorAttribute), inherit: false) && !t.IsAbstract
				select t)
			{
				PostProcessEditorAttribute attribute = item.GetAttribute<PostProcessEditorAttribute>();
				m_EditorTypes.Add(attribute.settingsType, item);
			}
			for (int i = 0; i < this.asset.settings.Count; i++)
			{
				CreateEditor(this.asset.settings[i], m_SettingsProperty.GetArrayElementAtIndex(i));
			}
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Combine((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedoPerformed));
		}

		private void OnUndoRedoPerformed()
		{
			asset.isDirty = true;
			m_SerializedObject.Update();
			m_SerializedObject.ApplyModifiedProperties();
			m_BaseEditor.Repaint();
		}

		private void CreateEditor(PostProcessEffectSettings settings, SerializedProperty property, int index = -1)
		{
			Type type = ((object)settings).GetType();
			if (!m_EditorTypes.TryGetValue(type, out var value))
			{
				value = typeof(DefaultPostProcessEffectEditor);
			}
			PostProcessEffectBaseEditor postProcessEffectBaseEditor = (PostProcessEffectBaseEditor)Activator.CreateInstance(value);
			postProcessEffectBaseEditor.Init(settings, m_BaseEditor);
			postProcessEffectBaseEditor.baseProperty = property.Copy();
			if (index < 0)
			{
				m_Editors.Add(postProcessEffectBaseEditor);
			}
			else
			{
				m_Editors[index] = postProcessEffectBaseEditor;
			}
		}

		private void RefreshEditors()
		{
			foreach (PostProcessEffectBaseEditor editor in m_Editors)
			{
				editor.OnDisable();
			}
			m_Editors.Clear();
			for (int i = 0; i < asset.settings.Count; i++)
			{
				CreateEditor(asset.settings[i], m_SettingsProperty.GetArrayElementAtIndex(i));
			}
		}

		public void Clear()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Expected O, but got Unknown
			if (m_Editors == null)
			{
				return;
			}
			foreach (PostProcessEffectBaseEditor editor in m_Editors)
			{
				editor.OnDisable();
			}
			m_Editors.Clear();
			m_EditorTypes.Clear();
			Undo.undoRedoPerformed = (UndoRedoCallback)Delegate.Remove((Delegate)(object)Undo.undoRedoPerformed, (Delegate)new UndoRedoCallback(OnUndoRedoPerformed));
		}

		public void OnGUI()
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Expected O, but got Unknown
			if ((Object)(object)asset == (Object)null)
			{
				return;
			}
			if (asset.isDirty)
			{
				RefreshEditors();
				asset.isDirty = false;
			}
			bool flag = !Provider.get_isActive() || AssetDatabase.IsOpenForEdit((Object)(object)asset, (StatusQueryOptions)1);
			DisabledScope val = default(DisabledScope);
			((DisabledScope)(ref val))._002Ector(!flag);
			try
			{
				EditorGUILayout.LabelField(EditorUtilities.GetContent("Overrides"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				for (int i = 0; i < m_Editors.Count; i++)
				{
					PostProcessEffectBaseEditor editor = m_Editors[i];
					string displayTitle = editor.GetDisplayTitle();
					int id = i;
					EditorUtilities.DrawSplitter();
					if (EditorUtilities.DrawHeader(displayTitle, editor.baseProperty, editor.activeProperty, editor.target, delegate
					{
						ResetEffectOverride(((object)editor.target).GetType(), id);
					}, delegate
					{
						RemoveEffectOverride(id);
					}))
					{
						DisabledScope val2 = new DisabledScope(!editor.activeProperty.get_boolValue());
						try
						{
							editor.OnInternalInspectorGUI();
						}
						finally
						{
							((IDisposable)(DisabledScope)(ref val2)).Dispose();
						}
					}
				}
				if (m_Editors.Count > 0)
				{
					EditorUtilities.DrawSplitter();
					EditorGUILayout.Space();
				}
				else
				{
					EditorGUILayout.HelpBox("No override set on this volume.", (MessageType)1);
				}
				if (GUILayout.Button("Add effect...", EditorStyles.get_miniButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					GenericMenu val3 = new GenericMenu();
					foreach (KeyValuePair<Type, PostProcessAttribute> settingsType in PostProcessManager.instance.settingsTypes)
					{
						Type type = settingsType.Key;
						GUIContent content = EditorUtilities.GetContent(settingsType.Value.menuItem);
						if (!asset.HasSettings(type))
						{
							val3.AddItem(content, false, (MenuFunction)delegate
							{
								AddEffectOverride(type);
							});
						}
						else
						{
							val3.AddDisabledItem(content);
						}
					}
					val3.ShowAsContext();
				}
				EditorGUILayout.Space();
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val)).Dispose();
			}
		}

		private void AddEffectOverride(Type type)
		{
			m_SerializedObject.Update();
			PostProcessEffectSettings postProcessEffectSettings = CreateNewEffect(type);
			Undo.RegisterCreatedObjectUndo((Object)(object)postProcessEffectSettings, "Add Effect Override");
			if (EditorUtility.IsPersistent((Object)(object)asset))
			{
				AssetDatabase.AddObjectToAsset((Object)(object)postProcessEffectSettings, (Object)(object)asset);
			}
			SerializedProperty settingsProperty = m_SettingsProperty;
			int arraySize = settingsProperty.get_arraySize();
			settingsProperty.set_arraySize(arraySize + 1);
			SerializedProperty arrayElementAtIndex = m_SettingsProperty.GetArrayElementAtIndex(m_SettingsProperty.get_arraySize() - 1);
			arrayElementAtIndex.set_objectReferenceValue((Object)(object)postProcessEffectSettings);
			CreateEditor(postProcessEffectSettings, arrayElementAtIndex);
			m_SerializedObject.ApplyModifiedProperties();
			if (EditorUtility.IsPersistent((Object)(object)asset))
			{
				EditorUtility.SetDirty((Object)(object)asset);
				AssetDatabase.SaveAssets();
			}
		}

		private void RemoveEffectOverride(int id)
		{
			bool isExpanded = false;
			if (id < m_Editors.Count - 1)
			{
				isExpanded = m_Editors[id + 1].baseProperty.get_isExpanded();
			}
			m_Editors[id].OnDisable();
			m_Editors.RemoveAt(id);
			m_SerializedObject.Update();
			SerializedProperty arrayElementAtIndex = m_SettingsProperty.GetArrayElementAtIndex(id);
			Object objectReferenceValue = arrayElementAtIndex.get_objectReferenceValue();
			arrayElementAtIndex.set_objectReferenceValue((Object)null);
			m_SettingsProperty.DeleteArrayElementAtIndex(id);
			for (int i = 0; i < m_Editors.Count; i++)
			{
				m_Editors[i].baseProperty = m_SettingsProperty.GetArrayElementAtIndex(i).Copy();
			}
			if (id < m_Editors.Count)
			{
				m_Editors[id].baseProperty.set_isExpanded(isExpanded);
			}
			m_SerializedObject.ApplyModifiedProperties();
			Undo.DestroyObjectImmediate(objectReferenceValue);
			EditorUtility.SetDirty((Object)(object)asset);
			AssetDatabase.SaveAssets();
		}

		private void ResetEffectOverride(Type type, int id)
		{
			m_Editors[id].OnDisable();
			m_Editors[id] = null;
			m_SerializedObject.Update();
			SerializedProperty arrayElementAtIndex = m_SettingsProperty.GetArrayElementAtIndex(id);
			Object objectReferenceValue = arrayElementAtIndex.get_objectReferenceValue();
			arrayElementAtIndex.set_objectReferenceValue((Object)null);
			PostProcessEffectSettings postProcessEffectSettings = CreateNewEffect(type);
			Undo.RegisterCreatedObjectUndo((Object)(object)postProcessEffectSettings, "Reset Effect Override");
			AssetDatabase.AddObjectToAsset((Object)(object)postProcessEffectSettings, (Object)(object)asset);
			arrayElementAtIndex.set_objectReferenceValue((Object)(object)postProcessEffectSettings);
			CreateEditor(postProcessEffectSettings, arrayElementAtIndex, id);
			m_SerializedObject.ApplyModifiedProperties();
			Undo.DestroyObjectImmediate(objectReferenceValue);
			EditorUtility.SetDirty((Object)(object)asset);
			AssetDatabase.SaveAssets();
		}

		private PostProcessEffectSettings CreateNewEffect(Type type)
		{
			PostProcessEffectSettings obj = (PostProcessEffectSettings)(object)ScriptableObject.CreateInstance(type);
			((Object)obj).set_hideFlags((HideFlags)3);
			((Object)obj).set_name(type.Name);
			obj.enabled.value = true;
			return obj;
		}
	}
}
