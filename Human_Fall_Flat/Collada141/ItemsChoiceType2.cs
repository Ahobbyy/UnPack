using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IncludeInSchema = false)]
	public enum ItemsChoiceType2
	{
		lookat,
		matrix,
		rotate,
		scale,
		skew,
		translate
	}
}
