using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MrovWeathers
{
	public class LightUtils
	{
		// Get all lights in a specific scene
		public static List<Light> GetLightsInScene(string sceneName)
		{
			List<Light> sceneLights = [];
			Scene scene = SceneManager.GetSceneByName(sceneName);

			if (!scene.isLoaded)
			{
				Plugin.DebugLogger.LogWarning($"Scene {sceneName} is not loaded!");
				return sceneLights;
			}

			GameObject[] rootObjects = scene.GetRootGameObjects();
			foreach (GameObject root in rootObjects)
			{
				// Get lights in children of root objects
				Light[] childLights = root.GetComponentsInChildren<Light>(true);
				sceneLights.AddRange(childLights);
			}

			return sceneLights;
		}

		// Get lights under a specific parent transform
		public static List<Light> GetLightsUnderParent(Transform parent, bool includeInactive = true)
		{
			List<Light> lights = [];
			if (parent == null)
				return lights;

			Light[] childLights = parent.GetComponentsInChildren<Light>(includeInactive);
			lights.AddRange(childLights);

			return lights;
		}

		// Get lights in specific scene and under specific parent
		public static List<Light> GetLightsInSceneUnderParent(string sceneName, string parentName, bool includeInactive = true)
		{
			List<Light> lights = [];
			Scene scene = SceneManager.GetSceneByName(sceneName);

			if (!scene.isLoaded)
			{
				Plugin.DebugLogger.LogWarning($"Scene {sceneName} is not loaded!");
				return lights;
			}

			GameObject[] rootObjects = scene.GetRootGameObjects();
			foreach (GameObject root in rootObjects)
			{
				if (root.name == parentName)
				{
					lights.AddRange(GetLightsUnderParent(root.transform, includeInactive));
					break;
				}

				// Search in children if not found at root
				Transform parent = root.transform.Find(parentName);
				if (parent != null)
				{
					lights.AddRange(GetLightsUnderParent(parent, includeInactive));
					break;
				}
			}

			return lights;
		}

		// Get all lights in multiple scenes
		public static Dictionary<string, List<Light>> GetLightsInScenes(string[] sceneNames)
		{
			Dictionary<string, List<Light>> sceneLights = [];

			foreach (string sceneName in sceneNames)
			{
				sceneLights[sceneName] = GetLightsInScene(sceneName);
			}

			return sceneLights;
		}

		// The previous methods remain unchanged...
		public static Light[] GetAllLightsInScene(bool includeInactive = true)
		{
			return Resources.FindObjectsOfTypeAll<Light>();
		}

		public static Light[] GetActiveLightsInScene()
		{
			return GameObject.FindObjectsOfType<Light>();
		}

		public static List<Light> GetLightsByType(LightType lightType)
		{
			Light[] allLights = GetAllLightsInScene();
			List<Light> filteredLights = [];

			foreach (Light light in allLights)
			{
				if (light.type == lightType)
				{
					filteredLights.Add(light);
				}
			}

			return filteredLights;
		}
	}
}
