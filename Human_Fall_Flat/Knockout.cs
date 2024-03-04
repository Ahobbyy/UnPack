using UnityEngine;

public class Knockout : MonoBehaviour
{
	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	public void OnCollisionEnter(Collision collision)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Entered Collision "));
		}
		HandleCollision(collision);
	}

	public void OnCollisionStay(Collision collision)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Stayed inside collision "));
		}
		HandleCollision(collision);
	}

	private void HandleCollision(Collision collision)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Handling Collision "));
		}
		if (collision.get_contacts().Length != 0)
		{
			CollisionSensor component = ((Component)collision.get_transform()).get_gameObject().GetComponent<CollisionSensor>();
			if (!((Object)(object)component == (Object)null) && component.knockdown != 0f && !component.human.grabManager.IsGrabbed(((Component)this).get_gameObject()))
			{
				component.human.ReceiveHit(-collision.GetImpulse() / collision.get_rigidbody().get_mass() * component.knockdown);
			}
		}
	}

	public Knockout()
		: this()
	{
	}
}
