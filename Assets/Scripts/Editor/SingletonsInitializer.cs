using Data;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Editor
{
    [InitializeOnLoad]
    public static class SingletonsInitializer
    {
        private static readonly string LogPrefix = $"<color=yellow>[{nameof(SingletonsInitializer)}]</color>";
        
        static SingletonsInitializer()
        {
            EditorApplication.delayCall += CheckAndCreateSingletons;
        }

        [MenuItem("Tools/Project Setup/Force Check Singletons")]
        private static void CheckAndCreateSingletons()
        {
            FastLog.Log($"{LogPrefix} Checking for singletons");
            
            if (AssetDatabase.LoadAssetAtPath<Singletons>(Config.SingletonsPath) is not null)
                return;

            FastLog.Log($"{LogPrefix} Asset not found. Starting creation process...");
            
            EnsureFoldersRecursive(Config.FolderDataPath);
            
            var asset = ScriptableObject.CreateInstance<Singletons>();
            
            try 
            {
                AssetDatabase.CreateAsset(asset, Config.SingletonsPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                FastLog.Log($"{LogPrefix} Asset successfully created: <b>{Config.SingletonsPath}</b>");
            }
            catch (System.Exception exception)
            {
                FastLog.Error($"{LogPrefix} Error saving asset: {exception.Message}");
            }
        }

        private static void EnsureFoldersRecursive(string fullPath)
        {
            var folders = fullPath.Split('/');
            var currentPath = folders[0];

            for (var i = 1; i < folders.Length; i++)
            {
                var folderName = folders[i];
                var nextPath = $"{currentPath}/{folderName}";

                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    var guid = AssetDatabase.CreateFolder(currentPath, folderName);
                    
                    if (!string.IsNullOrEmpty(guid))
                    {
                        FastLog.Log($"{LogPrefix} Folder created: <b>{nextPath}</b>");
                        continue;
                    }

                    FastLog.Error($"{LogPrefix} Unable to create folder: {nextPath}");
                }
                
                currentPath = nextPath;
            }
        }
    }
}