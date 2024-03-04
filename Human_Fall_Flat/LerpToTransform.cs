using UnityEngine;

public class LerpToTransform : MonoBehaviour
{
	public GameObject objectToLerp;

	public Transform destinationTransform;

	public float lerpTime = 1f;

	private float t = 1.1f;

	private void Reset()
	{
		objectToLerp = ((Component)this).get_gameObject();
	}

	private void OnValidate()
	{
		objectToLerp = objectToLerp ?? ((Component)this).get_gameObject();
	}

	public void BeginLerp()
	{
		t = 0f;
	}

	private void FixedUpdate()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if (t < 1f)
		{
			t += Mathf.Clamp01(1f / lerpTime * Time.get_fixedDeltaTime());
			objectToLerp.get_transform().set_position(Vector3.Lerp(objectToLerp.get_transform().get_position(), destinationTransform.get_position(), t));
		}
	}

	public LerpToTransform()
		: this()
	{
	}
}
