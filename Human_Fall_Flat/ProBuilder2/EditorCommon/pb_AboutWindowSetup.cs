using System;
using UnityEditor;

namespace ProBuilder2.EditorCommon
{
	[InitializeOnLoad]
	internal static class pb_AboutWindowSetup
	{
		static pb_AboutWindowSetup()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			EditorApplication.delayCall = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.delayCall, (Delegate)(CallbackFunction)delegate
			{
				pb_AboutWindow.Init(fromMenu: false);
			});
		}
	}
}
