using System.IO;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class StreamingAssetsHelper
    {
        public static Config GetConfig()
        {
            var json = File.ReadAllText($"{Application.streamingAssetsPath}/config.json");
            return JsonUtility.FromJson<Config>(json);
        }

        public static void SaveConfig(Config config)
        {
            var json = JsonUtility.ToJson(config);
            File.WriteAllText($"{Application.streamingAssetsPath}/config.json", json);
        }
    }
}
