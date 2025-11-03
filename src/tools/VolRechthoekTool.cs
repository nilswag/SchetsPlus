
namespace schetsplus.src.tools
{
    internal class VolRechthoekTool : RechthoekTool
    {
        public override void Compleet(Graphics g, Point p1, Point p2)
        {
            g.FillRectangle(brush, TweepuntTool.Punten2Rechthoek(p1, p2));
        }
        public override string ToString() { return "vlak"; }
    }
}
