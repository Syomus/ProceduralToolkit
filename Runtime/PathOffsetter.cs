using System.Collections.Generic;
using ProceduralToolkit.ClipperLib;
using UnityEngine;

namespace ProceduralToolkit
{
    /// <summary>
    /// ClipperOffset wrapper
    /// </summary>
    public class PathOffsetter
    {
        /// <summary>
        /// Maximum distance in multiples of delta that vertices can be offset from their original positions before squaring is applied.
        /// Only relevant when JoinType = jtMiter.
        /// </summary>
        public double arcTolerance
        {
            get => clipperOffset.ArcTolerance;
            set => clipperOffset.ArcTolerance = value;
        }
        /// <summary>
        /// Maximum acceptable imprecision when arcs are approximated in an offsetting operation.
        /// Only relevant when JoinType = jtRound and/or EndType = etRound.
        /// </summary>
        public double miterLimit
        {
            get => clipperOffset.MiterLimit;
            set => clipperOffset.MiterLimit = value;
        }

        private readonly ClipperOffset clipperOffset;

        /// <summary>
        /// Constructs a new PathOffsetter
        /// </summary>
        /// <param name="miterLimit">
        /// Maximum distance in multiples of delta that vertices can be offset from their original positions before squaring is applied.
        /// Only relevant when JoinType = jtMiter.
        /// </param>
        /// <param name="arcTolerance">
        /// Maximum acceptable imprecision when arcs are approximated in an offsetting operation.
        /// Only relevant when JoinType = jtRound and/or EndType = etRound.
        /// </param>
        public PathOffsetter(double miterLimit = 2.0, double arcTolerance = 0.25)
        {
            clipperOffset = new ClipperOffset(miterLimit, arcTolerance);
        }

        /// <summary>
        /// Adds a path to a ClipperOffset object in preparation for offsetting.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="joinType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/JoinType.htm </param>
        /// <param name="endType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/EndType.htm </param>
        public void AddPath(IList<Vector2> path, JoinType joinType = JoinType.jtMiter, EndType endType = EndType.etClosedPolygon)
        {
            clipperOffset.AddPath(ClipperUtility.ToIntPath(path), joinType, endType);
        }

        /// <summary>
        /// Adds a path to a ClipperOffset object in preparation for offsetting.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="joinType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/JoinType.htm </param>
        /// <param name="endType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/EndType.htm </param>
        public void AddPath(List<Vector2> path, JoinType joinType = JoinType.jtMiter, EndType endType = EndType.etClosedPolygon)
        {
            clipperOffset.AddPath(ClipperUtility.ToIntPath(path), joinType, endType);
        }

        /// <summary>
        /// Adds a path to a ClipperOffset object in preparation for offsetting.
        /// </summary>
        /// <param name="path"> Vertices of the path. </param>
        /// <param name="joinType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/JoinType.htm </param>
        /// <param name="endType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/EndType.htm </param>
        public void AddPath(List<IntPoint> path, JoinType joinType = JoinType.jtMiter, EndType endType = EndType.etClosedPolygon)
        {
            clipperOffset.AddPath(path, joinType, endType);
        }

        /// <summary>
        /// Adds paths to a ClipperOffset object in preparation for offsetting.
        /// </summary>
        /// <param name="paths"> List of paths. </param>
        /// <param name="joinType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/JoinType.htm </param>
        /// <param name="endType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/EndType.htm </param>
        public void AddPaths(List<List<Vector2>> paths, JoinType joinType = JoinType.jtMiter, EndType endType = EndType.etClosedPolygon)
        {
            clipperOffset.AddPaths(ClipperUtility.ToIntPaths(paths), joinType, endType);
        }

        /// <summary>
        /// Adds paths to a ClipperOffset object in preparation for offsetting.
        /// </summary>
        /// <param name="paths"> List of paths. </param>
        /// <param name="joinType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/JoinType.htm </param>
        /// <param name="endType"> See http://www.angusj.com/delphi/clipper/documentation/Docs/Units/ClipperLib/Types/EndType.htm </param>
        public void AddPaths(List<List<IntPoint>> paths, JoinType joinType = JoinType.jtMiter, EndType endType = EndType.etClosedPolygon)
        {
            clipperOffset.AddPaths(paths, joinType, endType);
        }

        /// <summary>
        /// Performs the offset operation.
        /// Can be called multiple times, offsetting the same paths by different amounts (ie using different deltas).
        /// </summary>
        /// <param name="output">The List that will receive the result of the offset operation.</param>
        /// <param name="delta">
        /// The amount to which the supplied paths will be offset.
        /// Positive values expand polygons and negative values shrink them.
        /// Scaled by <see cref="ClipperUtility.ClipperScale"/>.
        /// </param>
        public void Offset(ref List<List<Vector2>> output, double delta)
        {
            var intOutput = new List<List<IntPoint>>();
            clipperOffset.Execute(ref intOutput, delta*ClipperUtility.ClipperScale);
            ClipperUtility.ToVector2Paths(intOutput, ref output);
        }

        /// <summary>
        /// Performs the offset operation.
        /// Can be called multiple times, offsetting the same paths by different amounts (ie using different deltas).
        /// </summary>
        /// <param name="output">The List that will receive the result of the offset operation.</param>
        /// <param name="delta">
        /// The amount to which the supplied paths will be offset.
        /// Positive values expand polygons and negative values shrink them.
        /// Not scaled.
        /// </param>
        public void Offset(ref List<List<IntPoint>> output, double delta)
        {
            clipperOffset.Execute(ref output, delta);
        }

        /// <summary>
        /// Performs the offset operation.
        /// Can be called multiple times, offsetting the same paths by different amounts (ie using different deltas).
        /// </summary>
        /// <param name="output">The PolyTree that will receive the result of the offset operation.</param>
        /// <param name="delta">
        /// The amount to which the supplied paths will be offset.
        /// Positive values expand polygons and negative values shrink them.
        /// Not scaled.
        /// </param>
        public void Offset(ref PolyTree output, double delta)
        {
            clipperOffset.Execute(ref output, delta);
        }

        /// <summary>
        /// Clears all paths from the ClipperOffset object, allowing new paths to be assigned.
        /// </summary>
        public void Clear()
        {
            clipperOffset.Clear();
        }
    }
}
