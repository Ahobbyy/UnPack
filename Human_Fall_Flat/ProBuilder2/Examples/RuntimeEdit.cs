using System.Collections.Generic;
using ProBuilder2.Common;
using UnityEngine;

namespace ProBuilder2.Examples
{
	public class RuntimeEdit : MonoBehaviour
	{
		private class pb_Selection
		{
			public pb_Object pb;

			public pb_Face face;

			public pb_Selection(pb_Object _pb, pb_Face _face)
			{
				pb = _pb;
				face = _face;
			}

			public bool HasObject()
			{
				return (Object)(object)pb != (Object)null;
			}

			public bool IsValid()
			{
				if ((Object)(object)pb != (Object)null)
				{
					return face != null;
				}
				return false;
			}

			public bool Equals(pb_Selection sel)
			{
				if (sel != null && sel.IsValid())
				{
					if ((Object)(object)pb == (Object)(object)sel.pb)
					{
						return face == sel.face;
					}
					return false;
				}
				return false;
			}

			public void Destroy()
			{
				if ((Object)(object)pb != (Object)null)
				{
					Object.Destroy((Object)(object)((Component)pb).get_gameObject());
				}
			}

			public override string ToString()
			{
				if ("pb_Object: " + (object)pb != null)
				{
					return ((Object)pb).get_name() + "\npb_Face: " + ((face == null) ? "Null" : ((object)face).ToString());
				}
				return "Null";
			}
		}

		private pb_Selection currentSelection;

		private pb_Selection previousSelection;

		private pb_Object preview;

		public Material previewMaterial;

		private Vector2 mousePosition_initial = Vector2.get_zero();

		private bool dragging;

		public float rotateSpeed = 100f;

		private void Awake()
		{
			SpawnCube();
		}

		private void OnGUI()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (GUI.Button(new Rect(5f, (float)(Screen.get_height() - 25), 80f, 20f), "Reset"))
			{
				currentSelection.Destroy();
				Object.Destroy((Object)(object)((Component)preview).get_gameObject());
				SpawnCube();
			}
		}

		private void SpawnCube()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			pb_Object val = pb_ShapeGenerator.CubeGenerator(Vector3.get_one());
			((Component)val).get_gameObject().AddComponent<MeshCollider>().set_convex(false);
			currentSelection = new pb_Selection(val, null);
		}

		public void LateUpdate()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			if (!currentSelection.HasObject())
			{
				return;
			}
			if (Input.GetMouseButtonDown(1) || (Input.GetMouseButtonDown(0) && Input.GetKey((KeyCode)308)))
			{
				mousePosition_initial = Vector2.op_Implicit(Input.get_mousePosition());
				dragging = true;
			}
			if (dragging)
			{
				Vector2 val = Vector2.op_Implicit(Vector2.op_Implicit(mousePosition_initial) - Input.get_mousePosition());
				Vector3 val2 = default(Vector3);
				((Vector3)(ref val2))._002Ector(val.y, val.x, 0f);
				((Component)currentSelection.pb).get_gameObject().get_transform().RotateAround(Vector3.get_zero(), val2, rotateSpeed * Time.get_deltaTime());
				if (currentSelection.IsValid())
				{
					RefreshSelectedFacePreview();
				}
			}
			if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
			{
				dragging = false;
			}
		}

		public void Update()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			if (!Input.GetMouseButtonUp(0) || Input.GetKey((KeyCode)308) || !FaceCheck(Input.get_mousePosition()) || !currentSelection.IsValid())
			{
				return;
			}
			if (!currentSelection.Equals(previousSelection))
			{
				previousSelection = new pb_Selection(currentSelection.pb, currentSelection.face);
				RefreshSelectedFacePreview();
				return;
			}
			Vector3 val = pb_Math.Normal((IList<Vector3>)pbUtil.ValuesWithIndices<Vector3>(currentSelection.pb.get_vertices(), currentSelection.face.get_distinctIndices()));
			if (Input.GetKey((KeyCode)304))
			{
				pb_Object_Utility.TranslateVertices(currentSelection.pb, currentSelection.face.get_distinctIndices(), ((Vector3)(ref val)).get_normalized() * -0.5f);
			}
			else
			{
				pb_Object_Utility.TranslateVertices(currentSelection.pb, currentSelection.face.get_distinctIndices(), ((Vector3)(ref val)).get_normalized() * 0.5f);
			}
			currentSelection.pb.Refresh((RefreshMask)255);
			RefreshSelectedFacePreview();
		}

		public bool FaceCheck(Vector3 pos)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			Ray val = Camera.get_main().ScreenPointToRay(pos);
			RaycastHit val2 = default(RaycastHit);
			if (Physics.Raycast(((Ray)(ref val)).get_origin(), ((Ray)(ref val)).get_direction(), ref val2))
			{
				pb_Object component = ((Component)((RaycastHit)(ref val2)).get_transform()).get_gameObject().GetComponent<pb_Object>();
				if ((Object)(object)component == (Object)null)
				{
					return false;
				}
				Mesh msh = component.get_msh();
				int[] array = new int[3]
				{
					msh.get_triangles()[((RaycastHit)(ref val2)).get_triangleIndex() * 3],
					msh.get_triangles()[((RaycastHit)(ref val2)).get_triangleIndex() * 3 + 1],
					msh.get_triangles()[((RaycastHit)(ref val2)).get_triangleIndex() * 3 + 2]
				};
				currentSelection.pb = component;
				return pb_Object_Utility.FaceWithTriangle(component, array, ref currentSelection.face);
			}
			return false;
		}

		private void RefreshSelectedFacePreview()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Expected O, but got Unknown
			Vector3[] array = pb_Object_Utility.VerticesInWorldSpace(currentSelection.pb, currentSelection.face.get_indices());
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = i;
			}
			Vector3 val = pb_Math.Normal((IList<Vector3>)array);
			for (int j = 0; j < array.Length; j++)
			{
				ref Vector3 reference = ref array[j];
				reference += ((Vector3)(ref val)).get_normalized() * 0.01f;
			}
			if (Object.op_Implicit((Object)(object)preview))
			{
				Object.Destroy((Object)(object)((Component)preview).get_gameObject());
			}
			preview = pb_Object.CreateInstanceWithVerticesFaces(array, (pb_Face[])(object)new pb_Face[1]
			{
				new pb_Face(array2)
			});
			preview.SetFaceMaterial(preview.get_faces(), previewMaterial);
			preview.ToMesh();
			preview.Refresh((RefreshMask)255);
		}

		public RuntimeEdit()
			: this()
		{
		}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)

	}
}
