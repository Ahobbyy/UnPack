using UnityEngine;

public class ParticleController : MonoBehaviour
{
	private ParticleSystem particleSys;

	private void Awake()
	{
		particleSys = ((Component)this).GetComponent<ParticleSystem>();
	}

	public void StartSystem()
	{
		particleSys.Play();
	}

	public void StopSystem()
	{
		particleSys.Stop();
	}

	public ParticleController()
		: this()
	{
	}
}
