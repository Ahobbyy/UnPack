using System;

namespace HumanAPI
{
	[Serializable]
	public class RagdollPresetPartMetadata
	{
		public string modelPath;

		public string color1;

		public string color2;

		public string color3;

		[NonSerialized]
		public bool suppressCustomTexture;

		[NonSerialized]
		public byte[] bytes;

		public static RagdollPresetPartMetadata Clone(RagdollPresetPartMetadata source)
		{
			if (source == null)
			{
				return null;
			}
			return new RagdollPresetPartMetadata
			{
				modelPath = source.modelPath,
				color1 = source.color1,
				color2 = source.color2,
				color3 = source.color3
			};
		}

		public static bool IsEmpty(string modelPath)
		{
			if (!string.IsNullOrEmpty(modelPath) && !"builtin:HumanNoHat".Equals(modelPath) && !"builtin:HumanNoLower".Equals(modelPath))
			{
				return "builtin:HumanNoUpper".Equals(modelPath);
			}
			return true;
		}

		public static bool IsEmpty(RagdollPresetPartMetadata part)
		{
			if (part != null)
			{
				return IsEmpty(part.modelPath);
			}
			return true;
		}
	}
}
