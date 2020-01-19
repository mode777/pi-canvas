using System;
using System.Drawing;

using System.Diagnostics;

namespace Svg
{
    /// <summary>
    /// SvgPolyline defines a set of connected straight line segments. Typically, <see cref="SvgPolyline"/> defines open shapes.
    /// </summary>
    [SvgElement("polyline")]
    public class SvgPolyline : SvgPolygon
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgPolyline>();
        }
    }
}
