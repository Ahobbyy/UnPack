using I2.Loc;
using Multiplayer;
using UnityEngine;

public class FreeRoamCam : MonoBehaviour
{
	public struct CameraKeyFrame
	{
		public float frame;

		public bool targetFocus;

		public bool humanRelative;

		public Vector3 pos;

		public Vector3 targetPos;

		public Quaternion rot;
	}

	private Camera cam;

	public GameObject logoOverlay;

	private Vector3 pos1;

	private Vector3 pos2;

	private Quaternion rot1;

	private Quaternion rot2;

	private float duration = 10f;

	private float animationPhase = 1f;

	public static bool allowFreeRoam;

	private CameraKeyFrame[] keyframes = new CameraKeyFrame[9];

	private const float kFogSpeed = 10f;

	private const float kFogMin = 0f;

	private const float kFogMax = 60f;

	private bool coopCameraToggle;

	private void OnEnable()
	{
		ResetKeyFrames();
	}

	private void ResetKeyFrames()
	{
		for (int i = 0; i < keyframes.Length; i++)
		{
			keyframes[i].frame = -1f;
		}
		if ((Object)(object)ReplayUI.instance != (Object)null)
		{
			ReplayUI.instance.HideCameras();
		}
	}

	public static void CleanUp()
	{
		allowFreeRoam = false;
		MenuCameraEffects.instance.RemoveOverride();
	}

