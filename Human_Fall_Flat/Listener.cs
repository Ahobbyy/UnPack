using System;
using System.Collections.Generic;
using UnityEngine;

public class Listener : MonoBehaviour
{
	public static Listener instance;

	private Transform originalParent;

	[NonSerialized]
	public List<Transform> earList = new List<Transform>();

	private bool transformOverride;

	private void Awake()
	{
		if ((Object)(object)originalParent == (Object)null)
		{
			originalParent = ((Component)this).get_transform().get_parent();
		}
	}

	private void OnEnable()
	{
		instance = this;
	}

	private void OnDisable()
	{
		Update();
	}

	private void OnDestroy()
	{
		if ((Object)(object)instance == (Object)(object)this)
		{
			instance = null;
		}
	}

	public void OverrideTransform(Transform t)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		transformOverride = true;
		((Component)this).get_transform().SetParent(t, false);
		((Component)this).get_transform().set_localPosition(Vector3.get_zero());
		((Component)this).get_transform().set_localRotation(Quaternion.get_identity());
	}

	public void EndTransfromOverride()
	{
		transformOverride = false;
		UpdateHierarchy();
	}

	public void AddListenTransform(Transform ears)
	{
		earList.Add(ears);
		UpdateHierarchy();
	}

	public void RemoveListenTransform(Transform ears)
	{
		earList.Remove(ears);
		UpdateHierarchy();
	}

	public void AddHuman(Human human)
	{
		AddListenTransform(human.ragdoll.partHead.transform);
	}

	public void RemoveHuman(Human human)
	{
		RemoveListenTransform(human.ragdoll.partHead.transform);
	}

	private void UpdateHierarchy()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		if (transformOverride)
		{
			return;
		}
		earList.Remove(null);
		if (earList.Count == 1)
		{
			if ((Object)(object)((Component)this).get_transform().get_parent() != (Object)(object)earList[0])
			{
				((Component)this).get_transform().SetParent(earList[0], false);
				((Component)this).get_transform().set_localPosition(Vector3.get_zero());
				((Component)this).get_transform().set_localRotation(Quaternion.get_identity());
			}
		}
		else if ((Object)(object)((Component)this).get_transform().get_parent() != (Object)(object)originalParent && (Object)(object)originalParent != (Object)null)
		{
			((Component)this).get_transform().SetParent(originalParent);
		}
	}

	private void Update()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		if (!transformOverride && earList.Count > 0)
		{
			Vector3 val = Vector3.get_zero();
			Quaternion val2 = Quaternion.get_identity();
			for (int i = 0; i < earList.Count; i++)
			{
				val = Vector3.Lerp(val, earList[i].get_position(), 1f / (float)(i + 1));
				val2 = Quaternion.Slerp(val2, earList[i].get_rotation(), 1f / (float)(i + 1));
			}
			((Component)this).get_transform().set_position(val);
			((Component)this).get_transform().set_rotation(val2);
		}
	}

	public Listener()
		: this()
	{
	}
}
