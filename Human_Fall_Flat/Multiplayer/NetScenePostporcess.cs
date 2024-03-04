using System.Collections.Generic;
using System.Linq;
using System.Text;
using HumanAPI;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Multiplayer
{
	public static class NetScenePostporcess
	{
		private static StringBuilder hashBuilder;

		private static int id = 1;

		[PostProcessScene]
		public static void OnPostProcessScene()
		{
			if (!CheckNetIds(report: true))
			{
				Debug.LogErrorFormat("Scene contains wrong net scene ids", new object[0]);
			}
		}

		public static void AssignIds()
		{
			id = 1;
			Undo.RecordObjects(Resources.FindObjectsOfTypeAll(typeof(NetIdentity)).ToArray(), "assignIds");
			hashBuilder = new StringBuilder();
			NetScope[] array = Object.FindObjectsOfType<NetScope>();
			for (int i = 0; i < array.Length; i++)
			{
				AssignRecursive(((Component)array[i]).get_transform());
			}
			Level level = Object.FindObjectOfType<Level>();
			if ((Object)(object)level != (Object)null)
			{
				Undo.RecordObject((Object)(object)level, "Set level hash");
				level.netHash = (uint)Animator.StringToHash(hashBuilder.ToString()) & 0xFFFFFFFFu;
			}
			NetChecksAsTheGameDoesThem(selectFirstErrorObject: false);
		}

		private static void AssignRecursive(Transform t)
		{
			NetIdentity component = ((Component)t).GetComponent<NetIdentity>();
			if ((Object)(object)component != (Object)null)
			{
				component.sceneId = (uint)id++;
				hashBuilder.Append(((Object)component).get_name());
				INetBehavior[] components = ((Component)component).GetComponents<INetBehavior>();
				foreach (INetBehavior netBehavior in components)
				{
					hashBuilder.Append(netBehavior.GetType().Name);
				}
			}
			for (int j = 0; j < t.get_childCount(); j++)
			{
				if ((Object)(object)((Component)t.GetChild(j)).GetComponent<NetScope>() == (Object)null)
				{
					AssignRecursive(t.GetChild(j));
				}
			}
		}

		public static void CheckIds()
		{
			CheckNetSceneManager();
			CheckNetIds(report: true);
			NetChecksAsTheGameDoesThem(selectFirstErrorObject: true);
		}

		public static bool CheckNetSceneManager()
		{
			bool result = true;
			Level[] array = Object.FindObjectsOfType<Level>();
			if (array.Length > 1)
			{
				Debug.LogError((object)"Multiple Level components found in scene");
				result = false;
			}
			Level[] array2 = array;
			foreach (Level level in array2)
			{
				if (!Object.op_Implicit((Object)(object)((Component)level).get_gameObject().GetComponent<NetSceneManager>()))
				{
					Debug.LogError((object)("No NetSceneManager component found in " + ((Object)((Component)level).get_gameObject()).get_name()), (Object)(object)((Component)level).get_gameObject());
					result = false;
				}
			}
			return result;
		}

		public static bool CheckNetIds(bool report)
		{
			bool valid = true;
			NetScope[] array = Object.FindObjectsOfType<NetScope>();
			if (array.Length == 0 && (Object)(object)Object.FindObjectOfType<Level>() != (Object)null)
			{
				Debug.Log((object)"No NetScope found. Missing NetScene?");
			}
			NetScope[] array2 = array;
			foreach (NetScope obj in array2)
			{
				CheckRecursive(list: new List<uint>(), t: ((Component)obj).get_transform(), report: report, valid: ref valid);
			}
			return valid;
		}

		public static void NetChecksAsTheGameDoesThem(bool selectFirstErrorObject)
		{
			uint num = 1054u;
			NetScope[] array = Object.FindObjectsOfType<NetScope>();
			foreach (NetScope netScope in array)
			{
				if ((Object)(object)((Component)netScope).get_gameObject() == (Object)null)
				{
					Debug.LogErrorFormat("Found a NetScope without a gameObject: type={0}", new object[1] { ((object)netScope).GetType().Name });
					continue;
				}
				Dictionary<uint, GameObject> dictionary = new Dictionary<uint, GameObject>();
				HashSet<GameObject> hashSet = new HashSet<GameObject>();
				NetIdentity[] componentsInChildren = ((Component)netScope).get_gameObject().GetComponentsInChildren<NetIdentity>(true);
				foreach (NetIdentity netIdentity in componentsInChildren)
				{
					if (!((Object)(object)((Component)netIdentity).GetComponentInParent<NetScope>() == (Object)(object)netScope))
					{
						continue;
					}
					if ((Object)(object)((Component)netIdentity).get_gameObject() == (Object)null)
					{
						Debug.LogErrorFormat("Found a NetIdentity that's not bound to a GameObject", new object[0]);
						continue;
					}
					if (hashSet.Contains(((Component)netIdentity).get_gameObject()))
					{
						Debug.LogErrorFormat((Object)(object)((Component)netIdentity).get_gameObject(), "Object {0} has multiple NetIdentities!!", new object[1] { ((Object)((Component)netIdentity).get_gameObject()).get_name() });
						if (selectFirstErrorObject)
						{
							selectFirstErrorObject = false;
							Selection.set_activeGameObject(((Component)netIdentity).get_gameObject());
						}
						continue;
					}
					hashSet.Add(((Component)netIdentity).get_gameObject());
					if (netIdentity.sceneId == 0 || netIdentity.sceneId > num)
					{
						Debug.LogErrorFormat((Object)(object)netIdentity, "Found an object with an invalid net id ({0}) : {1}  [runtime equivalent search]", new object[2]
						{
							netIdentity.sceneId,
							((Object)((Component)netIdentity).get_gameObject()).get_name()
						});
						if (selectFirstErrorObject)
						{
							selectFirstErrorObject = false;
							Selection.set_activeGameObject(((Component)netIdentity).get_gameObject());
						}
					}
					else if (dictionary.ContainsKey(netIdentity.sceneId))
					{
						Debug.LogErrorFormat((Object)(object)netIdentity, "Found objects with the same net id ({0}) : {1} and {2}  [runtime equivalent search]", new object[3]
						{
							netIdentity.sceneId,
							((Object)dictionary[netIdentity.sceneId]).get_name(),
							((Object)((Component)netIdentity).get_gameObject()).get_name()
						});
						if (selectFirstErrorObject)
						{
							selectFirstErrorObject = false;
							Selection.set_activeGameObject(((Component)netIdentity).get_gameObject());
						}
					}
					else
					{
						dictionary[netIdentity.sceneId] = ((Component)netIdentity).get_gameObject();
					}
				}
			}
		}

		private static void CheckRecursive(Transform t, List<uint> list, bool report, ref bool valid)
		{
			NetIdentity component = ((Component)t).GetComponent<NetIdentity>();
			if ((Object)(object)component != (Object)null)
			{
				if (component.sceneId == 0)
				{
					if (report)
					{
						Debug.LogFormat((Object)(object)component, "Missing sceneId {0}", new object[1] { component });
					}
					valid = false;
				}
				else if (list.Contains(component.sceneId))
				{
					if (report)
					{
						Debug.LogFormat((Object)(object)component, "Duplicate sceneId {0}", new object[1] { component });
					}
					valid = false;
				}
				else
				{
					list.Add(component.sceneId);
				}
			}
			for (int i = 0; i < t.get_childCount(); i++)
			{
				Transform child = t.GetChild(i);
				if ((Object)(object)((Component)child).GetComponent<NetScope>() == (Object)null)
				{
					CheckRecursive(child, list, report, ref valid);
				}
			}
		}

		private static Component GetComponentInParentNonRecursive<T>(GameObject gameObject) where T : Component
		{
			Component val = (Component)(object)gameObject.GetComponent<T>();
			if (Object.op_Implicit((Object)(object)val))
			{
				return val;
			}
			if ((Object)(object)gameObject.get_transform().get_parent() != (Object)null)
			{
				val = (Component)(object)((Component)gameObject.get_transform().get_parent()).get_gameObject().GetComponent<T>();
			}
			if (Object.op_Implicit((Object)(object)val))
			{
				return val;
			}
			return null;
		}

		public static void CheckNetBodyNonKinematic()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Object[] array = Object.FindObjectsOfType(typeof(Rigidbody));
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody val = (Rigidbody)array[i];
				if (!Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetBody>(((Component)val).get_gameObject())) && !val.get_isKinematic() && !Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetIdentity>(((Component)val).get_gameObject())))
				{
					Debug.Log((object)("Missing NetBody : " + ((Object)((Component)val).get_gameObject()).get_name()), (Object)(object)((Component)val).get_gameObject());
				}
			}
		}

		public static void CheckNetBodyAll()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Object[] array = Object.FindObjectsOfType(typeof(Rigidbody));
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody val = (Rigidbody)array[i];
				if (!Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetBody>(((Component)val).get_gameObject())) && !Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetIdentity>(((Component)val).get_gameObject())))
				{
					Debug.Log((object)("Missing NetBody : " + ((Object)((Component)val).get_gameObject()).get_name()), (Object)(object)((Component)val).get_gameObject());
				}
			}
		}

		public static void CheckNetBodyNonKinematicStrict()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Object[] array = Object.FindObjectsOfType(typeof(Rigidbody));
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody val = (Rigidbody)array[i];
				if (!Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetBody>(((Component)val).get_gameObject())) || (!val.get_isKinematic() && !Object.op_Implicit((Object)(object)NetScenePostporcess.GetComponentInParentNonRecursive<NetIdentity>(((Component)val).get_gameObject()))))
				{
					Debug.Log((object)("Missing Net objects : " + ((Object)((Component)val).get_gameObject()).get_name()), (Object)(object)((Component)val).get_gameObject());
				}
			}
		}

		public static void CheckNetBodyAllStrict()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			Object[] array = Object.FindObjectsOfType(typeof(Rigidbody));
			for (int i = 0; i < array.Length; i++)
			{
				Rigidbody val = (Rigidbody)array[i];
				if (!Object.op_Implicit((Object)(object)((Component)val).get_gameObject().GetComponent<NetBody>()) || !Object.op_Implicit((Object)(object)((Component)val).get_gameObject().GetComponent<NetIdentity>()))
				{
					Debug.Log((object)("Missing Net objects : " + ((Object)((Component)val).get_gameObject()).get_name()), (Object)(object)((Component)val).get_gameObject());
				}
			}
		}

		public static void CheckNetBodyHasRigidBody()
		{
			NetBody[] array = Object.FindObjectsOfType<NetBody>();
			foreach (NetBody netBody in array)
			{
				if ((Object)(object)((Component)netBody).get_gameObject().GetComponent<Rigidbody>() == (Object)null)
				{
					Debug.Log((object)("Object '" + ((Object)netBody).get_name() + "' has NetBody but no RigidBody"), (Object)(object)((Component)netBody).get_gameObject());
				}
			}
		}

		public static void DisableParticleSystems()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			GameObject[] gameObjects = Selection.get_gameObjects();
			for (int i = 0; i < gameObjects.Length; i++)
			{
				ParticleSystem[] componentsInChildren = gameObjects[i].GetComponentsInChildren<ParticleSystem>();
				foreach (ParticleSystem obj in componentsInChildren)
				{
					obj.Stop();
					EmissionModule emission = obj.get_emission();
					((EmissionModule)(ref emission)).set_enabled(false);
					MainModule main = obj.get_main();
					((MainModule)(ref main)).set_playOnAwake(false);
				}
			}
		}

		public static void CheckDuplicateNetBody()
		{
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			List<GameObject> list = new List<GameObject>();
			Object[] array = Object.FindObjectsOfType(typeof(NetBody));
			for (int i = 0; i < array.Length; i++)
			{
				NetBody netBody = (NetBody)(object)array[i];
				if (!list.Contains(((Component)netBody).get_gameObject()))
				{
					list.Add(((Component)netBody).get_gameObject());
					if (((Component)netBody).GetComponents<NetBody>().Length > 1)
					{
						Debug.Log((object)("Duplicate NetBody : " + ((Object)((Component)netBody).get_gameObject()).get_name()), (Object)(object)((Component)netBody).get_gameObject());
					}
				}
			}
			new List<GameObject>();
			array = Object.FindObjectsOfType(typeof(TriggerVolume));
			for (int i = 0; i < array.Length; i++)
			{
				TriggerVolume triggerVolume = (TriggerVolume)(object)array[i];
				Debug.Log((object)("trigger : " + ((Object)((Component)triggerVolume).get_gameObject()).get_name()), (Object)(object)((Component)triggerVolume).get_gameObject());
				if (!Object.op_Implicit((Object)(object)((Component)triggerVolume).GetComponent<NetIdentity>()))
				{
					Debug.Log((object)("Missing NetIdentity : " + ((Object)((Component)triggerVolume).get_gameObject()).get_name()), (Object)(object)((Component)triggerVolume).get_gameObject());
					((Component)triggerVolume).get_gameObject().AddComponent<NetIdentity>();
				}
				SerializedObject val = new SerializedObject((Object)(object)triggerVolume);
				val.FindProperty("trackColliders").set_boolValue(false);
				val.ApplyModifiedProperties();
			}
		}
	}
}
