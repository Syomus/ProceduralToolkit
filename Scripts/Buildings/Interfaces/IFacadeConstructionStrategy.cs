using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadeConstructionStrategy
    {
        void Construct(Transform parentTransform, List<Vector2> foundationPolygon, List<ILayout> layouts);
    }
}
