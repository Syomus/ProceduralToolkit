using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A procedural building generator
    /// </summary>
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Khrushchyovka
    /// </remarks>
    public static class BuildingGenerator
    {
        public static Color socleColor = ColorE.silver;
        public static Color socleWindowColor = ColorE.silver/2;
        public static Color doorColor = ColorE.silver/2;
        public static Color wallColor = ColorE.white;
        public static Color frameColor = ColorE.silver;
        public static Color glassColor = ColorE.white;
        public static Color roofColor = ColorE.gray/4;

        private const float SocleHeight = 1;
        private const float FloorHeight = 3;
        private const float AtticHeight = 1;

        private delegate MeshDraft PanelConstructor(Vector3 origin, Vector3 width, Vector3 heigth);

        private static readonly Dictionary<PanelType, PanelConstructor[]> panelConstructors = new Dictionary
            <PanelType, PanelConstructor[]>
        {
            {PanelType.Wall, new PanelConstructor[] {Panels.Wall}},
            {PanelType.Window, new PanelConstructor[] {Panels.Window}},
            {PanelType.Balcony, new PanelConstructor[] {Panels.Balcony, Panels.BalconyGlazed}},
            {PanelType.Entrance, new PanelConstructor[] {Panels.Entrance, Panels.EntranceRoofed}},
            {PanelType.EntranceWall, new PanelConstructor[] {Panels.EntranceWall, Panels.Window}},
            {PanelType.EntranceWallLast, new PanelConstructor[] {Panels.Wall}},
            {PanelType.Socle, new PanelConstructor[] {Panels.Socle, Panels.SocleWindowed}},
            {PanelType.Attic, new PanelConstructor[] {Panels.AtticVented, Panels.Wall}}
        };

        private static readonly Dictionary<PanelType, PanelConstructor> commonPanelConstructors =
            new Dictionary<PanelType, PanelConstructor>();

        private delegate MeshDraft RoofConstructor(Vector3 a, Vector3 b, Vector3 c, Vector3 d);

        private static readonly Dictionary<RoofType, RoofConstructor> roofConstructors = new Dictionary
            <RoofType, RoofConstructor>
        {
            {RoofType.Flat, Roofs.FlatRoof},
            {RoofType.FlatOverhang, Roofs.FlatOverhangRoof},
            {RoofType.Gabled, Roofs.GabledRoof},
            {RoofType.Hipped, Roofs.HippedRoof}
        };

        private static readonly Dictionary<PanelSize, float> sizeValues = new Dictionary<PanelSize, float>
        {
            {PanelSize.Narrow, 2.5f},
            {PanelSize.Wide, 3},
        };

        private class FloorPlan
        {
            public float height;
            public List<Panel> panels = new List<Panel>();
        }

        public static MeshDraft BuildingDraft(float width, float length, int floorCount, bool hasAttic, Color wallColor)
        {
            float height = FloorHeight*floorCount + SocleHeight + (hasAttic ? AtticHeight : 0);
            BuildingGenerator.wallColor = wallColor.WithA(0);

            var draft = new MeshDraft {name = "Building"};
            var corners = new Vector3[]
            {
                Vector3.left*length/2 + Vector3.back*width/2,
                Vector3.right*length/2 + Vector3.back*width/2,
                Vector3.right*length/2 + Vector3.forward*width/2,
                Vector3.left*length/2 + Vector3.forward*width/2
            };

            commonPanelConstructors[PanelType.Entrance] = panelConstructors[PanelType.Entrance].GetRandom();
            commonPanelConstructors[PanelType.EntranceWall] = panelConstructors[PanelType.EntranceWall].GetRandom();

            List<FloorPlan> facadePlan0 = FacadeGenerator(length, floorCount, hasAttic, true, true);
            draft.Add(Facade(corners[0], Vector3.right, facadePlan0));
            List<FloorPlan> facadePlan1 = FacadeGenerator(width, floorCount, hasAttic);
            draft.Add(Facade(corners[1], Vector3.forward, facadePlan1));
            List<FloorPlan> facadePlan2 = FacadeGenerator(length, floorCount, hasAttic, false, true);
            draft.Add(Facade(corners[2], Vector3.left, facadePlan2));
            List<FloorPlan> facadePlan3 = FacadeGenerator(width, floorCount, hasAttic);
            draft.Add(Facade(corners[3], Vector3.back, facadePlan3));

            draft.Add(Roof(corners[0], corners[1], corners[2], corners[3], Vector3.up*height));

            var basement = MeshDraft.Quad(corners[0], corners[1], corners[2], corners[3]);
            basement.Paint(roofColor);
            draft.Add(basement);

            return draft;
        }

        private static List<FloorPlan> FacadeGenerator(float width, int floorCount, bool hasAttic,
            bool hasEntrances = false,
            bool longFacade = false)
        {
            List<PanelSize> panelSizes = SplitWallIntoPanels(width);
            int panelCount = panelSizes.Count;

            var floors = new List<FloorPlan>(floorCount + 1);
            int entrances = (int) (width/10 - 1);
            int entranceCount = 1;
            int entranceIndex = panelCount*entranceCount/(entrances + 1);

            for (var i = 0; i < floorCount + 1; i++)
            {
                var floorPlan = new FloorPlan {height = i == 0 ? SocleHeight : FloorHeight};
                floors.Add(floorPlan);

                for (var j = 0; j < panelCount; j++)
                {
                    // On socle floor we have entrances and socle panels
                    if (i == 0)
                    {
                        if (hasEntrances && j == entranceIndex && entranceCount <= entrances)
                        {
                            floorPlan.panels.Add(new Panel
                            {
                                type = PanelType.Entrance,
                                size = panelSizes[j],
                                height = FloorHeight
                            });
                            entranceCount++;
                            entranceIndex = panelCount*entranceCount/(entrances + 1);
                        }
                        else
                        {
                            floorPlan.panels.Add(new Panel
                            {
                                type = PanelType.Socle,
                                size = panelSizes[j],
                                height = SocleHeight
                            });
                        }
                    }
                    // On first floor we decorate entrances with entrance walls,
                    // place windows on long facades and regular walls on short facades
                    else if (i == 1)
                    {
                        if (floors[0].panels[j].type == PanelType.Entrance)
                        {
                            floorPlan.panels.Add(new Panel
                            {
                                type = PanelType.EntranceWall,
                                size = panelSizes[j],
                                height = FloorHeight,
                                heightOffset = FloorHeight - SocleHeight
                            });
                        }
                        else if (longFacade)
                        {
                            floorPlan.panels.Add(new Panel
                            {
                                type = PanelType.Window,
                                size = panelSizes[j],
                                height = FloorHeight
                            });
                        }
                        else
                        {
                            floorPlan.panels.Add(new Panel
                            {
                                type = PanelType.Wall,
                                size = panelSizes[j],
                                height = FloorHeight
                            });
                        }
                    }
                    // On second floor and upper we repeat layout of the first floor
                    else
                    {
                        floorPlan.panels.Add(new Panel(floors[i - 1].panels[j]));
                    }

                    // Entrance-type panel on the last floor should have special model
                    if (i == floorCount && (floors[i - 1].panels[j].type == PanelType.Entrance ||
                                            floors[i - 1].panels[j].type == PanelType.EntranceWall))
                    {
                        floorPlan.panels[j].type = PanelType.EntranceWallLast;
                        floorPlan.panels[j].height = SocleHeight;
                        floorPlan.panels[j].heightOffset = FloorHeight - SocleHeight;
                    }
                }

                // Short facade can have windows, but their positions should have horizontal symmetry
                if (i == 1 && !longFacade)
                {
                    // Iterate from corner to center of facade and replace walls with windows
                    for (int j = 0; j <= panelCount/2; j++)
                    {
                        if (j != 0 && j != panelCount - 1 && RandomE.Chance(0.5f))
                        {
                            floorPlan.panels[j].type = PanelType.Window;
                            floorPlan.panels[panelCount - 1 - j].type = PanelType.Window;
                        }
                    }
                }

                // Symmetrically replace windows with balconies
                if (i == 2)
                {
                    for (int j = 0; j <= panelCount/2; j++)
                    {
                        if (floorPlan.panels[j].type == PanelType.Window &&
                            floorPlan.panels[panelCount - 1 - j].type == PanelType.Window && RandomE.Chance(0.5f))
                        {
                            floorPlan.panels[j].type = PanelType.Balcony;
                            floorPlan.panels[panelCount - 1 - j].type = PanelType.Balcony;
                        }
                    }
                }
            }

            // Attach attic on top
            if (hasAttic)
            {
                var floorPlan = new FloorPlan {height = AtticHeight};
                floors.Add(floorPlan);

                for (var i = 0; i < panelCount; i++)
                {
                    floorPlan.panels.Add(new Panel
                    {
                        type = PanelType.Attic,
                        size = panelSizes[i],
                        height = AtticHeight
                    });
                }
            }

            return floors;
        }

        private static List<PanelSize> SplitWallIntoPanels(float wallLength)
        {
            Dictionary<PanelSize, int> knapsack = PTUtils.Knapsack(sizeValues, wallLength);
            var panelSizes = new List<PanelSize>();
            foreach (var pair in knapsack)
            {
                for (var i = 0; i < pair.Value; i++)
                {
                    panelSizes.Add(pair.Key);
                }
            }
            panelSizes.Shuffle();
            return panelSizes;
        }

        private static MeshDraft Facade(Vector3 origin, Vector3 direction, List<FloorPlan> facadePlan)
        {
            var draft = new MeshDraft();

            Vector3 height = Vector3.zero;
            foreach (FloorPlan floor in facadePlan)
            {
                List<Panel> panels = floor.panels;

                Vector3 panelOrigin = origin + height;
                foreach (var panel in panels)
                {
                    Vector3 offset = Vector3.up*panel.heightOffset;
                    Vector3 panelWidth = direction.normalized*sizeValues[panel.size];
                    Vector3 panelHeight = Vector3.up*panel.height;

                    PanelConstructor panelConstructor;
                    if (commonPanelConstructors.ContainsKey(panel.type))
                    {
                        panelConstructor = commonPanelConstructors[panel.type];
                    }
                    else
                    {
                        panelConstructor = panelConstructors[panel.type].GetRandom();
                    }
                    draft.Add(panelConstructor(panelOrigin + offset, panelWidth, panelHeight));

                    panelOrigin += panelWidth;
                }
                height += Vector3.up*floor.height;
            }

            return draft;
        }

        private static MeshDraft Roof(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 height)
        {
            RoofConstructor roofConstructor = roofConstructors.GetRandom();
            return roofConstructor(a + height, b + height, c + height, d + height);
        }
    }
}