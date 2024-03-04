using System.Collections;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class PaintTool : MonoBehaviour
{
	public Color color = Color.get_white();

	public float cursorKernel = 10f;

	public float cursorFalloff = 10f;

	public const int layer = 31;

	public const int layerMask = int.MinValue;

	public bool paintBackface;

	public bool hasChanges;

	private Dictionary<WorkshopItemType, int> mask;

	private List<RagdollModel> parts = new List<RagdollModel>();

	private Camera unprojectCamera;

	public Material unprojectMaterial;

	private Camera myCam;

	private RagdollTexture texture;

	private RagdollCustomization customization;

	public bool inStroke;

	public void Initialize(RagdollCustomization customization)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		this.customization = customization;
		unprojectMaterial = new Material(Shaders.instance.paintShader);
		unprojectCamera = CustomizationController.instance.mainCamera;
		GameObject val = new GameObject();
		val.get_transform().SetParent(((Component)this).get_transform(), false);
		myCam = val.AddComponent<Camera>();
		((Behaviour)myCam).set_enabled(false);
		myCam.set_clearFlags((CameraClearFlags)4);
		myCam.set_cullingMask(int.MinValue);
		myCam.set_renderingPath((RenderingPath)1);
		mask = new Dictionary<WorkshopItemType, int>
		{
			{
				WorkshopItemType.ModelFull,
				7
			},
			{
				WorkshopItemType.ModelHead,
				7
			},
			{
				WorkshopItemType.ModelUpperBody,
				7
			},
			{
				WorkshopItemType.ModelLowerBody,
				7
			}
		};
		parts.Clear();
		if ((Object)(object)customization.main != (Object)null && customization.main.texture.paintingEnabled)
		{
			parts.Add(customization.main);
		}
		if ((Object)(object)customization.head != (Object)null && customization.head.texture.paintingEnabled)
		{
			parts.Add(customization.head);
		}
		if ((Object)(object)customization.upper != (Object)null && customization.upper.texture.paintingEnabled)
		{
			parts.Add(customization.upper);
		}
		if ((Object)(object)customization.lower != (Object)null && customization.lower.texture.paintingEnabled)
		{
			parts.Add(customization.lower);
		}
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].texture.EnterPaint();
		}
		((MonoBehaviour)this).StartCoroutine(EnsurePaddedMeshes());
	}

	public void SetMask(WorkshopItemType part, int number, bool on)
	{
		int num = 1 << number - 1;
		mask[part] = (mask[part] & ~num) | (on ? num : 0);
		RagdollModel model = customization.GetModel(part);
		if ((Object)(object)model != (Object)null)
		{
			model.SetMask(mask[part]);
		}
	}

	public int GetMask(WorkshopItemType part)
	{
		return mask[part];
	}

	public bool GetMask(WorkshopItemType part, int number)
	{
		int num = 1 << number - 1;
		return (mask[part] & num) != 0;
	}

	private IEnumerator EnsurePaddedMeshes()
	{
		yield return (object)new WaitForSeconds(0.3f);
		for (int i = 0; i < parts.Count; i++)
		{
			yield return null;
			_ = parts[i].padded;
		}
	}

	public void Commit()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].texture.SaveToPreset();
		}
		hasChanges = false;
	}

	public void Teardown()
	{
		Object.Destroy((Object)(object)((Component)myCam).get_gameObject());
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].texture.LeavePaint();
		}
		hasChanges = false;
	}

	public void Paint()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		unprojectMaterial.set_color(color);
		unprojectMaterial.SetVector("_PointPos", new Vector4(Input.get_mousePosition().x, Input.get_mousePosition().y, 0f, 0f));
		unprojectMaterial.SetFloat("_PointSize", cursorKernel);
		unprojectMaterial.SetFloat("_FalloffSize", cursorFalloff);
		unprojectMaterial.SetVector("_ScreenSize", new Vector4((float)Screen.get_width(), (float)Screen.get_height(), 0f, 0f));
		unprojectMaterial.SetFloat("_PaintBackface", (float)(paintBackface ? 1 : 0));
		Render();
	}

	public void Project(Texture texture, float mirror)
	{
		unprojectMaterial.SetTexture("_ProjectTex", texture);
		unprojectMaterial.SetFloat("_MirrorMultiplier", mirror * (float)Screen.get_width() / (float)Screen.get_height() * (float)texture.get_height() / (float)texture.get_width());
		unprojectMaterial.SetFloat("_PaintBackface", (float)(paintBackface ? 1 : 0));
		Render();
	}

	private void Render()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		hasChanges = true;
		if (!inStroke)
		{
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].texture.BeginStroke();
			}
			inStroke = true;
		}
		((Component)myCam).get_transform().set_position(((Component)unprojectCamera).get_transform().get_position());
		((Component)myCam).get_transform().set_rotation(((Component)unprojectCamera).get_transform().get_rotation());
		myCam.set_nearClipPlane(unprojectCamera.get_nearClipPlane());
		myCam.set_farClipPlane(unprojectCamera.get_farClipPlane());
		myCam.set_fieldOfView(unprojectCamera.get_fieldOfView());
		myCam.set_aspect(unprojectCamera.get_aspect());
		Matrix4x4 projectionMatrix = unprojectCamera.get_projectionMatrix();
		((Matrix4x4)(ref projectionMatrix)).set_Item(0, 2, MenuCameraEffects.instance.cameraCenter.x);
		((Matrix4x4)(ref projectionMatrix)).set_Item(1, 2, MenuCameraEffects.instance.cameraCenter.y);
		myCam.set_projectionMatrix(projectionMatrix);
		for (int j = 0; j < parts.Count; j++)
		{
			RagdollModel ragdollModel = parts[j];
			ragdollModel.texture.PaintStep(myCam, unprojectMaterial, mask[ragdollModel.ragdollPart]);
			ragdollModel.padded.Enable(enable: true);
			ragdollModel.padded.SetMaterial(unprojectMaterial);
			myCam.Render();
			myCam.set_targetTexture((RenderTexture)null);
			ragdollModel.padded.Enable(enable: false);
		}
	}

	public void CancelStroke()
	{
		if (inStroke)
		{
			inStroke = false;
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].texture.CancelStroke();
			}
		}
	}

	public void EndStroke()
	{
		if (inStroke)
		{
			inStroke = false;
			for (int i = 0; i < parts.Count; i++)
			{
				parts[i].texture.EndStroke();
			}
		}
	}

	public void Undo()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].texture.Undo();
		}
		hasChanges = true;
	}

	public void Redo()
	{
		for (int i = 0; i < parts.Count; i++)
		{
			parts[i].texture.Redo();
		}
		hasChanges = true;
	}

	public void BeginStream()
	{
		unprojectMaterial.set_shader(Shaders.instance.webcamShader);
	}

	public void StopStream()
	{
		unprojectMaterial.set_shader(Shaders.instance.paintShader);
	}

	public PaintTool()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}
