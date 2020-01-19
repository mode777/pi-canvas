namespace Svg
{
    /// <summary>
    /// Represents a list of re-usable SVG components.
    /// </summary>
    [SvgElement("defs")]
    public class SvgDefinitionList : SvgElement
    {
        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgDefinitionList>();
        }
    }
}
