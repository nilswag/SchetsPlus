
namespace schetsplus.src.tools
{
    internal class GumTool : PenTool
    {
        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawLine(MaakPen(Brushes.White, 7), p1, p2);
        }

        public override string ToString() { return "gum"; }
    }
}
