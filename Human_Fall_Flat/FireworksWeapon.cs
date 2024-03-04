using UnityEngine;

public class FireworksWeapon : MonoBehaviour
{
	public Transform muzzle;

	private float cooldown;

	public void Place(Vector3 pos, Quaternion dir)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_position(pos);
		((Component)this).get_transform().set_rotation(dir);
	}

	public bool CanShoot()
	{
		return cooldown < Time.get_time();
	}

	public void Shoot()
	{
		FireworksProjectile projectile = Fireworks.instance.GetProjectile();
		if ((Object)(object)projectile != (Object)null)
		{
			projectile.Shoot(muzzle);
			cooldown = Time.get_time() + 1f;
		}
	}

	public FireworksWeapon()
		: this()
	{
	}
}
