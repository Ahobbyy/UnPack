using System.Collections;
using HumanAPI;
using UnityEngine;

public class ProgressScriptMaterialChange1 : Node
{
	[Tooltip("Reference to the Render of the progress bar")]
	public MeshRenderer progressBar;

	[Tooltip("The amount of progree to show on the bar as a float of 1")]
	public NodeInput incomingProgress;

	private uint total;

	private uint lastGoal;

	private uint goal;

	private float phase;

	private float fromProgress;

	private float toProgress;

	private float currentProgress;

	private Material[] materials;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void Awake()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Awake "));
		}
		materials = ((Renderer)progressBar).get_materials();
		((Renderer)progressBar).set_sharedMaterials(materials);
	}

	private IEnumerator Start()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Start "));
		}
		if ((Object)(object)GiftService.instance != (Object)null)
		{
			GiftService.instance.RefreshStatus();
		}
		while (GiftService.status == null)
		{
			yield return null;
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

	public override void Process()
	{
		toProgress = incomingProgress.value;
		phase = 0f;
	}

	private void Sync()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		currentProgress = Mathf.Lerp(fromProgress, toProgress, Ease.easeInOutQuad(0f, 1f, phase));
		materials[1].set_mainTextureOffset(new Vector2(materials[1].get_mainTextureOffset().y, Mathf.Lerp(0.985f, 0.525f, currentProgress)));
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " CalculateMaxDeltaSizeInBits "));
		}
		return 232;
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public void ResetStateProgressBat(int checkpoint)
	{
		phase = 0f;
		fromProgress = 0f;
		toProgress = 0f;
		currentProgress = 0f;
	}

	private IEnumerator SkinUnlockChutes()
	{
		for (int i = 0; i < 100; i++)
		{
			Fireworks.instance.ShootFirework();
			yield return (object)new WaitForSeconds(Random.Range(0.05f, 0.7f));
		}
	}
}
