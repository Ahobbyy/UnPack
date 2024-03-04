using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ProGrids
{
	public static class pg_Util
	{
		private abstract class SnapEnabledOverride
		{
			public abstract bool IsEnabled();
		}

		private class SnapIsEnabledOverride : SnapEnabledOverride
		{
			private bool m_SnapIsEnabled;

			public SnapIsEnabledOverride(bool snapIsEnabled)
			{
				m_SnapIsEnabled = snapIsEnabled;
			}

			public override bool IsEnabled()
			{
				return m_SnapIsEnabled;
			}
		}

		private class ConditionalSnapOverride : SnapEnabledOverride
		{
			public Func<bool> m_IsEnabledDelegate;

			public ConditionalSnapOverride(Func<bool> d)
			{
				m_IsEnabledDelegate = d;
			}

			public override bool IsEnabled()
			{
				return m_IsEnabledDelegate();
			}
		}

		private const float EPSILON = 0.0001f;

		private static Dictionary<Transform, SnapEnabledOverride> m_SnapOverrideCache = new Dictionary<Transform, SnapEnabledOverride>();

		private static Dictionary<Type, bool> m_NoSnapAttributeTypeCache = new Dictionary<Type, bool>();

		private static Dictionary<Type, MethodInfo> m_ConditionalSnapAttributeCache = new Dictionary<Type, MethodInfo>();

		public static Color ColorWithString(string value)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			string valid = "01234567890.,";
			value = new string(value.Where((char c) => valid.Contains(c)).ToArray());
			string[] array = value.Split(',');
			if (array.Length < 4)
			{
				return new Color(1f, 0f, 1f, 1f);
			}
			return new Color(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
		}

		private static Vector3 VectorToMask(Vector3 vec)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3((Mathf.Abs(vec.x) > Mathf.Epsilon) ? 1f : 0f, (Mathf.Abs(vec.y) > Mathf.Epsilon) ? 1f : 0f, (Mathf.Abs(vec.z) > Mathf.Epsilon) ? 1f : 0f);
		}

		private static Axis MaskToAxis(Vector3 vec)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Axis axis = Axis.None;
			if (Mathf.Abs(vec.x) > 0f)
			{
				axis |= Axis.X;
			}
			if (Mathf.Abs(vec.y) > 0f)
			{
				axis |= Axis.Y;
			}
			if (Mathf.Abs(vec.z) > 0f)
			{
				axis |= Axis.Z;
			}
			return axis;
		}

		private static Axis BestAxis(Vector3 vec)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Abs(vec.x);
			float num2 = Mathf.Abs(vec.y);
			float num3 = Mathf.Abs(vec.z);
			if (!(num > num2) || !(num > num3))
			{
				if (!(num2 > num) || !(num2 > num3))
				{
					return Axis.Z;
				}
				return Axis.Y;
			}
			return Axis.X;
		}

		public static Axis CalcDragAxis(Vector3 movement, Camera cam)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = VectorToMask(movement);
			if (val.x + val.y + val.z == 2f)
			{
				return MaskToAxis(Vector3.get_one() - val);
			}
			switch (MaskToAxis(val))
			{
			case Axis.X:
				if (Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_up())) < Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_forward())))
				{
					return Axis.Z;
				}
				return Axis.Y;
			case Axis.Y:
				if (Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_right())) < Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_forward())))
				{
					return Axis.Z;
				}
				return Axis.X;
			case Axis.Z:
				if (Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_right())) < Mathf.Abs(Vector3.Dot(((Component)cam).get_transform().get_forward(), Vector3.get_up())))
				{
					return Axis.Y;
				}
				return Axis.X;
			default:
				return Axis.None;
			}
		}

		public static float ValueFromMask(Vector3 val, Vector3 mask)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (Mathf.Abs(mask.x) > 0.0001f)
			{
				return val.x;
			}
			if (Mathf.Abs(mask.y) > 0.0001f)
			{
				return val.y;
			}
			return val.z;
		}

		public static Vector3 SnapValue(Vector3 val, float snapValue)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			float x = val.x;
			float y = val.y;
			float z = val.z;
			return new Vector3(Snap(x, snapValue), Snap(y, snapValue), Snap(z, snapValue));
		}

		private static Type GetType(string type, string assembly = null)
		{
			Type type2 = Type.GetType(type);
			if (type2 == null)
			{
				IEnumerable<Assembly> enumerable = AppDomain.CurrentDomain.GetAssemblies();
				if (assembly != null)
				{
					enumerable = enumerable.Where((Assembly x) => x.FullName.Contains(assembly));
				}
				{
					foreach (Assembly item in enumerable)
					{
						type2 = item.GetType(type);
						if (type2 != null)
						{
							return type2;
						}
					}
					return type2;
				}
			}
			return type2;
		}

		public static void SetUnityGridEnabled(bool isEnabled)
		{
			try
			{
				GetType("UnityEditor.AnnotationUtility").GetProperty("showGrid", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, isEnabled, BindingFlags.Static | BindingFlags.NonPublic, null, null, null);
			}
			catch
			{
			}
		}

		public static bool GetUnityGridEnabled()
		{
			try
			{
				return (bool)GetType("UnityEditor.AnnotationUtility").GetProperty("showGrid", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null);
			}
			catch
			{
			}
			return false;
		}

		public static Vector3 SnapValue(Vector3 val, Vector3 mask, float snapValue)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			float x = val.x;
			float y = val.y;
			float z = val.z;
			return new Vector3((Mathf.Abs(mask.x) < 0.0001f) ? x : Snap(x, snapValue), (Mathf.Abs(mask.y) < 0.0001f) ? y : Snap(y, snapValue), (Mathf.Abs(mask.z) < 0.0001f) ? z : Snap(z, snapValue));
		}

		public static Vector3 SnapToCeil(Vector3 val, Vector3 mask, float snapValue)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			float x = val.x;
			float y = val.y;
			float z = val.z;
			return new Vector3((Mathf.Abs(mask.x) < 0.0001f) ? x : SnapToCeil(x, snapValue), (Mathf.Abs(mask.y) < 0.0001f) ? y : SnapToCeil(y, snapValue), (Mathf.Abs(mask.z) < 0.0001f) ? z : SnapToCeil(z, snapValue));
		}

		public static Vector3 SnapToFloor(Vector3 val, float snapValue)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			float x = val.x;
			float y = val.y;
			float z = val.z;
			return new Vector3(SnapToFloor(x, snapValue), SnapToFloor(y, snapValue), SnapToFloor(z, snapValue));
		}

		public static Vector3 SnapToFloor(Vector3 val, Vector3 mask, float snapValue)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			float x = val.x;
			float y = val.y;
			float z = val.z;
			return new Vector3((Mathf.Abs(mask.x) < 0.0001f) ? x : SnapToFloor(x, snapValue), (Mathf.Abs(mask.y) < 0.0001f) ? y : SnapToFloor(y, snapValue), (Mathf.Abs(mask.z) < 0.0001f) ? z : SnapToFloor(z, snapValue));
		}

		public static float Snap(float val, float round)
		{
			return round * Mathf.Round(val / round);
		}

		public static float SnapToFloor(float val, float snapValue)
		{
			return snapValue * Mathf.Floor(val / snapValue);
		}

		public static float SnapToCeil(float val, float snapValue)
		{
			return snapValue * Mathf.Ceil(val / snapValue);
		}

		public static Vector3 CeilFloor(Vector3 v)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			v.x = ((!(v.x < 0f)) ? 1 : (-1));
			v.y = ((!(v.y < 0f)) ? 1 : (-1));
			v.z = ((!(v.z < 0f)) ? 1 : (-1));
			return v;
		}

		public static void ClearSnapEnabledCache()
		{
			m_SnapOverrideCache.Clear();
		}

		public static bool SnapIsEnabled(Transform t)
		{
			if (m_SnapOverrideCache.TryGetValue(t, out var value))
			{
				return value.IsEnabled();
			}
			object[] source = null;
			MonoBehaviour[] components = ((Component)t).GetComponents<MonoBehaviour>();
			for (int i = 0; i < components.Length; i++)
			{
				Component c = (Component)(object)components[i];
				if ((Object)(object)c == (Object)null)
				{
					continue;
				}
				Type type = ((object)c).GetType();
				if (m_NoSnapAttributeTypeCache.TryGetValue(type, out var value2))
				{
					if (value2)
					{
						m_SnapOverrideCache.Add(t, new SnapIsEnabledOverride(!value2));
						return true;
					}
				}
				else
				{
					source = type.GetCustomAttributes(inherit: true);
					value2 = source.Any((object x) => x?.ToString().Contains("ProGridsNoSnap") ?? false);
					m_NoSnapAttributeTypeCache.Add(type, value2);
					if (value2)
					{
						m_SnapOverrideCache.Add(t, new SnapIsEnabledOverride(!value2));
						return true;
					}
				}
				if (m_ConditionalSnapAttributeCache.TryGetValue(type, out var mi))
				{
					if (mi != null)
					{
						m_SnapOverrideCache.Add(t, new ConditionalSnapOverride(() => (bool)mi.Invoke(c, null)));
						return (bool)mi.Invoke(c, null);
					}
				}
				else if (source.Any((object x) => x?.ToString().Contains("ProGridsConditionalSnap") ?? false))
				{
					mi = type.GetMethod("IsSnapEnabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
					m_ConditionalSnapAttributeCache.Add(type, mi);
					if (mi != null)
					{
						m_SnapOverrideCache.Add(t, new ConditionalSnapOverride(() => (bool)mi.Invoke(c, null)));
						return (bool)mi.Invoke(c, null);
					}
				}
				else
				{
					m_ConditionalSnapAttributeCache.Add(type, null);
				}
			}
			m_SnapOverrideCache.Add(t, new SnapIsEnabledOverride(snapIsEnabled: true));
			return true;
		}
	}
}
