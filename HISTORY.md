## Version history
### 0.1.9 (2017.03.23)
#### Features:
* Added CharacterGenerator example with integrated NameGenerator which replaces Datasets class
* Added VectorE class with useful extensions for vectors
* Rewritten building generator with new layouting system
* Added debug shaders for UV1, UV2, normals, tangents and bitangents

#### Improvements:
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

#### Changes:
* Dropped support for Unity 5.3
* Removed Datasets class and name generators from RandomE and with CharacterGenerator example due to error in IL2CPP compiler which was preventing builds for Android and iOS. Name generators are now in NameGenerator class and string constants are in PTUtils.
* Replaced CircularList with extension methods in ArrayE: GetLooped and SetLooped
* Renamed partial Hexahedron constructor to PartialBox
* Moved all common example classes to `ProceduralToolkit\Examples\Common`

### 0.1.8 (2016.09.06)
#### Features:
* Added GizmosE, GLE and DebugE helper classes with extra drawing methods such as DrawWireCircle, DrawWireHemisphere, DrawWireCone, and others. GizmosE and DebugE have API similar to Gizmos and Debug, GLE is just a bunch of wrappers over GL.Vertex which follow Gizmos convention.
* Added Draw helper class with generic drawing methods
* Added Gradient Skybox shader
* Added new random color generators: RandomE.ColorHue, ColorSaturation and ColorValue
* Added static palette generators: ColorHSV.GetAnalogousPalette, GetTriadicPalette and GetTetradicPalette
* Added random palette generators: RandomE.AnalogousPalette, TriadicPalette, TetradicPalette
* 3D examples now have dynamic generated skyboxes
* Added custom inspectors for BuildingGenerator, ChairGenerator and LowPolyTerrainGenerator. Changing values does not cause regeneration, but otherwise they bahave the same way as UI controls and work in editor and play mode.

#### Improvements:
* Optimized examples, mesh and texture helper classes to produce less garbage
* Fixed shadowcasting in vertex color shaders
* Fixed typos and bugs in some classes
* Added TextureE.Clear overload with ref array
* Added PTUtils.SignedAngle for 2D and 3D vectors
* Added PTUtils.PointsOnCircle3XY and PTUtils.PointsOnCircle3YZ
* Added RandomE.onUnitCircle3XY, RandomE.onUnitCircle3XZ, RandomE.onUnitCircle3YZ
* TerrainMesh received a full rewrite and is now LowPolyTerrainGenerator
* Added ColorHSV.WithH, WithS, WithV, WithA, WithOffsetH, Lerp and various useful methods (ToString, GetHashCode and the like)

#### Changes:
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

### 0.1.7 (2015.12.19)
#### Features:
* Added ColorHSV class
* Added Vector2Int class
* Added ArrayE class
* Added CircularList class
* Added CellularAutomaton example

#### Improvements:
* Added PTUtils.DrawFilledCircle
* Refactored drawing methods, eliminated overdraw
* Fixed a few ui and generator bugs in examples

#### Changes:
* Removed ColorE.HSVToRGB, use ColorHSV instead
* New CameraRotator behaviour, now requires Image to operate
* PTUtils.WuLine is now DrawAALine, BresenhamLine is DrawLine

### 0.1.6 (2015.10.02)
* Major refactoring, new UI for all examples
* Removed ColorE.ToHex and ColorE.FromHex
* Moved all static MeshDraft constructors from MeshE to MeshDraft
* Added PTUtils.PointsOnCircle3 and PTUtils.PointsOnCircle2
* Added RandomBreadthFirstTraversal maze algorithm
* Added datasets for names and last names
* RandomE
  * Removed meshDraft
  * Added onUnitCircle, insideUnitSquare, onUnitSquare, insideUnitCube, name constructors, new string and char constructos
* TextureE
  * Removed Texture2D constructors
  * Added DrawCircle, DrawGradient and DrawGradientRect

### 0.1.5 (2015.05.14)
* Unity 5 support
* Added mesh saver
* Added new example: Breakout
* Added Standard Vertex Color shader

### 0.1.4 (2015.02.08)
* Added new examples: Primitives and Mazes
* Added Texture extensions
* Added Bresenham and Wu line drawing algorithms

### 0.1.3 (2014.11.15)
* Added new example: Khrushchyovka
* Added knapsack problem solver
* Added new Color extensions
* Added random choice for Dictionary
* Added new mesh drafts
* Added specular vertex color shader
* Improved examples

### 0.1.2 (2014.11.10)
* Added new example: chair generator
* Small additions and fixes

### 0.1.1 (2014.10.26)
* Added new primitives: cylinder and sphere
* Added mesh extensions: move, rotate, scale, paint, flip faces
* Added RandomE.Range methods
* Added PTUtils methods for points on circle and sphere
* Improved documentation

### 0.1 (2014.10.13)
* First release
