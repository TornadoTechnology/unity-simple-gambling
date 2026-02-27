using System.Diagnostics;
using UnityEngine;

namespace Utilities
{
    public class FastLog
    {
        [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
        public static void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
        public static void Warning(string message, Object context = null)
        {
            UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
        public static void Error(string message, Object context = null)
        {
            UnityEngine.Debug.LogError(message, context);
        }
    
        [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
        public static void LogFormat(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }
        
        [Conditional("LOG_ENABLED"), Conditional("UNITY_EDITOR")]
        public static void Clear()
        {
            UnityEngine.Debug.ClearDeveloperConsole();
        }
    }
}