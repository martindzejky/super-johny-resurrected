using UnityEngine;

namespace AdvancedInspector
{
    [CreateAssetMenu]
    public class AIExample_ScriptableComponent : ScriptableObject
    {
        [SerializeField, CreateDerived]
        private AIExample_BaseScriptable derived;
    }
}
