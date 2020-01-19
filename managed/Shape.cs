using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    public class VgPath : VgObject
    {
        public static VgPath CreateEllipse(float x, float y, float w, float h)
        {
            return new VgPath(UnsafeNativeMethods.CreateEllipsePath(x, y, w, h));
        }

        public static VgPath CreateRect(float x, float y, float w, float h)
        {
            return new VgPath(UnsafeNativeMethods.CreateRectPath(x, y, w, h));
        }

        public static VgPath CreatePolygon(params PointF[] points)
        {
            unsafe
            {
                fixed (PointF* pointPtr = points)
                {
                    return new VgPath(UnsafeNativeMethods.CreatePolygonPath(pointPtr, points.Length));
                }
            }
        }

        public static VgPath CreateLine(float x1, float y1, float x2, float y2)
        {
            return new VgPath(UnsafeNativeMethods.CreateLinePath(x1, y1, x2, y2));
        }

        public static VgPath CreateRoundRectangle(float x, float y, float w, float h, float rw, float rh)
        {
            return new VgPath(UnsafeNativeMethods.CreateRoundRectPath(x, y, w, h, rw, rh));
        }

        internal VgPath(IntPtr ptr)
        {
            Handle = ptr;
        }
    }

    public class VgPathBuilder
    {
        internal List<PathSegment> Segments { get; } = new List<PathSegment>();
        internal List<float> Data { get; } = new List<float>();

        public VgPathBuilder Close()
        {
            Segments.Add(PathSegment.CLOSE_PATH);
            return this;
        }

        public VgPathBuilder Move(float x, float y, bool relative = false)
        {
            Segments.Add(PathSegment.MOVE_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x);
            Data.Add(y);
            return this;
        }

        public VgPathBuilder Line(float x, float y, bool relative = false)
        {
            Segments.Add(PathSegment.LINE_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x);
            Data.Add(y);
            return this;
        }        

        public VgPathBuilder HorizontalLine(float x, bool relative = false)
        {
            Segments.Add(PathSegment.HLINE_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x);
            return this;
        }

        public VgPathBuilder VerticalLine(float y, bool relative = false)
        {
            Segments.Add(PathSegment.VLINE_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(y);
            return this;
        }

        public VgPathBuilder Quadratic(float x0, float y0, float x1, float y1, bool relative = false)
        {
            Segments.Add(PathSegment.QUAD_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x0);
            Data.Add(y0);
            Data.Add(x1);
            Data.Add(y1);
            return this;
        }

        public VgPathBuilder Cubic(float x0, float y0, float x1, float y1, float x2, float y2, bool relative = false)
        {
            Segments.Add(PathSegment.CUBIC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x0);
            Data.Add(y0);
            Data.Add(x1);
            Data.Add(y1);
            Data.Add(x2);
            Data.Add(y2);
            return this;
        }

        public VgPathBuilder SmoothQuad(float x1, float y1, bool relative = false)
        {
            Segments.Add(PathSegment.SQUAD_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x1);
            Data.Add(y1);
            return this;
        }

        public VgPathBuilder SmoothCubic(float x1, float y1, float x2, float y2, bool relative = false)
        {
            Segments.Add(PathSegment.SCUBIC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(x1);
            Data.Add(y1);
            Data.Add(x2);
            Data.Add(y2);
            return this;
        }

        public VgPathBuilder SmallCCWArc(float rh, float rv, float rot, float x0, float y0, bool relative = false)
        {
            Segments.Add(PathSegment.SCCWARC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(rh);
            Data.Add(rv);
            Data.Add(rot);
            Data.Add(x0);
            Data.Add(y0);

            return this;
        }

        public VgPathBuilder SmallCWArc(float rh, float rv, float rot, float x0, float y0, bool relative = false)
        {
            Segments.Add(PathSegment.SCWARC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(rh);
            Data.Add(rv);
            Data.Add(rot);
            Data.Add(x0);
            Data.Add(y0);

            return this;
        }

        public VgPathBuilder LargeCWArc(float rh, float rv, float rot, float x0, float y0, bool relative = false)
        {
            Segments.Add(PathSegment.LCWARC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(rh);
            Data.Add(rv);
            Data.Add(rot);
            Data.Add(x0);
            Data.Add(y0);

            return this;
        }

        public VgPathBuilder LargeCCWArc(float rh, float rv, float rot, float x0, float y0, bool relative = false)
        {
            Segments.Add(PathSegment.LCCWARC_TO | (relative ? PathSegment.RELATIVE_FLAG : 0));
            Data.Add(rh);
            Data.Add(rv);
            Data.Add(rot);
            Data.Add(x0);
            Data.Add(y0);

            return this;
        }

        public VgPath Build()
        {
            unsafe
            {
                fixed(byte* bytePtr = Segments.Cast<byte>().ToArray())
                {
                    fixed(float* floatPtr = Data.ToArray())
                    {
                        var handle = UnsafeNativeMethods.CreatePath(bytePtr, Segments.Count, floatPtr);
                        var path = new VgPath(handle);
                        return path;
                    }
                }
            }
        }
        
        private int ArgsPerSegment(PathSegment seg)
        {
            // clear relative flag
            seg = seg & ~PathSegment.RELATIVE_FLAG;

            switch (seg)
            {
                case PathSegment.CLOSE_PATH:
                    return 0;
                case PathSegment.MOVE_TO:
                    return 2;
                case PathSegment.LINE_TO:
                    return 2;
                case PathSegment.HLINE_TO:
                    return 1;
                case PathSegment.VLINE_TO:
                    return 1;
                case PathSegment.QUAD_TO:
                    return 4;
                case PathSegment.CUBIC_TO:
                    return 6;
                case PathSegment.SQUAD_TO:
                    return 2;
                case PathSegment.SCUBIC_TO:
                    return 4;
                case PathSegment.SCCWARC_TO:
                    return 6;
                case PathSegment.SCWARC_TO:
                    return 6;
                case PathSegment.LCCWARC_TO:
                    return 6;
                case PathSegment.LCWARC_TO:
                    return 6;
                default:
                    throw new ArgumentOutOfRangeException(nameof(seg), seg, null);
            }
        }
    }
    
    [Flags]
    public enum PathSegment : byte
    {
        CLOSE_PATH = 0,
        RELATIVE_FLAG = 1,
        MOVE_TO = 2,
        LINE_TO = 4,
        HLINE_TO = 6,
        VLINE_TO = 8,
        QUAD_TO = 10,
        CUBIC_TO = 12,
        SQUAD_TO = 14,
        SCUBIC_TO = 16,
        SCCWARC_TO = 18,
        SCWARC_TO = 20,
        LCCWARC_TO = 22,
        LCWARC_TO = 24
    }
}
