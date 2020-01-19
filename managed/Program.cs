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
        static byte[] CreateImage()
        {
            var data = new byte[256*256*4];
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    var i = y * 256 + x;
                    data[i] = 255;
                    data[i+1] = 255;
                    data[i+2] = 255;
                    data[i+3] = 255;
                }
            }
            return data;
        }

        static void Main(string[] args)
        {
            var text = "Franz jagt im komplett verwahrlosten Taxi quer durch Bayern";
            Console.WriteLine("Creating Canvas...");
            using (var canvas = Canvas.Create())
            {
                var blackPaint = new ColorPaint(0, 0, 0);
                var rect = VgPath.CreateRect(100, 100, 200, 200);
                var font = new VgFont("Roboto.ttf");
                var img = VgImage.FromData(256, 256, CreateImage());

                canvas.BackgroundColor(ColorF.FromColor(Color.CornflowerBlue));

                var rot = 0;

                while (true)
                {
                    //canvas.LoadIdentity();
                    //canvas.Translate(canvas.Width / 2.0f, canvas.Height / 2.0f);
                    //canvas.Rotate(rot++);
                    //canvas.Translate(-canvas.Width / 2.0f, -canvas.Height / 2.0f);

                    canvas.Clear();
                    canvas.DrawText(font, new PointF(300, canvas.Height - 200), text, 12, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 224), text, 24, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 275), text, 48, blackPaint);
                    canvas.DrawText(font, new PointF(300, canvas.Height - 350), text, 72, blackPaint);
                    canvas.DrawPath(rect, null, blackPaint, 6);
                    canvas.DrawImage(img, 0, 300);
                    canvas.Present();
                }


            }

        }

        
    }
}