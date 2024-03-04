using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UnityEngine.Rendering.PostProcessing
{
	public static class TextureFormatUtilities
	{
		private static Dictionary<int, RenderTextureFormat> s_FormatAliasMap;

		private static Dictionary<int, bool> s_SupportedRenderTextureFormats;

		private static Dictionary<int, bool> s_SupportedTextureFormats;

		static TextureFormatUtilities()
		{
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			s_FormatAliasMap = new Dictionary<int, RenderTextureFormat>
			{
				{
					1,
					(RenderTextureFormat)0
				},
				{
					2,
					(RenderTextureFormat)5
				},
				{
					3,
					(RenderTextureFormat)0
				},
				{
					4,
					(RenderTextureFormat)0
				},
				{
					5,
					(RenderTextureFormat)0
				},
				{
					7,
					(RenderTextureFormat)4
				},
				{
					9,
					(RenderTextureFormat)15
				},
				{
					10,
					(RenderTextureFormat)0
				},
				{
					12,
					(RenderTextureFormat)0
				},
				{
					13,
					(RenderTextureFormat)5
				},
				{
					14,
					(RenderTextureFormat)0
				},
				{
					15,
					(RenderTextureFormat)15
				},
				{
					16,
					(RenderTextureFormat)13
				},
				{
					17,
					(RenderTextureFormat)2
				},
				{
					18,
					(RenderTextureFormat)14
				},
				{
					19,
					(RenderTextureFormat)12
				},
				{
					20,
					(RenderTextureFormat)11
				},
				{
					22,
					(RenderTextureFormat)2
				},
				{
					26,
					(RenderTextureFormat)16
				},
				{
					27,
					(RenderTextureFormat)13
				},
				{
					24,
					(RenderTextureFormat)2
				},
				{
					25,
					(RenderTextureFormat)0
				},
				{
					28,
					(RenderTextureFormat)0
				},
				{
					29,
					(RenderTextureFormat)0
				},
				{
					30,
					(RenderTextureFormat)0
				},
				{
					31,
					(RenderTextureFormat)0
				},
				{
					32,
					(RenderTextureFormat)0
				},
				{
					33,
					(RenderTextureFormat)0
				},
				{
					35,
					(RenderTextureFormat)0
				},
				{
					36,
					(RenderTextureFormat)0
				},
				{
					34,
					(RenderTextureFormat)0
				},
				{
					45,
					(RenderTextureFormat)0
				},
				{
					46,
					(RenderTextureFormat)0
				},
				{
					47,
					(RenderTextureFormat)0
				},
				{
					48,
					(RenderTextureFormat)0
				},
				{
					49,
					(RenderTextureFormat)0
				},
				{
					50,
					(RenderTextureFormat)0
				},
				{
					51,
					(RenderTextureFormat)0
				},
				{
					52,
					(RenderTextureFormat)0
				},
				{
					53,
					(RenderTextureFormat)0
				},
				{
					54,
					(RenderTextureFormat)0
				},
				{
					55,
					(RenderTextureFormat)0
				},
				{
					56,
					(RenderTextureFormat)0
				},
				{
					57,
					(RenderTextureFormat)0
				},
				{
					58,
					(RenderTextureFormat)0
				},
				{
					59,
					(RenderTextureFormat)0
				},
				{
					60,
					(RenderTextureFormat)0
				},
				{
					61,
					(RenderTextureFormat)0
				}
			};
			s_SupportedRenderTextureFormats = new Dictionary<int, bool>();
			foreach (object value3 in Enum.GetValues(typeof(RenderTextureFormat)))
			{
				if ((int)value3 >= 0 && !IsObsolete(value3))
				{
					bool value = SystemInfo.SupportsRenderTextureFormat((RenderTextureFormat)value3);
					s_SupportedRenderTextureFormats[(int)value3] = value;
				}
			}
			s_SupportedTextureFormats = new Dictionary<int, bool>();
			foreach (object value4 in Enum.GetValues(typeof(TextureFormat)))
			{
				if ((int)value4 >= 0 && !IsObsolete(value4))
				{
					bool value2 = SystemInfo.SupportsTextureFormat((TextureFormat)value4);
					s_SupportedTextureFormats[(int)value4] = value2;
				}
			}
		}

		private static bool IsObsolete(object value)
		{
			ObsoleteAttribute[] array = (ObsoleteAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(ObsoleteAttribute), inherit: false);
			if (array != null)
			{
				return array.Length != 0;
			}
			return false;
		}

		public static RenderTextureFormat GetUncompressedRenderTextureFormat(Texture texture)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected I4, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			Assert.IsNotNull<Texture>(texture);
			if (texture is RenderTexture)
			{
				return ((RenderTexture)((texture is RenderTexture) ? texture : null)).get_format();
			}
			if (texture is Texture2D)
			{
				TextureFormat format = ((Texture2D)texture).get_format();
				if (!s_FormatAliasMap.TryGetValue((int)format, out var value))
				{
					throw new NotSupportedException("Texture format not supported");
				}
				return value;
			}
			return (RenderTextureFormat)7;
		}

		internal static bool IsSupported(this RenderTextureFormat format)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected I4, but got Unknown
			s_SupportedRenderTextureFormats.TryGetValue((int)format, out var value);
			return value;
		}

		internal static bool IsSupported(this TextureFormat format)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected I4, but got Unknown
			s_SupportedTextureFormats.TryGetValue((int)format, out var value);
			return value;
		}
	}
}
