using UnityEngine;

public class WaterPlane : WaterBody
{
	[Tooltip("Reference to the down vector for this game object")]
	public Vector3 down;

	[Tooltip("Reference to the direction the water is moving in")]
	public Vector3 flow;

	[Tooltip("Reference to the direction all water should be moving in")]
	public Vector3 globalFlow;

	private float D;

	private Vector3 normal;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	[Tooltip("Water height changes runtime")]
	public bool dynamicHeight;

	[SerializeField]
	private bool SetPlaneHeight;

	[SerializeField]
	private float PlaneHeight;

	private void Start()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Setting up the directions for stuff "));
		}
		normal = ((Component)this).get_transform().TransformDirection(down);
		Vector3 position = default(Vector3);
		if (SetPlaneHeight && !dynamicHeight)
		{
			((Vector3)(ref position))._002Ector(0f, PlaneHeight, 0f);
		}
		else
		{
			position = ((Component)this).get_transform().get_position();
		}
		D = Vector3.Dot(position, normal);
		globalFlow = ((Component)this).get_transform().TransformVector(flow);
	}

	public override float SampleDepth(Vector3 pos, out Vector3 velocity)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Getting the direction we should be moving in "));
		}
		velocity = globalFlow;
		if (dynamicHeight)
		{
			D = Vector3.Dot(((Component)this).get_transform().get_position(), normal);
		}
		return Vector3.Dot(pos, normal) - D;
	}
}
