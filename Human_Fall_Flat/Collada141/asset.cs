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
	public class asset
	{
		private assetContributor[] contributorField;

		private DateTime createdField;

		private string keywordsField;

		private DateTime modifiedField;

		private string revisionField;

		private string subjectField;

		private string titleField;

		private assetUnit unitField;

		private UpAxisType up_axisField;

		[XmlElement("contributor")]
		public assetContributor[] contributor
		{
			get
			{
				return contributorField;
			}
			set
			{
				contributorField = value;
			}
		}

		public DateTime created
		{
			get
			{
				return createdField;
			}
			set
			{
				createdField = value;
			}
		}

		public string keywords
		{
			get
			{
				return keywordsField;
			}
			set
			{
				keywordsField = value;
			}
		}

		public DateTime modified
		{
			get
			{
				return modifiedField;
			}
			set
			{
				modifiedField = value;
			}
		}

		public string revision
		{
			get
			{
				return revisionField;
			}
			set
			{
				revisionField = value;
			}
		}

		public string subject
		{
			get
			{
				return subjectField;
			}
			set
			{
				subjectField = value;
			}
		}

		public string title
		{
			get
			{
				return titleField;
			}
			set
			{
				titleField = value;
			}
		}

		public assetUnit unit
		{
			get
			{
				return unitField;
			}
			set
			{
				unitField = value;
			}
		}

		[DefaultValue(UpAxisType.Y_UP)]
		public UpAxisType up_axis
		{
			get
			{
				return up_axisField;
			}
			set
			{
				up_axisField = value;
			}
		}

		public asset()
		{
			up_axisField = UpAxisType.Y_UP;
		}
	}
}
