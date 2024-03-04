using System;
using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightPool : MonoBehaviour
	{
		public Dictionary<Type, GameObject> prefabs;

		private Dictionary<Type, Stack<LightBase>> pool;

		private static LightPool _instance;

		private static LightPool Instance
		{
			get
			{
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				if ((Object)(object)_instance == (Object)null)
				{
					new GameObject("Light pool", new Type[1] { typeof(LightPool) });
				}
				return _instance;
			}
		}

		private void Awake()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Expected O, but got Unknown
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Expected O, but got Unknown
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = this;
			}
			else if ((Object)(object)_instance != (Object)(object)this)
			{
				Object.Destroy((Object)(object)this);
			}
			pool = new Dictionary<Type, Stack<LightBase>>();
			prefabs = new Dictionary<Type, GameObject>();
			prefabs.Add(typeof(LightBeam), (GameObject)Resources.Load("LightBeam_ray"));
			prefabs.Add(typeof(LaserBeam), (GameObject)Resources.Load("LaserBeam"));
			prefabs.Add(typeof(LightBeamConvex), (GameObject)Resources.Load("LightBeam_Convex"));
		}

		public static void DestroyLight<T>(T light) where T : LightBase
		{
			light.Reset();
			((Component)light).get_transform().set_parent(((Component)Instance).get_transform());
			if (!Instance.pool[typeof(T)].Contains(light))
			{
				Instance.pool[typeof(T)].Push(light);
			}
		}

		public static T Create<T>(Vector3 origin, Vector3 dir, Transform parent = null) where T : LightBase
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return Instance.Inner_Create<T>(new Ray(origin, dir), parent);
		}

		public static T Create<T>(Ray ray, Transform parent = null) where T : LightBase
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			return Instance.Inner_Create<T>(ray, parent);
		}

		private T Inner_Create<T>(Ray ray, Transform parent) where T : LightBase
		{
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			Type typeFromHandle = typeof(T);
			if (!pool.ContainsKey(typeFromHandle))
			{
				pool[typeFromHandle] = new Stack<LightBase>();
			}
			T val;
			if (pool[typeFromHandle].Count > 0)
			{
				val = (T)pool[typeFromHandle].Pop();
				((Component)val).get_transform().set_position(((Ray)(ref ray)).get_origin());
			}
			else
			{
				val = Object.Instantiate<GameObject>(prefabs[typeFromHandle], ((Ray)(ref ray)).get_origin(), Quaternion.get_identity()).GetComponent<T>();
			}
			val.Direction = ((Ray)(ref ray)).get_direction();
			val.EnableLight();
			return val;
		}

		public LightPool()
			: this()
		{
		}
	}
}
