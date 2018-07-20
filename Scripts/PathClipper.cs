using System;
using System.Collections.Generic;
using ProceduralToolkit.ClipperLib;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// Clipper wrapper
    /// </summary>
    public class PathClipper
    {
        /// <summary>
        /// If set to true, polygons returned by clipping operations will have orientations opposite to their normal orientations.
        /// </summary>
        public bool reverseSolution { get { return clipper.ReverseSolution; } set { clipper.ReverseSolution = value; } }
        /// <summary>
        /// If set to true, polygons returned by clipping operations will be strictly simple, otherwise they may be weakly simple.
        /// Computationally expensive.
        /// </summary>
        public bool strictlySimple { get { return clipper.StrictlySimple; } set { clipper.StrictlySimple = value; } }
        /// <summary>
        /// If set to true, collinear vertices in input paths will not be removed before clipping.
        /// </summary>
        public bool preserveCollinear { get { return clipper.PreserveCollinear; } set { clipper.PreserveCollinear = value; } }

        private readonly Clipper clipper;

        /// <summary>
        /// Constructs a new PathClipper
        /// </summary>
        /// <param name="initOptions"> A set of flags controlling the corresponding properties. </param>
        public PathClipper(InitOptions initOptions = 0)
        {
            clipper = new Clipper((int) initOptions);
        }

        /// <summary>
        /// Adds a path to a Clipper object in preparation for clipping.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="polyType"> Type of the path (Subject or Clip). </param>
        /// <param name="closed"> Controls whether the path is closed. Clipping paths must always be closed. </param>
        /// <returns> False if the path is invalid for clipping, true otherwise. </returns>
        public bool AddPath(IList<Vector2> path, PolyType polyType, bool closed = true)
        {
            return clipper.AddPath(ClipperUtility.ToIntPath(path), polyType, closed);
        }

        /// <summary>
        /// Adds a path to a Clipper object in preparation for clipping.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="polyType"> Type of the path (Subject or Clip). </param>
        /// <param name="closed"> Controls whether the path is closed. Clipping paths must always be closed. </param>
        /// <returns> False if the path is invalid for clipping, true otherwise. </returns>
        public bool AddPath(List<Vector2> path, PolyType polyType, bool closed = true)
        {
            return clipper.AddPath(ClipperUtility.ToIntPath(path), polyType, closed);
        }

        /// <summary>
        /// Adds a path to a Clipper object in preparation for clipping.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="polyType"> Type of the path (Subject or Clip). </param>
        /// <param name="closed"> Controls whether the path is closed. Clipping paths must always be closed. </param>
        /// <returns> False if the path is invalid for clipping, true otherwise. </returns>
        public bool AddPath(List<IntPoint> path, PolyType polyType, bool closed = true)
        {
            return clipper.AddPath(path, polyType, closed);
        }

        /// <summary>
        /// Adds paths to a Clipper object in preparation for clipping.
        /// </summary>
        /// <param name="paths"> List of paths. </param>
        /// <param name="polyType"> Type of the path (Subject or Clip). </param>
        /// <param name="closed"> Controls whether the path is closed. Clipping paths must always be closed. </param>
        /// <returns> False if all paths are invalid for clipping, true otherwise. </returns>
        public bool AddPaths(List<List<Vector2>> paths, PolyType polyType, bool closed = true)
        {
            return clipper.AddPaths(ClipperUtility.ToIntPaths(paths), polyType, closed);
        }

        /// <summary>
        /// Adds paths to a Clipper object in preparation for clipping.
        /// </summary>
        /// <param name="paths"> List of paths. </param>
        /// <param name="polyType"> Type of the path (Subject or Clip). </param>
        /// <param name="closed"> Controls whether the path is closed. Clipping paths must always be closed. </param>
        /// <returns> False if all paths are invalid for clipping, true otherwise. </returns>
        public bool AddPaths(List<List<IntPoint>> paths, PolyType polyType, bool closed = true)
        {
            return clipper.AddPaths(paths, polyType, closed);
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The List that will receive the result of the clipping operation. </param>
        /// <param name="fillType"> Fill rule that will be applied to the paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref List<List<Vector2>> output, PolyFillType fillType = PolyFillType.pftEvenOdd)
        {
            return Clip(clipType, ref output, fillType, fillType);
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The List that will receive the result of the clipping operation. </param>
        /// <param name="subjectFillType"> Fill rule that will be applied to the subject paths. </param>
        /// <param name="clipFillType"> Fill rule that will be applied to the clip paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref List<List<Vector2>> output, PolyFillType subjectFillType, PolyFillType clipFillType)
        {
            var intOutput = new List<List<IntPoint>>();
            bool succeeded = clipper.Execute(clipType, intOutput, subjectFillType, clipFillType);
            ClipperUtility.ToVector2Paths(intOutput, ref output);
            return succeeded;
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The List that will receive the result of the clipping operation. </param>
        /// <param name="fillType"> Fill rule that will be applied to the paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref List<List<IntPoint>> output, PolyFillType fillType = PolyFillType.pftEvenOdd)
        {
            return Clip(clipType, ref output, fillType, fillType);
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The List that will receive the result of the clipping operation. </param>
        /// <param name="subjectFillType"> Fill rule that will be applied to the subject paths. </param>
        /// <param name="clipFillType"> Fill rule that will be applied to the clip paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref List<List<IntPoint>> output, PolyFillType subjectFillType, PolyFillType clipFillType)
        {
            return clipper.Execute(clipType, output, subjectFillType, clipFillType);
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The PolyTree that will receive the result of the clipping operation. </param>
        /// <param name="fillType"> Fill rule that will be applied to the paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref PolyTree output, PolyFillType fillType = PolyFillType.pftEvenOdd)
        {
            return Clip(clipType, ref output, fillType, fillType);
        }

        /// <summary>
        /// Performs the clipping operation.
        /// Can be called multiple times without reassigning subject and clip polygons
        /// (ie when different clipping operations are required on the same polygon sets).
        /// </summary>
        /// <param name="clipType"> Type of the clipping operation. </param>
        /// <param name="output"> The PolyTree that will receive the result of the clipping operation. </param>
        /// <param name="subjectFillType"> Fill rule that will be applied to the subject paths. </param>
        /// <param name="clipFillType"> Fill rule that will be applied to the clip paths. </param>
        /// <returns> True if the operation was successful, false otherwise. </returns>
        public bool Clip(ClipType clipType, ref PolyTree output, PolyFillType subjectFillType, PolyFillType clipFillType)
        {
            return clipper.Execute(clipType, output, subjectFillType, clipFillType);
        }

        /// <summary>
        /// Clears all paths from the Clipper object, allowing new paths to be assigned.
        /// </summary>
        public void Clear()
        {
            clipper.Clear();
        }

        [Flags]
        public enum InitOptions : int
        {
            ReverseSolution = 1,
            StrictlySimple = 2,
            PreserveCollinear = 4,
        }
    }
}
