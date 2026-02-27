using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Singletons", menuName = "EntryPoint/Singletons", order = 10)]
    public sealed class Singletons : ScriptableObject
    {
        [SerializeField] private List<GameObject> _prefabs = new();
        
        [PublicAPI]
        public IReadOnlyList<GameObject> Prefabs => _prefabs;
    }
}