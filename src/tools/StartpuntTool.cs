
namespace schetsplus.src.tools
{
    internal abstract class StartpuntTool : ISchetsTool
    {
        protected Point start;
        protected Brush brush;

        public virtual void MouseDown(SchetsControl s, Point p)
        {
            start = p;
        }

        public virtual void MouseUp(SchetsControl s, Point p)
        {
            brush = new SolidBrush(s.PenColor);
        }

        public abstract void MouseDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
    }
}
