using System;
using UnityEditor;
using UnityEngine;

namespace Yondernauts.LayerManager
{
	public class LayerManagerData : ScriptableObject
	{
		[Serializable]
		public class LayerMapEntry
		{
			public string name;

			public string oldName;

			public int oldIndex;

			public int redirect = -1;

			public bool valid
			{
				get
				{
					if (redirect == -1 && string.IsNullOrEmpty(name))
					{
						return string.IsNullOrEmpty(oldName);
					}
					return true;
				}
			}

			public LayerMapEntry(string n, int i)
			{
				name = n;
				oldName = n;
				oldIndex = i;
			}
		}

		public class SerializedLayerMapEntry
		{
			private LayerManagerData m_Data;

			public SerializedProperty serializedProperty { get; private set; }

			public string name
			{
				get
				{
					return serializedProperty.FindPropertyRelative("name").get_stringValue();
				}
				set
				{
					Undo.RecordObject((Object)(object)m_Data, "Change Layer Name");
					serializedProperty.FindPropertyRelative("name").set_stringValue(value);
					if (string.IsNullOrEmpty(value))
					{
						SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
						foreach (SerializedLayerMapEntry serializedLayerMapEntry in allEntries)
						{
							if (serializedLayerMapEntry != this && serializedLayerMapEntry.redirect == oldIndex)
							{
								serializedLayerMapEntry.redirect = -1;
							}
						}
					}
					EditorUtility.SetDirty((Object)(object)m_Data);
				}
			}

			public int redirect
			{
				get
				{
					return serializedProperty.FindPropertyRelative("redirect").get_intValue();
				}
				set
				{
					Undo.RecordObject((Object)(object)m_Data, "Redirect Layer");
					int currentGroup = Undo.GetCurrentGroup();
					serializedProperty.FindPropertyRelative("redirect").set_intValue(value);
					if (value == -1)
					{
						if (string.IsNullOrEmpty(oldName))
						{
							SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
							foreach (SerializedLayerMapEntry serializedLayerMapEntry in allEntries)
							{
								if (serializedLayerMapEntry != this && serializedLayerMapEntry.redirect == oldIndex)
								{
									Undo.RecordObject((Object)(object)m_Data, "Redirect Layer");
									serializedLayerMapEntry.serializedProperty.FindPropertyRelative("redirect").set_intValue(-1);
								}
							}
						}
					}
					else
					{
						SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
						foreach (SerializedLayerMapEntry serializedLayerMapEntry2 in allEntries)
						{
							if (serializedLayerMapEntry2 != this && serializedLayerMapEntry2.redirect == oldIndex)
							{
								Undo.RecordObject((Object)(object)m_Data, "Redirect Layer");
								serializedLayerMapEntry2.serializedProperty.FindPropertyRelative("redirect").set_intValue(value);
							}
						}
					}
					Undo.CollapseUndoOperations(currentGroup);
					EditorUtility.SetDirty((Object)(object)m_Data);
				}
			}

			public string oldName => serializedProperty.FindPropertyRelative("oldName").get_stringValue();

			public int oldIndex => serializedProperty.FindPropertyRelative("oldIndex").get_intValue();

			public bool valid
			{
				get
				{
					if (!string.IsNullOrEmpty(name))
					{
						SerializedLayerMapEntry[] allEntries = m_Data.GetAllEntries();
						foreach (SerializedLayerMapEntry serializedLayerMapEntry in allEntries)
						{
							if (serializedLayerMapEntry != this && serializedLayerMapEntry.name == name)
							{
								return false;
							}
						}
					}
					if (redirect == -1 && string.IsNullOrEmpty(name))
					{
						return string.IsNullOrEmpty(oldName);
					}
					return true;
				}
			}

			public SerializedLayerMapEntry(LayerManagerData data, int index)
			{
				m_Data = data;
				serializedProperty = data.layerMapProperty.GetArrayElementAtIndex(index);
			}

			public string GetRedirectName()
			{
				LayerMapEntry[] layerMap = m_Data.m_LayerMap;
				foreach (LayerMapEntry layerMapEntry in layerMap)
				{
					if (layerMapEntry.oldIndex == oldIndex)
					{
						return layerMapEntry.name;
					}
				}
				return string.Empty;
			}
		}

		[SerializeField]
		private LayerMapEntry[] m_LayerMap;

		[SerializeField]
		private bool m_Dirty;

		private SerializedLayerMapEntry[] m_SerializedEntries;

		public bool dirty
		{
			get
			{
				return m_Dirty;
			}
			set
			{
				serializedObject.FindProperty("m_Dirty").set_boolValue(value);
			}
		}

		public bool valid
		{
			get
			{
				LayerMapEntry[] layerMap = m_LayerMap;
				for (int i = 0; i < layerMap.Length; i++)
				{
					if (!layerMap[i].valid)
					{
						return false;
					}
				}
				return true;
			}
		}

		public SerializedObject serializedObject { get; private set; }

		public SerializedProperty layerMapProperty { get; private set; }

		public void Initialise()
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			m_LayerMap = new LayerMapEntry[24];
			for (int i = 0; i < 24; i++)
			{
				int num = i + 8;
				m_LayerMap[i] = new LayerMapEntry(LayerMask.LayerToName(num), num);
			}
			m_Dirty = false;
			serializedObject = new SerializedObject((Object)(object)this);
			layerMapProperty = serializedObject.FindProperty("m_LayerMap");
			m_SerializedEntries = new SerializedLayerMapEntry[24];
			RebuildSerializedEntries();
			((Object)this).set_hideFlags((HideFlags)52);
		}

		public void RebuildSerializedEntries()
		{
			for (int i = 0; i < 24; i++)
			{
				m_SerializedEntries[i] = new SerializedLayerMapEntry(this, i);
			}
		}

		public SerializedLayerMapEntry GetEntryFromIndex(int index)
		{
			return m_SerializedEntries[index];
		}

		public SerializedLayerMapEntry GetEntryFromOldIndex(int oldIndex)
		{
			for (int i = 0; i < m_LayerMap.Length; i++)
			{
				if (m_LayerMap[i].oldIndex == oldIndex)
				{
					return m_SerializedEntries[i];
				}
			}
			return null;
		}

		public SerializedLayerMapEntry[] GetAllEntries()
		{
			return m_SerializedEntries;
		}

		public void ApplyModifiedProperties()
		{
			serializedObject.ApplyModifiedProperties();
		}

		public LayerManagerData()
			: this()
		{
		}
	}
}
