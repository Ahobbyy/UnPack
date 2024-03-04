namespace TMPro
{
	public static class TMP_Compatibility
	{
		public enum AnchorPositions
		{
			TopLeft,
			Top,
			TopRight,
			Left,
			Center,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			BaseLine,
			None
		}

		public static TextAlignmentOptions ConvertTextAlignmentEnumValues(TextAlignmentOptions oldValue)
		{
			return oldValue switch
			{
				(TextAlignmentOptions)0 => TextAlignmentOptions.TopLeft, 
				(TextAlignmentOptions)1 => TextAlignmentOptions.Top, 
				(TextAlignmentOptions)2 => TextAlignmentOptions.TopRight, 
				(TextAlignmentOptions)3 => TextAlignmentOptions.TopJustified, 
				(TextAlignmentOptions)4 => TextAlignmentOptions.Left, 
				(TextAlignmentOptions)5 => TextAlignmentOptions.Center, 
				(TextAlignmentOptions)6 => TextAlignmentOptions.Right, 
				(TextAlignmentOptions)7 => TextAlignmentOptions.Justified, 
				(TextAlignmentOptions)8 => TextAlignmentOptions.BottomLeft, 
				(TextAlignmentOptions)9 => TextAlignmentOptions.Bottom, 
				(TextAlignmentOptions)10 => TextAlignmentOptions.BottomRight, 
				(TextAlignmentOptions)11 => TextAlignmentOptions.BottomJustified, 
				(TextAlignmentOptions)12 => TextAlignmentOptions.BaselineLeft, 
				(TextAlignmentOptions)13 => TextAlignmentOptions.Baseline, 
				(TextAlignmentOptions)14 => TextAlignmentOptions.BaselineRight, 
				(TextAlignmentOptions)15 => TextAlignmentOptions.BaselineJustified, 
				(TextAlignmentOptions)16 => TextAlignmentOptions.MidlineLeft, 
				(TextAlignmentOptions)17 => TextAlignmentOptions.Midline, 
				(TextAlignmentOptions)18 => TextAlignmentOptions.MidlineRight, 
				(TextAlignmentOptions)19 => TextAlignmentOptions.MidlineJustified, 
				(TextAlignmentOptions)20 => TextAlignmentOptions.CaplineLeft, 
				(TextAlignmentOptions)21 => TextAlignmentOptions.Capline, 
				(TextAlignmentOptions)22 => TextAlignmentOptions.CaplineRight, 
				(TextAlignmentOptions)23 => TextAlignmentOptions.CaplineJustified, 
				_ => TextAlignmentOptions.TopLeft, 
			};
		}
	}
}
