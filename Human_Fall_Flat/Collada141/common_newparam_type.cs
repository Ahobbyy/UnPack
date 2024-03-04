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
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class common_newparam_type
	{
		private ItemChoiceType itemElementNameField;

		private object itemField;

		private string semanticField;

		private string sidField;

		[XmlElement(DataType = "NCName")]
		public string semantic
		{
			get
			{
				return semanticField;
			}
			set
			{
				semanticField = value;
			}
		}

		[XmlElement("float", typeof(double))]
		[XmlElement("float2", typeof(double))]
		[XmlElement("float3", typeof(double))]
		[XmlElement("float4", typeof(double))]
		[XmlElement("sampler2D", typeof(fx_sampler2D_common))]
		[XmlElement("surface", typeof(fx_surface_common))]
		[XmlChoiceIdentifier("ItemElementName")]
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

		[XmlIgnore]
		public ItemChoiceType ItemElementName
		{
			get
			{
				return itemElementNameField;
			}
			set
			{
				itemElementNameField = value;
			}
		}

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
	}
}
