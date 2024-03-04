using System.Collections.Generic;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class MessageList
	{
		private Vector2 m_ScrollPosition = Vector2.get_zero();

		private GUIStyle[] m_Style = (GUIStyle[])(object)new GUIStyle[2];

		private IEnumerable<AssetInfo> m_Selecteditems;

		private List<MessageSystem.Message> m_Messages;

		private Vector2 m_Dimensions = new Vector2(0f, 0f);

		private const float k_ScrollbarPadding = 16f;

		private const float k_BorderSize = 1f;

		internal MessageList()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			Init();
		}

		private void Init()
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Expected O, but got Unknown
			m_Style[0] = GUIStyle.op_Implicit("OL EntryBackOdd");
			m_Style[1] = GUIStyle.op_Implicit("OL EntryBackEven");
			m_Style[0].set_wordWrap(true);
			m_Style[1].set_wordWrap(true);
			m_Style[0].set_padding(new RectOffset(32, 0, 1, 4));
			m_Style[1].set_padding(new RectOffset(32, 0, 1, 4));
			m_Messages = new List<MessageSystem.Message>();
		}

		internal void OnGUI(Rect fullPos)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Expected O, but got Unknown
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			DrawOutline(fullPos, 1f);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref fullPos)).get_x() + 1f, ((Rect)(ref fullPos)).get_y() + 1f, ((Rect)(ref fullPos)).get_width() - 2f, ((Rect)(ref fullPos)).get_height() - 2f);
			if (m_Dimensions.y == 0f || m_Dimensions.x != ((Rect)(ref val)).get_width() - 16f)
			{
				m_Dimensions.x = ((Rect)(ref val)).get_width() - 16f;
				m_Dimensions.y = 0f;
				foreach (MessageSystem.Message message in m_Messages)
				{
					m_Dimensions.y += m_Style[0].CalcHeight(new GUIContent(message.message), m_Dimensions.x);
				}
			}
			m_ScrollPosition = GUI.BeginScrollView(val, m_ScrollPosition, new Rect(0f, 0f, m_Dimensions.x, m_Dimensions.y));
			int num = 0;
			float num2 = 0f;
			foreach (MessageSystem.Message message2 in m_Messages)
			{
				int num3 = num % 2;
				GUIContent val2 = new GUIContent(message2.message);
				float num4 = m_Style[num3].CalcHeight(val2, m_Dimensions.x);
				GUI.Box(new Rect(0f, num2, m_Dimensions.x, num4), val2, m_Style[num3]);
				GUI.DrawTexture(new Rect(0f, num2, 32f, 32f), (Texture)(object)message2.icon);
				num++;
				num2 += num4;
			}
			GUI.EndScrollView();
		}

		internal void SetItems(IEnumerable<AssetInfo> items)
		{
			m_Selecteditems = items;
			CollectMessages();
		}

		internal void CollectMessages()
		{
			m_Messages.Clear();
			m_Dimensions.y = 0f;
			if (m_Selecteditems == null)
			{
				return;
			}
			foreach (AssetInfo selecteditem in m_Selecteditems)
			{
				m_Messages.AddRange(selecteditem.GetMessages());
			}
		}

		internal static void DrawOutline(Rect rect, float size)
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Invalid comparison between Unknown and I4
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			Color val = default(Color);
			((Color)(ref val))._002Ector(0.6f, 0.6f, 0.6f, 1.333f);
			if (EditorGUIUtility.get_isProSkin())
			{
				val.r = 0.12f;
				val.g = 0.12f;
				val.b = 0.12f;
			}
			if ((int)Event.get_current().get_type() == 7)
			{
				Color color = GUI.get_color();
				GUI.set_color(GUI.get_color() * val);
				GUI.DrawTexture(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y(), ((Rect)(ref rect)).get_width(), size), (Texture)(object)EditorGUIUtility.get_whiteTexture());
				GUI.DrawTexture(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_yMax() - size, ((Rect)(ref rect)).get_width(), size), (Texture)(object)EditorGUIUtility.get_whiteTexture());
				GUI.DrawTexture(new Rect(((Rect)(ref rect)).get_x(), ((Rect)(ref rect)).get_y() + 1f, size, ((Rect)(ref rect)).get_height() - 2f * size), (Texture)(object)EditorGUIUtility.get_whiteTexture());
				GUI.DrawTexture(new Rect(((Rect)(ref rect)).get_xMax() - size, ((Rect)(ref rect)).get_y() + 1f, size, ((Rect)(ref rect)).get_height() - 2f * size), (Texture)(object)EditorGUIUtility.get_whiteTexture());
				GUI.set_color(color);
			}
		}
	}
}
