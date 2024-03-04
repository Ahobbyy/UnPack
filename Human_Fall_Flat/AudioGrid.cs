using System;
using HumanAPI;
using UnityEngine;

public class AudioGrid : MonoBehaviour
{
	public static AudioGrid instance;

	public Texture2D map;

	public Vector3 size = new Vector3(100f, 10f, 100f);

	public AudioSource fallbackSource;

	public float fallbackVolume;

	public AudioSource[] sources;

	public Sound2[] sounds;

	public float[] maxVolumes;

	public float[] lowPass;

	public float[] currentVolumes;

	public float volumeTreshold = 0.01f;

	public bool interactive;

	public Color currentColor;

	public bool drawgrid;

	public float subdiv = 32f;

	public float fadeDuration = 2f;

	private AudioLowPassFilter[] lp;

	private void OnEnable()
	{
		instance = this;
	}

	private void Awake()
	{
		currentVolumes = new float[sounds.Length];
		lp = (AudioLowPassFilter[])(object)new AudioLowPassFilter[sounds.Length];
		for (int i = 0; i < sounds.Length; i++)
		{
			if ((Object)(object)sounds[i] != (Object)null)
			{
				lp[i] = ((Component)sounds[i]).GetComponent<AudioLowPassFilter>();
			}
		}
	}

	private void Update()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		if (!interactive)
		{
			Vector3 val = Vector3.Scale(((Component)this).get_transform().InverseTransformPoint(((Component)Listener.instance).get_transform().get_position()), new Vector3(1f / size.x, 1f / size.y, 1f / size.z));
			currentColor = map.GetPixelBilinear(val.x, val.z);
		}
		Color val2 = currentColor;
		float num = 0f;
		if (AudioGridOverride.all.Count > 0)
		{
			num = AudioGridOverride.all[0].ApplyVolume(0, ((Component)Listener.instance).get_transform().get_position());
		}
		float num2 = 1f - num;
		ApplyVolume(0, Mathf.Clamp01(2f * val2.r - 1f) * val2.a * num2);
		ApplyVolume(1, Mathf.Clamp01(1f - 2f * val2.r) * val2.a * num2);
		ApplyVolume(2, Mathf.Clamp01(2f * val2.g - 1f) * val2.a * num2);
		ApplyVolume(3, Mathf.Clamp01(1f - 2f * val2.g) * val2.a * num2);
		ApplyVolume(4, Mathf.Clamp01(2f * val2.b - 1f) * val2.a * num2);
		ApplyVolume(5, Mathf.Clamp01(1f - 2f * val2.b) * val2.a * num2);
		if (sounds.Length > 6 && (Object)(object)sounds[6] != (Object)null)
		{
			ApplyVolume(6, (1f - val2.a) * num2);
		}
	}

	internal void ZoneEntered(AudioGridOverride audioGridOverride)
	{
		throw new NotImplementedException();
	}

	private void ApplyVolume(int idx, float volume)
	{
		if (sounds.Length < idx || (Object)(object)sounds[idx] == (Object)null)
		{
			return;
		}
		if (volume < volumeTreshold)
		{
			if (sounds[idx].isPlaying)
			{
				sounds[idx].Stop();
			}
		}
		else if (!sounds[idx].isPlaying)
		{
			sounds[idx].Play();
		}
		currentVolumes[idx] = Mathf.MoveTowards(currentVolumes[idx], volume, Time.get_deltaTime() / fadeDuration);
		sounds[idx].SetVolume(currentVolumes[idx]);
		if (lowPass.Length > idx)
		{
			sounds[idx].SetLowPass(Mathf.Lerp(lowPass[idx], 22000f, Mathf.Sqrt(volume)));
		}
	}

	public void OnDrawGizmosSelected()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		if (!((Behaviour)this).get_enabled() || !((Component)this).get_gameObject().get_activeInHierarchy() || !drawgrid)
		{
			return;
		}
		for (float num = 0f; num <= 1f; num += 1f / subdiv)
		{
			for (float num2 = 0f; num2 <= 1f; num2 += 1f / subdiv)
			{
				Gizmos.set_color(map.GetPixelBilinear(num, num2));
				Gizmos.DrawCube(((Component)this).get_transform().TransformPoint(num * size.x, 0f, num2 * size.z), size / subdiv);
			}
		}
		if ((Object)(object)Listener.instance != (Object)null)
		{
			Vector3 val = Vector3.Scale(((Component)this).get_transform().InverseTransformPoint(((Component)Listener.instance).get_transform().get_position()), new Vector3(1f / size.x, 1f / size.y, 1f / size.z));
			Gizmos.set_color(map.GetPixelBilinear(val.x, val.z));
			Gizmos.DrawSphere(((Component)this).get_transform().TransformPoint(val.x * size.x, 0f, val.z * size.z), size.x / subdiv);
		}
	}

	public AudioGrid()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)

}
