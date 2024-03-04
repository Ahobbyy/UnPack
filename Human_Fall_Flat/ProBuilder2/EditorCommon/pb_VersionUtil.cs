using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ProBuilder2.EditorCommon
{
	internal static class pb_VersionUtil
	{
		public static bool GetAboutEntry(out pb_AboutEntry about)
		{
			about = null;
			string[] files = Directory.GetFiles("./Assets", "pc_AboutEntry_ProBuilder.txt", SearchOption.AllDirectories);
			if (files == null || files.Length < 1)
			{
				return false;
			}
			for (int i = 0; i < files.Length; i++)
			{
				if (about != null)
				{
					break;
				}
				about = ParseAboutEntry(files[i]);
			}
			return about != null;
		}

		public static bool GetCurrent(out pb_VersionInfo version)
		{
			if (!GetAboutEntry(out var about))
			{
				version = default(pb_VersionInfo);
				return false;
			}
			version = pb_VersionInfo.FromString(about.version);
			return true;
		}

		public static bool FormatChangelog(string raw, out pb_VersionInfo version, out string formatted_changes)
		{
			bool result = true;
			string[] array = Regex.Split(raw, "(?mi)^#\\s", RegexOptions.Multiline);
			try
			{
				Match match = Regex.Match(array[1], "(?<=^ProBuilder\\s).[0-9]*\\.[0-9]*\\.[0-9]*[a-z][0-9]*");
				version = pb_VersionInfo.FromString(match.Success ? match.Value : array[1].Split('\n')[0]);
			}
			catch
			{
				version = pb_VersionInfo.FromString("not found");
				result = false;
			}
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				string[] array2 = array[1].Trim().Split('\n');
				for (int i = 2; i < array2.Length; i++)
				{
					stringBuilder.AppendLine(array2[i]);
				}
				formatted_changes = stringBuilder.ToString();
				formatted_changes = Regex.Replace(formatted_changes, "^-", "•", RegexOptions.Multiline);
				formatted_changes = Regex.Replace(formatted_changes, "(?<=^##\\\\s).*", "<size=16><b>${0}</b></size>", RegexOptions.Multiline);
				formatted_changes = Regex.Replace(formatted_changes, "^##\\ ", "", RegexOptions.Multiline);
				return result;
			}
			catch
			{
				formatted_changes = "";
				return false;
			}
		}

		private static pb_AboutEntry ParseAboutEntry(string path)
		{
			if (!File.Exists(path))
			{
				return null;
			}
			pb_AboutEntry pb_AboutEntry2 = new pb_AboutEntry();
			string[] array = File.ReadAllLines(path);
			foreach (string text in array)
			{
				if (text.StartsWith("name: "))
				{
					pb_AboutEntry2.name = text.Replace("name: ", "").Trim();
				}
				else if (text.StartsWith("identifier: "))
				{
					pb_AboutEntry2.identifier = text.Replace("identifier: ", "").Trim();
				}
				else if (text.StartsWith("version: "))
				{
					pb_AboutEntry2.version = text.Replace("version: ", "").Trim();
				}
				else if (text.StartsWith("date: "))
				{
					pb_AboutEntry2.date = text.Replace("date: ", "").Trim();
				}
				else if (text.StartsWith("changelog: "))
				{
					pb_AboutEntry2.changelogPath = text.Replace("changelog: ", "").Trim();
				}
			}
			return pb_AboutEntry2;
		}
	}
}
