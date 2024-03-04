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
	public class visual_sceneEvaluate_scene
	{
		private string nameField;

		private visual_sceneEvaluate_sceneRender[] renderField;

		[XmlElement("render")]
		public visual_sceneEvaluate_sceneRender[] render
		{
			get
			{
				return renderField;
			}
			set
			{
				renderField = value;
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
