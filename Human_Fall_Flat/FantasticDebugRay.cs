using UnityEngine;

[ExecuteInEditMode]
public class FantasticDebugRay : MonoBehaviour
{
	[SerializeField]
	private bool local = true;

	[SerializeField]
	private Color color = Color.get_white();

	[SerializeField]
	private float length = 1f;

	private void Start()
	{
	}

	private void Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Vector3 forward = Vector3.get_forward();
		if (local)
		{
			forward = ((Component)this).get_transform().get_forward();
		}
		Debug.DrawRay(((Component)this).get_transform().get_position(), ((Vector3)(ref forward)).get_normalized() * length, color);
	}

	public FantasticDebugRay()
		: this()
	{
	}//IL_0008: Unknown result type (might be due to invalid IL or missing references)
	//IL_000d: Unknown result type (might be due to invalid IL or missing references)

}
