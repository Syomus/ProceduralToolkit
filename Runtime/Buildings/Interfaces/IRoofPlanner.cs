using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface IRoofPlanner
    {
        IConstructible<MeshDraft> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
