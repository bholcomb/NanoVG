//
// Copyright (c) 2009-2013 Mikko Mononen memon@inside.org
//
// This software is provided 'as-is', without any express or implied
// warranty.  In no event will the authors be held liable for any damages
// arising from the use of this software.
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software. If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 3. This notice may not be removed or altered from any source distribution.
//
#pragma once

#ifdef NVGL3_EXPORTS
#define NVGL3_DLL __declspec(dllexport)
#else
#define NVGL3_DLL __declspec(dllimport)
#endif

typedef struct NVGcontext NVGcontext;

struct NVGLUframebuffer {
	NVGcontext* ctx;
	unsigned int fbo;
	unsigned int rbo;
	unsigned int texture;
	int image;
};
typedef struct NVGLUframebuffer NVGLUframebuffer;

// Helper function to create GL frame buffer to render to.
NVGL3_DLL void nvgluBindFramebuffer(NVGLUframebuffer* fb);
NVGL3_DLL NVGLUframebuffer* nvgluCreateFramebuffer(NVGcontext* ctx, int w, int h, int imageFlags);
NVGL3_DLL void nvgluDeleteFramebuffer(NVGLUframebuffer* fb);
