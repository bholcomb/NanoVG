*This is a work in progress and will require some hand tweaking*

# NanoVG
This is my fork of NanoVG from Mikko Mononen (https://github.com/memononen/nanovg). I wanted a DLL build so that I could load use NanoVG with C#.  I also wanted to support several different renderers, so I've pulled in different renderers from: 
* default OpenGL renderers (from NanoVG repository)
* BGFX (from BGFX  https://github.com/bkaradzic/bgfx)
* Software Renderer (from https://github.com/syoyo/nanovg-nanort)
 
 I've also included a set of C# bindings (originally from https://github.com/sbarisic/nanovg_dotnet) that I'm modifying to bring up to date with latest version of NanoVG and organzied the code a bit.

##How to build 
cd build
premake5.exe 2019
*manually add the C# project file*
open solution and build
