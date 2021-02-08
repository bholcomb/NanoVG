newoption {
   trigger     = "with-gl3",
   description = "Build the GL3 backend for NanoVG"
}

newoption {
   trigger     = "with-soft",
   description = "Build the software rasterizer backend for NanoVG"
}

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
   
if _OPTIONS['with-gl3'] then
   project "nanoVG-GL3"
      kind "SharedLib"
      language "C++"
      location "nanoVG-GL3"
      targetdir("../lib")
      includedirs {"../include",  "../src/nanoVG/backends/gl3" }
      defines{"NVGL3_EXPORTS", "_CRT_SECURE_NO_WARNINGS"}
      files{"../src/nanoVG/backends/GL3/**.c", "../src/nanoVG/backends/GL3/**.h"}
      links {"nanoVG"}
      vpaths { 
         ["Headers"] = "../include/nanoVG/backends/GL3/*.h", 
         ["Source"] = "../src/nanoVG/backends/GL3/*.c"
      }
      systemversion("10.0")
end    
      
if _OPTIONS['with-soft'] then
   project "nanoVG-Soft"
	kind "SharedLib"
	language "C++"
	location "nanoVG-Soft"
	targetdir("../lib")
   includedirs {"../include", "../src/nanoVG/backends/soft" }
   defines{"NVGSOFT_EXPORTS", "_CRT_SECURE_NO_WARNINGS", "NANORT_USE_CPP11_FEATURE"}
	files{"../src/nanoVG/backends/soft/*.cxx", "../src/nanoVG/backends/soft/*.h"}
   links {"nanoVG"}
	vpaths { 
      ["Headers"] = "../include/nanoVG/backends/soft*.h", 
      ["Source"] = "../src/nanoVG/backends/soft/*.cxx"
   }
   systemversion("10.0")
end
