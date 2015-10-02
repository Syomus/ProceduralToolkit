# Procedural Toolkit v0.1.6

Procedural Toolkit is a collection of instruments for creating procedural generation systems. It is free, open source and does not require a Unity Pro license.

**Warning: Be aware that this is a programming toolkit, editor support is limited.**

## Classes
* RandomE: Random extensions for arrays, value generators.
* MeshE: Mesh extensions and constructors for primitives.
* MeshDraft: helper class for procedural mesh generation.
* TextureE: Texture extensions and constructors.
* ColorE: Color extensions, HTML colors.
* PTUtils: useful utility methods.
* Datasets: various data.

## Editor classes
* ProceduralToolkitMenu: constructors for primitives and about window.
* MeshFilterExtension: Mesh saver.

## Examples
<table>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Khrushchyovka">Khrushchyovka</a>: a procedural building generator</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-khrushchyovka-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/ChairGenerator">ChairGenerator</a>: a procedural chair generator</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-chair-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Boids">Boids</a>: a single-mesh particle system with birds-like behaviour</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-boids-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Breakout">Breakout</a>: a Breakout clone with procedurally generated levels</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-breakout-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/TerrainMesh">TerrainMesh</a>: a simple terrain based on Perlin noise and coloured according to height</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-terrain-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Mazes">Mazes</a>: a maze generators</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-mazes-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Primitives">Primitives</a>: a demonstration of primitives</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-primitives-300.jpg"></td>
  </tr>
</table>

## Links
* [GitHub](https://github.com/Syomus/ProceduralToolkit)
* [Asset Store](https://www.assetstore.unity3d.com/#!/content/16508)

## Version history
##### 2015.10.02 - 0.1.6:
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

##### 2015.05.14 - 0.1.5:
* Unity 5 support
* Added mesh saver
* Added new example: Breakout
* Added Standard Vertex Color shader

##### 2015.02.08 - 0.1.4:
* Added new examples: Primitives and Mazes
* Added Texture extensions
* Added Bresenham and Wu line drawing algorithms

##### 2014.11.15 - 0.1.3:
* Added new example: Khrushchyovka
* Added knapsack problem solver
* Added new Color extensions
* Added random choice for Dictionary
* Added new mesh drafts
* Added specular vertex color shader
* Improved examples

##### 2014.11.10 - 0.1.2:
* Added new example: chair generator
* Small additions and fixes

##### 2014.10.26 - 0.1.1:
* Added new primitives: cylinder and sphere
* Added mesh extensions: move, rotate, scale, paint, flip faces
* Added RandomE.Range methods
* Added PTUtils methods for points on circle and sphere
* Improved documentation

##### 2014.10.13 - 0.1:
* First release
