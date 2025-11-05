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

    public abstract void Rotate(Point center);
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

    private Point RotatePoint(Point p, Point center)
    {
        int x = center.X + (p.Y - center.Y);
        int y = center.Y - (p.X - center.X);
        return new Point(x, y);
    }

    public override void Rotate(Point center)
    {
        P1 = RotatePoint(P1, center);
        P2 = RotatePoint(P2, center);
    }

}

[Serializable]
public class RechthoekElement : TekenbaarElement
{
    public Rectangle Rect { get; set; }
    public bool Filled { get; set; }

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

    public override void Rotate(Point center)
    {
        Point rectCenter = new Point(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height / 2);
        int dx = rectCenter.X - center.X;
        int dy = rectCenter.Y - center.Y;
        var newCenter = new Point(center.X + dy, center.Y - dx);
        Rect = new Rectangle(
            newCenter.X - Rect.Height / 2,
            newCenter.Y - Rect.Width / 2,
            Rect.Height,
            Rect.Width
        );
    }
}

[Serializable]
public class CirkelElement : RechthoekElement
{

    public CirkelElement(Rectangle bounds, Color kleur, bool filled)
        : base(bounds, kleur, filled)
    { }

    public override void Draw(Graphics g)
    {
        using (Brush b = new SolidBrush(Kleur))
        using (Pen p = new Pen(Kleur, 3))
        {
            if (Filled) g.FillEllipse(b, Rect);
            else g.DrawEllipse(p, Rect);
        }
    }
}

[Serializable]
public class TekstElement : RechthoekElement
{
    private string Tekst;
    private float Grootte;

    public TekstElement(string tekst, Point locatie, Color kleur, int grootte = 40)
        : base(new Rectangle(locatie.X, locatie.Y, grootte, grootte), kleur, false)
    {
        Tekst = tekst;
        Grootte = grootte;
    }

    public override void Draw(Graphics g)
    {
        Font font = new Font("Tahoma", Grootte);
        Brush b = new SolidBrush(Kleur);
        g.DrawString(Tekst, font, b, Rect.X, Rect.Y, StringFormat.GenericTypographic);

    }

    public override bool HitTest(Point p)
    {
        Bitmap temp = new Bitmap(1, 1);
        Graphics g = Graphics.FromImage(temp);
        Font font = new Font("Tahoma", Grootte);
        SizeF size = g.MeasureString(Tekst, font, new Point(Rect.X, Rect.Y), StringFormat.GenericTypographic);
        return Rect.Contains(p);
    }
}


