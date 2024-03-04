using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public enum fx_surface_format_hint_channels_enum
	{
		RGB,
		RGBA,
		L,
		LA,
		D,
		XYZ,
		XYZW
	}
}
