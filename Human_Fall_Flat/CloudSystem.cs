using System;
using UnityEngine;

public class CloudSystem : MonoBehaviour
{
	public delegate void InitDelegate();

	public struct CloudData
	{
		public Vector3 worldPos;

		public Vector3 pos;

		public Vector3 size;

		public int startParticle;

		public int endParticle;
	}

	public struct CloudParticleData
	{
		public Vector3 pos;

		public Color color;

		public float size;

		public float angle;
	}

	public static CloudSystem instance;

	public int maxThreadCount = 4;

	public int threadCount = 4;

	public int maxClouds = 32;

	public int segmentsPerCloud = 4;

	public int particlesPerSegment = 8;

	public Vector3 cloudSetSize = new Vector3(400f, 400f, 400f);

	public float nearClipStart = 5f;

	public float nearClipEnd = 10f;

	public float farClipStart = 150f;

	public float farClipEnd = 200f;

	public float defaultNearClipStart = 5f;

	public float defaultNearClipEnd = 10f;

	public float defaultFarClipStart = 150f;

	public float defaultFarClipEnd = 200f;

	public Vector3 cloudSize = new Vector3(20f, 5f, 20f);

	public Vector3 cloudSegmentSize = new Vector3(15f, 10f, 15f);

	public float particleScale = 15f;

	public float rotateSpeed = 0.05f;

	public Vector3 moveSpeed = new Vector3(2f, 0f, 0f);

	public int maxParticles = 1024;

	public Color mainColor = Color32.op_Implicit(new Color32((byte)158, (byte)158, (byte)158, (byte)77));

	public Color tintColor = Color32.op_Implicit(new Color32((byte)79, (byte)79, (byte)79, byte.MaxValue));

	public float tintCenter = 0.7f;

	public float tintScale = 1f;

	public float tintOffset;

	public Vector3 cloudSetOffset;

	private MeshFilter meshFilter;

	public Mesh mesh;

	private int curQuality = -1;

	public CloudData[] cloudsData;

	public CloudParticleData[] particlesData;

	public int maxVisibleParticlesCount;

