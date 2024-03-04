using UnityEngine;

namespace HumanAPI
{
	public abstract class LevelObject : MonoBehaviour
	{
		protected Level level;

		protected bool active
		{
			get
			{
				if ((Object)(object)level == (Object)null)
				{
					level = ((Component)this).GetComponentInParent<Level>();
				}
				if (((Behaviour)this).get_enabled() && level.active)
				{
					return ((Component)this).get_gameObject().get_activeInHierarchy();
				}
				return false;
			}
		}

		protected virtual void OnEnable()
		{
			level = ((Component)this).GetComponentInParent<Level>();
			if ((Object)(object)level == (Object)null)
			{
				Debug.LogError((object)"LevelObject must be placed in a level");
			}
		}

		protected LevelObject()
			: this()
		{
		}
	}
}
