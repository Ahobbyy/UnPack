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
	public class instance_physics_model
	{
		private extra[] extraField;

		private InstanceWithExtra[] instance_force_fieldField;

		private instance_rigid_body[] instance_rigid_bodyField;

		private instance_rigid_constraint[] instance_rigid_constraintField;

		private string nameField;

		private string parentField;

		private string sidField;

		private string urlField;

		[XmlElement("instance_force_field")]
		public InstanceWithExtra[] instance_force_field
		{
			get
			{
				return instance_force_fieldField;
			}
			set
			{
				instance_force_fieldField = value;
			}
		}

		[XmlElement("instance_rigid_body")]
		public instance_rigid_body[] instance_rigid_body
		{
			get
			{
				return instance_rigid_bodyField;
			}
			set
			{
				instance_rigid_bodyField = value;
			}
		}

		[XmlElement("instance_rigid_constraint")]
		public instance_rigid_constraint[] instance_rigid_constraint
		{
			get
			{
				return instance_rigid_constraintField;
			}
			set
			{
				instance_rigid_constraintField = value;
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

		[XmlAttribute(DataType = "anyURI")]
		public string url
		{
			get
			{
				return urlField;
			}
			set
			{
				urlField = value;
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

		[XmlAttribute(DataType = "anyURI")]
		public string parent
		{
			get
			{
				return parentField;
			}
			set
			{
				parentField = value;
			}
		}
	}
}
