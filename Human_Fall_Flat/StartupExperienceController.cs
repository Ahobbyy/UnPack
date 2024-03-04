using System.Collections;
using Multiplayer;
using UnityEngine;

public class StartupExperienceController : MonoBehaviour
{
	private enum HumanState
	{
		HoldBeforeFall,
		Fall,
		ReleaseLatch
	}

	public IntroDrones drones;

	public static StartupExperienceController instance;

	private MenuCameraEffects cameraEffects;

	private CameraController3 cameraController;

	private Human human;

	private Ragdoll ragdoll;

	private HumanState humanState;

	public GameObject gamePrefab;

	private float offsetTime;

	private Vector3 offset;

	private bool syncCamera;

	public void Awake()
	{
		instance = this;
		if ((Object)(object)Game.instance == (Object)null)
		{
			Object.Instantiate<GameObject>(gamePrefab);
		}
	}

	private IEnumerator Start()
	{
		yield return null;
		yield return null;
		yield return null;
		Dependencies.Initialize<App>();
		App.instance.BeginStartup();
	}

	public void PlayStartupExperience()
	{
		((MonoBehaviour)this).StartCoroutine(StartupExperienceRoutine());
	}

	public void SkipStartupExperience(object multiplayerServer)
	{
		cameraEffects = Object.FindObjectOfType<MenuCameraEffects>();
		cameraController = Object.FindObjectOfType<CameraController3>();
		human = Object.FindObjectOfType<Human>();
		ragdoll = human.ragdoll;
		if ((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject() != (Object)null)
		{
			((Component)StartupExperienceUI.instance).get_gameObject().SetActive(false);
		}
		Object.Destroy((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject());
		LeaveGameStartupXP();
		DestroyStartupStuff();
		MenuSystem.instance.ShowMainMenu(hideLogo: true);
		if (multiplayerServer != null)
		{
			App.instance.AcceptInvite(multiplayerServer);
		}
	}

	private IEnumerator StartupExperienceRoutine()
	{
		cameraEffects = Object.FindObjectOfType<MenuCameraEffects>();
		cameraController = Object.FindObjectOfType<CameraController3>();
		human = Human.all[0];
		ragdoll = human.ragdoll;
		StartupExperienceUI startupUI = StartupExperienceUI.instance;
		MenuSystem.instance.EnterMenuInputMode();
		EnterGameStartupXP();
		Begin();
		drones.Play();
		while (drones.dronesTime < 1f)
		{
			yield return null;
		}
		FadeOutDim();
		while (drones.dronesTime < 0f + offsetTime)
		{
			yield return null;
		}
		startupUI.curveLogo.Play("CurveIn");
		while (drones.dronesTime < 4f + offsetTime)
		{
			yield return null;
		}
		startupUI.curveLogo.Play("CurveOut");
		while ((double)drones.dronesTime < 6.01 + (double)offsetTime)
		{
			yield return null;
		}
		((Component)startupUI.noBrakesLogo).get_gameObject().SetActive(true);
		startupUI.noBrakesLogo.Play("NbgLogoIn");
		while ((double)drones.dronesTime < 9.12 + (double)offsetTime)
		{
			yield return null;
		}
		startupUI.noBrakesLogo.Play("NbgLogoOut");
		while ((double)drones.dronesTime < 10.5 + (double)offsetTime)
		{
			yield return null;
		}
		((Component)startupUI.noBrakesLogo).get_gameObject().SetActive(false);
		((Component)startupUI.humanLogo).get_gameObject().SetActive(true);
		startupUI.humanLogo.Play("HumanLogoIn");
		startupUI.gameByLine1.Play("GameByInOut");
		startupUI.gameByLine2.Play("GameByInOut");
		yield return (object)new WaitForSeconds(1f);
		DropHuman();
		bool pressAnythingVisible = false;
		while ((double)drones.dronesTime < 13.2 + (double)offsetTime && !IsAnythingPressed())
		{
			yield return null;
		}
		if (!IsAnythingPressed())
		{
			pressAnythingVisible = true;
			startupUI.pressAnything.Play("PressAnythingIn");
			while (!IsAnythingPressed())
			{
				yield return null;
			}
		}
		ReleaseLatch();
		startupUI.humanLogo.Play("HumanLogoOut");
		if (pressAnythingVisible)
		{
			startupUI.pressAnything.Play("PressAnythingOut");
		}
	}

	private bool IsAnythingPressed()
	{
		return false;
	}

	private void EnterGameStartupXP()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		MenuSystem.instance.ExitMenuInputMode();
		if ((Object)(object)human != (Object)null)
		{
			human.disableInput = true;
			((Behaviour)((Component)human).GetComponent<HumanMotion2>()).set_enabled(false);
			ragdoll.AllowHandBallRotation(allow: false);
			((Behaviour)cameraController).set_enabled(false);
			((Component)cameraController).get_transform().set_position(new Vector3(-0.5f, 4f, 0f));
		}
	}

	public void LeaveGameStartupXP()
	{
		if ((Object)(object)human != (Object)null)
		{
			human.disableInput = false;
			((Behaviour)((Component)human).GetComponent<HumanMotion2>()).set_enabled(true);
			ragdoll.AllowHandBallRotation(allow: true);
			((Behaviour)cameraController).set_enabled(true);
			cameraController.TransitionFromCurrent(3f);
		}
	}

	public void DestroyStartupStuff()
	{
		Object.Destroy((Object)(object)((Component)this).get_gameObject());
	}

	public void Begin()
	{
		cameraEffects.BlackOut();
	}

	public void FadeOutDim()
	{
		cameraEffects.FadeOutBlackOut();
	}

	public void DropHuman()
	{
		humanState = HumanState.Fall;
	}

	public void ReleaseLatch()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		humanState = HumanState.ReleaseLatch;
		((Component)this).GetComponent<StartupExperienceGeometry>().ReleaseDoor();
		syncCamera = true;
		offset = ((Component)cameraController).get_transform().get_position() - ((Component)human).get_transform().get_position();
	}

	private void Update()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)human == (Object)null))
		{
			if (humanState == HumanState.HoldBeforeFall)
			{
				((Component)human).get_transform().set_position(((Component)this).get_transform().get_position().SetY(20f));
				human.ControlVelocity(15f, killHorizontal: true);
				ragdoll.StretchHandsLegs(Vector3.get_forward(), Vector3.get_right(), 50);
			}
			else if (humanState == HumanState.Fall && ((Component)human).get_transform().get_position().y > 2f)
			{
				((Component)human).get_transform().set_position(((Component)this).get_transform().get_position().SetY(((Component)human).get_transform().get_position().y));
				human.ControlVelocity(15f, killHorizontal: true);
				ragdoll.StretchHandsLegs(2f * Vector3.get_forward() + Vector3.get_right(), 2f * Vector3.get_right() - Vector3.get_forward(), 10);
			}
		}
	}

	private void LateUpdate()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (syncCamera)
		{
			if (((Component)cameraController).get_transform().get_position().y > -2f)
			{
				((Component)cameraController).get_transform().set_position(((Component)human).get_transform().get_position() + offset);
				return;
			}
			syncCamera = false;
			LeaveGameStartupXP();
			DestroyStartupStuff();
			App.instance.StartupFinished();
		}
	}

	public StartupExperienceController()
		: this()
	{
	}
}
