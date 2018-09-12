using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadeConstructionStrategy
    {
        void Construct(Transform parentTransform, ILayout layout);
    }
}
