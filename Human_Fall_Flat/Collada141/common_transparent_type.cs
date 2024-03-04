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
	public class common_transparent_type : common_color_or_texture_type
	{
		private fx_opaque_enum opaqueField;

		[XmlAttribute]
		[DefaultValue(fx_opaque_enum.A_ONE)]
		public fx_opaque_enum opaque
		{
			get
			{
				return opaqueField;
			}
			set
			{
				opaqueField = value;
			}
		}

		public common_transparent_type()
		{
			opaqueField = fx_opaque_enum.A_ONE;
		}
	}
}
