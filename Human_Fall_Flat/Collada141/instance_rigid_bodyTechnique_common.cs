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
	public class instance_rigid_bodyTechnique_common
	{
		private string angular_velocityField;

		private instance_rigid_bodyTechnique_commonDynamic dynamicField;

		private TargetableFloat3 inertiaField;

		private object itemField;

		private TargetableFloat massField;

		private object[] mass_frameField;

		private instance_rigid_bodyTechnique_commonShape[] shapeField;

		private string velocityField;

		[DefaultValue("0.0 0.0 0.0")]
		public string angular_velocity
		{
			get
			{
				return angular_velocityField;
			}
			set
			{
				angular_velocityField = value;
			}
		}

		[DefaultValue("0.0 0.0 0.0")]
		public string velocity
		{
			get
			{
				return velocityField;
			}
			set
			{
				velocityField = value;
			}
		}

		public instance_rigid_bodyTechnique_commonDynamic dynamic
		{
			get
			{
				return dynamicField;
			}
			set
			{
				dynamicField = value;
			}
		}

		public TargetableFloat mass
		{
			get
			{
				return massField;
			}
			set
			{
				massField = value;
			}
		}

		[XmlArrayItem("rotate", typeof(rotate), IsNullable = false)]
		[XmlArrayItem("translate", typeof(TargetableFloat3), IsNullable = false)]
		public object[] mass_frame
		{
			get
			{
				return mass_frameField;
			}
			set
			{
				mass_frameField = value;
			}
		}

		public TargetableFloat3 inertia
		{
			get
			{
				return inertiaField;
			}
			set
			{
				inertiaField = value;
			}
		}

		[XmlElement("instance_physics_material", typeof(InstanceWithExtra))]
		[XmlElement("physics_material", typeof(physics_material))]
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

		[XmlElement("shape")]
		public instance_rigid_bodyTechnique_commonShape[] shape
		{
			get
			{
				return shapeField;
			}
			set
			{
				shapeField = value;
			}
		}

		public instance_rigid_bodyTechnique_common()
		{
			angular_velocityField = "0.0 0.0 0.0";
			velocityField = "0.0 0.0 0.0";
		}
	}
}
