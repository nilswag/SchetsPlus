
namespace schetsplus.src.tools
{
    internal class RechthoekTool : TweepuntTool
    {
        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawRectangle(MaakPen(brush, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override string ToString() { return "kader"; }
    }
}
