using System;
using System.Reflection;

namespace TMPro
{
	public static class SortingLayerHelper
	{
		private static Type _utilityType;

		private static PropertyInfo _sortingLayerNamesProperty;

		private static MethodInfo _getSortingLayerUserIdMethod;

		public static string[] sortingLayerNames
		{
			get
			{
				if (_sortingLayerNamesProperty == null)
				{
					return null;
				}
				return _sortingLayerNamesProperty.GetValue(null, null) as string[];
			}
		}

		static SortingLayerHelper()
		{
			_utilityType = Type.GetType("UnityEditorInternal.InternalEditorUtility, UnityEditor");
			_sortingLayerNamesProperty = _utilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			_getSortingLayerUserIdMethod = _utilityType.GetMethod("GetSortingLayerUniqueID", BindingFlags.Static | BindingFlags.NonPublic);
		}

		public static string GetSortingLayerNameFromID(int id)
		{
			string[] array = sortingLayerNames;
			if (array == null)
			{
				return null;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (GetSortingLayerIDForIndex(i) == id)
				{
					return array[i];
				}
			}
			return null;
		}

		public static int GetSortingLayerIDForName(string name)
		{
			string[] array = sortingLayerNames;
			if (array == null)
			{
				return 0;
			}
			return GetSortingLayerIDForIndex(Array.IndexOf(array, name));
		}

		public static int GetSortingLayerIDForIndex(int index)
		{
			if (_getSortingLayerUserIdMethod == null)
			{
				return 0;
			}
			return (int)_getSortingLayerUserIdMethod.Invoke(null, new object[1] { index });
		}
	}
}
