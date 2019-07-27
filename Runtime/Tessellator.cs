using ProceduralToolkit.LibTessDotNet;
using System.Collections.Generic;
using UnityEngine;
using Mesh = UnityEngine.Mesh;

namespace ProceduralToolkit
{
    /// <summary>
    /// LibTessDotNet wrapper
    /// </summary>
    public class Tessellator
    {
        /// <summary>
        /// If true, will remove empty (zero area) polygons.
        /// </summary>
        public bool removeEmptyPolygons { get { return tess.NoEmptyPolygons; } set { tess.NoEmptyPolygons = value; } }
        /// <summary>
        /// Vertices of the tessellated mesh.
        /// </summary>
        public ContourVertex[] vertices { get { return tess.Vertices; } }
        /// <summary>
        /// Number of vertices in the tessellated mesh.
        /// </summary>
        public int vertexCount { get { return tess.VertexCount; } }
        /// <summary>
        /// Indices of the tessellated mesh. See <see cref="ElementType"/> for details on data layout.
        /// </summary>
        public int[] indices { get { return tess.Elements; } }
        /// <summary>
        /// Number of elements in the tessellated mesh.
        /// </summary>
        public int indexCount { get { return tess.ElementCount; } }

        private readonly Tess tess = new Tess {NoEmptyPolygons = true};

        /// <summary>
        /// Adds a closed contour to be tessellated.
        /// </summary>
        /// <param name="vertices"> Vertices of the contour. </param>
        /// <param name="forceOrientation">
        /// Orientation of the contour.
        /// <see cref="ContourOrientation.Original"/> keeps the orientation of the input vertices.
        /// <see cref="ContourOrientation.Clockwise"/> and <see cref="ContourOrientation.CounterClockwise"/> 
        /// force the vertices to have a specified orientation.
        /// </param>
        public void AddContour(IList<Vector2> vertices, ContourOrientation forceOrientation = ContourOrientation.Original)
        {
            var contour = new ContourVertex[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector2 vertex = vertices[i];
                contour[i].Position = new Vec3(vertex.x, vertex.y, 0);
            }
            tess.AddContour(contour, forceOrientation);
        }

        /// <summary>
        /// Adds a closed contour to be tessellated.
        /// </summary>
        /// <param name="vertices"> Vertices of the contour. </param>
        /// <param name="forceOrientation">
        /// Orientation of the contour.
        /// <see cref="ContourOrientation.Original"/> keeps the orientation of the input vertices.
        /// <see cref="ContourOrientation.Clockwise"/> and <see cref="ContourOrientation.CounterClockwise"/> 
        /// force the vertices to have a specified orientation.
        /// </param>
        public void AddContour(IList<Vector3> vertices, ContourOrientation forceOrientation = ContourOrientation.Original)
        {
            var contour = new ContourVertex[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                Vector3 vertex = vertices[i];
                contour[i].Position = new Vec3(vertex.x, vertex.y, vertex.z);
            }
            tess.AddContour(contour, forceOrientation);
        }

        /// <summary>
        /// Adds a closed contour to be tessellated.
        /// </summary>
        /// <param name="vertices"> Vertices of the contour. </param>
        /// <param name="forceOrientation">
        /// Orientation of the contour.
        /// <see cref="ContourOrientation.Original"/> keeps the orientation of the input vertices.
        /// <see cref="ContourOrientation.Clockwise"/> and <see cref="ContourOrientation.CounterClockwise"/> 
        /// force the vertices to have a specified orientation.
        /// </param>
        public void AddContour(IList<ContourVertex> vertices, ContourOrientation forceOrientation = ContourOrientation.Original)
        {
            tess.AddContour(vertices, forceOrientation);
        }

        /// <summary>
        /// Tessellates the input contours.
        /// </summary>
        /// <param name="windingRule"> Winding rule used for tessellation. See <see cref="WindingRule"/> for details. </param>
        /// <param name="elementType"> Tessellation output type. See <see cref="ElementType"/> for details. </param>
        /// <param name="polySize"> Number of vertices per polygon if output is polygons. </param>
        /// <param name="combineCallback"> Interpolator used to determine the data payload of generated vertices. </param>
        /// <param name="normal"> Normal of the input contours. If set to zero, the normal will be calculated during tessellation. </param>
        public void Tessellate(WindingRule windingRule = WindingRule.EvenOdd, ElementType elementType = ElementType.Polygons, int polySize = 3,
            CombineCallback combineCallback = null, Vector3 normal = new Vector3())
        {
            tess.Tessellate(windingRule, elementType, polySize, combineCallback, new Vec3(normal.x, normal.y, normal.z));
        }

        /// <summary>
        /// Converts the tessellated mesh to MeshDraft.
        /// </summary>
        public MeshDraft ToMeshDraft()
        {
            var draft = new MeshDraft();
            for (int i = 0; i < vertexCount; i++)
            {
                Vec3 vertex = vertices[i].Position;
                draft.vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }
            draft.triangles.AddRange(indices);
            return draft;
        }

        /// <summary>
        /// Converts the tessellated mesh to MeshDraft.
        /// </summary>
        public void ToMeshDraft(ref MeshDraft draft)
        {
            draft.vertices.Clear();
            draft.triangles.Clear();
            for (int i = 0; i < vertexCount; i++)
            {
                Vec3 vertex = vertices[i].Position;
                draft.vertices.Add(new Vector3(vertex.X, vertex.Y, vertex.Z));
            }
            draft.triangles.AddRange(indices);
        }

        /// <summary>
        /// Converts the tessellated mesh to Mesh.
        /// </summary>
        public Mesh ToMesh()
        {
            return ToMeshDraft().ToMesh();
        }

        /// <summary>
        /// Converts the tessellated mesh to Mesh.
        /// </summary>
        public void ToMesh(ref Mesh mesh)
        {
            ToMeshDraft().ToMesh(ref mesh);
        }
    }
}
