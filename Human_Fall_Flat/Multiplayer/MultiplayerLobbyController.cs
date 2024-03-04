using UnityEngine;

namespace Multiplayer
{
	public class MultiplayerLobbyController : MonoBehaviour
	{
		public Transform playerSpawn;

		public GameCamera gameCamera;

		public static MultiplayerLobbyController instance;

		private bool uiVisible;

		private void Awake()
		{
			Game.instance.BeforeLoad();
		}

		private void Start()
		{
			Game.instance.AfterLoad(0, 0);
		}

		private void OnEnable()
		{
			if ((Object)(object)instance != (Object)null)
			{
				Teardown();
			}
			instance = this;
			uiVisible = true;
			((Component)instance.gameCamera).get_gameObject().SetActive(true);
			MenuCameraEffects.instance.OverrideCamera(((Component)gameCamera).get_transform(), applyEffects: true);
			Dialogs.HideProgress();
		}

		public static void Teardown()
		{
			if ((Object)(object)instance != (Object)null && instance.uiVisible)
			{
				MenuCameraEffects.instance.RemoveOverride();
				((Component)instance.gameCamera).get_gameObject().SetActive(false);
				instance.uiVisible = false;
			}
			instance = null;
			Game.instance.AfterUnload();
		}

		protected void Update()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)instance != (Object)null && !uiVisible && Game.GetKeyDown((KeyCode)27) && MenuSystem.CanInvokeFromGame)
			{
				ShowUI();
			}
			if (!NetGame.isServer)
			{
				return;
			}
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				if (((Component)human).get_transform().get_position().y < -50f)
				{
					((Component)human).get_transform().set_position(Vector3.get_zero());
					human.KillHorizontalVelocity();
					human.MakeUnconscious(1f);
				}
			}
		}

		private void FixedUpdate()
		{
		}

		public void HideUI()
		{
			if (NetGame.instance.local.players.Count >= 1)
			{
				uiVisible = false;
				MenuCameraEffects.instance.RemoveOverride();
				((Component)gameCamera).get_gameObject().SetActive(false);
				MenuSystem.instance.HideMenus();
				NetGame.instance.local.players[0].cameraController.TransitionFrom(gameCamera, 10f, 1f);
			}
		}

		public void ShowUI()
		{
			uiVisible = true;
			((Component)gameCamera).get_gameObject().SetActive(true);
			MenuCameraEffects.instance.OverrideCamera(((Component)gameCamera).get_transform(), applyEffects: true);
			MenuSystem.instance.FadeInForward(MenuSystem.instance.GetMenu<MultiplayerLobbyMenu>());
			MenuSystem.instance.EnterMenuInputMode();
			MenuSystem.instance.state = MenuSystemState.MainMenu;
			SubtitleManager.instance.Hide();
		}

		public MultiplayerLobbyController()
			: this()
		{
		}
	}
}
