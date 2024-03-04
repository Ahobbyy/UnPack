using System.Collections;
using Multiplayer;
using UnityEngine;

public class GiftHole : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (NetGame.isClient || ReplayRecorder.isPlaying)
		{
			return;
		}
		GiftBase giftBase = ((Component)other).GetComponent<GiftBase>();
		if ((Object)(object)giftBase == (Object)null)
		{
			CapsuleCollider component = ((Component)other).GetComponent<CapsuleCollider>();
			if ((Object)(object)component != (Object)null && component.get_height() > 3f)
			{
				giftBase = ((Component)other).GetComponentInParent<GiftBase>();
			}
		}
		if ((Object)(object)giftBase != (Object)null)
		{
			if ((Object)(object)GiftService.instance != (Object)null)
			{
				GiftService.instance.DeliverGift(giftBase.userId, giftBase.giftId);
			}
			if (giftBase is BigGift)
			{
				((MonoBehaviour)this).StartCoroutine(BigGiftChutes());
			}
			else
			{
				Fireworks.instance.ShootFirework();
			}
		}
	}

	private IEnumerator BigGiftChutes()
	{
		for (int i = 0; i < 10; i++)
		{
			Fireworks.instance.ShootFirework();
			yield return (object)new WaitForSeconds(Random.Range(0.05f, 1f));
		}
	}

	public GiftHole()
		: this()
	{
	}
}
