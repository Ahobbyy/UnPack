using System;
using UnityEngine;

namespace ProGrids
{
	[Serializable]
	public class pg_ToggleContent
	{
		public string text_on;

		public string text_off;

		public Texture2D image_on;

		public Texture2D image_off;

		public string tooltip;

		private GUIContent gc = new GUIContent();

		public pg_ToggleContent(string t_on, string t_off, string tooltip)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			text_on = t_on;
			text_off = t_off;
			image_on = null;
			image_off = null;
			this.tooltip = tooltip;
			gc.set_tooltip(tooltip);
		}

		public pg_ToggleContent(string t_on, string t_off, Texture2D i_on, Texture2D i_off, string tooltip)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			text_on = t_on;
			text_off = t_off;
			image_on = i_on;
			image_off = i_off;
			this.tooltip = tooltip;
			gc.set_tooltip(tooltip);
		}

		public static bool ToggleButton(Rect r, pg_ToggleContent content, bool enabled, GUIStyle imageStyle, GUIStyle altStyle)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			content.gc.set_image((Texture)(object)(enabled ? content.image_on : content.image_off));
			content.gc.set_text((!((Object)(object)content.gc.get_image() == (Object)null)) ? "" : (enabled ? content.text_on : content.text_off));
			return GUI.Button(r, content.gc, ((Object)(object)content.gc.get_image() != (Object)null) ? imageStyle : altStyle);
		}
	}
}
