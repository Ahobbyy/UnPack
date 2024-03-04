using System.IO;
using UnityEngine;

public class RecordingStream
{
	public const int CURRENT_VERSION = 1;

	public int version;

	private bool write;

	public MemoryStream stream;

	private BinaryReader reader;

	private BinaryWriter writer;

	public bool isWriting => write;

	public bool isReading => !write;

	public RecordingStream(int version, bool write, MemoryStream baseStream)
	{
		this.version = version;
		stream = baseStream;
		this.write = write;
		if (write)
		{
			writer = new BinaryWriter(stream);
		}
		else
		{
			reader = new BinaryReader(stream);
		}
		Serialize(ref this.version);
	}

	public void Serialize(ref int data)
	{
		if (write)
		{
			WriteInt(data);
		}
		else
		{
			data = ReadInt();
		}
	}

	public void WriteInt(int data)
	{
		writer.Write(data);
	}

	public int ReadInt()
	{
		return reader.ReadInt32();
	}

	public void Serialize(ref float data)
	{
		if (write)
		{
			WriteFloat(data);
		}
		else
		{
			data = ReadFloat();
		}
	}

	public void WriteFloat(float data)
	{
		writer.Write(data);
	}

	public float ReadFloat()
	{
		return reader.ReadSingle();
	}

	public void Serialize(ref Vector3 data)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (write)
		{
			WriteVector3(data);
		}
		else
		{
			data = ReadVector3();
		}
	}

	public void WriteVector3(Vector3 data)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		WriteFloat(data.x);
		WriteFloat(data.y);
		WriteFloat(data.z);
	}

	public Vector3 ReadVector3()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
	}

	public void Serialize(ref Quaternion data)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (write)
		{
			WriteQuaternion(data);
		}
		else
		{
			data = ReadQuaternion();
		}
	}

	public void WriteQuaternion(Quaternion data)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		WriteFloat(data.x);
		WriteFloat(data.y);
		WriteFloat(data.z);
		WriteFloat(data.w);
	}

	public Quaternion ReadQuaternion()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		return new Quaternion(ReadFloat(), ReadFloat(), ReadFloat(), ReadFloat());
	}
}
