using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
	public class NetGraph : Graphic
	{
		private float[] values = new float[256];

		private int samplePos;

		private float range = 1f;

		public void PushValue(float value)
		{
			values[samplePos] = value;
			samplePos = (samplePos + 1) % values.Length;
			((Graphic)this).SetVerticesDirty();
		}

		public float GetMax()
		{
			float num = values[0];
			for (int i = 1; i < values.Length; i++)
			{
				if (num < values[i])
				{
					num = values[i];
				}
			}
			return num;
		}

		public void SetRange(float range)
		{
			this.range = range;
		}

		protected override void OnPopulateMesh(VertexHelper vh)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0259: Unknown result type (might be due to invalid IL or missing references)
			//IL_025e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0335: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0343: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034d: Unknown result type (might be due to invalid IL or missing references)
			//IL_035b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0375: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			vh.Clear();
			Rect rect = ((Graphic)this).get_rectTransform().get_rect();
			Vector2 size = ((Rect)(ref rect)).get_size();
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector((0f - ((Graphic)this).get_rectTransform().get_pivot().x) * size.x, (0f - ((Graphic)this).get_rectTransform().get_pivot().y) * size.y);
			UIVertex[] array = (UIVertex[])(object)new UIVertex[4]
			{
				UIVertex.simpleVert,
				UIVertex.simpleVert,
				UIVertex.simpleVert,
				UIVertex.simpleVert
			};
			array[0].color = new Color32((byte)0, (byte)0, (byte)0, (byte)64);
			array[0].position = Vector2.op_Implicit(new Vector2(0f * size.x + val.x, 0f * size.y + val.y));
			array[1].color = new Color32((byte)0, (byte)0, (byte)0, (byte)64);
			array[1].position = Vector2.op_Implicit(new Vector2(0f * size.x + val.x, 1f * size.y + val.y));
			array[2].color = new Color32((byte)0, (byte)0, (byte)0, (byte)64);
			array[2].position = Vector2.op_Implicit(new Vector2(1f * size.x + val.x, 1f * size.y + val.y));
			array[3].color = new Color32((byte)0, (byte)0, (byte)0, (byte)64);
			array[3].position = Vector2.op_Implicit(new Vector2(1f * size.x + val.x, 0f * size.y + val.y));
			vh.AddUIVertexQuad(array);
			for (int i = 0; i < values.Length - 1; i++)
			{
				float num = values[(i + samplePos) % values.Length] / range;
				float num2 = values[(i + samplePos + 1) % values.Length] / range;
				float num3 = 1f * (float)i / (float)values.Length;
				float num4 = 1f * (float)(i + 1) / (float)values.Length;
				array[0].color = Color32.op_Implicit(((Graphic)this).get_color());
				array[1].color = Color32.op_Implicit(((Graphic)this).get_color());
				array[2].color = Color32.op_Implicit(((Graphic)this).get_color());
				array[3].color = Color32.op_Implicit(((Graphic)this).get_color());
				array[0].position = Vector2.op_Implicit(new Vector2(num3 * size.x + val.x, 0f * size.y + val.y));
				array[1].position = Vector2.op_Implicit(new Vector2(num3 * size.x + val.x, num * size.y + val.y));
				array[2].position = Vector2.op_Implicit(new Vector2(num4 * size.x + val.x, num2 * size.y + val.y));
				array[3].position = Vector2.op_Implicit(new Vector2(num4 * size.x + val.x, 0f * size.y + val.y));
				vh.AddUIVertexQuad(array);
			}
		}

		public NetGraph()
			: this()
		{
		}
	}
}
