using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightConsumeByColor : LightConsume
	{
		public NodeOutput r;

		public NodeOutput g;

		public NodeOutput b;

		public NodeOutput a;

		protected override void CheckOutput()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			base.CheckOutput();
			Color litColor = base.LitColor;
			if (litColor.r != r.value)
			{
				r.SetValue(litColor.r);
			}
			if (litColor.g != g.value)
			{
				g.SetValue(litColor.g);
			}
			if (litColor.b != b.value)
			{
				b.SetValue(litColor.b);
			}
			if (litColor.a != a.value)
			{
				a.SetValue(litColor.a);
			}
		}
	}
}
