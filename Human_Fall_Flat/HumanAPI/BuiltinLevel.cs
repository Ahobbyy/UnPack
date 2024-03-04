using Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumanAPI
{
	public class BuiltinLevel : Level
	{
		public Transform debugSpawnPoint;

		public GameObject gamePrefab;

		public GameObject resourcePrefab;

		public float customCloudNearClipStart = 5f;

		public float customCloudNearClipEnd = 10f;

		public float customCloudFarClipStart = 150f;

		public float customCloudFarClipEnd = 200f;

		private void Start()
		{
			LevelWithMe[] array = Object.FindObjectsOfType<LevelWithMe>();
			for (int i = 0; i < array.Length; i++)
			{
				((Component)array[i]).get_transform().set_parent(((Component)this).get_transform());
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if ((Object)(object)resourcePrefab != (Object)null)
			{
				Object.Instantiate<GameObject>(resourcePrefab);
			}
			CloudSystem.OnSystemInit += delegate
			{
				CloudSystem.instance.nearClipStart = customCloudNearClipStart;
				CloudSystem.instance.nearClipEnd = customCloudNearClipEnd;
				CloudSystem.instance.farClipStart = customCloudFarClipStart;
				CloudSystem.instance.farClipEnd = customCloudFarClipEnd;
			};
		}

		private void OnDisable()
		{
			FreeRoamCam.CleanUp();
		}

		protected override void OnEnable()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)Game.instance == (Object)null)
			{
				Object.Instantiate<GameObject>(gamePrefab);
				Dependencies.Initialize<App>();
				for (int i = 0; i < Game.instance.levelCount; i++)
				{
					Scene activeScene = SceneManager.GetActiveScene();
					if (string.Equals(((Scene)(ref activeScene)).get_name(), Game.instance.levels[i]))
					{
						Game.instance.currentLevelNumber = i;
					}
				}
				App.instance.BeginLevel();
			}
			base.OnEnable();
			if ((Object)(object)debugSpawnPoint != (Object)null)
			{
				checkpoints[0] = debugSpawnPoint;
				if ((Object)(object)((Component)debugSpawnPoint).GetComponent<Checkpoint>() == (Object)null)
				{
					((Component)debugSpawnPoint).get_gameObject().AddComponent<Checkpoint>();
				}
			}
		}

		public override void CompleteLevel()
		{
			base.CompleteLevel();
			DisableOnExit.ExitingLevel(this);
		}
	}
}
