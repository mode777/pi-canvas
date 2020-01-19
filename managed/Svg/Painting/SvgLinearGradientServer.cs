using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

using System.Linq;

namespace Svg
{
    [SvgElement("linearGradient")]
    public sealed class SvgLinearGradientServer : SvgGradientServer
    {
        [SvgAttribute("x1")]
        public SvgUnit X1
        {
            get { return GetAttribute("x1", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["x1"] = value; }
        }

        [SvgAttribute("y1")]
        public SvgUnit Y1
        {
            get { return GetAttribute("y1", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["y1"] = value; }
        }

        [SvgAttribute("x2")]
        public SvgUnit X2
        {
            get { return GetAttribute("x2", false, new SvgUnit(SvgUnitType.Percentage, 100f)); }
            set { Attributes["x2"] = value; }
        }

        [SvgAttribute("y2")]
        public SvgUnit Y2
        {
            get { return GetAttribute("y2", false, new SvgUnit(SvgUnitType.Percentage, 0f)); }
            set { Attributes["y2"] = value; }
        }

        private bool IsInvalid
        {
            get
            {
                // Need at least 2 colours to do the gradient fill
                return this.Stops.Count < 2;
            }
        }

        private SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return (orig.Type == SvgUnitType.Percentage && this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                    new SvgUnit(SvgUnitType.User, orig.Value / 100) :
                    orig);
        }

        [Flags]
        private enum LinePoints
        {
            None = 0,
            Start = 1,
            End = 2
        }

        public struct GradientPoints
        {
            public PointF StartPoint;
            public PointF EndPoint;

            public GradientPoints(PointF startPoint, PointF endPoint)
            {
                this.StartPoint = startPoint;
                this.EndPoint = endPoint;
            }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgLinearGradientServer>();
        }

        private sealed class LineF
        {
            private float X1
            {
                get;
                set;
            }

            private float Y1
            {
                get;
                set;
            }

            private float X2
            {
                get;
                set;
            }

            private float Y2
            {
                get;
                set;
            }

            public LineF(float x1, float y1, float x2, float y2)
            {
                X1 = x1;
                Y1 = y1;
                X2 = x2;
                Y2 = y2;
            }

            public List<PointF> Intersection(RectangleF rectangle)
            {
                var result = new List<PointF>();

                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Y, rectangle.Right, rectangle.Y), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Y, rectangle.Right, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.Right, rectangle.Bottom, rectangle.X, rectangle.Bottom), result);
                AddIfIntersect(this, new LineF(rectangle.X, rectangle.Bottom, rectangle.X, rectangle.Y), result);

                return result;
            }

            /// <remarks>http://community.topcoder.com/tc?module=Static&amp;d1=tutorials&amp;d2=geometry2</remarks>
            private PointF? Intersection(LineF other)
            {
                const int precision = 8;

                var a1 = (double)Y2 - Y1;
                var b1 = (double)X1 - X2;
                var c1 = a1 * X1 + b1 * Y1;

                var a2 = (double)other.Y2 - other.Y1;
                var b2 = (double)other.X1 - other.X2;
                var c2 = a2 * other.X1 + b2 * other.Y1;

                var det = a1 * b2 - a2 * b1;
                if (det == 0)
                {
                    return null;
                }
                else
                {
                    var xi = (b2 * c1 - b1 * c2) / det;
                    var yi = (a1 * c2 - a2 * c1) / det;

                    if (Math.Round(Math.Min(X1, X2), precision) <= Math.Round(xi, precision) &&
                        Math.Round(xi, precision) <= Math.Round(Math.Max(X1, X2), precision) &&
                        Math.Round(Math.Min(Y1, Y2), precision) <= Math.Round(yi, precision) &&
                        Math.Round(yi, precision) <= Math.Round(Math.Max(Y1, Y2), precision) &&
                        Math.Round(Math.Min(other.X1, other.X2), precision) <= Math.Round(xi, precision) &&
                        Math.Round(xi, precision) <= Math.Round(Math.Max(other.X1, other.X2), precision) &&
                        Math.Round(Math.Min(other.Y1, other.Y2), precision) <= Math.Round(yi, precision) &&
                        Math.Round(yi, precision) <= Math.Round(Math.Max(other.Y1, other.Y2), precision))
                    {
                        return new PointF((float)xi, (float)yi);
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            private static void AddIfIntersect(LineF first, LineF second, ICollection<PointF> result)
            {
                var intersection = first.Intersection(second);

                if (intersection != null)
                {
                    result.Add(intersection.Value);
                }
            }
        }
    }
}
