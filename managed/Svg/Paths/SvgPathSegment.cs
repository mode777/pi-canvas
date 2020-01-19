using System.Drawing;


namespace Svg.Pathing
{
    public abstract class SvgPathSegment
    {
        public PointF Start { get; set; }
        public PointF End { get; set; }

        protected SvgPathSegment()
        {
        }

        protected SvgPathSegment(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        public SvgPathSegment Clone()
        {
            return MemberwiseClone() as SvgPathSegment;
        }
    }
}
