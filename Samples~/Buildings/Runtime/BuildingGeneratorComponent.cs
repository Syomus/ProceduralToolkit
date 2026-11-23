using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Samples.Buildings
{
    public class BuildingGeneratorComponent : MonoBehaviour
    {
        [SerializeField]
        private FacadePlanner facadePlanner;
        [SerializeField]
        private FacadeConstructor facadeConstructor;
        [SerializeField]
        private RoofPlanner roofPlanner;
        [SerializeField]
        private RoofConstructor roofConstructor;
        [SerializeField]
        private PolygonAsset foundationPolygon;
        [SerializeField]
        private BuildingGenerator.Config config = new();

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
