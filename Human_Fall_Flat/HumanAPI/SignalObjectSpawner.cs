using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	public class SignalObjectSpawner : Node, IReset
	{
		public NodeInput input;

		[Tooltip("Objects spawned will be copies of this object (Recommend you set it to inactive, objects will be activated on spawn!)")]
		public GameObject objectToDuplicate;

		[Tooltip("")]
		public Transform spawnLocation;

		public int maxSpawnedObjects = 5;

		private int nextObjToSpawn;

		private List<GameObject> spawnedObjects;

		private const string kNameSuffix = " (Managed by SignalObjectSpawner)";

		private void Awake()
		{
			spawnedObjects = new List<GameObject>();
			for (int i = 0; i < spawnLocation.get_childCount(); i++)
			{
				GameObject gameObject = ((Component)spawnLocation.GetChild(i)).get_gameObject();
				if (((Object)gameObject).get_name().Contains(" (Managed by SignalObjectSpawner)"))
				{
					spawnedObjects.Add(gameObject);
				}
			}
		}

		private void OnValidate()
		{
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Expected O, but got Unknown
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			spawnedObjects = new List<GameObject>();
			for (int i = 0; i < spawnLocation.get_childCount(); i++)
			{
				GameObject gameObject = ((Component)spawnLocation.GetChild(i)).get_gameObject();
				if (((Object)gameObject).get_name().Contains(" (Managed by SignalObjectSpawner)"))
				{
					spawnedObjects.Add(gameObject);
				}
			}
			bool flag = false;
			if (spawnedObjects.Count != maxSpawnedObjects)
			{
				flag = true;
				foreach (GameObject obj in spawnedObjects)
				{
					if (Object.op_Implicit((Object)(object)obj))
					{
						EditorApplication.delayCall = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.delayCall, (Delegate)(CallbackFunction)delegate
						{
							Object.DestroyImmediate((Object)(object)obj);
						});
					}
				}
			}
			if (flag)
			{
				spawnedObjects = new List<GameObject>();
				for (int j = 0; j < maxSpawnedObjects; j++)
				{
					GameObject item = SpawnObjectEditor();
					spawnedObjects.Add(item);
				}
			}
		}

		public override void Process()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			if (input.value > 0.5f)
			{
				spawnedObjects[nextObjToSpawn].get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
				SetObjectActive(spawnedObjects[nextObjToSpawn], active: true);
				nextObjToSpawn++;
				if (nextObjToSpawn == maxSpawnedObjects)
				{
					nextObjToSpawn = 0;
				}
			}
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			foreach (GameObject spawnedObject in spawnedObjects)
			{
				spawnedObject.get_transform().set_localPosition(new Vector3(0f, 0f, 0f));
				SetObjectActive(spawnedObject, active: false);
			}
		}

		private GameObject SpawnObjectEditor()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = Object.Instantiate<GameObject>(objectToDuplicate);
			((Object)val).set_name(((Object)val).get_name().Remove(((Object)val).get_name().Length - 7) + " (Managed by SignalObjectSpawner)");
			val.get_transform().set_parent((Transform)null);
			val.get_transform().set_localScale(Vector3.get_one());
			val.get_transform().set_parent(spawnLocation);
			val.get_transform().set_localPosition(Vector3.get_zero());
			val.get_transform().set_localRotation(Quaternion.get_identity());
			SetObjectActive(val, active: false);
			Rigidbody component = val.GetComponent<Rigidbody>();
			if (Object.op_Implicit((Object)(object)component))
			{
				component.set_isKinematic(false);
			}
			return val;
		}

		private void SetObjectActive(GameObject obj, bool active)
		{
			NetBody[] componentsInChildren = obj.GetComponentsInChildren<NetBody>();
			foreach (NetBody obj2 in componentsInChildren)
			{
				obj2.SetVisible(active);
				((Component)obj2).get_gameObject().SetActive(true);
			}
			obj.SetActive(active);
		}
	}
}
