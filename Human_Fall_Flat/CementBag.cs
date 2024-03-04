using System.Collections;
using UnityEngine;

public class CementBag : MonoBehaviour
{
	public float impactTreshold = 100f;

	public GameObject particlesPrefab;

	public float maxImpulse;

	public float explodeDelay = 1f;

	private float lastExplode;

	public bool ReportCollision(float impulse, Vector3 pos)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		maxImpulse = Mathf.Max(maxImpulse, impulse);
		float time = Time.get_time();
		if (impulse > impactTreshold && time > lastExplode + explodeDelay)
		{
			lastExplode = time;
			((MonoBehaviour)this).StartCoroutine(Explode(pos));
			return true;
		}
		return false;
	}

	private IEnumerator Explode(Vector3 pos)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		GameObject inst = Object.Instantiate<GameObject>(particlesPrefab, pos, Quaternion.get_identity());
		yield return (object)new WaitForSeconds(1f);
		Object.Destroy((Object)(object)inst);
	}

	public CementBag()
		: this()
	{
	}
}
