using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

public class Schets
{
    private Bitmap bitmap;

    private List<TekenbaarElement> elementen;

    public List<TekenbaarElement> Elementen { get { return elementen; } set { elementen = value; } }

    public Bitmap Bitmap { get { return bitmap; } }


    public Schets()
    {
        bitmap = new Bitmap(1, 1);
        elementen = new List<TekenbaarElement>();
    }

    public Graphics BitmapGraphics
    {
        get { return Graphics.FromImage(bitmap); }
    }

    public void SlaOp(string path, ImageFormat format)
    {
        bitmap.Save(path, format);
    }

    public void LaadPlaatje(string path)
    {
        bitmap.Dispose();
        bitmap = new Bitmap(path);
    }

    public void VeranderAfmeting(Size sz)
    {
        if (sz.Width > bitmap.Width || sz.Height > bitmap.Height)
        {
            Bitmap nieuw = new Bitmap(
                Math.Max(sz.Width, bitmap.Width),
                Math.Max(sz.Height, bitmap.Height)
            );

            Graphics gr = Graphics.FromImage(nieuw);
            gr.FillRectangle(Brushes.White, 0, 0, nieuw.Width, nieuw.Height);
            gr.DrawImage(bitmap, 0, 0);

            bitmap = nieuw;
        }
    }

    public void Herteken(Graphics g)
    {
        g.Clear(Color.White);
        foreach (var el in elementen)
        {
            el.Draw(g);
        }
    }

    public void Teken(Graphics gr)
    {
        gr.Clear(Color.White);
        gr.DrawImage(bitmap, 0, 0);
        // Redraws from object list instead of static bitmap
        Herteken(gr);
    }

    public void Schoon()
    {
        Graphics gr = Graphics.FromImage(bitmap);
        elementen = [];
        // gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
    }

    public void Roteer()
    {
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
    }
}
