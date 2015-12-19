# Procedural Toolkit v0.1.7

Procedural Toolkit is a collection of instruments for creating procedural generation systems in Unity game engine. It is free, open source and does not require a Unity Pro license.

**Warning: Be aware that this is a programming toolkit, editor support is limited.**

## Installation instructions
Download toolkit from repository or Asset Store and place it somewhere in your Unity project (e.g. "Assets\ProceduralToolkit"). After that you can import ProceduralToolkit namespace in your script and start building your own PCG systems:
```C#
using UnityEngine;
using ProceduralToolkit;

public class Example : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(RandomE.fullName);
    }
}
```

Folder containing example classes and scenes (".\Examples") can be safely removed. The same is true for ".\Shaders", if you are not using examples. Removing ".\Editor" will leave you with code-only toolkit.

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
    <td><a href="http://syomus.com/ProceduralToolkit/CellularAutomaton">CellularAutomaton</a>: a generic cellular automaton</td>
    <td><img src=""></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Mazes">Mazes</a>: a maze generators</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-mazes-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/TerrainMesh">TerrainMesh</a>: a simple terrain based on Perlin noise and coloured according to height</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-terrain-300.jpg"></td>
  </tr>
  <tr>
    <td><a href="http://syomus.com/ProceduralToolkit/Primitives">Primitives</a>: a demonstration of primitives</td>
    <td><img src="http://syomus.com/ProceduralToolkit/screenshot-primitives-300.jpg"></td>
  </tr>
</table>

## Links
* [GitHub](https://github.com/Syomus/ProceduralToolkit)
* [Asset Store](https://www.assetstore.unity3d.com/#!/content/16508)
 
## Support
Support is provided via GitHub [issues](https://github.com/Syomus/ProceduralToolkit/issues) and end [email](mailto:proceduraltoolkit@syomus.com).

Keep in mind, that toolkit is developed and tested in second to last minor release of Unity. As of this writing, the latest version is 5.3, so you can expect toolkit to be working in 5.2. Actually, it should work fine even on 4.6, but you are on your own. Also, UnityScript interoperability is not tested.

## License
The MIT License (MIT)

Copyright (c) 2015 Daniil Basmanov

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

## Version history
##### 0.1.7 (2015.12.19)
* Features:
  * Added ColorHSV class
  * Added Vector2Int class
  * Added ArrayE class
  * Added CircularList class
  * Added CellularAutomaton example
* Improvements:
  * Added PTUtils.DrawFilledCircle
  * Refactored drawing methods, eliminated overdraw
* Changes:
  * Removed ColorE.HSVToRGB, use ColorHSV instead
  * New CameraRotator behaviour, now requires Image to operate
  * PTUtils.WuLine is now DrawAALine, BresenhamLine is DrawLine
* Fixes:
  * Fixed a few ui and generator bugs in examples

##### 0.1.6 (2015.10.02)
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

##### 0.1.5 (2015.05.14)
* Unity 5 support
* Added mesh saver
* Added new example: Breakout
* Added Standard Vertex Color shader

##### 0.1.4 (2015.02.08)
* Added new examples: Primitives and Mazes
* Added Texture extensions
* Added Bresenham and Wu line drawing algorithms

##### 0.1.3 (2014.11.15)
* Added new example: Khrushchyovka
* Added knapsack problem solver
* Added new Color extensions
* Added random choice for Dictionary
* Added new mesh drafts
* Added specular vertex color shader
* Improved examples

##### 0.1.2 (2014.11.10)
* Added new example: chair generator
* Small additions and fixes

##### 0.1.1 (2014.10.26)
* Added new primitives: cylinder and sphere
* Added mesh extensions: move, rotate, scale, paint, flip faces
* Added RandomE.Range methods
* Added PTUtils methods for points on circle and sphere
* Improved documentation

##### 0.1 (2014.10.13)
* First release