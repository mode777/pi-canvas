using System;
using System.Drawing;
using System.Threading;


namespace OpenVg
{
    public sealed class Canvas : IDisposable
    {
        private static bool exists = false;

        public static Canvas Create()
        {
            if (exists == false)
            {
                exists = true;
                return new Canvas();
            }
            else
            {
                throw new InvalidOperationException("Canvas already exists");
            }
        }

        private Canvas()
        {
            var p = UnsafeNativeMethods.Init();
            Width = (int)p.Width;
            Height = (int)p.Height;
        }

        public int Width { get; }
        public int Height { get; }

        public void BackgroundColor(ColorF colorf)
        {
            UnsafeNativeMethods.ClearColor(colorf);
        }

        public void Clear()
        {
            UnsafeNativeMethods.Clear(0, 0, Width, Height);
        }

        public void Translate(float x, float y)
        {
            UnsafeNativeMethods.Translate(x,y);
        }

        public void Rotate(float r)
        {
            UnsafeNativeMethods.Rotate(r);
        }

        public void Scale(float x, float y)
        {
            UnsafeNativeMethods.Scale(x,y);
        }

        public void Shear(float x, float y)
        {
            UnsafeNativeMethods.Shear(x,y);
        }

        public void LoadIdentity()
        {
            UnsafeNativeMethods.LoadIdentity();
        }

        public void DrawPath(VgPath path, VgPaint fill, VgPaint stroke = null, float lineWidth = 1)
        {
            UnsafeNativeMethods.DrawPath(path.Handle, fill?.Handle ?? IntPtr.Zero, stroke?.Handle ?? IntPtr.Zero, lineWidth);
        }

        public void DrawText(VgFont font, PointF position, string text, float fontSize, VgPaint fill, VgPaint stroke = null, float lineWidth = 1)
        {
            UnsafeNativeMethods.DrawText(font.Handle, position, text, text.Length, fontSize, fill?.Handle ?? IntPtr.Zero, stroke?.Handle ?? IntPtr.Zero, lineWidth);
        }

        public void Present()
        {
            UnsafeNativeMethods.Present();
        }        

        private void ReleaseUnmanagedResources()
        {
            UnsafeNativeMethods.Dispose();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Canvas()
        {
            ReleaseUnmanagedResources();
        }
    }
}
