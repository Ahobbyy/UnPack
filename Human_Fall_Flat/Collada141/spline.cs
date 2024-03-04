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
	public class spline
	{
		private bool closedField;

		private splineControl_vertices control_verticesField;

		private extra[] extraField;

		private source[] sourceField;

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

		public splineControl_vertices control_vertices
		{
			get
			{
				return control_verticesField;
			}
			set
			{
				control_verticesField = value;
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

		[XmlAttribute]
		[DefaultValue(false)]
		public bool closed
		{
			get
			{
				return closedField;
			}
			set
			{
				closedField = value;
			}
		}

		public spline()
		{
			closedField = false;
		}
	}
}
