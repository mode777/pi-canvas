using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Drawing;


namespace Svg
{
    /// <summary>
    /// Represents the base class for all paint servers that are intended to be used as a fill or stroke.
    /// </summary>
    [TypeConverter(typeof(SvgPaintServerFactory))]
    public abstract class SvgPaintServer : SvgElement
    {

        /// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer None = new SvgColourServer();

        /// <summary>
        /// A <see cref="SvgPaintServer"/> that should inherit from its parent.
        /// </summary>
        public static readonly SvgPaintServer Inherit = new SvgColourServer();

        /// <summary>
        /// An unspecified <see cref="SvgPaintServer"/>.
        /// </summary>
        public static readonly SvgPaintServer NotSet = new SvgColourServer();

        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("url(#{0})", this.ID);
        }
    }
}
