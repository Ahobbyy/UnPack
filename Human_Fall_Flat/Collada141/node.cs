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
	public class node
	{
		private asset assetField;

		private extra[] extraField;

		private string idField;

		private InstanceWithExtra[] instance_cameraField;

		private instance_controller[] instance_controllerField;

		private instance_geometry[] instance_geometryField;

		private InstanceWithExtra[] instance_lightField;

		private InstanceWithExtra[] instance_nodeField;

		private ItemsChoiceType2[] itemsElementNameField;

		private object[] itemsField;

		private string[] layerField;

		private string nameField;

		private node[] node1Field;

		private string sidField;

		private NodeType typeField;

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

		[XmlElement("lookat", typeof(lookat))]
		[XmlElement("matrix", typeof(matrix))]
		[XmlElement("rotate", typeof(rotate))]
		[XmlElement("scale", typeof(TargetableFloat3))]
		[XmlElement("skew", typeof(skew))]
		[XmlElement("translate", typeof(TargetableFloat3))]
		[XmlChoiceIdentifier("ItemsElementName")]
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

		[XmlElement("ItemsElementName")]
		[XmlIgnore]
		public ItemsChoiceType2[] ItemsElementName
		{
			get
			{
				return itemsElementNameField;
			}
			set
			{
				itemsElementNameField = value;
			}
		}

		[XmlElement("instance_camera")]
		public InstanceWithExtra[] instance_camera
		{
			get
			{
				return instance_cameraField;
			}
			set
			{
				instance_cameraField = value;
			}
		}

		[XmlElement("instance_controller")]
		public instance_controller[] instance_controller
		{
			get
			{
				return instance_controllerField;
			}
			set
			{
				instance_controllerField = value;
			}
		}

		[XmlElement("instance_geometry")]
		public instance_geometry[] instance_geometry
		{
			get
			{
				return instance_geometryField;
			}
			set
			{
				instance_geometryField = value;
			}
		}

		[XmlElement("instance_light")]
		public InstanceWithExtra[] instance_light
		{
			get
			{
				return instance_lightField;
			}
			set
			{
				instance_lightField = value;
			}
		}

		[XmlElement("instance_node")]
		public InstanceWithExtra[] instance_node
		{
			get
			{
				return instance_nodeField;
			}
			set
			{
				instance_nodeField = value;
			}
		}

		[XmlElement("node")]
		public node[] node1
		{
			get
			{
				return node1Field;
			}
			set
			{
				node1Field = value;
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

		[XmlAttribute(DataType = "NCName")]
		public string sid
		{
			get
			{
				return sidField;
			}
			set
			{
				sidField = value;
			}
		}

		[XmlAttribute]
		[DefaultValue(NodeType.NODE)]
		public NodeType type
		{
			get
			{
				return typeField;
			}
			set
			{
				typeField = value;
			}
		}

		[XmlAttribute(DataType = "Name")]
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

		public node()
		{
			typeField = NodeType.NODE;
		}
	}
}
