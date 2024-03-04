using System;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
	public Vector3 axis = new Vector3(0f, 0f, 1f);

	private Quaternion originalRotation;

	public float timeOffset;

	public float period;

	public float amplitude;

	private Rigidbody body;

	private float time;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		body = ((Component)this).GetComponent<Rigidbody>();
		originalRotation = ((Component)this).get_transform().get_rotation();
	}

	public void FixedUpdate()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		body.MoveRotation(originalRotation * Quaternion.AngleAxis(Mathf.Sin((float)Math.PI * 2f * (timeOffset + time) / period) * amplitude, axis));
		time += Time.get_fixedDeltaTime();
	}

	public Pendulum()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)

}
