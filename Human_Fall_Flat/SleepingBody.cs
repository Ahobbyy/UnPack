using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class SleepingBody : MonoBehaviour, IPostReset
{
	private struct BodyState
	{
		public Rigidbody body;

		public Quaternion rotation;

		public Vector3 position;

		public int framesFrozen;
	}

	public Transform[] bodyParents;

	private BodyState[] states;

	private Rigidbody[] bodies;

	private Quaternion[] rotations;

	private Vector3[] positions;

	private SleepingBodyParameterOverride[] overrides;

	public bool putToSleep;

	public bool forceSleep;

	public bool hasAwake;

	public float angularFriction = 0.02f;

	public float linearFriction = 0.1f;

	public float angularTreshold = 10f;

	public float linearTreshold = 0.03f;

	public bool sleepOnAwake;

	private int skipFrames = 10;

	private int frozenFrames;

	public float highAngular;

	public float highLinear;

	private bool reported;

	private void Start()
	{
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if (bodyParents == null || bodyParents.Length == 0)
		{
			bodies = ((Component)this).GetComponentsInChildren<Rigidbody>();
		}
		else
		{
			List<Rigidbody> list = new List<Rigidbody>();
			for (int i = 0; i < bodyParents.Length; i++)
			{
				list.AddRange(((Component)bodyParents[i]).GetComponentsInChildren<Rigidbody>());
			}
			bodies = list.ToArray();
		}
		rotations = (Quaternion[])(object)new Quaternion[bodies.Length];
		positions = (Vector3[])(object)new Vector3[bodies.Length];
		states = new BodyState[bodies.Length];
		overrides = new SleepingBodyParameterOverride[bodies.Length];
		for (int j = 0; j < bodies.Length; j++)
		{
			_ = bodies[j];
			positions[j] = bodies[j].get_position();
			rotations[j] = bodies[j].get_rotation();
			states[j].body = bodies[j];
			overrides[j] = ((Component)bodies[j]).GetComponent<SleepingBodyParameterOverride>();
		}
		PostResetState(0);
	}

	public void FixedUpdate()
	{
	}

	public void PostResetState(int checkpoint)
	{
		if (sleepOnAwake)
		{
			for (int i = 0; i < bodies.Length; i++)
			{
				bodies[i].Sleep();
			}
			putToSleep = true;
			skipFrames = 60;
		}
	}

	public SleepingBody()
		: this()
	{
	}
}
