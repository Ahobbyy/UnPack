using HumanAPI;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationSelectModelMenuItem : ListViewItem
{
	public TextMeshProUGUI label;

	public RawImage image;

	public GameObject activeMarker;

	private WorkshopItemMetadata boundData;

	public override void Bind(int index, object data)
	{
		base.Bind(index, data);
		WorkshopItemMetadata workshopItemMetadata = data as WorkshopItemMetadata;
		if (workshopItemMetadata.isModel)
		{
			string name = ((Object)(data as RagdollModelMetadata).modelPrefab).get_name();
			label.text = LocalizationManager.GetTermTranslation("SKINS/" + name);
		}
		else
		{
			label.text = workshopItemMetadata.title;
		}
		image.set_texture((Texture)(object)workshopItemMetadata.thumbnailTexture);
		if (boundData != null)
		{
			boundData.ReleaseThumbnailReference();
		}
		boundData = workshopItemMetadata;
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
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_localScale(active ? (Vector3.get_one() * 1.05f) : Vector3.get_one());
		if ((Object)(object)activeMarker != (Object)null)
		{
			activeMarker.SetActive(active);
		}
		((Component)this).GetComponent<MenuButton>().isOn = active;
	}
}
