using System;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// Submenu with constructors for primitives at `GameObject > Procedural Toolkit`
    /// </summary>
    public class ProceduralToolkitMenu
    {
        public const string version = "0.2.2";

        private const string primitivesPath = "GameObject/Procedural Toolkit/";
        private const string create = "Create ";

        private const string tetrahedron = "Tetrahedron";
        private const string octahedron = "Octahedron";
        private const string dodecahedron = "Dodecahedron";
        private const string icosahedron = "Icosahedron";

        private const string pyramid = "Pyramid";
        private const string prism = "Prism";

        private static void PrimitiveTemplate(string name, Func<Mesh> mesh)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Undo.RegisterCreatedObjectUndo(go, create + name);
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            go.name = name;
            go.GetComponent<MeshFilter>().mesh = mesh();
        }

        #region Platonic solids

        [MenuItem(primitivesPath + tetrahedron)]
        public static void Tetrahedron()
        {
            PrimitiveTemplate(tetrahedron, () => MeshDraft.Tetrahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + octahedron)]
        public static void Octahedron()
        {
            PrimitiveTemplate(octahedron, () => MeshDraft.Octahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + dodecahedron)]
        public static void Dodecahedron()
        {
            PrimitiveTemplate(dodecahedron, () => MeshDraft.Dodecahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + icosahedron)]
        public static void Icosahedron()
        {
            PrimitiveTemplate(icosahedron, () => MeshDraft.Icosahedron(1).ToMesh());
        }

        #endregion Platonic solids

        #region Other

        [MenuItem(primitivesPath + pyramid)]
        public static void Pyramid()
        {
            PrimitiveTemplate(pyramid, () => MeshDraft.Pyramid(1, 6, 1).ToMesh());
        }

        [MenuItem(primitivesPath + prism)]
        public static void Prism()
        {
            PrimitiveTemplate(prism, () => MeshDraft.Prism(0.5f, 16, 2).ToMesh());
        }

        #endregion Other
    }
}
