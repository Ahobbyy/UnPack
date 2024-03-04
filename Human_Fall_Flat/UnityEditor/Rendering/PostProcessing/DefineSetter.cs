using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Rendering.PostProcessing
{
	[InitializeOnLoad]
	internal sealed class DefineSetter
	{
		private const string k_Define = "UNITY_POST_PROCESSING_STACK_V2";

		static DefineSetter()
		{
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			foreach (BuildTargetGroup item in from BuildTargetGroup x in Enum.GetValues(typeof(BuildTargetGroup))
				where (int)x > 0
				where !IsObsolete(x)
				select x)
			{
				string text = PlayerSettings.GetScriptingDefineSymbolsForGroup(item).Trim();
				List<string> list = (from x in text.Split(';', ' ')
					where !string.IsNullOrEmpty(x)
					select x).ToList();
				if (!list.Contains("UNITY_POST_PROCESSING_STACK_V2"))
				{
					list.Add("UNITY_POST_PROCESSING_STACK_V2");
					text = list.Aggregate((string a, string b) => a + ";" + b);
					PlayerSettings.SetScriptingDefineSymbolsForGroup(item, text);
				}
			}
		}

		private static bool IsObsolete(BuildTargetGroup group)
		{
			object[] customAttributes = typeof(BuildTargetGroup).GetField(((object)(BuildTargetGroup)(ref group)).ToString()).GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false);
			if (customAttributes != null)
			{
				return customAttributes.Length != 0;
			}
			return false;
		}
	}
}
