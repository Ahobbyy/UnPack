using System;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.EditorCommon
{
	[InitializeOnLoad]
	internal static class pb_UpdateCheck
	{
		private const string PROBUILDER_VERSION_URL = "http://procore3d.github.io/probuilder2/current.txt";

		private const string pbLastWebVersionChecked = "pbLastWebVersionChecked";

		private static WWW updateQuery;

		private static bool calledFromMenu;

		static pb_UpdateCheck()
		{
			if (pb_PreferencesInternal.GetBool("pbCheckForProBuilderUpdates"))
			{
				calledFromMenu = false;
				CheckForUpdate();
			}
		}

		[MenuItem("Tools/ProBuilder/Check for Updates", false, 1)]
		private static void MenuCheckForUpdate()
		{
			calledFromMenu = true;
			CheckForUpdate();
		}

		public static void CheckForUpdate()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			if (updateQuery == null)
			{
				updateQuery = new WWW("http://procore3d.github.io/probuilder2/current.txt");
				EditorApplication.update = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
			}
		}

		private static void Update()
		{
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Expected O, but got Unknown
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			if (updateQuery != null)
			{
				if (!updateQuery.get_isDone())
				{
					return;
				}
				try
				{
					if (string.IsNullOrEmpty(updateQuery.get_error()) || !Regex.IsMatch(updateQuery.get_text(), "404 not found", RegexOptions.IgnoreCase))
					{
						pb_VersionInfo version2;
						if (!pb_VersionUtil.FormatChangelog(updateQuery.get_text(), out var version, out var formatted_changes))
						{
							FailedConnection();
						}
						else if (!pb_VersionUtil.GetCurrent(out version2) || version.CompareTo(version2) > 0)
						{
							string @string = pb_PreferencesInternal.GetString("pbLastWebVersionChecked", "");
							if (calledFromMenu || !@string.Equals(version.text))
							{
								pb_UpdateAvailable.Init(version, formatted_changes);
								pb_PreferencesInternal.SetString("pbLastWebVersionChecked", version.text, (pb_PreferenceLocation)0);
							}
						}
						else
						{
							UpToDate(version2.ToString());
						}
					}
					else
					{
						FailedConnection();
					}
				}
				catch (Exception ex)
				{
					FailedConnection($"Error: Is build target is Webplayer?\n\n{ex.ToString()}");
				}
				updateQuery = null;
			}
			calledFromMenu = false;
			EditorApplication.update = (CallbackFunction)Delegate.Remove((Delegate)(object)EditorApplication.update, (Delegate)new CallbackFunction(Update));
		}

		private static void UpToDate(string version)
		{
			if (calledFromMenu)
			{
				EditorUtility.DisplayDialog("ProBuilder Update Check", string.Format("You're up to date!\n\nInstalled Version: {0}\nLatest Version: {0}", version), "Okay");
			}
		}

		private static void FailedConnection(string error = null)
		{
			if (calledFromMenu)
			{
				EditorUtility.DisplayDialog("ProBuilder Update Check", (error == null) ? "Failed to connect to server!" : $"Failed to connect to server!\n\n{error.ToString()}", "Okay");
			}
		}
	}
}
