using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IRoofConstructionStrategy
    {
        void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform);
    }
}
