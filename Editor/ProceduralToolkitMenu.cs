using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// Submenu with constructors for primitives at `GameObject > Procedural Toolkit`
    /// </summary>
    public class ProceduralToolkitMenu
    {
        private const string primitivesPath = "GameObject/Procedural Toolkit/";

        private const string tetrahedron = "Tetrahedron";
        private const string octahedron = "Octahedron";
        private const string dodecahedron = "Dodecahedron";
        private const string icosahedron = "Icosahedron";

        private const string pyramid = "Pyramid";
        private const string bipyramid = "Bipyramid";
        private const string prism = "Prism";
        private const string flatSphere = "Flat Sphere";

        private static void PrimitiveTemplate(MenuCommand menuCommand, MeshDraft draft)
        {
            Mesh mesh = draft.ToMesh();
            // Bake the convex collision data in player builds, otherwise the MeshCollider inspector shows a warning
            mesh.SetPreBakeCollisionMesh(true, true);

            var go = ObjectFactory.CreateGameObject(draft.name, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
            Undo.SetCurrentGroupName("Create " + draft.name);
            go.GetComponent<MeshFilter>().sharedMesh = mesh;
            go.GetComponent<MeshRenderer>().sharedMaterial = GetDefaultMaterial();
            var meshCollider = go.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;

            ObjectFactory.PlaceGameObject(go, menuCommand.context as GameObject);
            GameObjectUtility.EnsureUniqueNameForSibling(go);
            Selection.activeObject = go;
        }

        private static Material GetDefaultMaterial()
        {
            RenderPipelineAsset pipeline = GraphicsSettings.currentRenderPipeline;
            return pipeline != null
                ? pipeline.defaultMaterial
                : AssetDatabase.GetBuiltinExtraResource<Material>("Default-Material.mat");
        }

        #region Platonic solids

        [MenuItem(primitivesPath + tetrahedron, priority = 1)]
        public static void Tetrahedron(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Tetrahedron(0.5f));
        }

        [MenuItem(primitivesPath + octahedron, priority = 2)]
        public static void Octahedron(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Octahedron(0.5f));
        }

        [MenuItem(primitivesPath + dodecahedron, priority = 3)]
        public static void Dodecahedron(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Dodecahedron(0.5f));
        }

        [MenuItem(primitivesPath + icosahedron, priority = 4)]
        public static void Icosahedron(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Icosahedron(0.5f));
        }

        #endregion Platonic solids

        #region Other

        [MenuItem(primitivesPath + pyramid, priority = 20)]
        public static void Pyramid(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Pyramid(0.5f, 6, 0.5f));
        }

        [MenuItem(primitivesPath + bipyramid, priority = 21)]
        public static void Bipyramid(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.BiPyramid(0.5f, 6, 1));
        }

        [MenuItem(primitivesPath + prism, priority = 22)]
        public static void Prism(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.Prism(0.5f, 16, 1));
        }

        [MenuItem(primitivesPath + flatSphere, priority = 23)]
        public static void FlatSphere(MenuCommand menuCommand)
        {
            PrimitiveTemplate(menuCommand, MeshDraft.FlatSphere(0.5f, 16, 8));
        }

        #endregion Other
    }
}
