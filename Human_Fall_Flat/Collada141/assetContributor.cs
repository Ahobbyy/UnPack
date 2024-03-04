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
	public class assetContributor
	{
		private string authorField;

		private string authoring_toolField;

		private string commentsField;

		private string copyrightField;

		private string source_dataField;

		public string author
		{
			get
			{
				return authorField;
			}
			set
			{
				authorField = value;
			}
		}

		public string authoring_tool
		{
			get
			{
				return authoring_toolField;
			}
			set
			{
				authoring_toolField = value;
			}
		}

		public string comments
		{
			get
			{
				return commentsField;
			}
			set
			{
				commentsField = value;
			}
		}

		public string copyright
		{
			get
			{
				return copyrightField;
			}
			set
			{
				copyrightField = value;
			}
		}

		[XmlElement(DataType = "anyURI")]
		public string source_data
		{
			get
			{
				return source_dataField;
			}
			set
			{
				source_dataField = value;
			}
		}
	}
}
