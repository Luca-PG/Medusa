Medusa
======

Ribbon Trail Engine (C#, XNA)

A clone of my old project on codeplex (https://medusa.codeplex.com/).


! Project Description
{project:description}
The project is still in early development, but it is already good enough to make effects like blade swings and laser beams with a minimum effort.


! Engine todo list:
* Add rendering options to view trails correctly in pre-existing 3d worlds.
* Add a smoothing factor, to be used with fast moving trails. It will add interpolated segments between other segments, in order to always obtain smooth trails.
* Render all trails with the same settings in a single draw call.
* Wrap XNA Color to add interpolation and fading.
* Load xml files exported from the editor via content pipeline.
* Add support for animated textures.
* Add cool curves and paths to craft visual effects with trails.


! Editor todo list:
* Support trail freezing.
* Add more ways to view the trail, like dragging it with the mouse.
* Add a better type editor for Color.


A screenshot of the test project
[image:trailtest thumb.png]
full size : [url:http://i55.tinypic.com/2q0twnt.png]

A screenshot of the editor
[image:traileditor thumb.png]
full size : [url:http://i52.tinypic.com/2qx3h29.png]
