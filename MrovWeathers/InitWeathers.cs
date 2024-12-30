using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using WeatherRegistry;

namespace MrovWeathers
{
	public static class InitWeathers
	{
		public static Weather Blackout;
		public static Weather Cloudy;

		public static void Init()
		{
			InitCloudy();
			InitBlackout();
		}

		public static void InitCloudy()
		{
			Cloudy = new("Cloudy", new(null, null) { SunAnimatorBool = "overcast" })
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
			WeatherRegistry.WeatherManager.RegisterWeather(Cloudy);
		}

		public static void InitBlackout()
		{
			GameObject blackoutObject = GameObject.Instantiate(new GameObject() { name = "BlackoutWeather" });
			blackoutObject.hideFlags = HideFlags.HideAndDontSave;
			blackoutObject.AddComponent<Blackout>();

			GameObject.DontDestroyOnLoad(blackoutObject);

			var BlackoutAssets = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Plugin.PluginInformation.Location), "blackout"));
			AnimationClip blackoutClip = BlackoutAssets.LoadAsset<AnimationClip>("BlackoutSunClip");

			Blackout = new("Blackout", new(null, blackoutObject) { SunAnimatorBool = "eclipse", })
			{
				// in case i ever forget: screen is fucking green, so green channel *has to* have 20% less value to be gray
				Color = new(r: 0.5f, g: 0.4f, b: 0.5f, a: 1),
				Config =
				{
					ScrapAmountMultiplier = new(0.65f),
					ScrapValueMultiplier = new(1.7f),
					DefaultWeight = new(35),
					LevelWeights = new("Rend@200; Dine@200; Titan@200"),
					WeatherToWeatherWeights = new("None@200; Cloudy@250")
				},
				AnimationClip = blackoutClip
			};
			WeatherRegistry.WeatherManager.RegisterWeather(Blackout);
		}
	}
}
