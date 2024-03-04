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
	public class common_color_or_texture_typeColor
	{
		private string sidField;

		private double[] textField;

		[XmlAttribute(DataType = "NCName")]
		public string sid
		{
			get
			{
				return sidField;
			}
			set
			{
				sidField = value;
			}
		}

		[XmlText]
		public string _Text_
		{
			get
			{
				return COLLADA.ConvertFromArray(Values);
			}
			set
			{
				Values = COLLADA.ConvertDoubleArray(value);
			}
		}

		[XmlIgnore]
		public double[] Values
		{
			get
			{
				return textField;
			}
			set
			{
				textField = value;
			}
		}
	}
}
