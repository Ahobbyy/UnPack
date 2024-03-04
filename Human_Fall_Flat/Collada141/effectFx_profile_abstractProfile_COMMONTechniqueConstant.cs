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
	public class effectFx_profile_abstractProfile_COMMONTechniqueConstant
	{
		private common_color_or_texture_type emissionField;

		private common_float_or_param_type index_of_refractionField;

		private common_color_or_texture_type reflectiveField;

		private common_float_or_param_type reflectivityField;

		private common_float_or_param_type transparencyField;

		private common_transparent_type transparentField;

		public common_color_or_texture_type emission
		{
			get
			{
				return emissionField;
			}
			set
			{
				emissionField = value;
			}
		}

		public common_color_or_texture_type reflective
		{
			get
			{
				return reflectiveField;
			}
			set
			{
				reflectiveField = value;
			}
		}

		public common_float_or_param_type reflectivity
		{
			get
			{
				return reflectivityField;
			}
			set
			{
				reflectivityField = value;
			}
		}

		public common_transparent_type transparent
		{
			get
			{
				return transparentField;
			}
			set
			{
				transparentField = value;
			}
		}

		public common_float_or_param_type transparency
		{
			get
			{
				return transparencyField;
			}
			set
			{
				transparencyField = value;
			}
		}

		public common_float_or_param_type index_of_refraction
		{
			get
			{
				return index_of_refractionField;
			}
			set
			{
				index_of_refractionField = value;
			}
		}
	}
}
