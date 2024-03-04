using HumanAPI;
using UnityEngine;

public class ScrewHingePlatform : Node
{
	[SerializeField]
	private Rigidbody rotableRigidbody;

	[SerializeField]
	private float localMaxY;

	[SerializeField]
	private float localMinY;

	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private float maxAngularVelocityY = 1f;

	public NodeInput grabState;

	public NodeInput isCollidingBottom;

	private Vector3 topLocalPosition;

	private Vector3 bottomLocalPosition;

	private Vector3 maxAngularVelocityVector;

	private Vector3 targetPosition;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		topLocalPosition = new Vector3(0f, localMaxY, 0f);
		bottomLocalPosition = new Vector3(0f, localMinY, 0f);
		maxAngularVelocityVector = new Vector3(0f, maxAngularVelocityY, 0f);
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).get_transform().get_localPosition().y >= bottomLocalPosition.y + 0.1f && grabState.value < 1f && isCollidingBottom.value != 1f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Falling , player has let go "));
			}
			rotableRigidbody.AddRelativeTorque(Vector3.get_up() * 8000f, (ForceMode)0);
		}
		if (rotableRigidbody.get_angularVelocity().y > maxAngularVelocityY)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Soinning too fast + "));
			}
			rotableRigidbody.set_angularVelocity(maxAngularVelocityVector);
		}
		else if (rotableRigidbody.get_angularVelocity().y < 0f - maxAngularVelocityY)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Soinning too fast - "));
			}
			rotableRigidbody.set_angularVelocity(-maxAngularVelocityVector);
		}
		if ((((Component)this).get_transform().get_localPosition().y >= topLocalPosition.y - 0.05f && rotableRigidbody.get_angularVelocity().y < 0f) || (((Component)this).get_transform().get_localPosition().y <= bottomLocalPosition.y + 0.05f && rotableRigidbody.get_angularVelocity().y > 0f) || (rotableRigidbody.get_angularVelocity().y > 0f && isCollidingBottom.value == 1f))
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Sector D "));
			}
			rotableRigidbody.set_angularVelocity(Vector3.get_zero());
		}
		else if (rotableRigidbody.get_angularVelocity().y > 0f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Going Down"));
			}
			((Component)this).get_transform().set_localPosition(Vector3.MoveTowards(((Component)this).get_transform().get_localPosition(), bottomLocalPosition, Time.get_fixedDeltaTime() * speed * Mathf.Abs(rotableRigidbody.get_angularVelocity().y)));
		}
		else if (rotableRigidbody.get_angularVelocity().y < 0f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Going Up "));
			}
			((Component)this).get_transform().set_localPosition(Vector3.MoveTowards(((Component)this).get_transform().get_localPosition(), topLocalPosition, Time.get_fixedDeltaTime() * speed * Mathf.Abs(rotableRigidbody.get_angularVelocity().y)));
		}
	}
}
