using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Collada141;
using UnityEditor;
using UnityEngine;

public class ExportScene
{
	private static List<geometry> geometries = new List<geometry>();

	private static List<node> nodes = new List<node>();

	private static Dictionary<string, material> materials = new Dictionary<string, material>();

	private static List<effect> effects = new List<effect>();

	private static int nodeID = 0;

	[MenuItem("Tools/Export selected to FBX...")]
	public static void ExportSelectedFBX()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		List<GameObject> list = new List<GameObject>();
		Object[] objects = Selection.get_objects();
		foreach (Object val in objects)
		{
			if (val is GameObject)
			{
				list.Add((GameObject)val);
			}
		}
		List<GameObject> list2 = new List<GameObject>();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			GameObject val2 = list[num];
			bool flag = false;
			Transform parent = val2.get_transform().get_parent();
			while ((Object)(object)parent != (Object)null)
			{
				if (list2.Contains(((Component)parent).get_gameObject()))
				{
					flag = true;
					break;
				}
				parent = parent.get_parent();
			}
			if (!flag)
			{
				list2.Add(val2);
			}
		}
		string fileName = EditorUtility.SaveFilePanel("Export", "", Path.GetFileNameWithoutExtension(EditorApplication.get_currentScene()) + ".dae", "dae");
		nodes.Clear();
		geometries.Clear();
		materials.Clear();
		effects.Clear();
		nodeID = 0;
		nodes.Add(ToNode(list2[0].get_transform()));
		COLLADA cOLLADA = new COLLADA();
		cOLLADA.asset = new asset
		{
			created = DateTime.Now,
			modified = DateTime.Now,
			revision = "1.0"
		};
		cOLLADA.Items = new object[4]
		{
			new library_effects
			{
				effect = effects.ToArray()
			},
			new library_materials
			{
				material = materials.Values.ToArray()
			},
			new library_geometries
			{
				geometry = geometries.ToArray()
			},
			new library_visual_scenes
			{
				visual_scene = new visual_scene[1]
				{
					new visual_scene
					{
						id = "DefaultScene",
						node = nodes.ToArray()
					}
				}
			}
		};
		cOLLADA.scene = new COLLADAScene
		{
			instance_visual_scene = new InstanceWithExtra
			{
				url = "#DefaultScene"
			}
		};
		cOLLADA.Save(fileName);
	}

	private static float_array ToFloatArray(Vector3[] src, string id)
	{
		double[] array = new double[src.Length * 3];
		for (int i = 0; i < src.Length; i++)
		{
			array[i * 3] = 0f - src[i].x;
			array[i * 3 + 1] = src[i].y;
			array[i * 3 + 2] = src[i].z;
		}
		return new float_array
		{
			id = id,
			count = (ulong)src.Length * 3uL,
			Values = array
		};
	}

	private static source ToSource(Vector3[] src, string id)
	{
		float_array float_array = ToFloatArray(src, id + "-array");
		source source = new source();
		source.id = id;
		source.Item = float_array;
		source.technique_common = new sourceTechnique_common
		{
			accessor = new accessor
			{
				source = "#" + float_array.id,
				count = float_array.count / 3uL,
				stride = 3uL,
				param = new param[3]
				{
					new param
					{
						name = "X",
						type = "float"
					},
					new param
					{
						name = "Y",
						type = "float"
					},
					new param
					{
						name = "Z",
						type = "float"
					}
				}
			}
		};
		return source;
	}

	private static float_array ToFloatArray(Vector2[] src, string id)
	{
		double[] array = new double[src.Length * 2];
		for (int i = 0; i < src.Length; i++)
		{
			array[i * 2] = src[i].x;
			array[i * 2 + 1] = src[i].y;
		}
		return new float_array
		{
			id = id,
			count = (ulong)src.Length * 2uL,
			Values = array
		};
	}

	private static source ToSource(Vector2[] src, string id)
	{
		float_array float_array = ToFloatArray(src, id + "-array");
		source source = new source();
		source.id = id;
		source.Item = float_array;
		source.technique_common = new sourceTechnique_common
		{
			accessor = new accessor
			{
				source = "#" + float_array.id,
				count = float_array.count / 2uL,
				stride = 2uL,
				param = new param[2]
				{
					new param
					{
						name = "X",
						type = "float"
					},
					new param
					{
						name = "Y",
						type = "float"
					}
				}
			}
		};
		return source;
	}

	private static float_array ToFloatArray(Color[] src, string id)
	{
		double[] array = new double[src.Length * 3];
		for (int i = 0; i < src.Length; i++)
		{
			array[i * 3] = src[i].r;
			array[i * 3 + 1] = src[i].g;
			array[i * 3 + 2] = src[i].b;
		}
		return new float_array
		{
			id = id,
			count = (ulong)src.Length * 3uL,
			Values = array
		};
	}

	private static source ToSource(Color[] src, string id)
	{
		float_array float_array = ToFloatArray(src, id + "-array");
		source source = new source();
		source.id = id;
		source.Item = float_array;
		source.technique_common = new sourceTechnique_common
		{
			accessor = new accessor
			{
				source = "#" + float_array.id,
				count = float_array.count / 3uL,
				stride = 3uL,
				param = new param[3]
				{
					new param
					{
						name = "R",
						type = "float"
					},
					new param
					{
						name = "G",
						type = "float"
					},
					new param
					{
						name = "B",
						type = "float"
					}
				}
			}
		};
		return source;
	}

	private static geometry ToGeometry(Mesh mesh, Material[] sharedMaterials, string id, out bind_material bind_material)
	{
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		List<source> list = new List<source>();
		List<InputLocal> list2 = new List<InputLocal>();
		source source = ToSource(mesh.get_vertices(), id + "-Pos");
		list.Add(source);
		list2.Add(new InputLocal
		{
			semantic = "POSITION",
			source = "#" + source.id
		});
		Vector3[] normals = mesh.get_normals();
		if (normals != null)
		{
			source source2 = ToSource(normals, id + "-Normal");
			list.Add(source2);
			list2.Add(new InputLocal
			{
				semantic = "NORMAL",
				source = "#" + source2.id
			});
		}
		Vector2[] uv = mesh.get_uv();
		if (uv != null)
		{
			source source3 = ToSource(uv, id + "-Uv0");
			list.Add(source3);
			list2.Add(new InputLocal
			{
				semantic = "TEXCOORD",
				source = "#" + source3.id
			});
		}
		Vector2[] uv2 = mesh.get_uv2();
		if (uv2 != null)
		{
			source source4 = ToSource(uv2, id + "-Uv1");
			list.Add(source4);
			list2.Add(new InputLocal
			{
				semantic = "TEXCOORD",
				source = "#" + source4.id
			});
		}
		Color[] colors = mesh.get_colors();
		if (colors != null)
		{
			source source5 = ToSource(colors, id + "-Color");
			list.Add(source5);
			list2.Add(new InputLocal
			{
				semantic = "COLOR",
				source = "#" + source5.id
			});
		}
		List<instance_material> list3 = new List<instance_material>();
		List<object> list4 = new List<object>();
		for (int i = 0; i < mesh.get_subMeshCount(); i++)
		{
			int[] triangles = mesh.GetTriangles(i);
			List<string> list5 = new List<string>();
			for (int j = 0; j < triangles.Length; j += 3)
			{
				list5.Add(triangles[j + 2].ToString());
				list5.Add(triangles[j + 1].ToString());
				list5.Add(triangles[j].ToString());
			}
			string p = string.Join(" ", Enumerable.ToArray(list5));
			Material val = ((sharedMaterials.Length > i) ? sharedMaterials[i] : sharedMaterials[0]);
			string name = ((Object)val).get_name();
			if (!materials.ContainsKey(name))
			{
				materials.Add(name, new material
				{
					id = name,
					instance_effect = new instance_effect
					{
						url = "#" + name + "-FX"
					}
				});
				effects.Add(new effect
				{
					id = name + "-FX",
					Items = new effectFx_profile_abstractProfile_COMMON[1]
					{
						new effectFx_profile_abstractProfile_COMMON
						{
							technique = new effectFx_profile_abstractProfile_COMMONTechnique
							{
								sid = "phong1",
								Item = new effectFx_profile_abstractProfile_COMMONTechniquePhong
								{
									diffuse = new common_color_or_texture_type
									{
										Item = new common_color_or_texture_typeColor
										{
											Values = new double[4]
											{
												val.get_color().r,
												val.get_color().g,
												val.get_color().b,
												val.get_color().a
											}
										}
									}
								}
							}
						}
					}
				});
			}
			list3.Add(new instance_material
			{
				symbol = name,
				target = "#" + name
			});
			list4.Add(new triangles
			{
				count = (uint)triangles.Length / 3u,
				material = name,
				input = new InputLocalOffset[1]
				{
					new InputLocalOffset
					{
						offset = 0uL,
						semantic = "VERTEX",
						source = "#" + id + "-Verts"
					}
				},
				p = p
			});
		}
		bind_material = new bind_material
		{
			technique_common = list3.ToArray()
		};
		return new geometry
		{
			id = id,
			name = id,
			Item = new mesh
			{
				source = list.ToArray(),
				vertices = new vertices
				{
					id = id + "-Verts",
					input = list2.ToArray()
				},
				Items = list4.ToArray()
			}
		};
	}

	private static node ToNode(Transform transform)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		Quaternion localRotation = transform.get_localRotation();
		Vector3 eulerAngles = ((Quaternion)(ref localRotation)).get_eulerAngles();
		node node = new node();
		node.id = "node" + nodeID;
		node.name = ((Object)transform).get_name();
		node.Items = new object[5]
		{
			new TargetableFloat3
			{
				Values = new double[3]
				{
					0f - transform.get_localPosition().x,
					transform.get_localPosition().y,
					transform.get_localPosition().z
				}
			},
			new rotate
			{
				Values = new double[4]
				{
					0.0,
					1.0,
					0.0,
					0f - eulerAngles.y
				}
			},
			new rotate
			{
				Values = new double[4] { 1.0, 0.0, 0.0, eulerAngles.x }
			},
			new rotate
			{
				Values = new double[4]
				{
					0.0,
					0.0,
					1.0,
					0f - eulerAngles.z
				}
			},
			new TargetableFloat3
			{
				Values = new double[3]
				{
					transform.get_localScale().x,
					transform.get_localScale().y,
					transform.get_localScale().z
				}
			}
		};
		node.ItemsElementName = new ItemsChoiceType2[5]
		{
			ItemsChoiceType2.translate,
			ItemsChoiceType2.rotate,
			ItemsChoiceType2.rotate,
			ItemsChoiceType2.rotate,
			ItemsChoiceType2.scale
		};
		node node2 = node;
		MeshFilter component = ((Component)transform).GetComponent<MeshFilter>();
		if ((Object)(object)component != (Object)null)
		{
			bind_material bind_material;
			geometry geometry = ToGeometry(component.get_sharedMesh(), ((Renderer)((Component)transform).GetComponent<MeshRenderer>()).get_sharedMaterials(), "geom" + nodeID, out bind_material);
			geometries.Add(geometry);
			node2.instance_geometry = new instance_geometry[1]
			{
				new instance_geometry
				{
					url = "#" + geometry.id,
					bind_material = bind_material
				}
			};
		}
		nodeID++;
		List<node> list = new List<node>();
		for (int i = 0; i < transform.get_childCount(); i++)
		{
			list.Add(ToNode(transform.GetChild(i)));
		}
		node2.node1 = list.ToArray();
		return node2;
	}
}
