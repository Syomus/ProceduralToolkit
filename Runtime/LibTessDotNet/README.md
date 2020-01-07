LibTessDotNet [![Build Status](https://ci.appveyor.com/api/projects/status/ypuw4wca67vr5k8u?svg=true)](https://ci.appveyor.com/project/speps/libtessdotnet)
=============

### Goal

Provide a robust and fast tessellator (polygons with N vertices in the output) for .NET, also does triangulation.

### Requirements

* .NET framework 2.0 (pure CLR, should work with Mono, Unity or XNA)
* C# 3.0 compiler if you want to compile (I am guilty of using 'var')
    - WinForms for the testbed
    - Solution file for Visual Studio 2010

### Features

* Tessellate arbitrary complex polygons
    - self-intersecting (see "star-intersect" sample)
    - with coincident vertices (see "clipper" sample)
    - advanced winding rules : even/odd, non zero, positive, negative, |winding| >= 2 (see "redbook-winding" sample)
* Custom input
    - Custom vertex attributes (eg. UV coordinates) with merging callback
    - Force orientation of input contour (clockwise/counterclockwise, eg. for GIS systems, see "force-winding" sample)
* Choice of output
    - polygons with N vertices (with N >= 3)
    - connected polygons (didn't quite tried this yet, but should work)
    - boundary only (to have a basic union of two contours)
* Handles polygons computed with [Clipper](http://www.angusj.com/delphi/clipper.php) - an open source freeware polygon clipping library
* Single/Double precision support

### Screenshot

![Redbook winding example](https://raw.github.com/speps/LibTessDotNet/master/TessBed/Misc/screenshot.png)

### Comparison

![Benchmarks](https://raw.github.com/speps/LibTessDotNet/master/TessBed/Misc/benchmarks.png)

### Notes

* When using `ElementType.BoundaryContours`, `Tess.Elements` will contain a list of ranges `[startVertexIndex, vertexCount]`.
  Those ranges are to used with `Tess.Vertices`.

### TODO

* Profile GC allocations
* Any suggestions are welcome ;)

### License

SGI FREE SOFTWARE LICENSE B (Version 2.0, Sept. 18, 2008)
More information in LICENSE.txt.

### Links
* [Reference implementation](http://oss.sgi.com/projects/ogl-sample) - the original SGI reference implementation
* [libtess2](https://github.com/memononen/libtess2) - Mikko Mononen cleaned up the original GLU tesselator
* [Poly2Tri](http://code.google.com/p/poly2tri/) - another triangulation library for .NET (other ports also available)
    - Does not support polygons from Clipper, more specifically vertices with same coordinates (coincident)
* [Clipper](http://www.angusj.com/delphi/clipper.php) - an open source freeware polygon clipping library
