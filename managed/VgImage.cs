using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    public class VgImage : VgObject
    {
        public static VgImage FromData(int w, int h, byte[] data)
        {
            unsafe
            {
                fixed(byte* ptr = data)
                {
                    var handle = UnsafeNativeMethods.CreateImage(w, h, ptr);
                    return new VgImage(handle);
                }
            }
        }

        protected override void Destroy(IntPtr handle)
        {
            UnsafeNativeMethods.DeleteImage(handle);
        }

        private VgImage(IntPtr handle)
        {
            Handle = handle;
        }
    }
}
