using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class TMP_SpriteAssetMenu
	{
		[MenuItem("CONTEXT/TMP_SpriteAsset/Add Default Material", false, 2000)]
		private static void CopyTexture(MenuCommand command)
		{
			TMP_SpriteAsset tMP_SpriteAsset = (TMP_SpriteAsset)(object)command.context;
			if ((Object)(object)tMP_SpriteAsset != (Object)null && (Object)(object)tMP_SpriteAsset.material == (Object)null)
			{
				AddDefaultMaterial(tMP_SpriteAsset);
			}
		}

		[MenuItem("Assets/Create/TextMeshPro/Sprite Asset", false, 100)]
		public static void CreateTextMeshProObjectPerform()
		{
			Object activeObject = Selection.get_activeObject();
			if (activeObject == (Object)null || ((object)activeObject).GetType() != typeof(Texture2D))
			{
				Debug.LogWarning((object)"A texture which contains sprites must first be selected in order to create a TextMesh Pro Sprite Asset.");
				return;
			}
			Texture2D val = (Texture2D)(object)((activeObject is Texture2D) ? activeObject : null);
			string assetPath = AssetDatabase.GetAssetPath((Object)(object)val);
			string fileName = Path.GetFileName(assetPath);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
			string text = assetPath.Replace(fileName, "");
			TMP_SpriteAsset tMP_SpriteAsset = AssetDatabase.LoadAssetAtPath(text + fileNameWithoutExtension + ".asset", typeof(TMP_SpriteAsset)) as TMP_SpriteAsset;
			if (((Object)(object)tMP_SpriteAsset == (Object)null) ? true : false)
			{
				tMP_SpriteAsset = ScriptableObject.CreateInstance<TMP_SpriteAsset>();
				AssetDatabase.CreateAsset((Object)(object)tMP_SpriteAsset, text + fileNameWithoutExtension + ".asset");
				tMP_SpriteAsset.hashCode = TMP_TextUtilities.GetSimpleHashCode(((Object)tMP_SpriteAsset).get_name());
				tMP_SpriteAsset.spriteSheet = (Texture)(object)val;
				tMP_SpriteAsset.spriteInfoList = GetSpriteInfo((Texture)(object)val);
				AddDefaultMaterial(tMP_SpriteAsset);
			}
			else
			{
				tMP_SpriteAsset.spriteInfoList = UpdateSpriteInfo(tMP_SpriteAsset);
				if ((Object)(object)tMP_SpriteAsset.material == (Object)null)
				{
					AddDefaultMaterial(tMP_SpriteAsset);
				}
			}
			EditorUtility.SetDirty((Object)(object)tMP_SpriteAsset);
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Object)(object)tMP_SpriteAsset));
		}

		private static List<TMP_Sprite> GetSpriteInfo(Texture source)
		{
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			Sprite[] array = (from x in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((Object)(object)source))
				select (Sprite)(object)((x is Sprite) ? x : null) into x
				where (Object)(object)x != (Object)null
				select x).OrderByDescending(delegate(Sprite x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rect rect3 = x.get_rect();
				return ((Rect)(ref rect3)).get_y();
			}).ThenBy(delegate(Sprite x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rect rect2 = x.get_rect();
				return ((Rect)(ref rect2)).get_x();
			}).ToArray();
			List<TMP_Sprite> list = new List<TMP_Sprite>();
			Vector2 val2 = default(Vector2);
			for (int i = 0; i < array.Length; i++)
			{
				TMP_Sprite tMP_Sprite = new TMP_Sprite();
				Sprite val = array[i];
				tMP_Sprite.id = i;
				tMP_Sprite.name = ((Object)val).get_name();
				tMP_Sprite.hashCode = TMP_TextUtilities.GetSimpleHashCode(tMP_Sprite.name);
				Rect rect = val.get_rect();
				tMP_Sprite.x = ((Rect)(ref rect)).get_x();
				tMP_Sprite.y = ((Rect)(ref rect)).get_y();
				tMP_Sprite.width = ((Rect)(ref rect)).get_width();
				tMP_Sprite.height = ((Rect)(ref rect)).get_height();
				Bounds bounds = val.get_bounds();
				float x2 = ((Bounds)(ref bounds)).get_min().x;
				bounds = val.get_bounds();
				float num = 0f - x2 / (((Bounds)(ref bounds)).get_extents().x * 2f);
				bounds = val.get_bounds();
				float y = ((Bounds)(ref bounds)).get_min().y;
				bounds = val.get_bounds();
				((Vector2)(ref val2))._002Ector(num, 0f - y / (((Bounds)(ref bounds)).get_extents().y * 2f));
				tMP_Sprite.pivot = new Vector2(0f - val2.x * ((Rect)(ref rect)).get_width(), ((Rect)(ref rect)).get_height() - val2.y * ((Rect)(ref rect)).get_height());
				tMP_Sprite.sprite = val;
				tMP_Sprite.xAdvance = ((Rect)(ref rect)).get_width();
				tMP_Sprite.scale = 1f;
				tMP_Sprite.xOffset = tMP_Sprite.pivot.x;
				tMP_Sprite.yOffset = tMP_Sprite.pivot.y;
				list.Add(tMP_Sprite);
			}
			return list;
		}

		private static void AddDefaultMaterial(TMP_SpriteAsset spriteAsset)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			Material val = new Material(Shader.Find("TextMeshPro/Sprite"));
			val.SetTexture(ShaderUtilities.ID_MainTex, spriteAsset.spriteSheet);
			spriteAsset.material = val;
			((Object)val).set_hideFlags((HideFlags)1);
			AssetDatabase.AddObjectToAsset((Object)(object)val, (Object)(object)spriteAsset);
		}

		private static List<TMP_Sprite> UpdateSpriteInfo(TMP_SpriteAsset spriteAsset)
		{
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			Sprite[] array = (from x in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath((Object)(object)spriteAsset.spriteSheet))
				select (Sprite)(object)((x is Sprite) ? x : null) into x
				where (Object)(object)x != (Object)null
				select x).OrderByDescending(delegate(Sprite x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rect rect3 = x.get_rect();
				return ((Rect)(ref rect3)).get_y();
			}).ThenBy(delegate(Sprite x)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Rect rect2 = x.get_rect();
				return ((Rect)(ref rect2)).get_x();
			}).ToArray();
			Vector2 val = default(Vector2);
			for (int i = 0; i < array.Length; i++)
			{
				Sprite sprite = array[i];
				int num = -1;
				if (spriteAsset.spriteInfoList.Count > i && (Object)(object)spriteAsset.spriteInfoList[i].sprite != (Object)null)
				{
					num = spriteAsset.spriteInfoList.FindIndex((TMP_Sprite item) => ((Object)item.sprite).GetInstanceID() == ((Object)sprite).GetInstanceID());
				}
				TMP_Sprite tMP_Sprite = ((num == -1) ? new TMP_Sprite() : spriteAsset.spriteInfoList[num]);
				Rect rect = sprite.get_rect();
				tMP_Sprite.x = ((Rect)(ref rect)).get_x();
				tMP_Sprite.y = ((Rect)(ref rect)).get_y();
				tMP_Sprite.width = ((Rect)(ref rect)).get_width();
				tMP_Sprite.height = ((Rect)(ref rect)).get_height();
				Bounds bounds = sprite.get_bounds();
				float x2 = ((Bounds)(ref bounds)).get_min().x;
				bounds = sprite.get_bounds();
				float num2 = 0f - x2 / (((Bounds)(ref bounds)).get_extents().x * 2f);
				bounds = sprite.get_bounds();
				float y = ((Bounds)(ref bounds)).get_min().y;
				bounds = sprite.get_bounds();
				((Vector2)(ref val))._002Ector(num2, 0f - y / (((Bounds)(ref bounds)).get_extents().y * 2f));
				tMP_Sprite.pivot = new Vector2(0f - val.x * ((Rect)(ref rect)).get_width(), ((Rect)(ref rect)).get_height() - val.y * ((Rect)(ref rect)).get_height());
				if (num == -1)
				{
					int[] array2 = spriteAsset.spriteInfoList.Select((TMP_Sprite item) => item.id).ToArray();
					int id = 0;
					for (int j = 0; j < array2.Length; j++)
					{
						if (array2[0] != 0)
						{
							break;
						}
						if (j > 0 && array2[j] - array2[j - 1] > 1)
						{
							id = array2[j - 1] + 1;
							break;
						}
						id = j + 1;
					}
					tMP_Sprite.sprite = sprite;
					tMP_Sprite.name = ((Object)sprite).get_name();
					tMP_Sprite.hashCode = TMP_TextUtilities.GetSimpleHashCode(tMP_Sprite.name);
					tMP_Sprite.id = id;
					tMP_Sprite.xAdvance = ((Rect)(ref rect)).get_width();
					tMP_Sprite.scale = 1f;
					tMP_Sprite.xOffset = tMP_Sprite.pivot.x;
					tMP_Sprite.yOffset = tMP_Sprite.pivot.y;
					spriteAsset.spriteInfoList.Add(tMP_Sprite);
					spriteAsset.spriteInfoList = spriteAsset.spriteInfoList.OrderBy((TMP_Sprite s) => s.id).ToList();
				}
				else
				{
					spriteAsset.spriteInfoList[num] = tMP_Sprite;
				}
			}
			return spriteAsset.spriteInfoList;
		}
	}
}
