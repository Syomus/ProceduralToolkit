using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class RoofConstructionStrategy : ScriptableObject, IRoofConstructionStrategy
    {
        public abstract void Construct(Transform parentTransform, IConstructible<MeshDraft> constructible);
    }
}
