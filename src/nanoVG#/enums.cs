using System;
using System.Runtime.InteropServices;

using NVGColor = System.Numerics.Vector4;
using NVGVertex = System.Numerics.Vector4;

namespace NanoVG
{
   public enum Winding : int
   {
      CCW = 1,            // Winding for solid shapes
      CW = 2,             // Winding for holes
   }

   public enum Solidity : int
   {
      SOLID = 1,          // CCW
      HOLE = 2,           // CW
   }

   public enum LineCap : int
   {
      BUTT,
      ROUND,
      SQUARE,
      BEVEL,
      MITER,
   };

   [Flags]
   public enum Align : int
   {
      // Horizontal align
      LEFT = 1 << 0,	// Default, align text horizontally to left.
      CENTER = 1 << 1,	// Align text horizontally to center.
      RIGHT = 1 << 2,	// Align text horizontally to right.
      // Vertical align
      TOP = 1 << 3,	// Align text vertically to top.
      MIDDLE = 1 << 4,	// Align text vertically to middle.
      BOTTOM = 1 << 5,	// Align text vertically to bottom.
      BASELINE = 1 << 6, // Default, align text vertically to baseline.
   };

   public enum BlendFactor : int
   {
      ZERO = 1 << 0,
      ONE = 1 << 1,
      SRC_COLOR = 1 << 2,
      ONE_MINUS_SRC_COLOR = 1 << 3,
      DST_COLOR = 1 << 4,
      ONE_MINUS_DST_COLOR = 1 << 5,
      SRC_ALPHA = 1 << 6,
      ONE_MINUS_SRC_ALPHA = 1 << 7,
      DST_ALPHA = 1 << 8,
      ONE_MINUS_DST_ALPHA = 1 << 9,
      SRC_ALPHA_SATURATE = 1 << 10,
   };

   public enum CompositeOperation : int
   {
      SOURCE_OVER,
      SOURCE_IN,
      SOURCE_OUT,
      ATOP,
      DESTINATION_OVER,
      DESTINATION_IN,
      DESTINATION_OUT,
      DESTINATION_ATOP,
      LIGHTER,
      COPY,
      XOR,
   };


   [Flags]
   public enum ImageFlags : int
   {
      GENERATE_MIPMAPS = 1 << 0,     // Generate mipmaps during creation of the image.
      REPEATX = 1 << 1,     // Repeat image in X direction.
      REPEATY = 1 << 2,     // Repeat image in Y direction.
      FLIPY = 1 << 3,       // Flips (inverses) image in Y direction when rendered.
      PREMULTIPLIED = 1 << 4,       // Image data has premultiplied alpha.
      NEAREST = 1 << 5,     // Image interpolation is Nearest instead Linear
      NODELETE = 1 << 16,	// Do not delete GL texture handle.
   }
}
