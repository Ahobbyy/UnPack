using System.IO;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public class TMP_ContextMenus : Editor
	{
		private static Texture m_copiedTexture;

		private static Material m_copiedProperties;

		private static Material m_copiedAtlasProperties;

		[MenuItem("CONTEXT/Texture/Copy", false, 2000)]
		private static void CopyTexture(MenuCommand command)
		{
			Object context = command.context;
			m_copiedTexture = (Texture)(object)((context is Texture) ? context : null);
		}

		[MenuItem("CONTEXT/Material/Select Material", false, 500)]
		private static void SelectMaterial(MenuCommand command)
		{
			Object context = command.context;
			Object obj = ((context is Material) ? context : null);
			EditorUtility.FocusProjectWindow();
			EditorGUIUtility.PingObject(obj);
		}

		[MenuItem("CONTEXT/Material/Create Material Preset", false)]
		private static void DuplicateMaterial(MenuCommand command)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			Material val = (Material)command.context;
			if (!EditorUtility.IsPersistent((Object)(object)val))
			{
				Debug.LogWarning((object)"Material is an instance and cannot be converted into a permanent asset.");
				return;
			}
			string text = AssetDatabase.GetAssetPath((Object)(object)val).Split('.')[0];
			Material val2 = new Material(val);
			val2.set_shaderKeywords(val.get_shaderKeywords());
			AssetDatabase.CreateAsset((Object)(object)val2, AssetDatabase.GenerateUniqueAssetPath(text + ".mat"));
			if ((Object)(object)Selection.get_activeGameObject() != (Object)null)
			{
				TMP_Text component = Selection.get_activeGameObject().GetComponent<TMP_Text>();
				if ((Object)(object)component != (Object)null)
				{
					component.fontSharedMaterial = val2;
				}
				else
				{
					TMP_SubMesh component2 = Selection.get_activeGameObject().GetComponent<TMP_SubMesh>();
					if ((Object)(object)component2 != (Object)null)
					{
						component2.sharedMaterial = val2;
					}
					else
					{
						TMP_SubMeshUI component3 = Selection.get_activeGameObject().GetComponent<TMP_SubMeshUI>();
						if ((Object)(object)component3 != (Object)null)
						{
							component3.sharedMaterial = val2;
						}
					}
				}
			}
			EditorUtility.FocusProjectWindow();
			EditorGUIUtility.PingObject((Object)(object)val2);
		}

		[MenuItem("CONTEXT/Material/Copy Material Properties", false)]
		private static void CopyMaterialProperties(MenuCommand command)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			Material val = null;
			val = (Material)((((object)command.context).GetType() != typeof(Material)) ? ((object)Selection.get_activeGameObject().GetComponent<CanvasRenderer>().GetMaterial()) : ((object)(Material)command.context));
			m_copiedProperties = new Material(val);
			m_copiedProperties.set_shaderKeywords(val.get_shaderKeywords());
			((Object)m_copiedProperties).set_hideFlags((HideFlags)52);
		}

		[MenuItem("CONTEXT/Material/Paste Material Properties", false)]
		private static void PasteMaterialProperties(MenuCommand command)
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			if ((Object)(object)m_copiedProperties == (Object)null)
			{
				Debug.LogWarning((object)"No Material Properties to Paste. Use Copy Material Properties first.");
				return;
			}
			Material val = null;
			val = (Material)((((object)command.context).GetType() != typeof(Material)) ? ((object)Selection.get_activeGameObject().GetComponent<CanvasRenderer>().GetMaterial()) : ((object)(Material)command.context));
			Undo.RecordObject((Object)(object)val, "Paste Material");
			ShaderUtilities.GetShaderPropertyIDs();
			if (val.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				m_copiedProperties.SetTexture(ShaderUtilities.ID_MainTex, val.GetTexture(ShaderUtilities.ID_MainTex));
				m_copiedProperties.SetFloat(ShaderUtilities.ID_GradientScale, val.GetFloat(ShaderUtilities.ID_GradientScale));
				m_copiedProperties.SetFloat(ShaderUtilities.ID_TextureWidth, val.GetFloat(ShaderUtilities.ID_TextureWidth));
				m_copiedProperties.SetFloat(ShaderUtilities.ID_TextureHeight, val.GetFloat(ShaderUtilities.ID_TextureHeight));
			}
			EditorShaderUtilities.CopyMaterialProperties(m_copiedProperties, val);
			val.set_shaderKeywords(m_copiedProperties.get_shaderKeywords());
			TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, val);
		}

		[MenuItem("CONTEXT/Material/Reset", false, 2100)]
		private static void ResetSettings(MenuCommand command)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			Material val = null;
			val = (Material)((((object)command.context).GetType() != typeof(Material)) ? ((object)Selection.get_activeGameObject().GetComponent<CanvasRenderer>().GetMaterial()) : ((object)(Material)command.context));
			Undo.RecordObject((Object)(object)val, "Reset Material");
			Material val2 = new Material(val.get_shader());
			ShaderUtilities.GetShaderPropertyIDs();
			if (val.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				val2.SetTexture(ShaderUtilities.ID_MainTex, val.GetTexture(ShaderUtilities.ID_MainTex));
				val2.SetFloat(ShaderUtilities.ID_GradientScale, val.GetFloat(ShaderUtilities.ID_GradientScale));
				val2.SetFloat(ShaderUtilities.ID_TextureWidth, val.GetFloat(ShaderUtilities.ID_TextureWidth));
				val2.SetFloat(ShaderUtilities.ID_TextureHeight, val.GetFloat(ShaderUtilities.ID_TextureHeight));
				val2.SetFloat(ShaderUtilities.ID_StencilID, val.GetFloat(ShaderUtilities.ID_StencilID));
				val2.SetFloat(ShaderUtilities.ID_StencilComp, val.GetFloat(ShaderUtilities.ID_StencilComp));
				val.CopyPropertiesFromMaterial(val2);
				val.set_shaderKeywords(new string[0]);
			}
			else
			{
				val.CopyPropertiesFromMaterial(val2);
			}
			Object.DestroyImmediate((Object)(object)val2);
			TMPro_EventManager.ON_MATERIAL_PROPERTY_CHANGED(isChanged: true, val);
		}

		[MenuItem("CONTEXT/Material/Copy Atlas", false, 2000)]
		private static void CopyAtlas(MenuCommand command)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			Object context = command.context;
			m_copiedAtlasProperties = new Material((Material)(object)((context is Material) ? context : null));
			((Object)m_copiedAtlasProperties).set_hideFlags((HideFlags)52);
		}

		[MenuItem("CONTEXT/Material/Paste Atlas", false, 2001)]
		private static void PasteAtlas(MenuCommand command)
		{
			Object context = command.context;
			Material val = (Material)(object)((context is Material) ? context : null);
			if ((Object)(object)m_copiedAtlasProperties != (Object)null)
			{
				Undo.RecordObject((Object)(object)val, "Paste Texture");
				ShaderUtilities.GetShaderPropertyIDs();
				val.SetTexture(ShaderUtilities.ID_MainTex, m_copiedAtlasProperties.GetTexture(ShaderUtilities.ID_MainTex));
				val.SetFloat(ShaderUtilities.ID_GradientScale, m_copiedAtlasProperties.GetFloat(ShaderUtilities.ID_GradientScale));
				val.SetFloat(ShaderUtilities.ID_TextureWidth, m_copiedAtlasProperties.GetFloat(ShaderUtilities.ID_TextureWidth));
				val.SetFloat(ShaderUtilities.ID_TextureHeight, m_copiedAtlasProperties.GetFloat(ShaderUtilities.ID_TextureHeight));
			}
			else if ((Object)(object)m_copiedTexture != (Object)null)
			{
				Undo.RecordObject((Object)(object)val, "Paste Texture");
				val.SetTexture(ShaderUtilities.ID_MainTex, m_copiedTexture);
			}
		}

		[MenuItem("CONTEXT/TMP_FontAsset/Extract Atlas", false, 2000)]
		private static void ExtractAtlas(MenuCommand command)
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			TMP_FontAsset tMP_FontAsset = command.context as TMP_FontAsset;
			string assetPath = AssetDatabase.GetAssetPath((Object)(object)tMP_FontAsset);
			string text = Path.GetDirectoryName(assetPath) + "/" + Path.GetFileNameWithoutExtension(assetPath) + " Atlas.png";
			SerializedObject val = new SerializedObject((Object)(object)tMP_FontAsset.material.GetTexture(ShaderUtilities.ID_MainTex));
			val.FindProperty("m_IsReadable").set_boolValue(true);
			val.ApplyModifiedProperties();
			Texture obj = Object.Instantiate<Texture>(tMP_FontAsset.material.GetTexture(ShaderUtilities.ID_MainTex));
			Texture2D val2 = (Texture2D)(object)((obj is Texture2D) ? obj : null);
			val.FindProperty("m_IsReadable").set_boolValue(false);
			val.ApplyModifiedProperties();
			Debug.Log((object)text);
			byte[] bytes = ImageConversion.EncodeToPNG(val2);
			File.WriteAllBytes(text, bytes);
			AssetDatabase.Refresh();
			Object.DestroyImmediate((Object)(object)val2);
		}

		public TMP_ContextMenus()
			: this()
		{
		}
	}
}
