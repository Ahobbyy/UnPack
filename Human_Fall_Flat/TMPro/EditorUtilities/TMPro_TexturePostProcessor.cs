using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public class TMPro_TexturePostProcessor : AssetPostprocessor
	{
		private void OnPostprocessTexture(Texture2D texture)
		{
			Object obj = AssetDatabase.LoadAssetAtPath(((AssetPostprocessor)this).get_assetPath(), typeof(Texture2D));
			Texture2D val = (Texture2D)(object)((obj is Texture2D) ? obj : null);
			if ((Object)(object)val != (Object)null)
			{
				TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(isChanged: true, (Object)(object)val);
			}
		}

		public TMPro_TexturePostProcessor()
			: this()
		{
		}
	}
}
