using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.ImageEffects;

public class MenuCameraEffects : MonoBehaviour, IDependency
{
	public enum AntialiasType
	{
		None,
		FastApproximateAntialiasing,
		SubpixelMorphologicalAntialiasing,
		TemporalAntialiasing,
		MSAA
	}

	public enum AmbientOcclusionMode
	{
		None,
		ScalableAmbientObscurance,
		MultiScaleVolumetricObscurance
	}

	public class GameCamera
	{
		public Camera camera;

		public Transform holder;

		public BlurOptimized blur;

		public PostProcessLayer layer;

		public DepthOfField depthOfField;

		internal int resetCounter;

		public void SetViewport(int number, int total)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			if (number >= 4 || number >= total)
			{
				camera.set_rect(new Rect(1.5f, 0f, 0.1f, 0.1f));
				return;
			}
			float num = 0f;
			float num2 = 1f;
			float num3 = 0f;
			float num4 = 1f;
			if (total > 2)
			{
				num3 = 0.5f;
				if (number >= 2)
				{
					num4 -= 0.5f;
					num3 -= 0.5f;
				}
			}
			if (total > 1)
			{
				num2 = 0.5f;
				if (((uint)number & (true ? 1u : 0u)) != 0)
				{
					num += 0.5f;
					num2 += 0.5f;
				}
			}
			num = Math.Max(num, ((Rect)(ref sonyScreenClip)).get_xMin());
			num2 = Math.Min(num2, ((Rect)(ref sonyScreenClip)).get_xMax());
			num3 = Math.Max(num3, ((Rect)(ref sonyScreenClip)).get_yMin());
			num4 = Math.Min(num4, ((Rect)(ref sonyScreenClip)).get_yMax());
			camera.set_rect(new Rect(num, num3, num2 - num, num4 - num3));
			((Behaviour)camera).set_enabled(true);
		}
	}

	public class FloatTransition
	{
		public float from;

		public float to;

		public float current;

		public float duration;

		public float time;

		public FloatTransition(float value)
		{
			from = (to = (current = value));
			time = (duration = 0f);
		}

		public void Transition(float to, float duration)
		{
			from = current;
			this.to = to;
			this.duration = duration;
			time = 0f;
		}

		public bool Step(float dt)
		{
			float num = current;
			time += dt;
			if (duration == 0f || time >= duration)
			{
				current = to;
			}
			else
			{
				current = Ease.easeInOutQuad(from, to, time / duration);
			}
			return num != current;
		}
	}

	public PostProcessProfile postProfile;

	public PostProcessProfile[] profiles;

	public PostProcessVolume creditsPostProcessVolume;

	public static MenuCameraEffects instance;

	public static Rect sonyScreenClip = new Rect(0f, 0f, 1f, 1f);

	private List<GameCamera> activeCameras = new List<GameCamera>();

	private List<GameCamera> gameCameras = new List<GameCamera>();

	private GameCamera tempCamera;

	private bool destroyTempOnExit;

	private AntialiasType antialiasing = AntialiasType.TemporalAntialiasing;

	private AmbientOcclusionMode occlusion = AmbientOcclusionMode.MultiScaleVolumetricObscurance;

	private bool allowHDR = true;

	private bool allowBloom = true;

	private bool allowDepthOfField = true;

	private bool allowChromaticAberration = true;

	private bool allowExposure = true;

	private bool blockBlur;

	public float cameraZoom = 1f;

	public Vector2 cameraCenter = Vector2.get_zero();

	public float creditsAdjust;

	[NonSerialized]
	public FloatTransition zoom = new FloatTransition(1f);

	private float transitionDuration = 0.5f;

	private FloatTransition blur = new FloatTransition(0f);

	private FloatTransition offsetX = new FloatTransition(0f);

	private FloatTransition offsetY = new FloatTransition(0f);

	private FloatTransition vignette = new FloatTransition(0f);

	private FloatTransition dim = new FloatTransition(0f);

	private FloatTransition credits = new FloatTransition(0f);

	private FloatTransition superMasterFade = new FloatTransition(0f);

	private int bonusViewports;

	private Material defaultSkyboxMaterial;

	private Material skyboxMaterial;

	private Color defaultSkyboxColor;

	private Color creditsSkyboxColor = new Color(121f / 255f, 25f / 51f, 127f / 255f);

	public bool CreditsAdjustInTransition => credits.current != credits.to;

	public static void EnterCustomization()
	{
		instance.blockBlur = true;
		ApplyEffects();
	}

	public static void LeaveCustomization()
	{
		instance.blockBlur = false;
		ApplyEffects();
	}

	public void Initialize()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		defaultSkyboxMaterial = RenderSettings.get_skybox();
		skyboxMaterial = new Material(defaultSkyboxMaterial);
		RenderSettings.set_skybox(skyboxMaterial);
		defaultSkyboxColor = new Color(0.585f, 0.585f, 0.585f);
		Dependencies.OnInitialized(this);
	}

	private static GameCamera WrapCamera(Camera camera)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		PostProcessLayer postProcessLayer = ((Component)camera).GetComponent<PostProcessLayer>();
		CameraController3 componentInParent = ((Component)camera).GetComponentInParent<CameraController3>();
		if ((Object)(object)postProcessLayer == (Object)null)
		{
			postProcessLayer = new PostProcessLayer();
		}
		GameCamera gameCamera = new GameCamera
		{
			camera = camera,
			blur = ((Component)camera).GetComponent<BlurOptimized>(),
			holder = ((Component)camera).get_transform().get_parent(),
			resetCounter = 3,
			layer = postProcessLayer
		};
		if ((Object)(object)componentInParent != (Object)null)
		{
			postProcessLayer.volumeTrigger = ((Component)componentInParent.human).get_transform();
			GameObject val = new GameObject("Quick Volume (override focus)");
			val.get_transform().SetParent(postProcessLayer.volumeTrigger, false);
			val.set_layer(30);
			SphereCollider obj = val.AddComponent<SphereCollider>();
			((Collider)obj).set_isTrigger(true);
			obj.set_radius(0.1f);
			PostProcessVolume postProcessVolume = val.AddComponent<PostProcessVolume>();
			postProcessVolume.priority = 2f;
			PostProcessProfile postProcessProfile = (postProcessVolume.sharedProfile = postProcessVolume.profile);
			gameCamera.depthOfField = ScriptableObject.CreateInstance<DepthOfField>();
			gameCamera.depthOfField.enabled.Override(instance.allowDepthOfField && !instance.blockBlur);
			postProcessProfile.AddSettings(gameCamera.depthOfField);
		}
		else
		{
			postProcessLayer.volumeTrigger = ((Component)camera).get_transform();
		}
		return gameCamera;
	}

	public void SetNetJoinLocalSpinner(bool on)
	{
		bonusViewports = (on ? 1 : 0);
		SetupViewports();
	}

	public void AddHuman(NetPlayer player)
	{
		player.cameraController.gameCam = AddCamera(((Component)player.cameraController).get_transform());
		((Component)player.cameraController.gameCam).get_gameObject().AddComponent<RagdollTransparency>().Initialize(player.cameraController, player.human.ragdoll);
		SetupViewports();
	}

	public void RemoveHuman(NetPlayer player)
	{
		RemoveCamera(((Component)player.cameraController).get_transform());
		SetupViewports();
	}

	public void SetupViewports()
	{
		for (int i = 0; i < gameCameras.Count; i++)
		{
			gameCameras[i].SetViewport(i, gameCameras.Count + bonusViewports);
		}
	}

	public void SetCOOPFullScreenViewport(bool both)
	{
		if (both)
		{
			gameCameras[0].SetViewport(0, 2);
			gameCameras[1].SetViewport(1, 2);
		}
		else
		{
			gameCameras[0].SetViewport(0, 1);
			((Behaviour)gameCameras[1].camera).set_enabled(false);
		}
	}

	private GameCamera FindCamera(Camera cam)
	{
		for (int i = 0; i < activeCameras.Count; i++)
		{
			if ((Object)(object)activeCameras[i].camera == (Object)(object)cam)
			{
				return activeCameras[i];
			}
		}
		return null;
	}

	private Camera AddCamera(Transform parent)
	{
		Camera component = Object.Instantiate<GameObject>(((Component)Game.instance.cameraPrefab).get_gameObject(), parent, false).GetComponent<Camera>();
		GameCamera item = WrapCamera(component);
		gameCameras.Add(item);
		if (tempCamera == null)
		{
			activeCameras.Add(item);
			ApplyEffects();
		}
		else
		{
			((Component)component).get_gameObject().SetActive(false);
		}
		return component;
	}

	private void RemoveCamera(Transform parent)
	{
		for (int num = gameCameras.Count - 1; num >= 0; num--)
		{
			if ((Object)(object)gameCameras[num].holder == (Object)(object)parent)
			{
				gameCameras.RemoveAt(num);
			}
		}
		for (int num2 = activeCameras.Count - 1; num2 >= 0; num2--)
		{
			if ((Object)(object)activeCameras[num2].holder == (Object)(object)parent)
			{
				activeCameras.RemoveAt(num2);
			}
		}
	}

	public Camera OverrideCamera(Transform parent, bool applyEffects)
	{
		Camera component = ((Component)parent).GetComponent<Camera>();
		if ((Object)(object)component == (Object)null)
		{
			component = Object.Instantiate<GameObject>(((Component)Game.instance.cameraPrefab).get_gameObject(), parent, false).GetComponent<Camera>();
			destroyTempOnExit = true;
		}
		else
		{
			destroyTempOnExit = false;
		}
		tempCamera = WrapCamera(component);
		for (int i = 0; i < gameCameras.Count; i++)
		{
			((Component)gameCameras[i].camera).get_gameObject().SetActive(false);
		}
		activeCameras.Clear();
		if (applyEffects)
		{
			activeCameras.Add(tempCamera);
			ApplyEffects();
		}
		Listener.instance.OverrideTransform(((Component)component).get_transform());
		return component;
	}

	public void RemoveOverride()
	{
		if (Object.op_Implicit((Object)(object)Listener.instance))
		{
			Listener.instance.EndTransfromOverride();
		}
		if (destroyTempOnExit)
		{
			Object.Destroy((Object)(object)((Component)tempCamera.camera).get_gameObject());
		}
		destroyTempOnExit = false;
		for (int i = 0; i < gameCameras.Count; i++)
		{
			((Component)gameCameras[i].camera).get_gameObject().SetActive(true);
		}
		activeCameras.Clear();
		activeCameras.AddRange(gameCameras);
		tempCamera = null;
		ApplyEffects();
	}

	private void Transition(float cameraZoom, Vector2 cameraOffset, float blur, float vignette, float dim, float credits = 0f)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		zoom.Transition(cameraZoom, transitionDuration);
		offsetX.Transition(cameraOffset.x, transitionDuration);
		offsetY.Transition(cameraOffset.y, transitionDuration);
		this.blur.Transition(blur, transitionDuration);
		this.vignette.Transition(vignette, transitionDuration);
		this.dim.Transition(dim, transitionDuration);
		this.credits.Transition(credits, transitionDuration);
	}

	private void ApplyTransition()
	{
		Step(transitionDuration * 2f);
	}

	public void BlackOut()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Transition(1f, Vector2.get_zero(), 0f, 1f, 1f);
		ApplyTransition();
	}

	public void FadeOutBlackOut()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		transitionDuration = 5f;
		Transition(1f, Vector2.get_zero(), 0f, 0.7f, 0f);
	}

	public static void FadeInMainMenu()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		instance.transitionDuration = 0.5f;
		instance.Transition(1f, Vector2.get_one(), 0f, 0.7f, 0f);
	}

	public static void SuperMasterFade(float target, float duration)
	{
		instance.superMasterFade.Transition(target, duration);
	}

	public static void FadeToBlack(float duration)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		instance.transitionDuration = duration;
		instance.Transition(instance.zoom.to, new Vector2(instance.offsetX.to, instance.offsetY.to), 1f, 1f, 1f);
	}

	public static void FadeFromBlack(float duration)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		instance.transitionDuration = duration;
		instance.Transition(instance.zoom.to, new Vector2(instance.offsetX.to, instance.offsetY.to), 0f, 0.7f, 0f);
	}

	public static void FadeInWebcamMenu()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		instance.transitionDuration = 0.5f;
		instance.Transition(1.5f, new Vector2(0f, 1f), 0f, 1f, 0f);
	}

	public static void FadeInCredits()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		instance.transitionDuration = 1.5f;
		instance.Transition(1f, Vector2.get_zero(), 0f, 0f, 0f, 1f);
	}

	public static void FadeInPauseMenu()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		CaveRender.fogDensityMultiplier = 1f;
		instance.Transition(1.5f, Vector2.get_one(), 1f, 1f, 0.5f);
	}

	public static void FadeInLobby()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		instance.Transition(1.5f, Vector2.get_one(), 0f, 1f, 0.75f);
	}

	public static void FadeInCoopMenu()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		instance.Transition(1.5f, Vector2.get_zero(), 1f, 1f, 0.5f);
	}

	public static void FadeOut(float duration = 0f)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (duration == 0f)
		{
			duration = 0.5f;
		}
		instance.transitionDuration = duration;
		instance.Transition(1f, Vector2.get_zero(), 0f, 0f, 0f);
	}

	private void LateUpdate()
	{
		Step(Time.get_unscaledDeltaTime());
	}

	private void Step(float time)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		if (DialogOverlay.IsOn())
		{
			if (superMasterFade.to != superMasterFade.from)
			{
				if (superMasterFade.from > superMasterFade.to)
				{
					superMasterFade.time = 0f;
				}
				else
				{
					superMasterFade.time = superMasterFade.duration;
				}
			}
			flag = superMasterFade.Step(0f);
		}
		else
		{
			flag = superMasterFade.Step(time);
		}
		offsetX.Step(time);
		offsetY.Step(time);
		cameraCenter = new Vector2(-0.23f * offsetX.current, 0.23f * offsetY.current);
		zoom.Step(time);
		cameraZoom = zoom.current;
		if (blur.Step(time) || flag)
		{
			ApplyBlur();
		}
		if ((vignette.Step(time) | dim.Step(time)) || flag)
		{
			ApplyVignette();
		}
		if ((Object)(object)RenderSettings.get_skybox() == (Object)(object)defaultSkyboxMaterial)
		{
			RenderSettings.set_skybox(skyboxMaterial);
		}
		if (credits.Step(time))
		{
			creditsAdjust = credits.current;
			skyboxMaterial.SetColor("_Tint", Color.Lerp(defaultSkyboxColor, creditsSkyboxColor, creditsAdjust));
			creditsPostProcessVolume.weight = instance.creditsAdjust;
			((Component)creditsPostProcessVolume).get_gameObject().SetActive(creditsAdjust != 0f);
		}
	}

	public static void EnableEffects(int antialiasing, int occlusion, bool enableHDR, bool enableExposure, bool enableBloom, bool enableDepthOfField, bool enableChromaticAberration)
	{
		if (!((Object)(object)instance == (Object)null))
		{
			instance.antialiasing = (AntialiasType)antialiasing;
			instance.occlusion = (AmbientOcclusionMode)occlusion;
			instance.allowHDR = enableHDR;
			instance.allowExposure = enableExposure;
			instance.allowBloom = enableBloom;
			instance.allowDepthOfField = enableDepthOfField;
			instance.allowChromaticAberration = enableChromaticAberration;
			ApplyEffects();
		}
	}

	private void ApplyEffect<T>(bool enable) where T : PostProcessEffectSettings
	{
		for (int i = 0; i < profiles.Length; i++)
		{
			if (profiles[i].TryGetSettings<T>(out var outSetting))
			{
				outSetting.enabled.Override(enable);
			}
		}
	}

	public void ForceDisableOcclusion(bool forceDisableOcclusion)
	{
		ApplyOcclusion((!forceDisableOcclusion) ? occlusion : AmbientOcclusionMode.None);
	}

	private void ApplyOcclusion(AmbientOcclusionMode occlusion)
	{
		for (int i = 0; i < profiles.Length; i++)
		{
			if (profiles[i].TryGetSettings<AmbientOcclusion>(out var outSetting))
			{
				outSetting.enabled.Override(occlusion != AmbientOcclusionMode.None);
				outSetting.mode.Override((occlusion == AmbientOcclusionMode.MultiScaleVolumetricObscurance) ? UnityEngine.Rendering.PostProcessing.AmbientOcclusionMode.MultiScaleVolumetricObscurance : UnityEngine.Rendering.PostProcessing.AmbientOcclusionMode.ScalableAmbientObscurance);
			}
		}
	}

	public static void SuspendEffects(bool suspend)
	{
		instance.ApplyOcclusion((!suspend) ? instance.occlusion : AmbientOcclusionMode.None);
		instance.ApplyEffect<AutoExposure>(instance.allowExposure && !suspend);
		instance.ApplyEffect<DepthOfField>(instance.allowDepthOfField && !instance.blockBlur && !suspend);
		instance.ApplyEffect<Bloom>(instance.allowBloom && !suspend);
		instance.ApplyEffect<ChromaticAberration>(instance.allowChromaticAberration && !suspend);
		instance.ApplyEffect<ColorGrading>(!suspend);
		instance.ApplyEffect<Vignette>(!suspend);
		for (int i = 0; i < instance.activeCameras.Count; i++)
		{
			if ((Object)(object)instance.activeCameras[i].depthOfField != (Object)null)
			{
				instance.activeCameras[i].depthOfField.enabled.Override(instance.allowDepthOfField && !instance.blockBlur && !suspend);
			}
		}
		if (instance.postProfile.TryGetSettings<ColorGrading>(out var outSetting))
		{
			outSetting.enabled.Override(!suspend);
		}
		for (int j = 0; j < instance.activeCameras.Count; j++)
		{
			instance.activeCameras[j].camera.set_depthTextureMode((DepthTextureMode)0);
		}
	}

	private static void ApplyEffects()
	{
		SuspendEffects(suspend: false);
		instance.ApplyBlur();
		instance.ApplyVignette();
		for (int i = 0; i < instance.activeCameras.Count; i++)
		{
			GameCamera gameCamera = instance.activeCameras[i];
			gameCamera.camera.set_allowHDR(instance.allowHDR);
			gameCamera.camera.set_allowMSAA(instance.antialiasing == AntialiasType.MSAA);
			gameCamera.layer.antialiasingMode = (PostProcessLayer.Antialiasing)((instance.antialiasing != AntialiasType.MSAA) ? instance.antialiasing : AntialiasType.None);
		}
	}

	private void ApplyVignette()
	{
		for (int i = 0; i < profiles.Length; i++)
		{
			if (profiles[i].TryGetSettings<Vignette>(out var outSetting))
			{
				float x = 1f - (1f - dim.current) * (1f - superMasterFade.current);
				outSetting.opacity.Override(x);
				x = 1f - (1f - vignette.current) * (1f - superMasterFade.current);
				outSetting.intensity.Override(x / 3f);
			}
		}
	}

	private void ApplyBlur()
	{
		for (int i = 0; i < activeCameras.Count; i++)
		{
			if (activeCameras[i] != null && !((Object)(object)activeCameras[i].blur == (Object)null))
			{
				if (blur.current == 0f)
				{
					((Behaviour)activeCameras[i].blur).set_enabled(false);
					continue;
				}
				((Behaviour)activeCameras[i].blur).set_enabled(true);
				float current = blur.current;
				current = 1f - (1f - current) * (1f - superMasterFade.current);
				activeCameras[i].blur.blurSize = current * current * 10f;
				activeCameras[i].blur.downsample = ((!(current > 0.5f)) ? 1 : 2);
			}
		}
	}

	public static void SetGamma(float gamma)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)instance == (Object)null) && instance.postProfile.TryGetSettings<ColorGrading>(out var outSetting))
		{
			outSetting.postExposure.Override(Mathf.Lerp(-1f, 1f, gamma));
			gamma = Mathf.Lerp(-0.5f, 0.5f, gamma);
			outSetting.gamma.Override(new Vector4(0f, 0f, 0f, gamma));
		}
	}

	public void OverrideDepthOfField(Camera cam, float value)
	{
		instance.FindCamera(cam)?.depthOfField.focusDistance.Override(value);
	}

	public MenuCameraEffects()
		: this()
	{
	}//IL_0053: Unknown result type (might be due to invalid IL or missing references)
	//IL_0058: Unknown result type (might be due to invalid IL or missing references)
	//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
	//IL_00fd: Unknown result type (might be due to invalid IL or missing references)

}
