using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A procedural chair generator
    /// </summary>
    public static class ChairGenerator
    {
        private delegate MeshDraft StretchersConstructor(Vector3[] legCenters, float legWidth, float legHeight);

        private static readonly StretchersConstructor[] stretchersConstructors =
        {
            Stretchers.XStretchers,
            Stretchers.HStretchers,
            Stretchers.BoxStretchers
        };

        private delegate MeshDraft BackConstructor(Vector3 center, float width, float length, float height);

        private static readonly BackConstructor[] backConstructors =
        {
            Backs.Back0,
            Backs.Back1,
            Backs.RodBack
        };

        private delegate MeshDraft ArmrestsConstructor(float seatWidth, float seatDepth, Vector3 backCenter,
            float backHeight, float legWidth);

        private static readonly ArmrestsConstructor[] armrestsConstructors =
        {
            Armrests.Armrests0,
            Armrests.Armrests1
        };

        public static MeshDraft Chair(
            float legWidth,
            float legHeight,
            float seatWidth,
            float seatDepth,
            float seatHeight,
            float backHeight,
            bool hasStretchers,
            bool hasArmrests,
            Color color)
        {
            Vector3 right = Vector3.right*(seatWidth - legWidth)/2;
            Vector3 forward = Vector3.forward*(seatDepth - legWidth)/2;

            var chair = new MeshDraft {name = "Chair"};

            // Generate legs
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

            // Generate stretchers
            if (hasStretchers)
            {
                var stretchersConstructor = stretchersConstructors.GetRandom();
                chair.Add(stretchersConstructor(legCenters, legWidth, legHeight));
            }

            // Generate seat
            chair.Add(Seat0(Vector3.up*legHeight, seatWidth, seatDepth, seatHeight));

            // Generate chair back
            Vector3 backCenter = Vector3.up*(legHeight + seatHeight) + Vector3.forward*(seatDepth - legWidth)/2;
            var backConstructor = backConstructors.GetRandom();
            chair.Add(backConstructor(backCenter, seatWidth, legWidth, backHeight));

            // Generate armrests
            if (hasArmrests)
            {
                var armrestsConstructor = armrestsConstructors.GetRandom();
                chair.Add(armrestsConstructor(seatWidth, seatDepth, backCenter, backHeight, legWidth));
            }

            chair.Paint(color);

            return chair;
        }

        private static MeshDraft Leg0(Vector3 center, float width, float height)
        {
            var draft = MeshDraft.Hexahedron(width, width, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private static MeshDraft Seat0(Vector3 center, float width, float length, float height)
        {
            var draft = MeshDraft.Hexahedron(width, length, height);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        public static MeshDraft BeamDraft(Vector3 from, Vector3 to, float width, float rotation = 0)
        {
            var up = to - from;
            var draft = MeshDraft.Hexahedron(width, width, up.magnitude);
            Vector3 direction = up;
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
    }
}