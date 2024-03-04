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
	public class rigid_bodyTechnique_common
	{
		private rigid_bodyTechnique_commonDynamic dynamicField;

		private TargetableFloat3 inertiaField;

		private object itemField;

		private TargetableFloat massField;

		private object[] mass_frameField;

		private rigid_bodyTechnique_commonShape[] shapeField;

		public rigid_bodyTechnique_commonDynamic dynamic
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
		public rigid_bodyTechnique_commonShape[] shape
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
	}
}
