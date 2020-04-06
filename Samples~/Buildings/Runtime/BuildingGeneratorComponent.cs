using ProceduralToolkit.Buildings;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProceduralToolkit.Samples.Buildings
{
    public class BuildingGeneratorComponent : MonoBehaviour
    {
        [SerializeField, FormerlySerializedAs("facadePlanningStrategy")]
        private FacadePlanner facadePlanner = null;
        [SerializeField, FormerlySerializedAs("facadeConstructionStrategy")]
        private FacadeConstructor facadeConstructor = null;
        [SerializeField, FormerlySerializedAs("roofPlanningStrategy")]
        private RoofPlanner roofPlanner = null;
        [SerializeField, FormerlySerializedAs("roofConstructionStrategy")]
        private RoofConstructor roofConstructor = null;
        [SerializeField]
        private PolygonAsset foundationPolygon = null;
        [SerializeField]
        private BuildingGenerator.Config config = new BuildingGenerator.Config();

        private void Awake()
        {
            Generate();
        }

        public Transform Generate()
        {
            var generator = new BuildingGenerator();
            generator.SetFacadePlanner(facadePlanner);
            generator.SetFacadeConstructor(facadeConstructor);
            generator.SetRoofPlanner(roofPlanner);
            generator.SetRoofConstructor(roofConstructor);
            return generator.Generate(foundationPolygon.vertices, config);
        }
    }
}
