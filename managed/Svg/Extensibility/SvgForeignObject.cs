using System.Drawing;


namespace Svg
{
    /// <summary>
    /// The 'foreignObject' element allows for inclusion of a foreign namespace which has its graphical content drawn by a different user agent
    /// </summary>
    [SvgElement("foreignObject")]
    public class SvgForeignObject : SvgVisualElement
    {
        ///// <summary>
        ///// Renders the <see cref="SvgElement"/> and contents to the specified <see cref="Graphics"/> object.
        ///// </summary>
        ///// <param name="renderer">The <see cref="Graphics"/> object to render to.</param>
        //protected override void Render(SvgRenderer renderer)
        //{
        //    if (!Visible || !Displayable)
        //        return;

        //    this.PushTransforms(renderer);
        //    this.SetClip(renderer);
        //    base.RenderChildren(renderer);
        //    this.ResetClip(renderer);
        //    this.PopTransforms(renderer);
        //}

        public override SvgElement DeepCopy()
        {
            return DeepCopy<SvgForeignObject>();
        }
    }
}
