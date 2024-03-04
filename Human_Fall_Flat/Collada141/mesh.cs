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
	[XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	[XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class mesh
	{
		private extra[] extraField;

		private object[] itemsField;

		private source[] sourceField;

		private vertices verticesField;

		[XmlElement("source")]
		public source[] source
		{
			get
			{
				return sourceField;
			}
			set
			{
				sourceField = value;
			}
		}

		public vertices vertices
		{
			get
			{
				return verticesField;
			}
			set
			{
				verticesField = value;
			}
		}

		[XmlElement("lines", typeof(lines))]
		[XmlElement("linestrips", typeof(linestrips))]
		[XmlElement("polygons", typeof(polygons))]
		[XmlElement("polylist", typeof(polylist))]
		[XmlElement("triangles", typeof(triangles))]
		[XmlElement("trifans", typeof(trifans))]
		[XmlElement("tristrips", typeof(tristrips))]
		public object[] Items
		{
			get
			{
				return itemsField;
			}
			set
			{
				itemsField = value;
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
