using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleLightProbePlacer
{
	[AddComponentMenu("")]
	public class TransformVolume : MonoBehaviour
	{
		[SerializeField]
		private Volume m_volume = new Volume(Vector3.get_zero(), Vector3.get_one());

		public Volume Volume
		{
			get
			{
				return m_volume;
			}
			set
			{
				m_volume = value;
			}
		}

		public Vector3 Origin => m_volume.Origin;

		public Vector3 Size => m_volume.Size;

		public bool IsInBounds(Vector3[] points)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Bounds bounds = GetBounds();
			return ((Bounds)(ref bounds)).Intersects(GetBounds(points));
		}

		public bool IsOnBorder(Vector3[] points)
		{
			if (points.All((Vector3 x) => !IsInVolume(x)))
			{
				return false;
			}
			return !points.All(IsInVolume);
		}

		public bool IsInVolume(Vector3[] points)
		{
			return points.All(IsInVolume);
		}

		public bool IsInVolume(Vector3 position)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			Plane val = default(Plane);
			for (int i = 0; i < 6; i++)
			{
				((Plane)(ref val))._002Ector(GetSideDirection(i), GetSidePosition(i));
				if (((Plane)(ref val)).GetSide(position))
				{
					return false;
				}
			}
			return true;
		}

		public Vector3[] GetCorners()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] array = (Vector3[])(object)new Vector3[8]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			};
			for (int i = 0; i < array.Length; i++)
			{
				array[i].x *= m_volume.Size.x;
				array[i].y *= m_volume.Size.y;
				array[i].z *= m_volume.Size.z;
				array[i] = ((Component)this).get_transform().TransformPoint(m_volume.Origin + array[i]);
			}
			return array;
		}

		public Bounds GetBounds()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return GetBounds(GetCorners());
		}

		public Bounds GetBounds(Vector3[] points)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = points.Aggregate(Vector3.get_zero(), (Vector3 result, Vector3 point) => result + point) / (float)points.Length;
			Bounds result2 = default(Bounds);
			((Bounds)(ref result2))._002Ector(val, Vector3.get_zero());
			for (int i = 0; i < points.Length; i++)
			{
				((Bounds)(ref result2)).Encapsulate(points[i]);
			}
			return result2;
		}

		public GameObject[] GetGameObjectsInBounds(LayerMask layerMask)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			MeshRenderer[] array = Object.FindObjectsOfType<MeshRenderer>();
			List<GameObject> list = new List<GameObject>();
			Bounds bounds = GetBounds();
			for (int i = 0; i < array.Length; i++)
			{
				if (!((Object)(object)((Component)array[i]).get_gameObject() == (Object)(object)((Component)((Component)this).get_transform()).get_gameObject()) && !((Object)(object)((Component)array[i]).GetComponent<TransformVolume>() != (Object)null) && ((1 << ((Component)array[i]).get_gameObject().get_layer()) & ((LayerMask)(ref layerMask)).get_value()) != 0 && ((Bounds)(ref bounds)).Intersects(((Renderer)array[i]).get_bounds()))
				{
					list.Add(((Component)array[i]).get_gameObject());
				}
			}
			return list.ToArray();
		}

		public Vector3 GetSideDirection(int side)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] array = (Vector3[])(object)new Vector3[6];
			Vector3 right = Vector3.get_right();
			Vector3 up = Vector3.get_up();
			Vector3 forward = Vector3.get_forward();
			array[0] = right;
			array[1] = -right;
			array[2] = up;
			array[3] = -up;
			array[4] = forward;
			array[5] = -forward;
			return ((Component)this).get_transform().TransformDirection(array[side]);
		}

		public Vector3 GetSidePosition(int side)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] array = (Vector3[])(object)new Vector3[6];
			Vector3 right = Vector3.get_right();
			Vector3 up = Vector3.get_up();
			Vector3 forward = Vector3.get_forward();
			array[0] = right;
			array[1] = -right;
			array[2] = up;
			array[3] = -up;
			array[4] = forward;
			array[5] = -forward;
			return ((Component)this).get_transform().TransformPoint(array[side] * GetSizeAxis(side) + m_volume.Origin);
		}

		public float GetSizeAxis(int side)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			switch (side)
			{
			case 0:
			case 1:
				return m_volume.Size.x * 0.5f;
			case 2:
			case 3:
				return m_volume.Size.y * 0.5f;
			default:
				return m_volume.Size.z * 0.5f;
			}
		}

		public static Volume EditorVolumeControl(TransformVolume transformVolume, float handleSize, Color color)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Expected O, but got Unknown
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Expected O, but got Unknown
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Expected O, but got Unknown
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_0187: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			Vector3[] array = (Vector3[])(object)new Vector3[6];
			Transform transform = ((Component)transformVolume).get_transform();
			Handles.set_color(color);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transformVolume.GetSidePosition(i);
			}
			array[0] = Handles.Slider(array[0], transform.get_right(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			array[1] = Handles.Slider(array[1], transform.get_right(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			array[2] = Handles.Slider(array[2], transform.get_up(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			array[3] = Handles.Slider(array[3], transform.get_up(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			array[4] = Handles.Slider(array[4], transform.get_forward(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			array[5] = Handles.Slider(array[5], transform.get_forward(), handleSize, new CapFunction(Handles.DotHandleCap), 1f);
			Vector3 origin = default(Vector3);
			origin.x = transform.InverseTransformPoint((array[0] + array[1]) * 0.5f).x;
			origin.y = transform.InverseTransformPoint((array[2] + array[3]) * 0.5f).y;
			origin.z = transform.InverseTransformPoint((array[4] + array[5]) * 0.5f).z;
			Vector3 size = default(Vector3);
			size.x = transform.InverseTransformPoint(array[0]).x - transform.InverseTransformPoint(array[1]).x;
			size.y = transform.InverseTransformPoint(array[2]).y - transform.InverseTransformPoint(array[3]).y;
			size.z = transform.InverseTransformPoint(array[4]).z - transform.InverseTransformPoint(array[5]).z;
			return new Volume(origin, size);
		}

		public TransformVolume()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
