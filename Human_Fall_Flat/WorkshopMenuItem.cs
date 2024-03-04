using HumanAPI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorkshopMenuItem : ListViewItem
{
	public GameObject label;

	public RawImage image;

	public WorkshopLevelMetadata level;

	public int campaignLevelID = -1;

	public Texture2D noThumbnailImage;

	public GameObject activeMarker;

	public GameObject newLevelStar;

	public WorkshopLevelMetadata boundData;

	private bool labelIsUIText;

	private Component labelComponent;

	private void OnEnable()
	{
		if (!((Object)(object)label == (Object)null))
		{
			labelComponent = (Component)(object)label.GetComponentInChildren<Text>();
			if ((Object)(object)labelComponent != (Object)null)
			{
				labelIsUIText = true;
			}
			else
			{
				labelComponent = (Component)(object)label.GetComponentInChildren<TextMeshProUGUI>();
			}
		}
	}

	public override void Bind(int index, object data)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		base.Bind(index, data);
		level = (WorkshopLevelMetadata)data;
		if (level == null)
		{
			((Component)this).get_gameObject().SetActive(false);
		}
		else
		{
			((Component)this).get_gameObject().SetActive(true);
			if ((Object)(object)label != (Object)null)
			{
				((object)label).GetType();
				if (labelIsUIText)
				{
					if ((Object)(object)labelComponent != (Object)null)
					{
						((Text)labelComponent).set_text(level.title);
					}
				}
				else if ((Object)(object)labelComponent != (Object)null)
				{
					((TextMeshProUGUI)(object)labelComponent).text = level.title;
				}
				if ((Object)(object)image != (Object)null && level != null)
				{
					image.set_texture((Texture)(object)level.thumbnailTexture);
				}
			}
			if ((Object)(object)newLevelStar != (Object)null)
			{
				bool active = false;
				if (!GameSave.HasSeenLatestLevel() && level.levelType == WorkshopItemSource.EditorPick && level.workshopId == 3)
				{
					active = true;
				}
				newLevelStar.SetActive(active);
			}
		}
		if (boundData != null)
		{
			boundData.ReleaseThumbnailReference();
		}
		boundData = level;
	}

	public void OnDestroy()
	{
		if (boundData != null)
		{
			boundData.ReleaseThumbnailReference();
		}
		boundData = null;
	}

	public void SetActive(bool active)
	{
		if ((Object)(object)activeMarker != (Object)null)
		{
			activeMarker.SetActive(active);
		}
		((Component)this).GetComponent<MenuButton>().isOn = active;
	}

	public override void OnPointerEnter(PointerEventData pointerEventData)
	{
		base.OnPointerEnter(pointerEventData);
	}
}
