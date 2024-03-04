using UnityEngine;

namespace HumanAPI
{
	public class IgnoreCollisionOverlap : MonoBehaviour
	{
		public Collider volumeCollider;

		public bool recursive = true;

		private void OnEnable()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			volumeCollider.set_enabled(true);
			Bounds bounds = volumeCollider.get_bounds();
			Collider[] array = Physics.OverlapBox(((Bounds)(ref bounds)).get_center(), ((Bounds)(ref bounds)).get_extents());
			volumeCollider.set_enabled(false);
			if (recursive)
			{
				Collider[] componentsInChildren = ((Component)this).GetComponentsInChildren<Collider>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					for (int j = 0; j < array.Length; j++)
					{
						Physics.IgnoreCollision(componentsInChildren[i], array[j], true);
					}
				}
			}
			else
			{
				Collider component = ((Component)this).GetComponent<Collider>();
				for (int k = 0; k < array.Length; k++)
				{
					Physics.IgnoreCollision(component, array[k], true);
				}
			}
		}

		public IgnoreCollisionOverlap()
			: this()
		{
		}
	}
}
