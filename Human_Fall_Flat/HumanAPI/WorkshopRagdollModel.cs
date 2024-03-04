using System;
using UnityEngine;

namespace HumanAPI
{
	[Serializable]
	[CreateAssetMenu(fileName = "RagdollModel", menuName = "RagdollModel")]
	public class WorkshopRagdollModel : ScriptableObject
	{
		public ulong workshopId;

		public RagdollModel model;

		public Texture2D thumbnail;

		public string title;

		[Multiline]
		public string description;

		[Multiline]
		public string updateNotes;

		public WorkshopItemMetadata meta => new RagdollModelMetadata
		{
			folder = "builtin:" + ((Object)this).get_name(),
			itemType = model.ragdollPart,
			title = title,
			description = description,
			cachedThumbnail = thumbnail,
			modelPrefab = model
		};

		public WorkshopRagdollModel()
			: this()
		{
		}
	}
}
