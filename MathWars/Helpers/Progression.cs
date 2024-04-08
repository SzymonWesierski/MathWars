using MathWars.Entities;

namespace MathWars.Helpers
{
	public static class Progression
	{
		public static ApplicationUser GetHowManyLevelsReached(
			ApplicationUser user, int lvlMultplier)
		{
			while (user.Experience >= user.ExpToReachNewLvl)
			{
				user.Experience -= user.ExpToReachNewLvl;
				user.Level += 1;
				user.ExpToReachNewLvl = user.Level * lvlMultplier;//Exp to reach new level pattern
			}
			return user;
		}

		public static ApplicationUser GetHowManyExperienceReached(
			ApplicationUser user, int difficultyLevel, int expMultiplier)
		{
			user.Experience += GetExp(difficultyLevel, expMultiplier);
			return user;
		}

		public static int GetExp(int difficultyLevel, int expMultiplier)
		{
			return (difficultyLevel * expMultiplier);// exp points pattern
		}
	}
}
