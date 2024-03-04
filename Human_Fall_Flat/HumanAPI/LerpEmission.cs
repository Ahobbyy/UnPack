using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Lerp Emission", 10)]
	public class LerpEmission : LerpBase
	{
		[ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
		[Tooltip("Default Colour to Lerp from")]
		public Color from = Color.get_black();

		[ColorUsage(false, true, 0f, 8f, 0.125f, 3f)]
		[Tooltip("Colour to Lerp to")]
		public Color to = Color.get_white();

		[Tooltip("Element number to apply the lerp to")]
		public int elementNumber;

		[Tooltip("Whether or not to use the element number")]
		public bool useMaterialElementNumber;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private MeshRenderer meshRenderer;

		protected override void Awake()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Awake "));
			}
			meshRenderer = ((Component)this).GetComponent<MeshRenderer>();
			base.Awake();
		}

		protected override void ApplyValue(float value)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Applying value "));
			}
			Color val = Color.LerpUnclamped(from, to, value);
			if (useMaterialElementNumber)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Using the Element value " + elementNumber));
				}
				Material[] materials = ((Renderer)meshRenderer).get_materials();
				Material obj = materials[elementNumber];
				obj.SetColor("_EmissionColor", val);
				obj.EnableKeyword("_EMISSION");
				((Renderer)meshRenderer).set_materials(materials);
			}
			else
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Doing the normal stuff - not using element number "));
				}
				((Renderer)meshRenderer).get_material().SetColor("_EmissionColor", val);
				((Renderer)meshRenderer).get_material().EnableKeyword("_EMISSION");
			}
			DynamicGI.SetEmissive((Renderer)(object)meshRenderer, val);
		}
	}
}
