using MathWars.Interfaces;

namespace MathWars.Services
{
	public class ConfigurationService : IConfigurationService
	{
		private readonly IConfiguration _configuration;

		public ConfigurationService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public int GetExperienceMultiplier()
		{
			var expMultiplierSection = _configuration.GetSection("Experience&Level");
			if (expMultiplierSection.Exists())
			{
				return expMultiplierSection.GetValue<int>("experienceMultiplier");
			}
			return 0;
		}

		public int GetLevelMultiplier()
		{
			var lvlMultiplierSection = _configuration.GetSection("Experience&Level");
			if (lvlMultiplierSection.Exists())
			{
				return lvlMultiplierSection.GetValue<int>("lvlMultiplier");
			}
			return 0;
		}
	}
}
