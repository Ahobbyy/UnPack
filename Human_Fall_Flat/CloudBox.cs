using System.Collections.Generic;
using UnityEngine;

public class CloudBox : MonoBehaviour
{
	public float fadeInDuration;

	public float fadeInTime;

	public float fade = 1f;

	public Vector3 innerSize = new Vector3(100f, 50f, 100f);

	public Vector3 outerSize = new Vector3(120f, 70f, 120f);

	public static List<CloudBox> all = new List<CloudBox>();

	public static object cloudLock = new object();

	private Vector3 transformPosition;

	private void OnEnable()
	{
		lock (cloudLock)
		{
			all.Add(this);
		}
	}

	private void OnDisable()
	{
		lock (cloudLock)
		{
			all.Remove(this);
		}
	}

	public void ReadPos()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		transformPosition = ((Component)this).get_transform().get_position();
	}

	public float GetAlpha(Vector3 pos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		Vector3 val = pos - transformPosition;
		float num2 = 1f - (Mathf.Abs(val.x * 2f) - innerSize.x) / (outerSize.x - innerSize.x);
		if (num2 > 0f)
		{
			float num3 = 1f - (Mathf.Abs(val.y * 2f) - innerSize.y) / (outerSize.y - innerSize.y);
			if (num3 > 0f)
			{
				float num4 = 1f - (Mathf.Abs(val.z * 2f) - innerSize.z) / (outerSize.z - innerSize.z);
				if (num4 > 0f)
				{
					num = Mathf.Clamp01(num2) * Mathf.Clamp01(num3) * Mathf.Clamp01(num4);
				}
			}
		}
		num = 1f - num;
		return fade * num + (1f - fade);
	}

	public void OnDrawGizmosSelected()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_blue());
		Gizmos.DrawWireCube(((Component)this).get_transform().get_position(), innerSize);
		Gizmos.set_color(Color.get_yellow());
		Gizmos.DrawWireCube(((Component)this).get_transform().get_position(), outerSize);
	}

	private void Update()
	{
		if (fadeInDuration == 0f)
		{
			fade = 1f;
			return;
		}
		fadeInTime += Time.get_deltaTime();
		fade = Mathf.Clamp01(fadeInTime / fadeInDuration);
	}

	public void FadeIn(float duration)
	{
		fadeInTime = 0f;
		fadeInDuration = duration;
	}

	public CloudBox()
		: this()
	{
	}//IL_001b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Unknown result type (might be due to invalid IL or missing references)
	//IL_0035: Unknown result type (might be due to invalid IL or missing references)
	//IL_003a: Unknown result type (might be due to invalid IL or missing references)

}
