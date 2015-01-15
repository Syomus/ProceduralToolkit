using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public enum PanelSize
    {
        Narrow,
        Wide
    }

    public enum RoofType
    {
        Flat,
        FlatOverhang,
        Gabled,
        Hipped,
    }

    public enum PanelType
    {
        Wall,
        Window,
        Balcony,
        Entrance,
        EntranceWall,
        EntranceWallLast,
        Socle,
        Attic,
    };

    public class Panel
    {
        public PanelType type;
        public PanelSize size;
        public float height;
        public float heightOffset;

        public Panel()
        {
        }

        public Panel(Panel panel)
        {
            type = panel.type;
            size = panel.size;
            height = panel.height;
            heightOffset = panel.heightOffset;
        }
    }

    public class FloorPlan
    {
        public float height;
        public List<Panel> panels = new List<Panel>();
    }

    /// <summary>
    /// A procedural building generator
    /// http://en.wikipedia.org/wiki/Khrushchyovka
    /// </summary>
    [RequireComponent(typeof (MeshFilter), typeof (MeshRenderer))]
    public class Khrushchyovka : MonoBehaviour
    {
        public int widthLB = 10;
        public int widthUB = 13;
        public int lengthLB = 30;
        public int lengthUB = 55;
        public int floorCountLB = 1;
        public int floorCountUB = 7;

        private const float SocleHeight = 1;
        private const float SocleWindowWidth = 0.7f;
        private const float SocleWindowHeight = 0.4f;
        private const float SocleWindowDepth = 0.1f;
        private const float SocleWindowHeightOffset = 0.1f;

        private const float FloorHeight = 3;

        private const float EntranceDoorWidth = 1.8f;
        private const float EntranceDoorHeight = 2;
        private const float EntranceRoofLength = 1;
        private const float EntranceRoofHeight = 0.15f;

        private const float WindowHeight = 1.6f;
        private const float WindowDepth = 0.1f;
        private const float WindowWidthOffset = 0.5f;
        private const float WindowHeightOffset = 1;
        private const float WindowFrameWidth = 0.05f;
        private const float WindowSegmentMinWidth = 0.9f;

        private const float EntranceWindowHeight = 0.7f;
        private const float EntranceWindowWidthOffset = 0.4f;
        private const float EntranceWindowHeightOffset = 0.5f;

        private const float BalconyHeight = 1;
        private const float BalconyDepth = 0.8f;
        private const float BalconyThickness = 0.1f;

        private const float AtticHeight = 1;
        private const float AtticHoleWidth = 0.3f;
        private const float AtticHoleHeight = 0.3f;
        private const float AtticHoleDepth = 0.5f;

        private delegate MeshDraft PanelConstructor(Vector3 origin, Vector3 width, Vector3 heigth);

        private Dictionary<PanelType, PanelConstructor[]> panelConstructors = new Dictionary
            <PanelType, PanelConstructor[]>
        {
            {PanelType.Wall, new PanelConstructor[] {Wall}},
            {PanelType.Window, new PanelConstructor[] {Window}},
            {PanelType.Balcony, new PanelConstructor[] {Balcony, BalconyGlazed}},
            {PanelType.Entrance, new PanelConstructor[] {Entrance, EntranceRoofed}},
            {PanelType.EntranceWall, new PanelConstructor[] {EntranceWall, Window}},
            {PanelType.EntranceWallLast, new PanelConstructor[] {Wall}},
            {PanelType.Socle, new PanelConstructor[] {Socle, SocleWindowed}},
            {PanelType.Attic, new PanelConstructor[] {AtticVented, Wall}}
        };

        private Dictionary<PanelType, PanelConstructor> commonPanelConstructors =
            new Dictionary<PanelType, PanelConstructor>();

        private delegate MeshDraft RoofConstructor(Vector3 a, Vector3 b, Vector3 c, Vector3 d);

        private Dictionary<RoofType, RoofConstructor> roofConstructors = new Dictionary<RoofType, RoofConstructor>
        {
            {RoofType.Flat, FlatRoof},
            {RoofType.FlatOverhang, FlatOverhangRoof},
            {RoofType.Gabled, GabledRoof},
            {RoofType.Hipped, HippedRoof}
        };

        private Dictionary<PanelSize, float> sizeValues = new Dictionary<PanelSize, float>
        {
            {PanelSize.Narrow, 2.5f},
            {PanelSize.Wide, 3},
        };

        private static Color socleColor = ColorE.silver.WithA(0);
        private static Color socleWindowColor = ColorE.silver.WithA(0)/2;
        private static Color doorColor = ColorE.silver.WithA(0)/2;
        private static Color wallColor = ColorE.white.WithA(0);
        private static Color frameColor = ColorE.silver.WithA(0);
        private static Color glassColor = ColorE.white;
        private static Color roofColor = ColorE.gray.WithA(0.5f)/4;

        private void Start()
        {
            Generate();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Generate();
            }
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = KhrushchyovkaDraft().ToMesh();
        }

        private MeshDraft KhrushchyovkaDraft()
        {
            float width = Random.Range(widthLB, widthUB);
            float length = Random.Range(lengthLB, lengthUB);
            int floorCount = Random.Range(floorCountLB, floorCountUB);

            var hasAttic = RandomE.Chance(0.5f);
            float height = FloorHeight*floorCount + SocleHeight + (hasAttic ? AtticHeight : 0);

            var draft = new MeshDraft {name = "Khrushchyovka"};
            var corners = new Vector3[]
            {
                Vector3.left*length/2 + Vector3.back*width/2,
                Vector3.right*length/2 + Vector3.back*width/2,
                Vector3.right*length/2 + Vector3.forward*width/2,
                Vector3.left*length/2 + Vector3.forward*width/2
            };

            commonPanelConstructors[PanelType.Entrance] = panelConstructors[PanelType.Entrance].GetRandom();
            commonPanelConstructors[PanelType.EntranceWall] = panelConstructors[PanelType.EntranceWall].GetRandom();

            wallColor = RandomE.colorHSV.WithA(0);

            var facadePlan0 = FacadeGenerator(length, floorCount, hasAttic, true, true);
            draft.Add(Facade(corners[0], Vector3.right, facadePlan0));
            var facadePlan1 = FacadeGenerator(width, floorCount, hasAttic);
            draft.Add(Facade(corners[1], Vector3.forward, facadePlan1));
            var facadePlan2 = FacadeGenerator(length, floorCount, hasAttic, false, true);
            draft.Add(Facade(corners[2], Vector3.left, facadePlan2));
            var facadePlan3 = FacadeGenerator(width, floorCount, hasAttic);
            draft.Add(Facade(corners[3], Vector3.back, facadePlan3));

            draft.Add(Roof(corners[0], corners[1], corners[2], corners[3], Vector3.up*height));

            var basement = MeshE.QuadDraft(corners[0], corners[1], corners[2], corners[3]);
            basement.Paint(roofColor);
            draft.Add(basement);

            return draft;
        }

        private List<FloorPlan> FacadeGenerator(float width, int floorCount, bool hasAttic, bool hasEntrances = false,
            bool longFacade = false)
        {
            var panelSizes = PanelSizes(width);
            int panelCount = panelSizes.Count;

            var floors = new List<FloorPlan>(floorCount + 1);
            int entrances = (int) (width/10 - 1);
            var entranceCount = 1;
            var entranceIndex = panelCount*entranceCount/(entrances + 1);

            for (var i = 0; i < floorCount + 1; i++)
            {
                var floorPlan = new FloorPlan {height = i == 0 ? SocleHeight : FloorHeight};
                floors.Add(floorPlan);

                for (var j = 0; j < panelCount; j++)
                {
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
                    else
                    {
                        floorPlan.panels.Add(new Panel(floors[i - 1].panels[j]));
                    }

                    if (i == floorCount && (floors[i - 1].panels[j].type == PanelType.Entrance ||
                                            floors[i - 1].panels[j].type == PanelType.EntranceWall))
                    {
                        floorPlan.panels[j].type = PanelType.EntranceWallLast;
                        floorPlan.panels[j].height = SocleHeight;
                        floorPlan.panels[j].heightOffset = FloorHeight - SocleHeight;
                    }
                }

                if (i == 1 && !longFacade)
                {
                    for (int j = 0; j <= panelCount/2; j++)
                    {
                        if (j != 0 && j != panelCount - 1 && RandomE.Chance(0.5f))
                        {
                            floorPlan.panels[j].type = PanelType.Window;
                            floorPlan.panels[panelCount - 1 - j].type = PanelType.Window;
                        }
                    }
                }

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

        private List<PanelSize> PanelSizes(float wallLength)
        {
            var knapsack = PTUtils.Knapsack(sizeValues, wallLength);
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

        private MeshDraft Facade(Vector3 origin, Vector3 direction, List<FloorPlan> facadePlan)
        {
            var draft = new MeshDraft();

            Vector3 height = Vector3.zero;
            foreach (FloorPlan floor in facadePlan)
            {
                var panels = floor.panels;

                Vector3 panelOrigin = origin + height;
                foreach (var panel in panels)
                {
                    var offset = Vector3.up*panel.heightOffset;
                    var panelWidth = direction.normalized*sizeValues[panel.size];
                    var panelHeight = Vector3.up*panel.height;

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

        #region Panels

        private static MeshDraft Wall(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = MeshE.QuadDraft(origin, width, height);
            draft.Paint(wallColor);
            return draft;
        }

        private static MeshDraft Window(Vector3 origin, Vector3 width, Vector3 height)
        {
            return Window(origin, width, height, WindowWidthOffset, WindowHeightOffset, WindowHeight);
        }

        private static MeshDraft Window(Vector3 origin, Vector3 width, Vector3 height, float widthOffset,
            float heightOffset, float windowHeight)
        {
            var right = width.normalized;
            var frameOrigin = origin + right*widthOffset + Vector3.up*heightOffset;
            var frameWidth = right*(width.magnitude - widthOffset*2);
            var frameHeight = Vector3.up*windowHeight;
            var frameDepth = Vector3.Cross(width, height).normalized*WindowDepth;

            var draft = PerforatedQuad(origin, width, height, frameOrigin, frameWidth, frameHeight);

            var frame = MeshE.HexahedronDraft(frameWidth, -frameDepth, frameHeight, Directions.All & ~Directions.ZAxis);
            frame.Move(frameOrigin + frameWidth/2 + frameHeight/2 + frameDepth/2);
            draft.Add(frame);

            draft.Paint(wallColor);

            draft.Add(Windowpane(frameOrigin + frameDepth, frameWidth, frameHeight));

            return draft;
        }

        private static MeshDraft Balcony(Vector3 origin, Vector3 width, Vector3 height)
        {
            var right = width.normalized;
            var normal = Vector3.Cross(height, width).normalized;
            var balconyWidth = width;
            var balconyHeight = Vector3.up*BalconyHeight;
            var balconyDepth = normal*BalconyDepth;

            var draft = new MeshDraft();
            var balconyOuter = MeshE.HexahedronDraft(balconyWidth, balconyDepth, balconyHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balconyOuter.FlipFaces();
            var balconyCenter = origin + width/2 + balconyDepth/2 + balconyHeight/2;
            balconyOuter.Move(balconyCenter);
            balconyOuter.Paint(wallColor);

            var innerWidthOffset = right*BalconyThickness;
            var innerWidth = balconyWidth - innerWidthOffset*2;
            var innerHeightOffset = Vector3.up*BalconyThickness;
            var innerHeight = balconyHeight - innerHeightOffset;
            var innerDepthOffset = normal*BalconyThickness;
            var innerDepth = balconyDepth - innerDepthOffset;
            var balconyInner = MeshE.HexahedronDraft(innerWidth, innerDepth, innerHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balconyInner.Move(balconyCenter - innerDepthOffset/2 + innerHeightOffset/2);

            var borderOrigin = origin + balconyWidth + balconyHeight;
            var borderInnerOrigin = borderOrigin - innerWidthOffset;
            var balconyBorder = Bracket(borderOrigin, -balconyWidth, balconyDepth,
                borderInnerOrigin, -innerWidth, innerDepth);

            draft.Add(balconyOuter);
            draft.Add(balconyInner);
            draft.Add(balconyBorder);

            var windowWidthOffset = right*WindowWidthOffset;
            var windowHeightOffset = Vector3.up*WindowHeightOffset;
            var windowWidth = right*(width.magnitude - WindowWidthOffset*2);
            var windowHeight = Vector3.up*WindowHeight;
            var windowDepth = normal*WindowDepth;

            int rodCount = Mathf.FloorToInt(windowWidth.magnitude/WindowSegmentMinWidth);
            var doorWidth = right*windowWidth.magnitude/(rodCount + 1);
            var doorHeight = windowHeightOffset + windowHeight;

            var outerFrame = new List<Vector3>
            {
                origin + windowWidthOffset + innerHeightOffset,
                origin + windowWidthOffset + doorHeight,
                origin + windowWidthOffset + windowWidth + doorHeight,
                origin + windowWidthOffset + windowWidth + windowHeightOffset,
                origin + windowWidthOffset + doorWidth + windowHeightOffset,
                origin + windowWidthOffset + doorWidth + innerHeightOffset
            };

            var panel = MeshE.TriangleStripDraft(new List<Vector3>
            {
                outerFrame[0],
                origin,
                outerFrame[1],
                origin + height,
                outerFrame[2],
                origin + width + height,
                outerFrame[3],
                origin + width,
                outerFrame[4],
                origin + width,
                outerFrame[5]
            });

            draft.Add(panel);

            var innerFrame = new List<Vector3>();
            foreach (var vertex in outerFrame)
            {
                innerFrame.Add(vertex - windowDepth);
            }
            var frame = MeshE.FlatBandDraft(innerFrame, outerFrame);
            draft.Add(frame);

            draft.Paint(wallColor);

            draft.Add(Windowpane(outerFrame[0] - windowDepth, doorWidth, doorHeight - innerHeightOffset));
            draft.Add(Windowpane(outerFrame[4] - windowDepth, windowWidth - doorWidth, windowHeight));

            return draft;
        }

        private static MeshDraft BalconyGlazed(Vector3 origin, Vector3 width, Vector3 height)
        {
            var balconyWidth = width;
            var balconyHeight = height.normalized*BalconyHeight;
            var balconyDepth = Vector3.Cross(height, width).normalized*BalconyDepth;

            var draft = new MeshDraft();
            var balcony = MeshE.HexahedronDraft(balconyWidth, balconyDepth, balconyHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balcony.FlipFaces();
            balcony.Move(origin + width/2 + balconyDepth/2 + balconyHeight/2);
            balcony.Paint(wallColor);
            draft.Add(balcony);

            var roof0 = origin + height;
            var roof1 = roof0 + balconyWidth;
            var roof2 = roof1 + balconyDepth;
            var roof3 = roof0 + balconyDepth;
            var roof = MeshE.QuadDraft(roof0, roof1, roof2, roof3);
            roof.Paint(roofColor);
            draft.Add(roof);

            var glassHeight = height - balconyHeight;
            var glass0 = origin + balconyHeight;
            var glass1 = glass0 + balconyDepth;
            var glass2 = glass1 + balconyWidth;
            var glass = Windowpane(glass0, balconyDepth, glassHeight);
            glass.Add(Windowpane(glass1, balconyWidth, glassHeight));
            glass.Add(Windowpane(glass2, -balconyDepth, glassHeight));
            draft.Add(glass);

            return draft;
        }

        private static MeshDraft Entrance(Vector3 origin, Vector3 width, Vector3 height)
        {
            var doorWidth = width.normalized*EntranceDoorWidth;
            var doorOrigin = origin + width/2 - doorWidth/2;
            var doorHeight = Vector3.up*EntranceDoorHeight;
            var draft = Bracket(origin, width, height, doorOrigin, doorWidth, doorHeight);
            draft.Paint(wallColor);

            var door = MeshE.QuadDraft(doorOrigin, doorWidth, doorHeight);
            door.Paint(doorColor);
            draft.Add(door);
            return draft;
        }

        private static MeshDraft EntranceRoofed(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = Entrance(origin, width, height);
            var roofLength = Vector3.Cross(width, height).normalized*EntranceRoofLength;
            var roofHeight = Vector3.up*EntranceRoofHeight;
            var roof = MeshE.HexahedronDraft(width, roofLength, roofHeight);
            roof.Move(origin + width/2 - roofLength/2 + height - roofHeight/2);
            roof.Paint(roofColor);
            draft.Add(roof);
            return draft;
        }

        private static MeshDraft EntranceWall(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = Window(origin, width, height/2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeight);
            draft.Add(Window(origin + height/2, width, height/2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeight));
            return draft;
        }

        private static MeshDraft Socle(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = MeshE.QuadDraft(origin, width, height);
            draft.Paint(socleColor);
            return draft;
        }

        private static MeshDraft SocleWindowed(Vector3 origin, Vector3 width, Vector3 height)
        {
            var right = width.normalized;
            var windowOrigin = origin + width/2 - right*SocleWindowWidth/2 + Vector3.up*SocleWindowHeightOffset;
            var windowWidth = right*SocleWindowWidth;
            var windowHeigth = Vector3.up*SocleWindowHeight;
            var windowDepth = Vector3.Cross(width, height).normalized*SocleWindowDepth;

            var draft = PerforatedQuad(origin, width, height, windowOrigin, windowWidth, windowHeigth);

            var frame = MeshE.HexahedronDraft(windowWidth, -windowDepth, windowHeigth,
                Directions.All & ~Directions.ZAxis);
            frame.Move(windowOrigin + windowWidth/2 + windowHeigth/2 + windowDepth/2);
            draft.Add(frame);
            draft.Paint(socleColor);

            var window = MeshE.QuadDraft(windowOrigin + windowDepth/2, windowWidth, windowHeigth);
            window.Paint(socleWindowColor);
            draft.Add(window);

            return draft;
        }

        private static MeshDraft AtticVented(Vector3 origin, Vector3 width, Vector3 height)
        {
            var right = width.normalized;
            var center = origin + width/2 + height/2;
            var holeOrigin = center - right*AtticHoleWidth/2 - Vector3.up*AtticHoleHeight/2;
            var holeWidth = right*AtticHoleWidth;
            var holeHeight = Vector3.up*AtticHoleHeight;
            var holeDepth = Vector3.Cross(width, height).normalized*AtticHoleDepth;

            var draft = PerforatedQuad(origin, width, height, holeOrigin, holeWidth, holeHeight);
            draft.Paint(wallColor);

            var hexahedron = MeshE.HexahedronDraft(holeWidth, holeDepth, holeHeight, Directions.All & ~Directions.Back);
            hexahedron.Move(center + holeDepth/2);
            hexahedron.FlipFaces();
            hexahedron.Paint(roofColor);
            draft.Add(hexahedron);
            return draft;
        }

        #endregion Panels

        private MeshDraft Roof(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 height)
        {
            var roofConstructor = roofConstructors.GetRandom();
            return roofConstructor(a + height, b + height, c + height, d + height);
        }

        #region Roofs

        private static MeshDraft FlatRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var draft = MeshE.QuadDraft(a, d, c, b);
            draft.Paint(roofColor);
            return draft;
        }

        private static MeshDraft FlatOverhangRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var width = b - a;
            var length = c - b;
            var roofHeight = Vector3.up*0.3f;
            var draft = MeshE.HexahedronDraft(width + width.normalized, length + length.normalized, roofHeight);
            draft.Move((a + c)/2 + roofHeight/2);
            draft.Paint(roofColor);
            return draft;
        }

        private static MeshDraft GabledRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var ridgeHeight = Vector3.up*2;
            var ridge0 = (a + d)/2 + ridgeHeight;
            var ridge1 = (b + c)/2 + ridgeHeight;
            var draft = MeshE.QuadDraft(a, ridge0, ridge1, b);
            draft.Add(MeshE.TriangleDraft(b, ridge1, c));
            draft.Add(MeshE.QuadDraft(c, ridge1, ridge0, d));
            draft.Add(MeshE.TriangleDraft(d, ridge0, a));
            draft.Paint(roofColor);
            return draft;
        }

        private static MeshDraft HippedRoof(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            var ridgeHeight = Vector3.up*2;
            var ridgeOffset = (b - a).normalized*2;
            var ridge0 = (a + d)/2 + ridgeHeight + ridgeOffset;
            var ridge1 = (b + c)/2 + ridgeHeight - ridgeOffset;
            var draft = MeshE.QuadDraft(a, ridge0, ridge1, b);
            draft.Add(MeshE.TriangleDraft(b, ridge1, c));
            draft.Add(MeshE.QuadDraft(c, ridge1, ridge0, d));
            draft.Add(MeshE.TriangleDraft(d, ridge0, a));
            draft.Paint(roofColor);
            return draft;
        }

        #endregion Roofs

        private static MeshDraft PerforatedQuad(Vector3 origin, Vector3 width, Vector3 length, Vector3 innerOrigin,
            Vector3 innerWidth, Vector3 innerLength)
        {
            var normal = Vector3.Cross(length, width).normalized;
            var vertex0 = origin;
            var vertex1 = origin + length;
            var vertex2 = origin + length + width;
            var vertex3 = origin + width;
            var window0 = innerOrigin;
            var window1 = innerOrigin + innerLength;
            var window2 = innerOrigin + innerLength + innerWidth;
            var window3 = innerOrigin + innerWidth;

            return new MeshDraft
            {
                vertices = new List<Vector3>(8)
                {
                    vertex0,
                    vertex1,
                    vertex2,
                    vertex3,
                    window0,
                    window1,
                    window2,
                    window3,
                },
                normals = new List<Vector3>(8) {normal, normal, normal, normal, normal, normal, normal, normal},
                uv = new List<Vector2>(8)
                {
                    new Vector2(0, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                    new Vector2(1, 0),
                    new Vector2(0.25f, 0.25f),
                    new Vector2(0.25f, 0.75f),
                    new Vector2(0.75f, 0.75f),
                    new Vector2(0.75f, 0.25f)
                },
                triangles = new List<int>(24) {0, 1, 4, 4, 1, 5, 1, 2, 5, 5, 2, 6, 2, 3, 6, 6, 3, 7, 3, 0, 7, 7, 0, 4,}
            };
        }

        private static MeshDraft Bracket(Vector3 origin, Vector3 width, Vector3 length, Vector3 innerOrigin,
            Vector3 innerWidth, Vector3 innerLength)
        {
            return MeshE.TriangleStripDraft(new List<Vector3>
            {
                innerOrigin,
                origin,
                innerOrigin + innerLength,
                origin + length,
                innerOrigin + innerLength + innerWidth,
                origin + length + width,
                innerOrigin + innerWidth,
                origin + width
            });
        }

        private static MeshDraft Windowpane(Vector3 origin, Vector3 width, Vector3 heigth)
        {
            var right = width.normalized;
            var normal = Vector3.Cross(width, heigth).normalized;
            var draft = new MeshDraft();

            int rodCount = Mathf.FloorToInt(width.magnitude/WindowSegmentMinWidth);
            float interval = width.magnitude/(rodCount + 1);

            var frameWidth = right*WindowFrameWidth/2;
            var frameHeight = Vector3.up*WindowFrameWidth/2;
            var frameLength = normal*WindowFrameWidth/2;
            var startPosition = origin + heigth/2 + frameLength/2;
            for (int i = 0; i < rodCount; i++)
            {
                var frame = MeshE.HexahedronDraft(frameWidth*2, frameLength, heigth - frameHeight*2,
                    Directions.Left | Directions.Back | Directions.Right);
                frame.Move(startPosition + right*(i + 1)*interval);
                draft.Add(frame);
            }

            var windowCorner = origin + frameWidth + frameHeight;
            var windowWidth = width - frameWidth*2;
            var windowHeigth = heigth - frameHeight*2;
            var window = PerforatedQuad(origin, width, heigth, windowCorner, windowWidth, windowHeigth);
            draft.Add(window);

            var hole = MeshE.HexahedronDraft(windowWidth, frameLength, windowHeigth, Directions.All & ~Directions.ZAxis);
            hole.Move(startPosition + width/2);
            hole.FlipFaces();
            draft.Add(hole);

            draft.Paint(frameColor);

            var glass = MeshE.QuadDraft(windowCorner + frameLength, windowWidth, windowHeigth);
            glass.Paint(glassColor);
            draft.Add(glass);

            return draft;
        }

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "Click to generate new khrushchyovka");
        }
    }
}