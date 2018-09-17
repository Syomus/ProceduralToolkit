using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadePlanningStrategy : ScriptableObject, IFacadePlanningStrategy
    {
        public abstract List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
