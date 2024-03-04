using System.Collections.Generic;
using UnityEngine;

public class RigVolumeMesh : MonoBehaviour
{
	public struct Face
	{
		public Vector3 n;

		public float d;
	}

	public List<Face> faces = new List<Face>();

	public void Build(float normalSign)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		faces.Clear();
		MeshFilter component = ((Component)this).GetComponent<MeshFilter>();
		if ((Object)(object)component == (Object)null)
		{
			return;
		}
		Mesh sharedMesh = component.get_sharedMesh();
		if ((Object)(object)sharedMesh == (Object)null)
		{
			return;
		}
		int[] triangles = sharedMesh.get_triangles();
		Vector3[] vertices = sharedMesh.get_vertices();
		Vector3[] normals = sharedMesh.get_normals();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			Vector3 val = ((Component)this).get_transform().TransformDirection(normals[triangles[i]]) * normalSign;
			float num = Vector3.Dot(val, ((Component)this).get_transform().TransformPoint(vertices[triangles[i]]));
			bool flag = false;
			for (int j = 0; j < faces.Count; j++)
			{
				Vector3 val2 = faces[j].n - val;
				if (Mathf.Abs(((Vector3)(ref val2)).get_magnitude()) < 0.001f && Mathf.Abs(faces[j].d - num) < 0.001f)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				faces.Add(new Face
				{
					n = val,
					d = num
				});
			}
		}
	}

	public float GetDistInside(Vector3 pos)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		for (int i = 0; i < faces.Count; i++)
		{
			Face face = faces[i];
			float num2 = Vector3.Dot(face.n, pos) - face.d;
			if (num2 > 0f)
			{
				return 0f;
			}
			if (0f - num2 < num)
			{
				num = 0f - num2;
			}
		}
		return num;
	}

	public float GetDistOutside(Vector3 pos)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		float num2 = 0f;
		for (int i = 0; i < faces.Count; i++)
		{
			Face face = faces[i];
			float num3 = Vector3.Dot(face.n, pos) - face.d;
			if (num3 > num2)
			{
				num2 = num3;
				num = i;
			}
		}
		if (num2 == 0f)
		{
			return 0f;
		}
		Face face2 = faces[num];
		Vector3 val = pos - face2.n * num2;
		for (int j = 0; j < faces.Count; j++)
		{
			if (j != num)
			{
				Face face3 = faces[j];
				float num4 = Vector3.Dot(face3.n, pos) - face3.d;
				if (!(num4 <= 0f))
				{
					Vector3 val2 = face3.n - face2.n * Vector3.Dot(face3.n, face2.n);
					float num5 = Vector3.Dot(val2, face3.n);
					val += val2 * num4 / num5;
				}
			}
		}
		Vector3 val3 = pos - val;
		return ((Vector3)(ref val3)).get_magnitude();
	}

	public RigVolumeMesh()
		: this()
	{
	}
}
