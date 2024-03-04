using System.IO;
using Steamworks;
using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	[CustomEditor(typeof(WorkshopRagdollModel))]
	public class WorkshopRagdollModelEditor : Editor
	{
		private SerializedProperty workshopId;

		private SerializedProperty title;

		private SerializedProperty model;

		private SerializedProperty description;

		private SerializedProperty thumbnail;

		private SerializedProperty updateNotes;

		private Vector2 scrollPos;

		private void OnEnable()
		{
			workshopId = ((Editor)this).get_serializedObject().FindProperty("workshopId");
			title = ((Editor)this).get_serializedObject().FindProperty("title");
			model = ((Editor)this).get_serializedObject().FindProperty("model");
			description = ((Editor)this).get_serializedObject().FindProperty("description");
			thumbnail = ((Editor)this).get_serializedObject().FindProperty("thumbnail");
			updateNotes = ((Editor)this).get_serializedObject().FindProperty("updateNotes");
		}

		public override void OnInspectorGUI()
		{
			((Editor)this).get_serializedObject().Update();
			EditorGUILayout.PropertyField(workshopId, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(title, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(description, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(model, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(thumbnail, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag = GUILayout.Button("Capture Thumbnail", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Game Preview", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			string @string = EditorPrefs.GetString("HumanWorkshopRoot", "<paste workshop root>");
			string text = EditorGUILayout.TextField("Workshop root folder", @string, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (@string != text)
			{
				EditorPrefs.SetString("HumanWorkshopRoot", text);
			}
			bool flag2 = GUILayout.Button("Export to game", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Steam Workshop", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(updateNotes, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag3 = GUILayout.Button("Upload", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
			if (flag2 || flag3)
			{
				BuildBundle(text, flag3);
			}
			if (flag)
			{
				CaptureThumbnail(((Editor)this).get_serializedObject().get_targetObject() as WorkshopRagdollModel);
			}
		}

		private void BuildBundle(string workshop, bool upload)
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			WorkshopRagdollModel workshopRagdollModel = ((Editor)this).get_serializedObject().get_targetObject() as WorkshopRagdollModel;
			if ((Object)(object)workshopRagdollModel.model == (Object)null)
			{
				Debug.LogError((object)"No model defined", (Object)(object)workshopRagdollModel);
				return;
			}
			string name = ((Object)workshopRagdollModel).get_name();
			string text = "data";
			string text2 = Path.Combine(workshop, "Models/" + name);
			string destFileName = Path.Combine(text2, text);
			string destFileName2 = Path.Combine(text2, "thumbnail.png");
			RagdollModelMetadata ragdollModelMetadata = new RagdollModelMetadata
			{
				itemType = workshopRagdollModel.model.ragdollPart,
				workshopId = workshopRagdollModel.workshopId,
				title = workshopRagdollModel.title,
				description = workshopRagdollModel.description
			};
			string text3 = (ragdollModelMetadata.model = AssetDatabase.GetAssetPath((Object)(object)workshopRagdollModel.model));
			BuildAssetBundleOptions val = (BuildAssetBundleOptions)0;
			val = (BuildAssetBundleOptions)(val | 1);
			AssetBundleBuild[] array = (AssetBundleBuild[])(object)new AssetBundleBuild[1]
			{
				new AssetBundleBuild
				{
					assetBundleName = text,
					assetNames = new string[1] { text3 }
				}
			};
			Directory.CreateDirectory("AssetBundles");
			BuildPipeline.BuildAssetBundles("AssetBundles", array, val, (BuildTarget)5);
			Directory.CreateDirectory(text2);
			File.Copy("AssetBundles\\" + text, destFileName, overwrite: true);
			if (Object.op_Implicit((Object)(object)workshopRagdollModel.thumbnail))
			{
				File.Copy(AssetDatabase.GetAssetPath((Object)(object)workshopRagdollModel.thumbnail), destFileName2, overwrite: true);
			}
			ragdollModelMetadata.Save(text2);
			if (upload)
			{
				if (!Object.op_Implicit((Object)(object)workshopRagdollModel.thumbnail))
				{
					EditorUtility.DisplayDialog("Human Workshop Error", "Thumbnail is required", "Close");
				}
				else
				{
					WorkshopUpload.Upload(ragdollModelMetadata, text2, destFileName2, workshopRagdollModel.updateNotes, OnUploaded);
				}
			}
		}

		private void OnUploaded(WorkshopItemMetadata meta, bool needAgreement, EResult res)
		{
			if (needAgreement)
			{
				EditorUtility.DisplayDialog("Human Workshop Error", "You need to agree to steam workshop agreement", "Close");
				return;
			}
			if (res != EResult.k_EResultOK)
			{
				EditorUtility.DisplayDialog("Human Workshop Error", res.ToString(), "Close");
				return;
			}
			EditorUtility.DisplayDialog("Human Workshop", "Ragdoll model uploaded", "Close");
			(((Editor)this).get_serializedObject().get_targetObject() as WorkshopRagdollModel).workshopId = meta.workshopId;
		}

		public static void CaptureThumbnail(WorkshopRagdollModel model)
		{
			PhotoRoom photoRoom = Object.FindObjectOfType<PhotoRoom>();
			if ((Object)(object)photoRoom == (Object)null)
			{
				Debug.LogError((object)"PhotoRoom not found in scene!");
				return;
			}
			switch (model.model.ragdollPart)
			{
			case WorkshopItemType.ModelFull:
				CaptureThumbnail(model, photoRoom.fullCamera);
				break;
			case WorkshopItemType.ModelHead:
				CaptureThumbnail(model, photoRoom.headCamera);
				break;
			case WorkshopItemType.ModelUpperBody:
				CaptureThumbnail(model, photoRoom.upperCamera);
				break;
			case WorkshopItemType.ModelLowerBody:
				CaptureThumbnail(model, photoRoom.lowerCamera);
				break;
			}
		}

		public static void CaptureThumbnail(WorkshopRagdollModel model, Camera camera)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			RenderTexture val = new RenderTexture(512, 512, 16, (RenderTextureFormat)0);
			camera.set_targetTexture(val);
			camera.Render();
			RenderTexture.set_active(val);
			Texture2D val2 = new Texture2D(((Texture)val).get_width(), ((Texture)val).get_height(), (TextureFormat)5, false);
			val2.ReadPixels(new Rect(0f, 0f, (float)((Texture)val).get_width(), (float)((Texture)val).get_height()), 0, 0);
			RenderTexture.set_active((RenderTexture)null);
			camera.set_targetTexture((RenderTexture)null);
			byte[] bytes = ImageConversion.EncodeToPNG(val2);
			Object.DestroyImmediate((Object)(object)val);
			string text = Path.ChangeExtension(AssetDatabase.GetAssetPath((Object)(object)model), ".png");
			File.WriteAllBytes(text, bytes);
			AssetDatabase.ImportAsset(text, (ImportAssetOptions)0);
			AssetImporter atPath = AssetImporter.GetAtPath(text);
			AssetImporter obj = ((atPath is TextureImporter) ? atPath : null);
			((TextureImporter)obj).set_textureType((TextureImporterType)8);
			((TextureImporter)obj).set_mipmapEnabled(false);
			AssetDatabase.ImportAsset(text, (ImportAssetOptions)0);
			model.thumbnail = AssetDatabase.LoadAssetAtPath<Texture2D>(text);
		}

		public WorkshopRagdollModelEditor()
			: this()
		{
		}
	}
}
