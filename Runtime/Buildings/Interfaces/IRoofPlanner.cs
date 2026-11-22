using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IRoofPlanner
    {
        IConstructible<MeshDraft> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
