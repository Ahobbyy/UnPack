namespace UnityEngine.Rendering.PostProcessing
{
	public sealed class PropertySheet
	{
		public MaterialPropertyBlock properties { get; private set; }

		internal Material material { get; private set; }

		internal PropertySheet(Material material)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			this.material = material;
			properties = new MaterialPropertyBlock();
		}

		public void ClearKeywords()
		{
			material.set_shaderKeywords((string[])null);
		}

		public void EnableKeyword(string keyword)
		{
			material.EnableKeyword(keyword);
		}

		public void DisableKeyword(string keyword)
		{
			material.DisableKeyword(keyword);
		}

		internal void Release()
		{
			RuntimeUtilities.Destroy((Object)(object)material);
			material = null;
		}
	}
}
