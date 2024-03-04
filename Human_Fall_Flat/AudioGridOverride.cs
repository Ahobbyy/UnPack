using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AudioGridOverride : MonoBehaviour
{
	public float layer;

	public float innerZoneOffset;

	public float maxVolume = 1f;

	public float baseOpacity = 1f;

	private BoxCollider collider;

	private Sound2 sound;

	private AudioGrid grid;

	private float currentVolume;

	public static List<AudioGridOverride> all = new List<AudioGridOverride>();

	private void OnEnable()
	{
		sound = ((Component)this).GetComponent<Sound2>();
		collider = ((Component)this).GetComponent<BoxCollider>();
		grid = ((Component)this).GetComponentInParent<AudioGrid>();
		int i;
		for (i = 0; i < all.Count && !(all[i].layer > layer); i++)
		{
		}
		all.Insert(i, this);
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	public void OnDrawGizmosSelected()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		collider = ((Component)this).GetComponent<BoxCollider>();
		Matrix4x4 matrix = Gizmos.get_matrix();
		Gizmos.set_matrix(((Component)this).get_transform().get_localToWorldMatrix());
		Gizmos.set_color(new Color(1f, 0f, 0f, 0.8f));
		Gizmos.DrawCube(collider.get_center(), collider.get_size() - Vector3.get_one() * innerZoneOffset * 2f);
		Gizmos.set_color(new Color(1f, 1f, 0f, 0.2f));
		Gizmos.DrawCube(collider.get_center(), collider.get_size());
		Gizmos.set_matrix(matrix);
	}

	public float ApplyVolume(int idx, Vector3 pos)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		if (idx + 1 < all.Count)
		{
			num = all[idx + 1].ApplyVolume(idx + 1, pos);
		}
		float num2 = GetOpacity(pos) * baseOpacity;
		if (num2 < grid.volumeTreshold)
		{
			if (sound.isPlaying)
			{
				sound.Stop();
			}
		}
		else if (!sound.isPlaying)
		{
			sound.Play();
		}
		currentVolume = Mathf.MoveTowards(currentVolume, num2 * (1f - num), Time.get_deltaTime() / grid.fadeDuration);
		sound.SetVolume(currentVolume);
		return num + (1f - num) * num2;
	}

	public float GetOpacity(Vector3 pos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).get_transform().InverseTransformPoint(pos) - collider.get_center();
		float num = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().x / 2f - Mathf.Abs(val.x));
		float num2 = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().y / 2f - Mathf.Abs(val.y));
		float num3 = Mathf.InverseLerp(0f, innerZoneOffset, collider.get_size().z / 2f - Mathf.Abs(val.z));
		return Mathf.Min(Mathf.Min(num, num2), num3);
	}

	public AudioGridOverride()
		: this()
	{
	}
}
