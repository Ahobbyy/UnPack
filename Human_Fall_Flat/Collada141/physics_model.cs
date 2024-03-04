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
	public class physics_model
	{
		private asset assetField;

		private extra[] extraField;

		private string idField;

		private instance_physics_model[] instance_physics_modelField;

		private string nameField;

		private rigid_body[] rigid_bodyField;

		private rigid_constraint[] rigid_constraintField;

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

		[XmlElement("rigid_body")]
		public rigid_body[] rigid_body
		{
			get
			{
				return rigid_bodyField;
			}
			set
			{
				rigid_bodyField = value;
			}
		}

		[XmlElement("rigid_constraint")]
		public rigid_constraint[] rigid_constraint
		{
			get
			{
				return rigid_constraintField;
			}
			set
			{
				rigid_constraintField = value;
			}
		}

		[XmlElement("instance_physics_model")]
		public instance_physics_model[] instance_physics_model
		{
			get
			{
				return instance_physics_modelField;
			}
			set
			{
				instance_physics_modelField = value;
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
