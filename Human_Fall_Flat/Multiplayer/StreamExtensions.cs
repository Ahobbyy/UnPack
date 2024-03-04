using System.IO;
using UnityEngine;

namespace Multiplayer
{
	public static class StreamExtensions
	{
		public static void Write(this BinaryWriter writer, Vector3 vec3)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			writer.Write(vec3.x);
			writer.Write(vec3.y);
			writer.Write(vec3.z);
		}

		public static Vector3 ReadVector3(this BinaryReader reader)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}

		public static void Write(this BinaryWriter writer, Quaternion quat)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			writer.Write(quat.x);
			writer.Write(quat.y);
			writer.Write(quat.z);
			writer.Write(quat.w);
		}

		public static Quaternion ReadQuaternion(this BinaryReader reader)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
		}
	}
}
