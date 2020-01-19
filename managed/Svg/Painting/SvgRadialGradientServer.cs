﻿using System;
using System.Drawing;
using System.Collections.Generic;

using System.Linq;

namespace Svg
{
    [SvgElement("radialGradient")]
    public sealed class SvgRadialGradientServer : SvgGradientServer
    {
        [SvgAttribute("cx")]
        public SvgUnit CenterX
        {
            get { return GetAttribute("cx", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cx"] = value; }
        }

        [SvgAttribute("cy")]
        public SvgUnit CenterY
        {
            get { return GetAttribute("cy", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["cy"] = value; }
        }

        [SvgAttribute("r")]
        public SvgUnit Radius
        {
            get { return GetAttribute("r", false, new SvgUnit(SvgUnitType.Percentage, 50f)); }
            set { Attributes["r"] = value; }
        }

        [SvgAttribute("fx")]
        public SvgUnit FocalX
        {
            get
            {
                var value = GetAttribute("fx", false, SvgUnit.None);
                if (value.IsEmpty || value.IsNone)
                    value = CenterX;
                return value;
            }
            set { Attributes["fx"] = value; }
        }

        [SvgAttribute("fy")]
        public SvgUnit FocalY
        {
            get
            {
                var value = GetAttribute("fy", false, SvgUnit.None);
                if (value.IsEmpty || value.IsNone)
                    value = CenterY;
                return value;
            }
            set { Attributes["fy"] = value; }
        }

        private object _lockObj = new Object();

        private SvgUnit NormalizeUnit(SvgUnit orig)
        {
            return (orig.Type == SvgUnitType.Percentage && this.GradientUnits == SvgCoordinateUnits.ObjectBoundingBox ?
                    new SvgUnit(SvgUnitType.User, orig.Value / 100f) :
                    orig);
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgRadialGradientServer>();
        }
    }
}
