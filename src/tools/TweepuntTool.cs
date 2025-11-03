
using System.Drawing.Drawing2D;

namespace schetsplus.src.tools
{
    internal abstract class TweepuntTool : StartpuntTool
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

        public override void MouseDown(SchetsControl s, Point p)
        {
            base.MouseDown(s, p);
            brush = Brushes.Gray;
        }

        public override void MouseDrag(SchetsControl s, Point p)
        {
            s.Refresh();
            Bezig(s.CreateBitmapGraphics(), start, p);
        }

        public override void MouseUp(SchetsControl s, Point p)
        {
            base.MouseUp(s, p);
            Compleet(s.CreateBitmapGraphics(), start, p);
            s.Invalidate();
        }

        public override void Letter(SchetsControl s, char c) { }

        public abstract void Bezig(Graphics g, Point p1, Point p2);

        public virtual void Compleet(Graphics g, Point p1, Point p2)
        {
            Bezig(g, p1, p2);
        }

    }
}
