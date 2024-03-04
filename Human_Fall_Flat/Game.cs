using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HumanAPI;
using I2.Loc;
using Multiplayer;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour, IGame, IDependency
{
	public StartupExperienceController startupXP;

	public static Game instance;

	public string[] levels;

	public string[] editorPickLevels;

	public int levelCount;

	public int currentLevelNumber = -1;

	public int currentCheckpointNumber;

	public int currentCheckpointSubObjectives;

	public List<int> currentSolvedCheckpoints = new List<int>();

	public WorkshopItemSource currentLevelType;

	public string editorLanguage = "english";

	public int editorStartLevel = 3;

	public int editorStartCheckpoint = 3;

	public bool passedCheckpoint_ForSteelSeriesEvent;

	public Light defaultLight;

	public const int currentBuiltLevels = 13;

	public const int kMaxBuiltInLevels = 16;

	public const int kMaxBuiltInLobbies = 128;

	public static uint currentLevelID;

	public static Level currentLevel;

	public GameState state;

	public bool passedLevel;

	public NetPlayer playerPrefab;

	public Camera cameraPrefab;

	public Ragdoll ragdollPrefab;

	public Material skyboxMaterial;

	[NonSerialized]
	public GameProgress gameProgress;

	[NonSerialized]
	public static ulong multiplayerLobbyLevel;

	[NonSerialized]
	public bool singleRun;

	[NonSerialized]
	public bool HasSceneLoaded;

	public static Action<Human> OnDrowning;

	private AssetBundle bundle;

	public WorkshopLevelMetadata workshopLevel;

	public bool workshopLevelIsCustom;

	private Color skyColor;

	private const string kDefaultMixerIfNull = "Effects";

	private const int kInitialAudioSources = 200;

	public static bool GetKeyDown(KeyCode key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Input.GetKeyDown(key);
	}

	public static bool GetKeyUp(KeyCode key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Input.GetKeyUp(key);
	}

	public static bool GetKey(KeyCode key)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Input.GetKey(key);
	}

	public bool IsWorkshopLevel()
	{
		WorkshopItemSource workshopItemSource = currentLevelType;
		if (workshopItemSource - 2 <= WorkshopItemSource.EditorPick)
		{
			return true;
		}
		return false;
	}

	public void Initialize()
	{
		instance = this;
		Physics.set_autoSyncTransforms(false);
		Dependencies.Initialize<MenuCameraEffects>();
		Dependencies.OnInitialized(this);
		gameProgress = new GameProgress();
		defaultLight = ((Component)this).GetComponentInChildren<Light>();
		state = GameState.Inactive;
		levelCount = levels.Length - 1;
	}

	private void Awake()
	{
		PostProcessLayer[] componentsInChildren = ((Component)this).GetComponentsInChildren<PostProcessLayer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Behaviour)componentsInChildren[i]).set_enabled(true);
		}
	}

	private void OnEnable()
	{
		Debug.Log((object)"Game.OnEnable called");
		HasSceneLoaded = false;
		SceneManager.add_sceneLoaded((UnityAction<Scene, LoadSceneMode>)OnSceneLoaded);
		SceneManager.add_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		HasSceneLoaded = true;
		if (NetGame.isServer && NetGame.currentLevelInstanceID != NetGame.nextLevelInstanceID)
		{
			NetGame.currentLevelInstanceID = 0;
		}
		Debug.Log((object)("OnSceneLoaded: " + ((Scene)(ref scene)).get_name() + "  mode=" + ((object)(LoadSceneMode)(ref mode)).ToString()));
	}

	private void OnSceneUnloaded(Scene scene)
	{
		Debug.Log((object)("OnSceneUnloaded: " + ((Scene)(ref scene)).get_name()));
		if (NetGame.isServer && NetGame.currentLevelInstanceID != NetGame.nextLevelInstanceID)
		{
			NetGame.currentLevelInstanceID = 0;
		}
		GroundManager.ResetOnSceneUnload();
	}

	private void OnDisable()
	{
		Debug.Log((object)"Game.OnDisable called");
		HasSceneLoaded = false;
		SceneManager.remove_sceneLoaded((UnityAction<Scene, LoadSceneMode>)OnSceneLoaded);
		SceneManager.remove_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
	}

	public void Update()
	{
		if (!((Object)(object)MenuSystem.instance.activeMenu != (Object)null) && state == GameState.PlayingLevel && (Object)(object)MultiplayerLobbyController.instance != (Object)null)
		{
			_ = NetGame.isNetStarted;
		}
	}

	private void FixedUpdate()
	{
		if (!((Object)(object)MenuSystem.instance.activeMenu != (Object)null) && state == GameState.PlayingLevel && (Object)(object)MultiplayerLobbyController.instance != (Object)null)
		{
			_ = NetGame.isNetStarted;
		}
	}

	private void ShowPause()
	{
		if (MenuSystem.keyboardState == KeyboardState.None)
		{
			MenuSystem.instance.ShowPauseMenu();
			if (NetGame.isLocal)
			{
				Time.set_timeScale(0f);
				state = GameState.Paused;
			}
		}
	}

	public void Resume()
	{
		Time.set_timeScale(1f);
		state = GameState.PlayingLevel;
	}

	public void GameOver()
	{
		HumanAnalytics.instance.GameOver();
		App.instance.PauseLeave();
	}

	public void UnloadLevel()
	{
		StatsAndAchievements.Save();
		Resume();
		AfterUnload();
		MenuCameraEffects menuCameraEffects = MenuCameraEffects.instance;
		if (Object.op_Implicit((Object)(object)menuCameraEffects))
		{
			menuCameraEffects.ForceDisableOcclusion(forceDisableOcclusion: false);
		}
	}

	public void AfterUnload()
	{
		workshopLevel = null;
		workshopLevelIsCustom = false;
		state = GameState.Inactive;
		if ((Object)(object)currentLevel != (Object)null)
		{
			DisableOnExit.ExitingLevel(currentLevel);
			((Component)defaultLight).get_gameObject().SetActive(true);
			currentLevel = null;
			((MonoBehaviour)this).StartCoroutine(UnloadBundle());
		}
		currentLevelNumber = -1;
		state = GameState.Inactive;
	}

	private IEnumerator UnloadBundle()
	{
		yield return null;
		if ((Object)(object)bundle != (Object)null)
		{
			bundle.Unload(true);
		}
	}

	public bool LevelLoaded(Level level)
	{
		currentLevel = level;
		return NetGame.isClient;
	}

	public void SolvePuzzle(int puzzle)
	{
		if (state == GameState.PlayingLevel && !currentSolvedCheckpoints.Contains(puzzle))
		{
			currentSolvedCheckpoints.Add(puzzle);
			Debug.Log((object)"TODO: Save the puzzle progress");
		}
	}

	public bool IsCheckpointSolved(int puzzle)
	{
		return currentSolvedCheckpoints.Contains(puzzle);
	}

	public void EnterCheckpoint(int checkpoint, int subObjectives)
	{
		if (state != GameState.PlayingLevel)
		{
			return;
		}
		bool flag = false;
		if (currentCheckpointNumber < checkpoint)
		{
			flag = true;
		}
		else if (currentLevel.nonLinearCheckpoints && currentCheckpointNumber != checkpoint)
		{
			flag = true;
		}
		else if (currentCheckpointNumber == checkpoint && subObjectives != 0)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		Debug.Log((object)("Passed " + checkpoint + ", subobjectives: " + subObjectives));
		passedCheckpoint_ForSteelSeriesEvent = true;
		int num = currentCheckpointNumber;
		int num2 = currentCheckpointSubObjectives;
		if (currentCheckpointNumber != checkpoint)
		{
			currentCheckpointSubObjectives = 0;
		}
		if (subObjectives != 0)
		{
			currentCheckpointSubObjectives |= 1 << subObjectives - 1;
		}
		currentCheckpointNumber = checkpoint;
		if (workshopLevel == null && currentLevelNumber != -1)
		{
			if (NetGame.isLocal)
			{
				if (currentLevelType == WorkshopItemSource.EditorPick)
				{
					GameSave.PassCheckpointEditorPick((uint)currentLevelNumber, checkpoint, currentCheckpointSubObjectives);
				}
				else
				{
					GameSave.PassCheckpointCampaign((uint)currentLevelNumber, checkpoint, currentCheckpointSubObjectives);
				}
			}
			if (num != currentCheckpointNumber || num2 != currentCheckpointSubObjectives)
			{
				SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.SAVING, 1f, 1f);
			}
		}
		else
		{
			if (NetGame.isLocal)
			{
				GameSave.PassCheckpointWorkshop(workshopLevel.hash, checkpoint);
			}
			SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.SAVING, 1f, 1f);
		}
		if (NetGame.isServer)
		{
			NetSceneManager.EnterCheckpoint(checkpoint, subObjectives);
		}
	}

	public void EnterPassZone()
	{
		if (state == GameState.PlayingLevel && (!NetGame.isServer || Options.lobbyLockLevel != 1))
		{
			passedCheckpoint_ForSteelSeriesEvent = true;
			passedLevel = true;
		}
	}

	public void Fall(HumanBase humanBase, bool drown = false, bool fallAchievement = true)
	{
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		Human human = humanBase as Human;
		if (ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		if (passedLevel && ((currentLevelNumber >= 0 && currentLevelNumber < levels.Length) || workshopLevel != null || currentLevelType == WorkshopItemSource.EditorPick))
		{
			if (ReplayRecorder.isRecording)
			{
				ReplayRecorder.Stop();
			}
			else if (workshopLevel != null)
			{
				passedLevel = false;
				PlayerManager.SetSingle();
				App.instance.PauseLeave();
				GameSave.PassCheckpointWorkshop(workshopLevel.hash, 0);
			}
			else if (currentLevelType == WorkshopItemSource.EditorPick)
			{
				passedLevel = false;
				PlayerManager.SetSingle();
				App.instance.PauseLeave();
				if (NetGame.isLocal)
				{
					GameSave.StartFromBeginning();
				}
				for (int i = 0; i < Human.all.Count; i++)
				{
					Human.all[i].ReleaseGrab();
				}
			}
			else
			{
				StatsAndAchievements.PassLevel(levels[currentLevelNumber], human);
				for (int j = 0; j < Human.all.Count; j++)
				{
					Human.all[j].ReleaseGrab();
					Human.all[j].state = HumanState.Fall;
				}
				((MonoBehaviour)this).StartCoroutine(PassLevel());
			}
		}
		else
		{
			if (drown)
			{
				StatsAndAchievements.IncreaseDrownCount(human);
			}
			else if (fallAchievement)
			{
				StatsAndAchievements.IncreaseFallCount(human);
			}
			Respawn(human, Vector3.get_zero());
			CheckpointRespawned(currentCheckpointNumber);
		}
	}

	public void Drown(Human human)
	{
		if (OnDrowning != null)
		{
			OnDrowning(human);
		}
		Fall(human, drown: true);
	}

	private void ResetAllPLayers()
	{
		for (int i = 0; i < Human.all.Count; i++)
		{
			ResetPlayer(Human.all[i]);
		}
	}

	public void RespawnAllPlayers(NetHost host)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Checkpoint checkpoint = currentLevel.GetCheckpoint(currentCheckpointNumber);
		for (int i = 0; i < host.players.Count; i++)
		{
			float num = (checkpoint.tightSpawn ? 0.5f : 2f);
			Respawn(host.players[i].human, Vector3.get_left() * (float)(i % 3) * num + Vector3.get_back() * (float)(i / 3) * num);
		}
	}

	public void RespawnAllPlayers()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		Checkpoint checkpoint = currentLevel.GetCheckpoint(currentCheckpointNumber);
		for (int i = 0; i < Human.all.Count; i++)
		{
			float num = (checkpoint.tightSpawn ? 0.5f : 2f);
			Respawn(Human.all[i], Vector3.get_left() * (float)(i % 3) * num + Vector3.get_back() * (float)(i / 3) * num);
		}
	}

	private void ResetPlayer(Human human)
	{
		GrabManager.Release(((Component)human).get_gameObject());
		human.ReleaseGrab();
		human.Reset();
	}

	public void Respawn(Human human, Vector3 offset)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)currentLevel == (Object)null))
		{
			passedLevel = false;
			Transform checkpointTransform = currentLevel.GetCheckpointTransform(currentCheckpointNumber);
			ResetPlayer(human);
			human.SpawnAt(checkpointTransform, offset);
		}
	}

	public void DebugCompleteLevel()
	{
		if (state != GameState.PlayingLevel)
		{
			return;
		}
		AppSate appSate = App.state;
		if ((appSate != AppSate.PlayLevel && appSate != AppSate.ServerPlayLevel && appSate != AppSate.ClientPlayLevel) || (NetGame.isServer && Options.lobbyLockLevel == 1) || ReplayRecorder.isRecording || ReplayRecorder.isPlaying || NetGame.isClient || workshopLevel != null || currentLevelNumber < 0 || currentLevelNumber >= levels.Length)
		{
			return;
		}
		passedLevel = true;
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human.all[i].ReleaseGrab();
		}
		if (currentLevelNumber == levelCount)
		{
			if ((Object)(object)currentLevel != (Object)null)
			{
				CreditsFadeOutAndEnd.Trigger(((Component)currentLevel).get_gameObject(), null);
			}
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(PassLevel());
		}
	}

	private bool GameIsCompleted()
	{
		if (currentLevelNumber == levelCount - 1)
		{
			return true;
		}
		if (currentLevelNumber < levelCount - 1)
		{
			return false;
		}
		if (!DLC.instance.SupportsDLC())
		{
			return false;
		}
		int num = currentLevelNumber + 1;
		int num2 = levelCount - num;
		for (int i = 0; i < num2; i++)
		{
			if (DLC.instance.LevelIsAvailable(num + i))
			{
				return false;
			}
		}
		return true;
	}

	private int GetNextLevel(int currentLevel)
	{
		currentLevel++;
		if (!DLC.instance.SupportsDLC())
		{
			return currentLevel;
		}
		for (int i = 0; i < 0; i++)
		{
			if (DLC.instance.LevelIsAvailable(currentLevel))
			{
				return currentLevel;
			}
			currentLevel++;
		}
		return currentLevel;
	}

	public IEnumerator PassLevel()
	{
		currentLevel.CompleteLevel();
		if (NetGame.isLocal)
		{
			GameSave.PassCheckpointCampaign((uint)currentLevelNumber, 0, 0);
		}
		if (GameIsCompleted())
		{
			if (singleRun)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_SINGLE_RUN);
			}
			if (NetGame.isLocal)
			{
				GameSave.CompleteGame(levelCount);
			}
			yield return null;
			App.instance.StartNextLevel((uint)levelCount, 0);
		}
		else
		{
			int nextLevel = GetNextLevel(currentLevelNumber);
			if (NetGame.isLocal)
			{
				GameSave.PassCheckpointCampaign((uint)nextLevel, 0, 0);
			}
			yield return null;
			App.instance.StartNextLevel((uint)nextLevel, 0);
		}
		StatsAndAchievements.Save();
	}

	private void FixupLoadedBundle(Scene scene)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!currentLevel.keepSkybox)
		{
			RenderSettings.set_skybox(skyboxMaterial);
			RenderSettings.set_ambientMode((AmbientMode)3);
			RenderSettings.set_ambientLight(skyColor);
		}
		BundleRepository.RebindScene(scene);
	}

	public void ReloadBundle()
	{
		((MonoBehaviour)this).StartCoroutine(ReloadBundleCoroutine());
	}

	private IEnumerator ReloadBundleCoroutine()
	{
		SignalManager.BeginReset();
		if (workshopLevel != null)
		{
			MenuCameraEffects.instance.RemoveOverride();
			workshopLevel.Reload();
		}
		Time.set_timeScale(0f);
		SceneManager.LoadScene("Empty");
		yield return null;
		yield return null;
		yield return null;
		bundle = FileTools.LoadBundle(workshopLevel.dataPath);
		SceneManager.LoadScene(Path.GetFileNameWithoutExtension(bundle.GetAllScenePaths()[0]));
		yield return null;
		SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.LOADING, 1f, 1f);
		yield return null;
		FixupLoadedBundle(SceneManager.GetActiveScene());
		bundle.Unload(false);
		currentLevel.BeginLevel();
		ResetAllPLayers();
		Time.set_timeScale(1f);
		SignalManager.EndReset();
		FixAssetBundleImport();
	}

	public void BeginLoadLevel(string levelId, ulong levelNumber, int checkpointNumber, int checkpointSubObjectives, Action onComplete, WorkshopItemSource type)
	{
		passedLevel = false;
		((MonoBehaviour)this).StartCoroutine(LoadLevel(levelId, levelNumber, checkpointNumber, checkpointSubObjectives, onComplete, type));
	}

	private void FixAssetBundleAudioMixerGroups(bool lobby)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (workshopLevel == null && currentLevelType != WorkshopItemSource.EditorPick && (!lobby || multiplayerLobbyLevel <= 16))
		{
			return;
		}
		SceneManager.GetActiveScene();
		AudioSource[] array = Object.FindObjectsOfType<AudioSource>();
		GameAudio gameAudio = Object.FindObjectOfType<GameAudio>();
		if ((Object)(object)gameAudio == (Object)null)
		{
			return;
		}
		AudioMixer mainMixer = gameAudio.mainMixer;
		if ((Object)(object)mainMixer == (Object)null)
		{
			return;
		}
		AudioMixerGroup[] array2 = mainMixer.FindMatchingGroups("Effects");
		AudioSource[] array3 = array;
		foreach (AudioSource val in array3)
		{
			if ((Object)(object)val.get_outputAudioMixerGroup() != (Object)null)
			{
				AudioMixerGroup[] array4 = mainMixer.FindMatchingGroups(((Object)val.get_outputAudioMixerGroup()).get_name());
				if (array4.Length != 0)
				{
					val.set_outputAudioMixerGroup(array4[0]);
				}
			}
			else
			{
				val.set_outputAudioMixerGroup(array2[0]);
			}
		}
	}

	private void FixAssetBundleShaders(bool lobby)
	{
		if (workshopLevel == null && (!lobby || multiplayerLobbyLevel <= 16))
		{
			return;
		}
		RenderSettings.get_skybox().set_shader(Shader.Find(((Object)RenderSettings.get_skybox().get_shader()).get_name()));
		if ((Object)(object)RenderSettings.get_skybox().get_shader() == (Object)null)
		{
			Debug.Log((object)("FixAssetBundleShaders:shader not found:sb: " + ((Object)RenderSettings.get_skybox().get_shader()).get_name()));
		}
		Material[] array = Resources.FindObjectsOfTypeAll<Material>();
		foreach (Material val in array)
		{
			Shader val2 = Shader.Find(((Object)val.get_shader()).get_name());
			if ((Object)(object)val2 != (Object)null)
			{
				val.set_shader(val2);
				if (((Object)val.get_shader()).get_name() == "Standard")
				{
					float @float = val.GetFloat("_Mode");
					if (@float == 0f)
					{
						val.set_renderQueue(-1);
					}
					else if (@float == 1f)
					{
						val.set_renderQueue(2450);
					}
					else
					{
						val.set_renderQueue(3000);
					}
				}
			}
			else
			{
				Debug.Log((object)("FixAssetBundleShaders:shader not found: " + ((Object)val.get_shader()).get_name()));
			}
		}
	}

	public void FixAssetBundleImport(bool lobby = false)
	{
		FixAssetBundleAudioMixerGroups(lobby);
		FixAssetBundleShaders(lobby);
	}

	private IEnumerator LoadLevel(string levelId, ulong levelNumber, int checkpointNumber, int checkpointSubObjectives, Action onComplete, WorkshopItemSource levelType)
	{
		bool localLevel = false;
		NetScope.ClearAllButPlayers();
		BeforeLoad();
		skyColor = RenderSettings.get_ambientLight();
		skyboxMaterial = RenderSettings.get_skybox();
		state = GameState.LoadingLevel;
		bool isBundle = !string.IsNullOrEmpty(levelId) || (levelNumber > 16 && levelNumber != ulong.MaxValue);
		if (isBundle)
		{
			if (string.IsNullOrEmpty(levelId))
			{
				bool loaded2 = false;
				WorkshopRepository.instance.levelRepo.GetLevel(levelNumber, levelType, delegate(WorkshopLevelMetadata l)
				{
					workshopLevel = l;
					loaded2 = true;
				});
				while (!loaded2)
				{
					yield return null;
				}
			}
			else
			{
				localLevel = levelId.StartsWith("lvl:");
				workshopLevel = WorkshopRepository.instance.levelRepo.GetItem(levelId);
			}
			RatingMenu.instance.LoadInit();
			if (!localLevel && workshopLevel != null)
			{
				App.StartPlaytimeForItem(workshopLevel.workshopId);
				RatingMenu.instance.QueryRatingStatus(workshopLevel.workshopId);
			}
		}
		App.StartPlaytimeLocalPlayers();
		if (currentLevelNumber != (int)levelNumber)
		{
			_ = currentLevel;
			SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.LOADING);
			Application.set_backgroundLoadingPriority((ThreadPriority)0);
			string sceneName = string.Empty;
			currentLevelType = levelType;
			switch (levelType)
			{
			case WorkshopItemSource.BuiltIn:
				sceneName = levels[levelNumber];
				break;
			case WorkshopItemSource.EditorPick:
				Debug.Log((object)"Loading editor pick level");
				sceneName = editorPickLevels[(uint)levelNumber];
				break;
			}
			Debug.Log((object)("scename = " + sceneName));
			if (!isBundle)
			{
				if (string.IsNullOrEmpty(sceneName))
				{
					sceneName = levels[levelNumber];
				}
			}
			else
			{
				if (!localLevel && workshopLevel != null)
				{
					bool loaded = false;
					WorkshopRepository.instance.levelRepo.LoadLevel(workshopLevel.workshopId, delegate(WorkshopLevelMetadata l)
					{
						workshopLevel = l;
						loaded = true;
					});
					while (!loaded)
					{
						yield return null;
					}
				}
				bundle = null;
				if (workshopLevel != null)
				{
					bundle = FileTools.LoadBundle(workshopLevel.dataPath);
				}
				if ((Object)(object)bundle == (Object)null)
				{
					SubtitleManager.instance.ClearProgress();
					Debug.Log((object)"Level load failed.");
					App.instance.ServerFailedToLoad();
					SignalManager.EndReset();
					yield break;
				}
				string[] allScenePaths = bundle.GetAllScenePaths();
				if (string.IsNullOrEmpty(sceneName))
				{
					sceneName = Path.GetFileNameWithoutExtension(allScenePaths[0]);
				}
			}
			instance.HasSceneLoaded = false;
			SwitchAssetBundle.LoadingCurrentScene loader = SwitchAssetBundle.LoadSceneAsync(sceneName);
			if (loader == null)
			{
				AsyncOperation sceneLoader = SceneManager.LoadSceneAsync(sceneName, (LoadSceneMode)0);
				while (!sceneLoader.get_isDone() || !instance.HasSceneLoaded)
				{
					yield return null;
				}
				SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.LOADING, 1f, 1f);
				currentLevelNumber = (int)levelNumber;
			}
			else
			{
				while (!loader.isDone || !instance.HasSceneLoaded)
				{
					yield return null;
				}
				SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.LOADING, 1f, 1f);
				currentLevelNumber = (int)levelNumber;
			}
		}
		if ((Object)(object)currentLevel == (Object)null)
		{
			SignalManager.EndReset();
			onComplete?.Invoke();
			yield break;
		}
		if (isBundle)
		{
			FixupLoadedBundle(SceneManager.GetActiveScene());
			bundle.Unload(false);
		}
		if (currentLevelNumber >= 0 && !isBundle)
		{
			HumanAnalytics.instance.LoadLevel(levels[currentLevelNumber], (int)levelNumber, checkpointNumber, 0f);
		}
		FixAssetBundleImport();
		AfterLoad(checkpointNumber, checkpointSubObjectives);
		if (NetGame.isLocal)
		{
			if (levelType == WorkshopItemSource.BuiltIn && currentLevelNumber < levelCount - 1)
			{
				GameSave.PassCheckpointCampaign((uint)currentLevelNumber, checkpointNumber, checkpointSubObjectives);
			}
			if (levelType == WorkshopItemSource.EditorPick)
			{
				GameSave.PassCheckpointEditorPick((uint)currentLevelNumber, checkpointNumber, checkpointSubObjectives);
			}
		}
		onComplete?.Invoke();
	}

	public void BeforeLoad()
	{
		SignalManager.BeginReset();
	}

	public void AfterLoad(int checkpointNumber, int subobjectives)
	{
		MenuCameraEffects menuCameraEffects = MenuCameraEffects.instance;
		if (Object.op_Implicit((Object)(object)menuCameraEffects) && currentLevelNumber >= 0 && currentLevelNumber < levels.Length)
		{
			if (levels[currentLevelNumber] == "Halloween")
			{
				menuCameraEffects.ForceDisableOcclusion(forceDisableOcclusion: true);
			}
			else
			{
				menuCameraEffects.ForceDisableOcclusion(forceDisableOcclusion: false);
			}
		}
		state = GameState.PlayingLevel;
		((Component)defaultLight).get_gameObject().SetActive(false);
		currentCheckpointNumber = Mathf.Min(checkpointNumber, currentLevel.checkpoints.Length - 1);
		currentCheckpointSubObjectives = subobjectives;
		currentLevel.BeginLevel();
		if (currentLevel.prerespawn != null)
		{
			currentLevel.prerespawn(currentCheckpointNumber, startingLevel: true);
		}
		RespawnAllPlayers();
		currentLevel.Reset(currentCheckpointNumber, currentCheckpointSubObjectives);
		CheckpointLoaded(checkpointNumber);
		SignalManager.EndReset();
		currentLevel.PostEndReset(currentCheckpointNumber);
	}

	public void CheckpointLoaded(int checkpointNumber)
	{
		if (checkpointNumber > 0 && (Object)(object)currentLevel != (Object)null && checkpointNumber < currentLevel.checkpoints.Length && (Object)(object)currentLevel.checkpoints[checkpointNumber] != (Object)null)
		{
			Checkpoint component = ((Component)currentLevel.checkpoints[checkpointNumber]).GetComponent<Checkpoint>();
			if ((Object)(object)component != (Object)null)
			{
				component.LoadHere();
			}
		}
	}

	private void CheckpointRespawned(int checkpointNumber)
	{
		if (!((Object)(object)currentLevel == (Object)null) && currentLevel.checkpoints != null && checkpointNumber >= 0 && checkpointNumber < currentLevel.checkpoints.Length && !((Object)(object)currentLevel.checkpoints[checkpointNumber] == (Object)null) && Human.all.Count <= 1)
		{
			Checkpoint component = ((Component)currentLevel.checkpoints[checkpointNumber]).GetComponent<Checkpoint>();
			if ((Object)(object)component != (Object)null)
			{
				component.RespawnHere();
			}
		}
	}

	public void RestartCheckpoint()
	{
		RestartCheckpoint(currentCheckpointNumber, currentCheckpointSubObjectives);
	}

	public void RestartCheckpoint(int checkpointNumber, int subObjectives)
	{
		if (!currentLevel.respawnLocked)
		{
			currentCheckpointNumber = Mathf.Min(checkpointNumber, currentLevel.checkpoints.Length - 1);
			currentCheckpointSubObjectives = subObjectives;
			SignalManager.BeginReset();
			if (currentLevel.prerespawn != null)
			{
				currentLevel.prerespawn(checkpointNumber, startingLevel: false);
			}
			RespawnAllPlayers();
			currentLevel.Reset(currentCheckpointNumber, currentCheckpointSubObjectives);
			if (NetGame.isServer)
			{
				NetSceneManager.ResetLevel(currentCheckpointNumber, currentCheckpointSubObjectives);
			}
			currentLevel.BeginLevel();
			CheckpointLoaded(currentCheckpointNumber);
			SignalManager.EndReset();
			currentLevel.PostEndReset(currentCheckpointNumber);
		}
	}

	public void RestartLevel(bool reset = true)
	{
		if (currentLevel.respawnLocked)
		{
			return;
		}
		SignalManager.BeginReset();
		currentCheckpointNumber = 0;
		currentCheckpointSubObjectives = 0;
		if (currentLevel.prerespawn != null)
		{
			currentLevel.prerespawn(-1, startingLevel: false);
		}
		RespawnAllPlayers();
		if (reset)
		{
			currentLevel.Reset(currentCheckpointNumber, currentCheckpointSubObjectives);
			if (NetGame.isServer)
			{
				NetSceneManager.ResetLevel(currentCheckpointNumber, currentCheckpointSubObjectives);
			}
		}
		currentLevel.BeginLevel();
		SignalManager.EndReset();
		if (reset)
		{
			currentLevel.PostEndReset(currentCheckpointNumber);
		}
	}

	public Game()
		: this()
	{
	}
}
