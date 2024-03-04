using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace UnityEngine.Rendering.PostProcessing
{
	internal static class MeshUtilities
	{
		private static Dictionary<PrimitiveType, Mesh> s_Primitives;

		private static Dictionary<Type, PrimitiveType> s_ColliderPrimitives;

		static MeshUtilities()
		{
			s_Primitives = new Dictionary<PrimitiveType, Mesh>();
			s_ColliderPrimitives = new Dictionary<Type, PrimitiveType>
			{
				{
					typeof(BoxCollider),
					(PrimitiveType)3
				},
				{
					typeof(SphereCollider),
					(PrimitiveType)0
				},
				{
					typeof(CapsuleCollider),
					(PrimitiveType)1
				}
			};
		}

		internal static Mesh GetColliderMesh(Collider collider)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			Type type = ((object)collider).GetType();
			if (type == typeof(MeshCollider))
			{
				return ((MeshCollider)collider).get_sharedMesh();
			}
			Assert.IsTrue(s_ColliderPrimitives.ContainsKey(type), "Unknown collider");
			return GetPrimitive(s_ColliderPrimitives[type]);
		}

		internal static Mesh GetPrimitive(PrimitiveType primitiveType)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			if (!s_Primitives.TryGetValue(primitiveType, out var value))
			{
				value = GetBuiltinMesh(primitiveType);
				s_Primitives.Add(primitiveType, value);
			}
			return value;
		}

		private static Mesh GetBuiltinMesh(PrimitiveType primitiveType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			GameObject obj = GameObject.CreatePrimitive(primitiveType);
			Mesh sharedMesh = obj.GetComponent<MeshFilter>().get_sharedMesh();
			RuntimeUtilities.Destroy((Object)(object)obj);
			return sharedMesh;
		}
	}
}
