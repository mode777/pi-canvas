using System;
using System.Drawing;

using System.Globalization;

namespace Svg.Pathing
{
    public sealed class SvgArcSegment : SvgPathSegment
    {
        private const double RadiansPerDegree = Math.PI / 180.0;
        private const double DoublePI = Math.PI * 2;

        public float RadiusX { get; set; }

        public float RadiusY { get; set; }

        public float Angle { get; set; }

        public SvgArcSweep Sweep { get; set; }

        public SvgArcSize Size { get; set; }

        public SvgArcSegment(PointF start, float radiusX, float radiusY, float angle, SvgArcSize size, SvgArcSweep sweep, PointF end)
            : base(start, end)
        {
            RadiusX = Math.Abs(radiusX);
            RadiusY = Math.Abs(radiusY);
            Angle = angle;
            Sweep = sweep;
            Size = size;
        }

        private static double CalculateVectorAngle(double ux, double uy, double vx, double vy)
        {
            var ta = Math.Atan2(uy, ux);
            var tb = Math.Atan2(vy, vx);

            if (tb >= ta)
            {
                return tb - ta;
            }

            return DoublePI - (ta - tb);
        }


        public override string ToString()
        {
            var arcFlag = Size == SvgArcSize.Large ? "1" : "0";
            var sweepFlag = Sweep == SvgArcSweep.Positive ? "1" : "0";
            return "A" + RadiusX.ToString(CultureInfo.InvariantCulture) + " " + RadiusY.ToString(CultureInfo.InvariantCulture) + " " + Angle.ToString(CultureInfo.InvariantCulture) + " " + arcFlag + " " + sweepFlag + " " + End.ToSvgString();
        }
    }

    [Flags]
    public enum SvgArcSweep
    {
        Negative = 0,
        Positive = 1
    }

    [Flags]
    public enum SvgArcSize
    {
        Small = 0,
        Large = 1
    }
}
