using UnityEngine;

namespace Data
{
    public static class Root
    {
        public static string Path =>
#if UNITY_EDITOR
            System.IO.Path.Combine(Application.dataPath, "Data");
#else
    System.IO.Path.Combine(
        System.IO.Directory.GetParent(Application.dataPath)!.FullName,
        "Data"
    );
#endif
    }
}