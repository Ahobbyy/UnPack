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
	public class visual_sceneEvaluate_sceneRender
	{
		private string camera_nodeField;

		private instance_effect instance_effectField;

		private string[] layerField;

		[XmlElement("layer", DataType = "NCName")]
		public string[] layer
		{
			get
			{
				return layerField;
			}
			set
			{
				layerField = value;
			}
		}

		public instance_effect instance_effect
		{
			get
			{
				return instance_effectField;
			}
			set
			{
				instance_effectField = value;
			}
		}

		[XmlAttribute(DataType = "anyURI")]
		public string camera_node
		{
			get
			{
				return camera_nodeField;
			}
			set
			{
				camera_nodeField = value;
			}
		}
	}
}
