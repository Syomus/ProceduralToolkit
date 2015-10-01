using System;
using UnityEngine;

namespace ProceduralToolkit.Examples
{
    /// <summary>
    /// A procedural chair generator
    /// </summary>
    public static class ChairGenerator
    {
        public static MeshDraft Chair(float legWidth, float legHeight, Vector3 seatDimensions, float backHeight,
            bool hasStretchers, bool hasArmrests)
        {
            Vector3 right = Vector3.right*(seatDimensions.x - legWidth)/2;
            Vector3 forward = Vector3.forward*(seatDimensions.z - legWidth)/2;

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
                var stretcherFunc = new Func<Vector3[], float, float, MeshDraft>[]
                {
                    Stretchers.XStretchers,
                    Stretchers.HStretchers,
                    Stretchers.BoxStretchers
                }.GetRandom();
                chair.Add(stretcherFunc(legCenters, legWidth, legHeight));
            }

            chair.Add(Seat0(Vector3.up*legHeight, seatDimensions.x, seatDimensions.z, seatDimensions.y));

            // Generate chair back
            var backFunc = new Func<Vector3, float, float, float, MeshDraft>[]
            {
                Backs.Back0,
                Backs.Back1,
                Backs.RodBack
            }.GetRandom();
            Vector3 backCenter = Vector3.up*(legHeight + seatDimensions.y) +
                                 Vector3.forward*(seatDimensions.z - legWidth)/2;
            chair.Add(backFunc(backCenter, seatDimensions.x, legWidth, backHeight));

            // Generate armrests
            if (hasArmrests)
            {
                var armrestsFunc = new Func<Vector3, Vector3, float, float, MeshDraft>[]
                {
                    Armrests.Armrests0,
                    Armrests.Armrests1
                }.GetRandom();
                chair.Add(armrestsFunc(seatDimensions, backCenter, backHeight, legWidth));
            }

            chair.Paint(RandomE.colorHSV);

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