	private void Update()
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Game.instance == (Object)null)
		{
			return;
		}
		if (NetGame.isLocal && Game.GetKeyDown((KeyCode)287))
		{
			if (Time.get_timeScale() == 0f)
			{
				Time.set_timeScale(1f);
			}
			else
			{
				Time.set_timeScale(0f);
			}
		}
		if (Input.GetKeyDown((KeyCode)288) && NetGame.instance.players.Count == 2)
		{
			MenuCameraEffects.instance.SetCOOPFullScreenViewport(coopCameraToggle);
			coopCameraToggle = !coopCameraToggle;
		}
		if (Game.GetKeyDown((KeyCode)289))
		{
			ResetKeyFrames();
			if (!allowFreeRoam)
			{
				Camera gameCam = NetGame.instance.local.players[0].cameraController.gameCam;
				((Component)this).get_transform().set_position(((Component)gameCam).get_transform().get_position());
				((Component)this).get_transform().set_rotation(((Component)gameCam).get_transform().get_rotation());
				allowFreeRoam = true;
				cam = MenuCameraEffects.instance.OverrideCamera(((Component)this).get_transform(), applyEffects: true);
			}
			else
			{
				CleanUp();
			}
		}
		if (NetGame.isLocal && Game.GetKeyDown((KeyCode)116) && Game.GetKey((KeyCode)304) && Game.GetKey((KeyCode)306))
		{
			if (Time.get_timeScale() == 1f)
			{
				Time.set_timeScale(0.5f);
			}
			else
			{
				Time.set_timeScale(1f);
			}
		}
		if (Game.GetKey((KeyCode)91))
		{
			MenuCameraEffects.instance.zoom.to *= Mathf.Pow(1.4f, Time.get_unscaledDeltaTime());
		}
		if (Game.GetKey((KeyCode)93))
		{
			MenuCameraEffects.instance.zoom.to /= Mathf.Pow(1.4f, Time.get_unscaledDeltaTime());
		}
		if (Game.GetKey((KeyCode)44))
		{
			CaveRender.fogDensityMultiplier -= 10f * Time.get_unscaledDeltaTime();
		}
		if (Game.GetKey((KeyCode)46))
		{
			CaveRender.fogDensityMultiplier += 10f * Time.get_unscaledDeltaTime();
		}
		CaveRender.fogDensityMultiplier = Mathf.Clamp(CaveRender.fogDensityMultiplier, 0f, 60f);
		if (Game.GetKeyDown((KeyCode)290))
		{
			Camera gameCam2 = cam;
			if ((Object)(object)gameCam2 == (Object)null)
			{
				gameCam2 = NetGame.instance.local.players[0].cameraController.gameCam;
			}
			if (!((Behaviour)gameCam2).get_isActiveAndEnabled())
			{
				MultiplayerLobbyController multiplayerLobbyController = Object.FindObjectOfType<MultiplayerLobbyController>();
				if ((Object)(object)multiplayerLobbyController != (Object)null)
				{
					gameCam2 = multiplayerLobbyController.gameCamera.gameCam;
				}
			}
			if ((Object)(object)gameCam2 != (Object)null)
			{
				RenderTexture val = new RenderTexture(1024, 576, 16);
				gameCam2.set_targetTexture(val);
				gameCam2.Render();
				RenderTexture.set_active(val);
				Texture2D val2 = new Texture2D(((Texture)val).get_width(), ((Texture)val).get_height(), (TextureFormat)3, false);
				val2.ReadPixels(new Rect(0f, 0f, (float)((Texture)val).get_width(), (float)((Texture)val).get_height()), 0, 0);
				RenderTexture.set_active((RenderTexture)null);
				gameCam2.set_targetTexture((RenderTexture)null);
				FileTools.WriteTexture(FileTools.Combine(Application.get_persistentDataPath(), "thumbnail.png"), val2);
				Object.Destroy((Object)(object)val);
				SubtitleManager.instance.SetProgress(ScriptLocalization.Get("WORKSHOP/ThumbnailCaptured"), 2f, 0.5f);
			}
		}
		if (!allowFreeRoam)
		{
			return;
		}
		if (Game.GetKeyDown((KeyCode)48))
		{
			ResetKeyFrames();
		}
		for (int i = 0; i < keyframes.Length; i++)
		{
			if (Game.GetKeyDown((KeyCode)(49 + i)))
			{
				Human human = Human.all[0];
				keyframes[i] = new CameraKeyFrame
				{
					pos = ((Component)this).get_transform().get_position(),
					targetPos = ((Component)human).get_transform().get_position(),
					rot = ((Component)this).get_transform().get_rotation(),
					targetFocus = (Input.GetKey((KeyCode)304) || Input.GetKey((KeyCode)303) || Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305) || Input.GetKey((KeyCode)9)),
					humanRelative = (Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)305) || Input.GetKey((KeyCode)9)),
					frame = ReplayRecorder.instance.currentFrame
				};
				if (keyframes[i].humanRelative)
				{
					ref Vector3 pos = ref keyframes[i].pos;
					pos -= ((Component)human).get_transform().get_position();
				}
				ReplayUI.instance.SyncCameras(keyframes);
			}
		}
		if (ReplayRecorder.instance.state == ReplayRecorder.ReplayState.PlayForward || ReplayRecorder.instance.state == ReplayRecorder.ReplayState.PlayBackward)
		{
			for (int j = 0; j < keyframes.Length; j++)
			{
				CameraKeyFrame cameraKeyFrame = keyframes[j];
				if (cameraKeyFrame.frame < 0f)
				{
					break;
				}
				if (cameraKeyFrame.frame >= (float)ReplayRecorder.instance.currentFrame)
				{
					CameraKeyFrame prevKeyframe = ((j > 0) ? keyframes[j - 1] : cameraKeyFrame);
					float t = Mathf.InverseLerp(prevKeyframe.frame, cameraKeyFrame.frame, (float)ReplayRecorder.instance.currentFrame);
					SyncCamera(cameraKeyFrame, prevKeyframe, t);
					return;
				}
			}
		}
		else if (ReplayRecorder.instance.state == ReplayRecorder.ReplayState.None && keyframes[0].pos != Vector3.get_zero() && keyframes[1].pos != Vector3.get_zero())
		{
			if (Game.GetKeyDown((KeyCode)32))
			{
				if (animationPhase < 1f)
				{
					animationPhase = 1f;
				}
				else
				{
					animationPhase = 0f;
				}
			}
			if (animationPhase < 1f)
			{
				animationPhase += Time.get_unscaledDeltaTime() / duration;
				SyncCamera(keyframes[1], keyframes[0], animationPhase);
				return;
			}
		}
		bool num = Game.GetKey((KeyCode)304) || Input.GetKey((KeyCode)303);
		int num2 = (num ? 1 : 10);
		if (Game.GetKey((KeyCode)119))
		{
			Transform transform = ((Component)this).get_transform();
			transform.set_position(transform.get_position() + ((Component)this).get_transform().get_forward() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		if (Game.GetKey((KeyCode)115))
		{
			Transform transform2 = ((Component)this).get_transform();
			transform2.set_position(transform2.get_position() - ((Component)this).get_transform().get_forward() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		if (Game.GetKey((KeyCode)97))
		{
			Transform transform3 = ((Component)this).get_transform();
			transform3.set_position(transform3.get_position() - ((Component)this).get_transform().get_right() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		if (Game.GetKey((KeyCode)100))
		{
			Transform transform4 = ((Component)this).get_transform();
			transform4.set_position(transform4.get_position() + ((Component)this).get_transform().get_right() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		if (Game.GetKey((KeyCode)113))
		{
			Transform transform5 = ((Component)this).get_transform();
			transform5.set_position(transform5.get_position() + ((Component)this).get_transform().get_up() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		if (Game.GetKey((KeyCode)122))
		{
			Transform transform6 = ((Component)this).get_transform();
			transform6.set_position(transform6.get_position() - ((Component)this).get_transform().get_up() * Time.get_unscaledDeltaTime() * (float)num2);
		}
		float y = Input.get_mouseScrollDelta().y;
		Camera obj = cam;
		obj.set_fieldOfView(obj.get_fieldOfView() * Mathf.Pow(1.1f, 0f - y));
		cam.set_fieldOfView(Mathf.Clamp(cam.get_fieldOfView(), 5f, 120f));
		cam.set_nearClipPlane(0.05f * Mathf.Pow(1.1f, 0f - Mathf.Log(cam.get_fieldOfView() / 60f, 1.1f)));
		if (Input.GetMouseButton(2))
		{
			cam.set_fieldOfView(60f);
			cam.set_nearClipPlane(0.05f);
		}
		float num3 = (num ? 0.1f : 0.2f);
		float axis = Input.GetAxis("mouse x");
		float axis2 = Input.GetAxis("mouse y");
		if (axis != 0f || axis2 != 0f)
		{
			Quaternion rotation = ((Component)this).get_transform().get_rotation();
			Vector3 eulerAngles = ((Quaternion)(ref rotation)).get_eulerAngles();
			if (eulerAngles.x > 180f)
			{
				eulerAngles.x -= 360f;
			}
			if (eulerAngles.x < -180f)
			{
				eulerAngles.x += 360f;
			}
			eulerAngles.x -= axis2 * num3;
			if (eulerAngles.x < -89f)
			{
				eulerAngles.x = -89f;
			}
			if (eulerAngles.x > 89f)
			{
				eulerAngles.x = 89f;
			}
			eulerAngles.y += axis * num3;
			((Component)this).get_transform().set_rotation(Quaternion.Euler(eulerAngles));
		}
	}

	private void SyncCamera(CameraKeyFrame keyframe, CameraKeyFrame prevKeyframe, float t)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		Human human = Human.all[0];
		Vector3 val = prevKeyframe.pos;
		Quaternion val2 = prevKeyframe.rot;
		Vector3 val3 = keyframe.pos;
		Quaternion val4 = keyframe.rot;
		if (prevKeyframe.humanRelative)
		{
			val += ((Component)human).get_transform().get_position();
		}
		if (keyframe.humanRelative)
		{
			val3 += ((Component)human).get_transform().get_position();
		}
		Vector3 val5 = Vector3.Lerp(val, val3, t);
		Quaternion val6 = Quaternion.LookRotation(Vector3.Lerp(prevKeyframe.targetPos, keyframe.targetPos, t) - val5, Vector3.get_up());
		Quaternion val7 = Quaternion.LookRotation(((Component)human).get_transform().get_position() - val5, Vector3.get_up());
		if (prevKeyframe.targetFocus)
		{
			val2 = val6;
		}
		if (keyframe.targetFocus)
		{
			val4 = val6;
		}
		if (prevKeyframe.humanRelative)
		{
			val2 = val7;
		}
		if (keyframe.humanRelative)
		{
			val4 = val7;
		}
		Quaternion rotation = Quaternion.Lerp(val2, val4, t);
		((Component)this).get_transform().set_position(val5);
		((Component)this).get_transform().set_rotation(rotation);
	}

	public FreeRoamCam()
		: this()
	{
	}
}
