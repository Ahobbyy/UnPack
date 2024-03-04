using HumanAPI;
using UnityEngine;

public class FootCollisionAudioSensor : CollisionAudioSensor
{
	private Human human;

	protected override void OnEnable()
	{
		base.OnEnable();
		human = ((Component)this).GetComponentInParent<Human>();
	}

	protected override bool ReportCollision(SurfaceType surf1, SurfaceType surf2, Vector3 pos, float impulse, float normalVelocity, float tangentVelocity, float volume, float pitch)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)human == (Object)null)
		{
			return false;
		}
		float num = human.controls.walkSpeed * 2f + 0.5f;
		Vector3 val = human.velocity.ZeroY();
		float value = Mathf.Min(num, ((Vector3)(ref val)).get_magnitude());
		pitch = ((surf2 != SurfaceType.Gravel) ? (pitch * (Map(value, 0f, 2f, 0.9f, 1f) * Random.Range(0.95f, 1.1f))) : (pitch * (Map(value, 0f, 2f, 0.5f, 1f) * Random.Range(0.95f, 1.1f))));
		volume *= Random.Range(0.9f, 1.1f);
		return base.ReportCollision(surf1, surf2, pos, impulse, normalVelocity, tangentVelocity, volume, pitch);
	}

	public static float Map(float value, float sourceFrom, float sourceTo, float targetFrom, float targetTo)
	{
		if (value < sourceFrom)
		{
			value = 0f;
		}
		return Mathf.Lerp(targetFrom, targetTo, Mathf.InverseLerp(sourceFrom, sourceTo, value));
	}
}
