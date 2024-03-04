using UnityEngine;

public class MeshHooks : MonoBehaviour
{
	public Transform[] hooks;

	private Vector3[] vertexPos;

	private Vector3[] verts;

	private int[] vertexHook;

	private Matrix4x4[] matrices;

	private Mesh mesh;

	private MeshFilter meshFilter;

	private void Awake()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		meshFilter = ((Component)this).GetComponent<MeshFilter>();
		mesh = meshFilter.get_mesh();
		vertexPos = mesh.get_vertices();
		vertexHook = new int[vertexPos.Length];
		Vector3[] array = (Vector3[])(object)new Vector3[hooks.Length];
		for (int i = 0; i < hooks.Length; i++)
		{
			array[i] = hooks[i].get_position();
		}
		for (int j = 0; j < vertexPos.Length; j++)
		{
			Vector3 val = ((Component)this).get_transform().TransformPoint(vertexPos[j]);
			float num = float.MaxValue;
			int num2 = 0;
			for (int k = 0; k < array.Length; k++)
			{
				Vector3 val2 = array[k] - val;
				float sqrMagnitude = ((Vector3)(ref val2)).get_sqrMagnitude();
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					num2 = k;
				}
			}
			vertexPos[j] = hooks[num2].InverseTransformPoint(val);
			vertexHook[j] = num2;
		}
		verts = (Vector3[])(object)new Vector3[vertexPos.Length];
		matrices = (Matrix4x4[])(object)new Matrix4x4[hooks.Length];
	}

	private void LateUpdate()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < hooks.Length; i++)
		{
			matrices[i] = ((Component)this).get_transform().get_worldToLocalMatrix() * hooks[i].get_localToWorldMatrix();
		}
		for (int j = 0; j < vertexPos.Length; j++)
		{
			verts[j] = ((Matrix4x4)(ref matrices[vertexHook[j]])).MultiplyPoint3x4(vertexPos[j]);
		}
		mesh.set_vertices(verts);
		mesh.RecalculateBounds();
		meshFilter.set_sharedMesh(mesh);
	}

	public MeshHooks()
		: this()
	{
	}
}
