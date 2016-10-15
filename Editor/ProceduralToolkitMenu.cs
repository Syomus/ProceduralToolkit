using System;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    public class ProceduralToolkitMenu
    {
        public const string version = "0.1.8";

        private const string primitivesPath = "GameObject/Procedural Toolkit/";
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
        private const string sphere = "Sphere";

        [MenuItem("Help/About Procedural Toolkit")]
        private static void About()
        {
            AboutWindow.Open();
        }

        private static void PrimitiveTemplate(string name, Func<Mesh> mesh)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Undo.RegisterCreatedObjectUndo(go, create + name);
            UnityEngine.Object.DestroyImmediate(go.GetComponent<Collider>());
            go.name = name;
            go.GetComponent<MeshFilter>().mesh = mesh();
        }

        #region Platonic solids

        [MenuItem(primitivesPath + tetrahedron, false, platonicSolids + 0)]
        public static void Tetrahedron()
        {
            PrimitiveTemplate(tetrahedron, () => MeshE.Tetrahedron(1));
        }

        [MenuItem(primitivesPath + hexahedron, false, platonicSolids + 1)]
        public static void Hexahedron()
        {
            PrimitiveTemplate(hexahedron, () => MeshE.Hexahedron(1, 1, 1));
        }

        [MenuItem(primitivesPath + octahedron, false, platonicSolids + 2)]
        public static void Octahedron()
        {
            PrimitiveTemplate(octahedron, () => MeshE.Octahedron(1));
        }

        [MenuItem(primitivesPath + dodecahedron, false, platonicSolids + 3)]
        public static void Dodecahedron()
        {
            PrimitiveTemplate(dodecahedron, () => MeshE.Dodecahedron(1));
        }

        [MenuItem(primitivesPath + icosahedron, false, platonicSolids + 4)]
        public static void Icosahedron()
        {
            PrimitiveTemplate(icosahedron, () => MeshE.Icosahedron(1));
        }

        #endregion Platonic solids

        #region Other

        [MenuItem(primitivesPath + plane, false, other + 0)]
        public static void Plane()
        {
            PrimitiveTemplate(plane, () => MeshE.Plane(10, 10, 10, 10));
        }

        [MenuItem(primitivesPath + pyramid, false, other + 1)]
        public static void Pyramid()
        {
            PrimitiveTemplate(pyramid, () => MeshE.Pyramid(1, 6, 1));
        }

        [MenuItem(primitivesPath + prism, false, other + 2)]
        public static void Prism()
        {
            PrimitiveTemplate(prism, () => MeshE.Prism(1, 16, 1));
        }

        [MenuItem(primitivesPath + cylinder, false, other + 3)]
        public static void Cylinder()
        {
            PrimitiveTemplate(cylinder, () => MeshE.Cylinder(1, 16, 1));
        }

        [MenuItem(primitivesPath + sphere, false, other + 4)]
        public static void Sphere()
        {
            PrimitiveTemplate(sphere, () => MeshE.Sphere(1, 16, 16));
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
            EditorGUILayout.SelectableLabel("Version: " + ProceduralToolkitMenu.version + "\n" +
                                            "Copyright © Daniil Basmanov\n" +
                                            "Icon by Iuliana Koroviakovskaia", GUILayout.Height(50));

            EditorGUILayout.Space();
            if (GUILayout.Button("Repository"))
            {
                Application.OpenURL("https://github.com/Syomus/ProceduralToolkit/");
            }
            if (GUILayout.Button("Asset Store"))
            {
                Application.OpenURL("https://www.assetstore.unity3d.com/#!/content/16508");
            }
            if (GUILayout.Button("Issues"))
            {
                Application.OpenURL("https://github.com/Syomus/ProceduralToolkit/issues");
            }
            if (GUILayout.Button("Support email"))
            {
                Application.OpenURL("mailto:proceduraltoolkit@syomus.com");
            }
        }
    }
}