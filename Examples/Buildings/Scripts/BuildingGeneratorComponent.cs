using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    public class BuildingGeneratorComponent : MonoBehaviour
    {
        [SerializeField]
        private FacadePlanningStrategy facadePlanningStrategy;
        [SerializeField]
        private FacadeConstructionStrategy facadeConstructionStrategy;
        [SerializeField]
        private RoofPlanningStrategy roofPlanningStrategy;
        [SerializeField]
        private RoofConstructionStrategy roofConstructionStrategy;
        [SerializeField]
        private BuildingGenerator.Config config = new BuildingGenerator.Config();
        [SerializeField]
        private List<Vector2> foundationPolygon = Geometry.StarPolygon2(5, 10, 20);

        private void Awake()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanningStrategy(facadePlanningStrategy);
            generator.SetFacadeConstructionStrategy(facadeConstructionStrategy);
            generator.SetRoofPlanningStrategy(roofPlanningStrategy);
            generator.SetRoofConstructionStrategy(roofConstructionStrategy);
            generator.Generate(foundationPolygon, config, transform);
        }
    }
}
