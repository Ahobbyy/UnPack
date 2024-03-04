namespace HumanAPI
{
	public interface IGame : IDependency
	{
		bool LevelLoaded(Level level);

		void EnterCheckpoint(int number, int subObjective);

		void EnterPassZone();

		void Fall(HumanBase human, bool drown = false, bool fallAchievement = true);
	}
}
