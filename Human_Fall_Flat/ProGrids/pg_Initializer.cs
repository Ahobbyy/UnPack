using System;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	[InitializeOnLoad]
	public static class pg_Initializer
	{
		static pg_Initializer()
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			if (EditorPrefs.GetBool("pgProGridsIsEnabled"))
			{
				if ((Object)(object)pg_Editor.instance == (Object)null)
				{
					pg_Editor.InitProGrids();
				}
				else
				{
					EditorApplication.delayCall = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.delayCall, (Delegate)new CallbackFunction(pg_Editor.instance.Initialize));
				}
			}
		}
	}
}
