using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A procedural chair generator
    /// </summary>
    [RequireComponent(typeof (MeshRenderer), typeof (MeshFilter))]
    public class ChairGenerator : MonoBehaviour
    {
        public float legWidthLB = 0.05f;
        public float legWidthUB = 0.12f;
        public float legHeightLB = 0.5f;
        public float legHeightUB = 1.2f;

        public Vector3 seatLB = new Vector3(0.7f, 0.05f, 0.7f);
        public Vector3 seatUB = new Vector3(1, 0.2f, 0.9f);

        public float backHeightLB = 0.5f;
        public float backHeightUB = 1.3f;

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
            GetComponent<MeshFilter>().mesh = Chair().ToMesh();
        }

        private MeshDraft Chair()
        {
            var legWidth = Random.Range(legWidthLB, legWidthUB);
            var legHeight = Random.Range(legHeightLB, legHeightUB);
            var seatDimensions = RandomE.Range(seatLB, seatUB);
            var backHeight = Random.Range(backHeightLB, backHeightUB);

            var chair = new MeshDraft {name = "Chair"};

            var right = Vector3.right*(seatDimensions.x - legWidth)/2;
            var forward = Vector3.forward*(seatDimensions.z - legWidth)/2;

            var legCenters = new Vector3[]
            {
                -right - forward,
                right - forward,
                right + forward,
                -right + forward
            };
            chair.Add(Leg0(legCenters[0], legWidth, legHeight));
            chair.Add(Leg0(legCenters[1], legWidth, legHeight));
            chair.Add(Leg0(legCenters[2], legWidth, legHeight));
            chair.Add(Leg0(legCenters[3], legWidth, legHeight));

            if (RandomE.Chance(0.3f))
            {
                var stretcherFunc = new Func<Vector3[], float, float, MeshDraft>[]
                {
                    XStretchers,
                    HStretchers,
                    BoxStretchers
                }.GetRandom();
                chair.Add(stretcherFunc(legCenters, legWidth, legHeight));
            }

            chair.Add(Seat0(Vector3.up*legHeight, seatDimensions.x, seatDimensions.z, seatDimensions.y));

            var backFunc = new Func<Vector3, float, float, float, MeshDraft>[]
            {
                Back0,
                Back1,
                RodBack
            }.GetRandom();
            var backCenter = Vector3.up*(legHeight + seatDimensions.y) + Vector3.forward*(seatDimensions.z - legWidth)/2;
            chair.Add(backFunc(backCenter, seatDimensions.x, legWidth, backHeight));

            if (RandomE.Chance(0.3f))
            {
                var armrestsFunc = new Func<Vector3, Vector3, float, float, MeshDraft>[]
                {
                    Armrests0,
                    Armrests1
                }.GetRandom();
                chair.Add(armrestsFunc(seatDimensions, backCenter, backHeight, legWidth));
            }

            chair.Paint(RandomE.colorHSV);

            return chair;
        }

        private MeshDraft Leg0(Vector3 center, float width, float height)
        {
            var draft = MeshE.HexahedronDraft(width, width, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private MeshDraft XStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth*3/4, 2);
            draft.Add(BeamDraft(legCenters[0], legCenters[2], legWidth));
            draft.Add(BeamDraft(legCenters[1], legCenters[3], legWidth));
            draft.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight/2, 2));
            return draft;
        }

        private MeshDraft HStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth, 3);
            draft.Add(BeamDraft(legCenters[0], legCenters[3], legWidth));
            draft.Add(BeamDraft(legCenters[1], legCenters[2], legWidth));
            var leftCenter = (legCenters[3] + legCenters[0])/2;
            var rightCenter = (legCenters[2] + legCenters[1])/2;
            draft.Add(BeamDraft(leftCenter, rightCenter, legWidth));
            draft.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            return draft;
        }

        private MeshDraft BoxStretchers(Vector3[] legCenters, float legWidth, float legHeight)
        {
            var draft = new MeshDraft();
            legWidth = RandomE.Range(legWidth/2, legWidth, 3);
            var stretcher0 = BeamDraft(legCenters[0], legCenters[1], legWidth);
            stretcher0.Add(BeamDraft(legCenters[2], legCenters[3], legWidth));
            stretcher0.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            var stretcher1 = BeamDraft(legCenters[0], legCenters[3], legWidth);
            stretcher1.Add(BeamDraft(legCenters[1], legCenters[2], legWidth));
            stretcher1.Move(Vector3.up*RandomE.Range(legHeight/4, legHeight*3/4, 3));
            draft.Add(stretcher0);
            draft.Add(stretcher1);
            return draft;
        }

        private MeshDraft Seat0(Vector3 center, float width, float length, float height)
        {
            var draft = MeshE.HexahedronDraft(width, length, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private MeshDraft Back0(Vector3 center, float width, float length, float height)
        {
            var draft = MeshE.HexahedronDraft(width, length, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private MeshDraft Back1(Vector3 center, float width, float length, float height)
        {
            var draft = new MeshDraft();
            int plankCount = Random.Range(1, 5);
            float plankStep = height/plankCount;
            float plankHeight = plankStep*Random.Range(0.3f, 0.8f);
            float plankWidth = width - length*2;

            float offeset = 0;
            if (plankCount > 1)
            {
                offeset = RandomE.Range(0, (plankStep - plankHeight)/2, 3);
            }
            var startPosition = Vector3.up*(-height/2 + plankStep - plankHeight/2 - offeset);
            for (int i = 0; i < plankCount; i++)
            {
                var plank = MeshE.HexahedronDraft(plankWidth, length, plankHeight);
                plank.Move(startPosition + Vector3.up*i*plankStep);
                draft.Add(plank);
            }
            var rod = MeshE.HexahedronDraft(length, length, height);
            rod.Move(Vector3.left*(width/2 - length/2));
            draft.Add(rod);
            rod.Move(Vector3.right*(width - length));
            draft.Add(rod);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private MeshDraft RodBack(Vector3 center, float width, float length, float height)
        {
            var draft = new MeshDraft();
            int rodCount = Random.Range(1, 5);
            float maxWidth = (width - length*2)/rodCount;
            float rodWidth = RandomE.Range(maxWidth/4, maxWidth*3/4, 3);
            float interval = (width - length*2 - rodWidth*rodCount)/(rodCount + 1);
            float upperRodHeight = Mathf.Min(height*3/4, length*Random.Range(1, 4));
            float rodHeight = height - upperRodHeight;

            var leftRod = MeshE.HexahedronDraft(length, length, rodHeight);
            leftRod.Move(Vector3.left*(width - length)/2 + Vector3.down*upperRodHeight/2);
            draft.Add(leftRod);
            leftRod.Move(Vector3.right*(width - length));
            draft.Add(leftRod);

            var startPosition = Vector3.left*(width/2 - length - interval - rodWidth/2) + Vector3.down*upperRodHeight/2;
            for (int i = 0; i < rodCount; i++)
            {
                var rod = MeshE.HexahedronDraft(rodWidth, length, rodHeight);
                rod.Move(startPosition + Vector3.right*i*(rodWidth + interval));
                draft.Add(rod);
            }
            var upperRod = MeshE.HexahedronDraft(width, length, upperRodHeight);
            upperRod.Move(Vector3.up*rodHeight/2);
            draft.Add(upperRod);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private MeshDraft Armrests0(Vector3 seatDimensions, Vector3 backCenter, float backHeight, float legWidth)
        {
            var draft = new MeshDraft();
            float armrestHeight = RandomE.Range(backHeight/4, backHeight*3/4, 3);
            float armrestLength = seatDimensions.z - legWidth;

            var corner = backCenter + Vector3.left*(seatDimensions.x/2 - legWidth/2) + Vector3.back*legWidth/2;

            float offset = 0;
            if (RandomE.Chance(0.5f))
            {
                offset = RandomE.Range(legWidth/2, legWidth, 2);
            }
            var v0 = corner + Vector3.back*(armrestLength - legWidth/2);
            var v1 = v0 + Vector3.up*(armrestHeight - legWidth/2);
            var v2 = corner + Vector3.up*armrestHeight;
            var v3 = v2 + Vector3.back*(armrestLength + offset);

            var armrest = BeamDraft(v0, v1, legWidth);
            armrest.Add(BeamDraft(v2, v3, legWidth));
            draft.Add(armrest);
            armrest.Move(Vector3.right*(seatDimensions.x - legWidth));
            draft.Add(armrest);
            return draft;
        }

        private MeshDraft Armrests1(Vector3 seatDimensions, Vector3 backCenter, float backHeight, float legWidth)
        {
            var draft = new MeshDraft();
            float armrestHeight = RandomE.Range(backHeight/4, backHeight*3/4, 3);
            float armrestLength = RandomE.Range(seatDimensions.z*3/4, seatDimensions.z, 2);
            legWidth = RandomE.Range(legWidth*3/4, legWidth, 2);

            var corner = backCenter + Vector3.left*(seatDimensions.x/2 + legWidth/2) +
                         Vector3.forward*legWidth/2;

            float offset = 0;
            if (RandomE.Chance(0.5f))
            {
                offset = RandomE.Range(armrestLength/4, armrestLength/2, 2) - legWidth/2;
            }
            var v0 = corner + Vector3.back*(armrestLength - legWidth/2 - offset) + Vector3.down*legWidth;
            var v1 = v0 + Vector3.up*(armrestHeight + legWidth/2);
            var v2 = corner + Vector3.up*armrestHeight;
            var v3 = v2 + Vector3.back*armrestLength;

            var armrest = BeamDraft(v0, v1, legWidth);
            armrest.Add(BeamDraft(v2, v3, legWidth));
            draft.Add(armrest);
            armrest.Move(Vector3.right*(seatDimensions.x + legWidth));
            draft.Add(armrest);
            return draft;
        }

        public static MeshDraft BeamDraft(Vector3 from, Vector3 to, float width, float rotation = 0)
        {
            var up = to - from;
            var draft = MeshE.HexahedronDraft(width, width, up.magnitude);
            var direction = up;
            direction.y = 0;
            var quaternion = Quaternion.identity;
            if (direction != Vector3.zero)
            {
                quaternion = Quaternion.LookRotation(direction);
            }
            draft.Rotate(Quaternion.FromToRotation(Vector3.up, up)*Quaternion.Euler(0, rotation, 0)*quaternion);
            draft.Move((from + to)/2);
            return draft;
        }

        private void OnGUI()
        {
            GUI.color = Color.black;
            GUI.Label(new Rect(20, 20, Screen.width, Screen.height), "Click to generate new chair");
        }
    }
}