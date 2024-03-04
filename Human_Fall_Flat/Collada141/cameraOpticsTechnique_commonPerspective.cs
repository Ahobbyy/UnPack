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
	public class cameraOpticsTechnique_commonPerspective
	{
		private ItemsChoiceType1[] itemsElementNameField;

		private TargetableFloat[] itemsField;

		private TargetableFloat zfarField;

		private TargetableFloat znearField;

		[XmlElement("aspect_ratio", typeof(TargetableFloat))]
		[XmlElement("xfov", typeof(TargetableFloat))]
		[XmlElement("yfov", typeof(TargetableFloat))]
		[XmlChoiceIdentifier("ItemsElementName")]
		public TargetableFloat[] Items
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

		[XmlElement("ItemsElementName")]
		[XmlIgnore]
		public ItemsChoiceType1[] ItemsElementName
		{
			get
			{
				return itemsElementNameField;
			}
			set
			{
				itemsElementNameField = value;
			}
		}

		public TargetableFloat znear
		{
			get
			{
				return znearField;
			}
			set
			{
				znearField = value;
			}
		}

		public TargetableFloat zfar
		{
			get
			{
				return zfarField;
			}
			set
			{
				zfarField = value;
			}
		}
	}
}
