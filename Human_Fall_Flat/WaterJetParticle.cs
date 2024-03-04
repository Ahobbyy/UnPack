using System;
using UnityEngine;

public class WaterJetParticle : MonoBehaviour
{
	[NonSerialized]
	public float particleAge;

	private void Start()
	{
	}

	private void Update()
	{
		particleAge += Time.get_deltaTime();
	}

	public WaterJetParticle()
		: this()
	{
	}
}
