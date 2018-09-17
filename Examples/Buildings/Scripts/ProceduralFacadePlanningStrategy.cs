using System;
using System.Collections.Generic;
using ProceduralToolkit.Buildings;
using UnityEngine;

namespace ProceduralToolkit.Examples.Buildings
{
    [CreateAssetMenu(menuName = "ProceduralToolkit/Buildings/Procedural Facade Planning Strategy", order = 0)]
    public class ProceduralFacadePlanningStrategy : FacadePlanningStrategy
    {
        private const float socleHeight = 1;
        private const float floorHeight = 2.5f;
        private const float atticHeight = 1;

        private Dictionary<PanelType, List<Func<ILayoutElement>>> constructors =
            new Dictionary<PanelType, List<Func<ILayoutElement>>>();
        private Dictionary<PanelType, Func<ILayoutElement>> commonConstructors =
            new Dictionary<PanelType, Func<ILayoutElement>>();

        private Dictionary<PanelSize, float> sizeValues = new Dictionary<PanelSize, float>
        {
            {PanelSize.Narrow, 2.5f},
            {PanelSize.Wide, 3},
        };

        public override List<ILayout> Plan(List<Vector2> foundationPolygon, BuildingGenerator.Config config)
        {
            SetupConstructors(config.palette);

            var facadeLayouts = new List<ILayout>();
            for (int i = 0; i < foundationPolygon.Count; i++)
            {
                Vector2 a = foundationPolygon.GetLooped(i + 1);
                Vector2 b = foundationPolygon[i];
                float facadeWidth = (b - a).magnitude;

                if (i == 0)
                {
                    facadeLayouts.Add(CreateEntranceFacade(facadeWidth, config.floors, config.entranceInterval, config.hasAttic));
                }
                else
                {
                    facadeLayouts.Add(CreateNormalFacade(facadeWidth, config.floors, config.hasAttic));
                }
            }

            return facadeLayouts;
        }

