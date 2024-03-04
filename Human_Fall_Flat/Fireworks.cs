using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class Fireworks : NetScope
{
	public bool enableWeapons;

	public FireworksProjectile prefab;

	public FireworksWeapon weaponPrefab;

	public static Fireworks instance;

	private Queue<FireworksProjectile> queue = new Queue<FireworksProjectile>();

	private List<FireworksWeapon> weapons = new List<FireworksWeapon>();

	private List<FireworksProjectile> projectiles = new List<FireworksProjectile>();

	private float timer;

	protected override void OnEnable()
	{
		instance = this;
		((Component)weaponPrefab).get_gameObject().SetActive(false);
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		instance = null;
	}

	private void Start()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		NetGame.instance.preUpdate += OnPreFixedUpdate;
		queue.Enqueue(prefab);
		((Component)prefab).GetComponent<NetIdentity>().sceneId = 0u;
		for (int i = 1; i < 20; i++)
		{
			FireworksProjectile fireworksProjectile = Object.Instantiate<FireworksProjectile>(prefab, Vector3.get_down() * 200f, Quaternion.get_identity(), ((Component)this).get_transform());
			((Component)fireworksProjectile).GetComponent<NetIdentity>().sceneId = (uint)i;
			queue.Enqueue(fireworksProjectile);
		}
		StartNetwork();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		NetGame.instance.preUpdate -= OnPreFixedUpdate;
	}

	internal void Kill(FireworksProjectile projectile)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if (projectiles.Contains(projectile))
		{
			projectiles.Remove(projectile);
			((Component)projectile).get_transform().set_position(Vector3.get_down() * 200f);
			queue.Enqueue(projectile);
		}
	}

	public void MarkUsed(FireworksProjectile projectile)
	{
		if (!projectiles.Contains(projectile))
		{
			projectiles.Add(projectile);
		}
	}

	public FireworksProjectile GetProjectile()
	{
		while (queue.Count > 0)
		{
			FireworksProjectile fireworksProjectile = queue.Dequeue();
			if (!projectiles.Contains(fireworksProjectile))
			{
				projectiles.Add(fireworksProjectile);
				return fireworksProjectile;
			}
		}
		return null;
	}

	public void OnPreFixedUpdate()
	{
		if (NetGame.isClient || ReplayRecorder.isPlaying)
		{
			return;
		}
		for (int num = projectiles.Count - 1; num >= 0; num--)
		{
			if (!((Behaviour)projectiles[num]).get_isActiveAndEnabled())
			{
				Kill(projectiles[num]);
			}
		}
		SyncWeapons(fire: true);
	}

	private void SyncWeapons(bool fire)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		int num = (enableWeapons ? Human.all.Count : 0);
		while (weapons.Count > num)
		{
			Object.Destroy((Object)(object)((Component)weapons[0]).get_gameObject());
			weapons.RemoveAt(0);
		}
		while (weapons.Count < num)
		{
			FireworksWeapon fireworksWeapon = Object.Instantiate<FireworksWeapon>(weaponPrefab, ((Component)this).get_transform());
			((Component)fireworksWeapon).get_gameObject().SetActive(true);
			weapons.Add(fireworksWeapon);
		}
		for (int i = 0; i < num; i++)
		{
			Human human = Human.all[i];
			FireworksWeapon fireworksWeapon2 = weapons[i];
			if (human.targetDirection == Vector3.get_zero() || human.targetDirection == new Vector3(0f, 0f, 1f))
			{
				fireworksWeapon2.Place(human.ragdoll.partRightHand.transform.get_position(), Quaternion.LookRotation(-human.ragdoll.partRightHand.transform.get_up()));
			}
			else
			{
				fireworksWeapon2.Place(human.ragdoll.partRightHand.transform.get_position(), Quaternion.LookRotation(human.targetDirection));
			}
			if (fire)
			{
				bool rightGrab = human.controls.rightGrab;
				bool shootingFirework = human.controls.shootingFirework;
				human.controls.rightExtend = (rightGrab ? 1f : ((human.state == HumanState.Climb || human.state == HumanState.Idle || human.state == HumanState.Jump || human.state == HumanState.Slide || human.state == HumanState.Walk) ? 0.95f : 0f));
				human.controls.rightGrab = rightGrab;
				if (!rightGrab)
				{
					human.ragdoll.partRightHand.sensor.ReleaseGrab(Time.get_fixedDeltaTime() * 3f);
				}
				if (shootingFirework && fireworksWeapon2.CanShoot())
				{
					fireworksWeapon2.Shoot();
				}
			}
		}
	}

	private void LateUpdate()
	{
		SyncWeapons(fire: false);
	}

	public void ShootFirework()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		FireworksProjectile projectile = instance.GetProjectile();
		if ((Object)(object)projectile != (Object)null)
		{
			projectile.Shoot(((Component)this).get_transform().get_position(), 2f * Vector3.get_up() + Random.get_insideUnitSphere());
		}
	}
}
