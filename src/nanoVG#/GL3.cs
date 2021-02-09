using System;
using System.Runtime.InteropServices;

namespace NanoVG
{
   // Create flags
   [Flags]
   public enum CreateFlags : int
   {
      // Flag indicating if geometry based anti-aliasing is used (may not be needed when using MSAA).
      ANTIALIAS = 1 << 0,
      // Flag indicating if strokes should be drawn using stencil buffer. The rendering will be a little
      // slower, but path overlaps (i.e. self-intersecting or sharp turns) will be drawn just once.
      STENCIL_STROKES = 1 << 1,
      // Flag indicating that additional debug checks are done.
      DEBUG = 1 << 2,
   };

   public static unsafe partial class NVG
   {
      [DllImport("nanoVG-GL3.dll", CallingConvention = CConv, EntryPoint = "nvgCreateGL3")]
      public static extern NanoVGContext CreateGL3(CreateFlags flags);

   }
}
