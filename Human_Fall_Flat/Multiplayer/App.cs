using System;
using System.Collections;
using System.IO;
using HumanAPI;
using I2.Loc;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Multiplayer
{
	public class App : MonoBehaviour, IDependency
	{
		public const string kStartupScene = "Startup";

		public const string kCustomisationScene = "Customization";

		public static object stateLock = new object();

		private static AppSate _state = AppSate.Startup;

		public static App instance;

		private AssetBundle lobbyAssetbundle;

		private ulong previousLobbyID;

		private const float kCancelConnectDelay = 2f;

		private Action queueAfterLevelLoad;

		private ulong loadingLevel = 57005uL;

		private ulong loadedLevel = 57005uL;

		private uint loadedHash = 57005u;

		private ulong serverLoadedLevel = 57005uL;

		private uint serverLoadedHash = 57005u;

		public int startedCheckpoint;

		public static AppSate state
		{
			get
			{
				lock (stateLock)
				{
					return _state;
				}
			}
			set
			{
				if (NetGame.netlog)
				{
					Debug.Log((object)value);
				}
				lock (stateLock)
				{
					_state = value;
				}
			}
		}

		public static bool isServer
		{
			get
			{
				lock (stateLock)
				{
					return _state >= AppSate.ServerHost && _state <= AppSate.ServerPlayLevel;
				}
			}
		}

		public static bool isClient
		{
			get
			{
				lock (stateLock)
				{
					return _state >= AppSate.ClientJoin && _state <= AppSate.ClientPlayLevel;
				}
			}
		}

		private void AssertState(int where = -1)
		{
			Debug.LogErrorFormat("Wrong state {0} (site={1})", new object[2] { state, where });
		}

		private void WarnState(int where = -1)
		{
			Debug.LogWarningFormat("Wrong state {0} (site={1})", new object[2] { state, where });
		}

		public void Awake()
		{
			instance = this;
		}

		public void Initialize()
		{
			if (state != 0)
			{
				AssertState(1100);
				return;
			}
			instance = this;
			GameSave.Initialize();
			Dependencies.Initialize<StartupExperienceUI>();
			Dependencies.Initialize<NetGame>();
			Dependencies.Initialize<WorkshopRepository>();
			Dependencies.Initialize<RagdollTemplate>();
			Dependencies.Initialize<Game>();
			Dependencies.Initialize<GiftService>();
			Dependencies.OnInitialized(this);
			NetGame.instance.StartLocalGame();
			Options.Load();
			HumanAnalytics.instance.LogVersion(VersionDisplay.fullVersion);
		}

		public void Update()
		{
		}

		public void OnRelayConnection(NetHost client)
		{
			NetChat.Print(string.Format(ScriptLocalization.MULTIPLAYER.Relayed, client.name));
		}

		public void BeginStartup()
		{
			if (state != 0)
			{
				AssertState(1101);
				return;
			}
			object obj = NetGame.instance.transport.FetchLaunchInvitation();
			if (obj != null)
			{
				StartupExperienceController.instance.SkipStartupExperience(obj);
			}
			else
			{
				StartupExperienceController.instance.PlayStartupExperience();
			}
		}

		public void StartupFinished()
		{
			lock (stateLock)
			{
				if (state != 0)
				{
					AssertState(1102);
					return;
				}
				EnterMenu();
				MenuSystem.instance.ShowMainMenu(hideLogo: true);
			}
		}

		public void BeginMenuDev()
		{
			lock (stateLock)
			{
				if (state != 0)
				{
					AssertState(1103);
					return;
				}
				EnterMenu();
				((Component)StartupExperienceUI.instance).get_gameObject().SetActive(false);
			}
		}

		public void BeginLevel()
		{
			lock (stateLock)
			{
				if (state != 0)
				{
					AssertState(1104);
					return;
				}
				EnterMenu();
				((Component)StartupExperienceUI.instance).get_gameObject().SetActive(false);
				LaunchSinglePlayer((ulong)Game.instance.currentLevelNumber, Game.instance.currentLevelType, 0, 0);
			}
		}

		private void EnterMenu()
		{
			lock (stateLock)
			{
				if (state != AppSate.Menu)
				{
					if (state == AppSate.ClientLobby || state == AppSate.ServerLobby)
					{
						ExitLobby();
					}
					if (state == AppSate.Customize || state == AppSate.PlayLevel || state == AppSate.ClientLobby || state == AppSate.ServerLobby)
					{
						Game.instance.HasSceneLoaded = false;
						SceneManager.LoadSceneAsync("Empty");
					}
					else if (state != 0)
					{
						Game.instance.HasSceneLoaded = false;
						SceneManager.LoadScene("Empty");
					}
					Game.instance.state = GameState.Inactive;
					state = AppSate.Menu;
				}
			}
		}

		private void EnterLobby(bool isServer, Action callback = null)
		{
			if (!RatingMenu.instance.ShowRatingMenu())
			{
				MenuSystem.instance.HideMenus();
			}
			((MonoBehaviour)this).StartCoroutine(EnterLobbyAsync(isServer, callback));
		}

		private IEnumerator EnterLobbyAsync(bool asServer, Action callback = null)
		{
			NetScope.ClearAllButPlayers();
			lock (stateLock)
			{
				state = (asServer ? AppSate.ServerLoadLobby : AppSate.ClientLoadLobby);
				SuspendDeltasForLoad();
				Game.instance.HasSceneLoaded = false;
				string sceneName = null;
				if (Game.multiplayerLobbyLevel >= 128)
				{
					bool loaded = false;
					WorkshopLevelMetadata workshopLevel = null;
					WorkshopRepository.instance.levelRepo.LoadLevel(Game.multiplayerLobbyLevel, delegate(WorkshopLevelMetadata l)
					{
						workshopLevel = l;
						loaded = true;
					});
					while (!loaded)
					{
						yield return null;
					}
					if (workshopLevel != null)
					{
						lobbyAssetbundle = FileTools.LoadBundle(workshopLevel.dataPath);
						string[] allScenePaths = lobbyAssetbundle.GetAllScenePaths();
						sceneName = Path.GetFileNameWithoutExtension(allScenePaths[0]);
						StopPlaytimeForItem(previousLobbyID);
						StartPlaytimeForItem(workshopLevel.workshopId);
						previousLobbyID = workshopLevel.workshopId;
						goto IL_01c3;
					}
					if (!NetGame.isServer)
					{
						SubtitleManager.instance.ClearProgress();
						Debug.Log((object)"Level load failed.");
						instance.ServerFailedToLoad();
						SignalManager.EndReset();
						yield break;
					}
					goto IL_01c3;
				}
				sceneName = WorkshopRepository.GetLobbyFilename(Game.multiplayerLobbyLevel);
				goto IL_01c3;
				IL_01c3:
				if (string.IsNullOrEmpty(sceneName))
				{
					sceneName = WorkshopRepository.GetLobbyFilename(0uL);
					Game.multiplayerLobbyLevel = 0uL;
				}
				AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName);
				if (loader != null)
				{
					while (!loader.get_isDone() || !Game.instance.HasSceneLoaded)
					{
						yield return null;
					}
				}
				if (state != AppSate.ServerLoadLobby && state != AppSate.ClientLoadLobby)
				{
					Debug.Log((object)("Exiting wrong app state (" + state.ToString() + ")"));
				}
				state = (asServer ? AppSate.ServerLobby : AppSate.ClientLobby);
				ResumeDeltasAfterLoad();
				if (!RatingMenu.instance.ShowRatingMenu())
				{
					MenuSystem.instance.ShowMainMenu<MultiplayerLobbyMenu>();
				}
				Game.instance.state = GameState.Inactive;
				UpdateJoinable();
				callback?.Invoke();
				if (queueAfterLevelLoad != null)
				{
					Action action = queueAfterLevelLoad;
					queueAfterLevelLoad = null;
					if (NetGame.netlog)
					{
						Debug.Log((object)"Executing queue");
					}
					action();
				}
				if ((Object)(object)lobbyAssetbundle != (Object)null)
				{
					lobbyAssetbundle.Unload(false);
					lobbyAssetbundle = null;
				}
				Game.instance.FixAssetBundleImport(lobby: true);
			}
		}

		private void ExitLobby(bool startingGame = false)
		{
			lock (stateLock)
			{
				UpdateJoinable(startingGame);
				MultiplayerLobbyController.Teardown();
			}
		}

		public void OnClientCountChanged()
		{
			UpdateJoinable();
		}

		private void UpdateJoinable(bool startingGame = false)
		{
			if (!NetGame.isServer)
			{
				return;
			}
			NetTransport transport = NetGame.instance.transport;
			if (NetGame.instance.players.Count < Options.lobbyMaxPlayers)
			{
				Debug.Log((object)"UpdateJoinable : hasSlots");
				if (state == AppSate.ServerLobby && !startingGame)
				{
					Debug.Log((object)"UpdateJoinable : in lobby");
					transport.SetJoinable(joinable: true, haveStarted: false);
				}
				else
				{
					Debug.Log((object)("UpdateJoinable : join in progress = " + Options.lobbyJoinInProgress));
					transport.SetJoinable(Options.lobbyJoinInProgress != 0, haveStarted: true);
				}
			}
			else
			{
				Debug.Log((object)"UpdateJoinable : noSlots");
				transport.SetJoinable(joinable: false, state != AppSate.ServerLobby || startingGame);
			}
		}

		public void HostGame(object sessionArgs = null)
		{
			lock (stateLock)
			{
				if (state != AppSate.Menu)
				{
					AssertState(1105);
					return;
				}
				state = AppSate.ServerHost;
				Dialogs.ShowHostServerProgress(CancelHost);
				NetGame.instance.HostGame(sessionArgs);
			}
		}

		public void OnHostGameSuccess()
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerHost)
				{
					AssertState();
					return;
				}
				Dialogs.ShowLoadLevelProgress(57005uL);
				EnterLobby(isServer: true, delegate
				{
					NetChat.PrintHelp();
				});
			}
		}

		public void OnHostGameFail(string error)
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerHost)
				{
					AssertState(1107);
					return;
				}
				Dialogs.HideProgress(null, 2);
				EnterMenu();
				if (error != null && error.Equals("!"))
				{
					MenuSystem.instance.ShowMenu<MainMenu>(hideOldMenu: true);
					return;
				}
				Debug.LogError((object)("OnHostGameFail Error = " + (string.IsNullOrEmpty(error) ? "null" : error)));
				Dialogs.CreateServerFailed(error, delegate
				{
					MenuSystem.instance.ShowMenu<MainMenu>();
				}, hideOldMenu: true);
			}
		}

		public void CancelHost()
		{
			lock (stateLock)
			{
				EnterMenu();
				MenuSystem.instance.ShowMainMenu(hideLogo: false, hideOldMenu: true);
				NetGame.instance.StopServer();
				Game.instance.state = GameState.Inactive;
			}
		}

		public void StopServer()
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerLobby)
				{
					AssertState(1109);
					return;
				}
				EnterMenu();
				if ((Object)(object)MenuSystem.instance.activeMenu == (Object)null)
				{
					MenuSystem.instance.ShowMainMenu();
				}
				else
				{
					MenuSystem.instance.activeMenu.TransitionBack<MainMenu>();
				}
				NetGame.instance.StopServer();
				Game.instance.state = GameState.Inactive;
			}
		}

		public void LeaveLobby()
		{
			lock (stateLock)
			{
				if (state != AppSate.ClientLobby)
				{
					WarnState(1200);
					return;
				}
				EnterMenu();
				NetGame.instance.LeaveGame();
				if ((Object)(object)MenuSystem.instance.activeMenu != (Object)null)
				{
					MenuSystem.instance.activeMenu.TransitionBack<MainMenu>();
				}
				else
				{
					MenuSystem.instance.ShowMenu<MainMenu>();
				}
				Game.instance.state = GameState.Inactive;
			}
		}

		public void ChangeLobbyLevel(ulong level, WorkshopItemSource levelType)
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerLobby)
				{
					AssertState(1110);
					return;
				}
				NetGame.instance.ServerLoadLevel(level, levelType, start: false, 0u);
				if (MenuSystem.instance.activeMenu is MultiplayerLobbyMenu)
				{
					(MenuSystem.instance.activeMenu as MultiplayerLobbyMenu).RebindLevel();
				}
			}
		}

		public void AcceptInvite(object server)
		{
			lock (stateLock)
			{
				if (state == AppSate.LoadLevel || state == AppSate.ClientLoadLevel || state == AppSate.ServerLoadLevel || state == AppSate.ClientLoadLobby || state == AppSate.ServerLoadLobby)
				{
					if (NetGame.netlog)
					{
						Debug.Log((object)"Queueing accept invite");
					}
					queueAfterLevelLoad = delegate
					{
						AcceptInvite(server);
					};
					return;
				}
				if (state == AppSate.PlayLevel || state == AppSate.ClientPlayLevel || state == AppSate.ServerPlayLevel || state == AppSate.ClientWaitServerLoad)
				{
					PauseLeave(instantLeave: true);
				}
				if (isClient)
				{
					LeaveLobby();
				}
				else if (isServer)
				{
					StopServer();
				}
				else if (state == AppSate.Customize)
				{
					LeaveCustomization();
				}
				if (server == null)
				{
					return;
				}
				if (state == AppSate.Startup)
				{
					if ((Object)(object)StartupExperienceController.instance != (Object)null)
					{
						StartupExperienceController.instance.LeaveGameStartupXP();
						StartupExperienceController.instance.DestroyStartupStuff();
					}
					if ((Object)(object)StartupExperienceUI.instance != (Object)null)
					{
						if ((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject() != (Object)null)
						{
							((Component)StartupExperienceUI.instance).get_gameObject().SetActive(false);
						}
						Object.Destroy((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject());
					}
					state = AppSate.Menu;
				}
				JoinGame(server);
			}
		}

		public void JoinGame(object server)
		{
			lock (stateLock)
			{
				if (state != AppSate.Menu)
				{
					AssertState(1111);
					return;
				}
				state = AppSate.ClientJoin;
				loadingLevel = 57005uL;
				loadedLevel = 57005uL;
				serverLoadedLevel = 57005uL;
				queueAfterLevelLoad = null;
				Dialogs.ShowJoinGameProgress(CancelConnect);
				NetGame.instance.JoinGame(server);
			}
		}

		public void OnConnectFail(string error)
		{
			lock (stateLock)
			{
				if (state != AppSate.ClientJoin)
				{
					WarnState(1201);
					return;
				}
				EnterMenu();
				if (error != null && error.Equals("!"))
				{
					MenuSystem.instance.ShowMenu<MainMenu>(hideOldMenu: true);
					return;
				}
				if (error != null && error.Equals("&"))
				{
					MenuSystem.instance.ShowMenu<MultiplayerSelectLobbyMenu>(hideOldMenu: true);
					return;
				}
				Debug.LogError((object)error);
				Dialogs.ConnectionFailed(error, delegate
				{
					MenuSystem.instance.ShowMenu<MainMenu>();
				}, hideOldMenu: true);
			}
		}

		public void OnConnectFailVersion(string serverVersion)
		{
			lock (stateLock)
			{
				if (state != AppSate.ClientJoin)
				{
					WarnState(1202);
					return;
				}
				EnterMenu();
				Debug.LogError((object)serverVersion);
				Dialogs.ConnectionFailedVersion(serverVersion, delegate
				{
					MenuSystem.instance.ShowMenu<MainMenu>();
				}, hideOldMenu: true);
			}
		}

		private void FinishCancelConnect()
		{
			MenuSystem.instance.ShowMainMenu(hideLogo: false, hideOldMenu: true);
		}

		private IEnumerator DelayedCancelConnect()
		{
			yield return (object)new WaitForSeconds(2f);
			FinishCancelConnect();
			NetTransportSteam.RemoveOldPackets();
		}

		public void CancelConnect()
		{
			lock (stateLock)
			{
				if (state != AppSate.ClientJoin)
				{
					WarnState(1203);
					return;
				}
				NetGame.instance.LeaveGame();
				EnterMenu();
				MenuSystem.instance.HideMenus();
				((MonoBehaviour)this).StartCoroutine(DelayedCancelConnect());
			}
		}

		public void CancelConnectPhase2()
		{
			CancelConnect();
		}

		public void OnLostConnection(bool suppressMessage = false)
		{
			lock (stateLock)
			{
				if (state == AppSate.LoadLevel || state == AppSate.ClientLoadLevel || state == AppSate.ServerLoadLevel || state == AppSate.ClientLoadLobby || state == AppSate.ServerLoadLobby)
				{
					if (NetGame.netlog)
					{
						Debug.Log((object)"Queueing lostConnection");
					}
					queueAfterLevelLoad = delegate
					{
						OnLostConnection(suppressMessage);
					};
					return;
				}
				RatingMenu.instance.LevelOver();
				MultiplayerSelectLobbyMenu multiplayerSelectLobbyMenu = MenuSystem.instance.activeMenu as MultiplayerSelectLobbyMenu;
				if ((Object)(object)multiplayerSelectLobbyMenu != (Object)null)
				{
					multiplayerSelectLobbyMenu.OnBack();
				}
				else
				{
					NetGame.instance.LeaveGame();
					if (state == AppSate.PlayLevel || state == AppSate.ClientPlayLevel || state == AppSate.ServerPlayLevel || state == AppSate.ClientWaitServerLoad)
					{
						ExitGame();
					}
					EnterMenu();
				}
				if (RatingMenu.instance.ShowRatingMenu())
				{
					RatingMenu.RatingDestination destination = RatingMenu.RatingDestination.kMainMenu;
					if (!suppressMessage)
					{
						destination = RatingMenu.RatingDestination.kLostConnection;
					}
					GotoRatingsMenu(destination, showLoading: false);
					return;
				}
				NetGame.isNetStarting = false;
				if (suppressMessage)
				{
					MenuSystem.instance.ShowMainMenu();
					return;
				}
				Dialogs.ConnectionLost(delegate
				{
					MenuSystem.instance.ShowMainMenu();
				});
			}
		}

		public void KillGame()
		{
			NetGame.instance.LeaveGame();
			ExitGame();
			EnterMenu();
		}

		public void ServerFailedToLoad()
		{
			KillGame();
			Dialogs.FailedToLoad(delegate
			{
				MenuSystem.instance.ShowMainMenu();
			});
		}

		public void ServerKicked()
		{
			RatingMenu.instance.LevelOver();
			KillGame();
			if (RatingMenu.instance.ShowRatingMenu())
			{
				GotoRatingsMenu(RatingMenu.RatingDestination.kLostConnection, showLoading: false);
				return;
			}
			Dialogs.ConnectionKicked(delegate
			{
				MenuSystem.instance.ShowMainMenu();
			});
		}

		public void ServerLostConnection(bool suppressMessage = false)
		{
			NetGame.isNetStarting = false;
			OnLostConnection(suppressMessage);
		}

		public void SuspendDeltasForLoad()
		{
			NetGame.instance.ignoreDeltasMask |= 2u;
		}

		public void ResumeDeltasAfterLoad()
		{
			NetGame.instance.ignoreDeltasMask &= 4294967293u;
			if (NetGame.isServer)
			{
				NetGame.currentLevelInstanceID = NetGame.nextLevelInstanceID;
			}
		}

		public void OnRequestLoadLevel(ulong level, WorkshopItemSource levelType, bool start, uint hash)
		{
			if (start && loadingLevel != level)
			{
				RatingMenu.instance.LoadInit();
			}
			if (NetGame.netlog)
			{
				Debug.LogFormat("On RequestLoad {0} {1} {2} {3}", new object[4] { level, levelType, start, hash });
			}
			lock (stateLock)
			{
				if (state == AppSate.ClientLoadLevel || state == AppSate.ClientLoadLobby)
				{
					if (NetGame.netlog)
					{
						Debug.Log((object)"Queueing on request load");
					}
					queueAfterLevelLoad = delegate
					{
						OnRequestLoadLevel(level, levelType, start, hash);
					};
					return;
				}
				queueAfterLevelLoad = null;
				if (hash == 0)
				{
					serverLoadedLevel = 57005uL;
				}
				if (!start)
				{
					loadingLevel = (loadedLevel = 57005uL);
					loadedHash = 0u;
					queueAfterLevelLoad = null;
					if (state == AppSate.ClientLobby)
					{
						MultiplayerLobbyMenu multiplayerLobbyMenu = MenuSystem.instance.activeMenu as MultiplayerLobbyMenu;
						if ((Object)(object)multiplayerLobbyMenu != (Object)null)
						{
							multiplayerLobbyMenu.RebindLevel();
						}
						state = AppSate.ClientLobby;
					}
					else if (isClient)
					{
						RatingMenu.instance.LevelOver();
						if (Game.instance.state == GameState.Paused || Game.instance.state == GameState.PlayingLevel)
						{
							ExitGame();
						}
						if (RatingMenu.instance.ShowRatingMenu())
						{
							GotoRatingsMenu(RatingMenu.RatingDestination.kMultiplayerLobby, showLoading: true);
						}
						else
						{
							MenuCameraEffects.FadeToBlack(0.02f);
							Dialogs.ShowLoadLevelProgress(57005uL);
						}
						EnterLobby(isServer: false, delegate
						{
							state = AppSate.ClientLobby;
						});
					}
					else
					{
						WarnState(1204);
					}
					return;
				}
				if (loadingLevel != level)
				{
					if (state == AppSate.ClientLobby)
					{
						ExitLobby();
					}
					loadingLevel = level;
					LaunchGame(level, levelType, 0, 0, delegate
					{
						loadedLevel = level;
						loadedHash = Game.currentLevel.netHash;
						if (!LevelLoadedClient())
						{
							state = AppSate.ClientWaitServerLoad;
						}
					});
				}
				if (hash != 0)
				{
					serverLoadedLevel = level;
					serverLoadedHash = hash;
					LevelLoadedClient();
				}
			}
		}

		private bool LevelLoadedClient()
		{
			lock (stateLock)
			{
				if (serverLoadedLevel == loadedLevel)
				{
					Dialogs.HideProgress();
					MenuCameraEffects.FadeOut(1f);
					if (loadedHash == serverLoadedHash)
					{
						state = AppSate.ClientPlayLevel;
					}
					else
					{
						NetGame.instance.LeaveGame();
						ExitGame();
						EnterMenu();
						if (serverLoadedHash == 57005)
						{
							Dialogs.ConnectionFailed("LevelNotNetReady", delegate
							{
								MenuSystem.instance.ShowMainMenu();
							});
						}
						else
						{
							Debug.LogError((object)("Incompatible level. Server hash: " + serverLoadedHash + ". Client Hash: " + loadedHash));
							Dialogs.ConnectionFailed("IncompatibleLevel", delegate
							{
								MenuSystem.instance.ShowMainMenu();
							});
						}
						queueAfterLevelLoad = null;
					}
					return true;
				}
				return false;
			}
		}

		public void LaunchSinglePlayer(ulong level, WorkshopItemSource levelType, int checkpoint, int subObjectives)
		{
			startedCheckpoint = checkpoint;
			LaunchGame(level, levelType, checkpoint, subObjectives, delegate
			{
				MenuCameraEffects.FadeOut(1f);
				state = AppSate.PlayLevel;
			});
		}

		public void LaunchCustomLevel(string path, WorkshopItemSource levelType, int checkpoint, int subObjectives)
		{
			((MonoBehaviour)this).StartCoroutine(LaunchGame(path, 0uL, levelType, checkpoint, subObjectives, delegate
			{
				MenuCameraEffects.FadeOut(1f);
				state = AppSate.PlayLevel;
			}));
		}

		private void LaunchGame(ulong level, WorkshopItemSource levelType, int checkpoint, int subObjectives, Action onComplete)
		{
			StopPlaytimeForItem(previousLobbyID);
			previousLobbyID = 0uL;
			MenuSystem.instance.HideMenus();
			CheatCodes.cheatMode = false;
			if (levelType == WorkshopItemSource.BuiltIn && level == 0L && checkpoint == 0)
			{
				Game.instance.singleRun = true;
			}
			SuspendDeltasForLoad();
			((MonoBehaviour)this).StartCoroutine(LaunchGame(null, level, levelType, checkpoint, subObjectives, delegate
			{
				onComplete();
			}));
		}

		private IEnumerator LaunchGame(string levelPath, ulong level, WorkshopItemSource type, int checkpoint, int subObjectives, Action onComplete)
		{
			lock (stateLock)
			{
				if (state != AppSate.Menu && state != AppSate.PlayLevel && state != AppSate.ClientJoin && state != AppSate.ClientLobby && state != AppSate.ClientPlayLevel && state != AppSate.ClientWaitServerLoad && state != AppSate.ServerLobby && state != AppSate.ServerPlayLevel)
				{
					WarnState(1205);
				}
				bool flag = state == AppSate.Menu || state == AppSate.ServerLobby || state == AppSate.ClientLobby || state == AppSate.ClientJoin;
				if (flag)
				{
					MenuSystem.instance.FadeOutActive();
				}
				if (isServer || isClient)
				{
					MenuCameraEffects.FadeToBlack(flag ? 0.2f : 0.02f);
					Dialogs.ShowLoadLevelProgress(level);
				}
				if (isServer)
				{
					state = AppSate.ServerLoadLevel;
				}
				else if (isClient)
				{
					state = AppSate.ClientLoadLevel;
				}
				else
				{
					state = AppSate.LoadLevel;
				}
				queueAfterLevelLoad = null;
				if (flag)
				{
					yield return (object)new WaitForSeconds(0.2f);
				}
				NetStream.DiscardPools();
				Game.instance.BeginLoadLevel(levelPath, level, checkpoint, subObjectives, delegate
				{
					lock (stateLock)
					{
						if (state != AppSate.LoadLevel && state != AppSate.ServerLoadLevel && state != AppSate.ClientLoadLevel)
						{
							WarnState(1206);
						}
						MenuSystem.instance.ExitMenus();
						NetStream.DiscardPools();
						ResumeDeltasAfterLoad();
						if (onComplete != null)
						{
							onComplete();
						}
						if (queueAfterLevelLoad != null)
						{
							Action action = queueAfterLevelLoad;
							queueAfterLevelLoad = null;
							if (NetGame.netlog)
							{
								Debug.Log((object)"Executing queue");
							}
							action();
						}
					}
				}, type);
			}
		}

		public static void StartPlaytimeLocalPlayers()
		{
			foreach (NetPlayer player in NetGame.instance.players)
			{
				if (player.isLocalPlayer && player.skin != null)
				{
					StartPlaytimeForItem(player.skin.workshopId);
				}
			}
		}

		public static void StopPlaytimeForItem(ulong item)
		{
			if (item != 0L)
			{
				SteamUGC.StopPlaytimeTracking(new PublishedFileId_t[1]
				{
					new PublishedFileId_t(item)
				}, 1u);
			}
		}

		public static void StartPlaytimeForItem(ulong item)
		{
			if (item != 0L)
			{
				SteamUGC.StartPlaytimeTracking(new PublishedFileId_t[1]
				{
					new PublishedFileId_t(item)
				}, 1u);
			}
		}

		private void ExitGame()
		{
			Game.instance.singleRun = false;
			SteamUGC.StopPlaytimeTrackingForAllItems();
			Game.instance.UnloadLevel();
		}

		public void StartGameServer(ulong level, WorkshopItemSource levelType)
		{
			startedCheckpoint = 0;
			lock (stateLock)
			{
				if (state != AppSate.ServerLobby)
				{
					AssertState(1112);
					return;
				}
				ExitLobby(startingGame: true);
				NetGame.instance.ServerLoadLevel(level, levelType, start: true, 0u);
				LaunchGame(level, levelType, 0, 0, delegate
				{
					LevelLoadedServer(level, levelType, Game.currentLevel.netHash);
				});
			}
		}

		public void NextLevelServer(ulong level, int checkpoint)
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerPlayLevel)
				{
					AssertState(1113);
					return;
				}
				NetGame.instance.ServerLoadLevel(level, WorkshopItemSource.BuiltIn, start: true, 0u);
				LaunchGame(level, WorkshopItemSource.BuiltIn, checkpoint, 0, delegate
				{
					LevelLoadedServer(level, WorkshopItemSource.BuiltIn, Game.currentLevel.netHash);
				});
			}
		}

		public void LobbyLoadLevel(ulong level)
		{
			MenuCameraEffects.FadeToBlack(0.02f);
			Dialogs.ShowLoadLevelProgress(57005uL);
			Listener.instance.EndTransfromOverride();
			SceneManager.LoadScene("Empty");
			EnterLobby(NetGame.isServer);
			if (NetGame.isServer)
			{
				NetGame.instance.ServerLoadLobby(level, WorkshopItemSource.BuiltInLobbies, start: false, 0u);
			}
		}

		public void LevelLoadedServer(ulong level, WorkshopItemSource levelType, uint hash)
		{
			lock (stateLock)
			{
				if (state != AppSate.ServerLoadLevel)
				{
					AssertState(1114);
					return;
				}
				Dialogs.HideProgress();
				MenuCameraEffects.FadeOut(1f);
				if (hash == 0 || hash == 57005)
				{
					NetGame.instance.StopServer();
					ExitGame();
					EnterMenu();
					Debug.LogError((object)"LevelNotNetReady");
					Dialogs.CreateServerFailed("LevelNotNetReady", delegate
					{
						MenuSystem.instance.ShowMenu<MainMenu>();
					});
				}
				else
				{
					state = AppSate.ServerPlayLevel;
					NetGame.instance.ServerLoadLevel(level, levelType, start: true, (hash == 0) ? 57005u : hash);
				}
			}
		}

		public void StartNextLevel(ulong level, int checkpoint)
		{
			lock (stateLock)
			{
				if (state == AppSate.ServerPlayLevel)
				{
					NextLevelServer(level, checkpoint);
				}
				else
				{
					LaunchSinglePlayer(level, WorkshopItemSource.BuiltIn, checkpoint, 0);
				}
			}
		}

		private void GotoRatingsMenu(RatingMenu.RatingDestination destination, bool showLoading)
		{
			MenuCameraEffects.FadeInPauseMenu();
			RatingMenu.instance.SetDestination(destination);
			if (showLoading)
			{
				SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.LOADING, 1f, 1f);
			}
			MenuSystem.instance.ShowMainMenu<RatingMenu>();
		}

		public void PauseLeave(bool instantLeave = false)
		{
			lock (stateLock)
			{
				RatingMenu.instance.LevelOver();
				bool flag = RatingMenu.instance.ShowRatingMenu();
				bool flag2 = Game.instance.workshopLevel != null;
				ExitGame();
				if (state == AppSate.PlayLevel)
				{
					EnterMenu();
					PlayerManager.SetSingle();
					if (flag2)
					{
						MenuCameraEffects.instance.RemoveOverride();
						LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: false);
						LevelSelectMenu2.instance.ShowSubscribed();
						if (flag)
						{
							GotoRatingsMenu(RatingMenu.RatingDestination.kLevelSelectMenu, showLoading: false);
						}
						else
						{
							MenuSystem.instance.ShowMainMenu<LevelSelectMenu2>();
						}
					}
					else
					{
						MenuSystem.instance.ShowMainMenu();
					}
				}
				else if (state == AppSate.ServerPlayLevel)
				{
					if (!instantLeave)
					{
						if (flag)
						{
							GotoRatingsMenu(RatingMenu.RatingDestination.kMultiplayerLobby, showLoading: true);
						}
						else
						{
							MenuCameraEffects.FadeToBlack(0.02f);
							Dialogs.ShowLoadLevelProgress(57005uL);
						}
						EnterLobby(isServer: true);
						NetGame.instance.ServerLoadLevel(NetGame.instance.currentLevel, NetGame.instance.currentLevelType, start: false, 0u);
					}
					else
					{
						EnterMenu();
						MenuSystem.instance.ShowMainMenu();
						NetGame.instance.LeaveGame();
					}
				}
				else if (state == AppSate.ClientPlayLevel)
				{
					NetGame.instance.LeaveGame();
					EnterMenu();
					if (flag)
					{
						GotoRatingsMenu(RatingMenu.RatingDestination.kMainMenu, showLoading: false);
					}
					else
					{
						MenuSystem.instance.ShowMainMenu();
					}
				}
				else
				{
					AssertState(1116);
				}
			}
		}

		public void EnterCustomization(Action controllerLoaded)
		{
			lock (stateLock)
			{
				state = AppSate.Customize;
				CustomizationController.onInitialized = controllerLoaded;
				SceneManager.LoadSceneAsync("Customization");
			}
		}

		internal void LeaveCustomization()
		{
			lock (stateLock)
			{
				if (MenuSystem.instance.activeMenu is CustomizationPaintMenu)
				{
					MenuSystem.instance.GetMenu<CustomizationPaintMenu>().paint.CancelStroke();
				}
				if (MenuSystem.instance.activeMenu is CustomizationWebcamMenu)
				{
					MenuSystem.instance.GetMenu<CustomizationWebcamMenu>().Teardown();
				}
				EnterMenu();
				if ((Object)(object)CustomizationController.instance != (Object)null)
				{
					CustomizationController.instance.Teardown();
				}
			}
		}

		public void SetLobbyTitle(string lobbyTitle)
		{
			NetGame.instance.lobbyTitle = lobbyTitle;
			if (NetGame.isServer)
			{
				NetGame.instance.transport.UpdateLobbyTitle();
			}
		}

		public string GetLobbyTitle()
		{
			return NetGame.instance.lobbyTitle;
		}

		public void OnApplicationQuit()
		{
			lock (stateLock)
			{
				if (isServer)
				{
					NetGame.instance.StopServer();
				}
				else if (isClient)
				{
					NetGame.instance.LeaveGame();
				}
			}
		}

		public App()
			: this()
		{
		}
	}
}
