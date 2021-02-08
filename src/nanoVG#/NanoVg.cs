using System;
using System.Runtime.InteropServices;
using System.Reflection;

using NVGColor = System.Numerics.Vector4;
using NVGVertex = System.Numerics.Vector4;
using System.Text;

namespace NanoVG
{
   public static unsafe partial class NVG
   {
      internal const string FuncPrefix = "nvg";
      internal const string LibName = "nanovg";
      internal const CallingConvention CConv = CallingConvention.Cdecl;


      #region Frame functions
      //
      // Begin drawing a new frame
      // Calls to nanovg drawing API should be wrapped in nvgBeginFrame() & nvgEndFrame()
      // nvgBeginFrame() defines the size of the window to render to in relation currently
      // set viewport (i.e. glViewport on GL backends). Device pixel ration allows to
      // control the rendering on Hi-DPI devices.
      // For example, GLFW returns two dimension for an opened window: window size and
      // frame buffer size. In that case you would set windowWidth/Height to the window size
      // devicePixelRatio to: frameBufferWidth / windowWidth.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BeginFrame))]
      public static extern void BeginFrame(this NanoVGContext Ctx, float windowWidth, float windowHeight, float devicePixelRatio);

      // Cancels drawing the current frame.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CancelFrame))]
      public static extern void CancelFrame(this NanoVGContext Ctx);

      // Ends drawing flushing remaining render state.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(EndFrame))]
      public static extern void EndFrame(this NanoVGContext Ctx);

      #endregion

      #region composite operations
      //
      // Composite operation
      //
      // The composite operations in NanoVG are modeled after HTML Canvas API, and
      // the blend func is based on OpenGL (see corresponding manuals for more info).
      // The colors in the blending state have premultiplied alpha.

      // Sets the composite operation. The op parameter should be one of NVGcompositeOperation.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeOperation))]
      public static extern void GlobalCompositeOperation(this NanoVGContext Ctx, int op);

      // Sets the composite operation with custom pixel arithmetic. The parameters should be one of NVGblendFactor.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeBlendFunc))]
      public static extern void GlobalCompositeBlendFunc(this NanoVGContext Ctx, int sfactor, int dfactor);

