using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public abstract class RoofPlanner : ScriptableObject, IRoofPlanner
    {
        public abstract IConstructible<MeshDraft> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
