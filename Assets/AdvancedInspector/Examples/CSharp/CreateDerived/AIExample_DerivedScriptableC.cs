using UnityEngine;

namespace AdvancedInspector
{
    public class AIExample_DerivedScriptableC : AIExample_BaseScriptable
    {
        [SerializeField]
        private string myDerivedStringC;

        [SerializeField, CreateDerived]
        private AIExample_BaseScriptable subDerived;
    }
}