      // Sets the composite operation with custom pixel arithmetic for RGB and alpha components separately. The parameters should be one of NVGblendFactor.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalCompositeBlendFuncSeparate))]
      public static extern void GlobalCompositeBlendFuncSeparate(this NanoVGContext Ctx, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
      #endregion

      #region Color utils
      //
      // Color utils
      //
      // Colors in NanoVG are stored as unsigned ints in ABGR format.

      // Returns a color value from red, green, blue values. Alpha will be set to 255 (1.0f).
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGB))]
      public static extern NVGColor RGB(byte r, byte g, byte b);
      
      // Returns a color value from red, green, blue values. Alpha will be set to 1.0f.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBf))]
      public static extern NVGColor RGBf(float r, float g, float b);

      // Returns a color value from red, green, blue and alpha values.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBA))]
      public static extern NVGColor RGBA(byte r, byte g, byte b, byte a);

      // Returns a color value from red, green, blue and alpha values.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RGBAf))]
      public static extern NVGColor RGBAf(float r, float g, float b, float a);

      // Linearly interpolates from color c0 to c1, and returns resulting color value.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LerpRGBA))]
      public static extern NVGColor LerpRGBA(NVGColor c0, NVGColor c1, float u);

      // Sets transparency of a color value.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransRGBA))]
      public static extern NVGColor TransRGBA(NVGColor c0, byte a);

      // Sets transparency of a color value.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransRGBAf))]
      public static extern NVGColor TransRGBAf(NVGColor c0, float a);

      // Returns color value specified by hue, saturation and lightness.
      // HSL values are all in range [0..1], alpha will be set to 255.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(HSL))]
      public static extern NVGColor HSL(float h, float s, float l);

      // Returns color value specified by hue, saturation and lightness and alpha.
      // HSL values are all in range [0..1], alpha in range [0..255]
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(HSLA))]
      public static extern NVGColor HSLA(float h, float s, float l, byte a);

      #endregion

      #region State Handling 
      //
      // State Handling
      //
      // NanoVG contains state which represents how paths will be rendered.
      // The state contains transform, fill and stroke styles, text and font styles,
      // and scissor clipping.

      // Pushes and saves the current render state into a state stack.
      // A matching nvgRestore() must be used to restore the state.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Save))]
      public static extern void Save(this NanoVGContext Ctx);

      // Pops and restores current render state.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Restore))]
      public static extern void Restore(this NanoVGContext Ctx);

      // Resets current render state to default values. Does not affect the render state stack.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Reset))]
      public static extern void Reset(this NanoVGContext Ctx);

      #endregion

      #region Render Styles
      //
      // Render styles
      //
      // Fill and stroke render style can be either a solid color or a paint which is a gradient or a pattern.
      // Solid color is simply defined as a color value, different kinds of paints can be created
      // using nvgLinearGradient(), nvgBoxGradient(), nvgRadialGradient() and nvgImagePattern().
      //
      // Current render style can be saved and restored using nvgSave() and nvgRestore().

      // Sets whether to draw antialias for nvgStroke() and nvgFill(). It's enabled by default.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ShapeAntiAlias))]
      public static extern void ShapeAntiAlias(this NanoVGContext Ctx, int enabled);

      // Sets current stroke style to a solid color.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokeColor))]
      public static extern void StrokeColor(this NanoVGContext Ctx, NVGColor color);

      // Sets current stroke style to a paint, which can be a one of the gradients or a pattern.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokePaint))]
      public static extern void StrokePaint(this NanoVGContext Ctx, NVGPaint paint);

      // Sets current fill style to a solid color.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FillColor))]
      public static extern void FillColor(this NanoVGContext Ctx, NVGColor color);

      // Sets current fill style to a paint, which can be a one of the gradients or a pattern.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FillPaint))]
      public static extern void FillPaint(this NanoVGContext Ctx, NVGPaint paint);

      // Sets the miter limit of the stroke style.
      // Miter limit controls when a sharp corner is beveled.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(MiterLimit))]
      public static extern void MiterLimit(this NanoVGContext Ctx, float limit);

      // Sets the stroke width of the stroke style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(StrokeWidth))]
      public static extern void StrokeWidth(this NanoVGContext Ctx, float size);

      // Sets how the end of the line (cap) is drawn,
      // Can be one of: NVG_BUTT (default), NVG_ROUND, NVG_SQUARE.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineCap))]
      public static extern void LineCap(this NanoVGContext Ctx, int cap);

      // Sets how sharp path corners are drawn.
      // Can be one of NVG_MITER (default), NVG_ROUND, NVG_BEVEL.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineJoin))]
      public static extern void LineJoin(this NanoVGContext Ctx, int join);

      // Sets the transparency applied to all rendered shapes.
      // Already transparent paths will get proportionally more transparent as well.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(GlobalAlpha))]
      public static extern void GlobalAlpha(this NanoVGContext Ctx, float alpha);

      #endregion

      #region Transforms
      //
      // Transforms
      //
      // The paths, gradients, patterns and scissor region are transformed by an transformation
      // matrix at the time when they are passed to the API.
      // The current transformation matrix is a affine matrix:
      //   [sx kx tx]
      //   [ky sy ty]
      //   [ 0  0  1]
      // Where: sx,sy define scaling, kx,ky skewing, and tx,ty translation.
      // The last row is assumed to be 0,0,1 and is not stored.
      //
      // Apart from nvgResetTransform(), each transformation function first creates
      // specific transformation matrix and pre-multiplies the current transformation by it.
      //
      // Current coordinate system (transformation) can be saved and restored using nvgSave() and nvgRestore().

      // Resets current transform to a identity matrix.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetTransform))]
      public static extern void ResetTransform(this NanoVGContext Ctx);

      // Premultiplies current coordinate system by specified matrix.
      // The parameters are interpreted as matrix as follows:
      //   [a c e]
      //   [b d f]
      //   [0 0 1]
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Transform))]
      public static extern void Transform(this NanoVGContext Ctx, float a, float b, float c, float d, float e, float f);

      // Translates current coordinate system.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Translate))]
      public static extern void Translate(this NanoVGContext Ctx, float x, float y);

      // Rotates current coordinate system. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Rotate))]
      public static extern void Rotate(this NanoVGContext Ctx, float angle);

      // Skews the current coordinate system along X axis. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(SkewX))]
      public static extern void SkewX(this NanoVGContext Ctx, float angle);

      // Skews the current coordinate system along Y axis. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(SkewY))]
      public static extern void SkewY(this NanoVGContext Ctx, float angle);

      // Scales the current coordinate system.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Scale))]
      public static extern void Scale(this NanoVGContext Ctx, float x, float y);

      // Stores the top part (a-f) of the current transformation matrix in to the specified buffer.
      //   [a c e]
      //   [b d f]
      //   [0 0 1]
      // There should be space for 6 floats in the return buffer for the values a-f.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CurrentTransform))]
      public static extern void CurrentTransform(this NanoVGContext Ctx, float* xform);


      // The following functions can be used to make calculations on 2x3 transformation matrices.
      // A 2x3 matrix is represented as float[6].

      // Sets the transform to identity matrix.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformIdentity))]
      public static extern void TransformIdentity(float* dst);

      // Sets the transform to translation matrix matrix.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformTranslate))]
      public static extern void TransformTranslate(float* dst, float tx, float ty);

      // Sets the transform to scale matrix.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformScale))]
      public static extern void TransformScale(float* dst, float sx, float sy);

      // Sets the transform to rotate matrix. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformRotate))]
      public static extern void TransformRotate(float* dst, float a);

      // Sets the transform to skew-x matrix. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformSkewX))]
      public static extern void TransformSkewX(float* dst, float a);

      // Sets the transform to skew-y matrix. Angle is specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformSkewY))]
      public static extern void TransformSkewY(float* dst, float a);

      // Sets the transform to the result of multiplication of two transforms, of A = A*B.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformMultiply))]
      public static extern void TransformMultiply(float* dst, float* src);

      // Sets the transform to the result of multiplication of two transforms, of A = B*A.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformPremultiply))]
      public static extern void TransformPremultiply(float* dst, float* src);

      // Sets the destination to inverse of specified transform.
      // Returns 1 if the inverse could be calculated, else 0.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformInverse))]
      public static extern int TransformInverse(float* dst, float* src);

      // Transform a point by given transform.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TransformPoint))]
      public static extern void TransformPoint(float* dstx, float* dsty, float* xform, float srcx, float srcy);

      // Converts degrees to radians and vice versa.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DegToRad))]
      public static extern float DegToRad(float deg);

      // Converts degrees to radians and vice versa.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RadToDeg))]
      public static extern float RadToDeg(float rad);
      #endregion

      #region Images
      //
      // Images
      //
      // NanoVG allows you to load jpg, png, psd, tga, pic and gif files to be used for rendering.
      // In addition you can upload your own image. The image loading is provided by stb_image.
      // The parameter imageFlags is combination of flags defined in NVGimageFlags.

      // Creates image by loading it from the disk from specified file name.
      // Returns handle to the image.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImage))]
      public static extern int CreateImage(this NanoVGContext Ctx, byte[] filename, int imageFlags);

      // Creates image by loading it from the specified chunk of memory.
      // Returns handle to the image.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImageMem))]
      public static extern int CreateImageMem(this NanoVGContext Ctx, int imageFlags, byte* data, int ndata);

      // Creates image from specified image data.
      // Returns handle to the image.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateImageRGBA))]
      public static extern int CreateImageRGBA(this NanoVGContext Ctx, int w, int h, int imageFlags, byte* data);

      // Updates image data specified by image handle.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(UpdateImage))]
      public static extern void UpdateImage(this NanoVGContext Ctx, int image, byte* data);

      // Returns the dimensions of a created image.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ImageSize))]
      public static extern void ImageSize(this NanoVGContext Ctx, int image, int* w, int* h);

      // Deletes created image.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(DeleteImage))]
      public static extern void DeleteImage(this NanoVGContext Ctx, int image);
      #endregion

      #region Paints
      //
      // Paints
      //
      // NanoVG supports four types of paints: linear gradient, box gradient, radial gradient and image pattern.
      // These can be used as paints for strokes and fills.

      // Creates and returns a linear gradient. Parameters (sx,sy)-(ex,ey) specify the start and end coordinates
      // of the linear gradient, icol specifies the start color and ocol the end color.
      // The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LinearGradient))]
      public static extern NVGPaint LinearGradient(this NanoVGContext Ctx, float sx, float sy, float ex, float ey, NVGColor icol, NVGColor ocol);

      // Creates and returns a box gradient. Box gradient is a feathered rounded rectangle, it is useful for rendering
      // drop shadows or highlights for boxes. Parameters (x,y) define the top-left corner of the rectangle,
      // (w,h) define the size of the rectangle, r defines the corner radius, and f feather. Feather defines how blurry
      // the border of the rectangle is. Parameter icol specifies the inner color and ocol the outer color of the gradient.
      // The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BoxGradient))]
      public static extern NVGPaint BoxGradient(this NanoVGContext Ctx, float x, float y, float w, float h, float r, float f, NVGColor icol, NVGColor ocol);

      // Creates and returns a radial gradient. Parameters (cx,cy) specify the center, inr and outr specify
      // the inner and outer radius of the gradient, icol specifies the start color and ocol the end color.
      // The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RadialGradient))]
      public static extern NVGPaint RadialGradient(this NanoVGContext Ctx, float cx, float cy, float inr, float outr, NVGColor icol, NVGColor ocol);

      // Creates and returns an image pattern. Parameters (ox,oy) specify the left-top location of the image pattern,
      // (ex,ey) the size of one image, angle rotation around the top-left corner, image is handle to the image to render.
      // The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ImagePattern))]
      public static extern NVGPaint ImagePattern(this NanoVGContext Ctx, float ox, float oy, float ex, float ey, float angle, int image, float alpha);
      #endregion

      #region Scissoring
      //
      // Scissoring
      //
      // Scissoring allows you to clip the rendering into a rectangle. This is useful for various
      // user interface cases like rendering a text edit or a timeline.

      // Sets the current scissor rectangle.
      // The scissor rectangle is transformed by the current transform.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Scissor))]
      public static extern void Scissor(this NanoVGContext Ctx, float x, float y, float w, float h);

      // Intersects current scissor rectangle with the specified rectangle.
      // The scissor rectangle is transformed by the current transform.
      // Note: in case the rotation of previous scissor rect differs from
      // the current one, the intersection will be done between the specified
      // rectangle and the previous scissor rectangle transformed in the current
      // transform space. The resulting shape is always rectangle.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(IntersectScissor))]
      public static extern void IntersectScissor(this NanoVGContext Ctx, float x, float y, float w, float h);

      // Reset and disables scissoring.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetScissor))]
      public static extern void ResetScissor(this NanoVGContext Ctx);

      #endregion

      #region Paths
      //
      // Paths
      //
      // Drawing a new shape starts with nvgBeginPath(), it clears all the currently defined paths.
      // Then you define one or more paths and sub-paths which describe the shape. The are functions
      // to draw common shapes like rectangles and circles, and lower level step-by-step functions,
      // which allow to define a path curve by curve.
      //
      // NanoVG uses even-odd fill rule to draw the shapes. Solid shapes should have counter clockwise
      // winding and holes should have counter clockwise order. To specify winding of a path you can
      // call nvgPathWinding(). This is useful especially for the common shapes, which are drawn CCW.
      //
      // Finally you can fill the path using current fill style by calling nvgFill(), and stroke it
      // with current stroke style by calling nvgStroke().
      //
      // The curve segments and sub-paths are transformed by the current transform.

      // Clears the current path and sub-paths.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BeginPath))]
      public static extern void BeginPath(this NanoVGContext Ctx);

      // Starts new sub-path with specified point as first point.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(MoveTo))]
      public static extern void MoveTo(this NanoVGContext Ctx, float x, float y);

      // Adds line segment from the last point in the path to the specified point.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(LineTo))]
      public static extern void LineTo(this NanoVGContext Ctx, float x, float y);

      // Adds cubic bezier segment from last point in the path via two control points to the specified point.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(BezierTo))]
      public static extern void BezierTo(this NanoVGContext Ctx, float c1x, float c1y, float c2x, float c2y, float x, float y);

      // Adds quadratic bezier segment from last point in the path via a control point to the specified point.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(QuadTo))]
      public static extern void QuadTo(this NanoVGContext Ctx, float cx, float cy, float x, float y);

      // Adds an arc segment at the corner defined by the last path point, and two specified points.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ArcTo))]
      public static extern void ArcTo(this NanoVGContext Ctx, float x1, float y1, float x2, float y2, float radius);

      // Closes current sub-path with a line segment.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ClosePath))]
      public static extern void ClosePath(this NanoVGContext Ctx);

      // Sets the current sub-path winding, see NVGwinding and NVGsolidity.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(PathWinding))]
      public static extern void PathWinding(this NanoVGContext Ctx, Solidity dir);

      // Creates new circle arc shaped sub-path. The arc center is at cx,cy, the arc radius is r,
      // and the arc is drawn from angle a0 to a1, and swept in direction dir (NVG_CCW, or NVG_CW).
      // Angles are specified in radians.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Arc))]
      public static extern void Arc(this NanoVGContext Ctx, float cx, float cy, float r, float a0, float a1, Winding dir);

      // Creates new rectangle shaped sub-path.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Rect))]
      public static extern void Rect(this NanoVGContext Ctx, float x, float y, float w, float h);
      
      // Creates new rounded rectangle shaped sub-path.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RoundedRect))]
      public static extern void RoundedRect(this NanoVGContext Ctx, float x, float y, float w, float h, float r);

      // Creates new rounded rectangle shaped sub-path with varying radii for each corner.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(RoundedRectVarying))]
      public static extern void RoundedRectVarying(this NanoVGContext Ctx, float x, float y, float w, float h, float radTopLeft, float radTopRight, float radBottomRight, float radBottomLeft);

      // Creates new ellipse shaped sub-path.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Ellipse))]
      public static extern void Ellipse(this NanoVGContext Ctx, float cx, float cy, float rx, float ry);

      // Creates new circle shaped sub-path.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Circle))]
      public static extern void Circle(this NanoVGContext Ctx, float cx, float cy, float r);

      // Fills the current path with current fill style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Fill))]
      public static extern void Fill(this NanoVGContext Ctx);

      // Fills the current path with current stroke style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Stroke))]
      public static extern void Stroke(this NanoVGContext Ctx);
      #endregion

      #region Text
      //
      // Text
      //
      // NanoVG allows you to load .ttf files and use the font to render text.
      //
      // The appearance of the text can be defined by setting the current text style
      // and by specifying the fill color. Common text and font settings such as
      // font size, letter spacing and text align are supported. Font blur allows you
      // to create simple text effects such as drop shadows.
      //
      // At render time the font face can be set based on the font handles or name.
      //
      // Font measure functions return values in local space, the calculations are
      // carried in the same resolution as the final rendering. This is done because
      // the text glyph positions are snapped to the nearest pixels sharp rendering.
      //
      // The local space means that values are not rotated or scale as per the current
      // transformation. For example if you set font size to 12, which would mean that
      // line height is 16, then regardless of the current scaling and rotation, the
      // returned line height is always 16. Some measures may vary because of the scaling
      // since aforementioned pixel snapping.
      //
      // While this may sound a little odd, the setup allows you to always render the
      // same way regardless of scaling. I.e. following works regardless of scaling:
      //
      //		const char* txt = "Text me up.";
      //		nvgTextBounds(vg, x,y, txt, NULL, bounds);
      //		nvgBeginPath(vg);
      //		nvgRoundedRect(vg, bounds[0],bounds[1], bounds[2]-bounds[0], bounds[3]-bounds[1]);
      //		nvgFill(vg);
      //
      // Note: currently only solid color fill is supported for text.

      // Creates font by loading it from the disk from specified file name.
      // Returns handle to the font.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFont))]
      public static extern int CreateFont(this NanoVGContext Ctx, byte[] name, byte[] filename);

      // fontIndex specifies which font face to load from a .ttf/.ttc file.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFontAtIndex))]
      public static extern int CreateFontAtIndex(this NanoVGContext Ctx, string name, string filename, int fontIndex);

      // Creates font by loading it from the specified memory chunk.
      // Returns handle to the font.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFontMem))]
      public static extern int CreateFontMem(this NanoVGContext Ctx, byte[] name, byte* data, int ndata, int freeData);

      // fontIndex specifies which font face to load from a .ttf/.ttc file.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(CreateFontMemAtIndex))]
      public static extern int CreateFontMemAtIndex(this NanoVGContext Ctx, string name, IntPtr data, int ndata, int freeData, int fontIndex);

      // Finds a loaded font of specified name, and returns handle to it, or -1 if the font is not found.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FindFont))]
      public static extern int FindFont(this NanoVGContext Ctx, byte[] name);

      // Adds a fallback font by handle.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(AddFallbackFontId))]
      public static extern int AddFallbackFontId(this NanoVGContext Ctx, int baseFont, int fallbackFont);

      // Adds a fallback font by name.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(AddFallbackFont))]
      public static extern int AddFallbackFont(this NanoVGContext Ctx, byte[] baseFont, byte[] fallbackFont);

      // Resets fallback fonts by handle.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetFallbackFontsId))]
      public static extern void ResetFallbackFontsId(this NanoVGContext Ctx, int baseFont);

      // Resets fallback fonts by name.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(ResetFallbackFontsId))]
      public static extern void ResetFallbackFonts(this NanoVGContext Ctx, string baseFont);

      // Sets the font size of current text style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontSize))]
      public static extern void FontSize(this NanoVGContext Ctx, float size);

      // Sets the blur of current text style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontBlur))]
      public static extern void FontBlur(this NanoVGContext Ctx, float blur);

      // Sets the letter spacing of current text style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextLetterSpacing))]
      public static extern void TextLetterSpacing(this NanoVGContext Ctx, float spacing);

      // Sets the proportional line height of current text style. The line height is specified as multiple of font size.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextLineHeight))]
      public static extern void TextLineHeight(this NanoVGContext Ctx, float lineHeight);

      // Sets the text align of current text style, see NVGalign for options.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextAlign))]
      public static extern void TextAlign(this NanoVGContext Ctx, Align align);

      // Sets the font face based on specified id of current text style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontFaceId))]
      public static extern void FontFaceId(this NanoVGContext Ctx, int font);

      // Sets the font face based on specified id of current text style.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(FontFace))]
      public static extern void FontFace(this NanoVGContext Ctx, byte[] font);

      // Draws text string at specified location. If end is specified only the sub-string up to the end is drawn.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(Text))]
      public static extern float Text(this NanoVGContext Ctx, float x, float y, byte[] Str, byte[] end);

      // Draws multi-line text string at specified location wrapped at the specified width. If end is specified only the sub-string up to the end is drawn.
      // White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered.
      // Words longer than the max width are slit at nearest character (i.e. no hyphenation).
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBox))]
      public static extern void TextBox(this NanoVGContext Ctx, float x, float y, float breakRowWidth, byte[] Str, byte[] end);

      // Measures the specified text string. Parameter bounds should be a pointer to float[4],
      // if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax]
      // Returns the horizontal advance of the measured text (i.e. where the next character should drawn).
      // Measured values are returned in local coordinate space.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBounds))]
      public static extern float TextBounds(this NanoVGContext Ctx, float x, float y, byte[] Str, byte[] end, float* bounds);

      // Measures the specified multi-text string. Parameter bounds should be a pointer to float[4],
      // if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax]
      // Measured values are returned in local coordinate space.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBoxBounds))]
      public static extern void TextBoxBounds(this NanoVGContext Ctx, float x, float y, float breakRowWidth, byte[] Str, byte[] end, float* bounds);

      // Calculates the glyph x positions of the specified text. If end is specified only the sub-string will be used.
      // Measured values are returned in local coordinate space.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextGlyphPositions))]
      public static extern int TextGlyphPositions(this NanoVGContext Ctx, float x, float y, byte[] Str, byte[] end, /* NVGglyphPosition* */ IntPtr positions, int maxPositions);

      // Returns the vertical metrics based on the current text style.
      // Measured values are returned in local coordinate space.
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextMetrics))]
      public static extern void TextMetrics(this NanoVGContext Ctx, float* ascender, float* descender, float* lineh);

      // Breaks the specified text into lines. If end is specified only the sub-string will be used.
      // White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered.
      // Words longer than the max width are slit at nearest character (i.e. no hyphenation).
      [DllImport(LibName, CallingConvention = CConv, EntryPoint = FuncPrefix + nameof(TextBreakLines))]
      public static extern int TextBreakLines(this NanoVGContext Ctx, byte[] Str, byte[] end, float breakRowWidth, /*NVGtextRow* */ IntPtr rows, int maxRows);
      #endregion
   }
}