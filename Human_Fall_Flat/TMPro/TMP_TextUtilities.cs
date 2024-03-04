using UnityEngine;

namespace TMPro
{
	public static class TMP_TextUtilities
	{
		private struct LineSegment
		{
			public Vector3 Point1;

			public Vector3 Point2;

			public LineSegment(Vector3 p1, Vector3 p2)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				Point1 = p1;
				Point2 = p2;
			}
		}

		private static Vector3[] m_rectWorldCorners = (Vector3[])(object)new Vector3[4];

		private const string k_lookupStringL = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-";

		private const string k_lookupStringU = "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-";

		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			int num = FindNearestCharacter(textComponent, position, camera, visibleOnly: false);
			RectTransform rectTransform = textComponent.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			TMP_CharacterInfo tMP_CharacterInfo = textComponent.textInfo.characterInfo[num];
			Vector3 val = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.bottomLeft);
			Vector3 val2 = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.topRight);
			if ((position.x - val.x) / (val2.x - val.x) < 0.5f)
			{
				return num;
			}
			return num + 1;
		}

		public static int GetCursorIndexFromPosition(TMP_Text textComponent, Vector3 position, Camera camera, out CaretPosition cursor)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			int num = FindNearestLine(textComponent, position, camera);
			int num2 = FindNearestCharacterOnLine(textComponent, position, num, camera, visibleOnly: false);
			if (textComponent.textInfo.lineInfo[num].characterCount == 1)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			RectTransform rectTransform = textComponent.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			TMP_CharacterInfo tMP_CharacterInfo = textComponent.textInfo.characterInfo[num2];
			Vector3 val = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.bottomLeft);
			Vector3 val2 = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.topRight);
			if ((position.x - val.x) / (val2.x - val.x) < 0.5f)
			{
				cursor = CaretPosition.Left;
				return num2;
			}
			cursor = CaretPosition.Right;
			return num2;
		}

		public static int FindNearestLine(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = -1;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tMP_LineInfo = text.textInfo.lineInfo[i];
				float y = ((Transform)rectTransform).TransformPoint(new Vector3(0f, tMP_LineInfo.ascender, 0f)).y;
				float y2 = ((Transform)rectTransform).TransformPoint(new Vector3(0f, tMP_LineInfo.descender, 0f)).y;
				if (y > position.y && y2 < position.y)
				{
					return i;
				}
				float num2 = Mathf.Abs(y - position.y);
				float num3 = Mathf.Abs(y2 - position.y);
				float num4 = Mathf.Min(num2, num3);
				if (num4 < num)
				{
					num = num4;
					result = i;
				}
			}
			return result;
		}

		public static int FindNearestCharacterOnLine(TMP_Text text, Vector3 position, int line, Camera camera, bool visibleOnly)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			int firstCharacterIndex = text.textInfo.lineInfo[line].firstCharacterIndex;
			int lastCharacterIndex = text.textInfo.lineInfo[line].lastCharacterIndex;
			float num = float.PositiveInfinity;
			int result = lastCharacterIndex;
			for (int i = firstCharacterIndex; i < lastCharacterIndex; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tMP_CharacterInfo.isVisible)
				{
					Vector3 val = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.bottomLeft);
					Vector3 val2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
					Vector3 val3 = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.topRight);
					Vector3 val4 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
					if (PointIntersectRectangle(position, val, val2, val3, val4))
					{
						result = i;
						break;
					}
					float num2 = DistanceToLine(val, val2, position);
					float num3 = DistanceToLine(val2, val3, position);
					float num4 = DistanceToLine(val3, val4, position);
					float num5 = DistanceToLine(val4, val, position);
					float num6 = ((num2 < num3) ? num2 : num3);
					num6 = ((num6 < num4) ? num6 : num4);
					num6 = ((num6 < num5) ? num6 : num5);
					if (num > num6)
					{
						num = num6;
						result = i;
					}
				}
			}
			return result;
		}

		public static bool IsIntersectingRectTransform(RectTransform rectTransform, Vector3 position, Camera camera)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			rectTransform.GetWorldCorners(m_rectWorldCorners);
			if (PointIntersectRectangle(position, m_rectWorldCorners[0], m_rectWorldCorners[1], m_rectWorldCorners[2], m_rectWorldCorners[3]))
			{
				return true;
			}
			return false;
		}

		public static int FindIntersectingCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tMP_CharacterInfo.isVisible)
				{
					Vector3 a = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.bottomLeft);
					Vector3 b = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
					Vector3 c = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.topRight);
					Vector3 d = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
					if (PointIntersectRectangle(position, a, b, c, d))
					{
						return i;
					}
				}
			}
			return -1;
		}

		public static int FindNearestCharacter(TMP_Text text, Vector3 position, Camera camera, bool visibleOnly)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.characterCount; i++)
			{
				TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[i];
				if (!visibleOnly || tMP_CharacterInfo.isVisible)
				{
					Vector3 val = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.bottomLeft);
					Vector3 val2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.topRight.y, 0f));
					Vector3 val3 = ((Transform)rectTransform).TransformPoint(tMP_CharacterInfo.topRight);
					Vector3 val4 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.bottomLeft.y, 0f));
					if (PointIntersectRectangle(position, val, val2, val3, val4))
					{
						return i;
					}
					float num2 = DistanceToLine(val, val2, position);
					float num3 = DistanceToLine(val2, val3, position);
					float num4 = DistanceToLine(val3, val4, position);
					float num5 = DistanceToLine(val4, val, position);
					float num6 = ((num2 < num3) ? num2 : num3);
					num6 = ((num6 < num4) ? num6 : num4);
					num6 = ((num6 < num5) ? num6 : num5);
					if (num > num6)
					{
						num = num6;
						result = i;
					}
				}
			}
			return result;
		}

		public static int FindIntersectingWord(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0254: Unknown result type (might be due to invalid IL or missing references)
			//IL_0257: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0293: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0351: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_0386: Unknown result type (might be due to invalid IL or missing references)
			//IL_0388: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tMP_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 val = Vector3.get_zero();
				Vector3 val2 = Vector3.get_zero();
				Vector3 val3 = Vector3.get_zero();
				Vector3 val4 = Vector3.get_zero();
				float num = float.NegativeInfinity;
				float num2 = float.PositiveInfinity;
				for (int j = 0; j < tMP_WordInfo.characterCount; j++)
				{
					int num3 = tMP_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num3];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					bool isVisible = tMP_CharacterInfo.isVisible;
					num = Mathf.Max(num, tMP_CharacterInfo.ascender);
					num2 = Mathf.Min(num2, tMP_CharacterInfo.descender);
					if (!flag && isVisible)
					{
						flag = true;
						((Vector3)(ref val))._002Ector(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f);
						((Vector3)(ref val2))._002Ector(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f);
						if (tMP_WordInfo.characterCount == 1)
						{
							flag = false;
							((Vector3)(ref val3))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
							((Vector3)(ref val4))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
							val = ((Transform)rectTransform).TransformPoint(new Vector3(val.x, num2, 0f));
							val2 = ((Transform)rectTransform).TransformPoint(new Vector3(val2.x, num, 0f));
							val4 = ((Transform)rectTransform).TransformPoint(new Vector3(val4.x, num, 0f));
							val3 = ((Transform)rectTransform).TransformPoint(new Vector3(val3.x, num2, 0f));
							if (PointIntersectRectangle(position, val, val2, val4, val3))
							{
								return i;
							}
						}
					}
					if (flag && j == tMP_WordInfo.characterCount - 1)
					{
						flag = false;
						((Vector3)(ref val3))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
						((Vector3)(ref val4))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
						val = ((Transform)rectTransform).TransformPoint(new Vector3(val.x, num2, 0f));
						val2 = ((Transform)rectTransform).TransformPoint(new Vector3(val2.x, num, 0f));
						val4 = ((Transform)rectTransform).TransformPoint(new Vector3(val4.x, num, 0f));
						val3 = ((Transform)rectTransform).TransformPoint(new Vector3(val3.x, num2, 0f));
						if (PointIntersectRectangle(position, val, val2, val4, val3))
						{
							return i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num3 + 1].lineNumber)
					{
						flag = false;
						((Vector3)(ref val3))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f);
						((Vector3)(ref val4))._002Ector(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f);
						val = ((Transform)rectTransform).TransformPoint(new Vector3(val.x, num2, 0f));
						val2 = ((Transform)rectTransform).TransformPoint(new Vector3(val2.x, num, 0f));
						val4 = ((Transform)rectTransform).TransformPoint(new Vector3(val4.x, num, 0f));
						val3 = ((Transform)rectTransform).TransformPoint(new Vector3(val3.x, num2, 0f));
						num = float.NegativeInfinity;
						num2 = float.PositiveInfinity;
						if (PointIntersectRectangle(position, val, val2, val4, val3))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		public static int FindNearestWord(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0158: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_030b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0310: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			//IL_0313: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_0317: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_0328: Unknown result type (might be due to invalid IL or missing references)
			//IL_0330: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0348: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			float num = float.PositiveInfinity;
			int result = 0;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.wordCount; i++)
			{
				TMP_WordInfo tMP_WordInfo = text.textInfo.wordInfo[i];
				bool flag = false;
				Vector3 val = Vector3.get_zero();
				Vector3 val2 = Vector3.get_zero();
				Vector3 zero = Vector3.get_zero();
				Vector3 zero2 = Vector3.get_zero();
				for (int j = 0; j < tMP_WordInfo.characterCount; j++)
				{
					int num2 = tMP_WordInfo.firstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					bool isVisible = tMP_CharacterInfo.isVisible;
					if (!flag && isVisible)
					{
						flag = true;
						val = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						val2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_WordInfo.characterCount == 1)
						{
							flag = false;
							zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, val, val2, zero2, zero))
							{
								return i;
							}
							float num3 = DistanceToLine(val, val2, position);
							float num4 = DistanceToLine(val2, zero2, position);
							float num5 = DistanceToLine(zero2, zero, position);
							float num6 = DistanceToLine(zero, val, position);
							float num7 = ((num3 < num4) ? num3 : num4);
							num7 = ((num7 < num5) ? num7 : num5);
							num7 = ((num7 < num6) ? num7 : num6);
							if (num > num7)
							{
								num = num7;
								result = i;
							}
						}
					}
					if (flag && j == tMP_WordInfo.characterCount - 1)
					{
						flag = false;
						zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, val, val2, zero2, zero))
						{
							return i;
						}
						float num8 = DistanceToLine(val, val2, position);
						float num9 = DistanceToLine(val2, zero2, position);
						float num10 = DistanceToLine(zero2, zero, position);
						float num11 = DistanceToLine(zero, val, position);
						float num12 = ((num8 < num9) ? num8 : num9);
						num12 = ((num12 < num10) ? num12 : num10);
						num12 = ((num12 < num11) ? num12 : num11);
						if (num > num12)
						{
							num = num12;
							result = i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, val, val2, zero2, zero))
						{
							return i;
						}
						float num13 = DistanceToLine(val, val2, position);
						float num14 = DistanceToLine(val2, zero2, position);
						float num15 = DistanceToLine(zero2, zero, position);
						float num16 = DistanceToLine(zero, val, position);
						float num17 = ((num13 < num14) ? num13 : num14);
						num17 = ((num17 < num15) ? num17 : num15);
						num17 = ((num17 < num16) ? num17 : num16);
						if (num > num17)
						{
							num = num17;
							result = i;
						}
					}
				}
			}
			return result;
		}

		public static int FindIntersectingLine(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			int result = -1;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.lineCount; i++)
			{
				TMP_LineInfo tMP_LineInfo = text.textInfo.lineInfo[i];
				float y = ((Transform)rectTransform).TransformPoint(new Vector3(0f, tMP_LineInfo.ascender, 0f)).y;
				float y2 = ((Transform)rectTransform).TransformPoint(new Vector3(0f, tMP_LineInfo.descender, 0f)).y;
				if (y > position.y && y2 < position.y)
				{
					return i;
				}
			}
			return result;
		}

		public static int FindIntersectingLink(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			Transform transform = text.transform;
			ScreenPointToWorldPointInRectangle(transform, Vector2.op_Implicit(position), camera, out position);
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tMP_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 a = Vector3.get_zero();
				Vector3 b = Vector3.get_zero();
				Vector3 zero = Vector3.get_zero();
				Vector3 zero2 = Vector3.get_zero();
				for (int j = 0; j < tMP_LinkInfo.linkTextLength; j++)
				{
					int num = tMP_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					if (text.overflowMode == TextOverflowModes.Page && tMP_CharacterInfo.pageNumber + 1 != text.pageToDisplay)
					{
						continue;
					}
					if (!flag)
					{
						flag = true;
						a = transform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						b = transform.TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, a, b, zero2, zero))
							{
								return i;
							}
						}
					}
					if (flag && j == tMP_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, a, b, zero2, zero))
						{
							return i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num + 1].lineNumber)
					{
						flag = false;
						zero = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = transform.TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, a, b, zero2, zero))
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		public static int FindNearestLink(TMP_Text text, Vector3 position, Camera camera)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_022e: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_024e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0314: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0325: Unknown result type (might be due to invalid IL or missing references)
			//IL_0327: Unknown result type (might be due to invalid IL or missing references)
			//IL_0332: Unknown result type (might be due to invalid IL or missing references)
			//IL_0334: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0340: Unknown result type (might be due to invalid IL or missing references)
			//IL_0342: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0356: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			RectTransform rectTransform = text.rectTransform;
			ScreenPointToWorldPointInRectangle((Transform)(object)rectTransform, Vector2.op_Implicit(position), camera, out position);
			float num = float.PositiveInfinity;
			int result = 0;
			for (int i = 0; i < text.textInfo.linkCount; i++)
			{
				TMP_LinkInfo tMP_LinkInfo = text.textInfo.linkInfo[i];
				bool flag = false;
				Vector3 val = Vector3.get_zero();
				Vector3 val2 = Vector3.get_zero();
				Vector3 zero = Vector3.get_zero();
				Vector3 zero2 = Vector3.get_zero();
				for (int j = 0; j < tMP_LinkInfo.linkTextLength; j++)
				{
					int num2 = tMP_LinkInfo.linkTextfirstCharacterIndex + j;
					TMP_CharacterInfo tMP_CharacterInfo = text.textInfo.characterInfo[num2];
					int lineNumber = tMP_CharacterInfo.lineNumber;
					if (text.overflowMode == TextOverflowModes.Page && tMP_CharacterInfo.pageNumber + 1 != text.pageToDisplay)
					{
						continue;
					}
					if (!flag)
					{
						flag = true;
						val = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.descender, 0f));
						val2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.bottomLeft.x, tMP_CharacterInfo.ascender, 0f));
						if (tMP_LinkInfo.linkTextLength == 1)
						{
							flag = false;
							zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
							zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
							if (PointIntersectRectangle(position, val, val2, zero2, zero))
							{
								return i;
							}
							float num3 = DistanceToLine(val, val2, position);
							float num4 = DistanceToLine(val2, zero2, position);
							float num5 = DistanceToLine(zero2, zero, position);
							float num6 = DistanceToLine(zero, val, position);
							float num7 = ((num3 < num4) ? num3 : num4);
							num7 = ((num7 < num5) ? num7 : num5);
							num7 = ((num7 < num6) ? num7 : num6);
							if (num > num7)
							{
								num = num7;
								result = i;
							}
						}
					}
					if (flag && j == tMP_LinkInfo.linkTextLength - 1)
					{
						flag = false;
						zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, val, val2, zero2, zero))
						{
							return i;
						}
						float num8 = DistanceToLine(val, val2, position);
						float num9 = DistanceToLine(val2, zero2, position);
						float num10 = DistanceToLine(zero2, zero, position);
						float num11 = DistanceToLine(zero, val, position);
						float num12 = ((num8 < num9) ? num8 : num9);
						num12 = ((num12 < num10) ? num12 : num10);
						num12 = ((num12 < num11) ? num12 : num11);
						if (num > num12)
						{
							num = num12;
							result = i;
						}
					}
					else if (flag && lineNumber != text.textInfo.characterInfo[num2 + 1].lineNumber)
					{
						flag = false;
						zero = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.descender, 0f));
						zero2 = ((Transform)rectTransform).TransformPoint(new Vector3(tMP_CharacterInfo.topRight.x, tMP_CharacterInfo.ascender, 0f));
						if (PointIntersectRectangle(position, val, val2, zero2, zero))
						{
							return i;
						}
						float num13 = DistanceToLine(val, val2, position);
						float num14 = DistanceToLine(val2, zero2, position);
						float num15 = DistanceToLine(zero2, zero, position);
						float num16 = DistanceToLine(zero, val, position);
						float num17 = ((num13 < num14) ? num13 : num14);
						num17 = ((num17 < num15) ? num17 : num15);
						num17 = ((num17 < num16) ? num17 : num16);
						if (num > num17)
						{
							num = num17;
							result = i;
						}
					}
				}
			}
			return result;
		}

		private static bool PointIntersectRectangle(Vector3 m, Vector3 a, Vector3 b, Vector3 c, Vector3 d)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = b - a;
			Vector3 val2 = m - a;
			Vector3 val3 = c - b;
			Vector3 val4 = m - b;
			float num = Vector3.Dot(val, val2);
			float num2 = Vector3.Dot(val3, val4);
			if (0f <= num && num <= Vector3.Dot(val, val) && 0f <= num2)
			{
				return num2 <= Vector3.Dot(val3, val3);
			}
			return false;
		}

		public static bool ScreenPointToWorldPointInRectangle(Transform transform, Vector2 screenPoint, Camera cam, out Vector3 worldPoint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			worldPoint = Vector2.op_Implicit(Vector2.get_zero());
			Ray val = RectTransformUtility.ScreenPointToRay(cam, screenPoint);
			Plane val2 = new Plane(transform.get_rotation() * Vector3.get_back(), transform.get_position());
			float num = default(float);
			if (!((Plane)(ref val2)).Raycast(val, ref num))
			{
				return false;
			}
			worldPoint = ((Ray)(ref val)).GetPoint(num);
			return true;
		}

		private static bool IntersectLinePlane(LineSegment line, Vector3 point, Vector3 normal, out Vector3 intersectingPoint)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			intersectingPoint = Vector3.get_zero();
			Vector3 val = line.Point2 - line.Point1;
			Vector3 val2 = line.Point1 - point;
			float num = Vector3.Dot(normal, val);
			float num2 = 0f - Vector3.Dot(normal, val2);
			if (Mathf.Abs(num) < Mathf.Epsilon)
			{
				if (num2 == 0f)
				{
					return true;
				}
				return false;
			}
			float num3 = num2 / num;
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			intersectingPoint = line.Point1 + num3 * val;
			return true;
		}

		public static float DistanceToLine(Vector3 a, Vector3 b, Vector3 point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = b - a;
			Vector3 val2 = a - point;
			float num = Vector3.Dot(val, val2);
			if (num > 0f)
			{
				return Vector3.Dot(val2, val2);
			}
			Vector3 val3 = point - b;
			if (Vector3.Dot(val, val3) > 0f)
			{
				return Vector3.Dot(val3, val3);
			}
			Vector3 val4 = val2 - val * (num / Vector3.Dot(val, val));
			return Vector3.Dot(val4, val4);
		}

		public static char ToLowerFast(char c)
		{
			if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@abcdefghijklmnopqrstuvwxyz[-]^_`abcdefghijklmnopqrstuvwxyz{|}~-"[c];
		}

		public static char ToUpperFast(char c)
		{
			if (c > "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-".Length - 1)
			{
				return c;
			}
			return "-------------------------------- !-#$%&-()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[-]^_`ABCDEFGHIJKLMNOPQRSTUVWXYZ{|}~-"[c];
		}

		public static int GetSimpleHashCode(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ s[i];
			}
			return num;
		}

		public static uint GetSimpleHashCodeLowercase(string s)
		{
			uint num = 5381u;
			for (int i = 0; i < s.Length; i++)
			{
				num = ((num << 5) + num) ^ ToLowerFast(s[i]);
			}
			return num;
		}

		public static int HexToInt(char hex)
		{
			return hex switch
			{
				'0' => 0, 
				'1' => 1, 
				'2' => 2, 
				'3' => 3, 
				'4' => 4, 
				'5' => 5, 
				'6' => 6, 
				'7' => 7, 
				'8' => 8, 
				'9' => 9, 
				'A' => 10, 
				'B' => 11, 
				'C' => 12, 
				'D' => 13, 
				'E' => 14, 
				'F' => 15, 
				'a' => 10, 
				'b' => 11, 
				'c' => 12, 
				'd' => 13, 
				'e' => 14, 
				'f' => 15, 
				_ => 15, 
			};
		}

		public static int StringToInt(string s)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				num += HexToInt(s[i]) * (int)Mathf.Pow(16f, (float)(s.Length - 1 - i));
			}
			return num;
		}
	}
}
