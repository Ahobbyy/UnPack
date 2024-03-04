using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RagdollExport : MonoBehaviour
{
	[Serializable]
	public class SerializedFrame
	{
		public int frame;

		public List<SerializedTransform> objects = new List<SerializedTransform>();

		public void Lerp(SerializedFrame f2, float t)
		{
			for (int i = 0; i < objects.Count; i++)
			{
				objects[i].Lerp(f2.objects[i], t);
			}
		}
	}

	[Serializable]
	public class SerializedTransform
	{
		public string name;

		public float[] pos;

		public float[] rot;

		public Vector3 position => new Vector3(pos[0], pos[1], pos[2]);

		public Quaternion rotation => new Quaternion(rot[0], rot[1], rot[2], rot[3]);

		public void Lerp(SerializedTransform f2, float t)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Vector3.Lerp(position, f2.position, t);
			Quaternion val2 = Quaternion.Lerp(rotation, f2.rotation, t);
			pos = new float[3] { val.x, val.y, val.z };
			rot = new float[4] { val2.x, val2.y, val2.z, val2.w };
		}
	}

	private Ragdoll ragdoll;

	private Dictionary<string, Quaternion> initialRot = new Dictionary<string, Quaternion>();

	private string filename;

	private bool writing;

	private List<SerializedFrame> frames = new List<SerializedFrame>();

	private void Awake()
	{
		ragdoll = ((Component)this).GetComponent<Ragdoll>();
		SaveInitial(ragdoll.partHips.transform);
		SaveInitial(ragdoll.partWaist.transform);
		SaveInitial(ragdoll.partChest.transform);
		SaveInitial(ragdoll.partHead.transform);
		SaveInitial(ragdoll.partLeftArm.transform);
		SaveInitial(ragdoll.partLeftForearm.transform);
		SaveInitial(ragdoll.partLeftThigh.transform);
		SaveInitial(ragdoll.partLeftLeg.transform);
		SaveInitial(ragdoll.partRightArm.transform);
		SaveInitial(ragdoll.partRightForearm.transform);
		SaveInitial(ragdoll.partRightThigh.transform);
		SaveInitial(ragdoll.partRightLeg.transform);
	}

	private void SaveInitial(Transform t)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		initialRot[((Object)t).get_name()] = Quaternion.Inverse(t.get_localRotation());
		if (((Object)t).get_name() == "Hips")
		{
			initialRot[((Object)t).get_name()] = Quaternion.Inverse(t.get_rotation());
		}
	}

	private void Update()
	{
		if (Game.GetKeyDown((KeyCode)112))
		{
			writing = true;
			filename = string.Format("ragdoll-{0}.txt", DateTime.Now.ToString("HHmmss"));
			frames.Clear();
		}
	}

	private void FixedUpdate()
	{
		if (!writing)
		{
			return;
		}
		if (frames.Count > 1060)
		{
			writing = false;
			int num = 60;
			for (int i = 0; i < num; i++)
			{
				frames[i].Lerp(frames[i + 1000], 1f - 1f * (float)i / (float)num);
			}
			for (int j = 0; j < 1000; j++)
			{
				File.AppendAllText(filename, JsonUtility.ToJson((object)frames[j], false));
				File.AppendAllText(filename, ",\r\n");
			}
		}
		SerializedFrame serializedFrame = new SerializedFrame();
		serializedFrame.objects.Add(StoreTransform(ragdoll.partHips.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partWaist.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partChest.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partHead.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partLeftArm.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partLeftForearm.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partLeftThigh.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partLeftLeg.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partRightArm.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partRightForearm.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partRightThigh.transform));
		serializedFrame.objects.Add(StoreTransform(ragdoll.partRightLeg.transform));
		frames.Add(serializedFrame);
	}

	private SerializedTransform StoreTransform(Transform t)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = t.get_position();
		Quaternion val = initialRot[((Object)t).get_name()] * t.get_localRotation();
		if (((Object)t).get_name() == "Hips")
		{
			val = initialRot[((Object)t).get_name()] * t.get_rotation();
		}
		SerializedTransform serializedTransform = new SerializedTransform();
		serializedTransform.name = ((Object)t).get_name();
		serializedTransform.pos = new float[3] { position.x, position.y, position.z };
		serializedTransform.rot = new float[4] { val.x, val.y, val.z, val.w };
		return serializedTransform;
	}

	public RagdollExport()
		: this()
	{
	}
}
