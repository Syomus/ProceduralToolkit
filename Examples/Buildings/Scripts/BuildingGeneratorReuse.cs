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
        private int xCount = 10;
        [SerializeField]
        private int zCount = 10;
        [SerializeField]
        private float cellSize = 15;
        [SerializeField]
        private PolygonAsset foundationPolygon;
        [SerializeField]
        private BuildingGenerator.Config config = new BuildingGenerator.Config();

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
                    var building = generator.Generate(foundationPolygon.vertices, config);
                    building.position = new Vector3(x*cellSize - xOffset, 0, z*cellSize - zOffset);
                }
            }
        }
    }
}
