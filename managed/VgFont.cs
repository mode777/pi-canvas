using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    public class VgFont : VgObject
    {
        public VgFont(string filename)
        {
            Handle = UnsafeNativeMethods.LoadFont(filename);
        }

        protected override void Destroy(IntPtr handle)
        {
            UnsafeNativeMethods.DeleteFont(handle);
        }
    }
}
