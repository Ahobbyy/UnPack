using UnityEngine;

public class ChangeColour : MonoBehaviour
{
	[Tooltip("The material that needs it colour changed.")]
	public Material materialToChange;

	public Color newColor;

	private Material materialInstance;

	private Color originalColor;

	private void Start()
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)materialToChange != (Object)null))
		{
			return;
		}
		Material[] materials = ((Renderer)((Component)this).GetComponent<MeshRenderer>()).get_materials();
		for (int i = 0; i < materials.Length; i++)
		{
			if (((Object)materials[i]).get_name().Replace(" (Instance)", "") == ((Object)materialToChange).get_name())
			{
				materialInstance = materials[i];
				originalColor = materialInstance.get_color();
			}
		}
	}

	public void ChangeColourMat()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)materialInstance != (Object)null)
		{
			materialInstance.set_color(newColor);
		}
	}

	public void Reset()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)materialInstance != (Object)null)
		{
			materialInstance.set_color(originalColor);
		}
	}

	public ChangeColour()
		: this()
	{
	}
}
