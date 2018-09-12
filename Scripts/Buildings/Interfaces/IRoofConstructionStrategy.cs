using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IRoofConstructionStrategy
    {
        void Construct(Transform parentTransform, IConstructible<MeshDraft> constructible);
    }
}
