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
	public class skin
	{
		private string bind_shape_matrixField;

		private extra[] extraField;

		private skinJoints jointsField;

		private string source1Field;

		private source[] sourceField;

		private skinVertex_weights vertex_weightsField;

		public string bind_shape_matrix
		{
			get
			{
				return bind_shape_matrixField;
			}
			set
			{
				bind_shape_matrixField = value;
			}
		}

		[XmlElement("source")]
		public source[] source
		{
			get
			{
				return sourceField;
			}
			set
			{
				sourceField = value;
			}
		}

		public skinJoints joints
		{
			get
			{
				return jointsField;
			}
			set
			{
				jointsField = value;
			}
		}

		public skinVertex_weights vertex_weights
		{
			get
			{
				return vertex_weightsField;
			}
			set
			{
				vertex_weightsField = value;
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

		[XmlAttribute("source", DataType = "anyURI")]
		public string source1
		{
			get
			{
				return source1Field;
			}
			set
			{
				source1Field = value;
			}
		}
	}
}
