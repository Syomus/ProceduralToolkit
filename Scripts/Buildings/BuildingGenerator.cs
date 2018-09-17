using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Buildings
{
    public class BuildingGenerator
    {
        private IFacadePlanningStrategy facadePlanningStrategy;
        private IFacadeConstructionStrategy facadeConstructionStrategy;
        private IRoofPlanningStrategy roofPlanningStrategy;
        private IRoofConstructionStrategy roofConstructionStrategy;

        public void SetFacadePlanningStrategy(IFacadePlanningStrategy facadePlanningStrategy)
        {
            this.facadePlanningStrategy = facadePlanningStrategy;
        }

        public void SetFacadeConstructionStrategy(IFacadeConstructionStrategy facadeConstructionStrategy)
        {
            this.facadeConstructionStrategy = facadeConstructionStrategy;
        }

        public void SetRoofPlanningStrategy(IRoofPlanningStrategy roofPlanningStrategy)
        {
            this.roofPlanningStrategy = roofPlanningStrategy;
        }

        public void SetRoofConstructionStrategy(IRoofConstructionStrategy roofConstructionStrategy)
        {
            this.roofConstructionStrategy = roofConstructionStrategy;
        }

        public Transform Generate(List<Vector2> foundationPolygon, Config config, Transform parent = null)
        {
            Assert.IsTrue(config.floors > 0);
            Assert.IsTrue(config.entranceInterval > 0);

            List<ILayout> facadeLayouts = facadePlanningStrategy.Plan(foundationPolygon, config);
            float height = facadeLayouts[0].height;

            if (parent == null)
            {
                parent = new GameObject("Building").transform;
            }
            facadeConstructionStrategy.Construct(foundationPolygon, facadeLayouts, parent);

            if (roofPlanningStrategy != null && roofConstructionStrategy != null)
            {
                var roofLayout = roofPlanningStrategy.Plan(foundationPolygon, config);

                var roof = new GameObject("Roof").transform;
                roof.SetParent(parent, false);
                roof.localPosition = new Vector3(0, height, 0);
                roof.localRotation = Quaternion.identity;
                roofConstructionStrategy.Construct(roofLayout, roof);
            }
            return parent;
        }

        [Serializable]
        public class Config
        {
            public int floors = 5;
            public float entranceInterval = 12;
            public bool hasAttic = true;
            public RoofConfig roofConfig = new RoofConfig
            {
                type = RoofType.Flat,
                thickness = 0.2f,
                overhang = 0.2f,
            };
            public Palette palette = new Palette();
        }
    }

    [Serializable]
    public class RoofConfig
    {
        public RoofType type = RoofType.Flat;
        public float thickness;
        public float overhang;
    }

    [Serializable]
    public class Palette
    {
        public Color socleColor = ColorE.silver;
        public Color socleWindowColor = (ColorE.silver/2).WithA(1);
        public Color doorColor = (ColorE.silver/2).WithA(1);
        public Color wallColor = ColorE.white;
        public Color frameColor = ColorE.silver;
        public Color glassColor = ColorE.white;
        public Color roofColor = (ColorE.gray/4).WithA(1);
    }

    public enum RoofType
    {
        Flat,
        Hipped,
        Gabled,
    }
}
