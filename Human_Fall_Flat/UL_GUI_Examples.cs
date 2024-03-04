using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class UL_GUI_Examples : MonoBehaviour
{
	public enum AnimationKind
	{
		AnimateX,
		AnimateZ
	}

	[Multiline]
	public string description;

	public string prevScene;

	public string nextScene;

	[Header("References")]
	public Transform target;

	public AnimationKind animationKind;

	public PostProcessProfile volumeProfile;

	private Vector3 _initialTargetPosition;

	private Vector3 _deltaTargetPosition;

	private void Start()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)volumeProfile) && volumeProfile.TryGetSettings<UPGEN_Lighting>(out var outSetting))
		{
			outSetting.intensity.value = 1f;
		}
		if (!((Object)(object)target == (Object)null))
		{
			_initialTargetPosition = ((Component)target).get_transform().get_position();
		}
	}

	private void LateUpdate()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target == (Object)null)
		{
			return;
		}
		if (Application.get_isPlaying())
		{
			switch (animationKind)
			{
			case AnimationKind.AnimateX:
				_deltaTargetPosition.x = 3f * Mathf.Sin(Time.get_unscaledTime());
				break;
			case AnimationKind.AnimateZ:
				_deltaTargetPosition.z = 7f * Mathf.Sin(Time.get_unscaledTime() * 0.5f);
				break;
			}
		}
		if (_initialTargetPosition == Vector3.get_zero())
		{
			_initialTargetPosition = ((Component)target).get_transform().get_position();
		}
		else
		{
			((Component)target).get_transform().set_position(_initialTargetPosition + _deltaTargetPosition);
		}
	}

	private void OnGUI()
	{
		OnGUI_Tools();
		OnGUI_Scene();
	}

	private void OnGUI_Tools()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginArea(new Rect(0f, 0f, 200f, (float)Screen.get_height()));
		GUILayout.FlexibleSpace();
		if (Object.op_Implicit((Object)(object)volumeProfile) && volumeProfile.TryGetSettings<UPGEN_Lighting>(out var outSetting))
		{
			float value = outSetting.intensity.value;
			float num = UL_GUI_Utils.Slider("Intensity", value, 0f, 2f);
			if (num != value)
			{
				outSetting.intensity.value = num;
			}
		}
		if (Object.op_Implicit((Object)(object)target) && Application.get_isPlaying())
		{
			if (animationKind != 0)
			{
				_deltaTargetPosition.x = UL_GUI_Utils.Slider("X", _deltaTargetPosition.x, -2f, 2f);
			}
			_deltaTargetPosition.y = UL_GUI_Utils.Slider("Y", _deltaTargetPosition.y, -2f, 2f);
			if (animationKind != AnimationKind.AnimateZ)
			{
				_deltaTargetPosition.z = UL_GUI_Utils.Slider("Z", _deltaTargetPosition.z, -3f, 3f);
			}
			UL_RayTracedGI component = ((Component)target).GetComponent<UL_RayTracedGI>();
			if (Object.op_Implicit((Object)(object)component))
			{
				component.raysMatrixSize = (int)UL_GUI_Utils.Slider("Rays", component.raysMatrixSize, 2f, 15f);
			}
		}
		GUILayout.EndArea();
	}

	private void OnGUI_Scene()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(description))
		{
			Scene activeScene = SceneManager.GetActiveScene();
			GUILayout.BeginArea(new Rect((float)(Screen.get_width() - 500) * 0.5f, (float)(Screen.get_height() - 164), 500f, 36f));
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(prevScene) && SceneManager.get_sceneCountInBuildSettings() > 1 && Application.get_isPlaying() && GUILayout.Button("<size=24><b>◄</b></size>", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(32f),
				GUILayout.Height(34f)
			}))
			{
				SceneManager.LoadScene(prevScene);
			}
			GUILayout.Label("<size=24><b>" + ((Scene)(ref activeScene)).get_name() + "</b></size>", GUI.get_skin().get_box(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (!string.IsNullOrEmpty(nextScene) && SceneManager.get_sceneCountInBuildSettings() > 1 && Application.get_isPlaying() && GUILayout.Button("<size=24><b>►</b></size>", (GUILayoutOption[])(object)new GUILayoutOption[2]
			{
				GUILayout.Width(32f),
				GUILayout.Height(34f)
			}))
			{
				SceneManager.LoadScene(nextScene);
			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
			GUI.Box(new Rect((float)(Screen.get_width() - 1200) * 0.5f, (float)(Screen.get_height() - 100), 1200f, 60f), "<size=20>" + description + "</size>");
		}
	}

	public UL_GUI_Examples()
		: this()
	{
	}
}
