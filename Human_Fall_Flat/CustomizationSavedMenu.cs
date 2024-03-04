using HumanAPI;
using Multiplayer;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationSavedMenu : MenuTransition
{
	public static RagdollPresetMetadata preset;

	public Button PublishButton;

	public Button AgreeButton;

	private string tempFolder;

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		CustomizationController.instance.cameraController.FocusCharacter();
	}

	protected override void Update()
	{
		base.Update();
		bool sSteamServersConnected = NetTransportSteam.sSteamServersConnected;
		if ((Object)(object)PublishButton != (Object)null)
		{
			((Selectable)PublishButton).set_interactable(sSteamServersConnected);
		}
		if ((Object)(object)AgreeButton != (Object)null)
		{
			((Selectable)AgreeButton).set_interactable(sSteamServersConnected);
		}
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke && ((Component)this).get_gameObject().get_activeSelf())
		{
			TransitionBack<CustomizationRootMenu>();
		}
	}

	public void WokshopTermsClick()
	{
		if (MenuSystem.CanInvoke)
		{
			WorkshopUpload.ShowWorkshopAgreement();
		}
	}

	public void WorkshopUploadClick()
	{
		if (MenuSystem.CanInvoke)
		{
			SteamProgressOverlay.instance.ShowSteamProgress(showProgress: true, null, null);
			((Component)this).get_gameObject().SetActive(false);
			tempFolder = FileTools.GetTempDirectory();
			string text = FileTools.Combine(tempFolder, "thumbnail.png");
			preset.Save(tempFolder);
			FileTools.WriteTexture(text, preset.thumbnailTexture);
			preset.ReleaseThumbnailReference();
			PresetRepository.CopySkinTextures(preset, tempFolder);
			WorkshopUpload.Upload(preset, tempFolder, text, "", OnPublishOver);
		}
	}

	private void OnPublishOver(WorkshopItemMetadata meta, bool needAgreement, EResult error)
	{
		FileTools.DeleteTempDirectory(tempFolder);
		switch (error)
		{
		case EResult.k_EResultOK:
			SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, null, null);
			((Component)this).get_gameObject().SetActive(true);
			preset.workshopId = meta.workshopId;
			preset.Save(preset.folder);
			TransitionForward<CustomizationRootMenu>();
			return;
		case EResult.k_EResultFileNotFound:
			preset.workshopId = 0uL;
			preset.Save(preset.folder);
			break;
		}
		SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, "", DismissSteamError);
	}

	public void DismissSteamError()
	{
		SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, null, null);
		((Component)this).get_gameObject().SetActive(true);
		TransitionForward<CustomizationRootMenu>();
	}
}
