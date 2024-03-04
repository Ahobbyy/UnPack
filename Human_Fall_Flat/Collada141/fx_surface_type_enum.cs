using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public enum fx_surface_type_enum
	{
		UNTYPED,
		[XmlEnum("1D")]
		Item1D,
		[XmlEnum("2D")]
		Item2D,
		[XmlEnum("3D")]
		Item3D,
		RECT,
		CUBE,
		DEPTH
	}
}
