using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class Vector2Parameter : ParameterOverride<Vector2>
	{
		public override void Interp(Vector2 from, Vector2 to, float t)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			value.x = from.x + (to.x - from.x) * t;
			value.y = from.y + (to.y - from.y) * t;
		}

		public static implicit operator Vector3(Vector2Parameter prop)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Vector2.op_Implicit(prop.value);
		}

		public static implicit operator Vector4(Vector2Parameter prop)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Vector4.op_Implicit(prop.value);
		}
	}
}
