using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    public abstract class VgPaint : VgObject
    {
        protected override void Destroy(IntPtr handle)
        {
            UnsafeNativeMethods.DeletePaint(handle);
        }
    }

    public class ColorPaint : VgPaint
    {
        public ColorPaint(ColorF colorf)
        {
            Handle = UnsafeNativeMethods.CreateColorBrush(colorf);
        }

        public ColorPaint(float r, float g, float b, float a = 1)
            : this(new ColorF(r,g,b,a))
        {
        }
    }

    public class LinearGradientPaint : VgPaint
    {
        private readonly Line line;
        private readonly ColorStop[] stops;
        private readonly SpreadMode spreadMode;

        public LinearGradientPaint(Line line, ColorStop[] stops, SpreadMode spreadMode = SpreadMode.Reflect)
        {
            this.line = line;
            this.stops = stops;
            this.spreadMode = spreadMode;

            unsafe
            {
                fixed(ColorStop* stopsPtr = stops)
                {
                    Handle = UnsafeNativeMethods.CreateLinearGradientBrush(line, stopsPtr, stops.Length, spreadMode);
                }
            }
        }
    }

    public class RadialGradientPaint : VgPaint
    {
        private readonly RadialCoord coord;
        private readonly ColorStop[] stops;
        private readonly SpreadMode spreadMode;

        public RadialGradientPaint(RadialCoord coord, ColorStop[] stops, SpreadMode spreadMode = SpreadMode.Reflect)
        {
            this.coord = coord;
            this.stops = stops;
            this.spreadMode = spreadMode;

            unsafe
            {
                fixed (ColorStop* stopsPtr = stops)
                {
                    Handle = UnsafeNativeMethods.CreateRadialGradientBrush(coord, stopsPtr, stops.Length, spreadMode);
                }
            }

        }
    }

    public enum SpreadMode {
        Pad = 0x1C00,
        Repeat = 0x1C01,
        Reflect = 0x1C02
    }
}
