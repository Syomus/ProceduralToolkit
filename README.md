# Procedural Toolkit 0.2.0

Procedural Toolkit is a procedural generation library for the Unity game engine.

**Warning: This is a programming toolkit, editor support is limited.**

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donate_SM.gif)](https://paypal.me/ProceduralToolkit) | 
[GitHub](https://github.com/Syomus/ProceduralToolkit) |
[Asset Store](https://www.assetstore.unity3d.com/#!/content/16508) |
[Issues](https://github.com/Syomus/ProceduralToolkit/issues) |
[Support email](mailto:proceduraltoolkit@syomus.com)

## Installation instructions
Requires **Unity 2017.2** or later. Tested on Windows, WebGL, Android and iOS. UnityScript is not supported.

You can install Procedural Toolkit from any of the following sources:

1. Import from the [Asset Store](https://www.assetstore.unity3d.com/#!/content/16508)
2. Download from the [Releases page](https://github.com/Syomus/ProceduralToolkit/releases). Same packages as in the Asset Store.
3. Clone/download the repository from [GitHub](https://github.com/Syomus/ProceduralToolkit). In this case you will have to create a subfolder for the toolkit (preferably `\Assets\ProceduralToolkit`).

If you have any previous versions of the toolkit installed it is highly recommended to delete them before importing a newer version.

Renaming or moving toolkit's folder from the default path (`\Assets\ProceduralToolkit`) will break .cginc includes in the example shaders, otherwise it is safe to move the toolkit however you want.

The folder containing example classes and scenes (`ProceduralToolkit\Examples`) can be safely removed. The same is true for `ProceduralToolkit\Shaders`, if you are not using examples. Removing `ProceduralToolkit\Scripts\Editor` will leave you with a code-only toolkit, but you will lose drawers for some of the classes.

## Getting started

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

## Scripts
* ArrayE: Various Array and List extensions, such as looped getters/setters and flood fills.
* CellularAutomaton: Generic cellular automaton for two-state rulesets. Common rulesets can be found in CellularAutomaton.Ruleset.
* ColorE: Color extensions, HTML colors, Gradient constructors.
* ColorHSV: Serializable representation of color in HSV model.
* CompoundMeshDraft: Helper class for mesh generation supporting large meshes and submeshes.
* DebugE: Collection of drawing methods similar to Debug.DrawLine.
* Directions: Enum with direction flags along three axes.
* Draw: Collection of generic vector drawing algorithms.
* DrawRaster: Collection of generic raster drawing algorithms.
* Geometry: Collection of basic computational geometry algorithms (distance, intersection, closest point, etc.)
* GizmosE: Collection of drawing methods similar to Gizmos.
* GLE: Collection of GL drawing methods similar to Gizmos.
* MeshDraft: Helper class for mesh generation.
* MeshDraftPrimitives: Constructors for MeshDraft primitives.
* MeshE: Mesh extensions.
* PTUtils: Various useful methods and constants.
* RandomE: Class for generating random data. Contains extensions for arrays and other collections.
* TextureE: Texture extensions.
* VectorE: Vector extensions.

## Scripts\Editor
* ColorHSVDrawer: PropertyDrawer for ColorHSV.
* MeshFilterExtension: Mesh saving utility available at `MeshFilter context menu > Save Mesh`.
* ProceduralToolkitMenu: Submenu with constructors for primitives at `GameObject > Procedural Toolkit`.
* RulesetDrawer: PropertyDrawer for CellularAutomaton.Ruleset.

## Shaders
* Common.cginc: Collection of shaping and debug functions.
* Easing.cginc: Normalized easing functions.
* SDF.cginc: Collection of signed distance functions.
* Transitions.cginc: Collection of transition animations.
* Gradient Skybox: Simple gradient skybox.
* `Debug` folder: Debug shaders for some of the most common mesh channels.
* `VertexColor` folder: Textureless shaders for use with color information from the vertices.

## Examples
`Examples\Resources` contains UI prefabs and a skybox material used in examples. `Examples\Common` contains scripts for ui controls, a camera rotator and a skybox generator.

### <a href="http://syomus.com/ProceduralToolkit/Buildings">Buildings</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-buildings.png">

A fully procedural building generator, creates entire mesh from scratch and paints it's vertices. Keep in mind that there is no uv map in the generated mesh so using using it with Standard shader is pointless. If you are interested, the building design is called [Khrushchyovka](https://en.wikipedia.org/wiki/Khrushchyovka).
* BuildingGenerator: Main generator class. Generates buildings based on input configuration.
* BuildingGeneratorBase: Base class for the generator.
* BuildingGeneratorConfigurator: Configurator for BuildingGenerator with UI and editor controls.
* BuildingGeneratorConfiguratorEditor: Custom inspector for BuildingGeneratorConfigurator.
* IFacadeLayout, IFacadePanel, FacadeLayout, HorizontalLayout, VerticalLayout: Interfaces and classes used in facade layout generation.
* ProceduralFacadePanel: Fully procedural facade panels for building generator.
* RoofGenerator: A simple roof generator, generates a roof MeshDraft from a foundation polygon and a config.

### <a href="http://syomus.com/ProceduralToolkit/Chairs">Chairs</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-chairs.png">

A fully procedural chair generator, creates entire mesh from scratch and paints it's vertices.
* ChairGenerator: Main generator class. Generates chairs based on input configuration.
* ChairGeneratorConfigurator: Configurator for ChairGenerator with UI and editor controls.
* ChairGeneratorConfiguratorEditor: Custom inspector for ChairGeneratorConfigurator.
* Armrests, Backs, Stretchers: Chair parts constructors.

### <a href="http://syomus.com/ProceduralToolkit/LowPolyTerrain">Low Poly Terrain</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-low-poly-terrain.png">

A simple Perlin noise based low poly terrain generator. Doesn't support chunking or anything like that, just an example of how you can use a noise function in a plane generation algorithm.
* LowPolyTerrainGenerator: Main generator class. Generates terrain based on input configuration.
* LowPolyTerrainGeneratorConfigurator: Configurator for LowPolyTerrainGenerator with UI and editor controls.
* LowPolyTerrainGeneratorConfiguratorEditor: Custom inspector for LowPolyTerrainGeneratorConfigurator.

### <a href="http://syomus.com/ProceduralToolkit/SDF">Signed Distance Functions</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-sdf.png">

A collection of shaders showing how you can utilise functions from the shader library.
* StarPolygon: A simple shader that draws a star polygon and shows a difference beween "normal" and "cheap" functions.
* Shapes: A demonstration of some of the available shapes.
* DistanceOperations: A square and a circle combined with different functions.
* Easing: All easing functions in one shader showing the ease curve and the easig motion.
* Transitions: An example showing the sequencing and animation techniques.
* Animation: A more complex example on how you can create patterns and animate them in shader.

### <a href="http://syomus.com/ProceduralToolkit/CellularAutomata">Cellular Automata</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-cellular-automata.png">

A demonstration of CellularAutomaton from the main library, draws the automaton simulation on a texture. Note that some of the rulesets need noise value different from the default setting.
* CellularAutomatonConfigurator: Configurator for the automaton with UI controls.

### <a href="http://syomus.com/ProceduralToolkit/Mazes">Mazes</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-mazes.png">

A generic maze generator, draws the maze generation process on a texture.
* MazeGenerator: Main generator class. Generates mazes based on input configuration.
* MazeGeneratorConfigurator: Configurator for MazeGenerator with UI controls.
* Maze: Maze graph representation.

### <a href="http://syomus.com/ProceduralToolkit/Breakout">Breakout</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-breakout.png">

A [Breakout](https://en.wikipedia.org/wiki/Breakout_(video_game)) clone with procedurally generated levels.
* Breakout: Game engine and level generator.
* BreakoutConfigurator: Configurator for the game with UI controls.
* Brick: Disables game object on collision.

### <a href="http://syomus.com/ProceduralToolkit/Characters">Characters</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-characters.gif">

A simple 2D character generator. Sprites made by <a href="http://kenney.nl/">Kenney</a>.
* Character: A container for sprite renderers
* CharacterGenerator: Generates a sprite set and a name for character.
* CharacterGeneratorConfigurator: Configurator for CharacterGenerator.
* NameGenerator: Generates a random name from a large array of names.

### <a href="http://syomus.com/ProceduralToolkit/Primitives">Primitives</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-primitives.png">

A demonstration of some of the available MeshDraft primitives.
* Cylinder, Dodecahedron, FlatSphere, Hexahedron...: Mesh generators that can be configured via the inspector.

### Drawing
An example showing usage of DebugE, GLE and GizmosE.

### Common classes
* SkyBoxGenerator: Skybox generator, assuming that scene uses gradient skybox shader, animates transitions to new parameters every few seconds.
* ButtonControl, SliderControl, TextControl, ToggleControl: UI controls for generators.
* CameraRotator: Orbiting camera controller.
* ConfiguratorBase: Base class for configurators.

## Contributing
If you have found a bug or there is something you want added or changed, you are welcome to [submit an issue](https://github.com/Syomus/ProceduralToolkit/issues) or [pull request](https://github.com/Syomus/ProceduralToolkit/pulls) on the github page. Alternatively, you can send a message to the [support email](mailto:proceduraltoolkit@syomus.com).

## License
[MIT](LICENSE.txt)
