using System.Drawing;


namespace Svg.Pathing
{
    public sealed class SvgLineSegment : SvgPathSegment
    {
        public SvgLineSegment(PointF start, PointF end)
            : base(start, end)
        {
        }

        public override string ToString()
        {
            return "L" + End.ToSvgString();
        }
    }
}
