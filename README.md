# Procedural Toolkit v0.1.8

Procedural Toolkit is a collection of instruments for development of procedural generation systems in Unity game engine. 
It is free, open source and does not require a Unity Pro license.

**Warning: This is a programming toolkit, editor support is limited.**

[GitHub](https://github.com/Syomus/ProceduralToolkit) |
[Asset Store](https://www.assetstore.unity3d.com/#!/content/16508) |
[Issues](https://github.com/Syomus/ProceduralToolkit/issues) |
[Support email](mailto:proceduraltoolkit@syomus.com)

## Installation instructions
Requires **Unity 5.3** or later. Tested on PC and WebGL. UnityScript interoperability is not supported.

You can install Procedural Toolkit from any of the following sources:

1. Import from the [Asset Store](https://www.assetstore.unity3d.com/#!/content/16508)
2. Download from [Releases page](https://github.com/Syomus/ProceduralToolkit/releases). Same packages as in the Asset Store.
3. Clone/download repository from [GitHub](https://github.com/Syomus/ProceduralToolkit). In this case you will have to create a subfolder for toolkit (e.g. `\Assets\ProceduralToolkit`).

Folder containing example classes and scenes (`ProceduralToolkit\Examples`) can be safely removed. The same is true for `ProceduralToolkit\Shaders`, if you are not using examples. Removing `ProceduralToolkit\Editor` will leave you with code-only toolkit.

After installation you can import ProceduralToolkit namespace and start building your own PCG systems:
```C#
using UnityEngine;
using ProceduralToolkit;

public class Example : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(string.Format("<color=#{0}>{0}</color>", RandomE.colorHSV.ToHtmlStringRGB()));
    }
}
```

## Classes
* ArrayE: Array extensions.
* ColorE: Color extensions, HTML colors.
* ColorHSV: Representation of color in HSV model.
* Datasets: Constants with uppercase and lowercase letters, digits.
* DebugE: Drawing methods similar to Debug.
* Directions: Enum with directions along three axes.
* Draw: Collection of drawing method-independent generic drawing algorithms.
* GizmosE: Drawing methods similar to Gizmos.
* GLE: Drawing wrappers over GL.Vertex which follow Gizmos convention.
* MeshDraft: Helper class for mesh generation.
* MeshDraftPrimitives: Constructors for MeshDraft primitives.
* MeshE: Mesh extensions.
* MeshPrimitives: Constructors for Mesh primitives.
* PTUtils: Various utility methods.
* RandomE: Value generators and extensions for collections.
* TextureE: Texture extensions and constructors.
* Vector2Int: Vector2 analogue with integer components.
* VectorE: Vector extensions.

## Editor classes
* ProceduralToolkitMenu: `GameObject>Procedural Toolkit` constructors for primitives and about window.
* MeshFilterExtension: Mesh saving utility.

## Examples
Resources folder contains prefabs used for UI. UI folder contains scripts for ui controls, including camera rotator.

### <a href="http://syomus.com/ProceduralToolkit/BuildingGenerator">BuildingGenerator</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-building-generator.png">

Procedural building generator.
* BuildingGenerator: Main generator class. Generates buildings based on input configuration.
* BuildingGeneratorConfigurator: Configurator for generator with UI and editor controls.
* BuildingGeneratorUtils: Helper class for generator.
* IFacadeLayout, IFacadePanel, FacadeLayout, HorizontalLayout, VerticalLayout: Interfaces and classes used in facade layout generation.
* ProceduralFacadePanel: Fully procedural facade panels for building generator.
* BuildingGeneratorConfiguratorEditor: Custom inspector for BuildingGeneratorConfigurator.

### <a href="http://syomus.com/ProceduralToolkit/ChairGenerator">ChairGenerator</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-chair-generator.png">

Procedural chair generator.
* ChairGenerator: Main generator class. Generates chairs based on input configuration.
* ChairGeneratorConfigurator: Configurator for generator with UI and editor controls.
* Armrests, Backs, Stretchers: Chair parts constructors.
* ChairGeneratorConfiguratorEditor: Custom inspector for ChairGeneratorConfigurator.

### <a href="http://syomus.com/ProceduralToolkit/Boids">Boids</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-boids.png">

Single-mesh particle system with birds-like behaviour.
* BoidController: Generates animated mesh based on input configuration.
* BoidControllerConfigurator: Configurator for BoidController with UI controls.

### <a href="http://syomus.com/ProceduralToolkit/LowPolyTerrainGenerator">LowPolyTerrainGenerator</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-low-poly-terrain-generator.png">

Simple Perlin noise based low poly terrain generator.
* LowPolyTerrainGenerator: Main generator class. Generates terrain based on input configuration.
* LowPolyTerrainGeneratorConfigurator: Configurator for generator with UI and editor controls.
* LowPolyTerrainGeneratorConfiguratorEditor: Custom inspector for LowPolyTerrainGeneratorConfigurator.

### <a href="http://syomus.com/ProceduralToolkit/CellularAutomaton">CellularAutomaton</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-cellular-automata.png">

Generic cellular automaton for two-state rulesets.
* CellularAutomaton: Main generator class. Generates cellular automata based on input configuration.
* CellularAutomatonConfigurator: Configurator for generator with UI controls.
* Ruleset: Cellular automaton ruleset representation and static constructors.
* CellState: Enum for automaton cell states.

### <a href="http://syomus.com/ProceduralToolkit/Mazes">Mazes</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-mazes.png">

Generic maze generator.
* MazeGenerator: Main generator class. Generates mazes on input configuration.
* MazeGeneratorConfigurator: Configurator for generator with UI controls.
* Maze: Maze graph representation.
* Cell: Maze graph cell.
* Edge: Maze graph edge.

### <a href="http://syomus.com/ProceduralToolkit/Breakout">Breakout</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-breakout.png">

Breakout clone with procedurally generated levels.
* Breakout: Game engine and level generator.
* BreakoutConfigurator: Configurator for generator with UI controls.
* Brick: Disables game object on collision.

### <a href="http://syomus.com/ProceduralToolkit/Primitives">Primitives</a>
<img src="http://syomus.com/ProceduralToolkit/screenshot-primitives.png">

Demonstration of primitives
* Cylinder, Dodecahedron, FlatSphere, Hexahedron...: Mesh generators with exposed parameters in inspector.

### Common classes
* SkyBoxGenerator: Skybox generator, assuming that scene uses gradient skybox shader, animates transitions to new parameters every few seconds.
* ButtonControl, SliderControl, TextControl, ToggleControl: UI controls for generators.
* CameraRotator: Orbiting camera controller.
* ConfiguratorBase: Base class for configurators.

## Shaders
* Unlit, Diffuse, Specular and Standard Vertex Color: Textureless shaders which use color information from vertex attributes.
* Gradient Skybox: Simple gradient skybox.

## License
```
The MIT License (MIT)

Copyright (c) Daniil Basmanov

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Version history
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
