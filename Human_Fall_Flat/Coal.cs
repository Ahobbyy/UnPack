using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class Coal : MonoBehaviour, INetBehavior
{
	public Color coldColor = Color.get_red();

	public Color hotColor = Color.get_yellow();

	public Color emitColor = Color.get_yellow();

	public float emitIntensity = 2f;

	public float heatUpTime = 1f;

	public float coolDownTime = 3f;

	private float glowFrequency = 6f;

	private float colorFrequency = 6f;

	private float heat = 1E-06f;

	private float targetHeat;

	private Material material;

	private static Coroutine update;

	private static List<Coal> all = new List<Coal>();

	private void OnEnable()
	{
		all.Add(this);
		if (update == null)
		{
			update = Coroutines.StartGlobalCoroutine(ProcessUpdates());
		}
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	private void Awake()
	{
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
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		if (heat != targetHeat)
		{
			heat = Mathf.MoveTowards(heat, targetHeat, Time.get_deltaTime() / ((targetHeat > heat) ? heatUpTime : coolDownTime));
			Color color = Color.get_black();
			float num = 0f;
			if (heat > 0f)
			{
				num = heat * emitIntensity * (1f + 0.1f * Mathf.Sin(Time.get_time() / glowFrequency * (float)Math.PI * 2f));
				color = heat * Color.Lerp(coldColor, hotColor, heat + 0.1f * Mathf.Sin(Time.get_time() / colorFrequency * (float)Math.PI * 2f));
			}
			material.SetColor("_EmissionColor", emitColor * num);
			material.set_color(color);
		}
	}

	internal void Ignite()
	{
		targetHeat = 0.9f;
	}

	internal void Extinguish(bool instant = false)
	{
		targetHeat = 0f;
		if (instant)
		{
			heat = 0.01f;
		}
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public void CollectState(NetStream stream)
	{
		NetBoolEncoder.CollectState(stream, targetHeat > 0f);
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		targetHeat = (NetBoolEncoder.ApplyLerpedState(state0, state1, mix) ? 0.9f : 0f);
	}

	public void ApplyState(NetStream state)
	{
		targetHeat = (NetBoolEncoder.ApplyState(state) ? 0.9f : 0f);
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		NetBoolEncoder.CalculateDelta(state0, state1, delta);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		NetBoolEncoder.AddDelta(state0, delta, result);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return NetBoolEncoder.CalculateMaxDeltaSizeInBits();
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public Coal()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_0017: Unknown result type (might be due to invalid IL or missing references)
	//IL_001c: Unknown result type (might be due to invalid IL or missing references)

}
