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
	public class COLLADAScene
	{
		private extra[] extraField;

		private InstanceWithExtra[] instance_physics_sceneField;

		private InstanceWithExtra instance_visual_sceneField;

		[XmlElement("instance_physics_scene")]
		public InstanceWithExtra[] instance_physics_scene
		{
			get
			{
				return instance_physics_sceneField;
			}
			set
			{
				instance_physics_sceneField = value;
			}
		}

		public InstanceWithExtra instance_visual_scene
		{
			get
			{
				return instance_visual_sceneField;
			}
			set
			{
				instance_visual_sceneField = value;
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
