using System.IO;
using UnityEngine;

public class ScreenshotMovie : MonoBehaviour
{
	public string folder = "ScreenshotMovieOutput";

	public int frameRate = 60;

	public int sizeMultiplier = 1;

	private string realFolder = "";

	private bool recording;

	private int frame;

	private void Update()
	{
		if (Input.GetKeyDown((KeyCode)114) && Input.GetKey((KeyCode)304) && Input.GetKey((KeyCode)306))
		{
			if (!recording)
			{
				Time.set_captureFramerate(frameRate);
				realFolder = Path.Combine(Application.get_persistentDataPath(), folder);
				int num = 1;
				while (Directory.Exists(realFolder))
				{
					realFolder = Path.Combine(Application.get_persistentDataPath(), folder + num);
					num++;
				}
				Directory.CreateDirectory(realFolder);
				recording = true;
				frame = 0;
			}
			else
			{
				Time.set_captureFramerate(0);
				recording = false;
			}
		}
		if (recording)
		{
			ScreenCapture.CaptureScreenshot($"{realFolder}/shot {frame++:D04}.png", sizeMultiplier);
		}
	}

	public ScreenshotMovie()
		: this()
	{
	}
}
