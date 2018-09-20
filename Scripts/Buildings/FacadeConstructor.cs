using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadeConstructor : ScriptableObject, IFacadeConstructor
    {
        public abstract void Construct(List<Vector2> foundationPolygon, List<ILayout> layouts, Transform parentTransform);
    }
}
