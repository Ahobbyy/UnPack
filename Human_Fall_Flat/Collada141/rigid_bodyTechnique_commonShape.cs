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
	public class rigid_bodyTechnique_commonShape
	{
		private TargetableFloat densityField;

		private extra[] extraField;

		private rigid_bodyTechnique_commonShapeHollow hollowField;

		private object item1Field;

		private object itemField;

		private object[] itemsField;

		private TargetableFloat massField;

		public rigid_bodyTechnique_commonShapeHollow hollow
		{
			get
			{
				return hollowField;
			}
			set
			{
				hollowField = value;
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

		public TargetableFloat density
		{
			get
			{
				return densityField;
			}
			set
			{
				densityField = value;
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

		[XmlElement("box", typeof(box))]
		[XmlElement("capsule", typeof(capsule))]
		[XmlElement("cylinder", typeof(cylinder))]
		[XmlElement("instance_geometry", typeof(instance_geometry))]
		[XmlElement("plane", typeof(plane))]
		[XmlElement("sphere", typeof(sphere))]
		[XmlElement("tapered_capsule", typeof(tapered_capsule))]
		[XmlElement("tapered_cylinder", typeof(tapered_cylinder))]
		public object Item1
		{
			get
			{
				return item1Field;
			}
			set
			{
				item1Field = value;
			}
		}

		[XmlElement("rotate", typeof(rotate))]
		[XmlElement("translate", typeof(TargetableFloat3))]
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
	}
}
