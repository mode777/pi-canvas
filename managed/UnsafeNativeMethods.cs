using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    internal static class UnsafeNativeMethods
    {
        const string Library = "./pic.so";
        [DllImport(Library, EntryPoint = "picInit")] public static extern SizeF Init();
        [DllImport(Library, EntryPoint = "picDispose")] public static extern void Dispose();
        [DllImport(Library, EntryPoint = "picClearColor")] public static extern void ClearColor(ColorF colorf);
        [DllImport(Library, EntryPoint = "picClear")] public static extern void Clear(float x, float y, float w, float h);
        [DllImport(Library, EntryPoint = "picPresent")] public static extern void Present();
        [DllImport(Library, EntryPoint = "picCreateColor")] public static extern IntPtr CreateColorBrush(ColorF colorf);
        [DllImport(Library, EntryPoint = "picCreateLinearGradient")] public unsafe static extern IntPtr CreateLinearGradientBrush(Line line, ColorStop* stops, int n_stops, SpreadMode spread);
        [DllImport(Library, EntryPoint = "picCreateRadialGradient")] public unsafe static extern IntPtr CreateRadialGradientBrush(RadialCoord coord, ColorStop* stops, int n_stops, SpreadMode spread);
        [DllImport(Library, EntryPoint = "picDeletePaint")] public static extern void DeleteBrush(IntPtr path);
        [DllImport(Library, EntryPoint = "picCreateElipse")] public static extern IntPtr CreateEllipsePath(float x, float y, float w, float h);
        [DllImport(Library, EntryPoint = "picCreateRect")] public static extern IntPtr CreateRectPath(float x, float y, float w, float h);
        [DllImport(Library, EntryPoint = "picCreatePolygon")] public unsafe static extern IntPtr CreatePolygonPath(PointF * point, int n);
        [DllImport(Library, EntryPoint = "picCreateLine")] public static extern IntPtr CreateLinePath(float x1, float y1, float x2, float y2);
        [DllImport(Library, EntryPoint = "picCreateRoundRect")] public static extern IntPtr CreateRoundRectPath(float x, float y, float w, float h, float rw, float rh);
        [DllImport(Library, EntryPoint = "picCreatePath")] public unsafe static extern IntPtr CreatePath(byte* segments, int n_segments, float* data);
        [DllImport(Library, EntryPoint = "picDrawPath")] public static extern void DrawPath(IntPtr path, IntPtr fillBrush, IntPtr outlineBrush, float width);
        [DllImport(Library, EntryPoint = "picDeletePath")] public static extern void DeletePath(IntPtr path);
        [DllImport(Library, EntryPoint = "picTranslate")] public static extern void Translate(float x, float y);
        [DllImport(Library, EntryPoint = "picScale")] public static extern void Scale(float x, float y);
        [DllImport(Library, EntryPoint = "picRotate")] public static extern void Rotate(float r);
        [DllImport(Library, EntryPoint = "picShear")] public static extern void Shear(float x, float y);
        [DllImport(Library, EntryPoint = "picIdentity")] public static extern void LoadIdentity();
        [DllImport(Library, EntryPoint = "picPush")] public static extern void Push();
        [DllImport(Library, EntryPoint = "picPop")] public static extern void Pop();
        [DllImport(Library, EntryPoint = "picLoadFont", CharSet = CharSet.Ansi)] public static extern IntPtr LoadFont(string file);
        [DllImport(Library, EntryPoint = "picDeleteFont")] public static extern void DisposeFont(IntPtr font);
        [DllImport(Library, EntryPoint = "picDrawText", CharSet = CharSet.Ansi)] public static extern void DrawText(IntPtr font, PointF pos, string text, int count, float size, IntPtr fill, IntPtr stroke, float linewidth);

    }
}
