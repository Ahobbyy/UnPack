using System.Collections.Generic;
using System.Linq;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class BundleNameData
	{
		private List<string> m_PathTokens;

		private string m_FullBundleName;

		private string m_ShortName;

		private string m_VariantName;

		private string m_FullNativeName;

		internal string fullNativeName => m_FullNativeName;

		internal string bundleName => m_FullBundleName;

		internal string shortName => m_ShortName;

		internal string variant
		{
			get
			{
				return m_VariantName;
			}
			set
			{
				m_VariantName = value;
				m_FullNativeName = m_FullBundleName;
				m_FullNativeName += (string.IsNullOrEmpty(m_VariantName) ? "" : ("." + m_VariantName));
			}
		}

		internal List<string> pathTokens
		{
			get
			{
				return m_PathTokens;
			}
			set
			{
				m_PathTokens = value.GetRange(0, value.Count - 1);
				SetShortName(value.Last());
				GenerateFullName();
			}
		}

		internal BundleNameData(string name)
		{
			SetName(name);
		}

		internal BundleNameData(string path, string name)
		{
			string text = (string.IsNullOrEmpty(path) ? "" : (path + "/"));
			text += name;
			SetName(text);
		}

		public override int GetHashCode()
		{
			return fullNativeName.GetHashCode();
		}

		internal void SetBundleName(string bundleName, string variantName)
		{
			string text = bundleName;
			text += (string.IsNullOrEmpty(variantName) ? "" : ("." + variantName));
			SetName(text);
		}

		private void SetName(string name)
		{
			if (m_PathTokens == null)
			{
				m_PathTokens = new List<string>();
			}
			else
			{
				m_PathTokens.Clear();
			}
			int num = name.IndexOf('/');
			int num2 = 0;
			while (num != -1)
			{
				m_PathTokens.Add(name.Substring(num2, num - num2));
				num2 = num + 1;
				num = name.IndexOf('/', num2);
			}
			SetShortName(name.Substring(num2));
			GenerateFullName();
		}

		private void SetShortName(string inputName)
		{
			m_ShortName = inputName;
			int num = m_ShortName.LastIndexOf('.');
			if (num > -1)
			{
				m_VariantName = m_ShortName.Substring(num + 1);
				m_ShortName = m_ShortName.Substring(0, num);
			}
			else
			{
				m_VariantName = string.Empty;
			}
		}

		internal void PartialNameChange(string newToken, int indexFromBack)
		{
			if (indexFromBack == 0)
			{
				SetShortName(newToken);
			}
			else if (indexFromBack - 1 < m_PathTokens.Count)
			{
				m_PathTokens[m_PathTokens.Count - indexFromBack] = newToken;
			}
			GenerateFullName();
		}

		private void GenerateFullName()
		{
			m_FullBundleName = string.Empty;
			for (int i = 0; i < m_PathTokens.Count; i++)
			{
				m_FullBundleName += m_PathTokens[i];
				m_FullBundleName += "/";
			}
			m_FullBundleName += m_ShortName;
			m_FullNativeName = m_FullBundleName;
			m_FullNativeName += (string.IsNullOrEmpty(m_VariantName) ? "" : ("." + m_VariantName));
		}
	}
}
