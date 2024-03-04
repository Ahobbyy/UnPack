using UnityEngine;
using UnityEngine.Audio;

public class GridSnapshot : MonoBehaviour
{
	public AudioMixer mixer;

	public Texture2D map;

	public AudioMixerSnapshot[] snapshots;

	private float[] weights;

	public Vector3 size = new Vector3(100f, 10f, 100f);

	private void Update()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		if (weights == null)
		{
			weights = new float[snapshots.Length];
		}
		Vector3 val = Vector3.Scale(((Component)this).get_transform().InverseTransformPoint(((Component)Listener.instance).get_transform().get_position()), new Vector3(1f / size.x, 1f / size.y, 1f / size.z));
		Color pixelBilinear = map.GetPixelBilinear(val.x, val.z);
		weights[0] = Mathf.Clamp01(2f * pixelBilinear.r - 1f);
		weights[1] = Mathf.Clamp01(1f - 2f * pixelBilinear.r);
		weights[2] = Mathf.Clamp01(2f * pixelBilinear.g - 1f);
		weights[3] = Mathf.Clamp01(1f - 2f * pixelBilinear.g);
		weights[4] = Mathf.Clamp01(2f * pixelBilinear.b - 1f);
		weights[5] = Mathf.Clamp01(1f - 2f * pixelBilinear.b);
		mixer.TransitionToSnapshots(snapshots, weights, Time.get_deltaTime());
	}

	public void OnDrawGizmosSelected()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		if (!((Component)this).get_gameObject().get_activeInHierarchy() || !((Behaviour)this).get_enabled())
		{
			return;
		}
		float num = 32f;
		for (float num2 = 0f; num2 <= 1f; num2 += 1f / num)
		{
			for (float num3 = 0f; num3 <= 1f; num3 += 1f / num)
			{
				Gizmos.set_color(map.GetPixelBilinear(num2, num3));
				Gizmos.DrawCube(((Component)this).get_transform().TransformPoint(num2 * size.x, 0f, num3 * size.z), size / num);
			}
		}
		if ((Object)(object)Listener.instance != (Object)null)
		{
			Vector3 val = Vector3.Scale(((Component)this).get_transform().InverseTransformPoint(((Component)Listener.instance).get_transform().get_position()), new Vector3(1f / size.x, 1f / size.y, 1f / size.z));
			Gizmos.set_color(map.GetPixelBilinear(val.x, val.z));
			Gizmos.DrawSphere(((Component)this).get_transform().TransformPoint(val.x * size.x, 0f, val.z * size.z), size.x / num);
		}
	}

	public GridSnapshot()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)

}
