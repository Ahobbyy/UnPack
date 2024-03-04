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
	public class animation_clip
	{
		private asset assetField;

		private double endField;

		private bool endFieldSpecified;

		private extra[] extraField;

		private string idField;

		private InstanceWithExtra[] instance_animationField;

		private string nameField;

		private double startField;

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

		[XmlElement("instance_animation")]
		public InstanceWithExtra[] instance_animation
		{
			get
			{
				return instance_animationField;
			}
			set
			{
				instance_animationField = value;
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

		[XmlAttribute]
		[DefaultValue(0.0)]
		public double start
		{
			get
			{
				return startField;
			}
			set
			{
				startField = value;
			}
		}

		[XmlAttribute]
		public double end
		{
			get
			{
				return endField;
			}
			set
			{
				endField = value;
			}
		}

		[XmlIgnore]
		public bool endSpecified
		{
			get
			{
				return endFieldSpecified;
			}
			set
			{
				endFieldSpecified = value;
			}
		}

		public animation_clip()
		{
			startField = 0.0;
		}
	}
}
