using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProceduralToolkit.Examples.Axe
{
    /// <summary>
    /// A procedural axe generator
    /// </summary>
    public static class AxeGenerator
    {
        [Serializable]
        public class Config
        {
            public float handleWidth = 0.07f;
            public float handleHeight = 0.7f;
            
            public float headHeightPercent = 0.2f;
            public float headBottomWidthPercent = 0.8f;
            public float headTopWidth = 0.6f;

            public float headTopAngle = 10f;
            public float headBottomAngle = 10f;
            internal bool isTwoSided;

            public float GetHeadHeight() {
                return handleHeight * headHeightPercent;
            }

            public float GetHeadBottomWidth() {
                return headTopWidth * headBottomWidthPercent;
            }

            public float GetHeadTopShift() {
                var angleRad = headTopAngle * Mathf.Deg2Rad;
                return Mathf.Tan(angleRad) * headTopWidth;
            }

            public float GetHeadBottomShift() {
                var angleRad = headBottomAngle* Mathf.Deg2Rad;
                return Mathf.Tan(angleRad) * GetHeadBottomWidth();
            }
        }

        public static MeshDraft Axe(Config config)
        {
            Assert.IsTrue(config.handleWidth > 0);
            Assert.IsTrue(config.handleHeight > 0);
            

            var draft = new MeshDraft {name = "Axe"};

            // Generate handle
            draft.Add(Handle(config.handleWidth, config.handleHeight));

            // Generate head
            draft.Add(Head(config), 1);
            
            return draft;
        }

        static readonly int cylinderSegments = 8;

        private static MeshDraft Handle(float width, float height) {
            var draft = MeshDraft.Cylinder(width / 2, cylinderSegments, height);
            draft.Move(Vector3.up * height / 2);
            return draft;
        }

        private static MeshDraft Head(Config config) {
            var headHeight = config.GetHeadHeight();
            var headRadius = config.handleWidth * 1.1f / 2;
            var draft = MeshDraft.Cylinder(headRadius, cylinderSegments, headHeight);

            MeshDraft blade = Blade(config);
            draft.Add(blade);

            if (config.isTwoSided) {
                MeshDraft blade2 = Blade(config);
                blade2.Rotate(Quaternion.Euler(0, 180, 0));
                draft.Add(blade2);
            }

            /// add little shift so top plane not overlap
            var shift = 0.001f;
            draft.Move(Vector3.up * (config.handleHeight - headHeight / 2 - shift));
            return draft;
        }

        private static MeshDraft Blade(Config config) {
            var draft = new MeshDraft();
            var headHeight = config.GetHeadHeight();
            var headRadius = config.handleWidth / 2;

            Vector3 a = new Vector3(0, headHeight/2, -headRadius);
            Vector3 b = new Vector3(config.headTopWidth, headHeight / 2 + config.GetHeadTopShift());
            Vector3 c = new Vector3(config.GetHeadBottomWidth(), -headHeight / 2 - config.GetHeadBottomShift());
            Vector3 d = new Vector3(0,-headHeight/2, -headRadius);

            // front side
            draft.AddQuad(a, b, c, d);

            // back side
            var a1 = new Vector3(a.x, a.y, -a.z);
            var d1 = new Vector3(d.x, d.y, -d.z);
            draft.AddQuad(d1, c, b, a1);

            // top side
            draft.AddTriangle(a1, b, a);

            // bottom side
            draft.AddTriangle(d, c, d1);

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