using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadeConstructor
    {
        void Construct(List<Vector2> foundationPolygon, List<ILayout> layouts, Transform parentTransform);
    }
}
