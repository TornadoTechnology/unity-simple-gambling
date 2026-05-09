using Data;
using Data.Objects;
using UnityEngine;
using Utilities;

using UnityResources = UnityEngine.Resources;

public static class EntryPoint
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        FastLog.Clear();
        FastLog.Log("Initialization....");
        
        LoadSingletons();
        LoadResources();
    }
    
    private static void LoadSingletons()
    {
        var singletons = UnityResources.Load<Singletons>(Config.SingletonsResourcesPath);
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

    private static void LoadResources()
    {
        ResourceContainer.Load();
        ResourceGameContainer.Load();
    }
}

