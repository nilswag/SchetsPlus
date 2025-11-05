using System;
using System.Drawing;

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

public class LijnElement : TekenbaarElement
{
    private Point P1;
    private Point P2;

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

public class RechthoekElement : TekenbaarElement
{
    private Rectangle Rect;
    private bool Filled;

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

public class CirkelElement : TekenbaarElement
{
    private Rectangle Bounds;
    private bool Filled;

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

// public class TekstElement : TekenbaarElement
// {
//     private string Tekst;

//     public TekstElement(string tekst, Color kleur, bool filled)
//         : base(kleur)
//     {
//         Tekst = tekst;
//     }

//     public override void Draw(Graphics g)
//     {
//         using (Brush b = new SolidBrush(Kleur))
//         using (Pen p = new Pen(Kleur, 3))
//         {
//             if (Filled) g.FillEllipse(b, Bounds);
//             else g.DrawEllipse(p, Bounds);
//         }
//     }
// }

