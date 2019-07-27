using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IRoofConstructor
    {
        void Construct(IConstructible<MeshDraft> constructible, Transform parentTransform);
    }
}
