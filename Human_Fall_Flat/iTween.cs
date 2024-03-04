using System;
using System.Collections;
using UnityEngine;

public class iTween : MonoBehaviour
{
	private delegate float EasingFunction(float start, float end, float value);

	private delegate void ApplyTween();

	public enum EaseType
	{
		easeInQuad,
		easeOutQuad,
		easeInOutQuad,
		easeInCubic,
		easeOutCubic,
		easeInOutCubic,
		easeInQuart,
		easeOutQuart,
		easeInOutQuart,
		easeInQuint,
		easeOutQuint,
		easeInOutQuint,
		easeInSine,
		easeOutSine,
		easeInOutSine,
		easeInExpo,
		easeOutExpo,
		easeInOutExpo,
		easeInCirc,
		easeOutCirc,
		easeInOutCirc,
		linear,
		spring,
		easeInBounce,
		easeOutBounce,
		easeInOutBounce,
		easeInBack,
		easeOutBack,
		easeInOutBack,
		easeInElastic,
		easeOutElastic,
		easeInOutElastic,
		punch
	}

	public enum LoopType
	{
		none,
		loop,
		pingPong
	}

	public enum NamedValueColor
	{
		_Color,
		_SpecColor,
		_Emission,
		_ReflectColor
	}

	public static class Defaults
	{
		public static float time = 1f;

		public static float delay = 0f;

		public static NamedValueColor namedColorValue = NamedValueColor._Color;

		public static LoopType loopType = LoopType.none;

		public static EaseType easeType = EaseType.easeOutExpo;

		public static float lookSpeed = 3f;

		public static bool isLocal = false;

		public static Space space = (Space)1;

		public static bool orientToPath = false;

		public static Color color = Color.get_white();

		public static float updateTimePercentage = 0.05f;

		public static float updateTime = 1f * updateTimePercentage;

		public static int cameraFadeDepth = 999999;

		public static float lookAhead = 0.05f;

		public static bool useRealTime = false;

		public static Vector3 up = Vector3.get_up();
	}

	private class CRSpline
	{
		public Vector3[] pts;

		public CRSpline(params Vector3[] pts)
		{
			this.pts = (Vector3[])(object)new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		public Vector3 Interp(float t)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			int num = pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 val = pts[num2];
			Vector3 val2 = pts[num2 + 1];
			Vector3 val3 = pts[num2 + 2];
			Vector3 val4 = pts[num2 + 3];
			return 0.5f * ((-val + 3f * val2 - 3f * val3 + val4) * (num3 * num3 * num3) + (2f * val - 5f * val2 + 4f * val3 - val4) * (num3 * num3) + (-val + val3) * num3 + 2f * val2);
		}
	}

	public static ArrayList tweens = new ArrayList();

	private static GameObject cameraFade;

	public string id;

	public string type;

	public string method;

	public EaseType easeType;

	public float time;

	public float delay;

	public LoopType loopType;

	public bool isRunning;

	public bool isPaused;

	public string _name;

	private float runningTime;

	private float percentage;

	private float delayStarted;

	private bool kinematic;

	private bool isLocal;

	private bool loop;

	private bool reverse;

	private bool wasPaused;

	private bool physics;

	private Hashtable tweenArguments;

	private Space space;

	private EasingFunction ease;

	private ApplyTween apply;

	private AudioSource audioSource;

	private Vector3[] vector3s;

	private Vector2[] vector2s;

	private Color[,] colors;

	private float[] floats;

	private Rect[] rects;

	private CRSpline path;

	private Vector3 preUpdate;

	private Vector3 postUpdate;

	private NamedValueColor namedcolorvalue;

	private float lastRealTime;

	private bool useRealTime;

	public static void Init(GameObject target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		MoveBy(target, Vector3.get_zero(), 0f);
	}

