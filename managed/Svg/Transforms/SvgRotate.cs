
using System.Globalization;

namespace Svg.Transforms
{
    public sealed class SvgRotate : SvgTransform
    {
        public float Angle { get; set; }

        public float CenterX { get; set; }

        public float CenterY { get; set; }

        public override string WriteToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "rotate({0}, {1}, {2})", Angle, CenterX, CenterY);
        }

        public SvgRotate(float angle)
        {
            Angle = angle;
        }

        public SvgRotate(float angle, float centerX, float centerY)
            : this(angle)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public override object Clone()
        {
            return new SvgRotate(Angle, CenterX, CenterY);
        }
    }
}
