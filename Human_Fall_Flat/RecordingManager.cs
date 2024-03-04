using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordingManager : MonoBehaviour
{
	private enum RecorderState
	{
		Idle,
		Recording,
		Playing
	}

	private const int transformFrameBytes = 28;

	public List<Transform> roots = new List<Transform>();

	private List<Transform> transforms;

	private RecorderState state;

	private RecordingStream stream;

	public int frame;

	public float time;

	private void Start()
	{
		roots.Add(((Component)Human.instance).get_transform());
		transforms = new List<Transform>();
		foreach (Transform root in roots)
		{
			Transform[] componentsInChildren = ((Component)root).GetComponentsInChildren<Transform>();
			transforms.AddRange(componentsInChildren);
		}
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown((KeyCode)49))
		{
			BeginRecording();
		}
		if (Input.GetKeyDown((KeyCode)50))
		{
			EndRecording();
		}
		if (Input.GetKeyDown((KeyCode)51))
		{
			BeginPlayback();
		}
		if (Input.GetKeyDown((KeyCode)52))
		{
			EndPlayback();
		}
	}

	private void FixedUpdate()
	{
		if (frame++ % 2 == 0)
		{
			if (state == RecorderState.Recording)
			{
				RecordFrame();
			}
			if (state == RecorderState.Playing)
			{
				time += Time.get_fixedDeltaTime() * 2f;
				ReadFrame();
			}
		}
	}

	private void BeginRecording()
	{
		frame = 0;
		state = RecorderState.Recording;
		MemoryStream baseStream = new MemoryStream();
		stream = new RecordingStream(1, write: true, baseStream);
	}

	private void EndRecording()
	{
		File.WriteAllBytes("Recording.bytes", stream.stream.ToArray());
		state = RecorderState.Idle;
		stream = null;
	}

	private void BeginPlayback()
	{
		frame = 0;
		((Behaviour)Human.instance).set_enabled(false);
		state = RecorderState.Playing;
		MemoryStream baseStream = new MemoryStream(File.ReadAllBytes("Recording.bytes"));
		stream = new RecordingStream(0, write: false, baseStream);
		StripBehaviors();
	}

	public void BeginPlayback(byte[] data, float startingTime)
	{
		time = startingTime;
		((Behaviour)Human.instance).set_enabled(false);
		state = RecorderState.Playing;
		MemoryStream baseStream = new MemoryStream(data);
		stream = new RecordingStream(0, write: false, baseStream);
		StripBehaviors();
		int num = Mathf.RoundToInt(startingTime * 60f);
		stream.stream.Position = 4 + 28 * num * transforms.Count;
	}

	private void StripBehaviors()
	{
		foreach (Transform transform in transforms)
		{
			Component[] components = ((Component)transform).GetComponents<Component>();
			foreach (Component val in components)
			{
				if (!(val is Transform) && !(val is SkinnedMeshRenderer) && !(val is MeshRenderer) && !(val is MeshFilter))
				{
					if (val is Rigidbody)
					{
						((Rigidbody)((val is Rigidbody) ? val : null)).set_isKinematic(true);
					}
					else if (val is Human)
					{
						((Behaviour)(val as Human)).set_enabled(false);
					}
					else
					{
						Object.Destroy((Object)(object)val);
					}
				}
			}
		}
	}

	private void EndPlayback()
	{
		((Behaviour)Human.instance).set_enabled(true);
		state = RecorderState.Idle;
		stream = null;
	}

	private void RecordFrame()
	{
		Serialize(stream);
	}

	private void ReadFrame()
	{
		Serialize(stream);
		if (stream.stream.Position == stream.stream.Length)
		{
			EndPlayback();
		}
	}

	private void Serialize(RecordingStream rs)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform transform in transforms)
		{
			Vector3 data = transform.get_localPosition();
			Quaternion data2 = transform.get_localRotation();
			rs.Serialize(ref data);
			rs.Serialize(ref data2);
			if (rs.isReading)
			{
				transform.set_localPosition(data);
				transform.set_localRotation(data2);
			}
		}
	}

	public RecordingManager()
		: this()
	{
	}
}
