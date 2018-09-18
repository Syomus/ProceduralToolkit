# Procedural Toolkit 0.2.2

Procedural Toolkit is a procedural generation library for the Unity game engine.

**Warning: This is a programming toolkit, editor support is limited.**

[![Donate](https://syomus.com/ProceduralToolkit/donate_paypal.gif)](https://www.paypal.me/ProceduralToolkit/5usd)
[![Donate](https://syomus.com/ProceduralToolkit/donate_kofi.png)](https://ko-fi.com/ProceduralToolkit) | 
[GitHub](https://github.com/Syomus/ProceduralToolkit) |
[Asset Store](https://assetstore.unity.com/packages/tools/utilities/procedural-toolkit-16508) |
[Issues](https://github.com/Syomus/ProceduralToolkit/issues) |
[Support email](mailto:proceduraltoolkit@syomus.com)

## Installation instructions
Requires **Unity 2018.1** or later. Tested on Windows, WebGL, Android and iOS.

You can install Procedural Toolkit from any of the following sources:

1. Import from the [Asset Store](https://assetstore.unity.com/packages/tools/utilities/procedural-toolkit-16508)
2. Download from the [Releases page](https://github.com/Syomus/ProceduralToolkit/releases). Same packages as in the Asset Store.
3. Clone/download the repository from [GitHub](https://github.com/Syomus/ProceduralToolkit) and put it in a subfolder (preferably `/Assets/ProceduralToolkit`).

If you have any previous versions of the toolkit installed it is highly recommended to delete them before importing a newer version.

Renaming or moving toolkit's folder from the default path (`/Assets/ProceduralToolkit`) will break .cginc includes in the example shaders, otherwise it is safe to move the toolkit however you want.

The folder containing example classes and scenes (`ProceduralToolkit/Examples`) can be safely removed. The same is true for `ProceduralToolkit/Shaders`, if you are not using examples.

### Getting started
After installation you can import ProceduralToolkit namespace and start building your own PCG systems:
```C#
using UnityEngine;
using ProceduralToolkit;

public class ReadmeExample : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(string.Format("<color=#{0}>{0}</color>", RandomE.colorHSV.ToHtmlStringRGB()));
    }
}
```

## I have a problem with Procedural Toolkit
First, please search the [open issues](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aopen)
and [closed issues](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aclosed)
to see if your issue hasn't already been reported. If it does exist, add a
:thumbsup: to the issue to indicate this is also an issue for you, and add a
comment to the existing issue if there is extra information you can contribute.

If you can't find a matching issue, open a [new issue](https://github.com/Syomus/ProceduralToolkit/issues/new/choose),
choose the right template and provide us with enough information to investigate further. 
Alternatively, you can send a message to the [support email](mailto:proceduraltoolkit@syomus.com).

## Contributing
See [CONTRIBUTING](/.github/CONTRIBUTING.md) for a full guide on how you can help.

If you're looking for something to work on, check out the [help wanted](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aissue+is%3Aopen+label%3A"help+wanted") label.

If you just want to show your appreciation, you can send a donation through [PayPal](https://www.paypal.me/ProceduralToolkit/5usd) or [Ko-fi](https://ko-fi.com/ProceduralToolkit).

## Toolkit structure
### Scripts/
* [ArrayE](/Scripts/ArrayE.cs): Various Array and List extensions, such as looped getters/setters and flood fills.
* [CellularAutomaton](/Scripts/CellularAutomaton.cs): Generic cellular automaton for two-state rulesets. 
Common rulesets can be found in CellularAutomaton.Ruleset.
* [ClipperUtility](/Scripts/ClipperUtility.cs): Utility class for conversion of Clipper data from and to Unity format.
* [ColorE](/Scripts/ColorE.cs): Color extensions, HTML colors, Gradient constructors.
* [ColorHSV](/Scripts/ColorHSV.cs): Serializable representation of color in HSV model.
* [CompoundMeshDraft](/Scripts/CompoundMeshDraft.cs): Helper class for mesh generation supporting large meshes and submeshes.
* [DebugE](/Scripts/DebugE.cs): Collection of drawing methods similar to Debug.DrawLine.
* [Directions](/Scripts/Directions.cs): Enum with direction flags along three axes.
* [Draw](/Scripts/Draw.cs): Collection of generic vector drawing algorithms.
* [DrawRaster](/Scripts/DrawRaster.cs): Collection of generic raster drawing algorithms.
* [GizmosE](/Scripts/GizmosE.cs): Collection of drawing methods similar to Gizmos.
* [GLE](/Scripts/GLE.cs): Collection of GL drawing methods similar to Gizmos.
* [MeshDraft](/Scripts/MeshDraft.cs): Helper class for mesh generation.
* [MeshDraftPrimitives](/Scripts/MeshDraftPrimitives.cs): Constructors for MeshDraft primitives.
* [MeshE](/Scripts/MeshE.cs): Mesh extensions.
* [PathClipper](/Scripts/PathClipper.cs) and [PathOffsetter](/Scripts/PathOffsetter.cs): [Clipper](http://www.angusj.com/delphi/clipper.php) library wrappers.
* [PTUtils](/Scripts/PTUtils.cs): Various useful methods and constants.
* [RandomE](/Scripts/RandomE.cs): Class for generating random data. Contains extensions for arrays and other collections.
* [RendererProperties](/Scripts/RendererProperties.cs): 
Serializable Renderer properties, use Renderer.ApplyProperties extension to apply them to a target renderer.
* [Tessellator](/Scripts/Tessellator.cs): [LibTessDotNet](https://github.com/speps/LibTessDotNet) library wrapper.
* [TextureE](/Scripts/TextureE.cs): Texture extensions.
* [VectorE](/Scripts/VectorE.cs): Vector extensions.

### Scripts/Buildings/
* [BuildingGenerator](/Scripts/Buildings/BuildingGenerator.cs): 
The main generation class. Generates buildings based on input configuration and strategies, reusable. See examples for details.
* [ILayout](/Scripts/Buildings/Interfaces/ILayout.cs), [ILayoutElement](/Scripts/Buildings/Interfaces/ILayoutElement.cs), 
[IConstructible](/Scripts/Buildings/Interfaces/IConstructible.cs), [Layout](/Scripts/Buildings/Layout.cs), 
[HorizontalLayout](/Scripts/Buildings/HorizontalLayout.cs), [VerticalLayout](/Scripts/Buildings/VerticalLayout.cs): 
Interfaces and classes used in facade layout generation.
* [IFacadePlanningStrategy](/Scripts/Buildings/Interfaces/IFacadePlanningStrategy.cs), 
[IFacadeConstructionStrategy](/Scripts/Buildings/Interfaces/IFacadeConstructionStrategy.cs), 
[IRoofPlanningStrategy](/Scripts/Buildings/Interfaces/IRoofPlanningStrategy.cs), 
[IRoofConstructionStrategy](/Scripts/Buildings/Interfaces/IRoofConstructionStrategy.cs): 
Interfaces for strategies controlling the details of the building generation process.
* [FacadePlanningStrategy](/Scripts/Buildings/FacadePlanningStrategy.cs), [FacadeConstructionStrategy](/Scripts/Buildings/FacadeConstructionStrategy.cs), 
[RoofPlanningStrategy](/Scripts/Buildings/RoofPlanningStrategy.cs), [RoofConstructionStrategy](/Scripts/Buildings/RoofConstructionStrategy.cs): 
Serializable wrappers for strategy interfaces.
* [ProceduralRoofs](/Scripts/Buildings/ProceduralRoofs.cs): A collection of roof constructors for the building generator.

### Scripts/Geometry/
See [this wiki page](https://github.com/Syomus/ProceduralToolkit/wiki/Geometry-algorithms) for a matrix of available algorithms.
* [Circle2](/Scripts/Geometry/Circle2.cs), [Circle3](/Scripts/Geometry/Circle3.cs), [Sphere](/Scripts/Geometry/Sphere.cs), 
[Line2](/Scripts/Geometry/Line2.cs), [Line3](/Scripts/Geometry/Line3.cs), 
[Segment2](/Scripts/Geometry/Segment2.cs), [Segment3](/Scripts/Geometry/Segment3.cs): 
Representations of geometric primitives.
* [Geometry](/Scripts/Geometry/Geometry.cs): Utility class for computational geometry algorithms, contains various point samplers and helper methods.
* [Closest2D](/Scripts/Geometry/Closest2D.cs), [Closest3D](/Scripts/Geometry/Closest3D.cs): 
Collection of closest point(s) algorithms.
* [Distance2D](/Scripts/Geometry/Distance2D.cs), [Distance3D](/Scripts/Geometry/Distance3D.cs): 
Collection of distance calculation algorithms.
* [Intersect2D](/Scripts/Geometry/Intersect2D.cs), [Intersect3D](/Scripts/Geometry/Intersect3D.cs): 
Collection of intersection algorithms.
* [IntersectionType](/Scripts/Geometry/IntersectionType.cs): Enum used in intersection algorithms.
* [Intersections](/Scripts/Geometry/Intersections) folder: Structures containing information about intersections.

### Scripts/Geometry/StraightSkeleton/
Classes used in [straight skeleton](https://en.wikipedia.org/wiki/Straight_skeleton) generation.
* [StraightSkeletonGenerator](/Scripts/Geometry/StraightSkeleton/StraightSkeletonGenerator.cs): 
A straight skeleton generator, computes a straight skeleton for the input polygon, reusable. 
The generation algorithm is loosely based on the work of Tom Kelly (2014) 
[Unwritten procedural modeling with the straight skeleton](http://www.twak.co.uk/2014/02/unwritten-procedural-modeling-with.html).
* [StraightSkeleton](/Scripts/Geometry/StraightSkeleton/StraightSkeleton.cs): A straight skeleton representation.
* [Plan](/Scripts/Geometry/StraightSkeleton/Plan.cs): Representation of the active plan during generation process.

### Scripts/Clipper/
The [Clipper](http://www.angusj.com/delphi/clipper.php) library. 
Use [PathClipper](/Scripts/PathClipper.cs) and [PathOffsetter](/Scripts/PathOffsetter.cs) for seamless interoperability with Unity.

### Scripts/LibTessDotNet/
The [LibTessDotNet](https://github.com/speps/LibTessDotNet) library. 
Use [Tessellator](/Scripts/Tessellator.cs) for seamless interoperability with Unity.

### Scripts/Editor/
* [ColorHSVDrawer](/Scripts/Editor/ColorHSVDrawer.cs): PropertyDrawer for ColorHSV.
* [MeshFilterExtension](/Scripts/Editor/MeshFilterExtension.cs): Mesh saving utility available at `MeshFilter context menu > Save Mesh`.
* [ProceduralToolkitMenu](/Scripts/Editor/ProceduralToolkitMenu.cs): Submenu with constructors for primitives at `GameObject > Procedural Toolkit`.
* [RulesetDrawer](/Scripts/Editor/RulesetDrawer.cs): PropertyDrawer for CellularAutomaton.Ruleset.

### Shaders/
* [Common.cginc](/Shaders/Common.cginc): Collection of shaping and debug functions.
* [Easing.cginc](/Shaders/Easing.cginc): Normalized easing functions.
* [SDF.cginc](/Shaders/SDF.cginc): Collection of signed distance functions.
* [Transitions.cginc](/Shaders/Transitions.cginc): Collection of transition animations.
* [Gradient Skybox](/Shaders/Gradient%20Skybox.shader): Simple gradient skybox.
* [Debug](/Shaders/Debug) folder: Debug shaders for some of the most common mesh channels.
* [VertexColor](/Shaders/VertexColor) folder: Textureless shaders for use with color information from the vertices.

## Examples

### [Buildings](https://syomus.com/ProceduralToolkit/Buildings)
![](https://syomus.com/ProceduralToolkit/screenshot-buildings.png)

A fully procedural building generator, creates entire mesh from scratch and paints it's vertices. 
Keep in mind that the generated mesh has no uv map so using it with Standard shader is pointless.
* BuildingGeneratorComponent: A simple minimal example on how you can use BuildingGenerator
* BuildingGeneratorConfigurator: A configurator for BuildingGenerator with UI and editor controls.
* BuildingGeneratorReuse: An example on how you can reuse the same generator to generate multiple buildings.
* PolygonAsset: A ScriptableObject container for vertices.
* ProceduralFacadePlanningStrategy: A fully procedural facade planning strategy, controls the layouts of the facades.
* ProceduralFacadeConstructionStrategy: A fully procedural facade construction strategy, used in conjunction with ProceduralFacadePlanningStrategy.
* ProceduralRoofPlanningStrategy: A fully procedural roof planning strategy, generates a roof description based on the input config.
* ProceduralRoofConstructionStrategy: A fully procedural roof construction strategy, used in conjunction with ProceduralRoofPlanningStrategy.
* ProceduralFacadeElements: A collection of fully procedural facade panels for the building generator.
* BuildingGeneratorConfiguratorEditor: A custom inspector for BuildingGeneratorConfigurator.

### [Chairs](https://syomus.com/ProceduralToolkit/Chairs)
![](https://syomus.com/ProceduralToolkit/screenshot-chairs.png)

A fully procedural chair generator, creates entire mesh from scratch and paints it's vertices.
* ChairGenerator: Main generator class. Generates chairs based on input configuration.
* ChairGeneratorConfigurator: Configurator for ChairGenerator with UI and editor controls.
* ChairGeneratorConfiguratorEditor: Custom inspector for ChairGeneratorConfigurator.
* Armrests, Backs, Stretchers: Chair parts constructors.

### [Low Poly Terrain](https://syomus.com/ProceduralToolkit/LowPolyTerrain)
![](https://syomus.com/ProceduralToolkit/screenshot-low-poly-terrain.png)

A simple Perlin noise based low poly terrain generator. 
Doesn't support chunking or anything like that, just an example of how you can use a noise function in a plane generation algorithm.
* LowPolyTerrainGenerator: Main generator class. Generates terrain based on input configuration.
* LowPolyTerrainGeneratorConfigurator: Configurator for LowPolyTerrainGenerator with UI and editor controls.
* LowPolyTerrainGeneratorConfiguratorEditor: Custom inspector for LowPolyTerrainGeneratorConfigurator.

### [Signed Distance Functions](https://syomus.com/ProceduralToolkit/SDF)
![](https://syomus.com/ProceduralToolkit/screenshot-sdf.png)

A collection of shaders showing how you can utilise functions from the shader library.
* StarPolygon: A simple shader that draws a star polygon and shows a difference between "normal" and "cheap" functions.
* Shapes: A demonstration of some of the available shapes.
* DistanceOperations: A square and a circle combined with different functions.
* Easing: All easing functions in one shader showing the ease curve and the easig motion.
* Transitions: An example showing the sequencing and animation techniques.
* Animation: A more complex example on how you can create patterns and animate them in shader.

### [Cellular Automata](https://syomus.com/ProceduralToolkit/CellularAutomata)
![](https://syomus.com/ProceduralToolkit/screenshot-cellular-automata.png)

A demonstration of CellularAutomaton from the main library, draws the automaton simulation on a texture. 
Note that some of the rulesets need noise value different from the default setting.
* CellularAutomatonConfigurator: Configurator for the automaton with UI controls.

### [Mazes](https://syomus.com/ProceduralToolkit/Mazes)
![](https://syomus.com/ProceduralToolkit/screenshot-mazes.png)

A generic maze generator, draws the maze generation process on a texture.
* MazeGenerator: Main generator class. Generates mazes based on input configuration.
* MazeGeneratorConfigurator: Configurator for MazeGenerator with UI controls.
* Maze: Maze graph representation.

### [Breakout](https://syomus.com/ProceduralToolkit/Breakout)
![](https://syomus.com/ProceduralToolkit/screenshot-breakout.png)

A [Breakout](https://en.wikipedia.org/wiki/Breakout_(video_game)) clone with procedurally generated levels.
* Breakout: Game engine and level generator.
* BreakoutConfigurator: Configurator for the game with UI controls.
* Brick: Disables game object on collision.

### [Characters](https://syomus.com/ProceduralToolkit/Characters)
![](https://syomus.com/ProceduralToolkit/screenshot-characters.gif)

A simple 2D character generator. Sprites made by <a href="http://kenney.nl/">Kenney</a>.
* Character: A container for sprite renderers
* CharacterGenerator: Generates a sprite set and a name for character.
* CharacterGeneratorConfigurator: Configurator for CharacterGenerator.
* NameGenerator: Generates a random name from a large array of names.

### [Primitives](https://syomus.com/ProceduralToolkit/Primitives)
![](https://syomus.com/ProceduralToolkit/screenshot-primitives.png)

A demonstration of some of the available MeshDraft primitives.
* Cylinder, Dodecahedron, FlatSphere, Hexahedron...: Mesh generators that can be configured via the inspector.

### Drawing
Three identical shapes made with three different methods: DebugE, GLE and GizmosE.

### Clipper
A simple example demonstrating the api's of PathClipper and PathOffsetter.

### Tessellator
An example showing the usage of Tesselator.

### StraightSkeleton
An StraightSkeletonGenerator example showing how you can generate a straight skeleton from a polygon and use the result.

### Examples/Resources
UI prefabs and the skybox material used in examples.

### Examples/Common
* SkyBoxGenerator: Skybox generator, assuming that scene uses gradient skybox shader, animates transitions to new parameters every few seconds.
* ButtonControl, SliderControl, TextControl, ToggleControl: UI controls for generators.
* CameraRotator: Orbiting camera controller.
* ConfiguratorBase: Base class for configurators.

## License
[MIT](LICENSE.txt)
