using System;
using System.Drawing;

using System.Linq;
using Svg.DataTypes;

namespace Svg.FilterEffects
{
    /// <summary>
    /// A filter effect consists of a series of graphics operations that are applied to a given source graphic to produce a modified graphical result.
    /// </summary>
    [SvgElement("filter")]
    public sealed class SvgFilter : SvgElement
    {
        /// <summary>
        /// Gets or sets the position where the left point of the filter.
        /// </summary>
        [SvgAttribute("x")]
        public SvgUnit X
        {
            get { return GetAttribute<SvgUnit>("x", false); }
            set { Attributes["x"] = value; }
        }

        /// <summary>
        /// Gets or sets the position where the top point of the filter.
        /// </summary>
        [SvgAttribute("y")]
        public SvgUnit Y
        {
            get { return GetAttribute<SvgUnit>("y", false); }
            set { Attributes["y"] = value; }
        }

        /// <summary>
        /// Gets or sets the width of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("width")]
        public SvgUnit Width
        {
            get { return GetAttribute<SvgUnit>("width", false); }
            set { Attributes["width"] = value; }
        }

        /// <summary>
        /// Gets or sets the height of the resulting filter graphic.
        /// </summary>
        [SvgAttribute("height")]
        public SvgUnit Height
        {
            get { return GetAttribute<SvgUnit>("height", false); }
            set { Attributes["height"] = value; }
        }

        /// <summary>
        /// Gets or sets the color-interpolation-filters of the resulting filter graphic.
        /// NOT currently mapped through to bitmap
        /// </summary>
        [SvgAttribute("color-interpolation-filters")]
        public SvgColourInterpolation ColorInterpolationFilters
        {
            get { return GetAttribute<SvgColourInterpolation>("color-interpolation-filters", false); }
            set { Attributes["color-interpolation-filters"] = value; }
        }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgFilter>();
        }
    }
}
