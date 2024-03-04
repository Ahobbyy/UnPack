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
	public class Name_array
	{
		private ulong countField;

		private string idField;

		private string nameField;

		private string[] textField;

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

		[XmlElement("Name")]
		public string _Text_
		{
			get
			{
				return COLLADA.ConvertFromArray(Values);
			}
			set
			{
				Values = COLLADA.ConvertStringArray(value);
			}
		}

		[XmlIgnore]
		public string[] Values
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
