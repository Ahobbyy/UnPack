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
	[XmlType(Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class fx_samplerDEPTH_common
	{
		private extra[] extraField;

		private fx_sampler_filter_common magfilterField;

		private fx_sampler_filter_common minfilterField;

		private string sourceField;

		private fx_sampler_wrap_common wrap_sField;

		private fx_sampler_wrap_common wrap_tField;

		[XmlElement(DataType = "NCName")]
		public string source
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

		[DefaultValue(fx_sampler_wrap_common.WRAP)]
		public fx_sampler_wrap_common wrap_s
		{
			get
			{
				return wrap_sField;
			}
			set
			{
				wrap_sField = value;
			}
		}

		[DefaultValue(fx_sampler_wrap_common.WRAP)]
		public fx_sampler_wrap_common wrap_t
		{
			get
			{
				return wrap_tField;
			}
			set
			{
				wrap_tField = value;
			}
		}

		[DefaultValue(fx_sampler_filter_common.NONE)]
		public fx_sampler_filter_common minfilter
		{
			get
			{
				return minfilterField;
			}
			set
			{
				minfilterField = value;
			}
		}

		[DefaultValue(fx_sampler_filter_common.NONE)]
		public fx_sampler_filter_common magfilter
		{
			get
			{
				return magfilterField;
			}
			set
			{
				magfilterField = value;
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

		public fx_samplerDEPTH_common()
		{
			wrap_sField = fx_sampler_wrap_common.WRAP;
			wrap_tField = fx_sampler_wrap_common.WRAP;
			minfilterField = fx_sampler_filter_common.NONE;
			magfilterField = fx_sampler_filter_common.NONE;
		}
	}
}
