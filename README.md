# Procedural Toolkit 0.2.3

Procedural Toolkit is a procedural generation library for the Unity game engine.

**Warning: This is a programming toolkit, editor support is limited.**

[![Donate](https://syomus.com/ProceduralToolkit/donate_paypal.gif)](https://www.paypal.me/ProceduralToolkit/5usd)
[![Donate](https://syomus.com/ProceduralToolkit/donate_kofi.png)](https://ko-fi.com/ProceduralToolkit) | 
[GitHub](https://github.com/Syomus/ProceduralToolkit) |
[Asset Store](https://assetstore.unity.com/packages/tools/utilities/procedural-toolkit-16508) |
[Issues](https://github.com/Syomus/ProceduralToolkit/issues) |
[Support email](mailto:proceduraltoolkit@syomus.com)

## Installation instructions
Requires **Unity 2019.2** or later. Tested on Windows, WebGL, Android and iOS.

There are several ways to install Procedural Toolkit:

### Package Manager
The best way is to install this library as a [Git package](https://docs.unity3d.com/Manual/upm-git.html) using the Package Manager.
First, make sure that you to have Git installed and available in your system's PATH.

For Unity 2019.3 and later you can add the package using the link below, notice the upm branch at the end of the line:
```
https://github.com/Syomus/ProceduralToolkit.git#upm
```

For Unity 2019.2 you will need to add the following line to your project's `manifest.json`:
```
"com.syomus.proceduraltoolkit": "https://github.com/Syomus/ProceduralToolkit.git#upm",
```

It should look like this:
```
{
  "dependencies": {
    "com.syomus.proceduraltoolkit": "https://github.com/Syomus/ProceduralToolkit.git#upm",
    "com.unity.package-manager-ui": "1.0.0",
    "com.unity.modules.ai": "1.0.0",
    "com.unity.modules.animation": "1.0.0",
    ...
  }
}
```

If you don't want to use Git, you can download this library as an archive and install it as a [local package](https://docs.unity3d.com/Manual/upm-ui-local.html).

### Asset Store or .unitypackage
The other way is to import a package from the [Asset Store](https://assetstore.unity.com/packages/tools/utilities/procedural-toolkit-16508) 
or the [Releases page](https://github.com/Syomus/ProceduralToolkit/releases).
If you have any previous versions of the toolkit installed, it is highly recommended to delete them before importing a newer version.

### Getting started
After installation you can import `ProceduralToolkit` namespace and start building your own PCG systems:
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

If you are using [Assembly Definitions](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html), 
you will also need to add `ProceduralToolkit` as a reference.

## I have a problem with Procedural Toolkit
First, please search the [open issues](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aopen)
and [closed issues](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aclosed)
to see if your issue hasn't already been reported. If it does exist, add a
:thumbsup: to the issue to indicate this is also an issue for you, and add a
comment if there is extra information you can contribute.

If you can't find a matching issue, open a [new issue](https://github.com/Syomus/ProceduralToolkit/issues/new/choose),
choose the right template and provide us with enough information to investigate further. 
Alternatively, you can send a message to the [support email](mailto:proceduraltoolkit@syomus.com).

## Contributing
See [CONTRIBUTING](/.github/CONTRIBUTING.md) for a full guide on how you can help.

If you're looking for something to work on, check out the [help wanted](https://github.com/Syomus/ProceduralToolkit/issues?q=is%3Aissue+is%3Aopen+label%3A"help+wanted") label.

If you just want to show your appreciation, you can send a donation through [PayPal](https://www.paypal.me/ProceduralToolkit/5usd) or [Ko-fi](https://ko-fi.com/ProceduralToolkit).

## Toolkit structure
### Runtime/
* [ArrayE](/Runtime/ArrayE.cs): Various Array and List extensions, such as looped getters/setters and flood fills.
* [CellularAutomaton](/Runtime/CellularAutomaton.cs): Generic cellular automaton for two-state rulesets. 
Common rulesets can be found in CellularAutomaton.Ruleset.
* [ClipperUtility](/Runtime/ClipperUtility.cs): Utility class for conversion of Clipper data from and to Unity format.
* [ColorE](/Runtime/ColorE.cs): Color extensions, HTML colors, Gradient constructors.
* [ColorHSV](/Runtime/ColorHSV.cs): Serializable representation of color in HSV model.
* [CompoundMeshDraft](/Runtime/CompoundMeshDraft.cs): Helper class for mesh generation supporting large meshes and submeshes.
* [DebugE](/Runtime/DebugE.cs): Collection of drawing methods similar to Debug.DrawLine.
* [Directions](/Runtime/Directions.cs): Enum with direction flags along three axes.
* [Draw](/Runtime/Draw.cs): Collection of generic vector drawing algorithms.
* [DrawRaster](/Runtime/DrawRaster.cs): Collection of generic raster drawing algorithms.
* [GizmosE](/Runtime/GizmosE.cs): Collection of drawing methods similar to Gizmos.
* [GLE](/Runtime/GLE.cs): Collection of GL drawing methods similar to Gizmos.
* [MeshDraft](/Runtime/MeshDraft.cs): Helper class for mesh generation.
* [MeshDraftPrimitives](/Runtime/MeshDraftPrimitives.cs): Constructors for MeshDraft primitives.
* [MeshE](/Runtime/MeshE.cs): Mesh extensions.
* [PathClipper](/Runtime/PathClipper.cs) and [PathOffsetter](/Runtime/PathOffsetter.cs): [Clipper](http://www.angusj.com/delphi/clipper.php) library wrappers.
* [PTUtils](/Runtime/PTUtils.cs): Various useful methods and constants.
* [RandomE](/Runtime/RandomE.cs): Class for generating random data. Contains extensions for arrays and other collections.
* [RendererProperties](/Runtime/RendererProperties.cs): 
Serializable Renderer properties, use Renderer.ApplyProperties extension to apply them to a target renderer.
* [Tessellator](/Runtime/Tessellator.cs): [LibTessDotNet](https://github.com/speps/LibTessDotNet) library wrapper.
* [TextureE](/Runtime/TextureE.cs): Texture extensions.
* [VectorE](/Runtime/VectorE.cs): Vector extensions.

### Runtime/Buildings/
* [BuildingGenerator](/Runtime/Buildings/BuildingGenerator.cs): 
The main generation class. Generates buildings based on input configuration and strategies, reusable. See examples for details.
* [ILayout](/Runtime/Buildings/Interfaces/ILayout.cs), [ILayoutElement](/Runtime/Buildings/Interfaces/ILayoutElement.cs), 
[IConstructible](/Runtime/Buildings/Interfaces/IConstructible.cs), [Layout](/Runtime/Buildings/Layout.cs), 
[HorizontalLayout](/Runtime/Buildings/HorizontalLayout.cs), [VerticalLayout](/Runtime/Buildings/VerticalLayout.cs): 
Interfaces and classes used in facade layout generation.
* [IFacadePlanner](/Runtime/Buildings/Interfaces/IFacadePlanner.cs), [IFacadeConstructor](/Runtime/Buildings/Interfaces/IFacadeConstructor.cs), 
[IRoofPlanner](/Runtime/Buildings/Interfaces/IRoofPlanner.cs), [IRoofConstructor](/Runtime/Buildings/Interfaces/IRoofConstructor.cs): 
Interfaces for strategies controlling the details of the building generation process.
* [FacadePlanner](/Runtime/Buildings/FacadePlanner.cs), [FacadeConstructor](/Runtime/Buildings/FacadeConstructor.cs), 
[RoofPlanner](/Runtime/Buildings/RoofPlanner.cs), [RoofConstructor](/Runtime/Buildings/RoofConstructor.cs): 
Serializable wrappers for strategy interfaces.
* [ProceduralRoofs](/Runtime/Buildings/ProceduralRoofs.cs): A collection of roof constructors for the building generator.

### Runtime/Geometry/
See [this wiki page](https://github.com/Syomus/ProceduralToolkit/wiki/Geometry-algorithms) for a matrix of available algorithms.
* [Circle2](/Runtime/Geometry/Circle2.cs), [Circle3](/Runtime/Geometry/Circle3.cs), [Sphere](/Runtime/Geometry/Sphere.cs), 
[Line2](/Runtime/Geometry/Line2.cs), [Line3](/Runtime/Geometry/Line3.cs), 
[Segment2](/Runtime/Geometry/Segment2.cs), [Segment3](/Runtime/Geometry/Segment3.cs): 
Representations of geometric primitives.
* [Geometry](/Runtime/Geometry/Geometry.cs): Utility class for computational geometry algorithms, contains various point samplers and helper methods.
* [Closest2D](/Runtime/Geometry/Closest2D.cs), [Closest3D](/Runtime/Geometry/Closest3D.cs): 
Collection of closest point(s) algorithms.
* [Distance2D](/Runtime/Geometry/Distance2D.cs), [Distance3D](/Runtime/Geometry/Distance3D.cs): 
Collection of distance calculation algorithms.
* [Intersect2D](/Runtime/Geometry/Intersect2D.cs), [Intersect3D](/Runtime/Geometry/Intersect3D.cs): 
Collection of intersection algorithms.
* [IntersectionType](/Runtime/Geometry/IntersectionType.cs): Enum used in intersection algorithms.
* [Intersections](/Runtime/Geometry/Intersections) folder: Structures containing information about intersections.

### Runtime/Geometry/StraightSkeleton/
Classes used in [straight skeleton](https://en.wikipedia.org/wiki/Straight_skeleton) generation.
* [StraightSkeletonGenerator](/Runtime/Geometry/StraightSkeleton/StraightSkeletonGenerator.cs): 
A straight skeleton generator, computes a straight skeleton for the input polygon, reusable. 
The generation algorithm is loosely based on the work of Tom Kelly (2014) 
[Unwritten procedural modeling with the straight skeleton](http://www.twak.co.uk/2014/02/unwritten-procedural-modeling-with.html).
* [StraightSkeleton](/Runtime/Geometry/StraightSkeleton/StraightSkeleton.cs): A straight skeleton representation.
* [Plan](/Runtime/Geometry/StraightSkeleton/Plan.cs): Representation of the active plan during generation process.

### Runtime/ClipperLib/
The [Clipper](http://www.angusj.com/delphi/clipper.php) library. 
Use [PathClipper](/Runtime/PathClipper.cs) and [PathOffsetter](/Runtime/PathOffsetter.cs) for seamless interoperability with Unity.

### Runtime/LibTessDotNet/
The [LibTessDotNet](https://github.com/speps/LibTessDotNet) library. 
The recommended use is through the wrapper class [Tessellator](/Runtime/Tessellator.cs).

### Runtime/FastNoiseLib/
The [FastNoise](https://github.com/Auburns/FastNoise_CSharp) library.

### Shaders/
Depending on the installation type, the shader library can be used like this:
```HLSL
#include "Packages/com.syomus.proceduraltoolkit/Shaders/SDF.cginc"
```
Or like this:
```HLSL
#include "Assets/ProceduralToolkit/Shaders/SDF.cginc"
```
* [Common.cginc](/Shaders/Common.cginc): Collection of shaping and debug functions.
* [Easing.cginc](/Shaders/Easing.cginc): Normalized easing functions.
* [SDF.cginc](/Shaders/SDF.cginc): Collection of signed distance functions.
* [Transitions.cginc](/Shaders/Transitions.cginc): Collection of transition animations.
* [Gradient Skybox](/Shaders/Gradient%20Skybox.shader): Simple gradient skybox.
* [Debug](/Shaders/Debug) folder: Debug shaders for some of the most common mesh channels.
* [VertexColor](/Shaders/VertexColor) folder: Textureless shaders for use with color information from the vertices.

### Editor/
* [ColorHSVDrawer](/Runtime/Editor/ColorHSVDrawer.cs): PropertyDrawer for ColorHSV.
* [MeshFilterExtension](/Runtime/Editor/MeshFilterExtension.cs): Mesh saving utility available at `MeshFilter context menu > Save Mesh`.
* [ProceduralToolkitMenu](/Runtime/Editor/ProceduralToolkitMenu.cs): Submenu with constructors for primitives at `GameObject > Procedural Toolkit`.
* [RulesetDrawer](/Runtime/Editor/RulesetDrawer.cs): PropertyDrawer for CellularAutomaton.Ruleset.

### Tests/
Tests for the library

## Samples

### [Buildings](https://syomus.com/ProceduralToolkit/Buildings)
![](https://syomus.com/ProceduralToolkit/screenshot-buildings.png)

A fully procedural building generator, creates an entire mesh from scratch and paints it's vertices. 
Keep in mind that the generated mesh has no uv map so using it with Standard shader is pointless.
* BuildingGeneratorComponent: A simple minimal example on how you can use BuildingGenerator
* BuildingGeneratorConfigurator: A configurator for BuildingGenerator with UI and editor controls.
* BuildingGeneratorReuse: An example on how you can reuse the same generator to generate multiple buildings.
* ProceduralFacadePlanner: A facade planning strategy, controls the layouts of the facades.
* ProceduralFacadeConstructor: A facade construction strategy, used in conjunction with ProceduralFacadePlanner.
* ProceduralRoofPlanner: A roof planning strategy, generates a roof description based on the input config.
* ProceduralRoofConstructor: A roof construction strategy, used in conjunction with ProceduralRoofPlanner.
* ProceduralFacadeElements: A collection of fully procedural facade panels for the building generator.
* PolygonAsset: A ScriptableObject container for vertices.
* BuildingGeneratorConfiguratorEditor: A custom inspector for BuildingGeneratorConfigurator.

### [Chairs](https://syomus.com/ProceduralToolkit/Chairs)
![](https://syomus.com/ProceduralToolkit/screenshot-chairs.png)

A fully procedural chair generator, creates an entire mesh from scratch and paints it's vertices.
* ChairGenerator: Main generator class. Generates chairs based on input configuration.
* ChairGeneratorConfigurator: Configurator for ChairGenerator with UI and editor controls.
* ChairGeneratorConfiguratorEditor: Custom inspector for ChairGeneratorConfigurator.
* Armrests, Backs, Stretchers: Chair parts constructors.

### [Low Poly Terrain](https://syomus.com/ProceduralToolkit/LowPolyTerrain)
![](https://syomus.com/ProceduralToolkit/screenshot-low-poly-terrain.png)

A simple low poly terrain generator based on fractal noise. 
Doesn't support chunking or anything like that, just an example of how you can use a noise function in a plane generation algorithm.
* LowPolyTerrainGenerator: Main generator class. Generates terrain based on input configuration.
* LowPolyTerrainExample: Configurator for LowPolyTerrainGenerator with UI and editor controls.
* LowPolyTerrainExampleEditor: Custom inspector for LowPolyTerrainExample.

### [Signed Distance Functions](https://syomus.com/ProceduralToolkit/SDF)
![](https://syomus.com/ProceduralToolkit/screenshot-sdf.png)

A collection of shaders showing how you can utilise functions from the shader library.
* StarPolygon: A simple shader that draws a star polygon and shows a difference between "normal" and "cheap" functions.
* Shapes: A demonstration of some of the available shapes.
* DistanceOperations: A square and a circle combined with different functions.
* Easing: All easing functions in one shader showing the ease curve and the easing motion.
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

### [Noise](https://syomus.com/ProceduralToolkit/Noise)
![](https://syomus.com/ProceduralToolkit/screenshot-noise.png)

An example demonstrating various noise types from the [FastNoise](https://github.com/Auburns/FastNoise_CSharp) library.

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

### Common
UI prefabs and the skybox material used in examples.
* SkyBoxGenerator: Skybox generator, assuming that scene uses gradient skybox shader, animates transitions to new parameters every few seconds.
* ButtonControl, SliderControl, TextControl, ToggleControl: UI controls for generators.
* CameraRotator: Orbiting camera controller.
* ConfiguratorBase: Base class for configurators.

## License
[MIT](LICENSE.md)
