using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProceduralToolkit.Editor
{
    /// <summary>
    /// Mesh saving utility available at `MeshFilter context menu > Save Mesh`
    /// </summary>
    public class MeshFilterExtension
    {
        private const string menuPath = "CONTEXT/MeshFilter/Save Mesh";
        private const string title = "Save Mesh";
        private const string lastDirectoryKey = "ProceduralToolkit.SaveMesh.LastDirectory";
        private const string defaultDirectory = "Assets";

        /// <summary>
        /// Saves a copy of the mesh as an asset and links the MeshFilter to it. The source mesh is never modified.
        /// Overwriting an existing mesh asset updates it in place, keeping references to it intact.
        /// </summary>
        [MenuItem(menuPath)]
        private static void SaveMesh(MenuCommand menuCommand)
        {
            var meshFilter = (MeshFilter) menuCommand.context;
            var mesh = meshFilter.sharedMesh;

            string path = EditorUtility.SaveFilePanelInProject(title, mesh.name, "asset", title, GetLastDirectory());
            if (string.IsNullOrEmpty(path))
            {
                // The user closed the dialogue
                return;
            }

            EditorPrefs.SetString(lastDirectoryKey, Path.GetDirectoryName(path));

            Mesh savedMesh = SaveMeshAsset(mesh, path);
            Undo.RecordObject(meshFilter, title);
            meshFilter.sharedMesh = savedMesh;
            PrefabUtility.RecordPrefabInstancePropertyModifications(meshFilter);

            EditorGUIUtility.PingObject(savedMesh);
        }

        [MenuItem(menuPath, true)]
        private static bool SaveMeshTest(MenuCommand menuCommand)
        {
            var meshFilter = (MeshFilter) menuCommand.context;
            if (meshFilter.sharedMesh == null)
            {
                return false;
            }
            return Selection.GetFiltered<MeshFilter>(SelectionMode.Unfiltered).Length <= 1;
        }

        /// <summary>
        /// Saves the <paramref name="mesh"/> data at the <paramref name="path"/>.
        /// An existing mesh asset is overwritten in place, an asset of any other type is replaced.
        /// </summary>
        /// <returns>
        /// The saved asset
        /// </returns>
        private static Mesh SaveMeshAsset(Mesh mesh, string path)
        {
            string assetName = Path.GetFileNameWithoutExtension(path);

            if (AssetDatabase.LoadMainAssetAtPath(path) is not Mesh existingMesh)
            {
                var copy = Object.Instantiate(mesh);
                copy.name = assetName;
                // Remove all flags so that the mesh is visible and can be saved
                copy.hideFlags = HideFlags.None;
                // Replaces an existing asset of another type, if there is one
                AssetDatabase.CreateAsset(copy, path);
                return copy;
            }

            bool overwrite = existingMesh != mesh;
            if (overwrite)
            {
                // Overwrite in place to keep references to the asset intact
                EditorUtility.CopySerialized(mesh, existingMesh);
            }
            existingMesh.name = assetName;
            EditorUtility.SetDirty(existingMesh);
            AssetDatabase.SaveAssetIfDirty(existingMesh);
            if (overwrite)
            {
                // CopySerialized doesn't update the GPU copy of the mesh, renderers keep drawing the old geometry.
                // Unloading makes Unity reload the saved asset from disk on next access, existing references stay valid.
                Resources.UnloadAsset(existingMesh);
            }
            return existingMesh;
        }

        private static string GetLastDirectory()
        {
            // EditorPrefs are shared between projects, the folder may not exist in this one
            string directory = EditorPrefs.GetString(lastDirectoryKey, defaultDirectory);
            return AssetDatabase.IsValidFolder(directory) ? directory : defaultDirectory;
        }
    }
}
