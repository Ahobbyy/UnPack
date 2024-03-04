using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TMPro
{
	public class TMP_SpriteAsset : TMP_Asset
	{
		private Dictionary<int, int> m_UnicodeLookup;

		private Dictionary<int, int> m_NameLookup;

		public static TMP_SpriteAsset m_defaultSpriteAsset;

		public Texture spriteSheet;

		public List<TMP_Sprite> spriteInfoList;

		[SerializeField]
		public List<TMP_SpriteAsset> fallbackSpriteAssets;

		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				if ((Object)(object)m_defaultSpriteAsset == (Object)null)
				{
					m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				return m_defaultSpriteAsset;
			}
		}

		private void OnEnable()
		{
		}

		private void OnValidate()
		{
			TMPro_EventManager.ON_SPRITE_ASSET_PROPERTY_CHANGED(isChanged: true, (Object)(object)this);
		}

		private Material GetDefaultSpriteMaterial()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003f: Expected O, but got Unknown
			ShaderUtilities.GetShaderPropertyIDs();
			Material val = new Material(Shader.Find("TextMeshPro/Sprite"));
			val.SetTexture(ShaderUtilities.ID_MainTex, spriteSheet);
			((Object)val).set_hideFlags((HideFlags)1);
			AssetDatabase.AddObjectToAsset((Object)val, (Object)(object)this);
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Object)(object)this));
			return val;
		}

		public void UpdateLookupTables()
		{
			if (m_NameLookup == null)
			{
				m_NameLookup = new Dictionary<int, int>();
			}
			if (m_UnicodeLookup == null)
			{
				m_UnicodeLookup = new Dictionary<int, int>();
			}
			for (int i = 0; i < spriteInfoList.Count; i++)
			{
				int key = spriteInfoList[i].hashCode;
				if (!m_NameLookup.ContainsKey(key))
				{
					m_NameLookup.Add(key, i);
				}
				int unicode = spriteInfoList[i].unicode;
				if (!m_UnicodeLookup.ContainsKey(unicode))
				{
					m_UnicodeLookup.Add(unicode, i);
				}
			}
		}

		public int GetSpriteIndexFromHashcode(int hashCode)
		{
			if (m_NameLookup == null)
			{
				UpdateLookupTables();
			}
			int value = 0;
			if (m_NameLookup.TryGetValue(hashCode, out value))
			{
				return value;
			}
			return -1;
		}

		public int GetSpriteIndexFromUnicode(int unicode)
		{
			if (m_UnicodeLookup == null)
			{
				UpdateLookupTables();
			}
			int value = 0;
			if (m_UnicodeLookup.TryGetValue(unicode, out value))
			{
				return value;
			}
			return -1;
		}

		public int GetSpriteIndexFromName(string name)
		{
			if (m_NameLookup == null)
			{
				UpdateLookupTables();
			}
			int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(name);
			return GetSpriteIndexFromHashcode(simpleHashCode);
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(TMP_SpriteAsset spriteAsset, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if ((Object)(object)spriteAsset == (Object)null)
			{
				return null;
			}
			spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
			if (spriteIndex != -1)
			{
				return spriteAsset;
			}
			if (spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
			{
				for (int i = 0; i < spriteAsset.fallbackSpriteAssets.Count; i++)
				{
					if (spriteIndex != -1)
					{
						break;
					}
					TMP_SpriteAsset tMP_SpriteAsset = SearchFallbackForSprite(spriteAsset.fallbackSpriteAssets[i], unicode, out spriteIndex);
					if ((Object)(object)tMP_SpriteAsset != (Object)null)
					{
						return tMP_SpriteAsset;
					}
				}
			}
			return null;
		}

		public static TMP_SpriteAsset SearchFallbackForSprite(List<TMP_SpriteAsset> spriteAssets, int unicode, out int spriteIndex)
		{
			spriteIndex = -1;
			if (spriteAssets != null && spriteAssets.Count > 0)
			{
				for (int i = 0; i < spriteAssets.Count; i++)
				{
					TMP_SpriteAsset tMP_SpriteAsset = SearchFallbackForSprite(spriteAssets[i], unicode, out spriteIndex);
					if ((Object)(object)tMP_SpriteAsset != (Object)null)
					{
						return tMP_SpriteAsset;
					}
				}
			}
			return null;
		}
	}
}
