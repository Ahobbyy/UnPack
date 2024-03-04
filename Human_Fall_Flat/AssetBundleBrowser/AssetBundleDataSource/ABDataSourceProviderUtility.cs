using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AssetBundleBrowser.AssetBundleDataSource
{
	internal class ABDataSourceProviderUtility
	{
		private static List<Type> s_customNodes;

		internal static List<Type> CustomABDataSourceTypes
		{
			get
			{
				if (s_customNodes == null)
				{
					s_customNodes = BuildCustomABDataSourceList();
				}
				return s_customNodes;
			}
		}

		private static List<Type> BuildCustomABDataSourceList()
		{
			List<Type> list = new List<Type>();
			list.Add(null);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					List<Type> list2 = new List<Type>(from t in assembly.GetTypes()
						where t != typeof(ABDataSource)
						where typeof(ABDataSource).IsAssignableFrom(t)
						select t);
					for (int j = 0; j < list2.Count; j++)
					{
						if (list2[j].Name == "AssetDatabaseABDataSource")
						{
							list[0] = list2[j];
						}
						else if (list2[j] != null)
						{
							list.Add(list2[j]);
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return list;
		}
	}
}
