using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadeConstructionStrategy : ScriptableObject, IFacadeConstructionStrategy
    {
        public abstract void Construct(Transform parentTransform, ILayout layout);
    }
}
