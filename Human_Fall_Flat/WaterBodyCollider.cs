using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class WaterBodyCollider : MonoBehaviour
{
	public sealed class RespawnDelay
	{
		public NetBody respawnObject;

		public float delay;
	}

	private List<WaterSensor> sensors = new List<WaterSensor>();

	public WaterBody waterBody;

	private CameraController3 cameraController;

	private WaterSensor cameraWaterSensor;

	private Vector3 oldCameraPosition;

	private Collider[] colliders;

	[SerializeField]
	private bool respawnNetBody;

	[SerializeField]
	private float respawnNetBodyDelay = 2f;

	private const int kSizeDelayObjectCache = 16;

	private static List<RespawnDelay> delayObjects;

	private static List<RespawnDelay> objectsToRespawn;

	private static bool waterMasterSet;

	private bool waterMaster;

	private void RespawnSetup()
	{
		objectsToRespawn = new List<RespawnDelay>(16);
		delayObjects = new List<RespawnDelay>(16);
		for (int i = 0; i < 16; i++)
		{
			delayObjects.Add(new RespawnDelay());
		}
	}

	private void RespawnAdd(NetBody body)
	{
		for (int i = 0; i < objectsToRespawn.Count; i++)
		{
			if (((object)objectsToRespawn[i].respawnObject).Equals((object)body))
			{
				return;
			}
		}
		RespawnDelay respawnDelay = delayObjects[delayObjects.Count - 1];
		respawnDelay.respawnObject = body;
		respawnDelay.delay = respawnNetBodyDelay;
		objectsToRespawn.Add(respawnDelay);
		delayObjects.RemoveAt(delayObjects.Count - 1);
	}

	private void Awake()
	{
		if (!waterMasterSet)
		{
			waterMasterSet = true;
			waterMaster = true;
			RespawnSetup();
		}
	}

	private void RespawnProcess()
	{
		for (int num = objectsToRespawn.Count - 1; num >= 0; num--)
		{
			objectsToRespawn[num].delay -= Time.get_fixedDeltaTime();
			if (objectsToRespawn[num].delay < 0f)
			{
				NetBody respawnObject = objectsToRespawn[num].respawnObject;
				delayObjects.Add(objectsToRespawn[num]);
				objectsToRespawn.RemoveAt(num);
				respawnObject.Respawn();
			}
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)waterBody == (Object)null)
		{
			Debug.LogError((object)"waterbody null", (Object)(object)this);
		}
		WaterSensor component = ((Component)other).get_gameObject().GetComponent<WaterSensor>();
		if ((Object)(object)component != (Object)null)
		{
			component.OnEnterBody(waterBody);
			sensors.Add(component);
		}
		else if (respawnNetBody && !NetGame.isClient)
		{
			NetBody component2 = ((Component)other).get_gameObject().GetComponent<NetBody>();
			if ((Object)(object)component2 != (Object)null && component2.respawn && (Object)(object)((Component)other).get_gameObject().GetComponent<MeshFilter>() != (Object)null)
			{
				RespawnAdd(component2);
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		WaterSensor component = ((Component)other).get_gameObject().GetComponent<WaterSensor>();
		if ((Object)(object)component != (Object)null)
		{
			component.OnLeaveBody(waterBody);
			sensors.Remove(component);
		}
	}

	private void OnEnable()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)waterBody == (Object)null)
		{
			waterBody = ((Component)this).GetComponentInParent<WaterBody>();
		}
		if (NetGame.isClient)
		{
			cameraController = Object.FindObjectOfType<CameraController3>();
			cameraWaterSensor = ((Component)cameraController).get_gameObject().GetComponent<WaterSensor>();
			oldCameraPosition = ((Component)cameraController).get_transform().get_position();
			colliders = ((Component)this).GetComponents<Collider>();
		}
	}

	public void OnDisable()
	{
		for (int i = 0; i < sensors.Count; i++)
		{
			sensors[i].OnLeaveBody(waterBody);
		}
		sensors.Clear();
		if (NetGame.isClient)
		{
			cameraController = null;
			cameraWaterSensor = null;
			colliders = null;
		}
	}

	private void FixedUpdate()
	{
		if (waterMaster)
		{
			RespawnProcess();
		}
	}

	private void Update()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient || !((Object)(object)cameraWaterSensor != (Object)null) || !((Object)(object)waterBody != (Object)null) || colliders == null)
		{
			return;
		}
		Vector3 position = ((Component)cameraController).get_transform().get_position();
		Vector3 val = position - oldCameraPosition;
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		if (!(magnitude > 0.0001f))
		{
			return;
		}
		val = position - oldCameraPosition;
		Vector3 normalized = ((Vector3)(ref val)).get_normalized();
		Ray val2 = default(Ray);
		((Ray)(ref val2))._002Ector(oldCameraPosition, normalized);
		Ray val3 = default(Ray);
		((Ray)(ref val3))._002Ector(position, -normalized);
		bool flag = sensors.Contains(cameraWaterSensor);
		Collider[] array = colliders;
		RaycastHit val5 = default(RaycastHit);
		foreach (Collider val4 in array)
		{
			Bounds bounds = val4.get_bounds();
			bool flag2 = ((Bounds)(ref bounds)).Contains(position);
			if ((!flag2 && flag) || ((flag2 || flag) && (val4.Raycast(val2, ref val5, magnitude) || val4.Raycast(val3, ref val5, magnitude))))
			{
				if (flag)
				{
					cameraWaterSensor.OnLeaveBody(waterBody);
					sensors.Remove(cameraWaterSensor);
				}
				else
				{
					cameraWaterSensor.OnEnterBody(waterBody);
					sensors.Add(cameraWaterSensor);
				}
			}
		}
		oldCameraPosition = position;
	}

	public WaterBodyCollider()
		: this()
	{
	}
}
