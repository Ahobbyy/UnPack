using System;
using System.Collections;
using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEditor;
using UnityEngine;

public class CreditsExperience : MonoBehaviour, INetBehavior
{
	public class BlockBucket
	{
		public enum State
		{
			Init,
			Active,
			Pausing,
			Paused
		}

		public int bitsLeft;

		public NetScope scope;

		public GameObject go;

		public List<CreditsBlock> blocks = new List<CreditsBlock>();

		public State scopeState;

		public bool IsNeeded
		{
			get
			{
				int i = 0;
				for (int count = blocks.Count; i < count; i++)
				{
					if (blocks[i].isSpawned)
					{
						return true;
					}
				}
				return false;
			}
		}

		public void Collecting()
		{
			switch (scopeState)
			{
			case State.Init:
				scopeState = State.Active;
				break;
			case State.Pausing:
				scope.SuspendCollect = true;
				scopeState = State.Paused;
				break;
			case State.Active:
			case State.Paused:
				break;
			}
		}
	}

	private enum State
	{
		Falling,
		NearGround,
		Ground,
		FadeOut
	}

	internal const uint DataFormatVersion = 1u;

	public GameObject startupDoor;

	private float startupDoorY;

	private float topY;

	private float currentDrag;

	private HashSet<Human> humansInLevel = new HashSet<Human>();

	private List<BlockBucket> buckets = new List<BlockBucket>();

	private bool hasFinishedCredits;

	private bool parsed;

	private bool skipDrag;

	private bool snapAlign;

	private State state;

	private Transform spawn;

	private Vector3 originalSpawn;

	private bool cloudsFiddled;

	private bool canSetClounds;

	private bool lastDrag = true;

	private bool fadeRunning;

	public CreditsGround ground;

	public CreditsText ctext;

	public float blockSpacing = 20f;

	public float showAhead = 100f;

	public float showBehind = 100f;

	public float spawnAbove = 20f;

	[HideInInspector]
	public TextAsset ActiveContent;

	[HideInInspector]
	public uint ContentHash;

	public TextAsset contentSA;

	public TextAsset contentPS;

	public TextAsset contentPSJP;

	public TextAsset contentXB;

	public TextAsset contentSwitch;

	public TextAsset contentSwitchJP;

	private List<CreditsBlock> blocks = new List<CreditsBlock>();

	private const ushort bitsForState = 2;

