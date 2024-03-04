using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayer;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightConsume : Node
	{
		private LightFilter[] filters;

		private Vector3 lastPosition;

		private Quaternion lastRotation;

		private Dictionary<LightBase, LightHitInfo> lightHits;

		public NodeOutput Intensity;

		public bool checkIfUnderSun;

		public bool debugLog;

		[HideInInspector]
		public List<LightBase> ignoreLights;

		public Action<LightBase> lightAdded = delegate
		{
		};

		public Action<LightBase> lightRemoved = delegate
		{
		};

		private Collider col;

		public bool isLit => lightHits.Count > 0;

		public Color LitColor
		{
			get
			{
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
				//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
				//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_010d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0139: Unknown result type (might be due to invalid IL or missing references)
				Color val = default(Color);
				((Color)(ref val))._002Ector(0f, 0f, 0f, 0f);
				if (lightHits.Count == 0)
				{
					return val;
				}
				foreach (KeyValuePair<LightBase, LightHitInfo> lightHit in lightHits)
				{
					if (lightHit.Value.intensity > -1f)
					{
						val.r += lightHit.Value.intensity;
						val.r += lightHit.Value.intensity;
						val.r += lightHit.Value.intensity;
					}
					else
					{
						val.r = lightHit.Key.color.r + val.r;
						val.g = lightHit.Key.color.g + val.g;
						val.b = lightHit.Key.color.b + val.b;
					}
					val.a = Mathf.Max(lightHit.Key.color.a, val.a);
				}
				return val;
			}
		}

		protected virtual void Awake()
		{
			filters = ((Component)this).GetComponentsInChildren<LightFilter>();
			filters = filters.OrderByDescending((LightFilter f) => f.priority).ToArray();
			LightFilter[] array = filters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Init(this);
			}
			lightHits = new Dictionary<LightBase, LightHitInfo>();
			ignoreLights = new List<LightBase>();
			col = ((Component)this).GetComponent<Collider>();
		}

		private void FixedUpdate()
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			if (!NetGame.isClient)
			{
				if (lastPosition != ((Component)this).get_transform().get_position() || lastRotation != ((Component)this).get_transform().get_rotation())
				{
					lastRotation = ((Component)this).get_transform().get_rotation();
					lastPosition = ((Component)this).get_transform().get_position();
					RecalculateAll();
				}
				if (checkIfUnderSun)
				{
					CheckIfUnderSun();
				}
				CheckOutput();
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			LightBase componentInParent = ((Component)other).GetComponentInParent<LightBase>();
			if (!((Object)(object)componentInParent == (Object)null))
			{
				if (debugLog)
				{
					Debug.Log((object)("Enter " + ((Object)other).get_name()));
				}
				AddLightSource(componentInParent);
			}
		}

		private void OnTriggerStay(Collider other)
		{
			LightBase componentInParent = ((Component)other).GetComponentInParent<LightBase>();
			if (!((Object)(object)componentInParent == (Object)null))
			{
				AddLightSource(componentInParent);
			}
		}

		private void OnTriggerExit(Collider other)
		{
			LightBase componentInParent = ((Component)other).GetComponentInParent<LightBase>();
			if (!((Object)(object)componentInParent == (Object)null))
			{
				if (debugLog)
				{
					Debug.Log((object)("Exit " + ((Object)other).get_name()));
				}
				RemoveLightSource(componentInParent);
				componentInParent.UpdateLight();
			}
		}

		private void AddLightSource(LightBase source)
		{
			if (!ignoreLights.Contains(source) && !lightHits.ContainsKey(source))
			{
				source.AddConsume(this);
				LightHitInfo lightHitInfo = new LightHitInfo(source);
				Recalculate(lightHitInfo);
				lightHits.Add(source, lightHitInfo);
				if (debugLog)
				{
					Debug.Log((object)("Added new light source: " + ((Object)source).get_name()));
				}
				lightAdded(source);
				source.UpdateLight();
			}
		}

		private void Recalculate(LightHitInfo info)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			info.contactPoint = info.source.ClosestPoint(((Component)this).get_transform().get_position());
			LightFilter[] array = filters;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ApplyFilter(info);
			}
		}

		protected virtual void CheckOutput()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			Color litColor = LitColor;
			float num = (litColor.r + litColor.g + litColor.b) / 3f;
			if (num != Intensity.value)
			{
				Intensity.SetValue(num);
			}
		}

		private T CreateDefaultOutput<T>(T input, Vector3 point) where T : LightBase
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return LightPool.Create<T>(point, point - ((Component)input).get_transform().get_position(), ((Component)this).get_transform());
		}

		private void CheckIfUnderSun()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)Sun.instance == (Object)null)
			{
				return;
			}
			Vector3 direction = Sun.instance.Direction;
			Vector3 val = ((Component)this).get_transform().InverseTransformDirection(Vector3.get_up());
			Quaternion rotation = ((Component)this).get_transform().get_rotation();
			Quaternion val2 = Quaternion.AngleAxis(((Quaternion)(ref rotation)).get_eulerAngles().y, val);
			Bounds bounds = col.get_bounds();
			Vector3[] array = (Vector3[])(object)new Vector3[4]
			{
				val2 * ((Component)this).get_transform().InverseTransformDirection(((Bounds)(ref bounds)).get_extents().x * 0.6f, 0f, 0f),
				val2 * ((Component)this).get_transform().InverseTransformDirection((0f - ((Bounds)(ref bounds)).get_extents().x) * 0.6f, 0f, 0f),
				val2 * ((Component)this).get_transform().InverseTransformDirection(0f, 0f, ((Bounds)(ref bounds)).get_extents().z * 0.6f),
				val2 * ((Component)this).get_transform().InverseTransformDirection(0f, 0f, (0f - ((Bounds)(ref bounds)).get_extents().z) * 0.6f)
			};
			float num = 4f;
			Vector3[] array2 = array;
			foreach (Vector3 val3 in array2)
			{
				if (Physics.Raycast(((Component)this).get_transform().TransformPoint(val3), -direction))
				{
					num -= 1f;
				}
			}
			if (num == 3f)
			{
				num = 2f;
			}
			float intensity = num / (float)array.Length;
			array2 = array;
			foreach (Vector3 val4 in array2)
			{
				Debug.DrawLine(((Component)this).get_transform().TransformPoint(val4), ((Component)this).get_transform().TransformPoint(val4) + Vector3.get_up() * 0.1f);
			}
			if (!lightHits.ContainsKey(Sun.instance))
			{
				LightHitInfo value = new LightHitInfo(Sun.instance, intensity);
				lightHits.Add(Sun.instance, value);
			}
			else
			{
				lightHits[Sun.instance].intensity = intensity;
			}
		}

		public override void Process()
		{
			base.Process();
		}

		public void RecalculateAll()
		{
			foreach (LightBase item in new List<LightBase>(lightHits.Keys))
			{
				item.UpdateLight();
			}
		}

		public void Recalculate(LightBase source)
		{
			if (lightHits.ContainsKey(source))
			{
				Recalculate(lightHits[source]);
			}
		}

		public void RemoveLightSource(LightBase source)
		{
			if (!lightHits.ContainsKey(source))
			{
				return;
			}
			source.RemoveConsume(this);
			foreach (LightBase output in lightHits[source].outputs)
			{
				output.DisableLight();
			}
			lightHits.Remove(source);
			lightRemoved(source);
		}
	}
}
