using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadeConstructor
    {
        void Construct(List<Vector2> foundationPolygon, List<ILayout> layouts, Transform parentTransform);
    }
}
