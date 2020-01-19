using System.Drawing;
using System.Linq;

namespace Svg.FilterEffects
{
    [SvgElement("feMerge")]
    public class SvgMerge : SvgFilterPrimitive
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgMerge>();
        }
    }
}
