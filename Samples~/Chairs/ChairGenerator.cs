using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Samples
{
    /// <summary>
    /// A fully procedural chair generator, creates entire mesh from scratch and paints it's vertices
    /// </summary>
    public static class ChairGenerator
    {
        [Serializable]
        public class Config
        {
            public float legWidth = 0.07f;
            public float legHeight = 0.7f;
            public float seatWidth = 0.7f;
            public float seatDepth = 0.7f;
            public float seatHeight = 0.05f;
            public float backHeight = 0.8f;
            public bool hasStretchers = true;
            public bool hasArmrests = false;
            public Color color = Color.white;
        }

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

        public static MeshDraft Chair(Config config)
        {
            Assert.IsTrue(config.legWidth > 0);
            Assert.IsTrue(config.legHeight > 0);
            Assert.IsTrue(config.seatWidth > 0);
            Assert.IsTrue(config.seatDepth > 0);
            Assert.IsTrue(config.seatHeight > 0);
            Assert.IsTrue(config.backHeight > 0);

            Vector3 right = Vector3.right*(config.seatWidth - config.legWidth)/2;
            Vector3 forward = Vector3.forward*(config.seatDepth - config.legWidth)/2;

            var chair = new MeshDraft {name = "Chair"};

            // Generate legs
            var legCenters = new Vector3[]
            {
                -right - forward,
                right - forward,
                right + forward,
                -right + forward
            };
            chair.Add(Leg0(legCenters[0], config.legWidth, config.legHeight));
            chair.Add(Leg0(legCenters[1], config.legWidth, config.legHeight));
            chair.Add(Leg0(legCenters[2], config.legWidth, config.legHeight));
            chair.Add(Leg0(legCenters[3], config.legWidth, config.legHeight));

            // Generate stretchers
            if (config.hasStretchers)
            {
                var stretchersConstructor = stretchersConstructors.GetRandom();
                chair.Add(stretchersConstructor(legCenters, config.legWidth, config.legHeight));
            }

            // Generate seat
            chair.Add(Seat0(Vector3.up*config.legHeight, config.seatWidth, config.seatDepth, config.seatHeight));

            // Generate chair back
            Vector3 backCenter = Vector3.up*(config.legHeight + config.seatHeight) +
                                 Vector3.forward*(config.seatDepth - config.legWidth)/2;
            var backConstructor = backConstructors.GetRandom();
            chair.Add(backConstructor(backCenter, config.seatWidth, config.legWidth, config.backHeight));

            // Generate armrests
            if (config.hasArmrests)
            {
                var armrestsConstructor = armrestsConstructors.GetRandom();
                chair.Add(armrestsConstructor(config.seatWidth, config.seatDepth, backCenter, config.backHeight,
                    config.legWidth));
            }

            chair.Paint(config.color);

            return chair;
        }

        private static MeshDraft Leg0(Vector3 center, float width, float height)
        {
            var draft = MeshDraft.Hexahedron(width, width, height, false);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        private static MeshDraft Seat0(Vector3 center, float width, float length, float height)
        {
            var draft = MeshDraft.Hexahedron(width, length, height, false);
            draft.Move(center + Vector3.up*height/2);
            return draft;
        }

        public static MeshDraft BeamDraft(Vector3 from, Vector3 to, float width, float rotation = 0)
        {
            var up = to - from;
            var draft = MeshDraft.Hexahedron(width, width, up.magnitude, false);
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
