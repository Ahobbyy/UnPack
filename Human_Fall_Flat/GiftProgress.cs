using System;
using System.Collections;
using I2.Loc;
using Multiplayer;
using UnityEngine;

public class GiftProgress : MonoBehaviour, INetBehavior
{
	public SkinnedDoll doll;

	private Quaternion startRot;

	public Transform[] digits;

	public MeshRenderer progressBar;

	private uint total;

	private uint lastGoal;

	private uint goal;

	private string prize = "";

	private float[] from = new float[6];

	private float[] to = new float[6];

	private float[] current = new float[6];

	private float phase;

	private float fromProgress;

	private float toProgress;

	private float currentProgress;

	public bool lobbyGuns = true;

	public bool gunsUnlocked;

	private Material[] materials;

	private string lockedPrize;

	private void Awake()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		startRot = digits[0].get_localRotation();
		materials = ((Renderer)progressBar).get_materials();
		((Renderer)progressBar).set_sharedMaterials(materials);
	}

	private IEnumerator Start()
	{
		if (!NetGame.isServer)
		{
			lobbyGuns = true;
			yield break;
		}
		if ((Object)(object)GiftService.instance != (Object)null)
		{
			GiftService.instance.RefreshStatus();
		}
		while (GiftService.status == null)
		{
			yield return null;
		}
		GiftService_statusUpdated(GiftService.status);
		GiftService.statusUpdated += GiftService_statusUpdated;
		yield return null;
		yield return null;
		if (gunsUnlocked)
		{
			NetChat.RegisterCommand(server: true, client: false, "bang", LobbyGuns, ScriptLocalization.Get("XTRA/gunsHelp"));
			lobbyGuns = PlayerPrefs.GetInt("lobbyGuns", 1) > 0;
			NetChat.serverCommands.OnHelp("bang");
		}
		while (true)
		{
			yield return (object)new WaitForSeconds((float)Random.Range(30, 90));
			if ((Object)(object)GiftService.instance != (Object)null)
			{
				GiftService.instance.RefreshStatus();
			}
		}
	}

	private void OnDisable()
	{
		NetChat.UnRegisterCommand(server: true, client: false, "bang", LobbyGuns, ScriptLocalization.Get("XTRA/gunsHelp"));
	}

	private void LobbyGuns()
	{
		lobbyGuns = !lobbyGuns;
		if (lobbyGuns)
		{
			NetChat.Print(ScriptLocalization.Get("XTRA/gunsOn"));
		}
		else
		{
			NetChat.Print(ScriptLocalization.Get("XTRA/gunsOff"));
		}
		PlayerPrefs.SetInt("lobbyGuns", lobbyGuns ? 1 : 0);
		SetValue(total, goal, lastGoal, prize);
	}

	private void OnDestroy()
	{
		GiftService.statusUpdated -= GiftService_statusUpdated;
	}

	private void GiftService_statusUpdated(XmasStatus status)
	{
		SetValue(status.total, status.goal, status.last_goal, status.prize);
	}

	private void SetValue(uint newTotal, uint newGoal, uint newLastGoal, string newPrize)
	{
		if (newTotal < newGoal)
		{
			lockedPrize = newPrize;
		}
		if (newPrize != prize)
		{
			prize = newPrize;
			doll.ApplySkin(prize);
		}
		if (newGoal - newLastGoal < 0)
		{
			newGoal = (newLastGoal = 0u);
		}
		if (newTotal != total || newGoal != goal || newLastGoal != lastGoal)
		{
			total = newTotal;
			goal = newGoal;
			lastGoal = newLastGoal;
			int num = 100000;
			phase = 0f;
			for (int i = 0; i < 6; i++)
			{
				from[i] = current[i];
				to[i] = (long)total / (long)num;
				num /= 10;
			}
			float num2 = ((goal != 0) ? Mathf.InverseLerp((float)lastGoal, (float)goal, (float)total) : 0f);
			fromProgress = currentProgress;
			toProgress = num2;
			bool flag = total < goal;
			if (doll.nailed != flag)
			{
				doll.Nail(flag);
				if (!NetGame.isClient && !ReplayRecorder.isPlaying && !flag && string.Equals(prize, lockedPrize))
				{
					((MonoBehaviour)this).StartCoroutine(SkinUnlockChutes());
				}
			}
		}
		gunsUnlocked = newTotal >= newGoal && "Guns".Equals(prize, StringComparison.InvariantCultureIgnoreCase);
		if ((Object)(object)Fireworks.instance != (Object)null)
		{
			Fireworks.instance.enableWeapons = gunsUnlocked && lobbyGuns;
		}
	}

	private void Update()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		materials[1].set_mainTextureOffset(materials[1].get_mainTextureOffset() + new Vector2(Time.get_deltaTime() / 10f, 0f));
		if (phase < 1f)
		{
			phase = Mathf.MoveTowards(phase, 1f, Time.get_deltaTime() / 2f);
			Sync();
		}
	}

	private void Sync()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		currentProgress = Mathf.Lerp(fromProgress, toProgress, Ease.easeInOutQuad(0f, 1f, phase));
		materials[1].set_mainTextureOffset(new Vector2(materials[1].get_mainTextureOffset().y, Mathf.Lerp(0.985f, 0.525f, currentProgress)));
		for (int i = 0; i < 6; i++)
		{
			current[i] = Mathf.Lerp(from[i], to[i], Ease.easeInOutQuad(0f, 1f, phase));
			float num = current[i] * 360f / 10f;
			digits[i].set_localRotation(startRot * Quaternion.Euler(0f, 0f, 0f - num));
		}
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public void CollectState(NetStream stream)
	{
		stream.Write(total, 24);
		stream.Write(goal, 24);
		stream.Write(lastGoal, 24);
		if ("Guns".Equals(prize, StringComparison.InvariantCultureIgnoreCase) && !lobbyGuns)
		{
			stream.Write("GunsLocked");
		}
		else
		{
			stream.Write(prize);
		}
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		if (state0 != null)
		{
			state0.ReadUInt32(24);
			state0.ReadUInt32(24);
			state0.ReadUInt32(24);
			state0.ReadString();
		}
		ApplyState(state1);
	}

	public void ApplyState(NetStream state)
	{
		uint newTotal = state.ReadUInt32(24);
		uint newGoal = state.ReadUInt32(24);
		uint newLastGoal = state.ReadUInt32(24);
		string newPrize = state.ReadString();
		SetValue(newTotal, newGoal, newLastGoal, newPrize);
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		uint num = state0?.ReadUInt32(24) ?? 0;
		uint num2 = state0?.ReadUInt32(24) ?? 0;
		uint num3 = state0?.ReadUInt32(24) ?? 0;
		string a = ((state0 == null) ? "" : state0.ReadString());
		uint num4 = state1.ReadUInt32(24);
		uint num5 = state1.ReadUInt32(24);
		uint num6 = state1.ReadUInt32(24);
		string text = state1.ReadString();
		if (num == num4 && num2 == num5 && num3 == num6 && string.Equals(a, text))
		{
			delta.Write(v: false);
			return;
		}
		delta.Write(v: true);
		delta.Write(num4, 24);
		delta.Write(num5, 24);
		delta.Write(num6, 24);
		delta.Write(text);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		uint x = state0?.ReadUInt32(24) ?? 0;
		uint x2 = state0?.ReadUInt32(24) ?? 0;
		uint x3 = state0?.ReadUInt32(24) ?? 0;
		string text = ((state0 == null) ? "" : state0.ReadString());
		if (delta.ReadBool())
		{
			x = delta.ReadUInt32(24);
			x2 = delta.ReadUInt32(24);
			x3 = delta.ReadUInt32(24);
			text = delta.ReadString();
		}
		result.Write(x, 24);
		result.Write(x2, 24);
		result.Write(x3, 24);
		result.Write(text);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return 232;
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	private IEnumerator SkinUnlockChutes()
	{
		for (int i = 0; i < 100; i++)
		{
			Fireworks.instance.ShootFirework();
			yield return (object)new WaitForSeconds(Random.Range(0.05f, 0.7f));
		}
	}

	public GiftProgress()
		: this()
	{
	}
}