	private void ParseContent()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		if (parsed)
		{
			return;
		}
		parsed = true;
		ctext.InitData();
		if (Application.get_isEditor() && EditorApplication.get_isPlaying())
		{
			CreditsBuildPreProc.PlayInEditor();
		}
		TextAsset activeContent = ActiveContent;
		List<CreditsBlock> list = new List<CreditsBlock>();
		string[] array = activeContent.get_text().Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Trim();
		}
		int j = 0;
		float num = 0f;
		while (j < array.Length)
		{
			for (; j < array.Length && string.IsNullOrEmpty(array[j]); j++)
			{
			}
			if (j < array.Length)
			{
				string text = array[j++];
				while (j < array.Length && !string.IsNullOrEmpty(array[j]))
				{
					text = text + "\r\n" + array[j++];
				}
				CreditsBlock component = Object.Instantiate<GameObject>(((Component)ctext.blockPrefab).get_gameObject()).GetComponent<CreditsBlock>();
				((Component)component).get_transform().SetParent(((Component)this).get_transform(), false);
				((Component)component).get_transform().set_position(num * Vector3.get_up());
				component.Initialize(num, text, ctext);
				blocks.Add(component);
				list.Add(component);
				num -= blockSpacing;
				NetIdentity netIdentity = ((Component)component).get_gameObject().AddComponent<NetIdentity>();
				netIdentity.sceneId = (uint)blocks.Count;
				component.blockId = netIdentity;
				((Object)((Component)component).get_gameObject()).set_name("CrBlock" + netIdentity.sceneId);
			}
		}
		startupDoorY = num - showAhead;
		topY = 0f;
		uint num2 = (uint)NetHost.CalcMaxPossibleSizeForContainerContentsTier0();
		buckets.Clear();
		uint num3 = 288u;
		int k = 0;
		for (int count = list.Count; k < count; k++)
		{
			CreditsBlock creditsBlock = list[k];
			int bitsRequired = creditsBlock.bitsRequired;
			int count2 = buckets.Count;
			int num4 = count2 - 1;
			if (num4 < 0 || buckets[num4].bitsLeft < bitsRequired)
			{
				num4++;
			}
			BlockBucket blockBucket;
			if (num4 < count2)
			{
				blockBucket = buckets[num4];
			}
			else
			{
				blockBucket = new BlockBucket();
				buckets.Add(blockBucket);
				if (num3 > 1054)
				{
					throw new ApplicationException("Run out of auxilliary net ids");
				}
				blockBucket.go = new GameObject("LettersScope" + num3);
				blockBucket.scope = blockBucket.go.AddComponent<NetScope>();
				((Behaviour)blockBucket.scope).set_enabled(false);
				blockBucket.scope.netId = num3++;
				blockBucket.scope.AllowSuspendCollect = true;
				blockBucket.go.get_transform().SetParent(((Component)this).get_gameObject().get_transform(), false);
				((Behaviour)blockBucket.scope).set_enabled(true);
				blockBucket.bitsLeft = (int)(num2 * 8) - NetScope.CalculateDeltaPrefixSize(blockBucket.scope.netId);
			}
			blockBucket.blocks.Add(creditsBlock);
			blockBucket.bitsLeft -= bitsRequired;
			if (blockBucket.bitsLeft < 0)
			{
				throw new ApplicationException($"Credits string failed to fit into a packet: required bits={bitsRequired}, packet max={(int)(num2 * 8) - NetScope.CalculateDeltaPrefixSize(blockBucket.scope.netId)}. Text=\"{creditsBlock.text}\"");
			}
			((Component)creditsBlock).get_transform().SetParent(blockBucket.go.get_transform(), true);
			creditsBlock.bucket = blockBucket;
		}
		int l = 0;
		for (int count3 = buckets.Count; l < count3; l++)
		{
			BlockBucket blockBucket2 = buckets[l];
			blockBucket2.scope.StartNetwork();
			int num5 = blockBucket2.scope.CalculateMaxDeltaSizeInBits(blockBucket2.scope.netId);
			int num6 = (int)(num2 * 8) - blockBucket2.bitsLeft;
			if (num5 != num6)
			{
				Debug.LogErrorFormat("Something wrong with delta calculation for bucket scope {0}: reports {1}, expected {2}", new object[3]
				{
					blockBucket2.scope.netId,
					num5,
					num6
				});
			}
		}
		CheckScopes();
	}

	public void CheckScopes()
	{
		int i = 0;
		for (int count = buckets.Count; i < count; i++)
		{
			BlockBucket blockBucket = buckets[i];
			switch (blockBucket.scopeState)
			{
			case BlockBucket.State.Active:
			case BlockBucket.State.Pausing:
				blockBucket.scopeState = (blockBucket.IsNeeded ? BlockBucket.State.Active : BlockBucket.State.Pausing);
				break;
			case BlockBucket.State.Paused:
				if (blockBucket.IsNeeded)
				{
					blockBucket.scopeState = BlockBucket.State.Active;
					blockBucket.scope.SuspendCollect = false;
				}
				break;
			}
		}
	}

	private void Awake()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		ParseContent();
		Level componentInParent = ((Component)this).GetComponentInParent<Level>();
		componentInParent.netHash ^= ContentHash;
		ContentHash = 0u;
		spawn = componentInParent.spawnPoint;
		spawn.set_position(Vector3.get_up() * (topY + spawnAbove));
		ReinitDoor(init: true);
		originalSpawn = ((Component)spawn).get_transform().get_position();
		componentInParent.prerespawn = delegate(int cp, bool startingLevel)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			if (cp == -1 || startingLevel)
			{
				((Component)spawn).get_transform().set_position(originalSpawn);
			}
			foreach (Human item in Human.all)
			{
				item.ControlVelocity(0.1f, killHorizontal: false);
			}
		};
	}

	private IEnumerator Start()
	{
		canSetClounds = false;
		StartingPlaces(initDoor: true);
		UpdateFromState();
		yield return (object)new WaitForSeconds(0.1f);
		if (!hasFinishedCredits)
		{
			canSetClounds = true;
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (checkpoint != 0)
		{
			return;
		}
		StartingPlaces(initDoor: false);
		if (!NetGame.isClient)
		{
			for (int i = 0; i < blocks.Count; i++)
			{
				blocks[i].DespawnAll();
			}
			CheckScopes();
		}
		UpdateFromState();
	}

	public void StartingPlaces(bool initDoor)
	{
		skipDrag = false;
		state = State.Falling;
		snapAlign = true;
		currentDrag = 0.4f;
		humansInLevel.Clear();
		for (int i = 0; i < Human.all.Count; i++)
		{
			if (!skipDrag)
			{
				Human.all[i].SetDrag(currentDrag);
			}
			humansInLevel.Add(Human.all[i]);
		}
		lastDrag = true;
		hasFinishedCredits = false;
		ReinitDoor(initDoor);
	}

	public void ReinitDoor(bool init)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient || init)
		{
			startupDoor.get_transform().set_position(Vector3.get_up() * startupDoorY);
			startupDoor.get_transform().set_rotation(Quaternion.get_identity());
		}
		if (!NetGame.isClient)
		{
			for (int i = 0; i < blocks.Count; i++)
			{
				CreditsBlock creditsBlock = blocks[i];
				((Component)creditsBlock).get_transform().set_position(creditsBlock.lineY * Vector3.get_up());
				((Component)creditsBlock).get_transform().set_rotation(Quaternion.get_identity());
			}
		}
	}

	private void OnDestroy()
	{
		if (NetGame.isClient)
		{
			SubtitleManager.instance.ClearProgress();
		}
		ClearDrag();
		SetCloudState(newState: false);
		canSetClounds = false;
		hasFinishedCredits = true;
	}

	private void ClearDrag()
	{
		skipDrag = true;
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human.all[i].ResetDrag();
		}
	}

	private void FixedUpdate()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		if (ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		AnalyzeHumans(out var lowHuman, out var _, out var center, out var avgSpeed);
		if (!((Object)(object)lowHuman != (Object)null))
		{
			return;
		}
		if (state < State.Ground)
		{
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				Vector3 val = center - ((Component)human).get_transform().get_position();
				Vector3 val2 = avgSpeed - human.ragdoll.partBall.rigidbody.get_velocity();
				if (((Vector3)(ref val)).get_magnitude() > 1f)
				{
					human.ragdoll.partChest.rigidbody.SafeAddForce(val * 40f + val2 * 40f, (ForceMode)0);
				}
			}
		}
		if (state < State.Ground && ((Component)lowHuman).get_transform().get_position().y - startupDoorY > 2f && !lowHuman.onGround)
		{
			((Component)spawn).get_transform().set_position(((Component)lowHuman).get_transform().get_position());
		}
	}

	private void Update()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		if (hasFinishedCredits || ReplayRecorder.isPlaying)
		{
			return;
		}
		AnalyzeHumans(out var lowHuman, out var highHuman, out var _, out var _);
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			if (humansInLevel.Contains(human))
			{
				if (!skipDrag)
				{
					human.SetDrag(currentDrag);
				}
			}
			else if ((((Object)(object)highHuman == (Object)null) ? (-1f) : (((Component)human).get_transform().get_position().y - ((Component)highHuman).get_transform().get_position().y - showBehind)) < 0f)
			{
				humansInLevel.Add(human);
				if (!skipDrag)
				{
					human.SetDrag(currentDrag);
				}
			}
		}
		if (!NetGame.isClient && (Object)(object)highHuman != (Object)null)
		{
			Despawn(((Component)highHuman).get_transform().get_position());
			if (state == State.Falling)
			{
				Spawn(((Component)lowHuman).get_transform().get_position(), ((Component)highHuman).get_transform().get_position());
			}
			Align(lowHuman, highHuman);
		}
		if (!NetGame.isClient)
		{
			float num = (((Object)(object)lowHuman == (Object)null) ? 1000f : (((Component)lowHuman).get_transform().get_position().y - startupDoorY));
			if (state == State.Falling && num < 20f)
			{
				state = State.NearGround;
			}
			if (state == State.NearGround && num < 2f && (Object)(object)lowHuman != (Object)null && lowHuman.onGround)
			{
				state = State.Ground;
			}
			if (state == State.Ground && num < -20f)
			{
				state = State.FadeOut;
			}
		}
		UpdateFromState();
		if (!NetGame.isClient)
		{
			CheckScopes();
		}
	}

	private void SetCloudState(bool newState)
	{
		if (canSetClounds && cloudsFiddled != newState)
		{
			cloudsFiddled = newState;
			float num = (newState ? 2f : 0.5f);
			CloudSystem.instance.nearClipStart *= num;
			CloudSystem.instance.nearClipEnd *= num;
		}
	}

	private void UpdateFromState()
	{
		if (state < State.Ground)
		{
			if (MenuCameraEffects.instance.creditsAdjust == 0f && MenuSystem.instance.state == MenuSystemState.Inactive)
			{
				MenuCameraEffects.FadeInCredits();
			}
		}
		else if (MenuCameraEffects.instance.creditsAdjust != 0f && !MenuCameraEffects.instance.CreditsAdjustInTransition && MenuSystem.instance.state == MenuSystemState.Inactive)
		{
			MenuCameraEffects.FadeOut(1.5f);
		}
		SetCloudState(state < State.Ground);
		if (!skipDrag)
		{
			bool flag = state < State.NearGround;
			if (flag != lastDrag)
			{
				lastDrag = flag;
				currentDrag = (flag ? 0.4f : 0.05f);
				for (int i = 0; i < Human.all.Count; i++)
				{
					Human human = Human.all[i];
					if (humansInLevel.Contains(human) && !skipDrag)
					{
						human.SetDrag(currentDrag);
					}
					human.state = HumanState.Spawning;
				}
			}
		}
		bool flag2 = state >= State.FadeOut;
		if (fadeRunning == flag2)
		{
			return;
		}
		fadeRunning = flag2;
		if (flag2)
		{
			CreditsFadeOutAndEnd.Trigger(((Component)this).get_gameObject(), delegate
			{
				((Component)this).GetComponentInParent<Level>().respawnLocked = true;
				ClearDrag();
				hasFinishedCredits = true;
				startupDoor.SetActive(false);
				for (int j = 0; j < blocks.Count; j++)
				{
					((Component)blocks[j]).get_gameObject().SetActive(false);
				}
			});
		}
		else
		{
			CreditsFadeOutAndEnd component = ((Component)this).get_gameObject().GetComponent<CreditsFadeOutAndEnd>();
			if ((Object)(object)component != (Object)null)
			{
				component.Cancel();
			}
		}
	}

	private void AnalyzeHumans(out Human lowHuman, out Human highHuman, out Vector3 center, out Vector3 avgSpeed)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		lowHuman = null;
		highHuman = null;
		center = Vector3.get_zero();
		avgSpeed = Vector3.get_zero();
		int num = 0;
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			if (!humansInLevel.Contains(human))
			{
				continue;
			}
			num++;
			if (flag)
			{
				flag = false;
				lowHuman = human;
				highHuman = human;
				center = ((Component)human).get_transform().get_position();
				avgSpeed = human.ragdoll.partBall.rigidbody.get_velocity();
				continue;
			}
			if (((Component)human).get_transform().get_position().y < ((Component)lowHuman).get_transform().get_position().y)
			{
				lowHuman = human;
			}
			if (((Component)human).get_transform().get_position().y > ((Component)highHuman).get_transform().get_position().y)
			{
				highHuman = human;
			}
			center += ((Component)human).get_transform().get_position();
			avgSpeed = human.ragdoll.partBall.rigidbody.get_velocity();
		}
		if (num > 0)
		{
			center /= (float)num;
			avgSpeed /= (float)num;
		}
	}

	public void AlignTransform(Transform transform, Human lowHuman, Human highHuman, bool snap)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.InverseLerp(((Component)highHuman).get_transform().get_position().y - ((Component)lowHuman).get_transform().get_position().y, 0f, 20f) * 0.5f + 0.5f;
		Vector3 val = Vector3.Lerp(((Component)highHuman).get_transform().get_position(), ((Component)lowHuman).get_transform().get_position(), num);
		Vector3 position = transform.get_position();
		if (!(val.y - position.y < 1f))
		{
			float num2 = (snap ? 1f : (Mathf.InverseLerp(5f, showAhead, ((Component)lowHuman).get_transform().get_position().y - position.y) * 0.3f));
			if (num2 != 0f)
			{
				position.x = val.x;
				position.z = val.z;
				transform.set_position(Vector3.Lerp(transform.get_position(), position, num2));
				Quaternion val2 = Quaternion.Euler(0f, Mathf.LerpAngle(highHuman.controls.targetYawAngle, lowHuman.controls.targetYawAngle, num), 0f);
				transform.set_rotation(Quaternion.Lerp(transform.get_rotation(), val2, Mathf.Pow(num2, 2f)));
			}
		}
	}

	private void Spawn(Vector3 lowHumanPos, Vector3 highHumanPos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		float num = highHumanPos.y + showBehind;
		float num2 = lowHumanPos.y - showAhead;
		for (int i = 0; i < blocks.Count; i++)
		{
			if (!blocks[i].isSpawned && blocks[i].lineY > num2 && blocks[i].lineY < num)
			{
				blocks[i].SpawnLetters(lowHumanPos.SetY(blocks[i].lineY));
			}
		}
	}

	private void Despawn(Vector3 highHumanPos)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		float num = highHumanPos.y + showBehind;
		for (int i = 0; i < blocks.Count; i++)
		{
			if (blocks[i].isSpawned && blocks[i].lineY > num)
			{
				blocks[i].Despawn(num);
			}
		}
	}

	private void Align(Human lowHuman, Human highHuman)
	{
		for (int i = 0; i < blocks.Count; i++)
		{
			AlignTransform(((Component)blocks[i]).get_transform(), lowHuman, highHuman, snapAlign);
		}
		AlignTransform(startupDoor.get_transform(), lowHuman, highHuman, snapAlign);
		snapAlign = false;
	}

	public void StartNetwork(NetIdentity identity)
	{
		ParseContent();
	}

	public void CollectState(NetStream stream)
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		stream.Write((uint)state, 2);
		NetBoolEncoder.CollectState(stream, (Object)(object)ground != (Object)null && ground.isOpen);
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		state0.ReadUInt32(2);
		State state2 = (State)state1.ReadUInt32(2);
		if (state != state2)
		{
			state = state2;
			UpdateFromState();
		}
		state0.ReadBool();
		bool newState = state1.ReadBool();
		if ((Object)(object)ground != (Object)null)
		{
			ground.ChangeState(newState);
		}
	}

	public void ApplyState(NetStream state)
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		State state2 = (State)state.ReadUInt32(2);
		if (this.state != state2)
		{
			this.state = state2;
			UpdateFromState();
		}
		bool newState = state.ReadBool();
		if ((Object)(object)ground != (Object)null)
		{
			ground.ChangeState(newState);
		}
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		uint num = 0u;
		bool flag = false;
		if (state0 != null)
		{
			num = state0.ReadUInt32(2);
			flag = state0.ReadBool();
		}
		uint num2 = state1.ReadUInt32(2);
		bool flag2 = state1.ReadBool();
		if (num == num2 && flag == flag2)
		{
			delta.Write(v: false);
			return;
		}
		delta.Write(v: true);
		delta.Write(num2, 2);
		delta.Write(flag2);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		uint x = 0u;
		bool v = false;
		if (state0 != null)
		{
			x = state0.ReadUInt32(2);
			v = state0.ReadBool();
		}
		if (delta.ReadBool())
		{
			x = delta.ReadUInt32(2);
			v = delta.ReadBool();
		}
		result.Write(x, 2);
		result.Write(v);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		if (!parsed)
		{
			throw new ApplicationException("Network activity before credits initialised");
		}
		return 0 + 4;
	}

	public void SetMaster(bool isMaster)
	{
	}

	public CreditsExperience()
		: this()
	{
	}
}
