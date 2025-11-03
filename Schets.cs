using System;
using System.Collections.Generic;
using System.Drawing;

public class Schets
{
    private Bitmap bitmap;

    public Schets()
    {
        bitmap = new Bitmap(1, 1);
    }

    public Graphics BitmapGraphics
    {
        get { return Graphics.FromImage(bitmap); }
    }

    public void VeranderAfmeting(Size sz)
    {
        if (sz.Width > bitmap.Width || sz.Height > bitmap.Height)
        {
            Bitmap nieuw = new Bitmap(
                Math.Max(sz.Width, bitmap.Width),
                Math.Max(sz.Height, bitmap.Height)
            );

            using (Graphics gr = Graphics.FromImage(nieuw))
            {
                gr.FillRectangle(Brushes.White, 0, 0, nieuw.Width, nieuw.Height);
                gr.DrawImage(bitmap, 0, 0);
            }

            bitmap = nieuw;
        }
    }

    public void Teken(Graphics gr)
    {
        gr.DrawImage(bitmap, 0, 0);
    }

    public void Schoon()
    {
        using (Graphics gr = Graphics.FromImage(bitmap))
        {
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
    }

    public void Roteer()
    {
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
    }
}
