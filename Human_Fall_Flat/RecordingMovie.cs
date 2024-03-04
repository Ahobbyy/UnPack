using UnityEngine;

public class RecordingMovie : MonoBehaviour
{
	public bool disableHuman;

	public AudioClip soundtrack;

	private RecordingManager recordingManager;

	private Camera playerCam;

	private RecordingMovieCamera[] cameraPositions;

	private RecordingMovieClip[] clips;

	public bool playing;

	public int currentFrame;

	public int startOffset;

	public float playbackStarted;

	public float lastTime;

	public float currentTime;

	public Animation recordedAnimation;

	public int animationFrame;

	public int fileFrame;

	private void Start()
	{
		playerCam = Camera.get_main();
		cameraPositions = ((Component)this).GetComponentsInChildren<RecordingMovieCamera>();
		clips = ((Component)this).GetComponentsInChildren<RecordingMovieClip>();
		recordingManager = Object.FindObjectOfType<RecordingManager>();
	}

	private void LateUpdate()
	{
		if (Input.GetKeyDown((KeyCode)54))
		{
			if ((Object)(object)recordedAnimation != (Object)null)
			{
				animationFrame++;
				ScreenCapture.CaptureScreenshot("RealSenseA" + fileFrame.ToString("00") + ".png");
				recordedAnimation.get_clip().SampleAnimation(((Component)recordedAnimation).get_gameObject(), 1f * (float)animationFrame / 60f);
				fileFrame++;
			}
			else
			{
				ScreenCapture.CaptureScreenshot("Screenshot.png");
			}
		}
		if (Input.GetKeyDown((KeyCode)49))
		{
			((Component)this).GetComponent<AudioSource>().PlayOneShot(soundtrack);
		}
		if (Input.GetKeyDown((KeyCode)53))
		{
			playbackStarted = Time.get_time();
			lastTime = 0f;
			playerCam.set_orthographic(true);
			((Behaviour)((Component)playerCam).GetComponent<CameraController3>()).set_enabled(false);
			playing = true;
			currentFrame = startOffset;
			AudioSource component = ((Component)this).GetComponent<AudioSource>();
			if ((Object)(object)component != (Object)null)
			{
				component.set_clip(soundtrack);
				component.set_timeSamples(soundtrack.get_frequency() * startOffset / 60);
				component.Play();
			}
			if (disableHuman)
			{
				((Component)Human.instance).get_gameObject().SetActive(false);
			}
		}
		if (!playing)
		{
			return;
		}
		currentTime = Time.get_time() - playbackStarted;
		for (int i = 0; i < clips.Length; i++)
		{
			if (lastTime <= clips[i].beginOnTime && clips[i].beginOnTime < currentTime)
			{
				Object.FindObjectOfType<RecordingManager>().BeginPlayback(clips[i].bytes, clips[i].startTime);
			}
		}
		lastTime = currentTime;
		for (int j = 0; j < cameraPositions.Length; j++)
		{
			if (cameraPositions[j].startTime <= currentTime && cameraPositions[j].endTime >= currentTime)
			{
				PlaceCam(cameraPositions[j]);
				currentFrame++;
				return;
			}
		}
		RecordingMovieCamera startCam = null;
		for (int k = 0; k < cameraPositions.Length; k++)
		{
			if (cameraPositions[k].endTime < currentTime)
			{
				startCam = cameraPositions[k];
			}
		}
		RecordingMovieCamera endCam = null;
		for (int num = cameraPositions.Length - 1; num >= 0; num--)
		{
			if (cameraPositions[num].startTime > currentTime)
			{
				endCam = cameraPositions[num];
			}
		}
		InterpolateCam(startCam, endCam);
		currentFrame++;
	}

	private void InterpolateCam(RecordingMovieCamera startCam, RecordingMovieCamera endCam)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)startCam == (Object)null) && !((Object)(object)endCam == (Object)null))
		{
			float value = (1f * currentTime - 1f * startCam.endTime) / (1f * endCam.startTime - 1f * startCam.endTime);
			float num = Ease.easeInOutQuad(0f, 1f, value);
			((Component)playerCam).get_transform().set_rotation(Quaternion.Lerp(((Component)startCam).get_transform().get_rotation(), ((Component)endCam).get_transform().get_rotation(), num));
			Vector3 val = Vector3.Lerp(((Component)startCam).get_transform().get_position(), ((Component)endCam).get_transform().get_position(), num);
			((Component)playerCam).get_transform().set_position(val - ((Component)playerCam).get_transform().get_forward() * Mathf.Lerp(startCam.dist, endCam.dist, num));
			playerCam.set_orthographicSize(Mathf.Lerp(startCam.ortographicsSize, endCam.ortographicsSize, num));
			playerCam.set_nearClipPlane(Mathf.Lerp(startCam.nearClip, endCam.nearClip, num));
			playerCam.set_farClipPlane(Mathf.Lerp(startCam.farClip, endCam.farClip, num));
		}
	}

	private void PlaceCam(RecordingMovieCamera cam)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		((Component)playerCam).get_transform().set_rotation(((Component)cam).get_transform().get_rotation());
		Vector3 position = ((Component)cam).get_transform().get_position();
		((Component)playerCam).get_transform().set_position(position - ((Component)playerCam).get_transform().get_forward() * cam.dist);
		playerCam.set_orthographicSize(cam.ortographicsSize);
		playerCam.set_nearClipPlane(cam.nearClip);
		playerCam.set_farClipPlane(cam.farClip);
	}

	public RecordingMovie()
		: this()
	{
	}
}
