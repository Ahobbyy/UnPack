using System;
using System.Text.RegularExpressions;

namespace ProBuilder2.EditorCommon
{
	[Serializable]
	public struct pb_VersionInfo : IEquatable<pb_VersionInfo>, IComparable<pb_VersionInfo>
	{
		public int major;

		public int minor;

		public int patch;

		public int build;

		public VersionType type;

		public string text;

		public bool valid;

		public override bool Equals(object o)
		{
			if (o is pb_VersionInfo)
			{
				return Equals((pb_VersionInfo)o);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = 13;
			if (valid)
			{
				num = num * 7 + major.GetHashCode();
				num = num * 7 + minor.GetHashCode();
				num = num * 7 + patch.GetHashCode();
				num = num * 7 + build.GetHashCode();
				return num * 7 + type.GetHashCode();
			}
			return text.GetHashCode();
		}

		public bool Equals(pb_VersionInfo version)
		{
			if (valid != version.valid)
			{
				return false;
			}
			if (valid)
			{
				if (major == version.major && minor == version.minor && patch == version.patch && type == version.type)
				{
					return build == version.build;
				}
				return false;
			}
			if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(version.text))
			{
				return false;
			}
			return text.Equals(version.text);
		}

		public int CompareTo(pb_VersionInfo version)
		{
			if (Equals(version))
			{
				return 0;
			}
			if (major > version.major)
			{
				return 1;
			}
			if (major < version.major)
			{
				return -1;
			}
			if (minor > version.minor)
			{
				return 1;
			}
			if (minor < version.minor)
			{
				return -1;
			}
			if (patch > version.patch)
			{
				return 1;
			}
			if (patch < version.patch)
			{
				return -1;
			}
			if (type > version.type)
			{
				return 1;
			}
			if (type < version.type)
			{
				return -1;
			}
			if (build > version.build)
			{
				return 1;
			}
			return -1;
		}

		public override string ToString()
		{
			return $"{major}.{minor}.{patch}{type.ToString().ToLower()[0]}{build}";
		}

		public static pb_VersionInfo FromString(string str)
		{
			pb_VersionInfo result = default(pb_VersionInfo);
			result.text = str;
			try
			{
				string[] array = Regex.Split(str, "[\\.A-Za-z]");
				Match match = Regex.Match(str, "A-Za-z");
				int.TryParse(array[0], out result.major);
				int.TryParse(array[1], out result.minor);
				int.TryParse(array[2], out result.patch);
				int.TryParse(array[3], out result.build);
				result.type = GetVersionType((match != null && match.Success) ? match.Value : "");
				result.valid = true;
				return result;
			}
			catch
			{
				result.valid = false;
				return result;
			}
		}

		private static VersionType GetVersionType(string type)
		{
			if (type.Equals("b") || type.Equals("B"))
			{
				return VersionType.Beta;
			}
			if (type.Equals("p") || type.Equals("P"))
			{
				return VersionType.Patch;
			}
			return VersionType.Final;
		}
	}
}
