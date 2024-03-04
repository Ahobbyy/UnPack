using UnityEngine;

namespace HumanAPI
{
	public class JamDoorAchievement : MonoBehaviour
	{
		public GapJoint door;

		private Collider trackedHuman;

		private float entryX;

		public void OnTriggerEnter(Collider other)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (((Component)other).get_tag() == "Player")
			{
				ServoMotor component = ((Component)door).GetComponent<ServoMotor>();
				if (!((Object)(object)component == (Object)null) && !(component.input.value > 0.5f) && CheckLevelName("Carry"))
				{
					Vector3 val = ((Component)this).get_transform().InverseTransformPoint(((Component)other).get_transform().get_position());
					entryX = val.x;
					trackedHuman = other;
				}
			}
		}

		public void OnTriggerExit(Collider other)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)other == (Object)(object)trackedHuman && ((Component)this).get_transform().InverseTransformPoint(((Component)other).get_transform().get_position()).x * entryX < 0f)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_CARRY_JAM_DOOR);
			}
		}

		public void OnTriggerStay(Collider other)
		{
			if ((Object)(object)trackedHuman != (Object)null)
			{
				ServoMotor component = ((Component)door).GetComponent<ServoMotor>();
				if ((Object)(object)component == (Object)null || component.input.value > 0.5f)
				{
					trackedHuman = null;
				}
			}
		}

		public static bool CheckLevelName(string whatWeNeed)
		{
			if ((Object)(object)Game.instance == (Object)null)
			{
				return false;
			}
			int currentLevelNumber = Game.instance.currentLevelNumber;
			string[] levels = Game.instance.levels;
			if (currentLevelNumber < 0 || currentLevelNumber >= levels.Length)
			{
				return false;
			}
			string text = levels[currentLevelNumber];
			if (!string.IsNullOrEmpty(text))
			{
				return whatWeNeed == text;
			}
			return false;
		}

		public JamDoorAchievement()
			: this()
		{
		}
	}
}
