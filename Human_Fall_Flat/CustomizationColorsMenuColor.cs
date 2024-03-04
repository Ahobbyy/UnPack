using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationColorsMenuColor : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public Graphic swatch;

	public GameObject activeMarker;

	public Color color => swatch.get_color();

	public void SetActive(bool active)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		activeMarker.SetActive(active);
		if (active && color.sqrMagnitude() > 2.5f)
		{
			activeMarker.GetComponent<Graphic>().set_color(Color.get_black());
		}
	}

	internal void Bind(Color color)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		swatch.set_color(color);
		SetActive(active: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		EventSystem.get_current().SetSelectedGameObject(((Component)this).get_gameObject());
	}

	public CustomizationColorsMenuColor()
		: this()
	{
	}
}
