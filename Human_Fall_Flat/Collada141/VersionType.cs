using System;
using System.CodeDom.Compiler;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public enum VersionType
	{
		[XmlEnum("1.4.0")]
		Item140,
		[XmlEnum("1.4.1")]
		Item141
	}
}
