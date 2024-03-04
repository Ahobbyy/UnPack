using System;
using System.IO;
using HumanAPI;
using UnityEngine;

public class PresetRepository : WorkshopTypeRepository<RagdollPresetMetadata>
{
	public static string[] skinPaths = new string[2] { "pr:MainCharacter", "pr:CoopCharacter" };

	private void EnsureSkin(string path)
	{
		if (GetItem(path) == null)
		{
			RagdollPresetMetadata ragdollPresetMetadata = WorkshopItemMetadata.Load<RagdollPresetMetadata>(path);
			if (ragdollPresetMetadata == null)
			{
				ragdollPresetMetadata = CreateDefaultSkin();
				ragdollPresetMetadata.folder = path;
			}
			AddItem(WorkshopItemSource.BuiltIn, ragdollPresetMetadata);
		}
	}

	public static RagdollPresetMetadata CreateDefaultSkin()
	{
		return new RagdollPresetMetadata
		{
			folder = null,
			itemType = WorkshopItemType.RagdollPreset,
			main = new RagdollPresetPartMetadata
			{
				modelPath = "builtin:HumanDefaultBody"
			},
			head = new RagdollPresetPartMetadata
			{
				modelPath = "builtin:HumanHardHat"
			}
		};
	}

	public RagdollPresetMetadata GetPlayerSkin(int index)
	{
		return GetItem(skinPaths[index]);
	}

	public void LoadSkins()
	{
		EnsureSkin(skinPaths[0]);
		EnsureSkin(skinPaths[1]);
	}

	public void SaveSkin(int index, RagdollPresetMetadata skin)
	{
		CopySkinTextures(skin, skinPaths[index]);
		skin.folder = skinPaths[index];
		skin.Save(skin.folder);
		AddItem(WorkshopItemSource.BuiltIn, skin);
	}

	public string GetSkinPresetReference(int index)
	{
		if (index == 0)
		{
			return PlayerPrefs.GetString("MainCharacterPreset");
		}
		return PlayerPrefs.GetString("CoopCharacterPreset");
	}

	private void SetSkinPresetReference(int index, string folder)
	{
		if (index == 0)
		{
			PlayerPrefs.SetString("MainCharacterPreset", folder);
		}
		else
		{
			PlayerPrefs.SetString("CoopCharacterPreset", folder);
		}
	}

	public static RagdollPresetMetadata ClonePreset(RagdollPresetMetadata preset)
	{
		return new RagdollPresetMetadata
		{
			folder = preset.folder,
			itemType = preset.itemType,
			title = preset.title,
			description = preset.description,
			workshopId = preset.workshopId,
			main = RagdollPresetPartMetadata.Clone(preset.main),
			head = RagdollPresetPartMetadata.Clone(preset.head),
			upperBody = RagdollPresetPartMetadata.Clone(preset.upperBody),
			lowerBody = RagdollPresetPartMetadata.Clone(preset.lowerBody)
		};
	}

	public void SelectSkinPreset(int playerIdx, RagdollPresetMetadata preset)
	{
		SetSkinPresetReference(playerIdx, preset.folder);
		SaveSkin(playerIdx, ClonePreset(preset));
	}

	public RagdollPresetMetadata SaveSkinAsPreset(int playerIdx, RagdollPresetMetadata skin, RagdollPresetMetadata saveOver, string title, string description)
	{
		RagdollPresetMetadata ragdollPresetMetadata = ClonePreset(skin);
		ragdollPresetMetadata.title = title;
		ragdollPresetMetadata.description = description;
		if (saveOver != null)
		{
			ragdollPresetMetadata.folder = saveOver.folder;
			ragdollPresetMetadata.workshopId = saveOver.workshopId;
		}
		else
		{
			ragdollPresetMetadata.folder = GenerateNewPresetPath();
			ragdollPresetMetadata.workshopId = 0uL;
		}
		ragdollPresetMetadata.Save(ragdollPresetMetadata.folder);
		AddItem(WorkshopItemSource.LocalWorkshop, ragdollPresetMetadata);
		SetSkinPresetReference(playerIdx, ragdollPresetMetadata.folder);
		CopySkinTextures(skin, ragdollPresetMetadata.folder);
		return ragdollPresetMetadata;
	}

