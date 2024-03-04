using UnityEngine;

namespace HumanAPI
{
	public class CollisionSound : MonoBehaviour, IReset
	{
		public bool acceptPlayer;

		public Collider[] acceptColliders;

		public float minImpulse;

		public float maxImpulse;

		public float minVolume;

		public float minDelay = 0.2f;

		private float lastSoundTime;

		private Sound2 sound;

		private void Awake()
		{
			sound = ((Component)this).GetComponentInChildren<Sound2>();
		}

		private void OnCollisionEnter(Collision collision)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Vector3 impulse = collision.get_impulse();
			float magnitude = ((Vector3)(ref impulse)).get_magnitude();
			if (magnitude < minImpulse)
			{
				return;
			}
			if (acceptPlayer)
			{
				if ((Object)(object)((Component)collision.get_collider()).GetComponentInParent<Human>() == (Object)null)
				{
					return;
				}
			}
			else
			{
				if (acceptColliders.Length == 0 || !(((Component)collision.get_collider()).get_tag() != "Player"))
				{
					return;
				}
				bool flag = false;
				for (int i = 0; i < acceptColliders.Length; i++)
				{
					if ((Object)(object)acceptColliders[i] == (Object)(object)collision.get_collider())
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
			}
			float time = Time.get_time();
			if (!(lastSoundTime + minDelay > time))
			{
				float num = Mathf.InverseLerp(minImpulse, maxImpulse, magnitude);
				if (!(num < minVolume))
				{
					sound.PlayOneShot(num);
					lastSoundTime = time;
				}
			}
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			lastSoundTime = 0f;
		}

		public CollisionSound()
			: this()
		{
		}
	}
}