	public static void CameraFadeFrom(float amount, float time)
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			CameraFadeFrom(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeFrom(Hashtable args)
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			ColorFrom(cameraFade, args);
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(float amount, float time)
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			CameraFadeTo(Hash("amount", amount, "time", time));
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void CameraFadeTo(Hashtable args)
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			ColorTo(cameraFade, args);
		}
		else
		{
			Debug.LogError((object)"iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.");
		}
	}

	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			Debug.LogError((object)"iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!");
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				Debug.LogError((object)"iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!");
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		Launch(target, args);
	}

	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		FadeFrom(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeFrom(GameObject target, Hashtable args)
	{
		ColorFrom(target, args);
	}

	public static void FadeTo(GameObject target, float alpha, float time)
	{
		FadeTo(target, Hash("alpha", alpha, "time", time));
	}

	public static void FadeTo(GameObject target, Hashtable args)
	{
		ColorTo(target, args);
	}

	public static void ColorFrom(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorFrom(target, Hash("color", color, "time", time));
	}

	public static void ColorFrom(GameObject target, Hashtable args)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		Color val = default(Color);
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.get_transform())
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorFrom(((Component)item).get_gameObject(), hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUITexture))))
		{
			val = (color = target.GetComponent<GUITexture>().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUIText))))
		{
			val = (color = target.GetComponent<GUIText>().get_material().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Renderer>()))
		{
			val = (color = target.GetComponent<Renderer>().get_material().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Light>()))
		{
			val = (color = target.GetComponent<Light>().get_color());
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUITexture))))
		{
			target.GetComponent<GUITexture>().set_color(color);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUIText))))
		{
			target.GetComponent<GUIText>().get_material().set_color(color);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Renderer>()))
		{
			target.GetComponent<Renderer>().get_material().set_color(color);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Light>()))
		{
			target.GetComponent<Light>().set_color(color);
		}
		args["color"] = val;
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ColorTo(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorTo(target, Hash("color", color, "time", time));
	}

	public static void ColorTo(GameObject target, Hashtable args)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.get_transform())
			{
				Hashtable hashtable = (Hashtable)args.Clone();
				hashtable["ischild"] = true;
				ColorTo(((Component)item).get_gameObject(), hashtable);
			}
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		AudioFrom(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioFrom(GameObject target, Hashtable args)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		AudioSource val;
		if (args.Contains("audiosource"))
		{
			val = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!Object.op_Implicit((Object)(object)target.GetComponent(typeof(AudioSource))))
			{
				Debug.LogError((object)"iTween Error: AudioFrom requires an AudioSource.");
				return;
			}
			val = target.GetComponent<AudioSource>();
		}
		Vector2 val2 = default(Vector2);
		Vector2 val3 = default(Vector2);
		val2.x = (val3.x = val.get_volume());
		val2.y = (val3.y = val.get_pitch());
		if (args.Contains("volume"))
		{
			val3.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			val3.y = (float)args["pitch"];
		}
		val.set_volume(val3.x);
		val.set_pitch(val3.y);
		args["volume"] = val2.x;
		args["pitch"] = val2.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		AudioTo(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		Stab(target, Hash("audioclip", audioclip, "delay", delay));
	}

	public static void Stab(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "stab";
		Launch(target, args);
	}

	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookFrom(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookFrom(GameObject target, Hashtable args)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		Vector3 eulerAngles = target.get_transform().get_eulerAngles();
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			target.get_transform().LookAt((Transform)args["looktarget"], (Vector3)(((_003F?)(Vector3?)args["up"]) ?? Defaults.up));
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			target.get_transform().LookAt((Vector3)args["looktarget"], (Vector3)(((_003F?)(Vector3?)args["up"]) ?? Defaults.up));
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.get_transform().get_eulerAngles();
			switch ((string)args["axis"])
			{
			case "x":
				eulerAngles2.y = eulerAngles.y;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "y":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.z = eulerAngles.z;
				break;
			case "z":
				eulerAngles2.x = eulerAngles.x;
				eulerAngles2.y = eulerAngles.y;
				break;
			}
			target.get_transform().set_eulerAngles(eulerAngles2);
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookTo(target, Hash("looktarget", looktarget, "time", time));
	}

	public static void LookTo(GameObject target, Hashtable args)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args["looktarget"];
			args["position"] = (object)new Vector3(val.get_position().x, val.get_position().y, val.get_position().z);
			args["rotation"] = (object)new Vector3(val.get_eulerAngles().x, val.get_eulerAngles().y, val.get_eulerAngles().z);
		}
		args["type"] = "look";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveTo(target, Hash("position", position, "time", time));
	}

	public static void MoveTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args["position"];
			args["position"] = (object)new Vector3(val.get_position().x, val.get_position().y, val.get_position().z);
			args["rotation"] = (object)new Vector3(val.get_eulerAngles().x, val.get_eulerAngles().y, val.get_eulerAngles().z);
			args["scale"] = (object)new Vector3(val.get_localScale().x, val.get_localScale().y, val.get_localScale().z);
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveFrom(target, Hash("position", position, "time", time));
	}

	public static void MoveFrom(GameObject target, Hashtable args)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = (Vector3[])(object)new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = (Vector3[])(object)new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].get_position();
				}
			}
			if (array2[array2.Length - 1] != target.get_transform().get_position())
			{
				Vector3[] array4 = (Vector3[])(object)new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.get_transform().get_localPosition();
					target.get_transform().set_localPosition(array4[0]);
				}
				else
				{
					array4[array4.Length - 1] = target.get_transform().get_position();
					target.get_transform().set_position(array4[0]);
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.get_transform().set_localPosition(array2[0]);
				}
				else
				{
					target.get_transform().set_position(array2[0]);
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 val;
			Vector3 val2 = ((!flag) ? (val = target.get_transform().get_position()) : (val = target.get_transform().get_localPosition()));
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					val = ((Transform)args["position"]).get_position();
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					val = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					val.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					val.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					val.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.get_transform().set_localPosition(val);
			}
			else
			{
				target.get_transform().set_position(val);
			}
			args["position"] = val2;
		}
		args["type"] = "move";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveAdd(target, Hash("amount", amount, "time", time));
	}

	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveBy(target, Hash("amount", amount, "time", time));
	}

	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleTo(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args["scale"];
			args["position"] = (object)new Vector3(val.get_position().x, val.get_position().y, val.get_position().z);
			args["rotation"] = (object)new Vector3(val.get_eulerAngles().x, val.get_eulerAngles().y, val.get_eulerAngles().z);
			args["scale"] = (object)new Vector3(val.get_localScale().x, val.get_localScale().y, val.get_localScale().z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleFrom(target, Hash("scale", scale, "time", time));
	}

	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		Vector3 localScale;
		Vector3 val = (localScale = target.get_transform().get_localScale());
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				localScale = ((Transform)args["scale"]).get_localScale();
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.get_transform().set_localScale(localScale);
		args["scale"] = val;
		args["type"] = "scale";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleAdd(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleBy(target, Hash("amount", amount, "time", time));
	}

	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateTo(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateTo(GameObject target, Hashtable args)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform val = (Transform)args["rotation"];
			args["position"] = (object)new Vector3(val.get_position().x, val.get_position().y, val.get_position().z);
			args["rotation"] = (object)new Vector3(val.get_eulerAngles().x, val.get_eulerAngles().y, val.get_eulerAngles().z);
			args["scale"] = (object)new Vector3(val.get_localScale().x, val.get_localScale().y, val.get_localScale().z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateFrom(target, Hash("rotation", rotation, "time", time));
	}

	public static void RotateFrom(GameObject target, Hashtable args)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		args = CleanArgs(args);
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		Vector3 val;
		Vector3 val2 = ((!flag) ? (val = target.get_transform().get_eulerAngles()) : (val = target.get_transform().get_localEulerAngles()));
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				val = ((Transform)args["rotation"]).get_eulerAngles();
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				val = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				val.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				val.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				val.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.get_transform().set_localEulerAngles(val);
		}
		else
		{
			target.get_transform().set_eulerAngles(val);
		}
		args["rotation"] = val2;
		args["type"] = "rotate";
		args["method"] = "to";
		Launch(target, args);
	}

	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateAdd(target, Hash("amount", amount, "time", time));
	}

	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		Launch(target, args);
	}

	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateBy(target, Hash("amount", amount, "time", time));
	}

	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		Launch(target, args);
	}

	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakePosition(target, Hash("amount", amount, "time", time));
	}

	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		Launch(target, args);
	}

	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakeScale(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		Launch(target, args);
	}

	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ShakeRotation(target, Hash("amount", amount, "time", time));
	}

	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		Launch(target, args);
	}

	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchPosition(target, Hash("amount", amount, "time", time));
	}

	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchRotation(target, Hash("amount", amount, "time", time));
	}

	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PunchScale(target, Hash("amount", amount, "time", time));
	}

	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = EaseType.punch;
		Launch(target, args);
	}

	private void GenerateTargets()
	{
		switch (type)
		{
		case "value":
			switch (method)
			{
			case "float":
				GenerateFloatTargets();
				apply = ApplyFloatTargets;
				break;
			case "vector2":
				GenerateVector2Targets();
				apply = ApplyVector2Targets;
				break;
			case "vector3":
				GenerateVector3Targets();
				apply = ApplyVector3Targets;
				break;
			case "color":
				GenerateColorTargets();
				apply = ApplyColorTargets;
				break;
			case "rect":
				GenerateRectTargets();
				apply = ApplyRectTargets;
				break;
			}
			break;
		case "color":
			if (method == "to")
			{
				GenerateColorToTargets();
				apply = ApplyColorToTargets;
			}
			break;
		case "audio":
			if (method == "to")
			{
				GenerateAudioToTargets();
				apply = ApplyAudioToTargets;
			}
			break;
		case "move":
			switch (method)
			{
			case "to":
				if (tweenArguments.Contains("path"))
				{
					GenerateMoveToPathTargets();
					apply = ApplyMoveToPathTargets;
				}
				else
				{
					GenerateMoveToTargets();
					apply = ApplyMoveToTargets;
				}
				break;
			case "by":
			case "add":
				GenerateMoveByTargets();
				apply = ApplyMoveByTargets;
				break;
			}
			break;
		case "scale":
			switch (method)
			{
			case "to":
				GenerateScaleToTargets();
				apply = ApplyScaleToTargets;
				break;
			case "by":
				GenerateScaleByTargets();
				apply = ApplyScaleToTargets;
				break;
			case "add":
				GenerateScaleAddTargets();
				apply = ApplyScaleToTargets;
				break;
			}
			break;
		case "rotate":
			switch (method)
			{
			case "to":
				GenerateRotateToTargets();
				apply = ApplyRotateToTargets;
				break;
			case "add":
				GenerateRotateAddTargets();
				apply = ApplyRotateAddTargets;
				break;
			case "by":
				GenerateRotateByTargets();
				apply = ApplyRotateAddTargets;
				break;
			}
			break;
		case "shake":
			switch (method)
			{
			case "position":
				GenerateShakePositionTargets();
				apply = ApplyShakePositionTargets;
				break;
			case "scale":
				GenerateShakeScaleTargets();
				apply = ApplyShakeScaleTargets;
				break;
			case "rotation":
				GenerateShakeRotationTargets();
				apply = ApplyShakeRotationTargets;
				break;
			}
			break;
		case "punch":
			switch (method)
			{
			case "position":
				GeneratePunchPositionTargets();
				apply = ApplyPunchPositionTargets;
				break;
			case "rotation":
				GeneratePunchRotationTargets();
				apply = ApplyPunchRotationTargets;
				break;
			case "scale":
				GeneratePunchScaleTargets();
				apply = ApplyPunchScaleTargets;
				break;
			}
			break;
		case "look":
			if (method == "to")
			{
				GenerateLookToTargets();
				apply = ApplyLookToTargets;
			}
			break;
		case "stab":
			GenerateStabTargets();
			apply = ApplyStabTargets;
			break;
		}
	}

	private void GenerateRectTargets()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		rects = (Rect[])(object)new Rect[3];
		rects[0] = (Rect)tweenArguments["from"];
		rects[1] = (Rect)tweenArguments["to"];
	}

	private void GenerateColorTargets()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		colors = new Color[1, 3];
		colors[0, 0] = (Color)tweenArguments["from"];
		colors[0, 1] = (Color)tweenArguments["to"];
	}

	private void GenerateVector3Targets()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (Vector3)tweenArguments["from"];
		vector3s[1] = (Vector3)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateVector2Targets()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		vector2s = (Vector2[])(object)new Vector2[3];
		vector2s[0] = (Vector2)tweenArguments["from"];
		vector2s[1] = (Vector2)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			Vector3 val = new Vector3(vector2s[0].x, vector2s[0].y, 0f);
			Vector3 val2 = default(Vector3);
			((Vector3)(ref val2))._002Ector(vector2s[1].x, vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(val, val2));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateFloatTargets()
	{
		floats = new float[3];
		floats[0] = (float)tweenArguments["from"];
		floats[1] = (float)tweenArguments["to"];
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(floats[0] - floats[1]);
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateColorToTargets()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUITexture))))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = ((Component)this).GetComponent<GUITexture>().get_color());
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUIText))))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = ((Component)this).GetComponent<GUIText>().get_material().get_color());
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Renderer>()))
		{
			colors = new Color[((Component)this).GetComponent<Renderer>().get_materials().Length, 3];
			for (int i = 0; i < ((Component)this).GetComponent<Renderer>().get_materials().Length; i++)
			{
				colors[i, 0] = ((Component)this).GetComponent<Renderer>().get_materials()[i].GetColor(namedcolorvalue.ToString());
				colors[i, 1] = ((Component)this).GetComponent<Renderer>().get_materials()[i].GetColor(namedcolorvalue.ToString());
			}
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Light>()))
		{
			colors = new Color[1, 3];
			colors[0, 0] = (colors[0, 1] = ((Component)this).GetComponent<Light>().get_color());
		}
		else
		{
			colors = new Color[1, 3];
		}
		if (tweenArguments.Contains("color"))
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				colors[j, 1] = (Color)tweenArguments["color"];
			}
		}
		else
		{
			if (tweenArguments.Contains("r"))
			{
				for (int k = 0; k < colors.GetLength(0); k++)
				{
					colors[k, 1].r = (float)tweenArguments["r"];
				}
			}
			if (tweenArguments.Contains("g"))
			{
				for (int l = 0; l < colors.GetLength(0); l++)
				{
					colors[l, 1].g = (float)tweenArguments["g"];
				}
			}
			if (tweenArguments.Contains("b"))
			{
				for (int m = 0; m < colors.GetLength(0); m++)
				{
					colors[m, 1].b = (float)tweenArguments["b"];
				}
			}
			if (tweenArguments.Contains("a"))
			{
				for (int n = 0; n < colors.GetLength(0); n++)
				{
					colors[n, 1].a = (float)tweenArguments["a"];
				}
			}
		}
		if (tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < colors.GetLength(0); num++)
			{
				colors[num, 1].a = (float)tweenArguments["amount"];
			}
		}
		else if (tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < colors.GetLength(0); num2++)
			{
				colors[num2, 1].a = (float)tweenArguments["alpha"];
			}
		}
	}

	private void GenerateAudioToTargets()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		vector2s = (Vector2[])(object)new Vector2[3];
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(AudioSource))))
		{
			audioSource = ((Component)this).GetComponent<AudioSource>();
		}
		else
		{
			Debug.LogError((object)"iTween Error: AudioTo requires an AudioSource.");
			Dispose();
		}
		vector2s[0] = (vector2s[1] = new Vector2(audioSource.get_volume(), audioSource.get_pitch()));
		if (tweenArguments.Contains("volume"))
		{
			vector2s[1].x = (float)tweenArguments["volume"];
		}
		if (tweenArguments.Contains("pitch"))
		{
			vector2s[1].y = (float)tweenArguments["pitch"];
		}
	}

	private void GenerateStabTargets()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		if (tweenArguments.Contains("audiosource"))
		{
			audioSource = (AudioSource)tweenArguments["audiosource"];
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(AudioSource))))
		{
			audioSource = ((Component)this).GetComponent<AudioSource>();
		}
		else
		{
			((Component)this).get_gameObject().AddComponent(typeof(AudioSource));
			audioSource = ((Component)this).GetComponent<AudioSource>();
			audioSource.set_playOnAwake(false);
		}
		audioSource.set_clip((AudioClip)tweenArguments["audioclip"]);
		if (tweenArguments.Contains("pitch"))
		{
			audioSource.set_pitch((float)tweenArguments["pitch"]);
		}
		if (tweenArguments.Contains("volume"))
		{
			audioSource.set_volume((float)tweenArguments["volume"]);
		}
		time = audioSource.get_clip().get_length() / audioSource.get_pitch();
	}

	private void GenerateLookToTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).get_transform().get_eulerAngles();
		if (tweenArguments.Contains("looktarget"))
		{
			if (tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				((Component)this).get_transform().LookAt((Transform)tweenArguments["looktarget"], (Vector3)(((_003F?)(Vector3?)tweenArguments["up"]) ?? Defaults.up));
			}
			else if (tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				((Component)this).get_transform().LookAt((Vector3)tweenArguments["looktarget"], (Vector3)(((_003F?)(Vector3?)tweenArguments["up"]) ?? Defaults.up));
			}
		}
		else
		{
			Debug.LogError((object)"iTween Error: LookTo needs a 'looktarget' property!");
			Dispose();
		}
		vector3s[1] = ((Component)this).get_transform().get_eulerAngles();
		((Component)this).get_transform().set_eulerAngles(vector3s[0]);
		if (tweenArguments.Contains("axis"))
		{
			switch ((string)tweenArguments["axis"])
			{
			case "x":
				vector3s[1].y = vector3s[0].y;
				vector3s[1].z = vector3s[0].z;
				break;
			case "y":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].z = vector3s[0].z;
				break;
			case "z":
				vector3s[1].x = vector3s[0].x;
				vector3s[1].y = vector3s[0].y;
				break;
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveToPathTargets()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array2;
		if (tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])tweenArguments["path"];
			if (array.Length == 1)
			{
				Debug.LogError((object)"iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = (Vector3[])(object)new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])tweenArguments["path"];
			if (array3.Length == 1)
			{
				Debug.LogError((object)"iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!");
				Dispose();
			}
			array2 = (Vector3[])(object)new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].get_position();
			}
		}
		bool flag;
		int num;
		if (((Component)this).get_transform().get_position() != array2[0])
		{
			if (!tweenArguments.Contains("movetopath") || (bool)tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		vector3s = (Vector3[])(object)new Vector3[array2.Length + num];
		if (flag)
		{
			vector3s[1] = ((Component)this).get_transform().get_position();
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, vector3s, num, array2.Length);
		vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
		vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] + (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);
		if (vector3s[1] == vector3s[vector3s.Length - 2])
		{
			Vector3[] array4 = (Vector3[])(object)new Vector3[vector3s.Length];
			Array.Copy(vector3s, array4, vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			vector3s = (Vector3[])(object)new Vector3[array4.Length];
			Array.Copy(array4, vector3s, array4.Length);
		}
		path = new CRSpline(vector3s);
		if (tweenArguments.Contains("speed"))
		{
			float num2 = PathLength(vector3s);
			time = num2 / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveToTargets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_localPosition());
		}
		else
		{
			vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_position());
		}
		if (tweenArguments.Contains("position"))
		{
			if (tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments["position"];
				vector3s[1] = val.get_position();
			}
			else if (tweenArguments["position"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["position"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateMoveByTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[6];
		vector3s[4] = ((Component)this).get_transform().get_eulerAngles();
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).get_transform().get_position()));
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = vector3s[0] + (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = vector3s[0].x + (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = vector3s[0].y + (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = vector3s[0].z + (float)tweenArguments["z"];
			}
		}
		((Component)this).get_transform().Translate(vector3s[1], space);
		vector3s[5] = ((Component)this).get_transform().get_position();
		((Component)this).get_transform().set_position(vector3s[0]);
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			tweenArguments["looktarget"] = vector3s[1];
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleToTargets()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_localScale());
		if (tweenArguments.Contains("scale"))
		{
			if (tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments["scale"];
				vector3s[1] = val.get_localScale();
			}
			else if (tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["scale"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleByTargets()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_localScale());
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = Vector3.Scale(vector3s[1], (Vector3)tweenArguments["amount"]);
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x *= (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y *= (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z *= (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateScaleAddTargets()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_localScale());
		if (tweenArguments.Contains("amount"))
		{
			ref Vector3 reference = ref vector3s[1];
			reference += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateRotateToTargets()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		if (isLocal)
		{
			vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_localEulerAngles());
		}
		else
		{
			vector3s[0] = (vector3s[1] = ((Component)this).get_transform().get_eulerAngles());
		}
		if (tweenArguments.Contains("rotation"))
		{
			if (tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)tweenArguments["rotation"];
				vector3s[1] = val.get_eulerAngles();
			}
			else if (tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				vector3s[1] = (Vector3)tweenArguments["rotation"];
			}
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x = (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y = (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z = (float)tweenArguments["z"];
			}
		}
		vector3s[1] = new Vector3(clerp(vector3s[0].x, vector3s[1].x, 1f), clerp(vector3s[0].y, vector3s[1].y, 1f), clerp(vector3s[0].z, vector3s[1].z, 1f));
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateRotateAddTargets()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[5];
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).get_transform().get_eulerAngles()));
		if (tweenArguments.Contains("amount"))
		{
			ref Vector3 reference = ref vector3s[1];
			reference += (Vector3)tweenArguments["amount"];
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateRotateByTargets()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[0] = (vector3s[1] = (vector3s[3] = ((Component)this).get_transform().get_eulerAngles()));
		if (tweenArguments.Contains("amount"))
		{
			ref Vector3 reference = ref vector3s[1];
			reference += Vector3.Scale((Vector3)tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (tweenArguments.Contains("x"))
			{
				vector3s[1].x += 360f * (float)tweenArguments["x"];
			}
			if (tweenArguments.Contains("y"))
			{
				vector3s[1].y += 360f * (float)tweenArguments["y"];
			}
			if (tweenArguments.Contains("z"))
			{
				vector3s[1].z += 360f * (float)tweenArguments["z"];
			}
		}
		if (tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(vector3s[0], vector3s[1]));
			time = num / (float)tweenArguments["speed"];
		}
	}

	private void GenerateShakePositionTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[3] = ((Component)this).get_transform().get_eulerAngles();
		vector3s[0] = ((Component)this).get_transform().get_position();
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateShakeScaleTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).get_transform().get_localScale();
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GenerateShakeRotationTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).get_transform().get_eulerAngles();
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GeneratePunchPositionTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[5];
		vector3s[4] = ((Component)this).get_transform().get_eulerAngles();
		vector3s[0] = ((Component)this).get_transform().get_position();
		vector3s[1] = (vector3s[3] = Vector3.get_zero());
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GeneratePunchRotationTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[4];
		vector3s[0] = ((Component)this).get_transform().get_eulerAngles();
		vector3s[1] = (vector3s[3] = Vector3.get_zero());
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void GeneratePunchScaleTargets()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		vector3s = (Vector3[])(object)new Vector3[3];
		vector3s[0] = ((Component)this).get_transform().get_localScale();
		vector3s[1] = Vector3.get_zero();
		if (tweenArguments.Contains("amount"))
		{
			vector3s[1] = (Vector3)tweenArguments["amount"];
			return;
		}
		if (tweenArguments.Contains("x"))
		{
			vector3s[1].x = (float)tweenArguments["x"];
		}
		if (tweenArguments.Contains("y"))
		{
			vector3s[1].y = (float)tweenArguments["y"];
		}
		if (tweenArguments.Contains("z"))
		{
			vector3s[1].z = (float)tweenArguments["z"];
		}
	}

	private void ApplyRectTargets()
	{
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		((Rect)(ref rects[2])).set_x(ease(((Rect)(ref rects[0])).get_x(), ((Rect)(ref rects[1])).get_x(), percentage));
		((Rect)(ref rects[2])).set_y(ease(((Rect)(ref rects[0])).get_y(), ((Rect)(ref rects[1])).get_y(), percentage));
		((Rect)(ref rects[2])).set_width(ease(((Rect)(ref rects[0])).get_width(), ((Rect)(ref rects[1])).get_width(), percentage));
		((Rect)(ref rects[2])).set_height(ease(((Rect)(ref rects[0])).get_height(), ((Rect)(ref rects[1])).get_height(), percentage));
		tweenArguments["onupdateparams"] = rects[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = rects[1];
		}
	}

	private void ApplyColorTargets()
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		colors[0, 2].r = ease(colors[0, 0].r, colors[0, 1].r, percentage);
		colors[0, 2].g = ease(colors[0, 0].g, colors[0, 1].g, percentage);
		colors[0, 2].b = ease(colors[0, 0].b, colors[0, 1].b, percentage);
		colors[0, 2].a = ease(colors[0, 0].a, colors[0, 1].a, percentage);
		tweenArguments["onupdateparams"] = colors[0, 2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = colors[0, 1];
		}
	}

	private void ApplyVector3Targets()
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		tweenArguments["onupdateparams"] = vector3s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector3s[1];
		}
	}

	private void ApplyVector2Targets()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		tweenArguments["onupdateparams"] = vector2s[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = vector2s[1];
		}
	}

	private void ApplyFloatTargets()
	{
		floats[2] = ease(floats[0], floats[1], percentage);
		tweenArguments["onupdateparams"] = floats[2];
		if (percentage == 1f)
		{
			tweenArguments["onupdateparams"] = floats[1];
		}
	}

	private void ApplyColorToTargets()
	{
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < colors.GetLength(0); i++)
		{
			colors[i, 2].r = ease(colors[i, 0].r, colors[i, 1].r, percentage);
			colors[i, 2].g = ease(colors[i, 0].g, colors[i, 1].g, percentage);
			colors[i, 2].b = ease(colors[i, 0].b, colors[i, 1].b, percentage);
			colors[i, 2].a = ease(colors[i, 0].a, colors[i, 1].a, percentage);
		}
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUITexture))))
		{
			((Component)this).GetComponent<GUITexture>().set_color(colors[0, 2]);
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUIText))))
		{
			((Component)this).GetComponent<GUIText>().get_material().set_color(colors[0, 2]);
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Renderer>()))
		{
			for (int j = 0; j < colors.GetLength(0); j++)
			{
				((Component)this).GetComponent<Renderer>().get_materials()[j].SetColor(namedcolorvalue.ToString(), colors[j, 2]);
			}
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Light>()))
		{
			((Component)this).GetComponent<Light>().set_color(colors[0, 2]);
		}
		if (percentage != 1f)
		{
			return;
		}
		if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUITexture))))
		{
			((Component)this).GetComponent<GUITexture>().set_color(colors[0, 1]);
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent(typeof(GUIText))))
		{
			((Component)this).GetComponent<GUIText>().get_material().set_color(colors[0, 1]);
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Renderer>()))
		{
			for (int k = 0; k < colors.GetLength(0); k++)
			{
				((Component)this).GetComponent<Renderer>().get_materials()[k].SetColor(namedcolorvalue.ToString(), colors[k, 1]);
			}
		}
		else if (Object.op_Implicit((Object)(object)((Component)this).GetComponent<Light>()))
		{
			((Component)this).GetComponent<Light>().set_color(colors[0, 1]);
		}
	}

	private void ApplyAudioToTargets()
	{
		vector2s[2].x = ease(vector2s[0].x, vector2s[1].x, percentage);
		vector2s[2].y = ease(vector2s[0].y, vector2s[1].y, percentage);
		audioSource.set_volume(vector2s[2].x);
		audioSource.set_pitch(vector2s[2].y);
		if (percentage == 1f)
		{
			audioSource.set_volume(vector2s[1].x);
			audioSource.set_pitch(vector2s[1].y);
		}
	}

	private void ApplyStabTargets()
	{
	}

	private void ApplyMoveToPathTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_position();
		float num = ease(0f, 1f, percentage);
		if (isLocal)
		{
			((Component)this).get_transform().set_localPosition(path.Interp(Mathf.Clamp(num, 0f, 1f)));
		}
		else
		{
			((Component)this).get_transform().set_position(path.Interp(Mathf.Clamp(num, 0f, 1f)));
		}
		if (tweenArguments.Contains("orienttopath") && (bool)tweenArguments["orienttopath"])
		{
			float num2 = ((!tweenArguments.Contains("lookahead")) ? Defaults.lookAhead : ((float)tweenArguments["lookahead"]));
			float num3 = ease(0f, 1f, Mathf.Min(1f, percentage + num2));
			tweenArguments["looktarget"] = path.Interp(Mathf.Clamp(num3, 0f, 1f));
		}
		postUpdate = ((Component)this).get_transform().get_position();
		if (physics)
		{
			((Component)this).get_transform().set_position(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveToTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_position();
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).get_transform().set_localPosition(vector3s[2]);
		}
		else
		{
			((Component)this).get_transform().set_position(vector3s[2]);
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				((Component)this).get_transform().set_localPosition(vector3s[1]);
			}
			else
			{
				((Component)this).get_transform().set_position(vector3s[1]);
			}
		}
		postUpdate = ((Component)this).get_transform().get_position();
		if (physics)
		{
			((Component)this).get_transform().set_position(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyMoveByTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_position();
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = ((Component)this).get_transform().get_eulerAngles();
			((Component)this).get_transform().set_eulerAngles(vector3s[4]);
		}
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).get_transform().Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			((Component)this).get_transform().set_eulerAngles(eulerAngles);
		}
		postUpdate = ((Component)this).get_transform().get_position();
		if (physics)
		{
			((Component)this).get_transform().set_position(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyScaleToTargets()
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).get_transform().set_localScale(vector3s[2]);
		if (percentage == 1f)
		{
			((Component)this).get_transform().set_localScale(vector3s[1]);
		}
	}

	private void ApplyLookToTargets()
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).get_transform().set_localRotation(Quaternion.Euler(vector3s[2]));
		}
		else
		{
			((Component)this).get_transform().set_rotation(Quaternion.Euler(vector3s[2]));
		}
	}

	private void ApplyRotateToTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_eulerAngles();
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		if (isLocal)
		{
			((Component)this).get_transform().set_localRotation(Quaternion.Euler(vector3s[2]));
		}
		else
		{
			((Component)this).get_transform().set_rotation(Quaternion.Euler(vector3s[2]));
		}
		if (percentage == 1f)
		{
			if (isLocal)
			{
				((Component)this).get_transform().set_localRotation(Quaternion.Euler(vector3s[1]));
			}
			else
			{
				((Component)this).get_transform().set_rotation(Quaternion.Euler(vector3s[1]));
			}
		}
		postUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (physics)
		{
			((Component)this).get_transform().set_eulerAngles(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyRotateAddTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_eulerAngles();
		vector3s[2].x = ease(vector3s[0].x, vector3s[1].x, percentage);
		vector3s[2].y = ease(vector3s[0].y, vector3s[1].y, percentage);
		vector3s[2].z = ease(vector3s[0].z, vector3s[1].z, percentage);
		((Component)this).get_transform().Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (physics)
		{
			((Component)this).get_transform().set_eulerAngles(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyShakePositionTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_position();
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = ((Component)this).get_transform().get_eulerAngles();
			((Component)this).get_transform().set_eulerAngles(vector3s[3]);
		}
		if (percentage == 0f)
		{
			((Component)this).get_transform().Translate(vector3s[1], space);
		}
		((Component)this).get_transform().set_position(vector3s[0]);
		float num = 1f - percentage;
		vector3s[2].x = Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		((Component)this).get_transform().Translate(vector3s[2], space);
		if (tweenArguments.Contains("looktarget"))
		{
			((Component)this).get_transform().set_eulerAngles(eulerAngles);
		}
		postUpdate = ((Component)this).get_transform().get_position();
		if (physics)
		{
			((Component)this).get_transform().set_position(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyShakeScaleTargets()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		if (percentage == 0f)
		{
			((Component)this).get_transform().set_localScale(vector3s[1]);
		}
		((Component)this).get_transform().set_localScale(vector3s[0]);
		float num = 1f - percentage;
		vector3s[2].x = Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		Transform transform = ((Component)this).get_transform();
		transform.set_localScale(transform.get_localScale() + vector3s[2]);
	}

	private void ApplyShakeRotationTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (percentage == 0f)
		{
			((Component)this).get_transform().Rotate(vector3s[1], space);
		}
		((Component)this).get_transform().set_eulerAngles(vector3s[0]);
		float num = 1f - percentage;
		vector3s[2].x = Random.Range((0f - vector3s[1].x) * num, vector3s[1].x * num);
		vector3s[2].y = Random.Range((0f - vector3s[1].y) * num, vector3s[1].y * num);
		vector3s[2].z = Random.Range((0f - vector3s[1].z) * num, vector3s[1].z * num);
		((Component)this).get_transform().Rotate(vector3s[2], space);
		postUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (physics)
		{
			((Component)this).get_transform().set_eulerAngles(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyPunchPositionTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_position();
		Vector3 eulerAngles = default(Vector3);
		if (tweenArguments.Contains("looktarget"))
		{
			eulerAngles = ((Component)this).get_transform().get_eulerAngles();
			((Component)this).get_transform().set_eulerAngles(vector3s[4]);
		}
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).get_transform().Translate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		if (tweenArguments.Contains("looktarget"))
		{
			((Component)this).get_transform().set_eulerAngles(eulerAngles);
		}
		postUpdate = ((Component)this).get_transform().get_position();
		if (physics)
		{
			((Component)this).get_transform().set_position(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MovePosition(postUpdate);
		}
	}

	private void ApplyPunchRotationTargets()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		preUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).get_transform().Rotate(vector3s[2] - vector3s[3], space);
		vector3s[3] = vector3s[2];
		postUpdate = ((Component)this).get_transform().get_eulerAngles();
		if (physics)
		{
			((Component)this).get_transform().set_eulerAngles(preUpdate);
			((Component)this).GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(postUpdate));
		}
	}

	private void ApplyPunchScaleTargets()
	{
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		if (vector3s[1].x > 0f)
		{
			vector3s[2].x = punch(vector3s[1].x, percentage);
		}
		else if (vector3s[1].x < 0f)
		{
			vector3s[2].x = 0f - punch(Mathf.Abs(vector3s[1].x), percentage);
		}
		if (vector3s[1].y > 0f)
		{
			vector3s[2].y = punch(vector3s[1].y, percentage);
		}
		else if (vector3s[1].y < 0f)
		{
			vector3s[2].y = 0f - punch(Mathf.Abs(vector3s[1].y), percentage);
		}
		if (vector3s[1].z > 0f)
		{
			vector3s[2].z = punch(vector3s[1].z, percentage);
		}
		else if (vector3s[1].z < 0f)
		{
			vector3s[2].z = 0f - punch(Mathf.Abs(vector3s[1].z), percentage);
		}
		((Component)this).get_transform().set_localScale(vector3s[0] + vector3s[2]);
	}

	private IEnumerator TweenDelay()
	{
		delayStarted = Time.get_time();
		yield return (object)new WaitForSeconds(delay);
		if (wasPaused)
		{
			wasPaused = false;
			TweenStart();
		}
	}

	private void TweenStart()
	{
		CallBack("onstart");
		if (!loop)
		{
			ConflictCheck();
			GenerateTargets();
		}
		if (type == "stab")
		{
			audioSource.PlayOneShot(audioSource.get_clip());
		}
		if (type == "move" || type == "scale" || type == "rotate" || type == "punch" || type == "shake" || type == "curve" || type == "look")
		{
			EnableKinematic();
		}
		isRunning = true;
	}

	private IEnumerator TweenRestart()
	{
		if (delay > 0f)
		{
			delayStarted = Time.get_time();
			yield return (object)new WaitForSeconds(delay);
		}
		loop = true;
		TweenStart();
	}

	private void TweenUpdate()
	{
		apply();
		CallBack("onupdate");
		UpdatePercentage();
	}

	private void TweenComplete()
	{
		isRunning = false;
		if (percentage > 0.5f)
		{
			percentage = 1f;
		}
		else
		{
			percentage = 0f;
		}
		apply();
		if (type == "value")
		{
			CallBack("onupdate");
		}
		if (loopType == LoopType.none)
		{
			Dispose();
		}
		else
		{
			TweenLoop();
		}
		CallBack("oncomplete");
	}

	private void TweenLoop()
	{
		DisableKinematic();
		switch (loopType)
		{
		case LoopType.loop:
			percentage = 0f;
			runningTime = 0f;
			apply();
			((MonoBehaviour)this).StartCoroutine("TweenRestart");
			break;
		case LoopType.pingPong:
			reverse = !reverse;
			runningTime = 0f;
			((MonoBehaviour)this).StartCoroutine("TweenRestart");
			break;
		}
	}

	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		return new Rect(FloatUpdate(((Rect)(ref currentValue)).get_x(), ((Rect)(ref targetValue)).get_x(), speed), FloatUpdate(((Rect)(ref currentValue)).get_y(), ((Rect)(ref targetValue)).get_y(), speed), FloatUpdate(((Rect)(ref currentValue)).get_width(), ((Rect)(ref targetValue)).get_width(), speed), FloatUpdate(((Rect)(ref currentValue)).get_height(), ((Rect)(ref targetValue)).get_height(), speed));
	}

	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = targetValue - currentValue;
		currentValue += val * speed * Time.get_deltaTime();
		return currentValue;
	}

	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = targetValue - currentValue;
		currentValue += val * speed * Time.get_deltaTime();
		return currentValue;
	}

	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.get_deltaTime();
		return currentValue;
	}

	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		ColorUpdate(target, args);
	}

	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		FadeUpdate(target, Hash("alpha", alpha, "time", time));
	}

	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Color[] array = (Color[])(object)new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			foreach (Transform item in target.get_transform())
			{
				ColorUpdate(((Component)item).get_gameObject(), args);
			}
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUITexture))))
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUIText))))
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().get_material().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Renderer>()))
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().get_material().get_color());
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Light>()))
		{
			array[0] = (array[1] = target.GetComponent<Light>().get_color());
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUITexture))))
		{
			target.GetComponent<GUITexture>().set_color(array[3]);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent(typeof(GUIText))))
		{
			target.GetComponent<GUIText>().get_material().set_color(array[3]);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Renderer>()))
		{
			target.GetComponent<Renderer>().get_material().set_color(array[3]);
		}
		else if (Object.op_Implicit((Object)(object)target.GetComponent<Light>()))
		{
			target.GetComponent<Light>().set_color(array[3]);
		}
	}

	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ColorUpdate(target, Hash("color", color, "time", time));
	}

	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector2[] array = (Vector2[])(object)new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		AudioSource val;
		if (args.Contains("audiosource"))
		{
			val = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!Object.op_Implicit((Object)(object)target.GetComponent(typeof(AudioSource))))
			{
				Debug.LogError((object)"iTween Error: AudioUpdate requires an AudioSource.");
				return;
			}
			val = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(val.get_volume(), val.get_pitch()));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		val.set_volume(array[3].x);
		val.set_pitch(array[3].y);
	}

	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		AudioUpdate(target, Hash("volume", volume, "pitch", pitch, "time", time));
	}

	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		Vector3 eulerAngles = target.get_transform().get_eulerAngles();
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = target.get_transform().get_localEulerAngles();
		}
		else
		{
			array[0] = target.get_transform().get_eulerAngles();
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args["rotation"];
				array[1] = val.get_eulerAngles();
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.get_transform().set_localEulerAngles(array[3]);
		}
		else
		{
			target.get_transform().set_eulerAngles(array[3]);
		}
		if ((Object)(object)target.GetComponent<Rigidbody>() != (Object)null)
		{
			Vector3 eulerAngles2 = target.get_transform().get_eulerAngles();
			target.get_transform().set_eulerAngles(eulerAngles);
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		RotateUpdate(target, Hash("rotation", rotation, "time", time));
	}

	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = (array[1] = target.get_transform().get_localScale());
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args["scale"];
				array[1] = val.get_localScale();
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.get_transform().set_localScale(array[3]);
	}

	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		ScaleUpdate(target, Hash("scale", scale, "time", time));
	}

	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[4];
		Vector3 position = target.get_transform().get_position();
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		bool flag = ((!args.Contains("islocal")) ? Defaults.isLocal : ((bool)args["islocal"]));
		if (flag)
		{
			array[0] = (array[1] = target.get_transform().get_localPosition());
		}
		else
		{
			array[0] = (array[1] = target.get_transform().get_position());
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform val = (Transform)args["position"];
				array[1] = val.get_position();
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			LookUpdate(target, args);
		}
		if (flag)
		{
			target.get_transform().set_localPosition(array[3]);
		}
		else
		{
			target.get_transform().set_position(array[3]);
		}
		if ((Object)(object)target.GetComponent<Rigidbody>() != (Object)null)
		{
			Vector3 position2 = target.get_transform().get_position();
			target.get_transform().set_position(position);
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		MoveUpdate(target, Hash("position", position, "time", time));
	}

	public static void LookUpdate(GameObject target, Hashtable args)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		CleanArgs(args);
		Vector3[] array = (Vector3[])(object)new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= Defaults.updateTimePercentage;
		}
		else
		{
			num = Defaults.updateTime;
		}
		array[0] = target.get_transform().get_eulerAngles();
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				target.get_transform().LookAt((Transform)args["looktarget"], (Vector3)(((_003F?)(Vector3?)args["up"]) ?? Defaults.up));
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				target.get_transform().LookAt((Vector3)args["looktarget"], (Vector3)(((_003F?)(Vector3?)args["up"]) ?? Defaults.up));
			}
			array[1] = target.get_transform().get_eulerAngles();
			target.get_transform().set_eulerAngles(array[0]);
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.get_transform().set_eulerAngles(array[3]);
			if (args.Contains("axis"))
			{
				array[4] = target.get_transform().get_eulerAngles();
				switch ((string)args["axis"])
				{
				case "x":
					array[4].y = array[0].y;
					array[4].z = array[0].z;
					break;
				case "y":
					array[4].x = array[0].x;
					array[4].z = array[0].z;
					break;
				case "z":
					array[4].x = array[0].x;
					array[4].y = array[0].y;
					break;
				}
				target.get_transform().set_eulerAngles(array[4]);
			}
		}
		else
		{
			Debug.LogError((object)"iTween Error: LookUpdate needs a 'looktarget' property!");
		}
	}

	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		LookUpdate(target, Hash("looktarget", looktarget, "time", time));
	}

	public static float PathLength(Transform[] path)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].get_position();
		}
		Vector3[] pts = PathControlPointGenerator(array);
		Vector3 val = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 val2 = Interp(pts, t);
			num += Vector3.Distance(val, val2);
			val = val2;
		}
		return num;
	}

	public static float PathLength(Vector3[] path)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 val = Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 val2 = Interp(pts, t);
			num += Vector3.Distance(val, val2);
			val = val2;
		}
		return num;
	}

	public static Texture2D CameraTexture(Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Texture2D val = new Texture2D(Screen.get_width(), Screen.get_height(), (TextureFormat)5, false);
		Color[] array = (Color[])(object)new Color[Screen.get_width() * Screen.get_height()];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		val.SetPixels(array);
		val.Apply();
		return val;
	}

	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		target.get_transform().set_position(Interp(PathControlPointGenerator(path), percent));
	}

	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		target.set_position(Interp(PathControlPointGenerator(path), percent));
	}

	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].get_position();
		}
		target.get_transform().set_position(Interp(PathControlPointGenerator(array), percent));
	}

	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].get_position();
		}
		target.set_position(Interp(PathControlPointGenerator(array), percent));
	}

	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].get_position();
		}
		return Interp(PathControlPointGenerator(array), percent);
	}

	public static void DrawLine(Vector3[] line)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Vector3[] line, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLine(Transform[] line, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, color, "gizmos");
		}
	}

	public static void DrawLineHandles(Vector3[] line)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			DrawLineHelper(line, color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawLineHandles(Transform[] line, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (line.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].get_position();
			}
			DrawLineHelper(array, color, "handles");
		}
	}

	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Interp(PathControlPointGenerator(path), percent);
	}

	public static void DrawPath(Vector3[] path)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Vector3[] path, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPath(Transform[] path, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, Defaults.color, "gizmos");
		}
	}

	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, color, "gizmos");
		}
	}

	public static void DrawPathHandles(Vector3[] path)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			DrawPathHelper(path, color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, Defaults.color, "handles");
		}
	}

	public static void DrawPathHandles(Transform[] path, Color color)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (path.Length != 0)
		{
			Vector3[] array = (Vector3[])(object)new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].get_position();
			}
			DrawPathHelper(array, color, "handles");
		}
	}

	public static void CameraFadeDepth(int depth)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			cameraFade.get_transform().set_position(new Vector3(cameraFade.get_transform().get_position().x, cameraFade.get_transform().get_position().y, (float)depth));
		}
	}

	public static void CameraFadeDestroy()
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			Object.Destroy((Object)(object)cameraFade);
		}
	}

	public static void CameraFadeSwap(Texture2D texture)
	{
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			cameraFade.GetComponent<GUITexture>().set_texture((Texture)(object)texture);
		}
	}

	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float)depth));
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().set_texture((Texture)(object)texture);
		cameraFade.GetComponent<GUITexture>().set_color(new Color(0.5f, 0.5f, 0.5f, 0f));
		return cameraFade;
	}

	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth));
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().set_texture((Texture)(object)texture);
		cameraFade.GetComponent<GUITexture>().set_color(new Color(0.5f, 0.5f, 0.5f, 0f));
		return cameraFade;
	}

	public static GameObject CameraFadeAdd()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)cameraFade))
		{
			return null;
		}
		cameraFade = new GameObject("iTween Camera Fade");
		cameraFade.get_transform().set_position(new Vector3(0.5f, 0.5f, (float)Defaults.cameraFadeDepth));
		cameraFade.AddComponent<GUITexture>();
		cameraFade.GetComponent<GUITexture>().set_texture((Texture)(object)CameraTexture(Color.get_black()));
		cameraFade.GetComponent<GUITexture>().set_color(new Color(0.5f, 0.5f, 0.5f, 0f));
		return cameraFade;
	}

	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			((Behaviour)(iTween)(object)components[i]).set_enabled(true);
		}
	}

	public static void Resume(GameObject target, bool includechildren)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Resume(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Resume(((Component)item).get_gameObject(), includechildren: true);
		}
	}

	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				((Behaviour)iTween2).set_enabled(true);
			}
		}
	}

	public static void Resume(GameObject target, string type, bool includechildren)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				((Behaviour)iTween2).set_enabled(true);
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Resume(((Component)item).get_gameObject(), type, includechildren: true);
		}
	}

	public static void Resume()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Resume((GameObject)((Hashtable)tweens[i])["target"]);
		}
	}

	public static void Resume(string type)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Resume((GameObject)arrayList[j], type);
		}
	}

	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if (iTween2.delay > 0f)
			{
				iTween2.delay -= Time.get_time() - iTween2.delayStarted;
				((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
			}
			iTween2.isPaused = true;
			((Behaviour)iTween2).set_enabled(false);
		}
	}

	public static void Pause(GameObject target, bool includechildren)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Pause(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Pause(((Component)item).get_gameObject(), includechildren: true);
		}
	}

	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.get_time() - iTween2.delayStarted;
					((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				((Behaviour)iTween2).set_enabled(false);
			}
		}
	}

	public static void Pause(GameObject target, string type, bool includechildren)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				if (iTween2.delay > 0f)
				{
					iTween2.delay -= Time.get_time() - iTween2.delayStarted;
					((MonoBehaviour)iTween2).StopCoroutine("TweenDelay");
				}
				iTween2.isPaused = true;
				((Behaviour)iTween2).set_enabled(false);
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Pause(((Component)item).get_gameObject(), type, includechildren: true);
		}
	}

	public static void Pause()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Pause((GameObject)((Hashtable)tweens[i])["target"]);
		}
	}

	public static void Pause(string type)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Pause((GameObject)arrayList[j], type);
		}
	}

	public static int Count()
	{
		return tweens.Count;
	}

	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)tweens[i];
			if (((string)hashtable["type"] + (string)hashtable["method"]).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static int Count(GameObject target)
	{
		return target.GetComponents(typeof(iTween)).Length;
	}

	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	public static void Stop()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		for (int i = 0; i < tweens.Count; i++)
		{
			Stop((GameObject)((Hashtable)tweens[i])["target"]);
		}
		tweens.Clear();
	}

	public static void Stop(string type)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			Stop((GameObject)arrayList[j], type);
		}
	}

	public static void StopByName(string name)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < tweens.Count; i++)
		{
			GameObject value = (GameObject)((Hashtable)tweens[i])["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			StopByName((GameObject)arrayList[j], name);
		}
	}

	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			((iTween)(object)components[i]).Dispose();
		}
	}

	public static void Stop(GameObject target, bool includechildren)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		Stop(target);
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Stop(((Component)item).get_gameObject(), includechildren: true);
		}
	}

	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
	}

	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
	}

	public static void Stop(GameObject target, string type, bool includechildren)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if ((iTween2.type + iTween2.method).Substring(0, type.Length).ToLower() == type.ToLower())
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			Stop(((Component)item).get_gameObject(), type, includechildren: true);
		}
	}

	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Component[] components = target.GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if (iTween2._name == name)
			{
				iTween2.Dispose();
			}
		}
		if (!includechildren)
		{
			return;
		}
		foreach (Transform item in target.get_transform())
		{
			StopByName(((Component)item).get_gameObject(), name, includechildren: true);
		}
	}

	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			Debug.LogError((object)"Tween Error: Hash requires an even number of arguments!");
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	private void Awake()
	{
		RetrieveArgs();
		lastRealTime = Time.get_realtimeSinceStartup();
	}

	private IEnumerator Start()
	{
		if (delay > 0f)
		{
			yield return ((MonoBehaviour)this).StartCoroutine("TweenDelay");
		}
		TweenStart();
	}

	private void Update()
	{
		if (!isRunning || physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	private void FixedUpdate()
	{
		if (!isRunning || !physics)
		{
			return;
		}
		if (!reverse)
		{
			if (percentage < 1f)
			{
				TweenUpdate();
			}
			else
			{
				TweenComplete();
			}
		}
		else if (percentage > 0f)
		{
			TweenUpdate();
		}
		else
		{
			TweenComplete();
		}
	}

	private void LateUpdate()
	{
		if (tweenArguments.Contains("looktarget") && isRunning && (type == "move" || type == "shake" || type == "punch"))
		{
			LookUpdate(((Component)this).get_gameObject(), tweenArguments);
		}
	}

	private void OnEnable()
	{
		if (isRunning)
		{
			EnableKinematic();
		}
		if (isPaused)
		{
			isPaused = false;
			if (delay > 0f)
			{
				wasPaused = true;
				ResumeDelay();
			}
		}
	}

	private void OnDisable()
	{
		DisableKinematic();
	}

	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(color);
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				Debug.LogError((object)"iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
		}
	}

	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		Vector3[] pts = PathControlPointGenerator(path);
		Vector3 val = Interp(pts, 0f);
		Gizmos.set_color(color);
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 val2 = Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(val2, val);
			}
			else if (method == "handles")
			{
				Debug.LogError((object)"iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!");
			}
			val = val2;
		}
	}

	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		int num = 2;
		Vector3[] array = (Vector3[])(object)new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = (Vector3[])(object)new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = (Vector3[])(object)new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	private static Vector3 Interp(Vector3[] pts, float t)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 val = pts[num2];
		Vector3 val2 = pts[num2 + 1];
		Vector3 val3 = pts[num2 + 2];
		Vector3 val4 = pts[num2 + 3];
		return 0.5f * ((-val + 3f * val2 - 3f * val3 + val4) * (num3 * num3 * num3) + (2f * val - 5f * val2 + 4f * val3 - val4) * (num3 * num3) + (-val + val3) * num3 + 2f * val2);
	}

	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		tweens.Insert(0, args);
		target.AddComponent<iTween>();
	}

	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		foreach (DictionaryEntry arg in args)
		{
			hashtable.Add(arg.Key, arg.Value);
		}
		foreach (DictionaryEntry item in hashtable)
		{
			if (item.Value.GetType() == typeof(int))
			{
				float num = (int)item.Value;
				args[item.Key] = num;
			}
			if (item.Value.GetType() == typeof(double))
			{
				float num2 = (float)(double)item.Value;
				args[item.Key] = num2;
			}
		}
		foreach (DictionaryEntry arg2 in args)
		{
			hashtable2.Add(arg2.Key.ToString().ToLower(), arg2.Value);
		}
		args = hashtable2;
		return args;
	}

	private static string GenerateID()
	{
		int num = 15;
		char[] array = new char[61]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D',
			'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N',
			'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
			'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7',
			'8'
		};
		int num2 = array.Length - 1;
		string text = "";
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor((float)Random.Range(0, num2))];
		}
		return text;
	}

	private void RetrieveArgs()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		foreach (Hashtable tween in tweens)
		{
			if ((Object)(GameObject)tween["target"] == (Object)(object)((Component)this).get_gameObject())
			{
				tweenArguments = tween;
				break;
			}
		}
		id = (string)tweenArguments["id"];
		type = (string)tweenArguments["type"];
		_name = (string)tweenArguments["name"];
		method = (string)tweenArguments["method"];
		if (tweenArguments.Contains("time"))
		{
			time = (float)tweenArguments["time"];
		}
		else
		{
			time = Defaults.time;
		}
		if ((Object)(object)((Component)this).GetComponent<Rigidbody>() != (Object)null)
		{
			physics = true;
		}
		if (tweenArguments.Contains("delay"))
		{
			delay = (float)tweenArguments["delay"];
		}
		else
		{
			delay = Defaults.delay;
		}
		if (tweenArguments.Contains("namedcolorvalue"))
		{
			if (tweenArguments["namedcolorvalue"].GetType() == typeof(NamedValueColor))
			{
				namedcolorvalue = (NamedValueColor)tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					namedcolorvalue = (NamedValueColor)Enum.Parse(typeof(NamedValueColor), (string)tweenArguments["namedcolorvalue"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported namedcolorvalue supplied! Default will be used.");
					namedcolorvalue = NamedValueColor._Color;
				}
			}
		}
		else
		{
			namedcolorvalue = Defaults.namedColorValue;
		}
		if (tweenArguments.Contains("looptype"))
		{
			if (tweenArguments["looptype"].GetType() == typeof(LoopType))
			{
				loopType = (LoopType)tweenArguments["looptype"];
			}
			else
			{
				try
				{
					loopType = (LoopType)Enum.Parse(typeof(LoopType), (string)tweenArguments["looptype"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported loopType supplied! Default will be used.");
					loopType = LoopType.none;
				}
			}
		}
		else
		{
			loopType = LoopType.none;
		}
		if (tweenArguments.Contains("easetype"))
		{
			if (tweenArguments["easetype"].GetType() == typeof(EaseType))
			{
				easeType = (EaseType)tweenArguments["easetype"];
			}
			else
			{
				try
				{
					easeType = (EaseType)Enum.Parse(typeof(EaseType), (string)tweenArguments["easetype"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported easeType supplied! Default will be used.");
					easeType = Defaults.easeType;
				}
			}
		}
		else
		{
			easeType = Defaults.easeType;
		}
		if (tweenArguments.Contains("space"))
		{
			if (tweenArguments["space"].GetType() == typeof(Space))
			{
				space = (Space)tweenArguments["space"];
			}
			else
			{
				try
				{
					space = (Space)Enum.Parse(typeof(Space), (string)tweenArguments["space"], ignoreCase: true);
				}
				catch
				{
					Debug.LogWarning((object)"iTween: Unsupported space supplied! Default will be used.");
					space = Defaults.space;
				}
			}
		}
		else
		{
			space = Defaults.space;
		}
		if (tweenArguments.Contains("islocal"))
		{
			isLocal = (bool)tweenArguments["islocal"];
		}
		else
		{
			isLocal = Defaults.isLocal;
		}
		if (tweenArguments.Contains("ignoretimescale"))
		{
			useRealTime = (bool)tweenArguments["ignoretimescale"];
		}
		else
		{
			useRealTime = Defaults.useRealTime;
		}
		GetEasingFunction();
	}

	private void GetEasingFunction()
	{
		switch (easeType)
		{
		case EaseType.easeInQuad:
			ease = easeInQuad;
			break;
		case EaseType.easeOutQuad:
			ease = easeOutQuad;
			break;
		case EaseType.easeInOutQuad:
			ease = easeInOutQuad;
			break;
		case EaseType.easeInCubic:
			ease = easeInCubic;
			break;
		case EaseType.easeOutCubic:
			ease = easeOutCubic;
			break;
		case EaseType.easeInOutCubic:
			ease = easeInOutCubic;
			break;
		case EaseType.easeInQuart:
			ease = easeInQuart;
			break;
		case EaseType.easeOutQuart:
			ease = easeOutQuart;
			break;
		case EaseType.easeInOutQuart:
			ease = easeInOutQuart;
			break;
		case EaseType.easeInQuint:
			ease = easeInQuint;
			break;
		case EaseType.easeOutQuint:
			ease = easeOutQuint;
			break;
		case EaseType.easeInOutQuint:
			ease = easeInOutQuint;
			break;
		case EaseType.easeInSine:
			ease = easeInSine;
			break;
		case EaseType.easeOutSine:
			ease = easeOutSine;
			break;
		case EaseType.easeInOutSine:
			ease = easeInOutSine;
			break;
		case EaseType.easeInExpo:
			ease = easeInExpo;
			break;
		case EaseType.easeOutExpo:
			ease = easeOutExpo;
			break;
		case EaseType.easeInOutExpo:
			ease = easeInOutExpo;
			break;
		case EaseType.easeInCirc:
			ease = easeInCirc;
			break;
		case EaseType.easeOutCirc:
			ease = easeOutCirc;
			break;
		case EaseType.easeInOutCirc:
			ease = easeInOutCirc;
			break;
		case EaseType.linear:
			ease = linear;
			break;
		case EaseType.spring:
			ease = spring;
			break;
		case EaseType.easeInBounce:
			ease = easeInBounce;
			break;
		case EaseType.easeOutBounce:
			ease = easeOutBounce;
			break;
		case EaseType.easeInOutBounce:
			ease = easeInOutBounce;
			break;
		case EaseType.easeInBack:
			ease = easeInBack;
			break;
		case EaseType.easeOutBack:
			ease = easeOutBack;
			break;
		case EaseType.easeInOutBack:
			ease = easeInOutBack;
			break;
		case EaseType.easeInElastic:
			ease = easeInElastic;
			break;
		case EaseType.easeOutElastic:
			ease = easeOutElastic;
			break;
		case EaseType.easeInOutElastic:
			ease = easeInOutElastic;
			break;
		}
	}

	private void UpdatePercentage()
	{
		if (useRealTime)
		{
			runningTime += Time.get_realtimeSinceStartup() - lastRealTime;
		}
		else
		{
			runningTime += Time.get_deltaTime();
		}
		if (reverse)
		{
			percentage = 1f - runningTime / time;
		}
		else
		{
			percentage = runningTime / time;
		}
		lastRealTime = Time.get_realtimeSinceStartup();
	}

	private void CallBack(string callbackType)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		if (tweenArguments.Contains(callbackType) && !tweenArguments.Contains("ischild"))
		{
			GameObject val = (GameObject)((!tweenArguments.Contains(callbackType + "target")) ? ((object)((Component)this).get_gameObject()) : ((object)(GameObject)tweenArguments[callbackType + "target"]));
			if (tweenArguments[callbackType].GetType() == typeof(string))
			{
				val.SendMessage((string)tweenArguments[callbackType], tweenArguments[callbackType + "params"], (SendMessageOptions)1);
				return;
			}
			Debug.LogError((object)"iTween Error: Callback method references must be passed as a String!");
			Object.Destroy((Object)(object)this);
		}
	}

	private void Dispose()
	{
		for (int i = 0; i < tweens.Count; i++)
		{
			if ((string)((Hashtable)tweens[i])["id"] == id)
			{
				tweens.RemoveAt(i);
				break;
			}
		}
		Object.Destroy((Object)(object)this);
	}

	private void ConflictCheck()
	{
		Component[] components = ((Component)this).GetComponents(typeof(iTween));
		for (int i = 0; i < components.Length; i++)
		{
			iTween iTween2 = (iTween)(object)components[i];
			if (iTween2.type == "value")
			{
				break;
			}
			if (!iTween2.isRunning || !(iTween2.type == type))
			{
				continue;
			}
			if (iTween2.method != method)
			{
				break;
			}
			if (iTween2.tweenArguments.Count != tweenArguments.Count)
			{
				iTween2.Dispose();
				break;
			}
			foreach (DictionaryEntry tweenArgument in tweenArguments)
			{
				if (!iTween2.tweenArguments.Contains(tweenArgument.Key))
				{
					iTween2.Dispose();
					return;
				}
				if (!iTween2.tweenArguments[tweenArgument.Key].Equals(tweenArguments[tweenArgument.Key]) && (string)tweenArgument.Key != "id")
				{
					iTween2.Dispose();
					return;
				}
			}
			Dispose();
		}
	}

	private void EnableKinematic()
	{
	}

	private void DisableKinematic()
	{
	}

	private void ResumeDelay()
	{
		((MonoBehaviour)this).StartCoroutine("TweenDelay");
	}

	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float num4 = 0f;
		float num5 = 0f;
		if (end - start < 0f - num3)
		{
			num5 = (num2 - start + end) * value;
			return start + num5;
		}
		if (end - start > num3)
		{
			num5 = (0f - (num2 - end + start)) * value;
			return start + num5;
		}
		return start + (end - start) * value;
	}

	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * (float)Math.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * value * (value - 2f) + start;
	}

	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return (0f - end) / 2f * (value * (value - 2f) - 1f) + start;
	}

	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return (0f - end) * (value * value * value * value - 1f) + start;
	}

	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return (0f - end) / 2f * (value * value * value * value - 2f) + start;
	}

	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * Mathf.Cos(value / 1f * ((float)Math.PI / 2f)) + end + start;
	}

	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * ((float)Math.PI / 2f)) + start;
	}

	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return (0f - end) / 2f * (Mathf.Cos((float)Math.PI * value / 1f) - 1f) + start;
	}

	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (0f - Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (0f - Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return (0f - end) * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return (0f - end) / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - easeOutBounce(0f, end, num - value) + start;
	}

	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.363636374f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.727272749f)
		{
			value -= 0.545454562f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.90909090909090906)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 21f / 22f;
		return end * (7.5625f * value * value + 63f / 64f) + start;
	}

	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	private float punch(float amplitude, float value)
	{
		float num = 9f;
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num2 = 0.3f;
		num = num2 / ((float)Math.PI * 2f) * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num) * ((float)Math.PI * 2f) / num2);
	}

	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		return 0f - num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) + start;
	}

	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		return num4 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) + end + start;
	}

	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		float num4 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		if (num4 == 0f || num4 < Mathf.Abs(end))
		{
			num4 = end;
			num3 = num2 / 4f;
		}
		else
		{
			num3 = num2 / ((float)Math.PI * 2f) * Mathf.Asin(end / num4);
		}
		if (value < 1f)
		{
			return -0.5f * (num4 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2)) + start;
		}
		return num4 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num3) * ((float)Math.PI * 2f) / num2) * 0.5f + end + start;
	}

	public iTween()
		: this()
	{
	}
}
