using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Svg;
using Svg.Pathing;


namespace OpenVg.SvgBridge
{
    public interface ISvgRendererElement
    {
        void Render(Canvas canvas);
    }

    public class SvgRendererFactory
    {
        private static IEnumerable<ISvgRendererElement> WrapChildren(IEnumerable<SvgElement> elements)
        {
            foreach (var element in elements)
            {
                yield return Wrap(element);
            }
        }

        private static ISvgRendererElement Wrap(SvgElement svg)
        {
            var children = WrapChildren(svg.Children).ToArray();
            switch (svg)
            {
                case SvgCircle circle:
                    return new SvgRendererCircle(circle, children);
                case SvgRectangle rect:
                    return  new SvgRendererRectangle(rect, children);
                case SvgPath path:
                    return new SvgRendererPath(path, children);
                case SvgLine line:
                    throw new NotImplementedException();
                default:
                    return new SvgRendererElement<SvgElement>(svg, children);
            }
        }

        public static ISvgRendererElement WrapSvgDoc(SvgDocument doc)
        {
            return Wrap(doc);
        }
    }

    public class SvgRendererElement<T> : IDisposable, ISvgRendererElement where T : SvgElement
    {
        protected T Svg { get; }
        protected IEnumerable<ISvgRendererElement> Children { get; }

        public SvgRendererElement(T svg, IEnumerable<ISvgRendererElement> children)
        {
            Svg = svg;
            Children = children;
        }
        
        public virtual void Render(Canvas canvas)
        {
            foreach (var child in Children)
            {
                child.Render(canvas);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class SvgRendererVisualElement<T> : SvgRendererElement<T> where T : SvgVisualElement
    {
        public VgPaint Fill { get; }
        public VgPaint Stroke { get; set; }

        public SvgRendererVisualElement(T svg, IEnumerable<ISvgRendererElement> children) 
            : base(svg, children)
        {
            if (svg.Fill != null && svg.Fill != SvgPaintServer.None)
            {
                Fill = CreateBrush(svg.Fill);
            }

            if (svg.Stroke != null && svg.Stroke != SvgPaintServer.None)
            {
                Stroke = CreateBrush(svg.Stroke);
            }
        }

        private VgPaint CreateBrush(SvgPaintServer server)
        {
            switch(server)
            {
                case SvgColourServer c:
                    return new ColorPaint(ColorF.FromColor(c.Colour));
                    break;
                case SvgLinearGradientServer l:
                    throw new NotImplementedException();
                    break;
                case SvgRadialGradientServer r:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported paint type.");
            }
        }

        protected override void Dispose(bool disposing)
        {
            Fill?.Dispose();
            Stroke?.Dispose();;
            base.Dispose(disposing);
        }
    }

    public abstract class SvgRendererPathElement<T> : SvgRendererVisualElement<T> where T : SvgPathBasedElement
    {
        public VgPath Path { get; }

        public SvgRendererPathElement(T svg, IEnumerable<ISvgRendererElement> children) 
            : base(svg, children)
        {
            Path = CreatePath();
        }

        protected abstract VgPath CreatePath();

        protected override void Dispose(bool disposing)
        {
            Path?.Dispose();
            base.Dispose(disposing);
        }

        public override void Render(Canvas canvas)
        {
            canvas.DrawPath(Path, Fill, Stroke, Svg.StrokeWidth);
        }
    }

    public class SvgRendererRectangle : SvgRendererPathElement<SvgRectangle>
    {
        public SvgRendererRectangle(SvgRectangle svg, IEnumerable<ISvgRendererElement> children) : base(svg, children)
        {
        }

        protected override VgPath CreatePath()
        {
            if (Svg.CornerRadiusX != 0 && Svg.CornerRadiusY != 0)
            {
                return VgPath.CreateRoundRectangle(Svg.X, Svg.Y, Svg.Width, Svg.Height, Svg.CornerRadiusX, Svg.CornerRadiusY);
            }
            else
            {
                return VgPath.CreateRect(Svg.X, Svg.Y, Svg.Width, Svg.Height);
            }
        }
    }

    public class SvgRendererCircle : SvgRendererPathElement<SvgCircle>
    {
        public SvgRendererCircle(SvgCircle svg, IEnumerable<ISvgRendererElement> children) : base(svg, children)
        {
        }

        protected override VgPath CreatePath()
        {
            return VgPath.CreateEllipse(Svg.CenterX, Svg.CenterY, Svg.Radius*2, Svg.Radius*2);
        }
    }

    public class SvgRendererPath : SvgRendererPathElement<SvgPath>
    {
        public SvgRendererPath(SvgPath svg, IEnumerable<ISvgRendererElement> children) : base(svg, children)
        {
        }

        protected override VgPath CreatePath()
        {
            var builder = new VgPathBuilder();

            foreach (var segment in Svg.PathData)
            {
                switch (segment)
                {
                    case SvgArcSegment arc:
                        throw new NotImplementedException();
                        break;
                    case SvgClosePathSegment close:
                        //Console.WriteLine("close");
                        builder.Close();
                        break;
                    case SvgCubicCurveSegment cubic:
                        //Console.WriteLine("cubic");
                        builder.Cubic(
                            cubic.FirstControlPoint.X,
                            cubic.FirstControlPoint.Y,
                            cubic.SecondControlPoint.X,
                            cubic.SecondControlPoint.Y,
                            cubic.End.X,
                            cubic.End.Y);
                        break;
                    case SvgQuadraticCurveSegment quad:
                        //Console.WriteLine("quad");
                        builder.Quadratic(quad.ControlPoint.X, quad.ControlPoint.Y, quad.End.X, quad.End.Y);
                        break;
                    case SvgLineSegment line:
                        //Console.WriteLine("line");
                        builder.Line(line.End.X, line.End.Y);
                        break;
                    case SvgMoveToSegment move:
                        //Console.WriteLine("move");
                        builder.Move(move.End.X, move.End.Y);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown segment type");
                }
            }

            return builder.Build();
        }
    }
}
