using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class LevelPhysicsInfo : MonoBehaviour
{
	public class SavedRigidBodyInfo
	{
		public string restartableRigidName;

		public string rigidBodyName;

		public Vector3 pos;

		public Quaternion rot;

		public SavedRigidBodyInfo(string savedRestartableRigidName, string savedRigidBodyName, Vector3 savedPos, Quaternion savedRot)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			restartableRigidName = savedRestartableRigidName;
			rigidBodyName = savedRigidBodyName;
			pos = savedPos;
			rot = savedRot;
		}
	}

	public List<TextAsset> levels = new List<TextAsset>();

	private static Dictionary<string, List<SavedRigidBodyInfo>> levelInfos = new Dictionary<string, List<SavedRigidBodyInfo>>();

	private void Start()
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 savedPos = default(Vector3);
		Quaternion savedRot = default(Quaternion);
		foreach (TextAsset level in levels)
		{
			if (!((Object)(object)level != (Object)null))
			{
				continue;
			}
			List<SavedRigidBodyInfo> list = new List<SavedRigidBodyInfo>();
			string[] array = level.get_text().Split('\n');
			for (int i = 0; i < array.Length - 1; i += 4)
			{
				string text = array[i + 2];
				text = text.Substring(1, text.Length - 3);
				string[] array2 = text.Split(',');
				((Vector3)(ref savedPos))._002Ector(float.Parse(array2[0], CultureInfo.InvariantCulture), float.Parse(array2[1], CultureInfo.InvariantCulture), float.Parse(array2[2], CultureInfo.InvariantCulture));
				text = array[i + 3];
				text = text.Substring(1, text.Length - 3);
				array2 = text.Split(',');
				((Quaternion)(ref savedRot))._002Ector(float.Parse(array2[0], CultureInfo.InvariantCulture), float.Parse(array2[1], CultureInfo.InvariantCulture), float.Parse(array2[2], CultureInfo.InvariantCulture), float.Parse(array2[3], CultureInfo.InvariantCulture));
				list.Add(new SavedRigidBodyInfo(array[i].TrimEnd(), array[i + 1].TrimEnd(), savedPos, savedRot));
			}
			for (int j = 0; j < list.Count - 1; j++)
			{
				for (int k = j + 1; k < list.Count; k++)
				{
					if (list[j].restartableRigidName == list[k].restartableRigidName && list[j].rigidBodyName == list[k].rigidBodyName)
					{
						Debug.Log((object)("duplicate " + ((Object)level).get_name() + " " + list[j].restartableRigidName));
					}
				}
			}
			levelInfos[((Object)level).get_name()] = list;
		}
	}

	public static void SetSavedRigidBodies(string levelName)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (!levelInfos.ContainsKey(levelName))
		{
			return;
		}
		List<SavedRigidBodyInfo> list = levelInfos[levelName];
		if (list == null)
		{
			return;
		}
		foreach (SavedRigidBodyInfo item in list)
		{
			GameObject val = GameObject.Find(item.restartableRigidName);
			if ((Object)(object)val != (Object)null)
			{
				RestartableRigid component = val.GetComponent<RestartableRigid>();
				if ((Object)(object)component != (Object)null)
				{
					component.SetRecordedInfo(item.rigidBodyName, item.pos, item.rot);
				}
			}
		}
	}

	public LevelPhysicsInfo()
		: this()
	{
	}
}
