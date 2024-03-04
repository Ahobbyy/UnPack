using System;
using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Level/Level", 10)]
	public class Level : MonoBehaviour
	{
		public delegate void PreRespawnDelegate(int checkpoint, bool startingLevel);

		public uint netHash = 57005u;

		public bool keepSkybox;

		public bool noClouds;

		public Transform spawnPoint;

		public Color fogColor;

		public float fogDensity;

		public bool nonLinearCheckpoints;

		public bool HasWeather;

		public float MaxHumanVelocity = 150f;

		[NonSerialized]
		public Transform[] checkpoints;

		[NonSerialized]
		public PreRespawnDelegate prerespawn;

		[NonSerialized]
		public bool respawnLocked;

		private bool isActive;

		private bool isClient;

		private List<Checkpoint> checkpointsList;

		public List<Rigidbody[]> chains = new List<Rigidbody[]>();

		public Vector3[] cachedPos;

		public Quaternion[] cachedRot;

		public bool active => isActive;

		protected virtual void OnEnable()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			fogColor = RenderSettings.get_fogColor();
			fogDensity = RenderSettings.get_fogDensity();
			isClient = Dependencies.Get<IGame>().LevelLoaded(this);
			Checkpoint[] componentsInChildren = ((Component)this).GetComponentsInChildren<Checkpoint>();
			checkpointsList = new List<Checkpoint>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].subObjective == 0)
				{
					checkpointsList.Add(componentsInChildren[i]);
				}
			}
			checkpoints = (Transform[])(object)new Transform[checkpointsList.Count];
			for (int j = 0; j < checkpointsList.Count; j++)
			{
				checkpoints[checkpointsList[j].number] = ((Component)checkpointsList[j]).get_transform();
			}
			for (int k = 0; k < checkpoints.Length; k++)
			{
				if ((Object)(object)checkpoints[k] == (Object)null)
				{
					throw new InvalidOperationException("Checkpoint " + k + " missing");
				}
			}
		}

		public virtual void Reset(int checkpoint, int subObjectives)
		{
			IPreReset[] componentsInChildren = ((Component)this).GetComponentsInChildren<IPreReset>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].PreResetState(checkpoint);
			}
			IReset[] componentsInChildren2 = ((Component)this).GetComponentsInChildren<IReset>(true);
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				componentsInChildren2[j].ResetState(checkpoint, subObjectives);
			}
			IPostReset[] componentsInChildren3 = ((Component)this).GetComponentsInChildren<IPostReset>(true);
			for (int k = 0; k < componentsInChildren3.Length; k++)
			{
				componentsInChildren3[k].PostResetState(checkpoint);
			}
		}

		public void PostEndReset(int checkpoint)
		{
			IPostEndReset[] componentsInChildren = ((Component)this).GetComponentsInChildren<IPostEndReset>(true);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].PostEndResetState(checkpoint);
			}
		}

		public virtual void BeginLevel()
		{
			isActive = true;
			HumanBase[] array = Object.FindObjectsOfType<HumanBase>();
			for (int i = 0; i < array.Length; i++)
			{
				IBeginLevel[] componentsInChildren = ((Component)array[i]).GetComponentsInChildren<IBeginLevel>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].BeginLevel();
				}
			}
		}

		public virtual void CompleteLevel()
		{
			isActive = false;
			Collider[] componentsInChildren = ((Component)this).GetComponentsInChildren<Collider>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				if (componentsInChildren[i].get_isTrigger())
				{
					componentsInChildren[i].set_enabled(false);
				}
			}
		}

		public Transform GetCheckpointTransform(int checkpoint)
		{
			return checkpoints[checkpoint];
		}

		public Checkpoint GetCheckpoint(int checkpointID)
		{
			return checkpointsList[checkpointID];
		}

		protected virtual void Awake()
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			chains = new List<Rigidbody[]>();
			Rigidbody[] componentsInChildren = ((Component)this).GetComponentsInChildren<Rigidbody>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				Rigidbody[] componentsInParent = ((Component)componentsInChildren[i]).GetComponentsInParent<Rigidbody>();
				if (componentsInParent.Length != 0)
				{
					chains.Add(componentsInParent);
				}
			}
			cachedPos = (Vector3[])(object)new Vector3[chains.Count];
			cachedRot = (Quaternion[])(object)new Quaternion[chains.Count];
			for (int j = 0; j < chains.Count; j++)
			{
				Rigidbody val = chains[j][0];
				cachedPos[j] = val.get_position();
				cachedRot[j] = val.get_rotation();
			}
		}

		private void LateUpdate()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			if (isClient)
			{
				return;
			}
			for (int i = 0; i < chains.Count; i++)
			{
				Rigidbody[] array = chains[i];
				Rigidbody val = array[0];
				if (val.IsSleeping())
				{
					for (int j = 1; j < array.Length; j++)
					{
						if (!array[j].IsSleeping())
						{
							((Component)val).get_transform().set_position(cachedPos[i]);
							((Component)val).get_transform().set_rotation(cachedRot[i]);
							break;
						}
					}
				}
				else
				{
					cachedPos[i] = val.get_position();
					cachedRot[i] = val.get_rotation();
				}
			}
		}

		public Level()
			: this()
		{
		}
	}
}