	public static event InitDelegate OnSystemInit;

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		instance = this;
		meshFilter = ((Component)this).GetComponent<MeshFilter>();
		mesh = new Mesh();
		((Object)mesh).set_name("Clouds");
		CloudSystem.OnSystemInit?.Invoke();
	}

	public void SetQuality(int quality)
	{
		if (quality != curQuality)
		{
			curQuality = quality;
			Debug.Log((object)("CloudSystem quality=" + curQuality));
			switch (quality)
			{
			case 0:
				threadCount = 1;
				maxClouds = 0;
				segmentsPerCloud = 2;
				particlesPerSegment = 2;
				break;
			case 1:
				threadCount = 1;
				maxClouds = 48;
				segmentsPerCloud = 3;
				particlesPerSegment = 3;
				break;
			case 2:
				threadCount = 1;
				maxClouds = 64;
				segmentsPerCloud = 4;
				particlesPerSegment = 4;
				break;
			case 3:
				threadCount = 1;
				maxClouds = 96;
				segmentsPerCloud = 4;
				particlesPerSegment = 6;
				break;
			case 4:
				threadCount = 1;
				maxClouds = 128;
				segmentsPerCloud = 6;
				particlesPerSegment = 8;
				break;
			}
			InitializeCloud();
		}
	}

	private void InitializeCloud()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		CloudRender.all[0].KillAllThreads();
		maxParticles = maxClouds * segmentsPerCloud * particlesPerSegment;
		Debug.Log((object)("CloudSystem maxParticles=" + maxParticles + " " + CloudRender.all.Count));
		GenerateCloudSet(new Vector3(400f, 500f, 400f));
		for (int i = 0; i < CloudRender.all.Count; i++)
		{
			CloudRender.all[i].InitializeCloud();
		}
		if (CloudRender.all.Count > 0)
		{
			mesh.Clear();
			Vector2[] array = (Vector2[])(object)new Vector2[CloudRender.all[0].maxMeshParticles * 4];
			int[] array2 = new int[CloudRender.all[0].maxMeshParticles * 6];
			for (int j = 0; j < CloudRender.all[0].maxMeshParticles; j++)
			{
				array2[j * 6] = j * 4;
				array2[j * 6 + 1] = j * 4 + 1;
				array2[j * 6 + 2] = j * 4 + 2;
				array2[j * 6 + 3] = j * 4 + 2;
				array2[j * 6 + 4] = j * 4 + 3;
				array2[j * 6 + 5] = j * 4;
				array[j * 4] = new Vector2(0f, 0f);
				array[j * 4 + 1] = new Vector2(0f, 1f);
				array[j * 4 + 2] = new Vector2(1f, 1f);
				array[j * 4 + 3] = new Vector2(1f, 0f);
			}
			mesh.set_vertices(CloudRender.all[0].vertices);
			mesh.set_colors32(CloudRender.all[0].colors);
			mesh.set_uv(array);
			mesh.set_triangles(array2);
			meshFilter.set_mesh(mesh);
		}
	}

	private void GenerateCloudSet(Vector3 size)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		cloudsData = new CloudData[maxClouds];
		particlesData = new CloudParticleData[maxParticles];
		Vector3 center = default(Vector3);
		for (int i = 0; i < maxClouds; i++)
		{
			((Vector3)(ref center))._002Ector(size.x * Random.get_value(), size.y * Random.get_value(), size.z * Random.get_value());
			GenerateCloud(i, center);
		}
	}

	private void GenerateCloud(int index, Vector3 center)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		int num = index * segmentsPerCloud * particlesPerSegment;
		int num2 = (index + 1) * segmentsPerCloud * particlesPerSegment;
		cloudsData[index].startParticle = num;
		cloudsData[index].endParticle = num2;
		for (int i = num; i < num2; i += particlesPerSegment)
		{
			BuildCloudPart(Vector3.Scale(Random.get_insideUnitSphere(), cloudSize), i, i + particlesPerSegment);
		}
		Bounds val = default(Bounds);
		((Bounds)(ref val))._002Ector(particlesData[num].pos, Vector3.get_zero());
		Vector3 val2 = default(Vector3);
		for (int j = num; j < num2; j++)
		{
			((Vector3)(ref val2))._002Ector(particlesData[j].size / 2f, particlesData[j].size / 2f, particlesData[j].size / 2f);
			((Bounds)(ref val)).Encapsulate(particlesData[j].pos - val2);
			((Bounds)(ref val)).Encapsulate(particlesData[j].pos + val2);
		}
		cloudsData[index].pos = ((Bounds)(ref val)).get_center() + center;
		cloudsData[index].size = ((Bounds)(ref val)).get_size();
		for (int k = num; k < num2; k++)
		{
			ref Vector3 pos = ref particlesData[k].pos;
			pos -= ((Bounds)(ref val)).get_center();
		}
		ColorCloud(num, num2);
	}

	private void BuildCloudPart(Vector3 pos, int start, int end)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		for (int i = start; i < end; i++)
		{
			particlesData[i].pos = pos + Vector3.Scale(Random.get_insideUnitSphere(), cloudSegmentSize);
			particlesData[i].size = particleScale * Random.Range(0.6f, 1f);
			particlesData[i].angle = Random.get_value() * (float)Math.PI * 2f - (float)Math.PI;
		}
	}

	private void ColorCloud(int start, int end)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		float num2 = float.MinValue;
		for (int i = start; i < end; i++)
		{
			float y = particlesData[i].pos.y;
			num = Mathf.Min(num, y);
			num2 = Mathf.Max(num2, y);
		}
		for (int j = start; j < end; j++)
		{
			float num3 = Mathf.Abs((particlesData[j].pos.y - num) / (num2 - num) - tintCenter) * tintScale + tintOffset;
			particlesData[j].color = Color.Lerp(mainColor, tintColor, num3);
		}
	}

	private void LateUpdate()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		float deltaTime = Time.get_deltaTime();
		cloudSetOffset += moveSpeed * deltaTime;
		for (int i = 0; i < CloudBox.all.Count; i++)
		{
			CloudBox.all[i].ReadPos();
		}
		for (int j = 0; j < CloudRender.all.Count; j++)
		{
			CloudRender.all[j].StartUpdate();
		}
	}

	public void Scroll(Vector3 offset)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		cloudSetOffset += offset;
	}

	public float DistanceClipAlpha(float alpha, float distance)
	{
		if (distance < nearClipEnd)
		{
			alpha *= Mathf.Clamp01((distance - nearClipStart) / (nearClipEnd - nearClipStart));
		}
		if (distance > farClipStart)
		{
			alpha *= Mathf.Clamp01((distance - farClipEnd) / (farClipStart - farClipEnd));
		}
		return alpha;
	}

	public CloudSystem()
		: this()
	{
	}//IL_0034: Unknown result type (might be due to invalid IL or missing references)
	//IL_0039: Unknown result type (might be due to invalid IL or missing references)
	//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
	//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
	//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
	//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
	//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
	//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
	//IL_0117: Unknown result type (might be due to invalid IL or missing references)
	//IL_011c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0121: Unknown result type (might be due to invalid IL or missing references)
	//IL_0132: Unknown result type (might be due to invalid IL or missing references)
	//IL_0137: Unknown result type (might be due to invalid IL or missing references)
	//IL_013c: Unknown result type (might be due to invalid IL or missing references)

}
