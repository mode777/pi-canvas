using System;
using System.Drawing;

namespace Svg.FilterEffects
{
    public enum BlurType
    {
        Both,
        HorizontalOnly,
        VerticalOnly,
    }

    [SvgElement("feGaussianBlur")]
    public class SvgGaussianBlur : SvgFilterPrimitive
    {
        private float _stdDeviation;
        private int[] _kernel;
        private int _kernelSum;
        private int[,] _multable;

        public SvgGaussianBlur()
            : this(1f, BlurType.Both)
        {
        }

        public SvgGaussianBlur(float stdDeviation)
            : this(stdDeviation, BlurType.Both)
        {
        }

        public SvgGaussianBlur(float stdDeviation, BlurType blurType)
            : base()
        {
            _stdDeviation = stdDeviation;
            BlurType = blurType;
        }

        /// <summary>
        /// Gets or sets the radius of the blur (only allows for one value - not the two specified in the SVG Spec)
        /// </summary>
        [SvgAttribute("stdDeviation")]
        public float StdDeviation
        {
            get { return _stdDeviation; }
            set
            {
                if (value <= 0f)
                {
                    throw new InvalidOperationException("Radius must be greater then 0");
                }
                _stdDeviation = value;
                Attributes["stdDeviation"] = value;
            }
        }

        public BlurType BlurType { get; set; }

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgGaussianBlur>();
        }

        public override SvgElement DeepCopy<T>()
        {
            var newObj = base.DeepCopy<T>() as SvgGaussianBlur;

            newObj._stdDeviation = _stdDeviation;
            newObj.BlurType = BlurType;
            return newObj;
        }
    }
}
