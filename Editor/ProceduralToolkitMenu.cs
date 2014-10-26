using System;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    public class ProceduralToolkitMenu
    {
        public const string version = "v0.1";

        private const string menuPath = "GameObject/Procedural Toolkit/";
        private const string create = "Create ";

        private const int platonicSolids = 0;
        private const string tetrahedron = "Tetrahedron";
        private const string hexahedron = "Hexahedron";
        private const string octahedron = "Octahedron";
        private const string dodecahedron = "Dodecahedron";
        private const string icosahedron = "Icosahedron";

        private const int other = 20;
        private const string plane = "Plane";
        private const string pyramid = "Pyramid";
        private const string prism = "Prism";
        private const string cylinder = "Cylinder";

        [MenuItem("Help/About Procedural Toolkit")]
        private static void About()
        {
            AboutWindow.Open();
        }

        private static void Template(string name, Func<Mesh> mesh)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Undo.RegisterCreatedObjectUndo(go, create + name);
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            go.name = name;
            go.GetComponent<MeshFilter>().mesh = mesh();
        }

        #region Platonic solids

        [MenuItem(menuPath + tetrahedron, false, platonicSolids + 0)]
        public static void Tetrahedron()
        {
            Template(tetrahedron, () => MeshE.Tetrahedron(1));
        }

        [MenuItem(menuPath + hexahedron, false, platonicSolids + 1)]
        public static void Hexahedron()
        {
            Template(hexahedron, () => MeshE.Hexahedron(1, 1, 1));
        }

        [MenuItem(menuPath + octahedron, false, platonicSolids + 2)]
        public static void Octahedron()
        {
            Template(octahedron, () => MeshE.Octahedron(1));
        }

        [MenuItem(menuPath + dodecahedron, false, platonicSolids + 3)]
        public static void Dodecahedron()
        {
            Template(dodecahedron, () => MeshE.Dodecahedron(1));
        }

        [MenuItem(menuPath + icosahedron, false, platonicSolids + 4)]
        public static void Icosahedron()
        {
            Template(icosahedron, () => MeshE.Icosahedron(1));
        }

        #endregion Platonic solids

        #region Other

        [MenuItem(menuPath + plane, false, other + 0)]
        public static void Plane()
        {
            Template(plane, () => MeshE.Plane(10, 10, 10, 10));
        }

        [MenuItem(menuPath + pyramid, false, other + 1)]
        public static void Pyramid()
        {
            Template(pyramid, () => MeshE.Pyramid(1, 6, 1));
        }

        [MenuItem(menuPath + prism, false, other + 2)]
        public static void Prism()
        {
            Template(prism, () => MeshE.Prism(1, 16, 1));
        }

        [MenuItem(menuPath + cylinder, false, other + 3)]
        public static void Cylinder()
        {
            Template(cylinder, () => MeshE.Cylinder(1, 16, 1));
        }

        #endregion Other
    }

    public class AboutWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<AboutWindow>(true, "About Procedural Toolkit");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Version: " + ProceduralToolkitMenu.version);

            EditorGUILayout.SelectableLabel("Copyright © 2014 Daniil Basmanov");
            EditorGUILayout.SelectableLabel("Icon by Iuliana Koroviakovskaia");

            EditorGUILayout.Space();
            if (GUILayout.Button("Repository"))
            {
                Application.OpenURL("https://github.com/Syomus/ProceduralToolkit/");
            }
        }
    }
}