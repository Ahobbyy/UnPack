using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IncludeInSchema = false)]
	public enum ItemChoiceType
	{
		@float,
		float2,
		float3,
		float4,
		sampler2D,
		surface
	}
}
