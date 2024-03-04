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
	public class effect
	{
		private fx_annotate_common[] annotateField;

		private asset assetField;

		private extra[] extraField;

		private string idField;

		private image[] imageField;

		private effectFx_profile_abstractProfile_COMMON[] itemsField;

		private string nameField;

		private fx_newparam_common[] newparamField;

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

		[XmlElement("annotate")]
		public fx_annotate_common[] annotate
		{
			get
			{
				return annotateField;
			}
			set
			{
				annotateField = value;
			}
		}

		[XmlElement("image")]
		public image[] image
		{
			get
			{
				return imageField;
			}
			set
			{
				imageField = value;
			}
		}

		[XmlElement("newparam")]
		public fx_newparam_common[] newparam
		{
			get
			{
				return newparamField;
			}
			set
			{
				newparamField = value;
			}
		}

		[XmlElement("profile_COMMON")]
		public effectFx_profile_abstractProfile_COMMON[] Items
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
