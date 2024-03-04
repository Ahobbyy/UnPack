using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class EditorShaderUtilities
	{
		public static void CopyMaterialProperties(Material source, Material destination)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected I4, but got Unknown
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			Object[] array = (Object[])(object)new Material[1] { source };
			MaterialProperty[] materialProperties = MaterialEditor.GetMaterialProperties(array);
			for (int i = 0; i < materialProperties.Length; i++)
			{
				int num = Shader.PropertyToID(materialProperties[i].get_name());
				if (destination.HasProperty(num))
				{
					ShaderPropertyType propertyType = ShaderUtil.GetPropertyType(source.get_shader(), i);
					switch ((int)propertyType)
					{
					case 0:
						destination.SetColor(num, source.GetColor(num));
						break;
					case 2:
						destination.SetFloat(num, source.GetFloat(num));
						break;
					case 3:
						destination.SetFloat(num, source.GetFloat(num));
						break;
					case 4:
						destination.SetTexture(num, source.GetTexture(num));
						break;
					case 1:
						destination.SetVector(num, source.GetVector(num));
						break;
					}
				}
			}
		}
	}
}
