using System.IO;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using WeatherRegistry;

namespace MrovWeathers
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency(WeatherRegistry.Plugin.GUID, BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static MrovLib.Logger DebugLogger = new(PluginInfo.PLUGIN_GUID);
		internal static Harmony harmony = new(PluginInfo.PLUGIN_GUID);

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			Weather cloudyWeather =
				new("Cloudy", new(null, null) { SunAnimatorBool = "overcast" })
				{
					Color = new(r: 0, g: 1f, b: 0.55f, a: 1),
					Config =
					{
						ScrapAmountMultiplier = new(1.6f),
						ScrapValueMultiplier = new(0.8f),
						WeatherToWeatherWeights = new(["Eclipsed@50", "Stormy@80"]),
						DefaultWeight = new(25),
					},
				};
			WeatherRegistry.WeatherManager.RegisterWeather(cloudyWeather);

			GameObject blackoutObject = GameObject.Instantiate(new GameObject() { name = "BlackoutWeather" });
			blackoutObject.hideFlags = HideFlags.HideAndDontSave;
			blackoutObject.AddComponent<Blackout>();
			GameObject.DontDestroyOnLoad(blackoutObject);

			var BlackoutAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Info.Location), "blackout"));
			AnimationClip blackoutClip = BlackoutAssets.LoadAsset<AnimationClip>("BlackoutSunClip");

			Weather Blackout =
				new("Blackout", new(null, blackoutObject) { SunAnimatorBool = "eclipse", })
				{
					// in case i ever forget: screen is fucking green, so green channel *has to* have 20% less value to be gray
					Color = new(r: 0.5f, g: 0.4f, b: 0.5f, a: 1),
					Config =
					{
						ScrapAmountMultiplier = new(0.65f),
						ScrapValueMultiplier = new(1.7f),
						DefaultWeight = new(25),
						LevelWeights = new("Rend@200; Dine@200; Titan@200"),
						WeatherToWeatherWeights = new("None@200; Cloudy@250")
					},
					AnimationClip = blackoutClip
				};
			WeatherRegistry.WeatherManager.RegisterWeather(Blackout);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
