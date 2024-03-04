using System;
using UnityEditor;

namespace AssetBundleBrowser.AssetBundleDataSource
{
	public class ABBuildInfo
	{
		private string m_outputDirectory;

		private BuildAssetBundleOptions m_options;

		private BuildTarget m_buildTarget;

		private Action<string> m_onBuild;

		public string outputDirectory
		{
			get
			{
				return m_outputDirectory;
			}
			set
			{
				m_outputDirectory = value;
			}
		}

		public BuildAssetBundleOptions options
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_options;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_options = value;
			}
		}

		public BuildTarget buildTarget
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_buildTarget;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_buildTarget = value;
			}
		}

		public Action<string> onBuild
		{
			get
			{
				return m_onBuild;
			}
			set
			{
				m_onBuild = value;
			}
		}
	}
}