	public static void CopySkinTextures(RagdollPresetMetadata preset, string targetFolder)
	{
		CopyPartTexture(preset, WorkshopItemType.ModelFull, targetFolder);
		CopyPartTexture(preset, WorkshopItemType.ModelHead, targetFolder);
		CopyPartTexture(preset, WorkshopItemType.ModelLowerBody, targetFolder);
		CopyPartTexture(preset, WorkshopItemType.ModelUpperBody, targetFolder);
	}

	private static void CopyPartTexture(RagdollPresetMetadata preset, WorkshopItemType part, string targetFolder)
	{
		string sourcePath = FileTools.Combine(preset.folder, part.ToString() + ".png");
		string text = FileTools.Combine(targetFolder, part.ToString() + ".png");
		RagdollPresetPartMetadata part2 = preset.GetPart(part);
		if (part2 == null || string.IsNullOrEmpty(part2.modelPath) || part2.suppressCustomTexture)
		{
			FileTools.DeleteFile(text);
		}
		else
		{
			FileTools.Copy(sourcePath, text, deleteIfMissing: true);
		}
		if (part2 != null && part2.suppressCustomTexture)
		{
			part2.suppressCustomTexture = false;
		}
	}

	public void DeletePreset(RagdollPresetMetadata preset)
	{
		FileTools.DeleteFile(preset.metaPath);
		FileTools.DeleteFile(preset.thumbPath);
		FileTools.DeleteFile(FileTools.Combine(preset.folder, WorkshopItemType.ModelFull.ToString() + ".png"));
		FileTools.DeleteFile(FileTools.Combine(preset.folder, WorkshopItemType.ModelHead.ToString() + ".png"));
		FileTools.DeleteFile(FileTools.Combine(preset.folder, WorkshopItemType.ModelLowerBody.ToString() + ".png"));
		FileTools.DeleteFile(FileTools.Combine(preset.folder, WorkshopItemType.ModelUpperBody.ToString() + ".png"));
		RemoveItem(WorkshopItemSource.LocalWorkshop, preset);
	}

	public void ResetPresets()
	{
		RemoveItem(WorkshopItemSource.LocalWorkshop, GetPlayerSkin(0));
		RemoveItem(WorkshopItemSource.LocalWorkshop, GetPlayerSkin(1));
		LoadSkins();
	}

	public string GenerateNewPresetPath()
	{
		for (int i = 0; i < 100; i++)
		{
			string text = FileTools.Combine("pr:Presets", i.ToString());
			if (GetItem(text) == null)
			{
				return text;
			}
		}
		throw new InvalidOperationException("Too many presets!");
	}

	public void MigrateLegacySkin()
	{
		string text = Path.Combine(Application.get_persistentDataPath(), "characterTexture" + 0 + ".png");
		if (File.Exists(text))
		{
			RagdollPresetMetadata ragdollPresetMetadata = new RagdollPresetMetadata
			{
				folder = text,
				itemType = WorkshopItemType.RagdollPreset,
				main = new RagdollPresetPartMetadata
				{
					modelPath = "builtin:LegacyBody"
				}
			};
			ragdollPresetMetadata.title = "Legacy";
			ragdollPresetMetadata.folder = GenerateNewPresetPath();
			ragdollPresetMetadata.Save(ragdollPresetMetadata.folder);
			AddItem(WorkshopItemSource.LocalWorkshop, ragdollPresetMetadata);
			string targetPath = FileTools.Combine(ragdollPresetMetadata.folder, WorkshopItemType.ModelFull.ToString() + ".png");
			FileTools.Copy(text, targetPath, deleteIfMissing: true);
			File.Delete(text);
		}
	}
}
