using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CloudRender : MonoBehaviour
{
	private struct ParticleSort
	{
		public int index;

		public Vector3 worldPos;

		public float distance;

		public override string ToString()
		{
			return $"{index} {distance}";
		}
	}

	private class ThreadParams
	{
		public int start;

		public int end;

		public int vpCount;

		public bool running;

		public bool abort;

		public Vector3[] verts;

		public Color32[] cols;

		public Vector3 camX;

		public Vector3 camY;

		public ParticleSort[] psort;

		public AutoResetEvent startHandle;

		public AutoResetEvent completeHandle;
	}

	public static List<CloudRender> all = new List<CloudRender>();

	private int[] visibleClouds;

	public int visibleCloudsCount;

	private int[] visibleParticles;

	public int visibleParticlesCount;

	public int maxMeshParticles = 256;

	public Vector3[] vertices;

	public Color32[] colors;

	private int[] myThreads;

	private ParticleSort[] particleSort;

	private static ThreadParams[] helperParams;

	private static Thread[] helperThreads;

	private Vector3 camX;

	private Vector3 camY;

	private bool updateStarted;

	private Plane[] mPlanes = (Plane[])(object)new Plane[6];

	private void OnEnable()
	{
		all.Add(this);
		if ((Object)(object)CloudSystem.instance != (Object)null && CloudSystem.instance.cloudsData != null)
		{
			InitializeCloud();
		}
	}

	private void OnDisable()
	{
		KillThreads();
		all.Remove(this);
	}

	public void InitializeCloud()
	{
		visibleClouds = new int[CloudSystem.instance.maxClouds];
		visibleParticles = new int[CloudSystem.instance.maxParticles];
		maxMeshParticles = CloudSystem.instance.maxParticles / 4;
		vertices = (Vector3[])(object)new Vector3[maxMeshParticles * 4];
		colors = (Color32[])(object)new Color32[maxMeshParticles * 4];
		particleSort = new ParticleSort[CloudSystem.instance.maxParticles];
		myThreads = new int[CloudSystem.instance.maxThreadCount];
		if (helperThreads == null)
		{
			helperThreads = new Thread[CloudSystem.instance.maxThreadCount];
			helperParams = new ThreadParams[CloudSystem.instance.maxThreadCount];
			for (int i = 0; i < CloudSystem.instance.maxThreadCount; i++)
			{
				helperThreads[i] = new Thread(Worker);
				helperParams[i] = new ThreadParams
				{
					start = maxMeshParticles / CloudSystem.instance.threadCount * (i % CloudSystem.instance.threadCount),
					end = maxMeshParticles / CloudSystem.instance.threadCount * (i % CloudSystem.instance.threadCount + 1),
					running = (i < CloudSystem.instance.threadCount),
					abort = false,
					verts = vertices,
					cols = colors,
					psort = particleSort,
					vpCount = visibleParticlesCount,
					startHandle = new AutoResetEvent(initialState: false),
					completeHandle = new AutoResetEvent(initialState: false)
				};
				myThreads[i] = ((i < CloudSystem.instance.threadCount) ? i : (-1));
				helperThreads[i].Start(helperParams[i]);
			}
			return;
		}
		for (int j = 0; j < CloudSystem.instance.threadCount; j++)
		{
			myThreads[j] = -1;
			for (int k = 0; k < CloudSystem.instance.maxThreadCount; k++)
			{
				if (!helperParams[k].running)
				{
					helperParams[k].start = maxMeshParticles / CloudSystem.instance.threadCount * j;
					helperParams[k].end = maxMeshParticles / CloudSystem.instance.threadCount * (j + 1);
					helperParams[k].abort = false;
					helperParams[k].verts = vertices;
					helperParams[k].cols = colors;
					helperParams[k].psort = particleSort;
					helperParams[k].vpCount = visibleParticlesCount;
					myThreads[j] = k;
					helperParams[k].running = true;
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		KillThreads();
	}

	public void KillAllThreads()
	{
		if (helperThreads == null)
		{
			return;
		}
		for (int i = 0; i < CloudSystem.instance.maxThreadCount; i++)
		{
			helperParams[i].abort = true;
			helperParams[i].startHandle.Set();
		}
		for (int j = 0; j < CloudSystem.instance.maxThreadCount; j++)
		{
			if (helperParams[j].running)
			{
				helperParams[j].completeHandle.WaitOne(100);
			}
		}
	}

	public void KillThreads()
	{
		if (helperThreads == null)
		{
			return;
		}
		for (int i = 0; i < CloudSystem.instance.threadCount; i++)
		{
			for (int j = 0; j < CloudSystem.instance.maxThreadCount; j++)
			{
				if (myThreads[i] == j)
				{
					helperParams[j].abort = true;
					helperParams[j].startHandle.Set();
					myThreads[i] = -1;
				}
			}
		}
		for (int k = 0; k < CloudSystem.instance.maxThreadCount; k++)
		{
			if (helperParams[k].running)
			{
				helperParams[k].completeHandle.WaitOne(100);
			}
		}
	}

	private void Worker(object options)
	{
		ThreadParams threadParams = options as ThreadParams;
		while (true)
		{
			threadParams.startHandle.WaitOne();
			if (threadParams.abort)
			{
				threadParams.running = false;
				threadParams.cols = null;
				threadParams.verts = null;
			}
			if (threadParams.running)
			{
				BuildMesh(threadParams.start, threadParams.end, threadParams);
			}
			threadParams.completeHandle.Set();
		}
	}

	private void ShellSort(ParticleSort[] inputArray, int length)
	{
		for (int num = length / 2; num > 0; num = ((num / 2 == 0) ? ((num != 1) ? 1 : 0) : (num / 2)))
		{
			for (int i = 0; i < length; i++)
			{
				int num2 = i;
				ParticleSort particleSort = inputArray[i];
				while (num2 >= num && inputArray[num2 - num].distance > particleSort.distance)
				{
					inputArray[num2] = inputArray[num2 - num];
					num2 -= num;
				}
				inputArray[num2] = particleSort;
			}
		}
	}

	public void StartUpdate()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		Camera component = ((Component)this).GetComponent<Camera>();
		if ((Object)(object)component == (Object)null)
		{
			return;
		}
		camX = ((Component)component).get_transform().get_right();
		camY = ((Component)component).get_transform().get_up();
		CullClouds(component);
		SortParticles(component);
		camX = ((Component)component).get_transform().get_right();
		camY = ((Component)component).get_transform().get_up();
		for (int i = 0; i < CloudSystem.instance.threadCount; i++)
		{
			if (myThreads[i] != -1)
			{
				helperParams[myThreads[i]].camX = camX;
				helperParams[myThreads[i]].camY = camY;
				helperParams[myThreads[i]].vpCount = visibleParticlesCount;
				helperParams[myThreads[i]].startHandle.Set();
			}
		}
		updateStarted = true;
	}

	private void OnPreRender()
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		if (!updateStarted)
		{
			return;
		}
		updateStarted = false;
		bool flag = true;
		for (int i = 0; i < CloudSystem.instance.threadCount; i++)
		{
			if (myThreads[i] != -1 && helperParams[myThreads[i]].running && !helperParams[myThreads[i]].completeHandle.WaitOne(1000))
			{
				flag = false;
			}
		}
		if (flag)
		{
			CloudSystem.instance.mesh.set_vertices(vertices);
			CloudSystem.instance.mesh.set_colors32(colors);
			CloudSystem.instance.mesh.set_bounds(new Bounds(Vector3.get_zero(), Vector3.get_one() * 10000f));
		}
	}

	private void FastSinCos(float x, out float sin, out float cos)
	{
		if (x < 0f)
		{
			sin = 4f / (float)Math.PI * x + 0.405284733f * x * x;
		}
		else
		{
			sin = 4f / (float)Math.PI * x - 0.405284733f * x * x;
		}
		x += (float)Math.PI / 2f;
		if (x > (float)Math.PI)
		{
			x -= (float)Math.PI * 2f;
		}
		if (x < 0f)
		{
			cos = 4f / (float)Math.PI * x + 0.405284733f * x * x;
		}
		else
		{
			cos = 4f / (float)Math.PI * x - 0.405284733f * x * x;
		}
	}

	private void CullClouds(Camera camera)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)camera).get_transform().get_position();
		Vector3 val = MathUtils.WrapSigned(CloudSystem.instance.cloudSetOffset - position, CloudSystem.instance.cloudSetSize) + position;
		GeometryUtility.CalculateFrustumPlanes(camera, mPlanes);
		visibleCloudsCount = 0;
		if ((Object)(object)Game.currentLevel != (Object)null && Game.currentLevel.noClouds)
		{
			return;
		}
		for (int i = 0; i < CloudSystem.instance.maxClouds; i++)
		{
			CloudSystem.CloudData cloudData = CloudSystem.instance.cloudsData[i];
			Vector3 val2 = cloudData.pos + val;
			val2 = MathUtils.WrapSigned(val2 - position, CloudSystem.instance.cloudSetSize) + position;
			Vector3 val3 = val2 - position;
			if (!(((Vector3)(ref val3)).get_magnitude() - Mathf.Max(Mathf.Max(cloudData.size.x, cloudData.size.y), cloudData.size.z) > CloudSystem.instance.farClipEnd) && GeometryUtility.TestPlanesAABB(mPlanes, new Bounds(val2, CloudSystem.instance.cloudsData[i].size)))
			{
				CloudSystem.instance.cloudsData[i].worldPos = val2;
				visibleClouds[visibleCloudsCount++] = i;
			}
		}
	}

	private void SortParticles(Camera camera)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)camera).get_transform().get_position();
		CloudSystem.CloudParticleData[] particlesData = CloudSystem.instance.particlesData;
		visibleParticlesCount = 0;
		for (int i = 0; i < visibleCloudsCount; i++)
		{
			CloudSystem.CloudData cloudData = CloudSystem.instance.cloudsData[visibleClouds[i]];
			int startParticle = cloudData.startParticle;
			int endParticle = cloudData.endParticle;
			for (int j = startParticle; j < endParticle; j++)
			{
				Vector3 val = particlesData[j].pos + cloudData.worldPos;
				Vector3 val2 = val - position;
				float magnitude = ((Vector3)(ref val2)).get_magnitude();
				if (!(magnitude <= CloudSystem.instance.nearClipStart) && !(magnitude >= CloudSystem.instance.farClipEnd))
				{
					particleSort[visibleParticlesCount].index = j;
					particleSort[visibleParticlesCount].worldPos = val;
					particleSort[visibleParticlesCount].distance = 0f - magnitude;
					visibleParticlesCount++;
				}
			}
		}
		if (visibleParticlesCount > CloudSystem.instance.maxVisibleParticlesCount)
		{
			CloudSystem.instance.maxVisibleParticlesCount = visibleParticlesCount;
		}
		ShellSort(particleSort, visibleParticlesCount);
	}

	private void BuildMesh(int start, int end, ThreadParams tp)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] verts = tp.verts;
		Color32[] cols = tp.cols;
		List<CloudBox> list = new List<CloudBox>();
		lock (CloudBox.cloudLock)
		{
			for (int i = 0; i < CloudBox.all.Count; i++)
			{
				list.Add(CloudBox.all[i]);
			}
		}
		int num = Mathf.Clamp(tp.vpCount - maxMeshParticles, 0, tp.vpCount);
		Vector3 val = default(Vector3);
		Vector3 val2 = default(Vector3);
		for (int j = start; j < end; j++)
		{
			if (j >= tp.vpCount)
			{
				verts[j * 4] = (verts[j * 4 + 1] = (verts[j * 4 + 2] = (verts[j * 4 + 3] = Vector3.get_zero())));
				continue;
			}
			ParticleSort obj = tp.psort[j + num];
			int index = obj.index;
			CloudSystem.CloudParticleData cloudParticleData = CloudSystem.instance.particlesData[index];
			float size = cloudParticleData.size;
			float angle = cloudParticleData.angle;
			Color color = cloudParticleData.color;
			Vector3 worldPos = obj.worldPos;
			float distance = 0f - obj.distance;
			FastSinCos(angle, out var sin, out var cos);
			float num2 = (sin + cos) * size;
			float num3 = (cos - sin) * size;
			((Vector3)(ref val))._002Ector(tp.camX.x * num3 + tp.camY.x * num2, tp.camX.y * num3 + tp.camY.y * num2, tp.camX.z * num3 + tp.camY.z * num2);
			((Vector3)(ref val2))._002Ector(tp.camX.x * num2 - tp.camY.x * num3, tp.camX.y * num2 - tp.camY.y * num3, tp.camX.z * num2 - tp.camY.z * num3);
			float a = color.a;
			a = CloudSystem.instance.DistanceClipAlpha(a, distance);
			int num4 = 0;
			while (a > 0.01f && num4 < list.Count)
			{
				a *= list[num4].GetAlpha(worldPos);
				num4++;
			}
			color.a = a;
			if (a > 0.01f)
			{
				verts[j * 4] = new Vector3(worldPos.x - val.x, worldPos.y - val.y, worldPos.z - val.z);
				verts[j * 4 + 1] = new Vector3(worldPos.x - val2.x, worldPos.y - val2.y, worldPos.z - val2.z);
				verts[j * 4 + 2] = new Vector3(worldPos.x + val.x, worldPos.y + val.y, worldPos.z + val.z);
				verts[j * 4 + 3] = new Vector3(worldPos.x + val2.x, worldPos.y + val2.y, worldPos.z + val2.z);
			}
			else
			{
				verts[j * 4] = worldPos;
				verts[j * 4 + 1] = worldPos;
				verts[j * 4 + 2] = worldPos;
				verts[j * 4 + 3] = worldPos;
			}
			cols[j * 4] = (cols[j * 4 + 1] = (cols[j * 4 + 2] = (cols[j * 4 + 3] = Color32.op_Implicit(color))));
		}
	}

	public CloudRender()
		: this()
	{
	}
}
