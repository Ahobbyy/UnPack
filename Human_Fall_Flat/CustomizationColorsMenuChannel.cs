using HumanAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationColorsMenuChannel : MonoBehaviour
{
	public TextMeshProUGUI text;

	public GameObject activeMarker;

	public WorkshopItemType part;

	public int number;

	public string label;

	private ColorBlock defaultColors;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		defaultColors = ((Selectable)((Component)this).GetComponent<Button>()).get_colors();
	}

	internal void Bind(WorkshopItemType part, int number, string label)
	{
		this.part = part;
		this.number = number;
		this.label = label;
		text.text = label;
	}

	public bool GetActive()
	{
		return activeMarker.get_activeSelf();
	}

	public void SetActive(bool active)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_localScale(active ? (Vector3.get_one() * 1.05f) : Vector3.get_one());
		activeMarker.SetActive(active);
		((Component)this).GetComponent<MenuButton>().isOn = active;
	}

	public CustomizationColorsMenuChannel()
		: this()
	{
	}
}
