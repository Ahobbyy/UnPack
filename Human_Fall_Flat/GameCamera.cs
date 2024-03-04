using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GameCamera : MonoBehaviour
{
	[NonSerialized]
	public Camera gameCam;

	private PostProcessLayer pp_layer;

	private PostProcessLayer.Antialiasing previousSetting;

	private void Awake()
	{
		gameCam = ((Component)this).GetComponent<Camera>();
		pp_layer = ((Component)gameCam).GetComponent<PostProcessLayer>();
		int num = 24;
		float num2 = 60f;
		float[] array = new float[32];
		array[num] = num2;
		gameCam.set_layerCullDistances(array);
	}

	private void NameTagCallback(bool active)
	{
		if (!((Object)(object)pp_layer == (Object)null))
		{
			if (active)
			{
				previousSetting = pp_layer.antialiasingMode;
				pp_layer.antialiasingMode = PostProcessLayer.Antialiasing.None;
			}
			else
			{
				pp_layer.antialiasingMode = previousSetting;
			}
		}
	}

	private void OnDisable()
	{
		NameTag.UnRegisterForNameTagCallbacks(NameTagCallback);
	}

	private void OnEnable()
	{
		NameTag.RegisterForNameTagCallbacks(NameTagCallback);
		if ((Object)(object)pp_layer != (Object)null && pp_layer.haveBundlesBeenInited)
		{
			pp_layer.ResetHistory();
		}
	}

	private void OnPreCull()
	{
		ApplyCameraOffset();
	}

	public void ApplyCameraOffset()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)pp_layer != (Object)null)
		{
			pp_layer.projOffset = MenuCameraEffects.instance.cameraCenter;
		}
		if (MenuCameraEffects.instance.cameraCenter != Vector2.get_zero())
		{
			OffsetCenter(MenuCameraEffects.instance.cameraCenter);
		}
		else
		{
			ResetCenterOffset();
		}
	}

	private void OffsetCenter(Vector2 center)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		gameCam.ResetProjectionMatrix();
		Matrix4x4 projectionMatrix = gameCam.get_projectionMatrix();
		((Matrix4x4)(ref projectionMatrix)).set_Item(0, 2, center.x);
		((Matrix4x4)(ref projectionMatrix)).set_Item(1, 2, center.y);
		gameCam.set_projectionMatrix(projectionMatrix);
		gameCam.set_nonJitteredProjectionMatrix(projectionMatrix);
	}

	public void ResetCenterOffset()
	{
		gameCam.ResetProjectionMatrix();
	}

	public GameCamera()
		: this()
	{
	}
}
