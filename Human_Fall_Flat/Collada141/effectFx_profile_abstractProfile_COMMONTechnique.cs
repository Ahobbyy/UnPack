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
	public class effectFx_profile_abstractProfile_COMMONTechnique
	{
		private asset assetField;

		private extra[] extraField;

		private string idField;

		private object itemField;

		private object[] itemsField;

		private string sidField;

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

		[XmlElement("image", typeof(image))]
		[XmlElement("newparam", typeof(common_newparam_type))]
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

		[XmlElement("blinn", typeof(effectFx_profile_abstractProfile_COMMONTechniqueBlinn))]
		[XmlElement("constant", typeof(effectFx_profile_abstractProfile_COMMONTechniqueConstant))]
		[XmlElement("lambert", typeof(effectFx_profile_abstractProfile_COMMONTechniqueLambert))]
		[XmlElement("phong", typeof(effectFx_profile_abstractProfile_COMMONTechniquePhong))]
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
