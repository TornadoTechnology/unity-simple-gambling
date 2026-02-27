using Data;
using UnityEditor;
using UnityEngine;
using Utilities;

public static class EntryPoint
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        FastLog.Clear();
        FastLog.Log("Initialization....");
        
        var singletons = AssetDatabase.LoadAssetAtPath<Singletons>(Config.SingletonsPath);
        if (singletons is null)
        {
            FastLog.Error($"Singletons not found, at path: {Config.SingletonsPath}");
            return;
        }
        
        foreach (var prefab in singletons.Prefabs)
        {
            Object.Instantiate(prefab);
            
            FastLog.Log($"Singleton: {prefab.name}");
        }
    }
}

