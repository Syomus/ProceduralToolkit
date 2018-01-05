using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    public abstract class ProceduralFacadePanel : IFacadePanel
    {
        public Vector2? origin { get; set; }
        public float? width { get; set; }
        public float? height { get; set; }

        protected const string WallDraftName = "Wall";
        protected const string GlassDraftName = "Glass";

        private const float SocleWindowWidth = 0.7f;
        private const float SocleWindowHeight = 0.4f;
        private const float SocleWindowDepth = 0.1f;
        private const float SocleWindowHeightOffset = 0.1f;

        private const float EntranceDoorWidth = 1.8f;
        private const float EntranceDoorHeight = 2;
        private const float EntranceDoorThickness = 0.05f;

        private const float EntranceRoofLength = 1;
        private const float EntranceRoofHeight = 0.15f;

        private const float EntranceWindowWidthOffset = 0.4f;
        private const float EntranceWindowHeightOffset = 0.3f;

        private const float WindowDepth = 0.1f;
        protected const float WindowWidthOffset = 0.5f;
        protected const float WindowBottomOffset = 1;
        protected const float WindowTopOffset = 0.3f;
        private const float WindowFrameWidth = 0.05f;
        private const float WindowSegmentMinWidth = 0.9f;
        private const float WindowsillWidthOffset = 0.1f;
        private const float WindowsillDepth = 0.15f;
        private const float WindowsillThickness = 0.05f;

        private const float BalconyHeight = 1;
        private const float BalconyDepth = 0.8f;
        private const float BalconyThickness = 0.1f;

        private const float AtticHoleWidth = 0.3f;
        private const float AtticHoleHeight = 0.3f;
        private const float AtticHoleDepth = 0.5f;

        public MeshDraft GetMeshDraft()
        {
            throw new NotImplementedException();
        }

        public abstract CompoundMeshDraft GetCompoundMeshDraft();

        protected static MeshDraft PerforatedQuad(
            Vector3 min,
            Vector3 max,
            Vector3 innerMin,
            Vector3 innerMax)
        {
            Vector3 size = max - min;
            Vector3 widthVector = size.ToVector3XZ();
            Vector3 heightVector = size.ToVector3Y();
            Vector3 normal = Vector3.Cross(heightVector, widthVector).normalized;

            Vector3 innerSize = innerMax - innerMin;
            Vector3 innerHeight = innerSize.ToVector3Y();
            Vector3 innerWidth = innerSize.ToVector3XZ();

            Vector3 vertex0 = min;
            Vector3 vertex1 = min + heightVector;
            Vector3 vertex2 = max;
            Vector3 vertex3 = min + widthVector;
            Vector3 window0 = innerMin;
            Vector3 window1 = innerMin + innerHeight;
            Vector3 window2 = innerMax;
            Vector3 window3 = innerMin + innerWidth;

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
                triangles = new List<int>(24) {0, 1, 4, 4, 1, 5, 1, 2, 5, 5, 2, 6, 2, 3, 6, 6, 3, 7, 3, 0, 7, 7, 0, 4,}
            };
        }

        protected static CompoundMeshDraft Window(
            Vector3 min,
            float width,
            float height,
            float widthOffset,
            float bottomOffset,
            float topOffset,
            Color wallColor,
            Color frameColor,
            Color glassColor,
            bool hasWindowsill = false)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;
            Vector3 max = min + widthVector + heightVector;
            Vector3 frameMin = min + Vector3.right*widthOffset + Vector3.up*bottomOffset;
            Vector3 frameMax = max - Vector3.right*widthOffset - Vector3.up*topOffset;
            Vector3 frameWidth = Vector3.right*(width - widthOffset*2);
            Vector3 frameHeight = Vector3.up*(height - bottomOffset - topOffset);
            Vector3 frameDepth = Vector3.forward*WindowDepth;
            Vector3 frameSize = frameMax - frameMin;

            var frame = MeshDraft.PartialBox(frameWidth, -frameDepth, frameHeight, Directions.All & ~Directions.ZAxis, false)
                .Move(frameMin + frameSize/2 + frameDepth/2);

            var wall = PerforatedQuad(min, max, frameMin, frameMax)
                .Add(frame)
                .Paint(wallColor);
            wall.name = WallDraftName;

            var windowpane = Windowpane(frameMin + frameDepth, frameMax + frameDepth, frameColor, glassColor);

            var compoundDraft = new CompoundMeshDraft().Add(wall).Add(windowpane);

            if (hasWindowsill)
            {
                Vector3 windowsillWidth = frameWidth + Vector3.right*WindowsillWidthOffset;
                Vector3 windowsillDepth = Vector3.forward*WindowsillDepth;
                Vector3 windowsillHeight = Vector3.up*WindowsillThickness;
                var windowsill = MeshDraft.PartialBox(windowsillWidth, windowsillDepth, windowsillHeight,
                        Directions.All & ~Directions.Forward, false)
                    .Move(frameMin + frameWidth/2 + frameDepth - windowsillDepth/2)
                    .Paint(frameColor);
                windowsill.name = WallDraftName;
                compoundDraft.Add(windowsill);
            }
            return compoundDraft;
        }

        private static CompoundMeshDraft Windowpane(Vector3 min, Vector3 max, Color frameColor, Color glassColor)
        {
            Vector3 size = max - min;
            Vector3 widthVector = size.ToVector3XZ();
            Vector3 heightVector = size.ToVector3Y();
            Vector3 right = widthVector.normalized;
            Vector3 normal = Vector3.Cross(widthVector, heightVector).normalized;
            var frame = new MeshDraft {name = WallDraftName};

            int rodCount = Mathf.FloorToInt(widthVector.magnitude/WindowSegmentMinWidth);
            float interval = widthVector.magnitude/(rodCount + 1);

            Vector3 frameWidth = right*WindowFrameWidth/2;
            Vector3 frameHeight = Vector3.up*WindowFrameWidth/2;
            Vector3 frameLength = normal*WindowFrameWidth/2;
            Vector3 startPosition = min + heightVector/2 + frameLength/2;
            for (int i = 0; i < rodCount; i++)
            {
                var rod = MeshDraft.PartialBox(frameWidth*2, frameLength, heightVector - frameHeight*2,
                        Directions.Left | Directions.Back | Directions.Right, false)
                    .Move(startPosition + right*(i + 1)*interval);
                frame.Add(rod);
            }

            Vector3 windowMin = min + frameWidth + frameHeight;
            Vector3 windowWidth = widthVector - frameWidth*2;
            Vector3 windowHeight = heightVector - frameHeight*2;
            Vector3 windowMax = windowMin + windowWidth + windowHeight;
            frame.Add(PerforatedQuad(min, max, windowMin, windowMax));

            var hole = MeshDraft.PartialBox(windowWidth, frameLength, windowHeight, Directions.All & ~Directions.ZAxis, false)
                .Move(startPosition + widthVector/2)
                .FlipFaces();
            frame.Add(hole);
            frame.Paint(frameColor);

            var glass = new MeshDraft().AddQuad(windowMin + frameLength, windowWidth, windowHeight)
                .Paint(glassColor);
            glass.name = GlassDraftName;

            return new CompoundMeshDraft().Add(frame).Add(glass);
        }

        protected static CompoundMeshDraft Balcony(
            Vector3 origin,
            float width,
            float height,
            Color wallColor,
            Color frameColor,
            Color glassColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;

            var wall = Balcony(origin, width, wallColor);

            Vector3 innerHeightOffset = Vector3.up*BalconyThickness;

            Vector3 windowHeightOffset = Vector3.up*WindowBottomOffset;
            Vector3 windowWidth = Vector3.right*(width - WindowWidthOffset*2);
            Vector3 windowHeight = Vector3.up*(height - WindowBottomOffset - WindowTopOffset);
            Vector3 windowDepth = Vector3.forward*WindowDepth;

            int rodCount = Mathf.FloorToInt(windowWidth.magnitude/WindowSegmentMinWidth);
            Vector3 doorWidth = Vector3.right*windowWidth.magnitude/(rodCount + 1);
            Vector3 doorHeight = windowHeightOffset + windowHeight;

            Vector3 outerFrameOrigin = origin + Vector3.right*WindowWidthOffset + innerHeightOffset;
            List<Vector3> outerFrame = new List<Vector3>
            {
                outerFrameOrigin,
                outerFrameOrigin + doorWidth,
                outerFrameOrigin + doorWidth + windowHeightOffset,
                outerFrameOrigin + windowWidth + windowHeightOffset,
                outerFrameOrigin + windowWidth + doorHeight,
                outerFrameOrigin + doorHeight,
            };

            var panel = new MeshDraft().AddTriangleStrip(new List<Vector3>
            {
                outerFrame[0],
                origin,
                outerFrame[5],
                origin + heightVector,
                outerFrame[4],
                origin + widthVector + heightVector,
                outerFrame[3],
                origin + widthVector,
                outerFrame[2],
                origin + widthVector,
                outerFrame[1]
            }).Paint(wallColor);

            var innerFrame = new List<Vector3>();
            foreach (Vector3 vertex in outerFrame)
            {
                innerFrame.Add(vertex + windowDepth);
            }
            var frame = new MeshDraft().AddFlatQuadBand(innerFrame, outerFrame, false)
                .Paint(wallColor);

            wall.Add(panel);
            wall.Add(frame);
            wall.name = WallDraftName;

            Vector3 windowpaneMin1 = outerFrame[0] + windowDepth;
            Vector3 windowpaneMin2 = outerFrame[2] + windowDepth;
            var windowpane = Windowpane(windowpaneMin1, windowpaneMin1 + doorWidth + doorHeight, frameColor, glassColor);
            windowpane.Add(Windowpane(windowpaneMin2, windowpaneMin2 + windowWidth - doorWidth + windowHeight, frameColor, glassColor));

            return new CompoundMeshDraft().Add(wall).Add(windowpane);
        }

        private static MeshDraft Balcony(Vector3 origin, float width, Color wallColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 balconyHeight = Vector3.up*BalconyHeight;
            Vector3 balconyDepth = Vector3.back*BalconyDepth;

            var balconyOuter = MeshDraft.PartialBox(widthVector, balconyDepth, balconyHeight,
                Directions.All & ~Directions.Up & ~Directions.Back, false);
            balconyOuter.FlipFaces();
            Vector3 balconyCenter = origin + widthVector/2 + balconyDepth/2 + balconyHeight/2;
            balconyOuter.Move(balconyCenter);

            Vector3 innerWidthOffset = Vector3.right*BalconyThickness;
            Vector3 innerWidth = widthVector - innerWidthOffset*2;
            Vector3 innerHeightOffset = Vector3.up*BalconyThickness;
            Vector3 innerHeight = balconyHeight - innerHeightOffset;
            Vector3 innerDepthOffset = Vector3.back*BalconyThickness;
            Vector3 innerDepth = balconyDepth - innerDepthOffset;
            var balconyInner = MeshDraft.PartialBox(innerWidth, innerDepth, innerHeight,
                Directions.All & ~Directions.Up & ~Directions.Back, false);
            balconyInner.Move(balconyCenter - innerDepthOffset/2 + innerHeightOffset/2);

            Vector3 borderOrigin = origin + widthVector + balconyHeight;
            Vector3 borderInnerOrigin = borderOrigin - innerWidthOffset;
            var balconyBorder = Bracket(borderOrigin, -widthVector, balconyDepth,
                borderInnerOrigin, -innerWidth, innerDepth);

            var balcony = new MeshDraft();
            balcony.Add(balconyOuter);
            balcony.Add(balconyInner);
            balcony.Add(balconyBorder);
            balcony.Paint(wallColor);
            return balcony;
        }

        private static MeshDraft Bracket(Vector3 origin, Vector3 width, Vector3 length,
            Vector3 innerOrigin, Vector3 innerWidth, Vector3 innerLength)
        {
            return new MeshDraft().AddTriangleStrip(new List<Vector3>
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

        protected static CompoundMeshDraft BalconyGlazed(Vector3 origin, float width, float height, Color wallColor,
            Color frameColor, Color glassColor, Color roofColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;

            Vector3 balconyHeight = Vector3.up*BalconyHeight;
            Vector3 balconyDepth = Vector3.back*BalconyDepth;

            var balcony = MeshDraft.PartialBox(widthVector, balconyDepth, balconyHeight,
                    Directions.All & ~Directions.Up & ~Directions.Back, false)
                .FlipFaces()
                .Move(origin + widthVector/2 + balconyDepth/2 + balconyHeight/2)
                .Paint(wallColor);

            Vector3 roof0 = origin + heightVector;
            Vector3 roof1 = roof0 + widthVector;
            Vector3 roof2 = roof1 + balconyDepth;
            Vector3 roof3 = roof0 + balconyDepth;
            var roof = new MeshDraft()
                .AddQuad(roof0, roof1, roof2, roof3, Vector3.up)
                .Paint(roofColor);

            var wall = new MeshDraft {name = WallDraftName}.Add(balcony).Add(roof);

            Vector3 glassHeight = heightVector - balconyHeight;
            Vector3 glass0 = origin + balconyHeight;
            Vector3 glass1 = glass0 + balconyDepth;
            Vector3 glass2 = glass1 + widthVector;
            var windowpane = Windowpane(glass0, glass0 + balconyDepth + glassHeight, frameColor, glassColor)
                .Add(Windowpane(glass1, glass1 + widthVector + glassHeight, frameColor, glassColor))
                .Add(Windowpane(glass2, glass2 - balconyDepth + glassHeight, frameColor, glassColor));

            return new CompoundMeshDraft().Add(wall).Add(windowpane);
        }

        protected static MeshDraft Entrance(Vector3 origin, float width, float height, Color wallColor, Color doorColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;

            Vector3 doorWidth = Vector3.right*EntranceDoorWidth;
            Vector3 doorHeight = Vector3.up*EntranceDoorHeight;
            Vector3 doorThickness = Vector3.back*EntranceDoorThickness;
            Vector3 doorOrigin = origin + widthVector/2 - doorWidth/2;

            var draft = Bracket(origin, widthVector, heightVector, doorOrigin, doorWidth, doorHeight)
                .Paint(wallColor);

            var doorFrame = MeshDraft.PartialBox(doorWidth, -doorThickness, doorHeight, Directions.All & ~Directions.ZAxis, false)
                .Move(doorOrigin + doorWidth/2 + doorHeight/2 + doorThickness/2)
                .Paint(doorColor);
            draft.Add(doorFrame);

            var door = new MeshDraft().AddQuad(doorOrigin + doorThickness, doorWidth, doorHeight)
                .Paint(doorColor);
            draft.Add(door);
            return draft;
        }

        protected static MeshDraft EntranceRoofed(Vector3 origin, float width, float height, Color wallColor,
            Color doorColor, Color roofColor)
        {
            var draft = Entrance(origin, width, height, wallColor, doorColor);
            Vector3 widthVector = Vector3.right*width;
            Vector3 lengthVector = Vector3.forward*EntranceRoofLength;

            var roof = MeshDraft.PartialBox(widthVector, lengthVector, Vector3.up*EntranceRoofHeight, Directions.All & ~Directions.Forward, false)
                .Move(origin + widthVector/2 + Vector3.up*(height - EntranceRoofHeight/2) - lengthVector/2)
                .Paint(roofColor);
            draft.Add(roof);
            return draft;
        }

        protected static CompoundMeshDraft EntranceWindow(Vector3 origin, float width, float height, Color wallColor,
            Color frameColor, Color glassColor)
        {
            var compoundDraft = Window(origin, width, height/2, EntranceWindowWidthOffset, EntranceWindowHeightOffset,
                EntranceWindowHeightOffset, wallColor, frameColor, glassColor);
            compoundDraft.Add(Window(origin + Vector3.up*height/2, width, height/2, EntranceWindowWidthOffset,
                EntranceWindowHeightOffset, EntranceWindowHeightOffset, wallColor, frameColor, glassColor));
            return compoundDraft;
        }

        protected static CompoundMeshDraft SocleWindowed(Vector3 origin, float width, float height, Color wallColor,
            Color glassColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;

            Vector3 windowWidth = Vector3.right*SocleWindowWidth;
            Vector3 windowHeigth = Vector3.up*SocleWindowHeight;
            Vector3 windowDepth = Vector3.forward*SocleWindowDepth;
            Vector3 windowOrigin = origin + widthVector/2 - windowWidth/2 + Vector3.up*SocleWindowHeightOffset;
            Vector3 windowMax = windowOrigin + windowWidth + windowHeigth;

            var frame = MeshDraft.PartialBox(windowWidth, -windowDepth, windowHeigth, Directions.All & ~Directions.ZAxis, false)
                .Move(windowOrigin + windowWidth/2 + windowHeigth/2 + windowDepth/2);

            var wall = PerforatedQuad(origin, origin + widthVector + heightVector, windowOrigin, windowMax)
                .Add(frame)
                .Paint(wallColor);
            wall.name = WallDraftName;

            var glass = new MeshDraft()
                .AddQuad(windowOrigin + windowDepth/2, windowWidth, windowHeigth)
                .Paint(glassColor);
            glass.name = GlassDraftName;

            return new CompoundMeshDraft().Add(wall).Add(glass);
        }

        protected static MeshDraft AtticVented(Vector3 origin, float width, float height, Color wallColor,
            Color holeColor)
        {
            Vector3 widthVector = Vector3.right*width;
            Vector3 heightVector = Vector3.up*height;
            Vector3 center = origin + widthVector/2 + heightVector/2;

            Vector3 holeOrigin = center - Vector3.right*AtticHoleWidth/2 - Vector3.up*AtticHoleHeight/2;
            Vector3 holeWidth = Vector3.right*AtticHoleWidth;
            Vector3 holeHeight = Vector3.up*AtticHoleHeight;
            Vector3 holeDepth = Vector3.forward*AtticHoleDepth;

            var wall = PerforatedQuad(origin, origin + widthVector + heightVector, holeOrigin, holeOrigin + holeWidth + holeHeight)
                .Paint(wallColor);

            var hole = MeshDraft.PartialBox(holeWidth, holeDepth, holeHeight, Directions.All & ~Directions.Back, false)
                .Move(center + holeDepth/2)
                .FlipFaces()
                .Paint(holeColor);
            wall.Add(hole);
            wall.name = WallDraftName;
            return wall;
        }
    }

    public class ProceduralWall : ProceduralFacadePanel
    {
        private Color wallColor;

        public ProceduralWall(Color wallColor)
        {
            this.wallColor = wallColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            var wallDraft = new MeshDraft()
                .AddQuad(origin.Value, Vector3.right*width.Value, Vector3.up*height.Value)
                .Paint(wallColor);
            wallDraft.name = WallDraftName;
            return new CompoundMeshDraft().Add(wallDraft);
        }
    }

    public class ProceduralWindow : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color frameColor;
        private Color glassColor;

        public ProceduralWindow(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            return Window(origin.Value, width.Value, height.Value, WindowWidthOffset, WindowBottomOffset, WindowTopOffset,
                wallColor, frameColor, glassColor, true);
        }
    }

    public class ProceduralBalcony : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color frameColor;
        private Color glassColor;

        public ProceduralBalcony(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            return Balcony(origin.Value, width.Value, height.Value, wallColor, frameColor, glassColor);
        }
    }

    public class ProceduralBalconyGlazed : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color frameColor;
        private Color glassColor;
        private Color roofColor;

        public ProceduralBalconyGlazed(Color wallColor, Color frameColor, Color glassColor, Color roofColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
            this.roofColor = roofColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            return BalconyGlazed(origin.Value, width.Value, height.Value, wallColor, frameColor, glassColor, roofColor);
        }
    }

    public class ProceduralEntrance : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color doorColor;

        public ProceduralEntrance(Color wallColor, Color doorColor)
        {
            this.wallColor = wallColor;
            this.doorColor = doorColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            var entranceDraft = Entrance(origin.Value, width.Value, height.Value, wallColor, doorColor);
            entranceDraft.name = WallDraftName;
            return new CompoundMeshDraft().Add(entranceDraft);
        }
    }

    public class ProceduralEntranceRoofed : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color doorColor;
        private Color roofColor;

        public ProceduralEntranceRoofed(Color wallColor, Color doorColor, Color roofColor)
        {
            this.wallColor = wallColor;
            this.doorColor = doorColor;
            this.roofColor = roofColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            var entranceDraft = EntranceRoofed(origin.Value, width.Value, height.Value, wallColor, doorColor, roofColor);
            entranceDraft.name = WallDraftName;
            return new CompoundMeshDraft().Add(entranceDraft);
        }
    }

    public class ProceduralEntranceWindow : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color frameColor;
        private Color glassColor;

        public ProceduralEntranceWindow(Color wallColor, Color frameColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.frameColor = frameColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            return EntranceWindow(origin.Value, width.Value, height.Value, wallColor, frameColor, glassColor);
        }
    }

    public class ProceduralSocle : ProceduralWall
    {
        public ProceduralSocle(Color wallColor) : base(wallColor)
        {
        }
    }

    public class ProceduralSocleWindowed : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color glassColor;

        public ProceduralSocleWindowed(Color wallColor, Color glassColor)
        {
            this.wallColor = wallColor;
            this.glassColor = glassColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            return SocleWindowed(origin.Value, width.Value, height.Value, wallColor, glassColor);
        }
    }

    public class ProceduralAtticVented : ProceduralFacadePanel
    {
        private Color wallColor;
        private Color holeColor;

        public ProceduralAtticVented(Color wallColor, Color holeColor)
        {
            this.wallColor = wallColor;
            this.holeColor = holeColor;
        }

        public override CompoundMeshDraft GetCompoundMeshDraft()
        {
            var atticVentedDraft = AtticVented(origin.Value, width.Value, height.Value, wallColor, holeColor);
            return new CompoundMeshDraft().Add(atticVentedDraft);
        }
    }
}
