using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class RoofConstructionStrategy : ScriptableObject, IRoofConstructionStrategy
    {
        public abstract void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform);
    }
}
