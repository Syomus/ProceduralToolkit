using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public abstract class FacadePlanner : ScriptableObject, IFacadePlanner
    {
        public abstract List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
