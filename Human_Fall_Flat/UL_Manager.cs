using UnityEngine;

[AddComponentMenu("UPGEN Lighting/UPGEN Lighting Manager")]
[ExecuteInEditMode]
public sealed class UL_Manager : MonoBehaviour
{
	public static UL_Manager instance;

	public LayerMask layersToRayTrace = LayerMask.op_Implicit(-5);

	public bool showDebugRays;

	public bool showDebugGUI = true;

	private void OnEnable()
	{
		if ((Object)(object)instance == (Object)null)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning((object)"There are 2 audio UPGEN Lighting Managers in the scene. Please ensure there is always exactly one Manager in the scene.");
		}
	}

	private void OnDisable()
	{
		if ((Object)(object)instance == (Object)(object)this)
		{
			instance = null;
		}
	}

	private void OnGUI()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!showDebugGUI)
		{
			return;
		}
		GUILayout.BeginArea(new Rect(0f, 0f, 200f, (float)Screen.get_height()));
		if (UL_Renderer.HasLightsToRender)
		{
			int renderedLightsCount = UL_Renderer.RenderedLightsCount;
			if (renderedLightsCount > 0)
			{
				UL_GUI_Utils.Text($"Capacity: <b>{renderedLightsCount} / {UL_Renderer.MaxRenderingLightsCount}</b>");
			}
			renderedLightsCount = UL_FastLight.all.Count;
			if (renderedLightsCount > 0)
			{
				UL_GUI_Utils.Text($"Fast Lights: <b>{renderedLightsCount}</b>");
			}
			renderedLightsCount = UL_FastGI.all.Count;
			if (renderedLightsCount > 0)
			{
				UL_GUI_Utils.Text($"Fast GI: <b>{renderedLightsCount}</b>");
			}
			renderedLightsCount = UL_RayTracedGI.all.Count;
			if (renderedLightsCount > 0)
			{
				UL_GUI_Utils.Text($"RayTraced GI: <b>{renderedLightsCount}</b>");
			}
		}
		GUILayout.EndArea();
	}

	public UL_Manager()
		: this()
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)

}
