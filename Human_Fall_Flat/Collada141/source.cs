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
	public class source
	{
		private asset assetField;

		private string idField;

		private object itemField;

		private string nameField;

		private technique[] techniqueField;

		private sourceTechnique_common technique_commonField;

		public asset asset
		{
			get
			{
				return assetField;
			}
			set
			{
				assetField = value;
			}
		}

		[XmlElement("IDREF_array", typeof(IDREF_array))]
		[XmlElement("Name_array", typeof(Name_array))]
		[XmlElement("bool_array", typeof(bool_array))]
		[XmlElement("float_array", typeof(float_array))]
		[XmlElement("int_array", typeof(int_array))]
		public object Item
		{
			get
			{
				return itemField;
			}
			set
			{
				itemField = value;
			}
		}

		public sourceTechnique_common technique_common
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
	}
}
