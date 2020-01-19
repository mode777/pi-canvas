using System.Runtime.InteropServices;

namespace OpenVg
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public readonly float X;
        public readonly float Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Line
    {
        public Line(float x0, float y0, float x1, float y1)
        {
            From = new Point(x0, y0);
            To = new Point(x1, y1);
        }

        public Line(Point from, Point to)
        {
            From = from;
            To = to;
        }

        public readonly Point From;
        public readonly Point To;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RadialCoord
    {
        public RadialCoord(float cx, float cy, float ox, float oy, float radius)
            : this(new Point(cx,cy), new Point(ox,oy), radius)
        {
        }

        public RadialCoord(Point center, Point offset, float radius)
        {
            Center = center;
            Offset = offset;
            Radius = radius;
        }

        public readonly Point Center;
        public readonly Point Offset;
        public readonly float Radius;
    }
}