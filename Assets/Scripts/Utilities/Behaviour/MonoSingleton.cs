using UnityEngine;

namespace Utilities.Behaviour
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (!Instance)
            {
                Instance = (T) this;
                DontDestroyOnLoad(this);
                return;
            }

            DestroyImmediate(gameObject);
        }
    }
}