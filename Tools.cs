using ProtoBuf;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Policy;
using System.Windows.Forms;

public interface ISchetsTool
{
    void MuisVast(SchetsControl s, Point p);
    void MuisDrag(SchetsControl s, Point p);
    void MuisLos(SchetsControl s, Point p);
    void Letter(SchetsControl s, char c);
}

public abstract class StartpuntTool : ISchetsTool
{
    protected Point startpunt;
    protected Brush kwast;

    public virtual void MuisVast(SchetsControl s, Point p)
    {
        startpunt = p;
    }

    public virtual void MuisLos(SchetsControl s, Point p)
    {
        kwast = new SolidBrush(s.PenKleur);
    }

    public abstract void MuisDrag(SchetsControl s, Point p);
    public abstract void Letter(SchetsControl s, char c);
}

public class TekstTool : StartpuntTool
{
    public override string ToString() => "tekst";

    public override void MuisDrag(SchetsControl s, Point p) { }

    public override void Letter(SchetsControl s, char c)
    {
        if (c >= 32)
        {
            using (Graphics gr = s.MaakBitmapGraphics())
            {
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz = gr.MeasureString(tekst, font, startpunt, StringFormat.GenericTypographic);

                gr.DrawString(tekst, font, kwast, startpunt, StringFormat.GenericTypographic);

                // gr.DrawRectangle(Pens.Black, startpunt.X, startpunt.Y, sz.Width, sz.Height);
                startpunt.X += (int)sz.Width;
            }
            // s.Schets.elementen.Add(new TekstElement());
            s.Invalidate();
        }
    }
}

public abstract class TweepuntTool : StartpuntTool
{
    public static Rectangle Punten2Rechthoek(Point p1, Point p2)
    {
        return new Rectangle(
            new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y)),
            new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y))
        );
    }

    public static Pen MaakPen(Brush b, int dikte)
    {
        Pen pen = new Pen(b, dikte);
        pen.StartCap = LineCap.Round;
        pen.EndCap = LineCap.Round;
        return pen;
    }

    public override void MuisVast(SchetsControl s, Point p)
    {
        base.MuisVast(s, p);
        kwast = Brushes.Gray;
    }

    public override void MuisDrag(SchetsControl s, Point p)
    {
        s.Refresh();
        Bezig(s.CreateGraphics(), startpunt, p);
    }


    public override void Letter(SchetsControl s, char c) { }

    public abstract void Bezig(Graphics g, Point p1, Point p2);

    public virtual void Compleet(Graphics g, Point p1, Point p2)
    {
        Bezig(g, p1, p2);
    }
}

public class RechthoekTool : TweepuntTool
{
    protected virtual bool Filled => false;
    public override string ToString() => "kader";

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.DrawRectangle(MaakPen(kwast, 3), Punten2Rechthoek(p1, p2));
    }

    public override void MuisLos(SchetsControl s, Point p)
    {
        var rect = Punten2Rechthoek(startpunt, p);
        s.Schets.Elementen.Add(new RechthoekElement(rect, s.PenKleur, Filled));
        s.Invalidate();
    }

}

public class VolRechthoekTool : RechthoekTool
{
    protected override bool Filled => true;
    public override string ToString() => "vlak";

    public override void Compleet(Graphics g, Point p1, Point p2)
    {
        g.FillRectangle(kwast, Punten2Rechthoek(p1, p2));
    }
}

public class LijnTool : TweepuntTool
{
    public override string ToString() => "lijn";

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.DrawLine(MaakPen(kwast, 3), p1, p2);
    }

    public override void MuisLos(SchetsControl s, Point p)
    {
        base.MuisLos(s, p);
        // Compleet(s.MaakBitmapGraphics(), startpunt, p);
        s.Schets.Elementen.Add(new LijnElement(startpunt, p, s.PenKleur));
        s.Invalidate();
    }
}

public class PenTool : LijnTool
{
    public override string ToString() => "pen";

    public override void MuisDrag(SchetsControl s, Point p)
    {
        MuisLos(s, p);
        MuisVast(s, p);
    }
}

public class GumTool : PenTool
{
    public override string ToString() => "gum";

    public override void MuisVast(SchetsControl s, Point p)
    {
        base.MuisVast(s, p);
        DeleteHit(s, p);
    }

    public override void MuisDrag(SchetsControl s, Point p)
    {
        DeleteHit(s, p);
    }

    public override void MuisLos(SchetsControl s, Point p)
    {
        DeleteHit(s, p);
    }

    private void DeleteHit(SchetsControl s, Point p)
    {
        // Remove all elements that the point touches
        s.Schets.Elementen.RemoveAll(el => el.HitTest(p));
        s.Invalidate();
    }
}

public class CirkelTool : TweepuntTool
{
    protected virtual bool Filled => false;

    public override string ToString()
    {
        return "cirkel";
    }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.DrawEllipse(MaakPen(kwast, 3), Punten2Rechthoek(p1, p2));
    }

    public override void MuisLos(SchetsControl s, Point p)
    {
        var rect = Punten2Rechthoek(startpunt, p);
        s.Schets.Elementen.Add(new CirkelElement(rect, s.PenKleur, Filled));
        s.Invalidate();
    }
}

public class VolCirkelTool : CirkelTool
{
    protected override bool Filled => true;

    public override string ToString()
    {
        return "volle cirkel";
    }

    public override void Bezig(Graphics g, Point p1, Point p2)
    {
        g.FillEllipse(kwast, Punten2Rechthoek(p1, p2));
    }
}