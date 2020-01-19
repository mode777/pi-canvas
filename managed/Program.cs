using System.Runtime.InteropServices;
using System;

using OpenVg;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

using OpenVg.SvgBridge;

using Svg;


namespace MyMonoApp
{
    class OpenVg
    {
        static void Main(string[] args)
        {
            var text = "Franz jagt im komplett verwahrlosten Taxi quer durch Bayern";
            Console.WriteLine("Creating Canvas...");
            using (var canvas = Canvas.Create())
            {
                var blackPaint = new ColorPaint(0, 0, 0);
                var rect = VgPath.CreateRect(100, 100, 200, 200);   
                var font = new VgFont("Roboto.ttf");

                canvas.BackgroundColor(ColorF.FromColor(Color.White));

                var rot = 0;

                while (true)
                {
                    canvas.LoadIdentity();
                    canvas.Translate(canvas.Width/2.0f, canvas.Height/2.0f);
                    canvas.Rotate(rot++);
                    canvas.Translate(-canvas.Width/2.0f, -canvas.Height/2.0f);

                    canvas.Clear();
                    canvas.DrawText(font, new PointF(300, canvas.Height - 200), text, 12, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 224), text, 24, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 275), text, 48, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 350), text, 72, blackPaint);
                    canvas.DrawPath(rect, null, blackPaint, 6);
                    canvas.Present();
                }

                //var svgs = new List<ISvgRendererElement>();
                //foreach (var file in new DirectoryInfo("icons").EnumerateFiles().Where(x => x.Extension == ".svg"))
                //{
                //    Console.WriteLine($"Loading {file.Name}...");
                //    var doc = SvgDocument.Open(file.FullName);
                //    var renderable = SvgRendererFactory.WrapSvgDoc(doc);
                //    svgs.Add(renderable);                
                //}

                //canvas.BackgroundColor(Colorf.FromColor(Color.CadetBlue));

                //float rot = 0;
                //int ticks = 0;


                //while (true)
                //{
                //    canvas.Clear();
                //    canvas.LoadIdentity();
                //    canvas.Translate(canvas.Width/2, canvas.Height/2);
                //    //var sin = (float)Math.Sin(Math.PI * rot / 180.0);
                //    //canvas.Scale(sin+3,sin+3);
                //    canvas.Rotate(rot);
                //    canvas.Translate(-50, -50);
                //    svgs[((ticks++)/120)%svgs.Count].Render(canvas);
                //    canvas.Present();
                //    rot += 2f;
                //}
            }


             
            //var reader = new SvgDocumentReader("feed.svg");
            //var svg = reader.Read();

            Console.ReadKey();

            //    using (var canvas = Canvas.Create())
            //    {
            //        System.Console.WriteLine("Load SVG...");
            //        var svg = UnsafeNativeMethods.LoadSvg("erlang.svg");
            //        System.Console.WriteLine("Done.");
            //        var snowBrush = new ColorBrush(0.9f, 1, 1);
            //        var snow = new List<Path>();
            //        var speeds = new List<float>();
            //        var offsets = new List<float>();
            //        var r = new Random();
            //        for (int i = 0; i < 400; i++)
            //        {
            //            speeds.Add((r.Next(700) + 100) / 100.0f);
            //            var size = r.Next(15) + 5;
            //            offsets.Add(0);
            //            snow.Add(new Ellipse(r.Next(canvas.Width), canvas.Height + r.Next(canvas.Height), size, size));
            //        }

            //        var font = new Font("MountainsofChristmas-Bold.ttf");
            //        var screen = new Point(canvas.Width, canvas.Height);
            //        canvas.BackgroundColor(Colorf.Black);
            //        var bg = new Rectangle(0, 0, screen.X, screen.Y);
            //        var bgFill = new LinearGradientBrush(new Line(new Point(screen.X, 0), screen), new ColorStop[]
            //        {
            //            new ColorStop(0, 0.4f, 0.4f, 0.8f),
            //            new ColorStop(0.5f, 0.3f, 0.3f, 0.6f)
            //        });
            //        var redFill = new ColorBrush(0.8f, 0, 0.1f);

            //        while(true)
            //        {
            //            canvas.Clear();

            //            canvas.DrawPath(bg, bgFill);

            //            canvas.DrawText(font, new Point(300, screen.Y - 200), "Merry Christmas and a Happy New Year from your Babjor!", redFill);

            //            for (int i = 0; i < snow.Count; i++)
            //            {
            //                canvas.LoadIdentity();
            //                offsets[i] += speeds[i];

            //                if (offsets[i] > screen.Y)
            //                    offsets[i] = 0;

            //                canvas.Translate(0, -offsets[i]);
            //                canvas.DrawPath(snow[i], snowBrush);
            //            }
            //            canvas.LoadIdentity();

            //            canvas.Present();
            //            //System.Console.WriteLine(sw.ElapsedMilliseconds);                    
            //        }
            //    }
        }

        
    }
}