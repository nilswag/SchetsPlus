
namespace schetsplus.src.tools
{
    internal class PenTool : LijnTool
    {
        public override void MouseDrag(SchetsControl s, Point p)
        { 
            MouseUp(s, p);
            MouseDown(s, p);
        }

        public override string ToString() { return "pen"; }
    }
}
