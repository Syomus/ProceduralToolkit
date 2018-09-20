using ProceduralToolkit.Buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProceduralToolkit.Examples.Buildings
{
    public class BuildingGeneratorComponent : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("facadePlanningStrategy")]
        private FacadePlanner facadePlanner;
        [SerializeField, FormerlySerializedAs("facadeConstructionStrategy")]
        private FacadeConstructor facadeConstructor;
        [SerializeField, FormerlySerializedAs("roofPlanningStrategy")]
        private RoofPlanner roofPlanner;
        [SerializeField, FormerlySerializedAs("roofConstructionStrategy")]
        private RoofConstructor roofConstructor;
        [SerializeField]
        private PolygonAsset foundationPolygon;
        [SerializeField]
        private BuildingGenerator.Config config = new BuildingGenerator.Config();

        private void Awake()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanner(facadePlanner);
            generator.SetFacadeConstructor(facadeConstructor);
            generator.SetRoofPlanner(roofPlanner);
            generator.SetRoofConstructor(roofConstructor);
            generator.Generate(foundationPolygon.vertices, config, transform);
        }
    }
}
