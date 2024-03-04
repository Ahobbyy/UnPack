using System.Collections;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;
using UnityEngine.Profiling;

public class Respawnable : MonoBehaviour
{
	[Tooltip("Use this to respawn to a specific place in stead of its original start position.")]
	public bool respawnToSpecificLocation;

	public Transform resetPos;

	[Space]
	private Vector3 startPos;

	private Quaternion startRot;

	public float despawnHeight = -50f;

	public float respawnHeight;

	private static Coroutine update;

	private static List<Respawnable> all = new List<Respawnable>();

	private void OnDisable()
	{
		all.Remove(this);
	}

	private void OnEnable()
	{
		all.Add(this);
		if (update == null)
		{
			update = Coroutines.StartGlobalCoroutine(ProcessUpdates());
		}
	}

	private void Awake()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!respawnToSpecificLocation)
		{
			startPos = ((Component)this).get_transform().get_position();
			startRot = ((Component)this).get_transform().get_rotation();
		}
	}

	public static IEnumerator ProcessUpdates()
	{
		int throttle = 0;
		while (all.Count > 0)
		{
			yield return null;
			Profiler.BeginSample("Respawnable.Update");
			for (int i = 0; i < all.Count; i++)
			{
				if (throttle++ == 50)
				{
					throttle = 0;
					Profiler.EndSample();
					yield return null;
					Profiler.BeginSample("Respawnable.Update");
				}
				if (i < all.Count)
				{
					all[i].UpdateInternal();
				}
			}
			Profiler.EndSample();
		}
		Coroutines.StopGlobalCoroutine(update);
		update = null;
	}

	private void UpdateInternal()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).get_transform().get_position().y < despawnHeight)
		{
			Respawn();
		}
	}

	public void Respawn()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		if (ReplayRecorder.isPlaying || NetGame.isClient || GrabManager.IsGrabbedAny(((Component)this).get_gameObject()))
		{
			return;
		}
		RestartableRigid component = ((Component)this).GetComponent<RestartableRigid>();
		if ((Object)(object)component != (Object)null)
		{
			component.Reset(Vector3.get_up() * respawnHeight);
			return;
		}
		if (!respawnToSpecificLocation)
		{
			((Component)this).get_transform().set_position(startPos + Vector3.get_up() * respawnHeight);
			((Component)this).get_transform().set_rotation(startRot);
		}
		else
		{
			((Component)this).get_transform().set_position(resetPos.get_position());
		}
		Rigidbody component2 = ((Component)this).GetComponent<Rigidbody>();
		if ((Object)(object)component2 != (Object)null)
		{
			component2.set_velocity(Vector3.get_zero());
			component2.set_angularVelocity(Vector3.get_zero());
		}
	}

	public Respawnable()
		: this()
	{
	}
}
