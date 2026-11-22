using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Buildings
{
    public interface IFacadePlanner
    {
        List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config);
    }
}
