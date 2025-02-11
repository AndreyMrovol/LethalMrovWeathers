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

		internal static BepInEx.PluginInfo PluginInformation;

		private void Awake()
		{
			logger = Logger;
			harmony.PatchAll();

			ConfigManager.Init(Config);
			PluginInformation = Info;

			InitWeathers.Init();

			WeatherRegistry.EventManager.ShipLanding.AddListener(args => FoggyPatches.ToggleFogExclusionZones(args.level, false));
			WeatherRegistry.EventManager.DisableAllWeathers.AddListener(
				() => FoggyPatches.ToggleFogExclusionZones(StartOfRound.Instance.currentLevel, true)
			);
			WeatherRegistry.EventManager.SetupFinished.AddListener(() => FoggyPatches.CreateEffectOverrides());

			// Plugin startup logic
			Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