        private void SetupConstructors(Palette palette)
        {
            constructors[PanelType.Wall] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralWall(palette.wallColor)
            };
            constructors[PanelType.Window] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralWindow(palette.wallColor, palette.frameColor, palette.glassColor)
            };
            constructors[PanelType.Balcony] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralBalcony(palette.wallColor, palette.frameColor, palette.glassColor),
                () => new ProceduralBalconyGlazed(palette.wallColor, palette.frameColor, palette.glassColor, palette.roofColor)
            };
            constructors[PanelType.Entrance] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralEntrance(palette.wallColor, palette.doorColor),
                () => new ProceduralEntranceRoofed(palette.wallColor, palette.doorColor, palette.roofColor)
            };
            constructors[PanelType.EntranceWindow] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralEntranceWindow(palette.wallColor, palette.frameColor, palette.glassColor),
                () => new ProceduralWindow(palette.wallColor, palette.frameColor, palette.glassColor)
            };
            constructors[PanelType.EntranceWallLast] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralWall(palette.wallColor)
            };
            constructors[PanelType.Socle] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralSocle(palette.socleColor),
                () => new ProceduralSocleWindowed(palette.socleColor, palette.socleWindowColor)
            };
            constructors[PanelType.Attic] = new List<Func<ILayoutElement>>
            {
                () => new ProceduralAtticVented(palette.wallColor, palette.socleWindowColor),
                () => new ProceduralWall(palette.wallColor)
            };
        }

        private ILayout CreateNormalFacade(float facadeWidth, int floors, bool hasAttic)
        {
            float remainder;
            List<PanelSize> panelSizes = DivideFacade(facadeWidth, out remainder);
            bool hasBalconies = RandomE.Chance(0.5f);

            var vertical = CreateNormalFacadeVertical(panelSizes, 0, panelSizes.Count, floors, hasAttic, hasBalconies);
            if (remainder > Geometry.Epsilon)
            {
                return new HorizontalLayout
                {
                    CreateBufferWallVertical(remainder/2, floors, hasAttic),
                    vertical,
                    CreateBufferWallVertical(remainder/2, floors, hasAttic)
                };
            }
            return vertical;
        }

        private ILayout CreateEntranceFacade(float facadeWidth, int floors, float entranceInterval, bool hasAttic)
        {
            float remainder;
            List<PanelSize> panelSizes = DivideFacade(facadeWidth, out remainder);
            commonConstructors[PanelType.Entrance] = constructors[PanelType.Entrance].GetRandom();
            commonConstructors[PanelType.EntranceWindow] = constructors[PanelType.EntranceWindow].GetRandom();
            commonConstructors[PanelType.Wall] = constructors[PanelType.Wall].GetRandom();
            bool hasBalconies = RandomE.Chance(0.5f);

            var horizontal = new HorizontalLayout();

            bool hasRemainder = remainder > Geometry.Epsilon;
            if (hasRemainder)
            {
                horizontal.Add(CreateBufferWallVertical(remainder/2, floors, hasAttic));
            }

            int entranceCount = Mathf.Max(Mathf.FloorToInt(facadeWidth/entranceInterval) - 1, 1);
            int entranceIndexInterval = (panelSizes.Count - entranceCount)/(entranceCount + 1);

            int lastEntranceIndex = -1;
            for (int i = 0; i < entranceCount; i++)
            {
                int entranceIndex = (i + 1)*entranceIndexInterval + i;

                horizontal.Add(CreateNormalFacadeVertical(panelSizes, lastEntranceIndex + 1, entranceIndex, floors, hasAttic, hasBalconies));

                horizontal.Add(CreateEntranceVertical(sizeValues[panelSizes[entranceIndex]], floors, hasAttic));

                if (i == entranceCount - 1)
                {
                    horizontal.Add(CreateNormalFacadeVertical(panelSizes, entranceIndex + 1, panelSizes.Count, floors, hasAttic, hasBalconies));
                }

                lastEntranceIndex = entranceIndex;
            }
            if (hasRemainder)
            {
                horizontal.Add(CreateBufferWallVertical(remainder/2, floors, hasAttic));
            }
            return horizontal;
        }

        private VerticalLayout CreateBufferWallVertical(float width, int floors, bool hasAttic)
        {
            var vertical = new VerticalLayout
            {
                Construct(constructors[PanelType.Socle], width, socleHeight),
                CreateVertical(width, floorHeight, floors, commonConstructors[PanelType.Wall])
            };
            if (hasAttic)
            {
                vertical.Add(Construct(constructors[PanelType.Attic], width, atticHeight));
            }
            return vertical;
        }

        private VerticalLayout CreateNormalFacadeVertical(List<PanelSize> panelSizes, int from, int to, int floors, bool hasAttic, bool hasBalconies)
        {
            var vertical = new VerticalLayout();
            vertical.Add(CreateHorizontal(panelSizes, from, to, socleHeight, constructors[PanelType.Socle]));
            for (int floorIndex = 0; floorIndex < floors; floorIndex++)
            {
                if (floorIndex == 0 || !hasBalconies)
                {
                    vertical.Add(CreateHorizontal(panelSizes, from, to, floorHeight, constructors[PanelType.Window]));
                }
                else
                {
                    vertical.Add(CreateHorizontal(panelSizes, from, to, floorHeight, constructors[PanelType.Balcony]));
                }
            }
            if (hasAttic)
            {
                vertical.Add(CreateHorizontal(panelSizes, from, to, atticHeight, constructors[PanelType.Attic]));
            }
            return vertical;
        }

        private VerticalLayout CreateEntranceVertical(float width, int floors, bool hasAttic)
        {
            var vertical = new VerticalLayout();
            vertical.Add(Construct(commonConstructors[PanelType.Entrance], width, floorHeight));
            for (int i = 0; i < floors - 1; i++)
            {
                vertical.Add(Construct(commonConstructors[PanelType.EntranceWindow], width, floorHeight));
            }
            vertical.Add(Construct(PanelType.EntranceWallLast, width, socleHeight));

            if (hasAttic)
            {
                vertical.Add(Construct(PanelType.Attic, width, atticHeight));
            }
            return vertical;
        }

        private List<PanelSize> DivideFacade(float facadeWidth, out float remainder)
        {
            Dictionary<PanelSize, int> knapsack = PTUtils.Knapsack(sizeValues, facadeWidth);
            var sizes = new List<PanelSize>();
            remainder = facadeWidth;
            foreach (var pair in knapsack)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    sizes.Add(pair.Key);
                    remainder -= sizeValues[pair.Key];
                }
            }
            sizes.Shuffle();
            return sizes;
        }

        private HorizontalLayout CreateHorizontal(List<PanelSize> panelSizes, int from, int to, float height, List<Func<ILayoutElement>> constructors)
        {
            var horizontal = new HorizontalLayout();
            for (int i = from; i < to; i++)
            {
                float width = sizeValues[panelSizes[i]];
                horizontal.Add(Construct(constructors, width, height));
            }
            return horizontal;
        }

        private VerticalLayout CreateVertical(float width, float height, int floors, Func<ILayoutElement> constructor)
        {
            var verticalLayout = new VerticalLayout();
            for (int i = 0; i < floors; i++)
            {
                verticalLayout.Add(Construct(constructor, width, height));
            }
            return verticalLayout;
        }

        private ILayoutElement Construct(PanelType panelType, float width, float height)
        {
            return Construct(constructors[panelType], width, height);
        }

        private static ILayoutElement Construct(List<Func<ILayoutElement>> constructors, float width, float height)
        {
            return Construct(constructors.GetRandom(), width, height);
        }

        private static ILayoutElement Construct(Func<ILayoutElement> constructor, float width, float height)
        {
            var element = constructor();
            element.width = width;
            element.height = height;
            return element;
        }

        private enum PanelSize : byte
        {
            Narrow,
            Wide,
        }

        private enum PanelType : byte
        {
            Socle,
            Entrance,
            EntranceWindow,
            EntranceWallLast,
            Wall,
            Window,
            Balcony,
            Attic,
        }
    }
}
