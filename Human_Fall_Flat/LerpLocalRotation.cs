using System.Collections;
using UnityEngine;

public class LerpLocalRotation : MonoBehaviour
{
	private Quaternion startingRotation;

	private Quaternion currentRotation;

	public float duration = 1f;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		startingRotation = ((Component)this).get_transform().get_localRotation();
	}

	public void CheckRotation()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		currentRotation = ((Component)this).get_transform().get_localRotation();
		if (currentRotation != startingRotation)
		{
			((MonoBehaviour)this).StartCoroutine(ReturnToRotation());
		}
	}

	private IEnumerator ReturnToRotation()
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			((Component)this).get_transform().set_localRotation(Quaternion.Lerp(currentRotation, startingRotation, elapsedTime / duration));
			elapsedTime += Time.get_deltaTime();
			yield return (object)new WaitForEndOfFrame();
		}
		((Component)this).get_transform().set_localRotation(startingRotation);
	}

	public LerpLocalRotation()
		: this()
	{
	}
}
