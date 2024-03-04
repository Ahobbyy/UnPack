using HumanAPI;
using Multiplayer;
using UnityEngine;

public class SkinnedDoll : MonoBehaviour
{
	public Ragdoll ragdoll;

	public Transform skins;

	private RagdollCustomization customization;

	private Joint joint;

	public bool nailed => (Object)(object)joint != (Object)null;

	private void Start()
	{
		CollisionSensor[] componentsInChildren = ((Component)this).GetComponentsInChildren<CollisionSensor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.Destroy((Object)(object)componentsInChildren[i]);
		}
	}

	public void ApplySkin(string skin)
	{
		if (string.IsNullOrEmpty(skin))
		{
			((Component)ragdoll).get_gameObject().SetActive(false);
			return;
		}
		Transform val = skins.Find(skin.ToLowerInvariant());
		if ((Object)(object)val != (Object)null)
		{
			((Component)ragdoll).get_gameObject().SetActive(true);
			SkinnedDollPreset component = ((Component)val).GetComponent<SkinnedDollPreset>();
			RagdollPresetMetadata ragdollPresetMetadata = new RagdollPresetMetadata
			{
				folder = null,
				itemType = WorkshopItemType.RagdollPreset,
				main = new RagdollPresetPartMetadata
				{
					modelPath = (string.IsNullOrEmpty(component.full) ? "builtin:HumanDefaultBody" : ("builtin:" + component.full))
				},
				head = new RagdollPresetPartMetadata
				{
					modelPath = (string.IsNullOrEmpty(component.head) ? "builtin:HumanHardHat" : ("builtin:" + component.head))
				},
				upperBody = (string.IsNullOrEmpty(component.upper) ? null : new RagdollPresetPartMetadata
				{
					modelPath = "builtin:" + component.upper
				}),
				lowerBody = (string.IsNullOrEmpty(component.lower) ? null : new RagdollPresetPartMetadata
				{
					modelPath = "builtin:" + component.lower
				})
			};
			if (!string.IsNullOrEmpty(component.full) && string.IsNullOrEmpty(component.head))
			{
				ragdollPresetMetadata.head = null;
			}
			ApplyPreset(ragdollPresetMetadata, bake: false);
		}
		else
		{
			((Component)ragdoll).get_gameObject().SetActive(false);
		}
	}

	private void ApplyPreset(RagdollPresetMetadata preset, bool bake = true)
	{
		if ((Object)(object)customization == (Object)null)
		{
			customization = ((Component)ragdoll).get_gameObject().AddComponent<RagdollCustomization>();
		}
		customization.ApplyPreset(preset, forceRebuild: true);
		customization.RebindColors(bake, compress: true);
		RigClipVolume[] componentsInChildren = ((Component)this).GetComponentsInChildren<RigClipVolume>();
		if ((Object)(object)customization.main != (Object)null)
		{
			RagdollModelSkinnedMesh[] componentsInChildren2 = ((Component)customization.main).GetComponentsInChildren<RagdollModelSkinnedMesh>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].Clip(componentsInChildren);
			}
		}
	}

	public void ReapplySkin()
	{
		ApplyPreset(customization.preset);
	}

	public void Nail(bool nail)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (nail)
		{
			((Component)this).GetComponentInChildren<RespawnRoot>(true).Respawn(Vector3.get_zero());
			if ((Object)(object)joint == (Object)null)
			{
				joint = (Joint)(object)((Component)ragdoll.partLeftHand.transform).get_gameObject().AddComponent<FixedJoint>();
			}
		}
		else if ((Object)(object)joint != (Object)null)
		{
			Object.Destroy((Object)(object)joint);
		}
	}

	public SkinnedDoll()
		: this()
	{
	}
}
