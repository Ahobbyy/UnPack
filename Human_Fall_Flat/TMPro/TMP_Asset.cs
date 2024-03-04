using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_Asset : ScriptableObject
	{
		public int hashCode;

		public Material material;

		public int materialHashCode;

		public TMP_Asset()
			: this()
		{
		}
	}
}
