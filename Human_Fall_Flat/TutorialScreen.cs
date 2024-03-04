using HumanAPI;
using I2.Loc;
using UnityEngine;
using UnityEngine.Video;

public class TutorialScreen : MonoBehaviour
{
	public string videoName;

	public TextAsset subtitlesAsset;

	public Material blankMaterial;

	public Material movieMaterial;

	public Sound2 onSound;

	public Sound2 offSound;

	public Sound2 tutSound;

	public float width = 16f;

	private Renderer renderer;

	private BoxCollider collider;

	private VideoPlayer movie;

	private bool videoLoaded;

	private bool reinitMaterial;

	private AudioSource audiosource;

	private MeshFilter filter;

	private SrtSubtitles subtitles;

	private Mesh mesh;

	private Vector3[] meshVerts;

	private Vector2[] uvs;

	private Material movieMaterialInstance;

	private float endTimer;

	private static TutorialScreen playingScreen;

	private float pendingPlay = -1f;

	private float audiosourceStart;

	private float transitionSpeed;

	private float transitionPhase;

	private bool inTransition;

	private SrtSubtitles.SrtLine currentLine;

	public float time;

	public bool isVisible;

	public static bool lockedVOAndSubtitles { get; protected set; }

	private float audiosourceTime
	{
		get
		{
			if ((Object)(object)audiosource == (Object)null)
			{
				return 0f;
			}
			float num = audiosource.get_time();
			if (float.IsInfinity(num) || float.IsNaN(num))
			{
				num = 1f * (float)audiosource.get_timeSamples() / 48000f;
			}
			if (num == 0f)
			{
				return Time.get_unscaledTime() - audiosourceStart;
			}
			return num;
		}
	}

