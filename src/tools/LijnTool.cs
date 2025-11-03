

namespace schetsplus.src.tools
{
    internal class LijnTool : TweepuntTool
    {
        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawLine(MaakPen(this.brush, 3), p1, p2);
        }

        public override string ToString() { return "lijn"; }
    }
}
