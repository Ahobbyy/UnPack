using HumanAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationPresetMenuItem : ListViewItem
{
	public GameObject normalTemplate;

	public GameObject newSlotTemplate;

	public GameObject label;

	public TextMeshProUGUI newSlotLabel;

	public RawImage image;

	public GameObject activeMarker;

	private RagdollPresetMetadata boundData;

	private bool labelIsUIText;

	private Component labelComponent;

	private void OnEnable()
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

	public override void Bind(int index, object data)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		base.Bind(index, data);
		RagdollPresetMetadata ragdollPresetMetadata = data as RagdollPresetMetadata;
		if (ragdollPresetMetadata != null)
		{
			if ((Object)(object)labelComponent == (Object)null)
			{
				OnEnable();
			}
			if (labelIsUIText)
			{
				((Text)labelComponent).set_text(ragdollPresetMetadata.title);
			}
			else
			{
				((TextMeshProUGUI)(object)labelComponent).text = ragdollPresetMetadata.title;
			}
			image.set_texture((Texture)(object)ragdollPresetMetadata.thumbnailTexture);
		}
		normalTemplate.SetActive(ragdollPresetMetadata != null);
		newSlotTemplate.SetActive(ragdollPresetMetadata == null);
		MenuButton component = ((Component)this).GetComponent<MenuButton>();
		if (ragdollPresetMetadata == null)
		{
			component.SetLabel(newSlotLabel);
		}
		else if (labelIsUIText)
		{
			component.SetLabel((Text)labelComponent);
		}
		else
		{
			component.SetLabel((TextMeshProUGUI)(object)labelComponent);
		}
		if (boundData != null)
		{
			boundData.ReleaseThumbnailReference();
		}
		boundData = ragdollPresetMetadata;
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
