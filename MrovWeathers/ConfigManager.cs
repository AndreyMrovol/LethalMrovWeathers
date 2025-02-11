using BepInEx.Configuration;
using WeatherRegistry;

namespace MrovWeathers
{
	public class LevelListConfigHandler : WeatherRegistry.LevelListConfigHandler
	{
		public LevelListConfigHandler(string defaultValue, bool enabled = true)
			: base(defaultValue, enabled)
		{
			// Any additional initialization for the derived class can be done here
		}

		public void CreateConfigEntry(string configTitle, ConfigDescription configDescription = null)
		{
			ConfigEntry = ConfigManager.configFile.Bind($"Foggy", configTitle, DefaultValue, configDescription);
		}
	}

	public class ConfigManager
	{
		public static ConfigManager Instance { get; private set; }

		public static void Init(ConfigFile config)
		{
			Instance = new ConfigManager(config);
		}

		internal static ConfigFile configFile;

		public static LevelListConfigHandler FoggyIgnoreLevels;

		private ConfigManager(ConfigFile config)
		{
			configFile = config;

			FoggyIgnoreLevels = new LevelListConfigHandler("", false);
			FoggyIgnoreLevels.CreateConfigEntry(
				"Foggy weather override",
				new ConfigDescription("Levels to blacklist fog override from applying on (semicolon-separated)")
			);
		}
	}
}
