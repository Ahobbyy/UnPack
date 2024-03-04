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
	public class tapered_cylinder
	{
		private extra[] extraField;

		private double heightField;

		private string radius1Field;

		private string radius2Field;

		public double height
		{
			get
			{
				return heightField;
			}
			set
			{
				heightField = value;
			}
		}

		public string radius1
		{
			get
			{
				return radius1Field;
			}
			set
			{
				radius1Field = value;
			}
		}

		public string radius2
		{
			get
			{
				return radius2Field;
			}
			set
			{
				radius2Field = value;
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