	private void Start()
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		LoadMovie(videoName);
		TextAsset videoSrt = HFFResources.instance.GetVideoSrt(videoName + "Srt");
		subtitles = new SrtSubtitles();
		subtitles.Load(videoName, videoSrt.get_text());
		videoSrt = null;
		collider = ((Component)this).GetComponent<BoxCollider>();
		renderer = ((Component)this).GetComponent<Renderer>();
		filter = ((Component)this).GetComponent<MeshFilter>();
		mesh = new Mesh();
		((Object)mesh).set_name("rope " + ((Object)this).get_name());
		meshVerts = (Vector3[])(object)new Vector3[8];
		uvs = (Vector2[])(object)new Vector2[8];
		Create2SidedMesh(0f, -4.5f, 0f, 4.5f, 0f, 0f, 1f, 1f);
		mesh.set_vertices(meshVerts);
		mesh.set_uv(uvs);
		mesh.set_triangles(new int[12]
		{
			0, 1, 2, 0, 2, 3, 4, 5, 6, 4,
			6, 7
		});
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		filter.set_sharedMesh(mesh);
	}

	private void PrepareCompleted(VideoPlayer source)
	{
		source.set_isLooping(false);
		videoLoaded = true;
	}

	private void LoadMovie(string file)
	{
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		if ((Object)(object)movie == (Object)null)
		{
			movie = ((Component)this).get_gameObject().GetComponent<VideoPlayer>();
			if ((Object)(object)movie == (Object)null)
			{
				movie = ((Component)this).get_gameObject().AddComponent<VideoPlayer>();
			}
		}
		if ((Object)(object)audiosource == (Object)null)
		{
			audiosource = ((Component)this).get_gameObject().GetComponent<AudioSource>();
			if ((Object)(object)audiosource == (Object)null)
			{
				audiosource = ((Component)this).get_gameObject().AddComponent<AudioSource>();
			}
			if ((Object)(object)audiosource != (Object)null)
			{
				VideoLogMenu menu = MenuSystem.instance.GetMenu<VideoLogMenu>();
				if ((Object)(object)menu != (Object)null)
				{
					audiosource.set_outputAudioMixerGroup(menu.audioSource.get_outputAudioMixerGroup());
				}
			}
		}
		movie.set_playOnAwake(false);
		audiosource.set_playOnAwake(false);
		movie.set_source((VideoSource)1);
		string url = Application.get_streamingAssetsPath() + "/Videos/" + file + "Video.mp4";
		movie.set_url(url);
		movie.set_audioOutputMode((VideoAudioOutputMode)1);
		movie.set_controlledAudioTrackCount((ushort)1);
		movie.EnableAudioTrack((ushort)0, true);
		movie.SetTargetAudioSource((ushort)0, audiosource);
		movie.add_prepareCompleted(new EventHandler(PrepareCompleted));
		movie.Prepare();
	}

	private void Update()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		if (pendingPlay > 0f)
		{
			float num = pendingPlay - Time.get_deltaTime();
			Show();
			pendingPlay = num;
		}
		if (!videoLoaded)
		{
			return;
		}
		if (endTimer > 0f)
		{
			endTimer -= Time.get_deltaTime();
			if (endTimer <= 0f)
			{
				StopMovie();
			}
		}
		if ((Object)(object)movie != (Object)null && (Object)(object)movieMaterialInstance == (Object)null)
		{
			movieMaterialInstance = new Material(movieMaterial);
			reinitMaterial = true;
		}
		if (reinitMaterial && (Object)(object)movie != (Object)null && (Object)(object)movieMaterialInstance != (Object)null)
		{
			reinitMaterial = false;
			if ((Object)(object)movieMaterialInstance != (Object)null)
			{
				movieMaterialInstance.set_mainTexture(movie.get_texture());
				movieMaterialInstance.SetTexture("_EmissionMap", movie.get_texture());
			}
			renderer.set_sharedMaterial(movieMaterialInstance);
		}
		if ((Object)(object)movie != (Object)null && subtitles != null && transitionPhase != 0f && transitionSpeed > 0f && inTransition)
		{
			time = audiosourceTime;
			if (currentLine != null && !currentLine.ShouldShow(time))
			{
				if (lockedVOAndSubtitles)
				{
					SubtitleManager.instance.ClearSubtitle();
				}
				currentLine = null;
			}
			if (currentLine == null)
			{
				currentLine = subtitles.GetLineToDisplay(time);
				if (currentLine != null && lockedVOAndSubtitles)
				{
					SubtitleManager.instance.SetSubtitle(ScriptLocalization.Get("SUBTITLES/" + currentLine.key));
				}
			}
		}
		if (!inTransition)
		{
			return;
		}
		if (transitionPhase == 0f && transitionSpeed > 0f && (Object)(object)movie != (Object)null)
		{
			renderer.set_sharedMaterial(movieMaterialInstance);
			audiosourceStart = Time.get_unscaledTime();
			lockedVOAndSubtitles = true;
			movie.Play();
		}
		transitionPhase = Mathf.Clamp01(transitionPhase + Time.get_deltaTime() * transitionSpeed);
		float num2 = Ease.easeInOutQuad(0f, 1f, transitionPhase);
		float num3 = width / 16f * 9f;
		Create2SidedMesh((0f - width) / 2f * num2, (0f - num3) / 2f, width / 2f * num2, num3 / 2f, 0.5f - num2 / 2f, 0f, 0.5f + num2 / 2f, 1f);
		mesh.set_vertices(meshVerts);
		mesh.set_uv(uvs);
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		filter.set_sharedMesh(mesh);
		collider.set_size((Vector3)((num2 == 0f) ? Vector3.get_zero() : new Vector3(width * num2, num3, 0.1f)));
		if (transitionPhase == 0f && transitionSpeed < 0f)
		{
			if ((Object)(object)playingScreen == (Object)(object)this)
			{
				playingScreen = null;
			}
			inTransition = false;
			StopMovie();
			RemoveSubtitles();
			renderer.set_sharedMaterial(blankMaterial);
			GrabManager.Release(((Component)this).get_gameObject());
		}
	}

	internal void StopXBAudio()
	{
	}

	internal void RemoveSubtitles()
	{
		if (currentLine != null || lockedVOAndSubtitles)
		{
			SubtitleManager.instance.ClearSubtitle();
		}
		currentLine = null;
		lockedVOAndSubtitles = false;
	}

	internal void StopMovie()
	{
		if (!((Object)(object)movie == (Object)null))
		{
			movie.Stop();
			if (videoLoaded)
			{
				videoLoaded = false;
				movie.Prepare();
			}
			reinitMaterial = true;
			StopXBAudio();
			RemoveSubtitles();
		}
	}

	public void OnDestroy()
	{
		InstantKill();
	}

	public void OnDisable()
	{
		InstantKill();
	}

	protected void InstantKill()
	{
		if ((Object)(object)playingScreen == (Object)(object)this)
		{
			StopMovie();
			RemoveSubtitles();
			playingScreen = null;
			transitionSpeed = -2f;
		}
	}

	private void Create2SidedMesh(float xMin, float yMin, float xMax, float yMax, float uMin, float vMin, float uMax, float vMax)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		meshVerts[0] = (meshVerts[7] = new Vector3(xMin, yMin, 0f));
		meshVerts[1] = (meshVerts[6] = new Vector3(xMin, yMax, 0f));
		meshVerts[2] = (meshVerts[5] = new Vector3(xMax, yMax, 0f));
		meshVerts[3] = (meshVerts[4] = new Vector3(xMax, yMin, 0f));
		uvs[0] = (uvs[4] = new Vector2(uMin, vMin));
		uvs[1] = (uvs[5] = new Vector2(uMin, vMax));
		uvs[2] = (uvs[6] = new Vector2(uMax, vMax));
		uvs[3] = (uvs[7] = new Vector2(uMax, vMin));
	}

	internal void Show()
	{
		if ((Object)(object)playingScreen != (Object)null || isVisible || inTransition)
		{
			pendingPlay = 1f;
			return;
		}
		pendingPlay = -1f;
		isVisible = true;
		playingScreen = this;
		inTransition = true;
		transitionSpeed = 0.8f;
		MusicManager.instance.Pause();
		onSound.PlayOneShot();
		lockedVOAndSubtitles = true;
		currentLine = null;
		SubtitleManager.instance.ClearSubtitle();
	}

	internal void Hide()
	{
		pendingPlay = -1f;
		if (!((Object)(object)playingScreen != (Object)(object)this))
		{
			isVisible = false;
			offSound.PlayOneShot();
			inTransition = true;
			transitionSpeed = -2f;
			MusicManager.instance.Resume();
			StopXBAudio();
			RemoveSubtitles();
		}
	}

	public TutorialScreen()
		: this()
	{
	}
}
