using HumanAPI;
using UnityEngine;

public class WaterHose : Node
{
	public NodeInput activate;

	public GameObject waterBodyGroup;

	public float emitDelay = 0.5f;

	public float particleLifespan = 1f;

	public Vector3 forceVector = new Vector3(0f, 0f, 5f);

	public Vector3 emitOffset = new Vector3(0f, 0f, 0.5f);

	public LineRenderer line;

	private Rigidbody[] waterBodies;

	private WaterJetParticle[] waterJetParticles;

	private float emitTimer;

	private bool isEmitting;

	private int nextEmitIndex;

	private void DisableParticle(int i)
	{
		((Component)waterBodies[i]).get_gameObject().SetActive(false);
	}

	private void Start()
	{
		if ((Object)(object)waterBodyGroup != (Object)null)
		{
			waterBodies = waterBodyGroup.GetComponentsInChildren<Rigidbody>(true);
		}
		waterJetParticles = new WaterJetParticle[waterBodies.Length];
		for (int i = 0; i < waterBodies.Length; i++)
		{
			DisableParticle(i);
			waterJetParticles[i] = ((Component)waterBodies[i]).GetComponent<WaterJetParticle>();
			if ((Object)(object)waterJetParticles[i] == (Object)null)
			{
				Debug.LogError((object)"Water body missing WaterJetParticle component", (Object)(object)((Component)waterBodies[i]).get_gameObject());
			}
		}
		line.set_positionCount(waterBodies.Length + 1);
		line.set_useWorldSpace(true);
		emitTimer = 0f;
		nextEmitIndex = 0;
		isEmitting = false;
	}

	private void EnableParticle(int i)
	{
		((Component)waterBodies[i]).get_gameObject().SetActive(true);
	}

	private void Update()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		if (activate.value >= 0.5f)
		{
			isEmitting = true;
		}
		else
		{
			isEmitting = false;
		}
		if (!isEmitting)
		{
			return;
		}
		emitTimer += Time.get_deltaTime();
		while (emitTimer >= emitDelay)
		{
			EnableParticle(nextEmitIndex);
			((Component)waterBodies[nextEmitIndex]).get_transform().set_position(((Component)this).get_transform().TransformPoint(emitOffset));
			((Component)waterBodies[nextEmitIndex]).get_transform().set_rotation(((Component)this).get_transform().get_rotation());
			waterBodies[nextEmitIndex].set_velocity(new Vector3(0f, 0f, 0f));
			waterBodies[nextEmitIndex].AddForce(((Component)this).get_transform().TransformDirection(forceVector));
			waterJetParticles[nextEmitIndex].particleAge = 0f;
			nextEmitIndex++;
			if (nextEmitIndex >= waterBodies.Length)
			{
				nextEmitIndex = 0;
			}
			emitTimer -= emitDelay;
		}
	}

	private void LateUpdate()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((!isEmitting) ? line.GetPosition(1) : ((Component)this).get_transform().TransformPoint(emitOffset));
		line.SetPosition(0, val);
		for (int i = 0; i < waterBodies.Length; i++)
		{
			int num = (waterBodies.Length - i + nextEmitIndex - 1) % waterBodies.Length;
			if (waterJetParticles[num].particleAge > particleLifespan)
			{
				DisableParticle(num);
			}
			if (((Component)waterBodies[num]).get_gameObject().get_activeSelf())
			{
				val = ((Component)waterBodies[num]).get_transform().get_position();
			}
			line.SetPosition(i + 1, val);
		}
	}
}
