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
	[XmlRoot("profile_COMMON", Namespace = "http://www.collada.org/2005/11/COLLADASchema", IsNullable = false)]
	public class effectFx_profile_abstractProfile_COMMON
	{
		private asset assetField;

		private extra[] extraField;

		private string idField;

		private object[] itemsField;

		private effectFx_profile_abstractProfile_COMMONTechnique techniqueField;

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

		public effectFx_profile_abstractProfile_COMMONTechnique technique
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
	}
}
