using System;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    public class ProceduralToolkitMenu
    {
        public const string version = "0.1.9";

        private const string primitivesPath = "GameObject/Procedural Toolkit/";
        private const string create = "Create ";

        private const int platonicSolids = 0;
        private const string tetrahedron = "Tetrahedron";
        private const string cube = "Cube";
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
            PrimitiveTemplate(tetrahedron, () => MeshDraft.Tetrahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + cube, false, platonicSolids + 1)]
        public static void Cube()
        {
            PrimitiveTemplate(cube, () => MeshDraft.Cube(1).ToMesh());
        }

        [MenuItem(primitivesPath + octahedron, false, platonicSolids + 2)]
        public static void Octahedron()
        {
            PrimitiveTemplate(octahedron, () => MeshDraft.Octahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + dodecahedron, false, platonicSolids + 3)]
        public static void Dodecahedron()
        {
            PrimitiveTemplate(dodecahedron, () => MeshDraft.Dodecahedron(1).ToMesh());
        }

        [MenuItem(primitivesPath + icosahedron, false, platonicSolids + 4)]
        public static void Icosahedron()
        {
            PrimitiveTemplate(icosahedron, () => MeshDraft.Icosahedron(1).ToMesh());
        }

        #endregion Platonic solids

        #region Other

        [MenuItem(primitivesPath + plane, false, other + 0)]
        public static void Plane()
        {
            PrimitiveTemplate(plane, () => MeshDraft.Plane(10, 10, 10, 10).ToMesh());
        }

        [MenuItem(primitivesPath + pyramid, false, other + 1)]
        public static void Pyramid()
        {
            PrimitiveTemplate(pyramid, () => MeshDraft.Pyramid(1, 6, 1).ToMesh());
        }

        [MenuItem(primitivesPath + prism, false, other + 2)]
        public static void Prism()
        {
            PrimitiveTemplate(prism, () => MeshDraft.Prism(1, 16, 1).ToMesh());
        }

        [MenuItem(primitivesPath + cylinder, false, other + 3)]
        public static void Cylinder()
        {
            PrimitiveTemplate(cylinder, () => MeshDraft.Cylinder(1, 16, 1).ToMesh());
        }

        [MenuItem(primitivesPath + sphere, false, other + 4)]
        public static void Sphere()
        {
            PrimitiveTemplate(sphere, () => MeshDraft.Sphere(1, 16, 16).ToMesh());
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