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
	public class rigid_constraint
	{
		private rigid_constraintAttachment attachmentField;

		private extra[] extraField;

		private string nameField;

		private rigid_constraintRef_attachment ref_attachmentField;

		private string sidField;

		private technique[] techniqueField;

		private rigid_constraintTechnique_common technique_commonField;

		public rigid_constraintRef_attachment ref_attachment
		{
			get
			{
				return ref_attachmentField;
			}
			set
			{
				ref_attachmentField = value;
			}
		}

		public rigid_constraintAttachment attachment
		{
			get
			{
				return attachmentField;
			}
			set
			{
				attachmentField = value;
			}
		}

		public rigid_constraintTechnique_common technique_common
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
