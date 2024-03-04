using System;
using System.Collections.Generic;
using AssetBundleBrowser.AssetBundleDataSource;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	public class AssetBundleBrowserMain : EditorWindow, IHasCustomMenu, ISerializationCallbackReceiver
	{
		private enum Mode
		{
			Browser,
			Builder,
			Inspect
		}

		private static AssetBundleBrowserMain s_instance;

		internal const float kButtonWidth = 150f;

		[SerializeField]
		private Mode m_Mode;

		[SerializeField]
		private int m_DataSourceIndex;

		[SerializeField]
		internal AssetBundleManageTab m_ManageTab;

		[SerializeField]
		internal AssetBundleBuildTab m_BuildTab;

		[SerializeField]
		internal AssetBundleInspectTab m_InspectTab;

		private Texture2D m_RefreshTexture;

		private const float k_ToolbarPadding = 15f;

		private const float k_MenubarPadding = 32f;

		[SerializeField]
		internal bool multiDataSource;

		private List<ABDataSource> m_DataSourceList;

		internal static AssetBundleBrowserMain instance
		{
			get
			{
				if ((Object)(object)s_instance == (Object)null)
				{
					s_instance = EditorWindow.GetWindow<AssetBundleBrowserMain>();
				}
				return s_instance;
			}
		}

		[MenuItem("Window/AssetBundle Browser", priority = 2050)]
		private static void ShowWindow()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			s_instance = null;
			((EditorWindow)instance).set_titleContent(new GUIContent("AssetBundles"));
			((EditorWindow)instance).Show();
		}

		public virtual void AddItemsToMenu(GenericMenu menu)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0025: Expected O, but got Unknown
			if (menu != null)
			{
				menu.AddItem(new GUIContent("Custom Sources"), multiDataSource, new MenuFunction(FlipDataSource));
			}
		}

		internal void FlipDataSource()
		{
			multiDataSource = !multiDataSource;
		}

		private void OnEnable()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			Rect subWindowArea = GetSubWindowArea();
			if (m_ManageTab == null)
			{
				m_ManageTab = new AssetBundleManageTab();
			}
			m_ManageTab.OnEnable(subWindowArea, (EditorWindow)(object)this);
			if (m_BuildTab == null)
			{
				m_BuildTab = new AssetBundleBuildTab();
			}
			m_BuildTab.OnEnable((EditorWindow)(object)this);
			if (m_InspectTab == null)
			{
				m_InspectTab = new AssetBundleInspectTab();
			}
			m_InspectTab.OnEnable(subWindowArea);
			m_RefreshTexture = EditorGUIUtility.FindTexture("Refresh");
			InitDataSources();
		}

		private void InitDataSources()
		{
			multiDataSource = false;
			m_DataSourceList = new List<ABDataSource>();
			foreach (Type customABDataSourceType in ABDataSourceProviderUtility.CustomABDataSourceTypes)
			{
				m_DataSourceList.AddRange(customABDataSourceType.GetMethod("CreateDataSources").Invoke(null, null) as List<ABDataSource>);
			}
			if (m_DataSourceList.Count > 1)
			{
				multiDataSource = true;
				if (m_DataSourceIndex >= m_DataSourceList.Count)
				{
					m_DataSourceIndex = 0;
				}
				Model.DataSource = m_DataSourceList[m_DataSourceIndex];
			}
		}

		private void OnDisable()
		{
			if (m_BuildTab != null)
			{
				m_BuildTab.OnDisable();
			}
			if (m_InspectTab != null)
			{
				m_InspectTab.OnDisable();
			}
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
		}

		private Rect GetSubWindowArea()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			float num = 32f;
			if (multiDataSource)
			{
				num += 16f;
			}
			float num2 = num;
			Rect position = ((EditorWindow)this).get_position();
			float width = ((Rect)(ref position)).get_width();
			position = ((EditorWindow)this).get_position();
			return new Rect(0f, num2, width, ((Rect)(ref position)).get_height() - num);
		}

		private void Update()
		{
			switch (m_Mode)
			{
			case Mode.Builder:
			case Mode.Inspect:
				return;
			}
			m_ManageTab.Update();
		}

		private void OnGUI()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			ModeToggle();
			switch (m_Mode)
			{
			case Mode.Builder:
				m_BuildTab.OnGUI();
				break;
			case Mode.Inspect:
				m_InspectTab.OnGUI(GetSubWindowArea());
				break;
			default:
				m_ManageTab.OnGUI(GetSubWindowArea());
				break;
			}
		}

		private void ModeToggle()
		{
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Expected O, but got Unknown
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Expected O, but got Unknown
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Expected O, but got Unknown
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Expected O, but got Unknown
			//IL_0208: Expected O, but got Unknown
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Expected O, but got Unknown
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(15f);
			switch (m_Mode)
			{
			case Mode.Browser:
				if (GUILayout.Button((Texture)(object)m_RefreshTexture, (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					m_ManageTab.ForceReloadData();
				}
				break;
			case Mode.Builder:
				GUILayout.Space((float)((Texture)m_RefreshTexture).get_width() + 15f);
				break;
			case Mode.Inspect:
				if (GUILayout.Button((Texture)(object)m_RefreshTexture, (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					m_InspectTab.RefreshBundles();
				}
				break;
			}
			Rect position = ((EditorWindow)this).get_position();
			float num = ((Rect)(ref position)).get_width() - 60f - (float)((Texture)m_RefreshTexture).get_width();
			string[] array = new string[3] { "Configure", "Build", "Inspect" };
			m_Mode = (Mode)GUILayout.Toolbar((int)m_Mode, array, GUIStyle.op_Implicit("LargeButton"), (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(num) });
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			if (!multiDataSource)
			{
				return;
			}
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			HorizontalScope val = new HorizontalScope(EditorStyles.get_toolbar(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			try
			{
				GUILayout.Label("Bundle Data Source:", (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button(new GUIContent($"{Model.DataSource.Name} ({Model.DataSource.ProviderName})", "Select Asset Bundle Set"), EditorStyles.get_toolbarPopup(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
				{
					GenericMenu val2 = new GenericMenu();
					for (int i = 0; i < m_DataSourceList.Count; i++)
					{
						ABDataSource ds = m_DataSourceList[i];
						if (ds != null)
						{
							if (i > 0)
							{
								val2.AddSeparator("");
							}
							int counter = i;
							val2.AddItem(new GUIContent($"{ds.Name} ({ds.ProviderName})"), false, (MenuFunction)delegate
							{
								m_DataSourceIndex = counter;
								Model.DataSource = ds;
								m_ManageTab.ForceReloadData();
							});
						}
					}
					val2.ShowAsContext();
				}
				GUILayout.FlexibleSpace();
				if (Model.DataSource.IsReadOnly())
				{
					GUIStyle val3 = new GUIStyle(EditorStyles.get_toolbar());
					val3.set_alignment((TextAnchor)5);
					GUILayout.Label("Read Only", val3, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			GUILayout.EndHorizontal();
		}

		public AssetBundleBrowserMain()
			: this()
		{
		}
	}
}
