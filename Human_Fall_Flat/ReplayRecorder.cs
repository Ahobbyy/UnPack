using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class ReplayRecorder : MonoBehaviour
{
	public enum ReplayState
	{
		None,
		Record,
		Stop,
		PlayForward,
		PlayBackward
	}

	public static ReplayRecorder instance;

	public ReplayState state;

	public int currentFrame;

	public float recordingStartTime;

	private int lastFrame;

	private Dictionary<uint, NetFrames> capture = new Dictionary<uint, NetFrames>();

	public static bool isRecording => instance.state == ReplayState.Record;

	public static bool isPlaying
	{
		get
		{
			if (instance.state != ReplayState.PlayForward && instance.state != ReplayState.PlayBackward)
			{
				return instance.state == ReplayState.Stop;
			}
			return true;
		}
	}

	public static float time
	{
		get
		{
			if (instance.state == ReplayState.None)
			{
				return Time.get_time();
			}
			return instance.recordingStartTime + (float)instance.currentFrame * Time.get_fixedDeltaTime();
		}
	}

	private void Awake()
	{
		instance = this;
	}

	public void PostSimulate()
	{
		if ((Object)(object)Game.instance == (Object)null)
		{
			return;
		}
		if (Game.instance.state != GameState.PlayingLevel)
		{
			state = ReplayState.None;
			SubtitleManager.instance.ClearRecording();
		}
		if (state == ReplayState.None)
		{
			return;
		}
		if (state == ReplayState.Record)
		{
			CaptureFrame();
			return;
		}
		PlaybackFrame(currentFrame);
		if (state == ReplayState.PlayForward)
		{
			currentFrame++;
		}
		else if (state == ReplayState.PlayBackward)
		{
			currentFrame--;
		}
		currentFrame = Mathf.Clamp(currentFrame, 0, lastFrame);
	}

	public void PreLateUpdate()
	{
	}

	private void Update()
	{
		if ((Object)(object)Game.instance == (Object)null)
		{
			return;
		}
		if (Game.instance.state != GameState.PlayingLevel || NetGame.isClient)
		{
			Abort();
			return;
		}
		if (state == ReplayState.Stop)
		{
			ReplayUI.instance.Show(lastFrame);
		}
		else
		{
			ReplayUI.instance.Hide();
		}
		if (Input.GetKeyDown((KeyCode)268))
		{
			if (state == ReplayState.None)
			{
				ReplayUI.instance.Hide();
				BeginRecording();
				CheatCodes.NotifyCheat("REC");
				SubtitleManager.instance.SetRecording();
			}
			else if (state == ReplayState.Record)
			{
				StopRecording();
			}
			else
			{
				state = ReplayState.None;
			}
		}
		if (state == ReplayState.None || state == ReplayState.Record || state == ReplayState.None)
		{
			return;
		}
		bool key = Input.GetKey((KeyCode)304);
		if (Input.GetKey((KeyCode)276))
		{
			currentFrame -= ((!key) ? 1 : 10);
		}
		if (Input.GetKey((KeyCode)275))
		{
			currentFrame += ((!key) ? 1 : 10);
		}
		if (Input.GetKeyDown((KeyCode)60))
		{
			currentFrame--;
		}
		if (Input.GetKeyDown((KeyCode)62))
		{
			currentFrame++;
		}
		if (Input.GetKeyDown((KeyCode)32))
		{
			if (state == ReplayState.Stop)
			{
				state = (key ? ReplayState.PlayBackward : ReplayState.PlayForward);
			}
			else
			{
				state = ReplayState.Stop;
			}
		}
		currentFrame = Mathf.Clamp(currentFrame, 0, lastFrame);
	}

	public static void Stop()
	{
		if (isRecording)
		{
			instance.StopRecording();
		}
	}

	public static void Abort()
	{
		if (isRecording)
		{
			instance.StopRecording();
		}
		if (isPlaying)
		{
			instance.state = ReplayState.None;
		}
	}

	private void StopRecording()
	{
		SubtitleManager.instance.ClearRecording();
		state = ReplayState.Stop;
		currentFrame = lastFrame;
	}

	private void BeginRecording()
	{
		state = ReplayState.Record;
		recordingStartTime = Time.get_time();
		lastFrame = 0;
		currentFrame = 0;
		capture.Clear();
	}

	private void CaptureFrame()
	{
		lastFrame = currentFrame;
		currentFrame++;
	}

	public void SubmitFrame(NetScope scope, NetStream full, NetStream events)
	{
		if (!capture.TryGetValue(scope.netId, out var value))
		{
			value = new NetFrames();
			capture[scope.netId] = value;
		}
		if (full != null)
		{
			value.PushState(currentFrame, full.AddRef());
		}
		if (events != null)
		{
			value.PushEvents(currentFrame, events.AddRef());
		}
	}

	public void Play(NetScope scope)
	{
		if (!scope.exitingLevel && capture.TryGetValue(scope.netId, out var value))
		{
			scope.RenderState(value, currentFrame, 0f);
			if (state == ReplayState.PlayForward)
			{
				scope.PlaybackEvents(value, currentFrame - 1, currentFrame);
			}
		}
	}

	private void PlaybackFrame(int frame)
	{
	}

	public ReplayRecorder()
		: this()
	{
	}
}
