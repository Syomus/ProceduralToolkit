using UnityEngine;
using System.Collections.Generic;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadePlanner
    {
        List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
