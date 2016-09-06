using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public enum PanelSize
    {
        Narrow,
        Wide
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

    public static class Panels
    {
        private const float SocleWindowWidth = 0.7f;
        private const float SocleWindowHeight = 0.4f;
        private const float SocleWindowDepth = 0.1f;
        private const float SocleWindowHeightOffset = 0.1f;

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

        private const float AtticHoleWidth = 0.3f;
        private const float AtticHoleHeight = 0.3f;
        private const float AtticHoleDepth = 0.5f;

        public static MeshDraft Wall(Vector3 origin, Vector3 width, Vector3 height)
        {
            MeshDraft draft = MeshDraft.Quad(origin, width, height);
            draft.Paint(BuildingGenerator.wallColor);
            return draft;
        }

        public static MeshDraft Window(Vector3 origin, Vector3 width, Vector3 height)
        {
            return Window(origin, width, height, WindowWidthOffset, WindowHeightOffset, WindowHeight);
        }

        private static MeshDraft Window(Vector3 origin, Vector3 width, Vector3 height, float widthOffset,
            float heightOffset, float windowHeight)
        {
            Vector3 right = width.normalized;
            Vector3 frameOrigin = origin + right*widthOffset + Vector3.up*heightOffset;
            Vector3 frameWidth = right*(width.magnitude - widthOffset*2);
            Vector3 frameHeight = Vector3.up*windowHeight;
            Vector3 frameDepth = Vector3.Cross(width, height).normalized*WindowDepth;

            var draft = PerforatedQuad(origin, width, height, frameOrigin, frameWidth, frameHeight);

            var frame = MeshDraft.Hexahedron(frameWidth, -frameDepth, frameHeight, Directions.All & ~Directions.ZAxis);
            frame.Move(frameOrigin + frameWidth/2 + frameHeight/2 + frameDepth/2);
            draft.Add(frame);

            draft.Paint(BuildingGenerator.wallColor);

            draft.Add(Windowpane(frameOrigin + frameDepth, frameWidth, frameHeight));

            return draft;
        }

        public static MeshDraft Balcony(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 right = width.normalized;
            Vector3 normal = Vector3.Cross(height, width).normalized;
            Vector3 balconyWidth = width;
            Vector3 balconyHeight = Vector3.up*BalconyHeight;
            Vector3 balconyDepth = normal*BalconyDepth;

            var draft = new MeshDraft();
            var balconyOuter = MeshDraft.Hexahedron(balconyWidth, balconyDepth, balconyHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balconyOuter.FlipFaces();
            Vector3 balconyCenter = origin + width/2 + balconyDepth/2 + balconyHeight/2;
            balconyOuter.Move(balconyCenter);
            balconyOuter.Paint(BuildingGenerator.wallColor);

            Vector3 innerWidthOffset = right*BalconyThickness;
            Vector3 innerWidth = balconyWidth - innerWidthOffset*2;
            Vector3 innerHeightOffset = Vector3.up*BalconyThickness;
            Vector3 innerHeight = balconyHeight - innerHeightOffset;
            Vector3 innerDepthOffset = normal*BalconyThickness;
            Vector3 innerDepth = balconyDepth - innerDepthOffset;
            var balconyInner = MeshDraft.Hexahedron(innerWidth, innerDepth, innerHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balconyInner.Move(balconyCenter - innerDepthOffset/2 + innerHeightOffset/2);

            Vector3 borderOrigin = origin + balconyWidth + balconyHeight;
            Vector3 borderInnerOrigin = borderOrigin - innerWidthOffset;
            var balconyBorder = Bracket(borderOrigin, -balconyWidth, balconyDepth,
                borderInnerOrigin, -innerWidth, innerDepth);

            draft.Add(balconyOuter);
            draft.Add(balconyInner);
            draft.Add(balconyBorder);

            Vector3 windowWidthOffset = right*WindowWidthOffset;
            Vector3 windowHeightOffset = Vector3.up*WindowHeightOffset;
            Vector3 windowWidth = right*(width.magnitude - WindowWidthOffset*2);
            Vector3 windowHeight = Vector3.up*WindowHeight;
            Vector3 windowDepth = normal*WindowDepth;

            int rodCount = Mathf.FloorToInt(windowWidth.magnitude/WindowSegmentMinWidth);
            Vector3 doorWidth = right*windowWidth.magnitude/(rodCount + 1);
            Vector3 doorHeight = windowHeightOffset + windowHeight;

            List<Vector3> outerFrame = new List<Vector3>
            {
                origin + windowWidthOffset + innerHeightOffset,
                origin + windowWidthOffset + doorHeight,
                origin + windowWidthOffset + windowWidth + doorHeight,
                origin + windowWidthOffset + windowWidth + windowHeightOffset,
                origin + windowWidthOffset + doorWidth + windowHeightOffset,
                origin + windowWidthOffset + doorWidth + innerHeightOffset
            };

            var panel = MeshDraft.TriangleStrip(new List<Vector3>
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

            List<Vector3> innerFrame = new List<Vector3>();
            foreach (Vector3 vertex in outerFrame)
            {
                innerFrame.Add(vertex - windowDepth);
            }
            var frame = MeshDraft.FlatBand(innerFrame, outerFrame);
            draft.Add(frame);

            draft.Paint(BuildingGenerator.wallColor);

            draft.Add(Windowpane(outerFrame[0] - windowDepth, doorWidth, doorHeight - innerHeightOffset));
            draft.Add(Windowpane(outerFrame[4] - windowDepth, windowWidth - doorWidth, windowHeight));

            return draft;
        }

        public static MeshDraft BalconyGlazed(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 balconyWidth = width;
            Vector3 balconyHeight = height.normalized*BalconyHeight;
            Vector3 balconyDepth = Vector3.Cross(height, width).normalized*BalconyDepth;

            var draft = new MeshDraft();
            var balcony = MeshDraft.Hexahedron(balconyWidth, balconyDepth, balconyHeight,
                Directions.All & ~Directions.Up & ~Directions.Back);
            balcony.FlipFaces();
            balcony.Move(origin + width/2 + balconyDepth/2 + balconyHeight/2);
            balcony.Paint(BuildingGenerator.wallColor);
            draft.Add(balcony);

            Vector3 roof0 = origin + height;
            Vector3 roof1 = roof0 + balconyWidth;
            Vector3 roof2 = roof1 + balconyDepth;
            Vector3 roof3 = roof0 + balconyDepth;
            var roof = MeshDraft.Quad(roof0, roof1, roof2, roof3);
            roof.Paint(BuildingGenerator.roofColor);
            draft.Add(roof);

            Vector3 glassHeight = height - balconyHeight;
            Vector3 glass0 = origin + balconyHeight;
            Vector3 glass1 = glass0 + balconyDepth;
            Vector3 glass2 = glass1 + balconyWidth;
            var glass = Windowpane(glass0, balconyDepth, glassHeight);
            glass.Add(Windowpane(glass1, balconyWidth, glassHeight));
            glass.Add(Windowpane(glass2, -balconyDepth, glassHeight));
            draft.Add(glass);

            return draft;
        }

        public static MeshDraft Entrance(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 doorWidth = width.normalized*EntranceDoorWidth;
            Vector3 doorOrigin = origin + width/2 - doorWidth/2;
            Vector3 doorHeight = Vector3.up*EntranceDoorHeight;
            var draft = Bracket(origin, width, height, doorOrigin, doorWidth, doorHeight);
            draft.Paint(BuildingGenerator.wallColor);

            var door = MeshDraft.Quad(doorOrigin, doorWidth, doorHeight);
            door.Paint(BuildingGenerator.doorColor);
            draft.Add(door);
            return draft;
        }

        public static MeshDraft EntranceRoofed(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = Entrance(origin, width, height);
            Vector3 roofLength = Vector3.Cross(width, height).normalized*EntranceRoofLength;
            Vector3 roofHeight = Vector3.up*EntranceRoofHeight;
            var roof = MeshDraft.Hexahedron(width, roofLength, roofHeight);
            roof.Move(origin + width/2 - roofLength/2 + height - roofHeight/2);
            roof.Paint(BuildingGenerator.roofColor);
            draft.Add(roof);
            return draft;
        }

        public static MeshDraft EntranceWall(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = Window(origin, width, height/2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeight);
            draft.Add(Window(origin + height/2, width, height/2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeight));
            return draft;
        }

        public static MeshDraft Socle(Vector3 origin, Vector3 width, Vector3 height)
        {
            var draft = MeshDraft.Quad(origin, width, height);
            draft.Paint(BuildingGenerator.socleColor);
            return draft;
        }

        public static MeshDraft SocleWindowed(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 right = width.normalized;
            Vector3 windowOrigin = origin + width/2 - right*SocleWindowWidth/2 + Vector3.up*SocleWindowHeightOffset;
            Vector3 windowWidth = right*SocleWindowWidth;
            Vector3 windowHeigth = Vector3.up*SocleWindowHeight;
            Vector3 windowDepth = Vector3.Cross(width, height).normalized*SocleWindowDepth;

            var draft = PerforatedQuad(origin, width, height, windowOrigin, windowWidth, windowHeigth);

            var frame = MeshDraft.Hexahedron(windowWidth, -windowDepth, windowHeigth,
                Directions.All & ~Directions.ZAxis);
            frame.Move(windowOrigin + windowWidth/2 + windowHeigth/2 + windowDepth/2);
            draft.Add(frame);
            draft.Paint(BuildingGenerator.socleColor);

            var window = MeshDraft.Quad(windowOrigin + windowDepth/2, windowWidth, windowHeigth);
            window.Paint(BuildingGenerator.socleWindowColor);
            draft.Add(window);

            return draft;
        }

        public static MeshDraft AtticVented(Vector3 origin, Vector3 width, Vector3 height)
        {
            Vector3 right = width.normalized;
            Vector3 center = origin + width/2 + height/2;
            Vector3 holeOrigin = center - right*AtticHoleWidth/2 - Vector3.up*AtticHoleHeight/2;
            Vector3 holeWidth = right*AtticHoleWidth;
            Vector3 holeHeight = Vector3.up*AtticHoleHeight;
            Vector3 holeDepth = Vector3.Cross(width, height).normalized*AtticHoleDepth;

            var draft = PerforatedQuad(origin, width, height, holeOrigin, holeWidth, holeHeight);
            draft.Paint(BuildingGenerator.wallColor);

            var hexahedron = MeshDraft.Hexahedron(holeWidth, holeDepth, holeHeight, Directions.All & ~Directions.Back);
            hexahedron.Move(center + holeDepth/2);
            hexahedron.FlipFaces();
            hexahedron.Paint(BuildingGenerator.roofColor);
            draft.Add(hexahedron);
            return draft;
        }

        private static MeshDraft PerforatedQuad(Vector3 origin, Vector3 width, Vector3 length, Vector3 innerOrigin,
            Vector3 innerWidth, Vector3 innerLength)
        {
            Vector3 normal = Vector3.Cross(length, width).normalized;
            Vector3 vertex0 = origin;
            Vector3 vertex1 = origin + length;
            Vector3 vertex2 = origin + length + width;
            Vector3 vertex3 = origin + width;
            Vector3 window0 = innerOrigin;
            Vector3 window1 = innerOrigin + innerLength;
            Vector3 window2 = innerOrigin + innerLength + innerWidth;
            Vector3 window3 = innerOrigin + innerWidth;

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
            return MeshDraft.TriangleStrip(new List<Vector3>
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
            Vector3 right = width.normalized;
            Vector3 normal = Vector3.Cross(width, heigth).normalized;
            var draft = new MeshDraft();

            int rodCount = Mathf.FloorToInt(width.magnitude/WindowSegmentMinWidth);
            float interval = width.magnitude/(rodCount + 1);

            Vector3 frameWidth = right*WindowFrameWidth/2;
            Vector3 frameHeight = Vector3.up*WindowFrameWidth/2;
            Vector3 frameLength = normal*WindowFrameWidth/2;
            Vector3 startPosition = origin + heigth/2 + frameLength/2;
            for (int i = 0; i < rodCount; i++)
            {
                var frame = MeshDraft.Hexahedron(frameWidth*2, frameLength, heigth - frameHeight*2,
                    Directions.Left | Directions.Back | Directions.Right);
                frame.Move(startPosition + right*(i + 1)*interval);
                draft.Add(frame);
            }

            Vector3 windowCorner = origin + frameWidth + frameHeight;
            Vector3 windowWidth = width - frameWidth*2;
            Vector3 windowHeigth = heigth - frameHeight*2;
            var window = PerforatedQuad(origin, width, heigth, windowCorner, windowWidth, windowHeigth);
            draft.Add(window);

            var hole = MeshDraft.Hexahedron(windowWidth, frameLength, windowHeigth, Directions.All & ~Directions.ZAxis);
            hole.Move(startPosition + width/2);
            hole.FlipFaces();
            draft.Add(hole);

            draft.Paint(BuildingGenerator.frameColor);

            var glass = MeshDraft.Quad(windowCorner + frameLength, windowWidth, windowHeigth);
            glass.Paint(BuildingGenerator.glassColor);
            draft.Add(glass);

            return draft;
        }
    }
}