using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Collada141
{
	[Serializable]
	[GeneratedCode("xsd", "4.0.30319.1")]
	[DesignerCategory("code")]
	[XmlType(AnonymousType = true, Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	[XmlRoot(Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class float_array
	{
		private ulong countField;

		private short digitsField;

		private string idField;

		private short magnitudeField;

		private string nameField;

		private double[] textField;

		[XmlAttribute(DataType = "ID")]
		public string id
		{
			get
			{
				return idField;
			}
			set
			{
				idField = value;
			}
		}

		[XmlAttribute(DataType = "NCName")]
		public string name
		{
			get
			{
				return nameField;
			}
			set
			{
				nameField = value;
			}
		}

		[XmlAttribute]
		public ulong count
		{
			get
			{
				return countField;
			}
			set
			{
				countField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(short), "6")]
		public short digits
		{
			get
			{
				return digitsField;
			}
			set
			{
				digitsField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(typeof(short), "38")]
		public short magnitude
		{
			get
			{
				return magnitudeField;
			}
			set
			{
				magnitudeField = value;
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

		public float_array()
		{
			digitsField = 6;
			magnitudeField = 38;
		}
	}
}
