using System;
using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class CustomizationController : MonoBehaviour
{
	public static CustomizationController instance;

	public static Action onInitialized;

	public Camera thumbnailCamera;

	public CustomizationCameraController cameraController;

	public Transform turntable;

	public Transform mainCharacter;

	public Transform coopCharacter;

	public const int kStandardIconSize = 512;

	public const int kSwitchBrokenIconSize = 512;

	public const int kSwitchPresetIconSize = 128;

	public const int kSAPresetIconSize = 256;

	public const int kPresetIconSize = 166;

	private RagdollCustomization[] customizations = new RagdollCustomization[2];

	public bool showBoth = true;

	public int activePlayer;

	public RagdollCustomization activeCustomization;

	public float transitionDuration = 0.5f;

	public float transitionPhase = 1f;

	public float angleFrom;

	public float angleTo;

	private const bool kScaledIconEnable = true;

	private const int kScaledIconSize = 256;

	public Camera mainCamera => ((Component)cameraController.cameraHolder).GetComponentInChildren<Camera>();

	private void OnEnable()
	{
		instance = this;
		cameraController = ((Component)this).GetComponent<CustomizationCameraController>();
		if (onInitialized != null)
		{
			onInitialized();
		}
	}

	public void Initialize()
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		MenuCameraEffects.EnterCustomization();
		NetGame.instance.StopLocalGame();
		Listener.instance.OverrideTransform(cameraController.cameraHolder);
		customizations[0] = InitializeRagdoll(mainCharacter);
		customizations[1] = InitializeRagdoll(coopCharacter);
		ReloadSkin(0);
		ReloadSkin(1);
		activePlayer = PlayerPrefs.GetInt("MainSkinIndex", 0);
		activeCustomization = customizations[activePlayer];
		turntable.set_rotation(Quaternion.Euler(0f, (float)((activePlayer > 0) ? 180 : 0), 0f));
	}

	private RagdollCustomization InitializeRagdoll(Transform parent)
	{
		Ragdoll component = Object.Instantiate<GameObject>(((Component)Game.instance.ragdollPrefab).get_gameObject(), parent, false).GetComponent<Ragdoll>();
		component.Lock();
		((Component)component).get_gameObject().AddComponent<RagdollCustomization>();
		return ((Component)component).GetComponent<RagdollCustomization>();
	}

	public void Teardown()
	{
		MenuCameraEffects.LeaveCustomization();
		instance = null;
		Listener.instance.EndTransfromOverride();
		NetGame.instance.StartLocalGame();
	}

	public void SetActivePlayer(int index)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		PlayerPrefs.SetInt("MainSkinIndex", index);
		activePlayer = index;
		Quaternion rotation = turntable.get_rotation();
		angleFrom = ((Quaternion)(ref rotation)).get_eulerAngles().y;
		angleTo = ((index > 0) ? 180 : 0);
		if (angleFrom > angleTo)
		{
			angleFrom -= 360f;
		}
		transitionPhase = 0f;
		if (!showBoth)
		{
			ShowActive();
		}
		activeCustomization = customizations[index];
	}

	public void ShowBoth()
	{
		showBoth = true;
		((Component)mainCharacter).get_gameObject().SetActive(true);
		((Component)coopCharacter).get_gameObject().SetActive(true);
	}

	public void ShowActive()
	{
		showBoth = false;
		((Component)mainCharacter).get_gameObject().SetActive(activePlayer == 0);
		((Component)coopCharacter).get_gameObject().SetActive(activePlayer == 1);
	}

	private void Update()
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (transitionPhase < 1f)
		{
			transitionPhase = Math.Min(transitionPhase + Time.get_deltaTime() / transitionDuration, 1f);
			float num = Ease.easeInOutQuad(0f, 1f, transitionPhase);
			turntable.set_rotation(Quaternion.Euler(0f, Mathf.Lerp(angleFrom, angleTo, num), 0f));
		}
	}

	private void ReloadSkin(int playerIdx)
	{
		customizations[playerIdx].ApplyPreset(PresetRepository.ClonePreset(WorkshopRepository.instance.presetRepo.GetPlayerSkin(playerIdx)));
		customizations[playerIdx].RebindColors(bake: false);
	}

	public void SetColor(WorkshopItemType part, int channel, Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		activeCustomization.preset.SetColor(part, channel, color);
		activeCustomization.ApplyPreset(activeCustomization.preset);
		activeCustomization.RebindColors(bake: false);
	}

	public Color GetColor(WorkshopItemType part, int channel)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return activeCustomization.preset.GetColor(part, channel);
	}

	public void ClearColors()
	{
		activeCustomization.preset.ClearColors();
	}

	public RagdollPresetPartMetadata GetPart(WorkshopItemType part)
	{
		return activeCustomization.preset.GetPart(part);
	}

	public void SetPart(WorkshopItemType part, RagdollPresetPartMetadata data)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		if (data != null)
		{
			data.suppressCustomTexture = true;
		}
		activeCustomization.preset.SetPart(part, data);
		activeCustomization.ApplyPreset(activeCustomization.preset);
		RagdollModel model = activeCustomization.GetModel(part);
		if ((Object)(object)model != (Object)null)
		{
			data.color1 = HexConverter.ColorToHex(Color32.op_Implicit(model.color1));
			data.color2 = HexConverter.ColorToHex(Color32.op_Implicit(model.color2));
			data.color3 = HexConverter.ColorToHex(Color32.op_Implicit(model.color3));
		}
		activeCustomization.RebindColors(bake: false);
	}

	public void CommitParts()
	{
		WorkshopRepository.instance.presetRepo.SaveSkin(activePlayer, activeCustomization.preset);
		ReloadSkin(activePlayer);
	}

	public void Rollback()
	{
		ReloadSkin(activePlayer);
	}

	public string GetSkinPresetReference()
	{
		return WorkshopRepository.instance.presetRepo.GetSkinPresetReference(activePlayer);
	}

	public void LoadPreset(RagdollPresetMetadata preset)
	{
		activeCustomization.ApplyPreset(preset);
		activeCustomization.RebindColors(bake: false);
	}

	public void ApplyLoadedPreset()
	{
		WorkshopRepository.instance.presetRepo.SelectSkinPreset(activePlayer, activeCustomization.preset);
		ReloadSkin(activePlayer);
	}

	public RagdollPresetMetadata GetSkin()
	{
		return WorkshopRepository.instance.presetRepo.GetPlayerSkin(activePlayer);
	}

	public RagdollPresetMetadata SavePreset(RagdollPresetMetadata saveOver, string title, string description)
	{
		RagdollPresetMetadata ragdollPresetMetadata = WorkshopRepository.instance.presetRepo.SaveSkinAsPreset(activePlayer, activeCustomization.preset, saveOver, title, description);
		CaptureThumbnail(thumbnailCamera, ragdollPresetMetadata.thumbPath);
		ReloadSkin(activePlayer);
		return ragdollPresetMetadata;
	}

	public void EnsureThumbnails(List<RagdollPresetMetadata> items)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = showBoth;
		foreach (RagdollPresetMetadata item in items)
		{
			string thumbPath = item.thumbPath;
			if (FileTools.TestExists(thumbPath))
			{
				continue;
			}
			Debug.LogFormat("[THUMB] Rebuilding thumbnail {0} from preset...", new object[1] { thumbPath });
			if (!flag)
			{
				if (transitionPhase < 1f)
				{
					turntable.set_rotation(Quaternion.Euler(0f, angleTo, 0f));
				}
				if (showBoth)
				{
					ShowActive();
				}
			}
			instance.LoadPreset(item);
			CaptureThumbnail(thumbnailCamera, item.thumbPath);
			flag = true;
		}
		if (flag)
		{
			ReloadSkin(activePlayer);
			if (transitionPhase < 1f)
			{
				float num = Ease.easeInOutQuad(0f, 1f, transitionPhase);
				turntable.set_rotation(Quaternion.Euler(0f, Mathf.Lerp(angleFrom, angleTo, num), 0f));
			}
			if (flag2)
			{
				ShowBoth();
			}
		}
	}

	public void DeletePreset(RagdollPresetMetadata preset)
	{
		WorkshopRepository.instance.presetRepo.DeletePreset(preset);
	}

	private void CaptureThumbnail(Camera camera, string thumbPath)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		RenderTexture val = null;
		val = new RenderTexture(256, 256, 16, (RenderTextureFormat)0);
		RenderTexture val2 = null;
		RenderTexture val3 = null;
		val2 = new RenderTexture(512, 512, 16, (RenderTextureFormat)0);
		val3 = val2;
		camera.set_targetTexture(val3);
		camera.Render();
		Graphics.Blit((Texture)(object)val2, val);
		RenderTexture.set_active((RenderTexture)null);
		camera.set_targetTexture((RenderTexture)null);
		Object.DestroyImmediate((Object)(object)val2);
		RenderTexture.set_active(val);
		Texture2D val4 = new Texture2D(((Texture)val).get_width(), ((Texture)val).get_height(), (TextureFormat)3, false);
		val4.ReadPixels(new Rect(0f, 0f, (float)((Texture)val).get_width(), (float)((Texture)val).get_height()), 0, 0);
		RenderTexture.set_active((RenderTexture)null);
		camera.set_targetTexture((RenderTexture)null);
		Object.DestroyImmediate((Object)(object)val);
		FileTools.WriteTexture(thumbPath, val4);
	}

	public CustomizationController()
		: this()
	{
	}
}
