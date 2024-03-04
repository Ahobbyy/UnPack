using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class FlammableScriptHeatColourChange1 : MonoBehaviour, INetBehavior
{
	[Tooltip("How Red you would like the coal to get in colour")]
	public Color coldColor = Color.get_red();

	[Tooltip("How Rellow you would like the coal to get in colour")]
	public Color hotColor = Color.get_yellow();

	[Tooltip("What Yellow colour you would like to emit")]
	public Color emitColor = Color.get_yellow();

	[Tooltip("How intense the emittion should be")]
	public float emitIntensity = 2f;

	[Tooltip(" How long it will take this thing to heat up")]
	public float heatUpTime = 1f;

	[Tooltip("How long it will take for this thing to cool down")]
	public float coolDownTime = 3f;

	private Flammable flammbleSettings;

	private float glowFrequency = 6f;

	private float colorFrequency = 6f;

	private float heat = 1E-06f;

	private float targetHeat;

	private Material material;

	[Tooltip("Use this to see the messages coming from the script ")]
	public bool showDebug;

	private static Coroutine update;

	private static List<FlammableScriptHeatColourChange1> all = new List<FlammableScriptHeatColourChange1>();

	private void OnEnable()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " OnEnable "));
		}
		all.Add(this);
		if (update == null)
		{
			update = Coroutines.StartGlobalCoroutine(ProcessUpdates());
		}
		flammbleSettings = ((Component)this).GetComponent<Flammable>();
	}

	private void OnDisable()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " onDisable "));
		}
		all.Remove(this);
	}

	private void Awake()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Awake "));
		}
		MeshRenderer component = ((Component)this).GetComponent<MeshRenderer>();
		material = ((Renderer)component).get_material();
		((Renderer)component).set_sharedMaterial(material);
		glowFrequency = Random.Range(8, 25);
		colorFrequency = Random.Range(5, 10);
		UpdateInternal();
	}

	public static IEnumerator ProcessUpdates()
	{
		while (all.Count > 0)
		{
			yield return null;
			for (int i = 0; i < all.Count; i++)
			{
				all[i].UpdateInternal();
			}
		}
		Coroutines.StopGlobalCoroutine(update);
		update = null;
	}

	private void UpdateInternal()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		if (heat != targetHeat)
		{
			heat = Mathf.MoveTowards(heat, targetHeat, Time.get_deltaTime() / ((targetHeat > heat) ? heatUpTime : coolDownTime));
			Color color = Color.get_black();
			float num = 0f;
			if (heat > 0f)
			{
				num = heat * emitIntensity * (1f + 0.1f * Mathf.Sin(Time.get_time() / glowFrequency * (float)Math.PI * 2f));
				color = heat * Color.Lerp(coldColor, hotColor, heat + 0.1f * Mathf.Sin(Time.get_time() / colorFrequency * (float)Math.PI * 2f));
				flammbleSettings.heat = heat;
			}
			material.SetColor("_EmissionColor", emitColor * num);
			material.set_color(color);
		}
	}

	public void Ignite()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Ignite "));
		}
		targetHeat = 0.9f;
	}

	public void Extinguish()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Extinguish "));
		}
		targetHeat = 0f;
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public void CollectState(NetStream stream)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " CollectState "));
		}
		NetBoolEncoder.CollectState(stream, targetHeat > 0f);
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " ApplyLerpedState "));
		}
		targetHeat = (NetBoolEncoder.ApplyLerpedState(state0, state1, mix) ? 0.9f : 0f);
	}

	public void ApplyState(NetStream state)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " ApplyState "));
		}
		targetHeat = (NetBoolEncoder.ApplyState(state) ? 0.9f : 0f);
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " CalculateDelta "));
		}
		NetBoolEncoder.CalculateDelta(state0, state1, delta);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " AddDelta "));
		}
		NetBoolEncoder.AddDelta(state0, delta, result);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " CalculateMaxDeltaSizeInBits "));
		}
		return NetBoolEncoder.CalculateMaxDeltaSizeInBits();
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public FlammableScriptHeatColourChange1()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_0017: Unknown result type (might be due to invalid IL or missing references)
	//IL_001c: Unknown result type (might be due to invalid IL or missing references)

}
