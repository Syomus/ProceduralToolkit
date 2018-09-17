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
        private float width = 12;
        [SerializeField]
        private float length = 36;
        [SerializeField]
        private BuildingGenerator.Config config = new BuildingGenerator.Config();

        private void Awake()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanningStrategy(facadePlanningStrategy);
            generator.SetFacadeConstructionStrategy(facadeConstructionStrategy);
            generator.SetRoofPlanningStrategy(roofPlanningStrategy);
            generator.SetRoofConstructionStrategy(roofConstructionStrategy);

            var foundationPolygon = new List<Vector2>
            {
                Vector2.left*length/2 + Vector2.down*width/2,
                Vector2.left*length/2 + Vector2.up*width/2,
                Vector2.right*length/2 + Vector2.up*width/2,
                Vector2.right*length/2 + Vector2.down*width/2,
            };
            generator.Generate(foundationPolygon, config, transform);
        }
    }
}
