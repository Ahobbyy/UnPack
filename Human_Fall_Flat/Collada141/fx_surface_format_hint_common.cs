using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[DebuggerStepThrough]
	[DesignerCategory("code")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class fx_surface_format_hint_common
	{
		private fx_surface_format_hint_channels_enum channelsField;

		private extra[] extraField;

		private fx_surface_format_hint_option_enum[] optionField;

		private fx_surface_format_hint_precision_enum precisionField;

		private bool precisionFieldSpecified;

		private fx_surface_format_hint_range_enum rangeField;

		public fx_surface_format_hint_channels_enum channels
		{
			get
			{
				return channelsField;
			}
			set
			{
				channelsField = value;
			}
		}

		public fx_surface_format_hint_range_enum range
		{
			get
			{
				return rangeField;
			}
			set
			{
				rangeField = value;
			}
		}

		public fx_surface_format_hint_precision_enum precision
		{
			get
			{
				return precisionField;
			}
			set
			{
				precisionField = value;
			}
		}

		[XmlIgnore]
		public bool precisionSpecified
		{
			get
			{
				return precisionFieldSpecified;
			}
			set
			{
				precisionFieldSpecified = value;
			}
		}

		[XmlElement("option")]
		public fx_surface_format_hint_option_enum[] option
		{
			get
			{
				return optionField;
			}
			set
			{
				optionField = value;
			}
		}

		[XmlElement("extra")]
		public extra[] extra
		{
			get
			{
				return extraField;
			}
			set
			{
				extraField = value;
			}
		}
	}
}
