using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadeConstructionStrategy : ScriptableObject, IFacadeConstructionStrategy
    {
        public abstract void Construct(Transform parentTransform, List<Vector2> foundationPolygon, List<ILayout> layouts);
    }
}
