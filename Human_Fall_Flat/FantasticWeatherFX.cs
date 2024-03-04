using HumanAPI;
using Multiplayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ParticleSystem))]
public class FantasticWeatherFX : MonoBehaviour, IBeginLevel
{
	[SerializeField]
	private float speed = 5f;

	[SerializeField]
	private int frameSkip = 10;

	[SerializeField]
	private float minEmit;

	[SerializeField]
	private float maxEmit;

	[SerializeField]
	private float noise;

	[SerializeField]
	private Vector3 windDirection;

	[SerializeField]
	private float windStrength;

	[SerializeField]
	private NetPlayer player;

	private ParticleSystem pFX;

	private bool process;

	private bool inited;

	private bool hadWeather;

	private int frame;

	private void DisableWeather()
	{
		process = false;
		pFX.Stop();
		SceneManager.remove_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
	}

	private void EnableWeather()
	{
		process = true;
		pFX.Play();
		SceneManager.remove_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
		SceneManager.add_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
	}

	private bool LevelHasWeather()
	{
		Level level = Object.FindObjectOfType<Level>();
		if ((Object)(object)level != (Object)null)
		{
			return level.HasWeather;
		}
		return false;
	}

	private void OnDestroy()
	{
		SceneManager.remove_sceneUnloaded((UnityAction<Scene>)OnSceneUnloaded);
	}

	private void Start()
	{
		pFX = ((Component)this).GetComponent<ParticleSystem>();
		if (!player.isLocalPlayer || !LevelHasWeather())
		{
			DisableWeather();
			return;
		}
		hadWeather = true;
		EnableWeather();
		inited = true;
	}

	private void OnSceneUnloaded(Scene scene)
	{
		if (hadWeather)
		{
			DisableWeather();
		}
	}

	private void FixedUpdate()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		if (!inited)
		{
			Start();
			inited = true;
		}
		if (process)
		{
			float num = 0f;
			if (frame == frameSkip)
			{
				frame = 0;
				RaycastHit val = default(RaycastHit);
				num = ((!Physics.Raycast(((Component)this).get_transform().get_position() + new Vector3(0f, 2f, 0f), Vector3.get_up(), ref val, float.PositiveInfinity)) ? maxEmit : minEmit);
			}
			else
			{
				frame++;
			}
			ParticleSystem obj = pFX;
			EmissionModule emission = pFX.get_emission();
			MinMaxCurve rateOverTime = ((EmissionModule)(ref emission)).get_rateOverTime();
			obj.set_emissionRate(Mathf.Lerp(((MinMaxCurve)(ref rateOverTime)).get_constantMax(), num, speed * Time.get_deltaTime()));
			NoiseModule val2 = pFX.get_noise();
			NoiseModule val3 = pFX.get_noise();
			float strengthMultiplier = Mathf.Lerp(((NoiseModule)(ref val3)).get_strengthMultiplier(), noise, speed * Time.get_deltaTime());
			((NoiseModule)(ref val2)).set_strengthMultiplier(strengthMultiplier);
			ForceOverLifetimeModule forceOverLifetime = pFX.get_forceOverLifetime();
			Vector3 val4 = Vector3.Normalize(windDirection) * windStrength;
			((ForceOverLifetimeModule)(ref forceOverLifetime)).set_x(MinMaxCurve.op_Implicit(val4.x));
			((ForceOverLifetimeModule)(ref forceOverLifetime)).set_y(MinMaxCurve.op_Implicit(val4.y));
			((ForceOverLifetimeModule)(ref forceOverLifetime)).set_z(MinMaxCurve.op_Implicit(val4.z));
			((Component)this).get_transform().set_rotation(Quaternion.get_identity());
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)((Component)other).GetComponent<FantasticWeatherFXTrigger>()))
		{
			FantasticWeatherFXTrigger component = ((Component)other).GetComponent<FantasticWeatherFXTrigger>();
			maxEmit = component.getEmit();
			noise = component.getNoise();
			windStrength = component.getWindStrength();
			windDirection = component.getWindDirection();
		}
	}

	void IBeginLevel.BeginLevel()
	{
		Start();
	}

	public FantasticWeatherFX()
		: this()
	{
	}
}
