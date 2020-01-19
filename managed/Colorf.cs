using System.Drawing;
using System.Runtime.InteropServices;

namespace OpenVg {
    
    [StructLayout(LayoutKind.Sequential)]
    public struct ColorF 
    {    
        public static ColorF FromRGB(int r, int g, int b, int a = 255){
            return new ColorF(((float)r)/255, ((float)g)/255, ((float)b)/255, ((float)a)/255); 
        }

        public static ColorF FromColor(Color color)
        {
            return FromRGB(color.R, color.G, color.B, color.A);
        }
        
        public ColorF(float r, float g, float b, float a = 1){
            R = r;
            G = g;
            B = b;
            A = a;
        } 

        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public static readonly ColorF Red = new ColorF(1, 0, 0);
        public static readonly ColorF Green = new ColorF(0, 1, 0);
        public static readonly ColorF Blue = new ColorF(0, 0, 1);
        public static readonly ColorF Yellow = new ColorF(1, 1, 0);
        public static readonly ColorF Cyan = new ColorF(0, 1, 1);
        public static readonly ColorF Magenta = new ColorF(1, 0, 1);
        public static readonly ColorF Black = new ColorF(0, 0, 0);
        public static readonly ColorF White   = new ColorF(1, 1, 1);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ColorStop
    {
        public ColorStop(float position, float r, float g, float b, float a = 1)
            : this(position, new ColorF(r,g,b,a))
        {
        }

        public ColorStop(float postion, ColorF colorf)
        {
            Position = postion;
            Colorf = colorf;
        }

        public readonly float Position;
        public readonly ColorF Colorf;
    }
}