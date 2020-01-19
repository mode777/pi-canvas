using System.Drawing;


namespace Svg.Pathing
{
    public class SvgMoveToSegment : SvgPathSegment
    {
        public SvgMoveToSegment(PointF moveTo)
            : base(moveTo, moveTo)
        {
        }

        public override string ToString()
        {
            return "M" + Start.ToSvgString();
        }
    }
}
