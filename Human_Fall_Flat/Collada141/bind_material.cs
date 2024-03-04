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
	public class bind_material
	{
		private extra[] extraField;

		private param[] paramField;

		private technique[] techniqueField;

		private instance_material[] technique_commonField;

		[XmlElement("param")]
		public param[] param
		{
			get
			{
				return paramField;
			}
			set
			{
				paramField = value;
			}
		}

		[XmlArrayItem("instance_material", IsNullable = false)]
		public instance_material[] technique_common
		{
			get
			{
				return technique_commonField;
			}
			set
			{
				technique_commonField = value;
			}
		}

		[XmlElement("technique")]
		public technique[] technique
		{
			get
			{
				return techniqueField;
			}
			set
			{
				techniqueField = value;
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
