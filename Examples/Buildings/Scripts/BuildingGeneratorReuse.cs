using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    public class BuildingGeneratorReuse : MonoBehaviour
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
        private Config config = new Config();

        private const int xCount = 10;
        private const int zCount = 10;
        private const float cellSize = 15;

        private void Awake()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanningStrategy(facadePlanningStrategy);
            generator.SetFacadeConstructionStrategy(facadeConstructionStrategy);
            generator.SetRoofPlanningStrategy(roofPlanningStrategy);
            generator.SetRoofConstructionStrategy(roofConstructionStrategy);

            float xOffset = (xCount - 1)*0.5f*cellSize;
            float zOffset = (zCount - 1)*0.5f*cellSize;
            for (int z = 0; z < zCount; z++)
            {
                for (int x = 0; x < xCount; x++)
                {
                    config.roofConfig.type = RandomE.GetRandom(RoofType.Flat, RoofType.Hipped, RoofType.Gabled);
                    var building = generator.Generate(config);
                    building.position = new Vector3(x*cellSize - xOffset, 0, z*cellSize - zOffset);
                }
            }
        }
    }
}
