using System;
using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Roof Planning Strategy", order = 2)]
    public class ProceduralRoofPlanningStrategy : RoofPlanningStrategy
    {
        public override IConstructible<MeshDraft> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config)
        {
            switch (config.roofConfig.type)
            {
                case RoofType.Flat:
                    return new ProceduralFlatRoof(foundationPolygon, config.roofConfig, config.palette.roofColor);
                case RoofType.Hipped:
                    return new ProceduralHippedRoof(foundationPolygon, config.roofConfig, config.palette.roofColor);
                case RoofType.Gabled:
                    return new ProceduralGabledRoof(foundationPolygon, config.roofConfig, config.palette.roofColor);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
