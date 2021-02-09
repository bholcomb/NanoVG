using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

using NanoVG;

namespace TestNanoVG
{
   public class TestNanoVG : GameWindow
   {
      NanoVGContext vg;

      public TestNanoVG(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
      {
         this.VSync = VSyncMode.Off;
      }

      protected override void OnLoad()
      {
         base.OnLoad();

         string version = GL.GetString(StringName.Version);
         int major = System.Convert.ToInt32(version[0].ToString());
         int minor = System.Convert.ToInt32(version[2].ToString());
         if (major < 3 && minor < 3)
         {
            //MessageBox.Show("You need at least OpenGL 4.4 to run this example. Aborting.", "Ooops", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            System.Environment.Exit(-1);
         }
         System.Console.WriteLine("Found OpenGL Version: {0}.{1}", major, minor);

         GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
         GL.ClearDepth(1.0f);


         vg = NVG.CreateGL3(CreateFlags.ANTIALIAS | CreateFlags.STENCIL_STROKES | CreateFlags.DEBUG);

         int Icons = vg.CreateFont("icons", "../data/fonts/entypo.ttf");
         int Sans = vg.CreateFont("sans", "../data/fonts/Roboto-Regular.ttf");
         int SansBold = vg.CreateFont("sans-bold", "../data/fonts/Roboto-Bold.ttf");
         int Emoji = vg.CreateFont("emoji", "../data/fonts/NotoEmoji-Regular.ttf");

         vg.AddFallbackFontId(Sans, Emoji);
         vg.AddFallbackFontId(SansBold, Emoji);
      }

      protected override void OnRenderFrame(FrameEventArgs e)
      {
         base.OnRenderFrame(e);
         ProcessEvents();

         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         vg.BeginFrame(ClientSize.X, ClientSize.Y, 1.0f);

         demo();

         vg.EndFrame();
         SwapBuffers();
      }

      void demo()
      {
         int x = 0;
         int y = 0;

         // Widgets
         DrawWindow(vg, "Widgets 'n Stuff", 50, 50, 300, 400);
         x = 60;
         y = 95;
         DrawSearchBox(vg, "Search 😂🔫", x, y, 280, 25);

         DrawColorWheel(vg, 300, 150, 400, 400, 1);
      }

      static void DrawWindow(NanoVGContext Ctx, string Title, float X, float Y, float W, float H)
      {
         float CornerRadius = 3;
         NVGPaint ShadowPaint;
         NVGPaint HeaderPaint;
         Ctx.Save();

         // Window
         Ctx.BeginPath();
         Ctx.RoundedRect(X, Y, W, H, CornerRadius);
         Ctx.FillColor(NVG.RGBA(28, 30, 34, 192));
         Ctx.Fill();

         // Drop shadow
         ShadowPaint = Ctx.BoxGradient(X, Y + 2, W, H, CornerRadius * 2, 10, NVG.RGBA(0, 0, 0, 128), NVG.RGBA(0, 0, 0, 0));
         Ctx.BeginPath();
         Ctx.Rect(X - 10, Y - 10, W + 20, H + 30);
         Ctx.RoundedRect(X, Y, W, H, CornerRadius);
         Ctx.PathWinding(Solidity.HOLE);
         Ctx.FillPaint(ShadowPaint);
         Ctx.Fill();

         // Header
         HeaderPaint = Ctx.LinearGradient(X, Y, X, Y + 15, NVG.RGBA(255, 255, 255, 8), NVG.RGBA(0, 0, 0, 16));
         Ctx.BeginPath();
         Ctx.RoundedRect(X + 1, Y + 1, W - 2, 30, CornerRadius - 1);
         Ctx.FillPaint(HeaderPaint);
         Ctx.Fill();
         Ctx.BeginPath();
         Ctx.MoveTo(X + 0.5f, Y + 0.5f + 30);
         Ctx.LineTo(X + 0.5f + W - 1, Y + 0.5f + 30);
         Ctx.StrokeColor(NVG.RGBA(0, 0, 0, 32));
         Ctx.Stroke();

         Ctx.FontSize(18.0f);
         Ctx.FontFace("sans-bold");
         Ctx.TextAlign(Align.CENTER | Align.MIDDLE);

         Ctx.FontBlur(2);
         Ctx.FillColor(NVG.RGBA(0, 0, 0, 128));
         Ctx.Text(X + W / 2, Y + 16 + 1, Title, null);

         Ctx.FontBlur(0);
         Ctx.FillColor(NVG.RGBA(220, 220, 220, 160));
         Ctx.Text(X + W / 2, Y + 16, Title, null);

         Ctx.Restore();
      }

      static void DrawSearchBox(NanoVGContext vg, string text, float x, float y, float w, float h)
      {
         NVGPaint bg;
         float cornerRadius = h / 2 - 1;

         // Edit
         bg = NVG.BoxGradient(vg, x, y + 1.5f, w, h, h / 2, 5, NVG.RGBA(0, 0, 0, 16), NVG.RGBA(0, 0, 0, 92));
         NVG.BeginPath(vg);
         NVG.RoundedRect(vg, x, y, w, h, cornerRadius);
         NVG.FillPaint(vg, bg);
         NVG.Fill(vg);

         NVG.FontSize(vg, h * 1.3f);
         NVG.FontFace(vg, "icons");
         NVG.FillColor(vg, NVG.RGBA(255, 255, 255, 64));
         NVG.TextAlign(vg, Align.CENTER | Align.MIDDLE);
         NVG.Text(vg, x + h * 0.55f, y + h * 0.55f, Char.ConvertFromUtf32(ICON_SEARCH), null);

         NVG.FontSize(vg, 20.0f);
         NVG.FontFace(vg, "sans");
         NVG.FillColor(vg, NVG.RGBA(255, 255, 255, 32));

         NVG.TextAlign(vg, Align.LEFT | Align.MIDDLE);
         NVG.Text(vg, x + h * 1.05f, y + h * 0.5f, text, null);

         NVG.FontSize(vg, h * 1.3f);
         NVG.FontFace(vg, "icons");
         NVG.FillColor(vg, NVG.RGBA(255, 255, 255, 32));
         NVG.TextAlign(vg, Align.CENTER | Align.MIDDLE);
         NVG.Text(vg, x + w - h * 0.55f, y + h * 0.55f, Char.ConvertFromUtf32(ICON_CIRCLED_CROSS), null);
      }

      static void DrawColorWheel(NanoVGContext vg, float x, float y, float w, float h, float t)
      {
         int i;
         float r0, r1, ax, ay, bx, by, cx, cy, aeps, r;
         float hue = (float)Math.Sin(t * 0.12f);
         NVGPaint paint;

         NVG.Save(vg);

         cx = x + w * 0.5f;
         cy = y + h * 0.5f;
         r1 = (w < h ? w : h) * 0.5f - 5.0f;
         r0 = r1 - 20.0f;
         aeps = 0.5f / r1;   // half a pixel arc length in radians (2pi cancels out).

         for (i = 0; i < 6; i++)
         {
            float a0 = (float)i / 6.0f * (float)Math.PI * 2.0f - aeps;
            float a1 = (float)(i + 1.0f) / 6.0f * (float)Math.PI * 2.0f + aeps;
            NVG.BeginPath(vg);
            NVG.Arc(vg, cx, cy, r0, a0, a1, Winding.CW);
            NVG.Arc(vg, cx, cy, r1, a1, a0, Winding.CCW);
            NVG.ClosePath(vg);
            ax = cx + (float)Math.Cos(a0) * (r0 + r1) * 0.5f;
            ay = cy + (float)Math.Sin(a0) * (r0 + r1) * 0.5f;
            bx = cx + (float)Math.Cos(a1) * (r0 + r1) * 0.5f;
            by = cy + (float)Math.Sin(a1) * (r0 + r1) * 0.5f;
            paint = NVG.LinearGradient(vg, ax, ay, bx, by, NVG.HSLA(a0 / ((float)Math.PI * 2), 1.0f, 0.55f, 255), NVG.HSLA(a1 / ((float)Math.PI * 2), 1.0f, 0.55f, 255));
            NVG.FillPaint(vg, paint);
            NVG.Fill(vg);
         }

         NVG.BeginPath(vg);
         NVG.Circle(vg, cx, cy, r0 - 0.5f);
         NVG.Circle(vg, cx, cy, r1 + 0.5f);
         NVG.StrokeColor(vg, NVG.RGBA(0, 0, 0, 64));
         NVG.StrokeWidth(vg, 1.0f);
         NVG.Stroke(vg);

         // Selector
         NVG.Save(vg);
         NVG.Translate(vg, cx, cy);
         NVG.Rotate(vg, hue * (float)Math.PI * 2);

         // Marker on
         NVG.StrokeWidth(vg, 2.0f);
         NVG.BeginPath(vg);
         NVG.Rect(vg, r0 - 1, -3, r1 - r0 + 2, 6);
         NVG.StrokeColor(vg, NVG.RGBA(255, 255, 255, 192));
         NVG.Stroke(vg);

         paint = NVG.BoxGradient(vg, r0 - 3, -5, r1 - r0 + 6, 10, 2, 4, NVG.RGBA(0, 0, 0, 128), NVG.RGBA(0, 0, 0, 0));
         NVG.BeginPath(vg);
         NVG.Rect(vg, r0 - 2 - 10, -4 - 10, r1 - r0 + 4 + 20, 8 + 20);
         NVG.Rect(vg, r0 - 2, -4, r1 - r0 + 4, 8);
         NVG.PathWinding(vg, Solidity.HOLE);
         NVG.FillPaint(vg, paint);
         NVG.Fill(vg);

         // Center triangle
         r = r0 - 6;
         ax = (float)Math.Cos(120.0f / 180.0f * (float)Math.PI) * r;
         ay = (float)Math.Sin(120.0f / 180.0f * (float)Math.PI) * r;
         bx = (float)Math.Cos(-120.0f / 180.0f * (float)Math.PI) * r;
         by = (float)Math.Sin(-120.0f / 180.0f * (float)Math.PI) * r;
         NVG.BeginPath(vg);
         NVG.MoveTo(vg, r, 0);
         NVG.LineTo(vg, ax, ay);
         NVG.LineTo(vg, bx, by);
         NVG.ClosePath(vg);
         paint = NVG.LinearGradient(vg, r, 0, ax, ay, NVG.HSLA(hue, 1.0f, 0.5f, 255), NVG.RGBA(255, 255, 255, 255));
         NVG.FillPaint(vg, paint);
         NVG.Fill(vg);
         paint = NVG.LinearGradient(vg, (r + ax) * 0.5f, (0 + ay) * 0.5f, bx, by, NVG.RGBA(0, 0, 0, 0), NVG.RGBA(0, 0, 0, 255));
         NVG.FillPaint(vg, paint);
         NVG.Fill(vg);
         NVG.StrokeColor(vg, NVG.RGBA(0, 0, 0, 64));
         NVG.Stroke(vg);

         // Select circle on triangle
         ax = (float)Math.Cos(120.0f / 180.0f * (float)Math.PI) * r * 0.3f;
         ay = (float)Math.Sin(120.0f / 180.0f * (float)Math.PI) * r * 0.4f;
         NVG.StrokeWidth(vg, 2.0f);
         NVG.BeginPath(vg);
         NVG.Circle(vg, ax, ay, 5);
         NVG.StrokeColor(vg, NVG.RGBA(255, 255, 255, 192));
         NVG.Stroke(vg);

         paint = NVG.RadialGradient(vg, ax, ay, 7, 9, NVG.RGBA(0, 0, 0, 64), NVG.RGBA(0, 0, 0, 0));
         NVG.BeginPath(vg);
         NVG.Rect(vg, ax - 20, ay - 20, 40, 40);
         NVG.Circle(vg, ax, ay, 7);
         NVG.PathWinding(vg, Solidity.HOLE);
         NVG.FillPaint(vg, paint);
         NVG.Fill(vg);

         NVG.Restore(vg);

         NVG.Restore(vg);
      }

      const int ICON_SEARCH = 0x1F50D;
      const int ICON_CIRCLED_CROSS = 0x2716;
      const int ICON_CHEVRON_RIGHT = 0xE75E;
      const int ICON_CHECK = 0x2713;
      const int ICON_LOGIN = 0xE740;
      const int ICON_TRASH = 0xE729;
   };

   class Program
   {
      static GameWindowSettings gameWindowSettings = new GameWindowSettings();
      static NativeWindowSettings nativeWindowSettings = new NativeWindowSettings();
      public static int theWidth = 1280;
      public static int theHeigth = 700;

      static void Main(string[] args)
      {
         gameWindowSettings.RenderFrequency = 30.0;
         gameWindowSettings.UpdateFrequency = 30.0;
         nativeWindowSettings.API = ContextAPI.OpenGL;
         nativeWindowSettings.APIVersion = new Version(3, 3);
         nativeWindowSettings.Title = "TestNanoVG";
         nativeWindowSettings.Size = new Vector2i(theWidth, theHeigth);
         nativeWindowSettings.Location = new Vector2i(0, 30);

         using (TestNanoVG app = new TestNanoVG(gameWindowSettings, nativeWindowSettings))
         {
            app.CenterWindow();
            app.Title = "TestNanoVG";
            app.Run();
         }
      }
   }
}
