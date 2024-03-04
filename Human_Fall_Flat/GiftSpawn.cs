using UnityEngine;

public class GiftSpawn : MonoBehaviour
{
	public float probability = 1f;

	public Vector3 GetPosition()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)this).get_transform().TransformPoint(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
	}

	private void OnDrawGizmosSelected()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_matrix(((Component)this).get_transform().get_localToWorldMatrix());
		Gizmos.set_color(Color.get_green());
		Gizmos.DrawCube(Vector3.get_zero(), Vector3.get_one());
	}

	public GiftSpawn()
		: this()
	{
	}
}
