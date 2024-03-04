using System;

namespace ProBuilder2.EditorCommon
{
	[Serializable]
	internal class pb_AboutEntry
	{
		public string name;

		public string identifier;

		public string version;

		public string date;

		public string changelogPath;

		public const string KEY_NAME = "name: ";

		public const string KEY_IDENTIFIER = "identifier: ";

		public const string KEY_VERSION = "version: ";

		public const string KEY_DATE = "date: ";

		public const string KEY_CHANGELOG = "changelog: ";
	}
}
