solution "NanoVG"
   location("../")
   configurations { "Debug", "Release" }
   platforms{"x64"}
   startproject "testNanoVG"
 
  configuration { "Debug" }
    defines { "DEBUG", "TRACE"}
    symbols "On"
    optimize "Off"
    targetsuffix ("d")
 
  configuration { "Release" }
    optimize "Speed"
	
   
project "nanoVG"
	kind "SharedLib"
	language "C++"
	location "nanoVG"
	targetdir("../lib")
   includedirs {"../include" }
   defines{"NVG_EXPORTS", "_CRT_SECURE_NO_WARNINGS"}
	files{"../include/nanoVG/*.h", "../src/nanoVG/*.c", "../src/nanoVG/*.h"}
	vpaths { ["Headers"] = "../include/nanoVG/*.h", ["Source"] = {"../src/nanoVG/*.c", "../src/nanoVG/*.h" }}
   systemversion("10.0")
   
project "testNanoVG"
	kind "ConsoleApp"
	language "C++"
	location "testNanoVG"
	targetdir("../bin")
	files{"../src/testNanoVG/*.cxx"}
   includedirs {"../include", "../3rdParty/include"}
   libdirs {"../lib", "../3rdParty/lib"}
	links {"nanoVG", "glfw3", "OpenGL32"}
	systemversion("10.0")