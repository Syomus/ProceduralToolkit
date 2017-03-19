using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// Fully procedural building generator
    /// </summary>
    public class BuildingGenerator : BuildingGeneratorBase
    {
        private const float socleHeight = 1;
        private const float floorHeight = 2.5f;
        private const float atticHeight = 1;

        private Dictionary<PanelType, List<Func<IFacadePanel>>> constructors =
            new Dictionary<PanelType, List<Func<IFacadePanel>>>();
        private Dictionary<PanelType, Func<IFacadePanel>> commonConstructors =
            new Dictionary<PanelType, Func<IFacadePanel>>();

        private Dictionary<PanelSize, float> sizeValues = new Dictionary<PanelSize, float>
        {
            {PanelSize.Narrow, 2.5f},
            {PanelSize.Wide, 3},
        };

        public MeshDraft Generate(Config config)
        {
            Assert.IsTrue(config.width > 0);
            Assert.IsTrue(config.length > 0);
            Assert.IsTrue(config.floors > 0);
            Assert.IsTrue(config.entranceInterval > 0);

            InitializeConstructors(config.palette);

            var foundationPolygon = new List<Vector2>
            {
                Vector2.left*config.length/2 + Vector2.down*config.width/2,
                Vector2.right*config.length/2 + Vector2.down*config.width/2,
                Vector2.right*config.length/2 + Vector2.up*config.width/2,
                Vector2.left*config.length/2 + Vector2.up*config.width/2
            };

            commonConstructors[PanelType.Entrance] = constructors[PanelType.Entrance].GetRandom();
            commonConstructors[PanelType.EntranceWindow] = constructors[PanelType.EntranceWindow].GetRandom();

            var facadeLayouts = new List<FacadeLayout>();
            for (int i = 0; i < foundationPolygon.Count; i++)
            {
                Vector2 a = foundationPolygon[i];
                Vector2 b = foundationPolygon.GetLooped(i + 1);
                float? entranceInterval = i == 0 ? config.entranceInterval : (float?) null;
                bool hasBalconies = RandomE.Chance(0.5f);
                facadeLayouts.Add(GenerateFacade(a, b, config.floors, hasBalconies, config.hasAttic, entranceInterval));
            }

            float facadeHeight = floorHeight*config.floors + socleHeight + (config.hasAttic ? atticHeight : 0);

            var buildingDraft = GenerateFacades(foundationPolygon, facadeLayouts);
            buildingDraft.uv.Clear();

            var roof = RoofGenerator.Generate(foundationPolygon, facadeHeight, config.roofConfig);
            roof.Paint(config.palette.roofColor);
            buildingDraft.Add(roof);

            return buildingDraft;
        }

        private void InitializeConstructors(Palette palette)
        {
            constructors[PanelType.Wall] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralWall(palette.wallColor)
            };
            constructors[PanelType.Window] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralWindow(palette.wallColor, palette.frameColor, palette.glassColor)
            };
            constructors[PanelType.Balcony] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralBalcony(palette.wallColor, palette.frameColor, palette.glassColor),
                () => new ProceduralBalconyGlazed(palette.wallColor, palette.frameColor, palette.glassColor,
                    palette.roofColor)
            };
            constructors[PanelType.Entrance] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralEntrance(palette.wallColor, palette.doorColor),
                () => new ProceduralEntranceRoofed(palette.wallColor, palette.doorColor, palette.roofColor)
            };
            constructors[PanelType.EntranceWindow] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralEntranceWindow(palette.wallColor, palette.frameColor, palette.glassColor),
                () => new ProceduralWindow(palette.wallColor, palette.frameColor, palette.glassColor)
            };
            constructors[PanelType.EntranceWallLast] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralWall(palette.wallColor)
            };
            constructors[PanelType.Socle] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralSocle(palette.socleColor),
                () => new ProceduralSocleWindowed(palette.socleColor, palette.socleWindowColor)
            };
            constructors[PanelType.Attic] = new List<Func<IFacadePanel>>
            {
                () => new ProceduralAtticVented(palette.wallColor, palette.roofColor),
                () => new ProceduralWall(palette.wallColor)
            };
        }

        private FacadeLayout GenerateFacade(
            Vector2 a,
            Vector2 b,
            int floors,
            bool hasBalconies,
            bool hasAttic,
            float? entranceInterval)
        {
            float facadeWidth = (b - a).magnitude;
            List<PanelSize> panelSizes = DivideFacade(sizeValues, facadeWidth);

            var facadeLayout = GenerateFacadeChunk(facadeWidth, panelSizes, floors, hasBalconies, hasAttic,
                entranceInterval);
            facadeLayout.origin = Vector2.zero;
            return facadeLayout;
        }

        private FacadeLayout GenerateFacadeChunk(
            float facadeWidth,
            List<PanelSize> panelSizes,
            int floors,
            bool hasBalconies,
            bool hasAttic,
            float? entranceInterval)
        {
            var facadeLayout = new VerticalLayout();
            float facadeHeight = 0;

            if (entranceInterval.HasValue)
            {
                int entranceCount = Mathf.Max(Mathf.FloorToInt(facadeWidth/entranceInterval.Value) - 1, 1);
                int entranceIndexInterval = (panelSizes.Count - entranceCount)/(entranceCount + 1);

                var horizontal = new HorizontalLayout();
                int lastEntranceIndex = -1;
                for (int i = 0; i < entranceCount; i++)
                {
                    int entranceIndex = (i + 1)*entranceIndexInterval + i;

                    horizontal.Add(ConstructFacadeChunk(panelSizes, lastEntranceIndex + 1, entranceIndex, floors,
                        hasBalconies));

                    horizontal.Add(ConstructEntranceVertical(sizeValues[panelSizes[entranceIndex]], floors));

                    if (i == entranceCount - 1)
                    {
                        horizontal.Add(ConstructFacadeChunk(panelSizes, entranceIndex + 1, panelSizes.Count, floors,
                            hasBalconies));
                    }

                    lastEntranceIndex = entranceIndex;
                }

                facadeLayout.Add(horizontal);
                facadeHeight += socleHeight + floors*floorHeight;
            }
            else
            {
                var socle = ConstructHorizontal(
                    constructors: constructors[PanelType.Socle],
                    height: socleHeight,
                    getPanelWidth: index => sizeValues[panelSizes[index]],
                    count: panelSizes.Count);

                facadeLayout.Add(socle);
                facadeHeight += socleHeight;

                for (int floorIndex = 0; floorIndex < floors; floorIndex++)
                {
                    HorizontalLayout floor;
                    if (floorIndex == 0)
                    {
                        floor = ConstructHorizontal(
                            constructors: constructors[PanelType.Window],
                            height: floorHeight,
                            getPanelWidth: index => sizeValues[panelSizes[index]],
                            count: panelSizes.Count);
                    }
                    else
                    {
                        floor = ConstructHorizontal(
                            constructors: hasBalconies
                                ? constructors[PanelType.Balcony]
                                : constructors[PanelType.Window],
                            height: floorHeight,
                            getPanelWidth: index => sizeValues[panelSizes[index]],
                            count: panelSizes.Count);
                    }

                    facadeLayout.Add(floor);
                    facadeHeight += floorHeight;
                }
            }

            if (hasAttic)
            {
                var attic = ConstructHorizontal(
                    constructors: constructors[PanelType.Attic],
                    height: atticHeight,
                    getPanelWidth: index => sizeValues[panelSizes[index]],
                    count: panelSizes.Count);

                facadeLayout.Add(attic);
                facadeHeight += atticHeight;
            }

            facadeLayout.width = facadeWidth;
            facadeLayout.height = facadeHeight;
            return facadeLayout;
        }

        private VerticalLayout ConstructEntranceVertical(float width, int floors)
        {
            var vertical = ConstructVertical(
                constructor: commonConstructors[PanelType.EntranceWindow],
                width: width,
                panelHeight: floorHeight,
                count: floors - 1);

            var entrance = commonConstructors[PanelType.Entrance]();
            entrance.height = floorHeight;
            vertical.Insert(0, entrance);

            vertical.Add(constructors[PanelType.EntranceWallLast].GetRandom()());
            return vertical;
        }

        private FacadeLayout ConstructFacadeChunk(List<PanelSize> panelSizes, int from, int to, int floors,
            bool hasBalconies)
        {
            var sizes = panelSizes.GetRange(from, to - from);
            float chunkWidth = 0;
            foreach (var size in sizes)
            {
                chunkWidth += sizeValues[size];
            }
            return GenerateFacadeChunk(chunkWidth, sizes, floors, hasBalconies, false, null);
        }

        private static List<PanelSize> DivideFacade(Dictionary<PanelSize, float> sizeValues, float facadeWidth)
        {
            Dictionary<PanelSize, int> knapsack = PTUtils.Knapsack(sizeValues, facadeWidth);
            var sizes = new List<PanelSize>();
            foreach (var pair in knapsack)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    sizes.Add(pair.Key);
                }
            }
            sizes.Shuffle();
            return sizes;
        }

        [Serializable]
        public class Config
        {
            public float width = 12;
            public float length = 36;
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

        private enum PanelSize
        {
            Narrow,
            Wide
        }

        private enum PanelType
        {
            Wall,
            Window,
            Balcony,
            Entrance,
            EntranceWindow,
            EntranceWallLast,
            Socle,
            Attic,
        };
    }
}