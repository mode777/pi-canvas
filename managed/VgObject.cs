﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenVg
{
    public abstract class VgObject : IDisposable
    {
        internal IntPtr Handle { get; set; }
        
        private void ReleaseUnmanagedResources()
        {
            if (Handle != IntPtr.Zero)
            {
                Destroy(Handle);
            }
        }

        protected abstract void Destroy(IntPtr handle);

        protected virtual void Dispose(bool disposing)
        {
            ReleaseUnmanagedResources();
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~VgObject()
        {
            Dispose(false);
        }
    }
}
