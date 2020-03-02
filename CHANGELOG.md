# Changelog

## [0.2.3] - 2020-03-02
This release adds Unity Package Manager support and [FastNoise](https://github.com/Auburns/FastNoise_CSharp) library integration.
### Added
* Integrated [FastNoise](https://github.com/Auburns/FastNoise_CSharp) library, see Noise example on its usage.
* MeshDraft: Added new Capsule primitive.
* PTUtils: Added CreateMeshRenderer and CreateTexture.
* Geometry:
  * Added PointOnSegment2, PointsOnSegment2, PointOnSegment3 and PointsOnSegment3.
  * Added overrides with center for circle point samplers.
* Segment2 and Segment3:
  * Added GetPoints.
  * Added center and aabb.
* Circle2:
  * Added GetPoints.
  * Added perimeter and area.
* Circle3: Added perimeter and area.
* Sphere: Added area and volume.
* Draw, DebugE, GizmosE, GLE: Added Segment2 and Segment3.

### Changes
* Folder structure and some of the namespaces were changed in accordance with the package manager guidelines.
* Tests are now distributed with the library.
* Minimum supported Unity version is now 2019.2.
* Dropped support for the old scripting runtime.
* Buildings: Renamed PlanningStrategy to Planner, ConstructionStrategy to Constructor.
* LowPolyTerrain: Renamed classes and simplified a bit.
* SDF: Include instructions are now installation type dependent.
* RandomE: Renamed PointOnSegment and PointOnCircle generators.
* Draw: Renamed DrawWireRay to DrawRay.

### Fixes
* Geometry: Fixed PointsInCircle3* returning incorrect type.
* MeshDraft:
  * Fixed uv-less cylinders missing sides.
  * Fixed normal calculation for degenerate quads in AddQuad.
  * Fixed an exception in FlatRevolutionSurface ([#58](https://github.com/Syomus/ProceduralToolkit/issues/58)).
* MeshFilterExtension: Removed useless Instantiate breaking the link to the mesh.


## [0.2.2] - 2018-09-18
This release brings polygon tessellation, clipping and offsetting, straight skeleton generation and a completely rewritten Buildings example.
### Features
* Integrated [LibTessDotNet](https://github.com/speps/LibTessDotNet) library, see Tessellator wrapper class and its example for details.
* Integrated [Clipper](http://www.angusj.com/delphi/clipper.php) library, use PathClipper and PathOffsetter for seamless interoperability with Unity.
* Added [straight skeleton](https://en.wikipedia.org/wiki/Straight_skeleton) generator.
* New architecture for the Buildings example, all roof types now support convex and concave polygons.
* Added Circle3.
* Added serializable RendererProperties, use Renderer.ApplyProperties extension to apply them to a target renderer.

### Improvements
* Geometry:
  * Added OffsetPolygon, GetAngle, GetAngleBisector, Polygon2, StarPolygon2, PointsInCircle*, PointsOnSphere, GetRect and GetCircumradius.
* Segment2 and Segment3:
  * Added direction and length.
* Intersect3D:
  * Added SphereSphere.
* VectorE:
  * Added RotateCCW, RotateCW45, RotateCCW45, RotateCW90 and RotateCCW90.
* PTUtils:
  * Added new constants: Sqrt05, Sqrt2, Sqrt5, GoldenAngle.
* MeshDraft:
  * Added calculateBounds flag for ToMesh.
  * Added support for 32 bit Mesh index buffers.
* CompoundMeshDraft:
  * Added calculateBounds flag for ToMeshWithSubMeshes.
  * Added support for 32 bit Mesh index buffers.
  * Added Move, Rotate and other methods from MeshDraft.

### Changes
* Minimum supported Unity version is now 2018.1.
* Renamed PTUtils string constants.
* Renamed Circle to Circle2.
* Moved `Examples\Resources` to `Examples\Common\Resources`.
* MeshDraft: Made normals optional in AddTriangle and AddQuad.
* Intersect2D: Removed LineLine overload with IntersectionType return type.


## [0.2.1] - 2018-06-20
This release brings new computational geometry algorithms and geometric primitives.
### Features
* Added Circle, Sphere, Line2, Line3, Segment2 and Segment3. All new primitives are Serializable and IFormattable.
* Added many new geometry algorithms. See [this wiki page](https://github.com/Syomus/ProceduralToolkit/wiki/Geometry-algorithms) for a full matrix.
* Unit tests for the toolkit are now public and can be found [here](https://github.com/Syomus/ProceduralToolkit.Tests).

### Improvements
* Added assembly definition files.
* ColorHSV: Added new casting and arithmetic operators.
* CompoundMeshDraft: Fixed conversion to Mesh for Unity 2017.3.
* LowPolyTerrain: Added MeshCollider.
* RandomE: Added rotation2, PointOnSegment overloads, PointOnCircle, PointInCircle, PointOnSphere, PointInSphere.
* SDF: Fixed the WebGL build.

### Changes
* Minimum supported Unity version is now 2017.3.
* Split geometry algorithms into separate classes: Closest, Distance and Intersect.
* Moved PointOn* methods from PTUtils to Geometry.


## [0.2.0] - 2018-01-05
This release is mainly focused on refactoring and documenting of the codebase, expect better performance and overall stability. 
That being said, there are a few new features as well.
### Features
* Added a new shader library with signed distance functions and easings, see `ProceduralToolkit\Shaders\*.cginc` files and `ProceduralToolkit\Examples\SDF` folder for details.
* Added a new Geometry class with many computational geometry algorithms (distance, intersection, closest point, etc.).
* Added a new example showing usage of DebugE, GLE and GizmosE, see `ProceduralToolkit\Examples\Drawing` folder for details.
* Added a new CompoundMeshDraft class for generation of large meshes and submeshes.

### Improvements
* Lots of bugfixes and optimizations.
* ArrayE:
  * Visits now support IEqualityComparer.
* CellularAutomata:
  * CellularAutomaton.Ruleset is now Serializable and has a RulesetDrawer with a dropdown with common rulesets.
* ColorHSV:
  * ColorHSV is now Serializable and has a PropertyDrawer.
* Directions:
  * Added DirectionsExtensions to simplify the usage of flags.
* GLE:
  * Added BeginLines method, see usage in the Drawer example.
* MeshDraft:
  * Refactored and uv-mapped all primitives (except dodecahedron), use generateUV flag to control the generation of uv.
  * Most of the MeshDraft methods now return the reference to the draft to allow chaining of operations.
  * New API for construction: AddTriangle, AddQuad, AddTriangleFan, AddTriangleStrip, AddBaselessPyramid, AddFlatTriangleBand and AddFlatQuadBand.
* RandomE:
  * Added xRotation, yRotation and zRotation.
  * Added insideUnitCircle3XY, insideUnitCircle3XZ and insideUnitCircle3YZ.
  * Added PointOnSegment, PointInRect and PointOnRect.
* TextureE:
  * Added overloads for RectInt and Color arrays.
* VectorE:
  * Added ToVector2XY, ToVector2XZ and ToVector2YZ for Vector3.
  * Added Angle360 for Vector3.
* Breakout:
  * The game level now resets when all bricks are destroyed.
  * Added a simple pool for bricks.

### Changes
* Minimum supported Unity version is now 2017.2.
* Renamed most of the examples and restructured folders.
* Removed Boids example.
* Removed Vector2Int, use UnityEngine.Vector2Int instead.
* ArrayE:
  * Renamed all visit methods.
* CellularAutomata:
  * Moved CellularAutomaton to the core library.
  * Removed CellState and replaced it with bool to simplify integration.
* Draw:
  * Separated raster and vector methods into partial classes.
* MeshDraft:
  * Removed some of the primitives, use new MeshDraft.Add* methods instead.
* PTUtils:
  * Moved vector operations out from PTUtils to VectorE.
* VectorE:
  * Renamed Only* methods.
* Buildings:
  * Foundation polygon winding is now clockwise.
  * The generated mesh is now split into submeshes and uses multiple materials.
* Characters:
  * New sprites for characters.
  * Split CharacterGenerator into a generator, a configurator and a character.
* Mazes:
  * Removed RandomBreadthFirstTraversal algorithm.
* ProceduralToolkitMenu:
  * Removed "About" window.
  * Removed shapes from the editor menu that duplicated the Unity functionality.


## [0.1.9] - 2017-03-23
### Features
* Added CharacterGenerator example with integrated NameGenerator which replaces Datasets class
* Added VectorE class with useful extensions for vectors
* Rewritten building generator with new layouting system
* Added debug shaders for UV1, UV2, normals, tangents and bitangents

### Improvements
* MeshDraft:
  * Added missing tangents and uv channels
  * Added Clear method to clear all vertex data
  * Added ToMesh converter with ref Mesh
  * Added Serializable attribute
  * Added vertexCount shortcut
  * Added FlipUVHorizontally, FlipUVVertically and Spherify with similar extensions in MeshE
* Refactored all examples, added undo support for in-editor generation
* Added Draw.WireRay with wrappers in DebugE, GizmosE, GLE
* Added PTUtils.Angle360, PointOnSpheroid and InverseLerp
* Removed extra triangles from dodecahedron constructor
* Added cube, spheroid, teardrop and revolution surface constructors
* A few fixes to remove warnings in Unity 5.6
* Added RandomE.PointInBounds
* Added ColorHSV.ToHtmlStringRGB and Color.ToHtmlStringRGB
* Added extensions for LinkedListNode: NextOrFirst and PreviousOrLast
* Updated documentation and comments
* A few bugfixes and simplifications

### Changes
* Dropped support for Unity 5.3
* Removed Datasets class and name generators from RandomE and with CharacterGenerator example 
due to error in IL2CPP compiler which was preventing builds for Android and iOS. 
Name generators are now in NameGenerator class and string constants are in PTUtils.
* Replaced CircularList with extension methods in ArrayE: GetLooped and SetLooped
* Renamed partial Hexahedron constructor to PartialBox
* Moved all common example classes to `ProceduralToolkit\Examples\Common`


## [0.1.8] - 2016-09-06
### Features
* Added GizmosE, GLE and DebugE helper classes with extra drawing methods such as DrawWireCircle, 
DrawWireHemisphere, DrawWireCone, and others. GizmosE and DebugE have API similar to Gizmos and Debug, 
GLE is just a bunch of wrappers over GL.Vertex which follow Gizmos convention.
* Added Draw helper class with generic drawing methods
* Added Gradient Skybox shader
* Added new random color generators: RandomE.ColorHue, ColorSaturation and ColorValue
* Added static palette generators: ColorHSV.GetAnalogousPalette, GetTriadicPalette and GetTetradicPalette
* Added random palette generators: RandomE.AnalogousPalette, TriadicPalette, TetradicPalette
* 3D examples now have dynamic generated skyboxes
* Added custom inspectors for BuildingGenerator, ChairGenerator and LowPolyTerrainGenerator. 
Changing values does not cause regeneration, but otherwise they behave the same way as UI controls and work in editor and play mode.

### Improvements
* Optimized examples, mesh and texture helper classes to produce less garbage
* Fixed shadowcasting in vertex color shaders
* Fixed typos and bugs in some classes
* Added TextureE.Clear overload with ref array
* Added PTUtils.SignedAngle for 2D and 3D vectors
* Added PTUtils.PointsOnCircle3XY and PTUtils.PointsOnCircle3YZ
* Added RandomE.onUnitCircle3XY, RandomE.onUnitCircle3XZ, RandomE.onUnitCircle3YZ
* TerrainMesh received a full rewrite and is now LowPolyTerrainGenerator
* Added ColorHSV.WithH, WithS, WithV, WithA, WithOffsetH, Lerp and various useful methods (ToString, GetHashCode and the like)

### Changes
* Dropped support for Unity 5.2
* Renamed PTUtils.PointsOnCircle3 to PTUtils.PointsOnCircle3XZ
* Renamed Khrushchyovka to BuildingGenerator to avoid confusion
* PTUtils.PointsOnCircle methods now receive angle in degrees instead of radians
* Renamed RandomE.onUnitCircle to RandomE.onUnitCircle2
* Renamed TerrainMesh to LowPolyTerrainGenerator
* Moved CameraRotator to Examples\UI
* Replaced RGB<->HSV conversion methods with Unity implementation
* Removed "Procedural Toolkit/Unlit Color" shader, use "Unlit/Color" instead
* RandomE.colorHSV now returns ColorHSV
* Moved drawing methods from PTUtils to Draw


## [0.1.7] - 2015-12-19
### Features
* Added ColorHSV class
* Added Vector2Int class
* Added ArrayE class
* Added CircularList class
* Added CellularAutomaton example

### Improvements
* Added PTUtils.DrawFilledCircle
* Refactored drawing methods, eliminated overdraw
* Fixed a few ui and generator bugs in examples

### Changes
* Removed ColorE.HSVToRGB, use ColorHSV instead
* New CameraRotator behaviour, now requires Image to operate
* PTUtils.WuLine is now DrawAALine, BresenhamLine is DrawLine


## [0.1.6] - 2015-10-02
* Major refactoring, new UI for all examples
* Removed ColorE.ToHex and ColorE.FromHex
* Moved all static MeshDraft constructors from MeshE to MeshDraft
* Added PTUtils.PointsOnCircle3 and PTUtils.PointsOnCircle2
* Added RandomBreadthFirstTraversal maze algorithm
* Added datasets for names and last names
* RandomE
  * Removed meshDraft
  * Added onUnitCircle, insideUnitSquare, onUnitSquare, insideUnitCube, name constructors, new string and char constructors
* TextureE
  * Removed Texture2D constructors
  * Added DrawCircle, DrawGradient and DrawGradientRect


## [0.1.5] - 2015-05-14
* Unity 5 support
* Added mesh saver
* Added new example: Breakout
* Added Standard Vertex Color shader


## [0.1.4] - 2015-02-08
* Added new examples: Primitives and Mazes
* Added Texture extensions
* Added Bresenham and Wu line drawing algorithms


## [0.1.3] - 2014-11-15
* Added new example: Khrushchyovka
* Added knapsack problem solver
* Added new Color extensions
* Added random choice for Dictionary
* Added new mesh drafts
* Added specular vertex color shader
* Improved examples


## [0.1.2] - 2014-11-10
* Added new example: chair generator
* Small additions and fixes


## [0.1.1] - 2014-10-26
* Added new primitives: cylinder and sphere
* Added mesh extensions: move, rotate, scale, paint, flip faces
* Added RandomE.Range methods
* Added PTUtils methods for points on circle and sphere
* Improved documentation


## [0.1] - 2014-10-13
* First release
