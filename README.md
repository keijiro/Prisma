Prisma
======

![screenshot](http://i.imgur.com/MIOJhYpm.png)
![screenshot](http://i.imgur.com/G0WeuzIm.png)

**Prisma** is live visual software that was developed for presenting visuals
in a live performance by [Merzbow] & XXX Residents at DMM VR Theater. It uses
the big transparent projection screen installed in front of the stage of the
theater to make [Pepper's ghost] illusion.

The basic design of the project is much similar to my previous Pepper's ghost
project [Mirage], but it has been heavily optimized in several ways.

One of the biggest changes that made a large performance gain is introducing
*Shadow Slicer*. This made it possible to render shadows on the background
screen without regenerating shadow maps multiple times (it was the most
significant performance bottleneck in the previous project). For further
details of Shadow Slicer, see the [source code of Shadow Slicer].

[Merzbow]: https://en.wikipedia.org/wiki/Merzbow
[Pepper's ghost]: https://en.wikipedia.org/wiki/Pepper%27s_ghost
[Mirage]: https://github.com/keijiro/Mirage
[source code of Shadow Slicer]: Assets/Prisma/Scripts/ShadowSlicer.cs

System Requirements
-------------------

- Unity 5.6 or later

*Prisma* uses the special graphics features listed below. These features
should be supported on the target platform.

- [Compute shader]
- [Random Write Target]
- [GPU Instancing]

[Compute Shader]: https://docs.unity3d.com/Manual/ComputeShaders.html
[Random Write Target]: https://docs.unity3d.com/ScriptReference/Graphics.SetRandomWriteTarget.html
[GPU Instancing]: https://docs.unity3d.com/Manual/GPUInstancing.html

License
-------

[MIT](LICENSE.txt)
