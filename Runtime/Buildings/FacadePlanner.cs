using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadePlanner : ScriptableObject, IFacadePlanner
    {
        public abstract List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
