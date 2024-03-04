using System.Collections;
using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
	public Vector3 directionToMove = new Vector3(1f, 0f, 0f);

	public float moveDuration = 2f;

	public float speed = 1f;

	private Vector3 startLoc;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		startLoc = ((Component)this).get_transform().get_position();
	}

	public void StartMove()
	{
		((MonoBehaviour)this).StartCoroutine(MoveObject(moveDuration));
	}

	private IEnumerator MoveObject(float duration)
	{
		float elapsedTime = 0f;
		((Component)this).get_transform().get_position();
		while (elapsedTime < duration)
		{
			Transform transform = ((Component)this).get_transform();
			transform.set_position(transform.get_position() + directionToMove * Time.get_deltaTime() * speed);
			elapsedTime += Time.get_deltaTime();
			yield return (object)new WaitForEndOfFrame();
		}
	}

	public void ResetPosition()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_position(startLoc);
	}

	public MoveOverTime()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)

}
