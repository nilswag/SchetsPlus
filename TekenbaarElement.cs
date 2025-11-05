using System;
using System.Drawing;


[Serializable]
public abstract class TekenbaarElement
{
    public Color Kleur { get; set; }

    protected TekenbaarElement(Color kleur)
    {
        Kleur = kleur;
    }

    public abstract void Draw(Graphics g);

    public abstract bool HitTest(Point p);
}

[Serializable]
public class LijnElement : TekenbaarElement
{
    public Point P1 { get; set; }
    public Point P2 { get; set; }

    public LijnElement(Point p1, Point p2, Color kleur)
        : base(kleur)
    {
        P1 = p1;
        P2 = p2;
    }

    public override void Draw(Graphics g)
    {
        using (Pen pen = new Pen(Kleur, 3))
        {
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            g.DrawLine(pen, P1, P2);
        }
    }

    public override bool HitTest(Point p)
    {
        const float tolerance = 5f; // pixels
        float dx = P2.X - P1.X;
        float dy = P2.Y - P1.Y;
        float lengthSquared = dx * dx + dy * dy;

        if (lengthSquared == 0f)
        {
            // line is a single point
            return Math.Abs(p.X - P1.X) <= tolerance && Math.Abs(p.Y - P1.Y) <= tolerance;
        }

        // Project point onto the line segment (t = 0..1)
        float t = ((p.X - P1.X) * dx + (p.Y - P1.Y) * dy) / lengthSquared;
        t = Math.Max(0f, Math.Min(1f, t));

        // Closest point on the line segment
        float closestX = P1.X + t * dx;
        float closestY = P1.Y + t * dy;

        // Distance from mouse to closest point
        float distSq = (p.X - closestX) * (p.X - closestX) + (p.Y - closestY) * (p.Y - closestY);

        return distSq <= tolerance * tolerance;
    }

}

[Serializable]
public class RechthoekElement : TekenbaarElement
{
    public Rectangle Rect { get; set; }
    private bool Filled { get; set; }

    public RechthoekElement(Rectangle rect, Color kleur, bool filled)
        : base(kleur)
    {
        Rect = rect;
        Filled = filled;
    }

    public override void Draw(Graphics g)
    {
        using (Brush b = new SolidBrush(Kleur))
        using (Pen p = new Pen(Kleur, 3))
        {
            if (Filled) g.FillRectangle(b, Rect);
            else g.DrawRectangle(p, Rect);
        }
    }

    public override bool HitTest(Point p)
    {
        return Rect.Contains(p);
    }
}

[Serializable]
public class CirkelElement : TekenbaarElement
{
    public Rectangle Bounds { get; set; }
    public bool Filled { get; set; }

    public CirkelElement(Rectangle bounds, Color kleur, bool filled)
        : base(kleur)
    {
        Bounds = bounds;
        Filled = filled;
    }

    public override void Draw(Graphics g)
    {
        using (Brush b = new SolidBrush(Kleur))
        using (Pen p = new Pen(Kleur, 3))
        {
            if (Filled) g.FillEllipse(b, Bounds);
            else g.DrawEllipse(p, Bounds);
        }
    }

    public override bool HitTest(Point p)
    {
        return Bounds.Contains(p);
    }
}

[Serializable]
public class TekstElement : TekenbaarElement
{
    private string Tekst;
    private Point Locatie;
    private float Grootte;

    public TekstElement(string tekst, Point locatie, Color kleur, float grootte = 40f)
        : base(kleur)
    {
        Tekst = tekst;
        Locatie = locatie;
        Grootte = grootte;
    }

    public override void Draw(Graphics g)
    {
        Font font = new Font("Tahoma", Grootte);
        Brush b = new SolidBrush(Kleur);
        g.DrawString(Tekst, font, b, Locatie, StringFormat.GenericTypographic);

    }

    public override bool HitTest(Point p)
    {
        Bitmap temp = new Bitmap(1, 1);
        Graphics g = Graphics.FromImage(temp);
        Font font = new Font("Tahoma", Grootte);
        SizeF size = g.MeasureString(Tekst, font, Locatie, StringFormat.GenericTypographic);
        RectangleF bounds = new RectangleF(Locatie, size);
        return bounds.Contains(p);
    }
}


