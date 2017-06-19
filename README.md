Prisma
======

![screenshot](http://i.imgur.com/MIOJhYpm.png)
![screenshot](http://i.imgur.com/G0WeuzIm.png)

![gif](http://i.imgur.com/eBM8Uwm.gif)

**Prisma** is live visual software that was developed for presenting visuals
in a live performance by [Merzbow] & XXX Residents at [DMM VR Theater]. It uses
the big transparent projection screen installed in front of the stage of the
theater to make [Pepper's ghost illusion]. See the [photo and video collection]
for how it looks.

Although the basic design of the project is much similar to my previous
Pepper's ghost project [Mirage], it has been improved in several ways.

One of the biggest changes that made a large performance gain is introducing
*Shadow Slicer*. This made it possible to render shadows on the background
screen without regenerating shadow maps multiple times (it was the most
significant performance bottleneck in the previous project). For further
details of Shadow Slicer, see the [source code of Shadow Slicer].

[DMM VR Theater]: https://vr-theater.dmm.com/en/about
[Merzbow]: https://en.wikipedia.org/wiki/Merzbow
[Pepper's ghost illusion]: https://en.wikipedia.org/wiki/Pepper%27s_ghost
[photo and video collection]: http://radiumsoftware.tumblr.com/tagged/vrdgh4
[Mirage]: https://github.com/keijiro/Mirage
[source code of Shadow Slicer]: Assets/Prisma/Scripts/ShadowSlicer.cs

System Requirements
-------------------

- Unity 5.6 or later

*Prisma* uses the special graphics features listed below. These features
should be supported on the target platform.

- [Compute shader]
- [Random write target] (UAV)
- [GPU instancing] with [indirect draw]

[Compute Shader]: https://docs.unity3d.com/Manual/ComputeShaders.html
[Random write target]: https://docs.unity3d.com/ScriptReference/Graphics.SetRandomWriteTarget.html
[GPU instancing]: https://docs.unity3d.com/Manual/GPUInstancing.html
[indirect draw]: https://docs.unity3d.com/ScriptReference/Graphics.DrawMeshInstancedIndirect.html

License
-------

[MIT](LICENSE.txt)
